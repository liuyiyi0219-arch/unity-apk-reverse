# 4.7 作业：恢复 opcode 映射，把魔改字节码换回标准、跑出 flag

## 题干

`secret.opmod.luac` 是爬塔游戏 **battle 脚本编成的** Lua 5.1 字节码，跑起来会用算法拼出 flag（flag 运行时算法拼，`strings` 抠不到）。它的 opcode 被重排过：标准 lua 跑它报 `bad code`（过了 header、卡在指令校验）。kit 里配了一个真 Unity+xLua 游戏 APK（它的引擎就是能跑这套字节码的魔改 VM）、一组差分助手、以及一份标准 Lua 5.1 运行时。你要让这份字节码能在标准 Lua 5.1 里跑起来、读出它拼出的 flag。

## 材料

下载 `opcode-vm-kit.zip`：

| 文件 | 是什么 |
|---|---|
| `secret.opmod.luac` | 爬塔 battle 脚本编成、opcode 被重排的 Lua 5.1 字节码，**跑起来打印 flag**（flag 运行时算法拼，`strings` 抠不到）。 |
| `reward.vmmod.luac` + `reward.standard.ops.txt` | **差分助手**：同一套映射、且有源码 `reward.lua` 的标准反汇编锚点，供你比出映射表。 |
| `xlua-op-demo.apk` | 真 Unity+xLua 游戏，它的 `libxlua.so` 就是这套魔改 VM（另一条恢复映射的路：逆它）。 |
| `lua5.1/` | 标准 Lua 5.1 运行时（换回标准 opcode 后拿它跑）。 |

## 怎么交

```bash
check.exe FLAG{...}
```
