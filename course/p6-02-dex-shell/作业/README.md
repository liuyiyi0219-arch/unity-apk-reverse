# 6.2 作业：dex 有壳，脱壳 → 反编译找 flag

## 题干

`dexshell-demo.apk` 是一个**能玩的 Unity 爬塔小游戏**（回合制战斗，点 [攻击] / [重击] 爬 5 层塔），Java 层**加了 dex 壳**。游戏主体在 C#（IL2CPP），但真正的 flag 类 `com.course.dexshell.SecretFlag` **不在 `classes.dex` 里**——直接看 `classes.dex` 只看到壳的加载器、看不到 flag 类，把塔玩通关也拿不到 flag。你要认出壳、把真 dex 弄出来、读出 flag。

## 材料（`dex-shell-kit.zip`）

| 文件 | 是什么 |
|---|---|
| `dexshell-demo.apk` | 能玩的 Unity 爬塔（Java 层加壳），真 flag 类藏在加密的隐藏 dex 里 |

## 怎么交

读出真 dex 里的 flag，交给批改器：

```bash
check.exe FLAG{...}
```
