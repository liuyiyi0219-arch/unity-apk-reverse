# 6.1 作业：无混淆 dex，jadx 反编译找 flag

## 题干

`dexflag-demo.apk` 是一个**能玩的 Unity 爬塔小游戏**（回合制战斗，点 [攻击] / [重击] 打怪爬 5 层），**没做混淆 / 加壳**。游戏主逻辑在 C#（IL2CPP），但 flag **不在 C# 层**——它在一个 Java 层的 Android 插件类 `com.course.dexflag.FlagHolder` 里，`reward()` 方法**运行时拼**出来。界面 / logcat 不显、把塔玩通关也拿不到，`strings` 只抠到碎片。你要把完整的 flag 找出来。

## 材料（`dex-decompile-kit.zip`）

| 文件 | 是什么 |
|---|---|
| `dexflag-demo.apk` | 能玩的 Unity 爬塔（没混淆 / 没壳），flag 在 Java 层插件类 `FlagHolder` 里 |

## 怎么交

算出成品 flag，交给批改器：

```bash
check.exe FLAG{...}
```
