// 通用课程加载器（loader）：把「跑 check.exe 批改」和「跑正课演示」收进网页。
// 独立于任何一门具体课程维护，位于 docs/course/loader/——扫 mods/<pack>/ 或 ./mod 加载任意课程包。
// 跑在学员本机。绑死 server 模式——网页由它 serve，/api/* 同源调用。
//
// 结构按「日后拆老师后端 / 学员后端」留缝：
//   - 学员面：serveStatic（发网页）、handleCheck 的入口（交答案）
//   - 批改面：grade()（跑 check.exe、知道 flag）—— 将来可整段搬去老师后端
//
// 用法：
//   学员——server.exe 和 mod.zip 解开的 mod/（或 mods/<课>/）放同一目录，双击 server.exe。
//   作者 dev——在某门课的构建目录（含 爬塔.html，如 unity-apk-reverse/爬塔/）里 `go run <loader>` 或
//              把 server.exe 拷进去跑：cwd 有 爬塔.html 即进「源码直跑」模式，改完 _gen.py 刷新即见。
package main

import (
	"archive/zip"
	"context"
	_ "embed"
	"encoding/base64"
	"encoding/json"
	"fmt"
	"io"
	"net/http"
	"os"
	"os/exec"
	"path/filepath"
	"regexp"
	"strings"
	"time"
)

// 引擎 UI 壳：塔的网页外壳（自包含 CSS/JS，只加载各 mod 的 maps/content/manifest 数据 + 调 /api）。
// 内嵌进二进制 —— UI 属通用引擎、由加载器统一维护，mod 只带数据不带 UI。改 UI 后重编本加载器即全课生效。
//go:embed 爬塔.html
var towerHTML []byte

// 一个可加载的课程包（或 dev 源码）——菜单里一张卡 = 一个 mod。
type modEntry struct {
	ID         string         // 目录名 / "dev"
	StaticDir  string         // 网页+数据所在（爬塔.html / maps.js …）
	CourseRoot string         // 章节根（check.exe / 材料）
	Info       map[string]any // mod.json（dev 为合成）
}

var (
	mods       []modEntry // 客户端能看到的所有 mod（菜单列这个）
	activeID   string     // 当前进入的 mod
	staticDir  string     // = 当前 mod 的 StaticDir
	courseRoot string     // = 当前 mod 的 CourseRoot
	modInfo    map[string]any
	manifest   map[string]map[string]any
	flagRe     = regexp.MustCompile(`FLAG\{urev-[^}]+\}`)
)

const addr = "127.0.0.1:8770"

func main() {
	locate() // 扫描课程包 + 激活默认那门（内部已 loadManifest）
	mux := http.NewServeMux()
	// ---- 学员面 ----
	mux.HandleFunc("/api/health", handleHealth)
	mux.HandleFunc("/api/mods", handleMods)         // 列出所有课程包（菜单用）
	mux.HandleFunc("/api/load", handleLoad)         // 切到某个课程包
	mux.HandleFunc("/api/upload-mod", handleUploadMod) // 导入一个 mod.zip 课程包
	mux.HandleFunc("/api/check", handleCheck)       // 交答案 → 批改
	mux.HandleFunc("/api/demo", handleDemo)         // 跑正课演示
	mux.HandleFunc("/api/material", handleMaterial) // 下载靶子文件（如 upload 题的 APK）
	// 静态资源按“当前 mod 的 staticDir”动态发（切 mod 后要能发新包的网页/数据）
	mux.Handle("/", noCache(http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
		name := "爬塔.html"
		if r.URL.Path != "/" {
			name = strings.TrimPrefix(r.URL.Path, "/")
		}
		if name == "爬塔.html" { // 引擎 UI 壳：从内嵌发（不在 mod 里，全课共用一份）
			w.Header().Set("Content-Type", "text/html; charset=utf-8")
			w.Write(towerHTML)
			return
		}
		// 其余（maps.js / content.js / manifest.js 等数据）从当前 mod 的 staticDir 发
		fp := filepath.Join(staticDir, filepath.FromSlash(name))
		if rel, err := filepath.Rel(staticDir, fp); err != nil || strings.HasPrefix(rel, "..") {
			http.Error(w, "bad path", 400)
			return
		}
		http.ServeFile(w, r, fp)
	})))

	mode := "开发模式（源码目录）"
	if modInfo != nil {
		name, _ := modInfo["name"].(string)
		ver, _ := modInfo["version"].(string)
		mode = fmt.Sprintf("课程包：%s %s", name, ver)
	}
	fmt.Printf("爬塔 server 已启动\n  %s\n  章节根: %s\n  网页:   http://%s/爬塔.html\n", mode, courseRoot, addr)
	fmt.Println("按 Ctrl+C 停止。")
	if err := http.ListenAndServe(addr, mux); err != nil {
		fmt.Println("启动失败:", err)
		fmt.Println("按回车退出…")
		fmt.Scanln()
	}
}

