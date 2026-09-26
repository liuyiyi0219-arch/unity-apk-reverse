# 5.5 作业 · 破「整体加密的 QuickJS 字节码」

## 题干

`skills.qjbc.enc` 是爬塔游戏**技能脚本编成的 QuickJS 字节码**，被**整体加密**（`strings` 抠不到任何可读串）。解密后里面有一个字符串就是 flag。你要找到 key、认出算法、离线解密，从解出的字节码里读出 flag，交给 `check.exe`。

## 材料（`qjbc-aes-kit.zip`）

| 文件 | 是什么 |
|---|---|
| `skills.qjbc.enc` | 爬塔技能脚本编成、AES-128-CBC 整体加密的 QuickJS 字节码（你的靶子） |
| `libgame_rodata.bin` | 从 native 抠出的 `.rodata`，含算法指纹 + 解密函数名 + **16B key + 16B IV** |
| `qjbc-run.exe` | 标准 QuickJS 运行时（可跑解密后的字节码验证） |

## 怎么交

```
check.exe FLAG{...}
```
