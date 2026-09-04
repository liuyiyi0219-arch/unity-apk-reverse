// Unity APK 逆向教程 · 第 1.2 课「演示程序」
//
// 打开它、一直按回车：它会把 1.1 的“裸包”和本课要认识的“完整靶”都解压出来，
// 弹出文件管理器，一步步指给你看——完整靶比裸包多了什么（脚本层、游戏类、保护），
// 而这些“多出来的东西”，正是后面每一章要逐个逆的靶。
package main

import (
	"archive/zip"
	"bufio"
	"fmt"
	"os"
	"os/exec"
	"path/filepath"
	"runtime"
	"strings"
)

var stdin = bufio.NewScanner(os.Stdin)

// 完整靶里我们埋的游戏类（1.2 会现场对比：裸包没有、完整靶有）
var gameClasses = []string{"GameLogic", "License", "GameConfig", "SignatureCheck", "GameBoot"}

func pause() {
	fmt.Print("\n    ——（按回车继续）——")
	stdin.Scan()
	fmt.Print("\n")
}

func line() { fmt.Println(strings.Repeat("─", 60)) }

func step(n int, t string) {
	fmt.Println()
	line()
	fmt.Printf("  第 %d 步 · %s\n", n, t)
	line()
}

func human(n uint64) string {
	switch {
	case n >= 1<<20:
		return fmt.Sprintf("%.1f MB", float64(n)/(1<<20))
	case n >= 1<<10:
		return fmt.Sprintf("%.0f KB", float64(n)/(1<<10))
	default:
		return fmt.Sprintf("%d B", n)
	}
}

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

func findAPK(name string) string {
	cands := []string{
		name,
		filepath.Join("..", "demo", "build", name),
		filepath.Join("demo", "build", name),
	}
	if exe, err := os.Executable(); err == nil {
		d := filepath.Dir(exe)
		cands = append(cands, filepath.Join(d, name), filepath.Join(d, "..", "demo", "build", name))
	}
	for _, c := range cands {
		if _, err := os.Stat(c); err == nil {
			ab, _ := filepath.Abs(c)
			return ab
		}
	}
	return ""
}

func countWord(b []byte, word string) int {
	n, i, cnt := len(b), 0, 0
	for i < n {
		j := i
		for j < n && b[j] >= 0x20 && b[j] <= 0x7e {
			j++
		}
		if j-i >= 4 && string(b[i:j]) == word {
			cnt++
		}
		if j == i {
			i++
		} else {
			i = j
		}
	}
	return cnt
}

type apkInfo struct {
	name        string
	sizeAPK     int64
	files       int
	libil2cpp   uint64
	metadata    uint64
	hasLuaAB    bool
	classHits   map[string]int
	extractedTo string
}

// 解压 apk 到 dst，并顺手统计我们关心的东西
func analyze(apkPath, dst string) (*apkInfo, error) {
	fi, _ := os.Stat(apkPath)
	info := &apkInfo{name: filepath.Base(apkPath), sizeAPK: fi.Size(), classHits: map[string]int{}, extractedTo: dst}
	os.RemoveAll(dst)
	os.MkdirAll(dst, 0o755)
	absDst, _ := filepath.Abs(dst)

	zr, err := zip.OpenReader(apkPath)
	if err != nil {
		return info, err
	}
	defer zr.Close()

	for _, f := range zr.File {
		if f.FileInfo().IsDir() {
			continue
		}
		info.files++
		base := filepath.Base(f.Name)
		if base == "libil2cpp.so" {
			info.libil2cpp = f.UncompressedSize64
		}
		if strings.Contains(f.Name, "global-metadata.dat") {
			info.metadata = f.UncompressedSize64
		}
		if base == "gamelua.ab" {
			info.hasLuaAB = true
		}
		// 解压
		p := filepath.Join(dst, filepath.FromSlash(f.Name))
		if ap, _ := filepath.Abs(p); !strings.HasPrefix(ap, absDst) {
			continue
		}
		os.MkdirAll(filepath.Dir(p), 0o755)
		rc, e := f.Open()
		if e != nil {
			continue
		}
		out, e := os.Create(p)
		if e == nil {
			buf := make([]byte, 32*1024)
			for {
				nR, _ := rc.Read(buf)
				if nR == 0 {
					break
				}
				out.Write(buf[:nR])
			}
			out.Close()
		}
		rc.Close()
	}

	// 数埋点类名
	metaPath := filepath.Join(dst, "assets", "bin", "Data", "Managed", "Metadata", "global-metadata.dat")
	if data, e := os.ReadFile(metaPath); e == nil {
		for _, c := range gameClasses {
			info.classHits[c] = countWord(data, c)
		}
	}
	return info, nil
}