// mod 包布局（自包含、可整包更新）：
//   <mod>/mod.json                                   包元数据（名字/版本）
//   <mod>/web/爬塔.html + maps.js/content.js/…       网页 + 数据
//   <mod>/chapters/pX-YY/作业/{check.exe,材料/}      批改器 + 材料
//                          pX-YY/正课/*.exe          演示程序
//
// 从 <web>/manifest.json 数出章节数 + 节点数（菜单卡片显示用）。
func countManifest(webDir string) (chapters, nodes int) {
	b, err := os.ReadFile(filepath.Join(webDir, "manifest.json"))
	if err != nil {
		return 0, 0
	}
	var m map[string]map[string]any
	if json.Unmarshal(b, &m) != nil {
		return 0, 0
	}
	chs := map[string]bool{}
	for _, e := range m {
		if c, _ := e["chapter"].(string); c != "" {
			chs[c] = true
		}
	}
	return len(chs), len(m)
}

// 扫描所有能加载的课程包，重建 mods[]。上传新 mod 后也调它刷新。
func scanMods() {
	mods = nil
	seen := map[string]bool{}
	add := func(dir, id string) {
		// mod 有效 = 有元数据 + 有数据（UI 壳由引擎内嵌发，不要求 mod 自带）
		if !(fileExists(filepath.Join(dir, "web", "manifest.json")) && fileExists(filepath.Join(dir, "mod.json"))) {
			return
		}
		abs, _ := filepath.Abs(dir)
		if seen[abs] {
			return
		}
		seen[abs] = true
		info := map[string]any{}
		if b, err := os.ReadFile(filepath.Join(abs, "mod.json")); err == nil {
			json.Unmarshal(b, &info)
		}
		ch, nd := countManifest(filepath.Join(abs, "web"))
		info["chapters"], info["nodes"] = ch, nd
		if id == "" {
			id = filepath.Base(abs)
		}
		mods = append(mods, modEntry{ID: id, StaticDir: filepath.Join(abs, "web"),
			CourseRoot: filepath.Join(abs, "chapters"), Info: info})
	}
	// 1) mods/ 目录里的多个课程包
	for _, root := range modsRoots() {
		if ents, err := os.ReadDir(root); err == nil {
			for _, e := range ents {
				if e.IsDir() {
					add(filepath.Join(root, e.Name()), "")
				}
			}
		}
	}
	// 2) 单个 mod（UREV_MOD / ./mod）
	if e := os.Getenv("UREV_MOD"); e != "" {
		add(e, "")
	}
	for _, d := range baseDirs() {
		add(filepath.Join(d, "mod"), "")
	}
	// 3) dev 源码模式——作为一张“开发”卡（cwd/构建目录里有生成好的 maps.js 即认；UI 壳走内嵌）
	for _, c := range devCands() {
		if fileExists(filepath.Join(c, "maps.js")) {
			sd, _ := filepath.Abs(c)
			if !seen[sd] {
				seen[sd] = true
				ch, nd := countManifest(sd)
				mods = append(mods, modEntry{ID: "dev", StaticDir: sd, CourseRoot: filepath.Join(sd, ".."),
					Info: map[string]any{"name": "（开发）源码直跑", "version": "dev",
						"description": "从源码目录实时读，改完 _gen.py 刷新即见", "chapters": ch, "nodes": nd}})
			}
			break
		}
	}
}

