// Unity APK 逆向教程 · 第 0.0 课「演示程序」——设备体检
//
// 自动跑一遍前序 checklist：找 adb → adb devices → adb shell su -c id（验 root）→ 读架构/版本。
// 全是 adb 只读命令，不改设备任何东西；没连设备也能跑，会明确告诉你缺哪一步。
package main

import (
	"bufio"
	"fmt"
	"os"
	"os/exec"
	"path/filepath"
	"runtime"
	"strings"
	"time"
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

// 找 adb：先 PATH，再课程/系统常见目录
func findADB() string {
	if p, err := exec.LookPath("adb"); err == nil {
		return p
	}
	for _, c := range []string{
		`D:\tools\platform-tools\adb.exe`,
		`C:\platform-tools\adb.exe`,
		filepath.Join(os.Getenv("LOCALAPPDATA"), `Android\Sdk\platform-tools\adb.exe`),
		filepath.Join(os.Getenv("USERPROFILE"), `.reverse-tools\platform-tools\adb.exe`),
	} {
		if _, err := os.Stat(c); err == nil {
			return c
		}
	}
	return ""
}

// 跑一条 adb 命令，带超时（设备卡授权框时不至于永久挂住）
func adb(bin string, args ...string) (string, bool) {
	cmd := exec.Command(bin, args...)
	done := make(chan struct{})
	var out []byte
	var err error
	go func() { out, err = cmd.CombinedOutput(); close(done) }()
	select {
	case <-done:
		return strings.TrimSpace(string(out)), err == nil
	case <-time.After(12 * time.Second):
		_ = cmd.Process.Kill()
		return "(超时——手机上可能有 Magisk/授权弹窗在等你点『允许』)", false
	}
}

type check struct {
	name string
	ok   bool
	note string
}

var results []check

func record(name string, ok bool, note string) {
	results = append(results, check{name, ok, note})
	mark := "❌"
	if ok {
		mark = "✅"
	}
	fmt.Printf("  %s %-22s %s\n", mark, name, note)
}

func main() {
	enableUTF8Console()
	fmt.Println()
	fmt.Println("╔══════════════════════════════════════════════════════════╗")
	fmt.Println("║  Unity APK 逆向 · 第 0.0 课：设备体检（前序环境自检）      ║")
	fmt.Println("╚══════════════════════════════════════════════════════════╝")
	fmt.Println()
	fmt.Println("  自动跑一遍 checklist：adb → 设备连接/授权 → root → 架构/版本。")
	fmt.Println("  全是只读命令，不改设备。缺哪步会明确告诉你回本章哪一段。")
	pause()

	// 1) 系统 & adb
	step(1, "Windows + adb")
	if runtime.GOOS == "windows" {
		record("Windows 系统", true, runtime.GOOS+"（课程 CLI 工具链基线 Win10+）")
	} else {
		record("操作系统", true, runtime.GOOS+"（本课以 Win10+ 为准）")
	}
	adbBin := findADB()
	if adbBin == "" {
		record("adb 可用", false, "没找到 adb！装 platform-tools 并加 PATH（本章 A-2 / 见 0.1）")
		fmt.Println("\n  ⚠ 没有 adb，后面几步做不了。装好 adb 再来。")
		scorecard()
		return
	}
	ver, _ := adb(adbBin, "version")
	record("adb 可用", true, adbBin)
	fmt.Println("     " + firstLine(ver))
	pause()

	// 2) 设备连接 + 授权
	step(2, "设备连接 & USB 调试授权")
	out, ok := adb(adbBin, "devices")
	fmt.Println("  $ adb devices\n" + indent(out))
	serial, state := parseDevices(out)
	switch {
	case !ok:
		record("设备已连接", false, "adb devices 执行失败，重插/换线")
	case serial == "":
		record("设备已连接", false, "没有设备——插线、开 USB 调试（本章 B）")
	case state == "unauthorized":
		record("设备已连接", true, serial)
		record("USB 调试已授权", false, "手机上点『允许 USB 调试』弹窗（本章 B-4）")
	case state == "device":
		record("设备已连接", true, serial)
		record("USB 调试已授权", true, "state=device")
	default:
		record("设备已连接", true, serial+"（state="+state+"，重插/重启 adb）")
	}
	deviceReady := state == "device"
	pause()

	// 3) root
	step(3, "Root 验证（adb shell su -c id）")
	if !deviceReady {
		record("已 Root", false, "设备没就绪，先过第 2 步")
	} else {
		idOut, _ := adb(adbBin, "shell", "su", "-c", "id")
		fmt.Println("  $ adb shell su -c id\n" + indent(idOut))
		if strings.Contains(idOut, "uid=0") {
			record("已 Root", true, "输出含 uid=0(root)")
		} else {
			record("已 Root", false, "无 uid=0——root 没成/Magisk 未授权（本章 C/D）")
		}
	}
	pause()

	// 4) 架构 / 版本 / 机型
	step(4, "认设备（选 frida/so 版本要用）")
	if deviceReady {
		abi, _ := adb(adbBin, "shell", "getprop", "ro.product.cpu.abi")
		rel, _ := adb(adbBin, "shell", "getprop", "ro.build.version.release")
		model, _ := adb(adbBin, "shell", "getprop", "ro.product.model")
		record("CPU 架构", abi != "", firstLine(abi)+"（决定用哪版 frida-server/so）")
		record("安卓版本", rel != "", "Android "+firstLine(rel))
		fmt.Printf("     机型：%s\n", firstLine(model))
	} else {
		record("CPU 架构", false, "设备没就绪，跳过")
		record("安卓版本", false, "设备没就绪，跳过")
	}
	pause()

	// 5) 记分卡
	step(5, "体检记分卡")
	scorecard()

	absS, _ := filepath.Abs(".")
	fmt.Printf("\n  本章：\n    %s\n", absS)
	fmt.Println()
	fmt.Println("  全 ✅ = 逆向工作台就绪，可进 0.1 工具清单 / 1.1 开始逆向。")
	fmt.Println("  有 ❌ = 按记分卡提示回本章对应段落补齐。")
	fmt.Print("  按回车退出。")
	stdin.Scan()
}

func scorecard() {
	fmt.Println()
	pass := 0
	for _, r := range results {
		mark := "❌"
		if r.ok {
			mark = "✅"
			pass++
		}
		fmt.Printf("    %s %-22s %s\n", mark, r.name, r.note)
	}
	fmt.Printf("\n  合计 %d/%d 项通过。\n", pass, len(results))
}

// 从 `adb devices` 输出里取第一台设备的序列号 + 状态
func parseDevices(out string) (serial, state string) {
	for _, l := range strings.Split(out, "\n") {
		l = strings.TrimSpace(l)
		if l == "" || strings.HasPrefix(l, "List of devices") || strings.HasPrefix(l, "*") {
			continue
		}
		f := strings.Fields(l)
		if len(f) >= 2 {
			return f[0], f[1]
		}
	}
	return "", ""
}

func firstLine(s string) string {
	if i := strings.IndexByte(s, '\n'); i >= 0 {
		return strings.TrimSpace(s[:i])
	}
	return strings.TrimSpace(s)
}

func indent(s string) string {
	var b strings.Builder
	for _, l := range strings.Split(strings.TrimRight(s, "\n"), "\n") {
		b.WriteString("    " + strings.TrimRight(l, "\r") + "\n")
	}
	return strings.TrimRight(b.String(), "\n")
}

func init() { stdin.Buffer(make([]byte, 1024*1024), 1024*1024) }
