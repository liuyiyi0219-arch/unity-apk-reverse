// Unity APK 逆向教程 · 第 4.1 课「演示程序」——Lua 在 Unity 里的形态（扫描器）
//
// 脚本层 Lua 常是游戏逻辑的真正所在。本课认形态：哪个框架、原生 Lua vs LuaJIT、
// 在 APK 里怎么打包（源码/字节码/AB/加密/CDN）。扫真 CompleteDemo：libtolua + gamelua.ab
// → XOR 0x5A → UnityFS → 里面是明文 Lua 源码。
package main

import (
	"archive/zip"
	"bufio"
	"fmt"
	"io"
	"os"
	"os/exec"
	"path/filepath"
	"runtime"
	"strings"
)

var stdin = bufio.NewScanner(os.Stdin)

func init() { stdin.Buffer(make([]byte, 1024*1024), 1024*1024) }

func pause() { fmt.Print("\n    ——（按回车继续）——"); stdin.Scan(); fmt.Print("\n") }
func line()  { fmt.Println(strings.Repeat("─", 60)) }
func step(n int, t string) {
	fmt.Println()
	line()
	fmt.Printf("  第 %d 步 · %s\n", n, t)
	line()
}
func exists(p string) bool { _, err := os.Stat(p); return p != "" && err == nil }
func openFolder(path string) {
	switch runtime.GOOS {
	case "windows":
		exec.Command("explorer", path).Start()
	case "darwin":
		exec.Command("open", path).Start()
	default:
		exec.Command("xdg-open", path).Start()
	}
}

// Lua 桥接框架的 native 库指纹
var luaFrameworks = map[string]string{
	"libtolua.so":  "ToLua · 原生 Lua 5.1.5",
	"libxlua.so":   "xLua · Lua 5.3/5.4 或 LuaJIT · 支持 hotfix",
	"libslua.so":   "sLua · 原生 Lua / LuaJIT",
	"libluajit.so": "LuaJIT 运行时",
	"libpuerts.so": "puerts · 其实是 JS/TS（见 5.1）",
}

// Lua magic 识别
func luaMagic(b []byte) string {
	if len(b) >= 4 && b[0] == 0x1B && b[1] == 'L' && b[2] == 'u' && b[3] == 'a' {
		return "标准 luac 字节码（\\x1bLua）→ unluac/luadec 反编译（4.2）"
	}
	if len(b) >= 3 && b[0] == 0x1B && b[1] == 'L' && b[2] == 'J' {
		return "LuaJIT 字节码（\\x1bLJ）→ luajit-decompiler-v2/ljd（注意版本）"
	}
	if len(b) >= 7 && string(b[:7]) == "UnityFS" {
		return "UnityFS AssetBundle（可能含 Lua TextAsset）→ UnityPy 解（6.1）"
	}
	// 可读文本？
	printable := 0
	for _, c := range b {
		if c == '\n' || c == '\r' || c == '\t' || (c >= 0x20 && c < 0x7f) || c >= 0x80 {
			printable++
		}
	}
	if len(b) > 0 && printable*100/len(b) > 90 {
		return "明文文本（很可能是 Lua 源码）→ 直接读"
	}
	return "未知/加密（试 XOR/AES 或魔改 header）"
}

func findAPK() string {
	if len(os.Args) > 1 && exists(os.Args[1]) {
		ab, _ := filepath.Abs(os.Args[1])
		return ab
	}
	for _, c := range []string{filepath.Join("..", "demo", "build", "CompleteDemo.apk")} {
		if exists(c) {
			ab, _ := filepath.Abs(c)
			return ab
		}
	}
	return ""
}