func locate() {
	scanMods()
	if len(mods) == 0 {
		fmt.Println("没找到任何课程包（mods/<pack>/ 或 ./mod/，含 web/爬塔.html + mod.json），也没找到源码里的 爬塔.html。")
		os.Exit(1)
	}
	activate(mods[0].ID)
}

// 客户端里放课程包的可写目录（导入的 mod 落这）。
func modsDir() string {
	for _, d := range baseDirs() {
		// 优先已存在的 mods/
		if fi, err := os.Stat(filepath.Join(d, "mods")); err == nil && fi.IsDir() {
			p, _ := filepath.Abs(filepath.Join(d, "mods"))
			return p
		}
	}
	if e := os.Getenv("UREV_MODS"); e != "" {
		p, _ := filepath.Abs(e)
		return p
	}
	// 都没有：在 exe 旁建一个 mods/
	if exe, err := os.Executable(); err == nil {
		p, _ := filepath.Abs(filepath.Join(filepath.Dir(exe), "mods"))
		return p
	}
	p, _ := filepath.Abs("mods")
	return p
}

func modsRoots() []string {
	var r []string
	if e := os.Getenv("UREV_MODS"); e != "" {
		r = append(r, e)
	}
	for _, d := range baseDirs() {
		r = append(r, filepath.Join(d, "mods"))
	}
	return r
}
func baseDirs() []string {
	var d []string
	if wd, err := os.Getwd(); err == nil {
		d = append(d, wd, filepath.Join(wd, ".."))
	}
	if exe, err := os.Executable(); err == nil {
		e := filepath.Dir(exe)
		d = append(d, e, filepath.Join(e, ".."))
	}
	return d
}
// dev 源码模式候选：某门课的构建产物目录（builder/gen.py 生成的 <course>/_web/，含 maps.js 等）。
// 作者在课程根里跑加载器（cwd=<course>），即命中 <course>/_web。
func devCands() []string {
	var c []string
	if wd, err := os.Getwd(); err == nil {
		c = append(c, filepath.Join(wd, "_web"), wd, filepath.Join(wd, "..", "_web"))
	}
	if exe, err := os.Executable(); err == nil {
		d := filepath.Dir(exe)
		c = append(c, filepath.Join(d, "_web"), d)
	}
	return c
}

// 切到某个 mod：换静态目录 / 章节根 / manifest。菜单点卡片时调。
func activate(id string) bool {
	for _, m := range mods {
		if m.ID == id {
			staticDir, courseRoot, modInfo, activeID = m.StaticDir, m.CourseRoot, m.Info, id
			loadManifest()
			return true
		}
	}
	return false
}

func handleMods(w http.ResponseWriter, r *http.Request) {
	list := make([]map[string]any, 0, len(mods))
	for _, m := range mods {
		e := map[string]any{"id": m.ID, "active": m.ID == activeID}
		for k, v := range m.Info {
			e[k] = v
		}
		list = append(list, e)
	}
	writeJSON(w, map[string]any{"mods": list, "active": activeID})
}

func handleLoad(w http.ResponseWriter, r *http.Request) {
	id := r.URL.Query().Get("id")
	if !activate(id) {
		writeJSON(w, map[string]any{"ok": false, "error": "没有这个课程包: " + id})
		return
	}
	writeJSON(w, map[string]any{"ok": true, "active": activeID})
}

