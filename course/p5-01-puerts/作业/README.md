# 5.1 作业：拆开 V8 puerts 游戏，读明文 JS 里的 flag

## 题干

一个用 **puerts** 写的 Unity **爬塔小游戏**：主体是 C# 壳（uGUI 界面 + 回合循环），**战斗逻辑（伤害 / 出怪 / 奖励 / 通关码）全在 `battle.js` 里**，由 puerts 的 V8 引擎跑。flag 是 `battle.js` 里的一个字符串——游戏能玩、通关也只显示"通关码长度"、绝不显 flag。你要拆开这个包，把 flag 读出来。

## 材料

下载 `puerts-plain-kit.zip`，里面 `puerts-plain-demo.apk`。

## 怎么交

```bash
check.exe FLAG{...}
```
