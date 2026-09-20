# 5.3 作业 · 破「改格式的 QuickJS 字节码」

## 题干

`secret.fmt.qjbc` 是爬塔游戏 **battle 脚本编成的 QuickJS 字节码**（`qjsc -b` 产物），跑起来会用算法拼出一个 `FLAG{...}` 并打印。但它的**头部被魔改**过，标准 QuickJS 运行时读头部时直接拒。你要修好头部、用标准运行时把它跑起来，拿到打印出的 flag，交给 `check.exe`。

## 材料（`qjbc-format-kit.zip`）

| 文件 | 是什么 |
|---|---|
| `secret.fmt.qjbc` | 爬塔 battle 脚本编成、被魔改 version 头的 QuickJS 字节码（你的靶子） |
| `qjbc-run.exe` | **标准** QuickJS 字节码运行时（读一个 `.qjbc` 并执行；认标准版本 `0x1b`） |
| `qjbc-run-mod.exe` | **魔改**运行时（认 `0x42` 的那个 VM；对应真实游戏 `libpuerts.so` 里的 QuickJS） |
| `qjs-mod-src/` | 魔改 recipe：一行 `BC_VERSION 27 → 0x42` 的改动说明 |

## 怎么交

```
check.exe FLAG{...}
```
