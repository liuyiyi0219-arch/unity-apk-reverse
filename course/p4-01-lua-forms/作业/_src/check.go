// 4.1 作业 · 批改器（check.exe）
//
// 材料/blob1.bin … blob5.bin 是 5 个脚本 blob，各是一种 Lua 形态：
//   明文源码 / 原生 luac 字节码(\x1bLua) / LuaJIT 字节码(\x1bLJ) / 单字节 XOR 加密 / 分组加密(XXTEA 一类)。
// flag 藏在【被单字节 XOR 加密】的那个 blob 里——un-XOR 出来是 Lua 源码，里面有 flag。
//
// 破法（就是 4.1 的核心技能）：
//   1. 认形态：哪个是明文、哪个是字节码(有 magic)、哪个是高熵分组加密、哪个是单字节 XOR；
//   2. flag 在“被加密的脚本”里——分组加密(XXTEA)这一课解不了(→4.8)，所以是那个单字节 XOR 的；
//   3. 对 0–255 爆破 key、按 lua-token 密度(local/function/return/end)挑出解得出 Lua 的那个 key；
//   4. un-XOR → 读 Lua 源码里的 flag；
//   5. 交上来： check.exe FLAG{...}
//
// （明文 blob 里放的是【诱饵 flag】——直接读明文会被骗；真 flag 得 un-XOR 才拿得到。）
package main

import (
	"fmt"
	"os"
	"strings"
)

const FLAG = "FLAG{urev-4.1-xor-lua-cracked}"

func main() {
	enableUTF8Console()
	if len(os.Args) < 2 {
		fmt.Println("== 4.1 作业：认 Lua 形态，脱掉 XOR，读出脚本里的 flag ==")
		fmt.Println("材料 blob1..5 各是一种 Lua 形态（明文/luac/LuaJIT/单字节XOR/XXTEA）。")
		fmt.Println("flag 在【被加密的脚本】里：XXTEA 这课解不了(→4.8)，所以是那个单字节 XOR 的 blob。")
		fmt.Println("步骤：① 认出哪个是单字节 XOR（不是明文、无 \\x1bLua/\\x1bLJ magic、也不是高熵分组加密）；")
		fmt.Println("      ② 对 0–255 爆破 key，挑解得出 Lua 的那个；③ un-XOR 读 Lua 里的 flag；")
		fmt.Println("      ④ 交上来： check.exe FLAG{...}")
		fmt.Println("（明文 blob 里那个 flag 是诱饵，别交它。）")
		os.Exit(2)
	}
	got := strings.TrimSpace(os.Args[1])
	if strings.EqualFold(got, FLAG) {
		fmt.Println("对了 ✅ —— 你认出了单字节 XOR 形态、爆破出 key、un-XOR 读到了脚本里的 flag。")
		fmt.Println("Lua 层逆向的第一刀就是认形态：明文直接读、字节码反编译(4.2)、XOR 爆破、分组加密找 key(4.8)。")
		fmt.Println(FLAG)
		return
	}
	if strings.Contains(strings.ToLower(got), "plaintext-is-a-decoy") {
		fmt.Println("✘ 那是明文 blob 里的【诱饵 flag】——直接读明文会被骗。真 flag 在被 XOR 加密的那个 blob 里。")
		os.Exit(1)
	}
	fmt.Println("✘ 不对。flag 在【单字节 XOR 加密】的 blob 里(不是明文、不是 \\x1bLua/\\x1bLJ 字节码、不是高熵 XXTEA)。")
	fmt.Println("  对 0–255 爆破它的 key、挑解出合法 Lua 的那个，un-XOR 后 grep FLAG。")
	os.Exit(1)
}
