// 6.1 作业 · 批改器（check.exe）—— 无混淆 dex：jadx 反编译找 flag
//
// 材料 dex-decompile-kit.zip 里 dexflag-demo.apk 是一个能玩的 Unity 爬塔小游戏（没做混淆/加壳）。
// 游戏主体在 C#（IL2CPP），但 flag 不在 C# 层——它在 Java 层的 Android 插件类
// com.course.dexflag.FlagHolder，flag 在 reward() 方法里运行时拼出来（界面/logcat 不显、
// 把塔玩通关也拿不到，strings 只抠到碎片）。没混淆 → jadx 反编译 classes.dex 直接读到拼法。
//
// 破法（本课技能）：
//   1. unzip apk → 取 classes.dex；
//   2. jadx --no-res --no-debug-info -d out classes.dex；
//   3. 在 out/sources/com/course/dexflag/ 里读 FlagHolder.reward()——照它的拼法算出 flag；
//   4. 交上来： check.exe FLAG{...}
package main

import (
	"fmt"
	"os"
	"strings"
)

const FLAG = "FLAG{urev-6.1-dex-decompiled}"

func main() {
	enableUTF8Console()
	if len(os.Args) < 2 {
		fmt.Println("== 6.1 作业：无混淆 dex，jadx 反编译找 flag ==")
		fmt.Println("材料 dexflag-demo.apk：Unity 游戏（没混淆/没壳），flag 在自定义类 com.course.dexflag.FlagHolder 里运行时拼。")
		fmt.Println("步骤：① unzip apk 取 classes.dex；② jadx -d out classes.dex；")
		fmt.Println("      ③ 读 out/sources/com/course/dexflag/FlagHolder.reward()，照拼法算出 flag；")
		fmt.Println("      ④ 交上来： check.exe FLAG{...}")
		fmt.Println("（flag 运行时拼，strings 只抠到碎片——没混淆 jadx 反编译能读到完整拼法。）")
		os.Exit(2)
	}
	got := strings.TrimSpace(os.Args[1])
	if strings.EqualFold(got, FLAG) {
		fmt.Println("对了 ✅ —— 你 jadx 反编译了无混淆的 classes.dex、在应用自己的包里读出了拼 flag 的逻辑。")
		fmt.Println("这就是最常见的一条 dex 路：没混淆/没壳，jadx 一把过、反编译回可读 Java 直接读。")
		fmt.Println("（Unity 游戏主逻辑在 IL2CPP；dex 值得看的是 native 桥 + 应用自己那几个类，flag 就在后者。）")
		fmt.Println(FLAG)
		return
	}
	fmt.Println("✘ 不对。flag 在 dexflag-demo.apk 的 classes.dex 里、com.course.dexflag.FlagHolder 运行时拼出来：")
	fmt.Println("  · unzip apk 取 classes.dex；jadx -d out classes.dex；")
	fmt.Println("  · 读 out/sources/com/course/dexflag/FlagHolder.reward()，照拼法算出那行 FLAG{...}。")
	os.Exit(1)
}