// 扫 APK：native libs + 疑似 Lua 载体 + 抽出 gamelua.ab
func scanAPK(apk, dst string) (libs, luaCarriers []string, abData []byte) {
	zr, err := zip.OpenReader(apk)
	if err != nil {
		return
	}
	defer zr.Close()
	for _, f := range zr.File {
		n := f.Name
		if strings.Contains(n, "arm64-v8a/") && strings.HasSuffix(n, ".so") {
			libs = append(libs, filepath.Base(n))
		}
		if strings.HasSuffix(n, ".lua") || strings.HasSuffix(n, ".luac") ||
			strings.Contains(n, ".lua.") || strings.HasSuffix(n, ".ab") || strings.HasSuffix(n, "gamelua.ab") {
			luaCarriers = append(luaCarriers, n)
		}
		if strings.HasSuffix(n, "gamelua.ab") && abData == nil {
			rc, _ := f.Open()
			abData, _ = io.ReadAll(rc)
			rc.Close()
			os.MkdirAll(dst, 0o755)
			os.WriteFile(filepath.Join(dst, "gamelua.ab"), abData, 0o644)
		}
	}
	return
}

var so = filepath.Join("正课", "sample-output")

func main() {
	enableUTF8Console()
	fmt.Println()
	fmt.Println("╔══════════════════════════════════════════════════════════╗")
	fmt.Println("║   Unity APK 逆向 · 第 4.1 课：Lua 在 Unity 里的形态       ║")
	fmt.Println("╚══════════════════════════════════════════════════════════╝")
	fmt.Println()
	fmt.Println("  很多 Unity 手游的【游戏逻辑真正在 Lua 里】（il2cpp dump 里反而没有）。")
	fmt.Println("  本课认形态：哪个框架、原生 Lua vs LuaJIT、在 APK 里怎么打包。")

	apk := findAPK()
	pause()

	// 1) 扫 native 库
	step(1, "扫 native 库：用没用 Lua、哪个框架")
	var libs, carriers []string
	var abData []byte
	if apk == "" {
		fmt.Println("  （没找到 CompleteDemo.apk）")
	} else {
		libs, carriers, abData = scanAPK(apk, so)
		fmt.Printf("  包：%s\n  native so：%s\n\n", filepath.Base(apk), strings.Join(libs, "  "))
		hit := false
		for _, l := range libs {
			if desc, ok := luaFrameworks[l]; ok {
				fmt.Printf("  ⚡ %-14s → %s\n", l, desc)
				hit = true
			}
		}
		if !hit {
			fmt.Println("  没命中 Lua 框架库。")
		}
	}
	pause()

	// 2) 扫资源找 Lua 载体
	step(2, "扫资源：找 Lua 载体")
	fmt.Println("  疑似 Lua 载体（.lua/.luac/.lua.bytes/.ab）：")
	for _, c := range carriers {
		fmt.Println("    - " + c)
	}
	if len(carriers) == 0 {
		fmt.Println("    （没有——可能是 CDN 运行时下载，或散在别处）")
	}
	fmt.Println("\n  没有散落的 .lua/.luac → Lua 被打进了 AssetBundle（gamelua.ab）。")
	pause()

	// 3) 看 magic
	step(3, "看 magic：gamelua.ab 是什么")
	if abData != nil {
		fmt.Printf("  gamelua.ab 头 8 字节：% X\n", abData[:8])
		fmt.Printf("  识别：%s\n", luaMagic(abData))
		fmt.Println("  → 不是 UnityFS。试整包 XOR 0x5A：")
		dec := make([]byte, len(abData))
		for i := range abData {
			dec[i] = abData[i] ^ 0x5A
		}
		fmt.Printf("    XOR 后头 8 字节：% X  →  %s\n", dec[:8], luaMagic(dec))
		fmt.Println("  ⇒ 形态 ④加密：整包 XOR 0x5A 的 UnityFS AssetBundle。")
		os.WriteFile(filepath.Join(so, "gamelua.decrypted.ab"), dec, 0o644)
	}
	pause()

	// 4) 解 AB 看里面
	step(4, "解 AB 看里面：是源码还是字节码？")
	fmt.Println("  AB 里的 Lua 是 LZ4 压缩的，要 UnityPy 解 AssetBundle 拿 TextAsset（6.1 的活）。")
	fmt.Println("  sample-output/extracted-lua/ 是解好的 5 个 Lua：")
	ex := filepath.Join(so, "extracted-lua")
	es, _ := os.ReadDir(ex)
	for _, e := range es {
		b, _ := os.ReadFile(filepath.Join(ex, e.Name()))
		head := firstLine(b)
		fmt.Printf("    %-12s %s  | %s\n", e.Name(), luaKind(b), head)
	}
	fmt.Println("\n  → 全是【明文 Lua 源码】。看 Battle.lua 里的伤害公式：")
	if b, err := os.ReadFile(filepath.Join(ex, "Battle.lua")); err == nil {
		fmt.Println(indentGrep(string(b), "function Battle.CalcDamage", 4))
	}
	fmt.Println("  （Lua 层也有 CalcDamage——游戏逻辑真在脚本里，和 C# 那份并存。）")
	pause()

	// 5) 形态判定 + magic 速查
	step(5, "形态判定 + Lua magic 速查")
	fmt.Println("  本 demo 形态：ToLua + 明文源码 + UnityFS AB + XOR 加密（① → ③ → ④ 叠加）。")
	fmt.Println("  → “明文源码 + 轻加密”档：解 AB + 去 XOR 就能读，不用反编译字节码。")
	fmt.Println()
	fmt.Println("  Lua magic 速查：")
	fmt.Println("    可读文本      → 明文源码，直接读")
	fmt.Println("    1B 4C 75 61   → 标准 luac 字节码，unluac 反编译（4.2）")
	fmt.Println("    1B 4C 4A      → LuaJIT 字节码，luajit-decompiler-v2/ljd（注意版本）")
	fmt.Println("    UnityFS       → AssetBundle，UnityPy 解（6.1），里面可能是上面几种")
	fmt.Println("    其它          → 加密/魔改 header（4.6–4.8）")
	pause()

	// 6) 打开 + 小结
	step(6, "打开文件夹 + 小结")
	absS, _ := filepath.Abs(so)
	fmt.Printf("  gamelua.ab（加密）+ 解密版 + extracted-lua/ 在：\n    %s\n", absS)
	openFolder(absS)
	fmt.Println()
	fmt.Println("  · 逆 Lua 游戏，真逻辑常在脚本里（不在 il2cpp）。先认形态再动手。")
	fmt.Println("  · 三问：哪个框架（tolua/xlua）？原生 Lua 还是 LuaJIT？怎么打包（源码/字节码/AB/加密）？")
	fmt.Println("  · 本 demo：ToLua + 明文源码 + UnityFS AB + XOR 0x5A。")
	fmt.Println()
	fmt.Println("  下一课 4.2：Lua 反编译——把 luac 字节码还原成源码（明文直接读，字节码才是本事）。")
	fmt.Print("  按回车退出。")
	stdin.Scan()
}

func firstLine(b []byte) string {
	s := string(b)
	if i := strings.IndexAny(s, "\r\n"); i >= 0 {
		s = s[:i]
	}
	if len([]rune(s)) > 40 {
		s = string([]rune(s)[:40]) + "…"
	}
	return s
}
func luaKind(b []byte) string {
	if len(b) >= 2 && b[0] == 0x1B && b[1] == 'L' {
		return "字节码"
	}
	return "明文源码"
}
func indentGrep(s, needle string, n int) string {
	ls := strings.Split(s, "\n")
	for i, l := range ls {
		if strings.Contains(l, needle) {
			end := i + n
			if end > len(ls) {
				end = len(ls)
			}
			var sb strings.Builder
			for _, x := range ls[i:end] {
				sb.WriteString("      " + strings.TrimRight(x, "\r") + "\n")
			}
			return strings.TrimRight(sb.String(), "\n")
		}
	}
	return "      （没找到）"
}
