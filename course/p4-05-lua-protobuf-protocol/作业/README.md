# 4.5 作业：反推 proto → 改序列化数据 → 重打包 → 通关拿 flag

> ⚠ 本章需 Android 真机 / 模拟器 + frida，无法纯离线完成。

## 题干

一个 **Unity + toLua** 的魔塔式回合制小游戏（主体 C# 壳，战斗逻辑跑在 `libtolua.so` 的 Lua VM 里）：玩家 **HP 只有 1**，被怪物**一刀砍死**，正常玩过不了第 1 层。玩家/关卡数值走一层 **protobuf 协议**、序列化在一份随包数据里——你把这份数据反推出 schema、改掉、塞回去，让自己活着通关，登顶画面就吐 flag。

## 材料

下载 `tower-pb-game.apk`（包名 `com.course.lua45`）。里面两份关键文件（apk 就是 zip，解开就看到，在 `assets/` 下松散放着）：

- `assets/gamedata.bin` —— 游戏运行时读的数据：玩家 HP/攻击 + 每层怪物 + 通关码。出厂 `player_hp = 1`。
- `assets/gamedata_pb.lua` —— `protoc-gen-lua` 的 schema 定义（你要反推的对象）。

## 怎么交

进游戏一路通关，登顶画面显示 flag：

```bash
check.exe FLAG{...}
```
