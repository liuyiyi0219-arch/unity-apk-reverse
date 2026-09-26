# 4.6 作业：修好被改的 luac header，跑它，读出算法拼出的 flag

## 题干

`Reward.tampered.luac` 是爬塔游戏**通关奖励发放脚本**编出来的 Lua 5.1 字节码。它的 header 被改过：标准 `lua`/`unluac` 载入会当场拒绝（报 `bad header` / `non-standard lua format`）。而且 flag **不是明文常量**——`strings` 抠不到 `FLAG{`，它是脚本跑起来时用算法拼出来的。你要让这份字节码能在标准 Lua 5.1 里跑起来、读出它拼出的 flag。kit 里配了一份 Lua 5.1 运行时供你跑。

## 材料

下载 `reward-luac-kit.zip`：

| 文件 | 是什么 |
|---|---|
| `Reward.tampered.luac` | 爬塔通关奖励发放脚本编出来的 Lua 5.1 字节码，**header 被改过**，标准 `lua`/`unluac` 不认（报 `bad header` / `non-standard lua format`）。指令和常量表**一个字节没动**。 |
| `lua5.1/` | 一份 **Lua 5.1 运行时**（`lua.exe` + `lua51.dll`），拿它跑（5.1 的字节码 5.4 跑不了，版本要对上）。 |
| `xlua-demo.apk` | 一个真的 Unity+xLua 游戏，它的 `libxlua.so` 被真改过（见知识点解析——看"改格式"在真实游戏里长什么样）。 |

## 怎么交

```bash
check.exe FLAG{...}
```
