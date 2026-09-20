# 5.2 作业：去混淆 V8 puerts 的 JS，读出 flag

## 题干

和 5.1 同款 puerts（V8）爬塔小游戏——C# 壳管界面、战斗逻辑全在 `battle.js`，通关只显示"通关码长度"不显 flag——但这版的 `battle.js` 做了源码级混淆（字符串数组 + base64 + 控制流平坦化）。flag 编在混淆 JS 里，`strings` 抠不到（成了 base64 字符串数组）。你要把 JS 还原出来，读到 flag。

## 材料

下载 `puerts-obf-kit.zip`，里面 `puerts-obf-demo.apk`。

## 怎么交

```bash
check.exe FLAG{...}
```
