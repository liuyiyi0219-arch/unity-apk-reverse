# 5.6 作业：抓 puerts 引擎句柄注入 JS，读出运行时全局里的 flag

> ⚠ 本章需 Android 真机 / 模拟器 + frida，无法纯离线完成。

## 题干

一个 puerts 写的**爬塔小游戏**：C# 壳管界面、战斗逻辑全在 `battle.js`，游戏主循环每帧 `env.Eval("__tick(n)")`。flag **只作为运行时 JS 全局** `globalThis.__secret` 活在引擎里——落包 `battle.js` 只有一张编码字节表、没明文（`strings` 抠不到），界面/logcat 也不显。静态拆包（5.1–5.5）读不到；你得抓到活着的引擎、往里读那个全局，把 flag 拿出来。

## 材料

下载 `puerts-inject-kit.zip`：`puerts-inject.apk` + 验证过的 `inject.js` + `drive.py` + 说明。App 每帧 `env.Eval("__tick(n)")`——高频入口就在这。

> **务必 `adb install -r puerts-inject.apk` 覆盖安装**（`-r` 保留数据覆盖装）：本包名是 `com.course.v8inject`。设备上若装过本包的旧版，不覆盖直接装可能失败或注进旧版、读回旧 flag（撞 5.6 校验，白排查）。
>
> `drive.py` 走远程 attach 连 `27055`：先把 florida 起在该端口（`... -l 0.0.0.0:27055`）+ `adb forward tcp:27055`，或改 `drive.py` 回退到默认 `27042`。Unity/puerts 息屏时主循环停帧、`Eval` 不触发——先 `input keyevent 224 82` + `wm dismiss-keyguard` 解锁再注入。

## 怎么交

```bash
check.exe FLAG{...}
```
