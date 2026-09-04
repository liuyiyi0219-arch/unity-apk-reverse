// 1.2 环境检测：这一课的"作业"就是把靶子游戏装到你手机上。
// 直接探 adb / 设备在线 / 目标游戏是否已安装，装上了就吐 flag 过关。
package main

import (
	"context"
	"fmt"
	"os/exec"
	"strings"
	"time"
)

const FLAG = "FLAG{urev-1.2-target-installed}"

// 靶子游戏的候选包名（Vampire 真靶 / CourseDemo 备选，装了任一即算）。
var targetPkgs = []string{"com.course.vampire", "com.course.demo"}

func sh(name string, args ...string) (string, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()
	out, err := exec.CommandContext(ctx, name, args...).CombinedOutput()
	return string(out), err
}

type check struct {
	name   string
	ok     bool
	detail string
	fix    string
}

func main() {
	fmt.Println("== 1.2 环境检测：把靶子游戏装到手机上 ==")
	fmt.Println("（直接检查你的设备，装上了目标游戏就算这一课过了）")
	fmt.Println()

	var checks []check

	// 1. adb 可用
	adbOut, adbErr := sh("adb", "version")
	adbOK := adbErr == nil && strings.Contains(adbOut, "Android Debug Bridge")
	checks = append(checks, check{"adb 可用", adbOK, firstLine(adbOut),
		"装 platform-tools 并加 PATH（见 0.0）"})

	// 2. 有设备在线（收集所有在线设备的序列号）
	var serials []string
	if adbOK {
		dOut, _ := sh("adb", "devices")
		for _, ln := range strings.Split(dOut, "\n") {
			ln = strings.TrimSpace(ln)
			if strings.Contains(ln, "\t") && strings.HasSuffix(ln, "device") {
				serials = append(serials, strings.Fields(ln)[0])
			}
		}
	}
	checks = append(checks, check{"有设备在线", len(serials) > 0, strings.Join(serials, ", "),
		"USB 连上手机、开 USB 调试、`adb devices` 授权一次"})

	// 3. 目标游戏已安装（逐台设备查 pm list packages）
	installedOn, foundPkg := "", ""
	for _, s := range serials {
		pkgOut, _ := sh("adb", "-s", s, "shell", "pm", "list", "packages")
		for _, pkg := range targetPkgs {
			if strings.Contains(pkgOut, "package:"+pkg) {
				installedOn, foundPkg = s, pkg
				break
			}
		}
		if foundPkg != "" {
			break
		}
	}
	detail := ""
	if foundPkg != "" {
		detail = foundPkg + " @ " + installedOn
	}
	checks = append(checks, check{"目标游戏已安装", foundPkg != "", detail,
		"把靶子游戏 APK 装上：`adb install -r <游戏>.apk`（Vampire=com.course.vampire）"})

	report(checks)
}

func firstLine(s string) string {
	s = strings.TrimSpace(s)
	if i := strings.IndexByte(s, '\n'); i >= 0 {
		return s[:i]
	}
	return s
}

func report(checks []check) {
	allOK := true
	for _, c := range checks {
		mark := "✘"
		if c.ok {
			mark = "✔"
		} else {
			allOK = false
		}
		fmt.Printf("  %s %s", mark, c.name)
		if c.detail != "" {
			fmt.Printf("  —— %s", c.detail)
		}
		fmt.Println()
		if !c.ok {
			fmt.Printf("      怎么修：%s\n", c.fix)
		}
	}
	fmt.Println()
	if allOK {
		fmt.Println("靶子已就位 ✅ —— 这一课过了。")
		fmt.Println(FLAG)
	} else {
		fmt.Println("还没就位：把上面 ✘ 的项按提示配好，再点一次运行检查。")
	}
}