// 导入课程包：学员在菜单上传一个 mod.zip → 解到 mods/<名字>/ → 重新扫描 → 菜单里多一门课。
func handleUploadMod(w http.ResponseWriter, r *http.Request) {
	if r.Method != http.MethodPost {
		http.Error(w, "POST only", 405)
		return
	}
	if err := r.ParseMultipartForm(32 << 20); err != nil { // 大文件溢出到磁盘临时文件
		writeJSON(w, map[string]any{"ok": false, "error": "上传解析失败: " + err.Error()})
		return
	}
	f, _, err := r.FormFile("mod")
	if err != nil {
		writeJSON(w, map[string]any{"ok": false, "error": "没收到 mod 文件"})
		return
	}
	defer f.Close()
	// 落地成临时 zip
	tmpZip, err := os.CreateTemp("", "urev-mod-*.zip")
	if err != nil {
		writeJSON(w, map[string]any{"ok": false, "error": err.Error()})
		return
	}
	tmpZipPath := tmpZip.Name()
	defer os.Remove(tmpZipPath)
	io.Copy(tmpZip, f)
	tmpZip.Close()
	// 解到临时目录
	tmpDir, _ := os.MkdirTemp("", "urev-mod-")
	defer os.RemoveAll(tmpDir)
	if err := unzipTo(tmpZipPath, tmpDir); err != nil {
		writeJSON(w, map[string]any{"ok": false, "error": "解压失败: " + err.Error()})
		return
	}
	// 找 mod 根（zip 根就有 mod.json，或套了一层子目录）
	src := tmpDir
	if !fileExists(filepath.Join(src, "mod.json")) {
		if ents, _ := os.ReadDir(tmpDir); len(ents) == 1 && ents[0].IsDir() {
			src = filepath.Join(tmpDir, ents[0].Name())
		}
	}
	if !(fileExists(filepath.Join(src, "mod.json")) && fileExists(filepath.Join(src, "web", "manifest.json"))) {
		writeJSON(w, map[string]any{"ok": false, "error": "这不是有效的课程包（缺 mod.json 或 web/manifest.json）"})
		return
	}
	// 目录名：从 mod.json 的 name+version 起个安全名
	var info map[string]any
	if b, e := os.ReadFile(filepath.Join(src, "mod.json")); e == nil {
		json.Unmarshal(b, &info)
	}
	name, _ := info["name"].(string)
	ver, _ := info["version"].(string)
	dirName := slug(name + "-" + ver)
	if dirName == "-" || dirName == "" {
		dirName = fmt.Sprintf("mod-%d", time.Now().Unix())
	}
	dst := filepath.Join(modsDir(), dirName)
	os.MkdirAll(modsDir(), 0755)
	os.RemoveAll(dst) // 同名覆盖（重装/升级）
	if err := copyDir(src, dst); err != nil {
		writeJSON(w, map[string]any{"ok": false, "error": "落地失败: " + err.Error()})
		return
	}
	scanMods() // 刷新菜单
	writeJSON(w, map[string]any{"ok": true, "id": dirName, "name": name, "version": ver})
}

func unzipTo(zipPath, dst string) error {
	zr, err := zip.OpenReader(zipPath)
	if err != nil {
		return err
	}
	defer zr.Close()
	for _, f := range zr.File {
		fp := filepath.Join(dst, filepath.FromSlash(f.Name))
		if rel, err := filepath.Rel(dst, fp); err != nil || strings.HasPrefix(rel, "..") {
			continue // zip-slip 防护
		}
		if f.FileInfo().IsDir() {
			os.MkdirAll(fp, 0755)
			continue
		}
		os.MkdirAll(filepath.Dir(fp), 0755)
		rc, err := f.Open()
		if err != nil {
			return err
		}
		out, err := os.Create(fp)
		if err != nil {
			rc.Close()
			return err
		}
		io.Copy(out, rc)
		out.Close()
		rc.Close()
	}
	return nil
}

// 把名字变成安全的目录名（字母数字-_，其余变 -）。
func slug(s string) string {
	var b strings.Builder
	for _, r := range s {
		if r >= 'a' && r <= 'z' || r >= 'A' && r <= 'Z' || r >= '0' && r <= '9' || r == '-' || r == '_' || r == '.' {
			b.WriteRune(r)
		} else if r >= 0x4e00 && r <= 0x9fff { // 汉字保留（Windows 目录名支持）
			b.WriteRune(r)
		} else {
			b.WriteByte('-')
		}
	}
	return strings.Trim(strings.ReplaceAll(b.String(), "--", "-"), "-")
}

func loadManifest() {
	b, err := os.ReadFile(filepath.Join(staticDir, "manifest.json"))
	if err != nil {
		fmt.Println("读不到 manifest.json —— 先在 爬塔/ 跑 python _gen.py。", err)
		os.Exit(1)
	}
	if err := json.Unmarshal(b, &manifest); err != nil {
		fmt.Println("manifest.json 坏了:", err)
		os.Exit(1)
	}
}

