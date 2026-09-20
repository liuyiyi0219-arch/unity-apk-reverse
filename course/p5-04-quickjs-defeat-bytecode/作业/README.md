# 5.4 作业 · 破「改字节码 / 魔改 VM 的 QuickJS 字节码」

## 题干

`secret.op.qjbc` 是爬塔游戏 **battle 脚本编成的 QuickJS 字节码**，跑起来会用算法拼出一个 `FLAG{...}` 并打印。它的 **header/version 完全正常**（不是 5.3 那类）：标准运行时能读进去，却崩在执行阶段。你要让它正确跑起来，拿到打印出的 flag，交给 `check.exe`。

## 材料（`qjbc-opcode-kit.zip`）

| 文件 | 是什么 |
|---|---|
| `secret.op.qjbc` | 爬塔 battle 脚本编成、opcode 被重排的 QuickJS 字节码（你的靶子，运行时拼 flag） |
| `sample.js` | 已知源码（差分用，源码公开） |
| `sample.stock.qjbc` | `sample.js` 的**标准** qjsc 产物 |
| `sample.op.qjbc` | `sample.js` 的**魔改** qjsc 产物 |
| `diff-opcodes.py` | 差分助手：逐字节比两份、排除校验和、读出 opcode 置换 |
| `qjbc-run.exe` | **标准** QuickJS 运行时（跑 `secret.op` 会崩在执行阶段） |
| `qjbc-run-op.exe` | **映射正确**的运行时（opcode 表按恢复映射重编；对应游戏 `libpuerts.so` 里的 QuickJS） |
| `qjs-op-src/` | 魔改 recipe：opcode 互换的那几行（`quickjs-opcode.h`） |

## 怎么交

```
check.exe FLAG{...}
```
