# 7.2 作业：解开部分加密的 AB，提图读 flag

## 题干

`cryptopack.enc` 是爬塔游戏的**通关奖励包**（一个 AssetBundle，"第 60 层 · 通关奖励"横幅贴图上写着 flag）。但它被**加密**了——直接用 UnityPy / AssetStudioMod 提，**提出 0 个对象**（容器目录被盖住、进不去）。你要把它解开、提出那张贴图、读图上的 flag。

## 材料（`ab-crypto-kit.zip`）

| 文件 | 是什么 |
|---|---|
| `cryptopack.enc` | 被部分加密的爬塔通关奖励包（AssetBundle），里面一张奖励横幅贴图上写着 flag |

## 怎么交

读到图上那行 `FLAG{...}`，交给批改器：

```bash
check.exe FLAG{...}
```