func handleHealth(w http.ResponseWriter, r *http.Request) {
	res := map[string]any{"ok": true, "courseRoot": courseRoot, "nodes": len(manifest)}
	if modInfo != nil {
		res["mod"] = modInfo // 当前加载的课程包（名字/版本），dev 模式没有
	}
	writeJSON(w, res)
}

// ---- 批改面：交答案 → 在临时副本里跑 check.exe → 回 {ok, flag, output} ----
type checkReq struct {
	Node    string `json:"node"`
	Answer  string `json:"answer"`  // file 型：答案文件内容
	Params  string `json:"params"`  // args 型：命令行参数（空格分隔，支持引号）
	FileB64 string `json:"fileB64"` // upload 型：上传文件（如重打包的 APK）的 base64
}

func handleCheck(w http.ResponseWriter, r *http.Request) {
	if r.Method != http.MethodPost {
		http.Error(w, "POST only", 405)
		return
	}
	var req checkReq
	if err := json.NewDecoder(r.Body).Decode(&req); err != nil {
		writeJSON(w, map[string]any{"ok": false, "error": "请求解析失败"})
		return
	}
	m := manifest[req.Node]
	if m == nil {
		writeJSON(w, map[string]any{"ok": false, "error": "未知节点 " + req.Node})
		return
	}
	if b, _ := m["hasCheck"].(bool); !b {
		writeJSON(w, map[string]any{"ok": false, "error": "这一章没有批改器"})
		return
	}
	out, flag, err := grade(m, req)
	if err != nil {
		writeJSON(w, map[string]any{"ok": false, "error": err.Error(), "output": out})
		return
	}
	writeJSON(w, map[string]any{"ok": flag != "", "flag": flag, "output": out})
}

// grade：把 作业/ 拷到临时目录（不碰真目录），写答案文件或拼参数，跑 check.exe，抓 FLAG。
func grade(m map[string]any, req checkReq) (output, flag string, err error) {
	cwd := filepath.Join(courseRoot, filepath.FromSlash(m["cwd"].(string)))
	if !fileExists(filepath.Join(cwd, "check.exe")) {
		// 没编 exe，尝试 go build 一次
		if fileExists(filepath.Join(cwd, "check.go")) {
			bctx, cancel := context.WithTimeout(context.Background(), 60*time.Second)
			defer cancel()
			bcmd := exec.CommandContext(bctx, "go", "build", "-o", "check.exe", ".")
			bcmd.Dir = cwd
			if bo, berr := bcmd.CombinedOutput(); berr != nil {
				return string(bo), "", fmt.Errorf("check.exe 没编出来: %v", berr)
			}
		} else {
			return "", "", fmt.Errorf("找不到 check.exe / check.go")
		}
	}
	tmp, err := os.MkdirTemp("", "urev-grade-")
	if err != nil {
		return "", "", err
	}
	defer os.RemoveAll(tmp)
	if err := copyDir(cwd, tmp); err != nil {
		return "", "", fmt.Errorf("准备批改环境失败: %v", err)
	}

	var args []string
	switch m["mode"] {
	case "env":
		args = nil // 环境体检：不填答案，直接跑 check.exe 探本机
	case "upload":
		// 上传型：学生传一个文件（重打包的 APK / flatc 生成的 .py），存进临时目录再把路径传给 check.exe。
		raw, derr := base64.StdEncoding.DecodeString(strings.TrimSpace(req.FileB64))
		if derr != nil {
			return "", "", fmt.Errorf("上传文件解码失败: %v", derr)
		}
		name, _ := m["uploadName"].(string)
		if name == "" {
			name = "_upload.apk"
		}
		up := filepath.Join(tmp, name)
		if err := os.WriteFile(up, raw, 0644); err != nil {
			return "", "", err
		}
		args = []string{name}
	case "args", "flag":
		args = splitArgs(req.Params) // flag 模式=学生填拼好的 flag，当单参数传
	default: // file
		name, _ := m["answerName"].(string)
		if name == "" {
			name = "answer.txt"
		}
		if err := os.WriteFile(filepath.Join(tmp, name), []byte(req.Answer), 0644); err != nil {
			return "", "", err
		}
		args = []string{name} // 多数批改器吃 argv[1] 路径；固定名读者也能读到同名文件
	}

	ctx, cancel := context.WithTimeout(context.Background(), 30*time.Second)
	defer cancel()
	cmd := exec.CommandContext(ctx, filepath.Join(tmp, "check.exe"), args...)
	cmd.Dir = tmp
	ob, runErr := cmd.CombinedOutput()
	output = string(ob)
	// 通过闸门 = 批改器退出码 0（答错的 checker 退非 0）。
	// 只有退出 0 才认 flag——否则提示文本里出现的 flag 样式串会造成假通过。
	if runErr == nil {
		if fm := flagRe.FindString(output); fm != "" {
			flag = fm
		}
	}
	return output, flag, nil
}

