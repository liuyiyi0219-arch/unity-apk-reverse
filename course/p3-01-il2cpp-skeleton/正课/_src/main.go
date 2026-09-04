// Unity APK 逆向教程 · 第 3.1 课「演示程序」——出骨架 dump.cs
//
// 用 Il2CppDumper 把 libil2cpp.so + global-metadata.dat 反出【骨架】：
//   - dump.cs：一整份类型/方法/RVA 清单
//   - DummyDll：桩程序集 → ilspycmd 反编译成【按类型拆好的 C#】（含 RVA，但方法体是桩）
// 真正的方法体逻辑是 3.3 的活；这里先拿到"骨架 + RVA"。
// 需要已安装 Il2CppDumper 和 ilspycmd（见 0.1 环境配置）。
package main

import (
	"archive/zip"
	"bufio"
	"encoding/json"
	"fmt"
	"io"
	"os"
	"os/exec"
	"path/filepath"
	"runtime"
	"strings"
)

var stdin = bufio.NewScanner(os.Stdin)

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

// 找 Il2CppDumper.exe：课程自带 → env_manifest → ~/.reverse-tools glob
func findDumper() string {
	for _, base := range []string{filepath.Join("..", "tools"), "tools"} {
		p := filepath.Join(base, "il2cppdumper", "Il2CppDumper.exe")
		if exists(p) {
			ab, _ := filepath.Abs(p)
			return ab
		}
	}
	home := os.Getenv("USERPROFILE")
	if home == "" {
		home = os.Getenv("HOME")
	}
	mf := filepath.Join(home, ".reverse-tools", "env_manifest.json")
	if b, err := os.ReadFile(mf); err == nil {
		var m struct {
			Tools map[string]struct {
				Path string `json:"path"`
			} `json:"tools"`
		}
		if json.Unmarshal(b, &m) == nil {
			if t, ok := m.Tools["il2cppdumper"]; ok && exists(t.Path) {
				return t.Path
			}
		}
	}
	if g, _ := filepath.Glob(filepath.Join(home, ".reverse-tools", "il2cppdumper-*", "Il2CppDumper.exe")); len(g) > 0 {
		return g[0]
	}
	return ""
}

// 找我们为 CompleteDemo 写的原始源码（用来和逆出来的对比）
func findSource(name string) string {
	p := filepath.Join("..", "demo", "CourseDemo", "Assets", "Scripts", "Game", name)
	if exists(p) {
		ab, _ := filepath.Abs(p)
		return ab
	}
	return ""
}

func findAPK() string {
	if len(os.Args) > 1 && exists(os.Args[1]) {
		ab, _ := filepath.Abs(os.Args[1])
		return ab
	}
	for _, c := range []string{
		filepath.Join("..", "demo", "build", "CompleteDemo.apk"),
		filepath.Join("..", "demo", "VampireSurvivors", "build", "VampireSurvivors.apk"),
	} {
		if exists(c) {
			ab, _ := filepath.Abs(c)
			return ab
		}
	}
	return ""
}

func extract(apk, dst string) (soPath, metaPath string) {
	zr, err := zip.OpenReader(apk)
	if err != nil {
		return
	}
	defer zr.Close()
	for _, f := range zr.File {
		var out string
		if strings.HasSuffix(f.Name, "libil2cpp.so") && strings.Contains(f.Name, "arm64") {
			out = filepath.Join(dst, "libil2cpp.so")
		} else if strings.Contains(f.Name, "global-metadata.dat") {
			out = filepath.Join(dst, "global-metadata.dat")
		} else {
			continue
		}
		os.MkdirAll(dst, 0o755)
		rc, _ := f.Open()
		o, _ := os.Create(out)
		io.Copy(o, rc)
		o.Close()
		rc.Close()
		if strings.HasSuffix(out, "libil2cpp.so") {
			soPath = out
		} else {
			metaPath = out
		}
	}
	return
}

