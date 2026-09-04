// 7.1 作业 · 批改器（check.exe）—— 无保护资源提取：从 AB 里把图导出来、读图上的 flag
//
// 材料 ab-image-kit.zip 里 rewardpack 是爬塔游戏的通关奖励包——一个没加密的标准 UnityFS AssetBundle。
// 里面有一张 Texture2D（通关奖励横幅），flag 就写在图上（不是文件里的字符串，是图片像素——strings 抠不到）。
//
// 破法（本课技能）：
//   1. 用 AssetStudioMod（GUI 拖进去，或 AssetStudioModCLI rewardpack -t tex2d -o out）
//      或 UnityPy（env=UnityPy.load("rewardpack"); 遍历 Texture2D → data.image.save）把贴图导成 PNG；
//   2. 打开导出的 PNG，读图上那行 FLAG{...}；
//   3. 交上来： check.exe FLAG{...}
package main

import (
	"fmt"
	"os"
	"strings"
)

const FLAG = "FLAG{urev-7.1-ab-image}"

func main() {
	enableUTF8Console()
	if len(os.Args) < 2 {
		fmt.Println("== 7.1 作业：从 AssetBundle 里把图导出来，读图上的 flag ==")
		fmt.Println("材料 rewardpack：没加密的标准 UnityFS AssetBundle，里面一张贴图上写着 flag（图片像素，strings 抠不到）。")
		fmt.Println("步骤：① AssetStudioMod（GUI/CLI）或 UnityPy 把 Texture2D 导成 PNG；")
		fmt.Println("      ② 打开 PNG 读图上那行 FLAG{...}；③ 交上来： check.exe FLAG{...}")
		fmt.Println("（没加密 → 丢进工具点导出即可；flag 在图片像素里，不是文件字符串。）")
		os.Exit(2)
	}
	got := strings.TrimSpace(os.Args[1])
	if strings.EqualFold(got, FLAG) {
		fmt.Println("对了 ✅ —— 你把没加密的 AssetBundle 里的贴图导成了 PNG、读出了图上的 flag。")
		fmt.Println("没保护时资源提取就这么简单：丢进 AssetStudioMod / UnityPy，点导出、读。")
		fmt.Println("（AssetStudio 原版停维护，用活跃 fork AssetStudioMod；UnityPy 适合脚本化批处理。）")
		fmt.Println(FLAG)
		return
	}
	fmt.Println("✘ 不对。flag 写在 rewardpack 里那张贴图上（图片像素，不是文件字符串）：")
	fmt.Println("  · AssetStudioModCLI rewardpack -t tex2d -o out（或 UnityPy 遍历 Texture2D → data.image.save）；")
	fmt.Println("  · 打开导出的 PNG，读图上那行 FLAG{...}。")
	os.Exit(1)
}