// ---- 下载靶子文件（upload 题：让学生把 APK 下下来重打包）----
func handleMaterial(w http.ResponseWriter, r *http.Request) {
	m := manifest[r.URL.Query().Get("node")]
	if m == nil || m["material"] == nil {
		http.Error(w, "no material", 404)
		return
	}
	cwd := filepath.Join(courseRoot, filepath.FromSlash(m["cwd"].(string)))
	fp := filepath.Join(cwd, filepath.FromSlash(m["material"].(string)))
	// 只许发 cwd 内的文件
	if rel, err := filepath.Rel(cwd, fp); err != nil || strings.HasPrefix(rel, "..") {
		http.Error(w, "bad path", 400)
		return
	}
	w.Header().Set("Content-Disposition", `attachment; filename="`+filepath.Base(fp)+`"`)
	http.ServeFile(w, r, fp)
}

// ---- 跑正课演示 ----
func handleDemo(w http.ResponseWriter, r *http.Request) {
	if r.Method != http.MethodPost {
		http.Error(w, "POST only", 405)
		return
	}
	var req struct {
		Node string `json:"node"`
	}
	json.NewDecoder(r.Body).Decode(&req)
	m := manifest[req.Node]
	if m == nil || m["demo"] == nil {
		writeJSON(w, map[string]any{"ok": false, "error": "这一章没有演示程序"})
		return
	}
	demo := filepath.Join(courseRoot, filepath.FromSlash(m["demo"].(string)))
	ctx, cancel := context.WithTimeout(context.Background(), 60*time.Second)
	defer cancel()
	cmd := exec.CommandContext(ctx, demo)
	cmd.Dir = filepath.Dir(demo)
	ob, err := cmd.CombinedOutput()
	res := map[string]any{"ok": err == nil, "output": string(ob)}
	if err != nil {
		res["error"] = err.Error()
	}
	writeJSON(w, res)
}

// ---- 工具 ----
func splitArgs(s string) []string {
	var out []string
	var cur strings.Builder
	q := byte(0)
	for i := 0; i < len(s); i++ {
		c := s[i]
		switch {
		case q != 0:
			if c == q {
				q = 0
			} else {
				cur.WriteByte(c)
			}
		case c == '"' || c == '\'':
			q = c
		case c == ' ' || c == '\t':
			if cur.Len() > 0 {
				out = append(out, cur.String())
				cur.Reset()
			}
		default:
			cur.WriteByte(c)
		}
	}
	if cur.Len() > 0 {
		out = append(out, cur.String())
	}
	return out
}

func copyDir(src, dst string) error {
	return filepath.Walk(src, func(p string, info os.FileInfo, err error) error {
		if err != nil {
			return err
		}
		rel, _ := filepath.Rel(src, p)
		target := filepath.Join(dst, rel)
		if info.IsDir() {
			return os.MkdirAll(target, 0755)
		}
		in, err := os.Open(p)
		if err != nil {
			return err
		}
		defer in.Close()
		out, err := os.Create(target)
		if err != nil {
			return err
		}
		defer out.Close()
		_, err = io.Copy(out, in)
		return err
	})
}

func fileExists(p string) bool { _, err := os.Stat(p); return err == nil }

func writeJSON(w http.ResponseWriter, v any) {
	w.Header().Set("Content-Type", "application/json; charset=utf-8")
	json.NewEncoder(w).Encode(v)
}

func noCache(h http.Handler) http.Handler {
	return http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
		w.Header().Set("Cache-Control", "no-store")
		h.ServeHTTP(w, r)
	})
}
