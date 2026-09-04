// Unity APK 逆向教程 · 第 1.1 课「演示程序」
//
// 打开它、一直按回车：它会把真实的游戏包 BareDemo.apk 当压缩包**真解压**出来，
// 弹出文件管理器让你**亲眼看到**里面的文件长什么样，再一步步讲每层是什么。
// 屏幕上的数字都是现场算的，不是写死的。
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
	"sort"
	"strings"
	"time"
)

var stdin = bufio.NewScanner(os.Stdin)

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

// 在系统文件管理器里打开一个文件夹
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

// 打开文件管理器并选中某个文件（Windows 会高亮它）
func selectFile(path string) {
	if runtime.GOOS == "windows" {
		exec.Command("explorer", "/select,"+path).Start()
	} else {
		openFolder(filepath.Dir(path))
	}
}

func findAPK() (string, error) {
	if len(os.Args) > 1 {
		if _, err := os.Stat(os.Args[1]); err == nil {
			return filepath.Abs(os.Args[1])
		}
	}
	cands := []string{
		"BareDemo.apk",
		filepath.Join("..", "demo", "build", "BareDemo.apk"),
		filepath.Join("demo", "build", "BareDemo.apk"),
	}
	if exe, err := os.Executable(); err == nil {
		d := filepath.Dir(exe)
		cands = append(cands,
			filepath.Join(d, "BareDemo.apk"),
			filepath.Join(d, "..", "demo", "build", "BareDemo.apk"),
		)
	}
	for _, c := range cands {
		if _, err := os.Stat(c); err == nil {
			return filepath.Abs(c)
		}
	}
	return "", fmt.Errorf("没找到 BareDemo.apk（把它放到程序旁边，或运行时把路径当参数传进来）")
}

func layerOf(name string) string {
	base := filepath.Base(name)
	switch {
	case strings.HasSuffix(base, ".dex"):
		return "java"
	case strings.Contains(name, "global-metadata.dat"):
		return "C#"
	case strings.HasPrefix(name, "lib/") && strings.HasSuffix(name, ".so"):
		return "native"
	case strings.HasPrefix(name, "assets/"):
		return "resources"
	case strings.HasPrefix(name, "res/") || base == "resources.arsc":
		return "resources"
	default:
		return "安卓/签名"
	}
}

type entry struct {
	name string
	size uint64
}

// 把 zip 真解压到 dst，边解边打印（让你看到“正在解压”）。返回解出的文件名列表。
func extract(zr *zip.ReadCloser, dst string) ([]string, error) {
	var names []string
	absDst, _ := filepath.Abs(dst)
	for _, f := range zr.File {
		if f.FileInfo().IsDir() {
			continue
		}
		p := filepath.Join(dst, filepath.FromSlash(f.Name))
		// 防目录穿越
		if ap, _ := filepath.Abs(p); !strings.HasPrefix(ap, absDst) {
			continue
		}
		os.MkdirAll(filepath.Dir(p), 0o755)
		rc, err := f.Open()
		if err != nil {
			return names, err
		}
		out, err := os.Create(p)
		if err != nil {
			rc.Close()
			return names, err
		}
		io.Copy(out, rc)
		out.Close()
		rc.Close()
		names = append(names, f.Name)
		fmt.Printf("    解压  %s\n", f.Name)
		time.Sleep(25 * time.Millisecond) // 稍微慢一点，让你看清在解压
	}
	return names, nil
}

func scanStrings(b []byte, min int) (total, hello int, srcPath string) {
	n, i := len(b), 0
	for i < n {
		j := i
		for j < n && b[j] >= 0x20 && b[j] <= 0x7e {
			j++
		}
		if j-i >= min {
			total++
			s := string(b[i:j])
			if s == "HelloWorld" {
				hello++
			}
			if srcPath == "" && strings.Contains(s, "HelloWorld.cs") {
				srcPath = s
			}
		}
		if j == i {
			i++
		} else {
			i = j
		}
	}
	return
}