func main() {
	enableUTF8Console()
	fmt.Println()
	fmt.Println("╔══════════════════════════════════════════════════════════╗")
	fmt.Println("║   Unity APK 逆向 · 第 3.1 课：出骨架 dump.cs               ║")
	fmt.Println("╚══════════════════════════════════════════════════════════╝")
	fmt.Println()
	fmt.Println("  libil2cpp.so 是编译后的机器码、看不懂；但 global-metadata.dat 里有")
	fmt.Println("  所有类名/方法名/字段。Il2CppDumper 把两者一拼，就能反出【骨架】：")
	fmt.Println("  类型/方法签名 + 每个方法的 RVA（地址）。这一步不还原方法体（那是 3.3）。")

	dumper := findDumper()
	ilspy, _ := exec.LookPath("ilspycmd")
	apk := findAPK()
	if dumper == "" {
		fmt.Println("\n[出错] 没找到 Il2CppDumper.exe（装它、或放到 tools/il2cppdumper/，见 0.1 环境配置）。")
		fmt.Print("按回车退出。")
		stdin.Scan()
		return
	}
	if apk == "" {
		fmt.Println("\n[出错] 没找到 demo 包（../demo/build/CompleteDemo.apk）。")
		fmt.Print("按回车退出。")
		stdin.Scan()
		return
	}
	work := "_dump_out"
	os.RemoveAll(work)
	os.MkdirAll(filepath.Join(work, "in"), 0o755)
	pause()

	// 1) 抽 so + metadata
	step(1, "从 APK 抽出两个关键文件")
	so, meta := extract(apk, filepath.Join(work, "in"))
	fmt.Printf("  目标包：%s\n", filepath.Base(apk))
	fmt.Printf("  libil2cpp.so        : %s\n", ok(so))
	fmt.Printf("  global-metadata.dat : %s\n", ok(meta))
	fmt.Println("  （逆 IL2CPP 必须两个一起——1.1 讲过。）")
	pause()

	// 2) Il2CppDumper
	step(2, "Il2CppDumper：反出骨架")
	outDir := filepath.Join(work, "dump")
	os.MkdirAll(outDir, 0o755)
	cmd := exec.Command(dumper, so, meta, outDir)
	cmd.Stdin = strings.NewReader("\n\n") // 防它卡在“按任意键”
	out, _ := cmd.CombinedOutput()
	tailPrint(string(out), 3)
	fmt.Println("  产物：", strings.Join(lsNames(outDir), " "))
	pause()

	// 3) dump.cs 是什么
	step(3, "dump.cs —— 一整份骨架清单")
	dumpcs := filepath.Join(outDir, "dump.cs")
	nlines := countLines(dumpcs)
	fmt.Printf("  dump.cs 共 %d 行——所有类型/方法的签名 + RVA，一览无余。\n", nlines)
	fmt.Println("  找我们埋的 GameLogic（含方法 RVA）：")
	fmt.Println(grepAround(dumpcs, "class GameLogic", 6))
	pause()

	// 4) ilspycmd → 按模块拆好的 C#
	step(4, "DummyDll → 按类型拆好的 C#")
	recoveredCS := ""
	if ilspy == "" {
		fmt.Println("  （没装 ilspycmd，跳过这步——装它见 0.1：dotnet tool install -g ilspycmd）")
	} else {
		csOut := filepath.Join(work, "csharp", "Assembly-CSharp")
		os.MkdirAll(csOut, 0o755)
		dll := filepath.Join(outDir, "DummyDll", "Assembly-CSharp.dll")
		o2, _ := exec.Command(ilspy, dll, "-p", "-o", csOut).CombinedOutput()
		_ = o2
		fmt.Println("  Assembly-CSharp.dll（游戏代码）拆成了一个个 .cs：")
		for _, n := range lsNames(csOut) {
			b := filepath.Base(n)
			if strings.Contains(b, "Game") || strings.Contains(b, "License") || strings.Contains(b, "Signature") {
				fmt.Println("    - " + b)
			}
		}
		fmt.Println()
		recoveredCS = filepath.Join(csOut, "GameLogic.cs")
		fmt.Println("  GameLogic.cs 长这样（有签名 + RVA，方法体是桩 default）：")
		fmt.Println(headFile(recoveredCS, 14))
	}
	pause()

	// 5) 对比：原始源码 vs 还原产物
	step(5, "对比：你写的源码 vs 逆出来的骨架")
	orig := findSource("GameLogic.cs")
	if orig != "" && recoveredCS != "" {
		fmt.Println("  【你写的】GameLogic.CalcDamage（原始源码，有真逻辑）：")
		fmt.Println(grepAround(orig, "CalcDamage", 5))
		fmt.Println()
		fmt.Println("  【逆出来的】GameLogic.CalcDamage（还原产物，只剩壳）：")
		fmt.Println(grepAround(recoveredCS, "CalcDamage", 5))
		fmt.Println()
		fmt.Println("  对比结论：")
		fmt.Println("    · 签名 / 参数 / 返回值 / 类结构 —— 【完全一致】✓（骨架很忠实）")
		fmt.Println("    · 方法体（真正的算法） —— 【丢了】，变成桩 return default")
		fmt.Println("    · 多出 [Address(RVA=...)] —— 告诉你逻辑在 native 的哪个地址")
		fmt.Println("  → 骨架够你【定位】；要【看懂逻辑】得拿 RVA 去 native 读（3.2）、再翻译（3.3）。")
	} else {
		fmt.Println("  （没找到原始源码或还原产物，跳过对比。）")
	}
	pause()

	// 6) 打开看
	step(6, "打开产物文件夹")
	absW, _ := filepath.Abs(work)
	fmt.Printf("  dump.cs / DummyDll / 拆好的 C# 都在：\n    %s\n", absW)
	openFolder(absW)
	fmt.Println()
	fmt.Println("  两份骨架的分工：")
	fmt.Println("    · dump.cs      —— 一整份，全局搜类/方法/RVA 最快；")
	fmt.Println("    · 拆好的 C#    —— 按类型浏览，每个方法带 [Address(RVA=...)]。")
	pause()

	step(7, "小结")
	fmt.Println("  · Il2CppDumper 把 so+metadata 反成【骨架】：类型/方法签名 + RVA。")
	fmt.Println("  · dump.cs 是清单；DummyDll→ilspycmd 是按模块拆好的可浏览 C#。")
	fmt.Println("  · 方法体还是桩——真逻辑靠 RVA 去 native 里读（3.2）、再翻译成真 C#（3.3）。")
	fmt.Println()
	fmt.Printf("  产物在：%s\n", absW)
	fmt.Print("\n  要清理产物吗？（输入 y 清理，直接回车保留）：")
	stdin.Scan()
	if strings.TrimSpace(strings.ToLower(stdin.Text())) == "y" {
		os.RemoveAll(work)
		fmt.Println("  已清理。")
	} else {
		fmt.Println("  已保留。")
	}
	fmt.Println()
	fmt.Println("  下一课 3.2：拿 RVA 去 Ghidra 里找到方法、读伪 C。")
	fmt.Print("  按回车退出。")
	stdin.Scan()
}

