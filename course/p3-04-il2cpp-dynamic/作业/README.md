# 3.4 作业：hook 一个方法，读它的入参 flag

## 题干

点面板上的 **⬇ 下载材料**，拿到 `il2cppflag34.apk`。装上是个能点着玩的**爬塔小游戏**（`[攻击]`/`[重击]` 打穿 5 层 Boss 通关）。游戏里有个方法 `Vault34.Validate(string candidate)`，**它的入参就是 flag**——打穿 Boss 通关时游戏会调一次它，但方法本身只做个校验、**不把 flag 打出来**，flag 也是运行时 AES 解码的（`strings` 抠不到）。你要在它运行时把这个入参读出来。

## 材料

| 文件 | 是什么 |
|---|---|
| `il2cppflag34.apk` | Unity IL2CPP 靶包，装上是个能玩的爬塔小游戏。通关时会调 `Vault34.Validate(string candidate)`，它的入参就是 flag（运行时 AES 解出、方法本身不打印） |

## 怎么交

塔里点这个节点 → **✍ 做作业** → 把 hook 读到的 flag 填进去 → **▶ 运行批改**。
