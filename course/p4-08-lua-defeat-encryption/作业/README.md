# 4.8 作业：找 key 解密整体加密的 luac，读出代码里的 flag

## 题干

`skills.luac.enc` 是爬塔游戏的**技能脚本** luac，被整体加密过——`strings` 抠不到任何东西，整个是一坨密文。kit 里另给了一段从游戏 native/il2cpp 抠出来的 `.rodata` 片段 `libgame_rodata.bin`，解密相关的常量都在里面。你要把这份 luac 解出来、读出明文代码里的 flag。

## 材料

下载 `lua-aes-kit.zip`：

| 文件 | 是什么 |
|---|---|
| `skills.luac.enc` | 爬塔技能脚本 luac，被 **AES 加密**过（`strings` 抠不到任何东西，整体密文）。 |
| `libgame_rodata.bin` | 从游戏 native/il2cpp 抠出来的 `.rodata` 片段，解密相关的常量都在里面。 |

## 怎么交

```bash
check.exe FLAG{...}
```