func ok(p string) string {
	if exists(p) {
		return "✔ 抽出"
	}
	return "（没找到）"
}
func lsNames(dir string) []string {
	es, _ := os.ReadDir(dir)
	var out []string
	for _, e := range es {
		out = append(out, e.Name())
	}
	return out
}
func tailPrint(s string, n int) {
	ls := strings.Split(strings.TrimRight(s, "\n"), "\n")
	if len(ls) > n {
		ls = ls[len(ls)-n:]
	}
	for _, l := range ls {
		fmt.Println("    " + strings.TrimRight(l, "\r"))
	}
}
func countLines(p string) int {
	b, err := os.ReadFile(p)
	if err != nil {
		return 0
	}
	return strings.Count(string(b), "\n")
}
func grepAround(p, needle string, n int) string {
	b, err := os.ReadFile(p)
	if err != nil {
		return "    （dump.cs 没读到）"
	}
	ls := strings.Split(string(b), "\n")
	for i, l := range ls {
		if strings.Contains(l, needle) {
			end := i + n
			if end > len(ls) {
				end = len(ls)
			}
			var sb strings.Builder
			for _, x := range ls[i:end] {
				sb.WriteString("    " + strings.TrimRight(x, "\r") + "\n")
			}
			return strings.TrimRight(sb.String(), "\n")
		}
	}
	return "    （没找到 " + needle + "）"
}
func headFile(p string, n int) string {
	b, err := os.ReadFile(p)
	if err != nil {
		return "    （没读到）"
	}
	ls := strings.Split(string(b), "\n")
	if len(ls) > n {
		ls = ls[:n]
	}
	var sb strings.Builder
	for _, l := range ls {
		sb.WriteString("    " + strings.TrimRight(l, "\r") + "\n")
	}
	return strings.TrimRight(sb.String(), "\n")
}