func main() {
	enableUTF8Console()

	fmt.Println()
	fmt.Println("╔══════════════════════════════════════════════════════════╗")
	fmt.Println("║   Unity APK 逆向 · 第 1.2 课：认识我们全课要逆的“靶子”      ║")
	fmt.Println("╚══════════════════════════════════════════════════════════╝")
	fmt.Println()
	fmt.Println("  1.1 我们看的是一个“空”游戏包（裸包）。这节课认识全课真正要逆的")
	fmt.Println("  “完整靶”——它像个真实小游戏：有 C# 逻辑、有 Lua 脚本、有保护。")
	fmt.Println("  我把两个包都解压出来，指给你看完整靶比裸包多了什么。")

	bare := findAPK("BareDemo.apk")
	full := findAPK("CompleteDemo.apk")
	if bare == "" || full == "" {
		fmt.Println("\n[出错] 找不到 BareDemo.apk 或 CompleteDemo.apk（应在 ../demo/build/ 下）。")
		fmt.Print("按回车退出。")
		stdin.Scan()
		return
	}
	pause()

	// —— 第 1 步：两个包都解压出来 ——
	step(1, "把两个包都解压出来")
	root := "_extracted_1.2"
	os.RemoveAll(root)
	fmt.Println("  解压中……")
	b, _ := analyze(bare, filepath.Join(root, "BareDemo(裸包)"))
	f, _ := analyze(full, filepath.Join(root, "CompleteDemo(完整靶)"))
	absRoot, _ := filepath.Abs(root)
	fmt.Printf("  裸包   BareDemo.apk     ：%d 个文件\n", b.files)
	fmt.Printf("  完整靶 CompleteDemo.apk ：%d 个文件\n", f.files)
	fmt.Println()
	fmt.Println("  ▶ 打开文件管理器，你能并排看到两个解压出来的文件夹——")
	openFolder(absRoot)
	pause()

	// —— 第 2 步：完整靶多了什么（体量 + 脚本层）——
	step(2, "完整靶比裸包多了什么（一眼可见的）")
	fmt.Printf("  · libil2cpp.so（C# 代码）：裸包 %s  →  完整靶 %s\n", human(b.libil2cpp), human(f.libil2cpp))
	fmt.Println("      变大了，因为完整靶写了真实的游戏 C# 逻辑。")
	fmt.Printf("  · global-metadata.dat    ：裸包 %s  →  完整靶 %s\n", human(b.metadata), human(f.metadata))
	fmt.Println()
	fmt.Printf("  · 【脚本层】assets/gamelua.ab：裸包%s，完整靶%s ★\n",
		yn(b.hasLuaAB), yn(f.hasLuaAB))
	fmt.Println("      这是完整靶新增的一层——加密打包的 Lua 脚本（游戏逻辑）。")
	fmt.Println("      去文件管理器 CompleteDemo(完整靶)\\assets\\ 里能看到 gamelua.ab。")
	pause()

	// —— 第 3 步：多了哪些游戏类（现场对比）——
	step(3, "完整靶多了哪些“游戏自己的类”")
	fmt.Println("  我在两个包的 metadata 里各数了一遍我们埋的几个类——")
	fmt.Println()
	fmt.Printf("    %-16s  裸包   完整靶\n", "类名")
	for _, c := range gameClasses {
		fmt.Printf("    %-16s  %-5d  %d\n", c, b.classHits[c], f.classHits[c])
	}
	fmt.Println()
	fmt.Println("  裸包全是 0、完整靶都有——这些就是完整靶里“游戏自己写的代码”：")
	fmt.Println("    GameLogic=伤害/金币公式  License=激活码校验")
	fmt.Println("    GameConfig=数值配置      SignatureCheck=签名校验  GameBoot=入口")
	pause()

	// —— 第 4 步：多了保护 ——
	step(4, "完整靶还多了“保护”")
	fmt.Println("  真实游戏不会让你轻松逆，所以完整靶也埋了保护。举两个现在就有的——")
	fmt.Println()
	fmt.Println("  · 签名校验（SignatureCheck）：运行时检查 APK 有没有被重打包。")
	fmt.Println("  · Lua 加密：gamelua.ab 不是明文。我给你解一下它的头 8 个字节——")
	demoDecryptLuaAB(filepath.Join(f.extractedTo, "assets", "gamelua.ab"))
	fmt.Println("      异或一下就变回 “UnityFS”——说明它是被简单加密过的。")
	pause()

	// —— 第 5 步：这些就是后面要逆的靶 ——
	step(5, "这些“多出来的”，就是后面每一章的靶")
	fmt.Println("  · C# 逻辑（GameLogic/License）→ 第 3 部分：还原 C#、hook、反推算法")
	fmt.Println("  · 签名校验                    → 2.3 讲原理、2.4 讲绕过")
	fmt.Println("  · Lua 脚本（gamelua.ab）      → 第 4 部分：解密、反编译、动态注入")
	fmt.Println("  · 数值/资源（GameConfig）     → 第 7 部分：提取资源与数值")
	fmt.Println("  （metadata 加密、native 壳等更重的保护，会在各自章节再叠上去。）")
	fmt.Println()
	fmt.Println("  最后 7.1 综合实战，就是把这个完整靶从零解穿。")
	pause()

	// —— 小结 + 清理 ——
	step(6, "小结")
	fmt.Println("  这节课你认识了全课的靶子：")
	fmt.Println("    · 完整靶 = 裸包 + 真实 C# 逻辑 + 脚本层(加密 Lua) + 数值 + 保护；")
	fmt.Println("    · 你不用自己装 Unity 造它——我们已经 build 好当产物给你。")
	fmt.Println()
	fmt.Printf("  刚才解压出来的两个文件夹在：\n    %s\n", absRoot)
	fmt.Print("\n  要清理掉它们吗？（输入 y 清理，直接回车保留）：")
	stdin.Scan()
	ans := strings.TrimSpace(strings.ToLower(stdin.Text()))
	if ans == "y" || ans == "yes" {
		if err := os.RemoveAll(root); err == nil {
			fmt.Println("  已清理干净。")
		} else {
			fmt.Println("  清理失败：", err, "（多半是文件管理器窗口还开着，关掉再删）")
		}
	} else {
		fmt.Println("  已保留，随时可以自己去看或删。")
	}
	fmt.Println()
	fmt.Println("  下一课 2.1：怎么拿到一个游戏的 APK（各种下载源）。")
	fmt.Print("  按回车退出。")
	stdin.Scan()
}

func yn(b bool) string {
	if b {
		return "有"
	}
	return "没有"
}

// 读 gamelua.ab 头 8 字节，XOR 0x5A 演示解密
func demoDecryptLuaAB(path string) {
	data, err := os.ReadFile(path)
	if err != nil || len(data) < 8 {
		return
	}
	raw := data[:8]
	dec := make([]byte, 8)
	for i := 0; i < 8; i++ {
		dec[i] = raw[i] ^ 0x5A
	}
	fmt.Printf("      密文(hex)：% X\n", raw)
	fmt.Printf("      XOR 0x5A 后：%q\n", string(dec))
}
