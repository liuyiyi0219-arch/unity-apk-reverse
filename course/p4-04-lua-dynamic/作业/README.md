# 4.4 作业：hook `luaL_loadbuffer`，dump 出解密后的脚本读 flag

> ⚠ 本章需 Android 真机 / 模拟器 + frida，无法纯离线完成。靶包内 `.so` 只有 `arm64-v8a`，须用 **arm64（64 位）** 设备 / 模拟器。

## 题干

材料是一个 **Unity + toLua 爬塔小游戏**：游戏主体（UI + 回合循环）在 C#，**战斗数值逻辑全在一份 `battle.lua` 里**——伤害、出怪、奖励、通关码都问 lua 要。`battle.lua` 在包里是 **AES 加密**的（打进 Unity 资源，`strings` 抠不到明文）；第一次点 **[攻击]** 时，C# 把它解密成明文，送进 `libtolua.so` 的 `luaL_loadbuffer` 编译执行。flag 是 `battle.lua` 里的字符串字面量（内测通关码）——**盘上抠不到，通关也拿不到**（通关只把通关码的长度打日志、绝不显示）。**你不用破那个 AES**：脚本送进 VM 编译前一定是明文，hook `luaL_loadbuffer` 读它拿到的 buffer 就是明文源码，flag 在里面。

## 材料

下载 `lua-loadbuffer-hook.apk`（Unity + toLua 爬塔）。装上、进游戏能玩：点 [攻击]/[重击] 打怪、爬 5 层塔。所有战斗数值都由 `battle.lua`（AES 加密随包）在运行时算出。

## 怎么交

```bash
check.exe FLAG{...}
```
