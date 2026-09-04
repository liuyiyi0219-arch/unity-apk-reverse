// 0.0 环境体检：这一课的"作业"就是把机器与设备准备好。
// 不用填答题——直接去机器上探 adb / 设备在线 / 设备已 root，全绿就吐 flag 过关。
package main

import (
	"context"
	"fmt"
	"os/exec"
	"strings"
	"time"
)

const FLAG = "FLAG{urev-0.0-device-ready}"

// 跑一条命令，超时 15s，回合并输出。
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
	fmt.Println("== 0.0 环境体检：机器与设备准备 ==")
	fmt.Println("（直接检查你本机的真实环境，全部必需项通过就算这一课过了）")
	fmt.Println()

	var checks []check

	// 1. adb 可用
	adbOut, adbErr := sh("adb", "version")
	adbOK := adbErr == nil && strings.Contains(adbOut, "Android Debug Bridge")
	checks = append(checks, check{
		"adb 可用", adbOK,
		firstLine(adbOut),
		"装 platform-tools，把 adb 加进 PATH（0.0 讲义）",
	})

	// 2. 有设备在线
	devOnline := false
	var devLine string
	if adbOK {
		dOut, _ := sh("adb", "devices")
		for _, ln := range strings.Split(dOut, "\n") {
			ln = strings.TrimSpace(ln)
			if strings.Contains(ln, "\t") && strings.HasSuffix(ln, "device") {
				devOnline = true
				devLine = ln
				break
			}
		}
	}
	checks = append(checks, check{
		"有设备在线", devOnline,
		devLine,
		"USB 连上手机、开「USB 调试」，`adb devices` 里授权一次（别停在 unauthorized/offline）",
	})

	// 3. 设备已 root
	rooted := false
	var idOut string
	if devOnline {
		idOut, _ = sh("adb", "shell", "su", "-c", "id")
		rooted = strings.Contains(idOut, "uid=0")
	}
	checks = append(checks, check{
		"设备已 root", rooted,
		strings.TrimSpace(idOut),
		"刷 Magisk 授 root（magiskcn.com），`adb shell su -c id` 要能看到 uid=0(root)",
	})

	report(checks)
}

func firstLine(s string) string {
	s = strings.TrimSpace(s)
	if i := strings.IndexByte(s, '\n'); i >= 0 {
		return s[:i]
	}
	return s
}

// report：打印清单；全绿吐 flag 过关，否则列出还差哪几项 + 怎么修。
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
		fmt.Println("环境就绪 ✅ —— 这一课过了。")
		fmt.Println(FLAG)
	} else {
		fmt.Println("还没就绪：把上面 ✘ 的项按提示配好，再点一次运行检查。")
	}
}
