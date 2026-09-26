# 4.9 作业：抓 `lua_State` 注入 Lua，读出运行时全局里的 flag

> ⚠ 本章需 Android 真机 / 模拟器 + frida，无法纯离线完成。

## 题干

`luainject.apk` 是一个 **Unity + toLua** 的爬塔小游戏（包名 `com.course.lua49`，主体 C# 壳，战斗逻辑跑在 `libtolua.so` 的 Lua 5.1 VM 里）。flag **只作为一个运行时 Lua 全局** `__secret` 活在 `lua_State` 里——脚本文本里没有明文（`strings` 抠不到），界面/logcat 也不显。静态拆包读不到；你得抓到活着的 `lua_State`、往里读那个全局。C# 每帧调一次 `lua_pcall` 跑一小段 Lua（心跳）。

## 材料

`作业/材料/` 下：

| 文件 | 是什么 |
|---|---|
| `luainject.apk` | Unity + toLua 爬塔，flag 只作为运行时全局 `__secret` 活在 `lua_State` 里。C# 每帧调一次 `libtolua.so!lua_pcall` 跑心跳。 |
| `inject_secret.js` | 参考答案脚本：hook `lua_pcall` 抓 `lua_State*`、注入 `return __secret` 读回。 |

## 怎么交

```bash
check.exe FLAG{...}
```