func main() {
	enableUTF8Console()

	fmt.Println()
	fmt.Println("╔══════════════════════════════════════════════════════════╗")
	fmt.Println("║      Unity APK 逆向 · 第 1.1 课：一个游戏包里都装了什么     ║")
	fmt.Println("╚══════════════════════════════════════════════════════════╝")
	fmt.Println()
	fmt.Println("  这节课我们搞清：一个 Unity 手游的安装包（APK）里都有什么、")
	fmt.Println("  游戏逻辑到底藏在哪一层。")
	fmt.Println()
	fmt.Println("  你不用敲任何命令——一直按回车，我会把真实的游戏包解压给你看。")

	apkPath, err := findAPK()
	if err != nil {
		fmt.Println("\n[出错]", err)
		fmt.Print("按回车退出。")
		stdin.Scan()
		return
	}
	fi, _ := os.Stat(apkPath)
	pause()

	// —— 第 1 步：真解压给你看，并弹出文件管理器 ——
	step(1, "把 APK 当压缩包解压出来")
	fmt.Printf("  这个包：%s（%s）\n", filepath.Base(apkPath), human(uint64(fi.Size())))
	fmt.Println("  APK 其实就是个 zip。我现在把它解压到一个文件夹里——")
	fmt.Println()

	outDir := "_extracted_BareDemo"
	os.RemoveAll(outDir)
	os.MkdirAll(outDir, 0o755)

	zr, err := zip.OpenReader(apkPath)
	if err != nil {
		fmt.Println("\n[出错] 打不开这个包：", err)
		stdin.Scan()
		return
	}
	defer zr.Close()

	names, err := extract(zr, outDir)
	if err != nil {
		fmt.Println("\n[出错] 解压出问题：", err)
	}
	absOut, _ := filepath.Abs(outDir)
	fmt.Println()
	fmt.Printf("  解压完成！%d 个文件已经解到：\n    %s\n", len(names), absOut)
	fmt.Println()
	fmt.Println("  ▶ 我现在帮你打开文件管理器，你能亲眼看到这些文件——")
	fmt.Println("    就跟平时解压任何压缩包一模一样。")
	openFolder(absOut)
	pause()

	// 分层统计（用包里的文件名）
	byLayer := map[string][]entry{}
	for _, f := range zr.File {
		if f.FileInfo().IsDir() {
			continue
		}
		byLayer[layerOf(f.Name)] = append(byLayer[layerOf(f.Name)], entry{f.Name, f.UncompressedSize64})
	}

	// —— 第 2 步：这些文件分成五层 ——
	step(2, "刚解压出来的文件，分成五层")
	fmt.Println("  在打开的窗口里，你能对上下面这几层——")
	fmt.Println()
	order := []string{"java", "C#", "native", "脚本层", "resources", "安卓/签名"}
	desc := map[string]string{
		"java":      "启动入口 + 第三方 SDK（+ 可能的加固壳）",
		"C#":        "游戏的 C# 元信息（类名/方法名/字符串）",
		"native":    "引擎本体 + IL2CPP 运行时（.so 原生库）",
		"脚本层":       "游戏逻辑（lua / js / C# 热更）——最小 demo 里没有",
		"resources": "场景、图片、音频、数值配置",
		"安卓/签名":     "清单文件、签名等",
	}
	where := map[string]string{
		"java":      "根目录的 classes.dex",
		"C#":        "assets\\bin\\Data\\Managed\\Metadata\\ 里",
		"native":    "lib\\arm64-v8a\\ 文件夹",
		"resources": "assets\\bin\\Data\\ 文件夹",
		"安卓/签名":     "AndroidManifest.xml、META-INF\\",
	}
	for _, k := range order {
		if k == "脚本层" {
			fmt.Printf("  ● 脚本层         ：（这个最小 demo 没有，1.2 的完整靶才会有）\n")
			fmt.Printf("                     %s\n", desc[k])
			continue
		}
		es := byLayer[k]
		var sum uint64
		for _, e := range es {
			sum += e.size
		}
		fmt.Printf("  ● %-12s ：%d 个文件，共 %s —— 在【%s】\n", k, len(es), human(sum), where[k])
		fmt.Printf("                     %s\n", desc[k])
		sort.Slice(es, func(i, j int) bool { return es[i].size > es[j].size })
		for _, e := range es {
			b := filepath.Base(e.name)
			if b == "libil2cpp.so" || b == "libunity.so" || b == "libmain.so" ||
				b == "global-metadata.dat" || strings.HasSuffix(b, ".dex") {
				fmt.Printf("                       - %s (%s)\n", b, human(e.size))
			}
		}
	}
	pause()

	// —— 第 3 步：逻辑在哪 ——
	step(3, "游戏逻辑到底在哪一层？")
	fmt.Println("  · java（classes.dex）：多是启动和第三方 SDK，一般没有游戏逻辑。")
	fmt.Println("  · C# + native（libil2cpp.so / libunity.so / global-metadata.dat）：")
	fmt.Println("      这是【引擎和框架】。纯 C# 游戏（比如这个 demo）逻辑就在这。")
	fmt.Println("  · 脚本层（lua / js / C# 热更）：带热更的游戏把逻辑抽到这层。本 demo 没有。")
	fmt.Println("  · resources：图片、音频、数值。")
	fmt.Println()
	fmt.Println("  一句话记住：★ C# + native 是【引擎】，脚本层才是【游戏逻辑】。★")
	pause()

	// —— 第 4 步：认出你的代码（选中 metadata 文件给你看）——
	step(4, "在上万个引擎类里，认出「你自己写的代码」")
	metaPath := filepath.Join(outDir, "assets", "bin", "Data", "Managed", "Metadata", "global-metadata.dat")
	data, rerr := os.ReadFile(metaPath)
	if rerr != nil {
		fmt.Println("  （没找到 global-metadata.dat，跳过本步。）")
	} else {
		tot, hello, src := scanStrings(data, 4)
		absMeta, _ := filepath.Abs(metaPath)
		fmt.Println("  即使是空游戏，它也把【整个引擎 + .NET 基础类库】都打包了进去。")
		fmt.Println("  ▶ 我在文件管理器里把 global-metadata.dat 选中给你（高亮那个）——")
		fmt.Println("    它记录了所有类名/方法名/字符串。")
		selectFile(absMeta)
		fmt.Println()
		fmt.Println("  我读了一下这个文件：")
		fmt.Printf("    · 里面一共有 %d 条可读文字——绝大多数是引擎的（GameObject、Vector3…）。\n", tot)
		fmt.Printf("    · 而我们自己写的类  HelloWorld  只出现了 %d 次。\n", hello)
		if src != "" {
			fmt.Printf("    · 还能直接搜到它的源码路径：%s\n", src)
		}
		fmt.Println()
		fmt.Printf("  你的代码，是 %d 条噪声里的 %d 根针。\n", tot, hello)
		fmt.Println("  ★ 逆向的第一难点，就是从上万条里挑出「你的代码」。★")
	}
	pause()

	// —— 第 5 步：启动链 ——
	step(5, "这些层怎么串起来（开机顺序）")
	fmt.Println("  安卓先跑 classes.dex 的入口")
	fmt.Println("      → 加载 libmain.so（启动胶水）")
	fmt.Println("      → libunity.so（引擎）")
	fmt.Println("      → libil2cpp.so（你的 C#）")
	fmt.Println("      → 引擎再把脚本层拉起来")
	fmt.Println()
	fmt.Println("  dex 只是「点火器」，真正的活在后面几层。")
	pause()

	step(6, "小结")
	fmt.Println("  这节课你应该记住三点：")
	fmt.Println("    1) APK 就是个 zip，里面的东西分五层；")
	fmt.Println("    2) C# + native 是引擎，脚本层才是游戏逻辑；")
	fmt.Println("    3) 逆向第一步，是在引擎里认出「你的代码」。")
	fmt.Println()
	fmt.Println("  下一课 1.2：我们亲手造一个更完整、带各种保护的游戏包。")
	fmt.Println()

	// 收尾：让用户决定要不要清理解压出来的文件
	fmt.Printf("  刚才解压出来的文件在：\n    %s\n", absOut)
	fmt.Print("\n  要清理掉它们吗？（输入 y 清理，直接回车保留）：")
	stdin.Scan()
	ans := strings.TrimSpace(strings.ToLower(stdin.Text()))
	if ans == "y" || ans == "yes" {
		if err := os.RemoveAll(outDir); err == nil {
			fmt.Println("  已清理干净。")
		} else {
			fmt.Println("  清理失败：", err)
			fmt.Println("  （多半是那个文件管理器窗口还开着占用了，关掉再删即可。）")
		}
	} else {
		fmt.Println("  已保留，随时可以自己去看或删。")
	}
	fmt.Println()
	fmt.Print("  按回车退出。")
	stdin.Scan()
}
