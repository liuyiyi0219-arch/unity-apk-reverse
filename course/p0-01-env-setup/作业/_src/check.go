// 0.1 环境体检：这一课的"作业"就是把本课要用的工具装好。
// 本课只做离线扒包 + 装真机，必需项按本课 8 章实际用到的收窄——
// adb（装游戏到真机）/ python + UnityPy（导资源）/ jadx + java（反编译 DEX）。
// 不要求 ilspycmd（本课不反编译 C#，只 grep metadata）。
package main

import (
	"context"
	"fmt"
	"os"
	"os/exec"
	"strings"
	"time"
)

const FLAG = "FLAG{urev-0.1-toolchain-ready}"

func have(name string) bool {
	_, err := exec.LookPath(name)
	return err == nil
}

// 跑命令取版本行（超时 10s），失败返回空。
func ver(name string, args ...string) string {
	ctx, cancel := context.WithTimeout(context.Background(), 10*time.Second)
	defer cancel()
	out, err := exec.CommandContext(ctx, name, args...).CombinedOutput()
	if err != nil {
		return ""
	}
	s := strings.TrimSpace(string(out))
	if i := strings.IndexByte(s, '\n'); i >= 0 {
		s = s[:i]
	}
	return s
}

// python 解释器：优先 python，回退 python3。
func pyExe() string {
	if have("python") {
		return "python"
	}
	if have("python3") {
		return "python3"
	}
	return ""
}

// 探一个 python 模块装没装（import 成功即算有），返回 (ok, 版本串)。
func pymod(mod string) (bool, string) {
	py := pyExe()
	if py == "" {
		return false, ""
	}
	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()
	code := "import " + mod + ",sys;print(getattr(" + mod + ",'__version__',''))"
	out, err := exec.CommandContext(ctx, py, "-c", code).CombinedOutput()
	if err != nil {
		return false, ""
	}
	return true, mod + " " + strings.TrimSpace(string(out))
}

type check struct {
	name     string
	ok       bool
	detail   string
	fix      string
	required bool
}

func main() {
	fmt.Println("== 0.1 环境体检：工具链 ==")
	fmt.Println("（探你本机 PATH 上的工具。必需项全绿这一课就过；建议项缺了只提醒，不拦）")
	fmt.Println()

	var cs []check
	py := pyExe()

	// 必需：adb —— 把靶子游戏装到真机（1.2）、体检设备（0.0）
	cs = append(cs, check{"adb（装游戏到真机）", have("adb"), ver("adb", "version"),
		"装 platform-tools 并加 PATH（见 0.0）", true})
	// 必需：python —— 解包/脚本/生成器都靠它（3.1 grep、4.1 XOR、7.1 UnityPy）
	cs = append(cs, check{"python", py != "",
		pick(py != "", ver(py, "--version"), ""),
		"装 Python 3（python.org），`python --version` 能跑", true})
	// 必需：UnityPy —— 从 AssetBundle 导资源（7.1）
	upOK, upVer := pymod("UnityPy")
	cs = append(cs, check{"UnityPy（导 Unity 资源）", upOK, upVer,
		"`pip install UnityPy`（装完 `python -c \"import UnityPy\"` 不报错）", true})
	// 必需：java —— 跑 jadx（jadx 是 Java 写的）
	cs = append(cs, check{"Java（跑 jadx）", have("java"), ver("java", "-version"),
		"装 JDK/JRE，`java -version` 能跑", true})
	// 必需：jadx —— 反编译 DEX（6.1）
	cs = append(cs, check{"jadx（反编译 DEX）", have("jadx"), "",
		"下 jadx（github.com/skylot/jadx），解压后把 bin/ 加 PATH", true})

	// 建议：REVERSE_TOOLS 工具目录（把下载的工具放一处，可选）
	td := os.Getenv("REVERSE_TOOLS")
	tdOK := td != "" && dirExists(td)
	cs = append(cs, check{"工具目录 REVERSE_TOOLS", tdOK, td,
		"把下载的工具放一处、设环境变量 REVERSE_TOOLS 指向它（可选，更顺手）", false})

	report(cs)
}

func pick(cond bool, a, b string) string {
	if cond {
		return a
	}
	return b
}
func dirExists(p string) bool {
	fi, err := os.Stat(p)
	return err == nil && fi.IsDir()
}

func report(cs []check) {
	reqAllOK := true
	for _, c := range cs {
		mark := "✔"
		if !c.ok {
			if c.required {
				mark = "✘"
				reqAllOK = false
			} else {
				mark = "⚠"
			}
		}
		tag := "（建议）"
		if c.required {
			tag = "（必需）"
		}
		fmt.Printf("  %s %s%s", mark, c.name, tag)
		if c.ok && c.detail != "" {
			fmt.Printf("  —— %s", c.detail)
		}
		fmt.Println()
		if !c.ok {
			fmt.Printf("      %s\n", c.fix)
		}
	}
	fmt.Println()
	if reqAllOK {
		fmt.Println("工具链就绪 ✅ —— 这一课过了。（⚠ 的建议项配上更顺手）")
		fmt.Println(FLAG)
	} else {
		fmt.Println("还没就绪：把上面 ✘（必需）的项装好，再点一次运行检查。")
	}
}
