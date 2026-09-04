// 3.1 作业 · 批改器：出骨架 dump.cs——从 IL2CPP 靶包里把那个 string 成员变量的值挖出来。
// 材料 il2cppflag.apk 装上是个能玩的爬塔小游戏（[攻击]/[重击] 打穿 5 层 Boss），flag 藏在
// Vault31（继承 ClearCodeVault 的"通关码金库"）里的一个明文 string 字面量字段 clearCode。
// 下载 材料/il2cppflag.apk，取出 global-metadata.dat + libil2cpp.so，用 Il2CppDumper 出 dump.cs，
// 找到 Vault31 类的 string 字段（GetClearCode 返回它），它的值就是 flag——因为是明文字面量，
// 也直接在 global-metadata.dat 的字符串字面量表里（stringliteral.json / strings 都能捞到）。
package main

import (
	"fmt"
	"os"
	"strings"
)

const FLAG = "FLAG{urev-3.1-string-literal-in-metadata}"

func main() {
	if len(os.Args) < 2 {
		fmt.Println("== 3.1 作业：出骨架，挖 string 字段的值 ==")
		fmt.Println("下载 材料/il2cppflag.apk（Unity IL2CPP 包）。解开它，取 assets/bin/Data/Managed/Metadata/")
		fmt.Println("global-metadata.dat 和 lib/arm64-v8a/libil2cpp.so，用 Il2CppDumper 出 dump.cs：")
		fmt.Println("找到 Vault31 类里那个 string 成员变量——它的值就是 flag。交上来：")
		fmt.Println("  check.exe FLAG{urev-3.1-...}")
		os.Exit(2)
	}
	got := strings.TrimSpace(os.Args[1])
	if strings.EqualFold(got, FLAG) {
		fmt.Println("对了 ✅ —— 你会出骨架、也知道字符串字面量明文躺在 global-metadata.dat 里了。")
		fmt.Println(FLAG)
		return
	}
	fmt.Println("✘ 不对。用 Il2CppDumper 出 dump.cs 找 Vault31 的 string 字段值；或直接 strings global-metadata.dat | grep FLAG。")
	os.Exit(1)
}
