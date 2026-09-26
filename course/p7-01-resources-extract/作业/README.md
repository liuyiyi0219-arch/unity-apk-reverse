# 7.1 作业：从 AssetBundle 里把图导出来，读图上的 flag

## 题干

`rewardpack` 是爬塔（Tower Climb）游戏的**通关奖励包**——一个**没加密**的标准 `UnityFS` AssetBundle。里面有一张 `Texture2D`（"第 30 层 · 通关奖励"横幅），**flag 就写在图上**——是图片像素，不是文件里的字符串（`strings` 抠不到）。你要把这张贴图导成 PNG、打开读图上的 flag。

## 材料（`ab-image-kit.zip`）

| 文件 | 是什么 |
|---|---|
| `rewardpack` | 爬塔通关奖励包（没加密的 UnityFS AssetBundle），里面一张奖励横幅贴图上写着 flag |

## 怎么交

打开导出的 PNG，图上那行 `FLAG{...}` 就是答案：

```bash
check.exe FLAG{...}
```
