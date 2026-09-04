# 6.1　DEX 反编译

> 状态：🟢 已完成（正文 + 真 Unity APK 靶，jadx 真跑）｜第六部分（Java / DEX）

---

## 一、本章的目的

APK 的 Java 层编成 Dalvik 字节码打包成 `classes.dex`。**没做混淆 / 加壳时，jadx 直接把 dex 反编译回可读 Java**——类名、方法名、字符串、控制流全在。这一课就练这条最常见的路：提 dex → jadx 反编译 → 读逻辑 / 找字符串。下一课 6.2 讲加了壳怎么办。

> **DEX 是什么**：APK 本质是个 zip，Java 字节码编成 Dalvik 字节码打包成 `classes.dex`；代码多了拆成 `classes2.dex`…（单 dex 65536 方法数上限 → multi-dex）。工具 `jadx`（本仓库放在 `~/.reverse-tools/jadx-1.5.1/`）。

---

## 二、操作指南

### 第 1 步 · 提取 dex

APK 是 zip，直接解出 `classes*.dex`（`unzip` 或任意 zip 库，不需专门工具）。

> **XAPK / split**：现代 Play 分发常是 `base.apk` + `config.<abi/density/lang>.apk`，dex 在 **`base.apk`** 里、split 里只有 native `.so` 和资源。先把 base 拆出来。

### 第 2 步 · jadx headless 反编译

```bash
jadx --no-res --no-debug-info -d <输出目录> <apk 或 dex>
```

`--no-res` 跳过资源解码（只要代码更快）、`--no-debug-info` 输出更干净。反编译结果在 `<输出目录>/sources/` 下按包名铺开，直接读 `.java`。jadx 只反编译 dex、不碰 `.so`（native 在第三部分 3.8 讲）。

> **两个真实调用坑**：① headless 用官方启动脚本 `jadx.bat`/`jadx`（内部 `java -cp jadx-all.jar jadx.cli.JadxCLI`）最稳；`-jar jadx-all.jar` 或 `jadx-gui` 会弹 GUI、在无头环境写空目录。② jadx 吃内存，OOM 时换 `-XX:+UseSerialGC` 或分批喂 dex（一次几个 `classes*.dex`）——大 app 的 OOM 常是 G1GC 内存碎片，换 GC / 分批就过。

### 第 3 步 · 读反编译结果：分类 + 定位

反编译出的类按顶层包归类，一眼分三类：

| 占比 | 内容 |
|---|---|
| 大头 | `com/unity3d/player/*`（**Unity 启动器**，引擎自带家家一样） |
| 一批 | `com/google/*`、`org/fmod/*`、`bitter/jnibridge/*`（第三方 SDK / 引擎粘合：登录 / 支付 / 广告 / 推送 / JNI 桥） |
| 少数 | 应用 / 厂商自己的包——**要读的东西在这里**：自定义 Activity、Android 插件、Java 层的校验 / 反调试 / 支付回调 |

**对 Unity 游戏，主逻辑在 C#（IL2CPP 的 `.so`），不在 dex**。所以 dex 里值得看的通常是两处：
- **通往 C# 的 native 桥**：`System.loadLibrary("main")` → 拉起 `libil2cpp.so`；`native` 声明（只有声明没函数体）= java ↔ C++ 的 JNI 边界，实现在 `.so` 里。想追游戏逻辑，从这里跳出 dex、进第三部分。
- **应用自己那几个类**：Android 插件、Java 层校验 / SDK 关键逻辑——**没混淆时直接读**，字符串、算法一目了然。本课作业的 flag 就藏在这种自定义类里。

### 第 4 步 · 找字符串 / flag

没混淆时，`jadx` 反编译出的 Java 和源码几乎一样：`grep -r "FLAG" <输出目录>/sources` 就能扫到硬编码字符串、key、URL；读方法体就能看懂拼接 / 校验逻辑。**运行时才拼出来的字符串**（`strings` 抠不到成品），反编译能读到**拼法**——照着拼法算出来即可。

---

## 三、检查点

- 能从 APK（含 XAPK/split）提取 `classes*.dex`（dex 在 `base.apk`），知道 multi-dex 是怎么回事。
- 会用 jadx headless 反编译，知道两个坑（点名 `jadx.cli.JadxCLI` / OOM 换 SerialGC / 分批）。
- 能对反编译结果分类：Unity 启动器 / 第三方 SDK / 应用自己的包，知道**要读的在应用自己的包**。
- 会在无混淆的反编译结果里读逻辑、`grep` 字符串、照着运行时拼法算出 flag。
- 知道对 Unity 游戏 dex 通常不是主逻辑（在 IL2CPP），dex 值得看的是 native 桥 + 应用自己那几个类。

---

## 动手

- **作业（反编译找 flag）**：下载 `作业/材料/dex-decompile-kit.zip` 里 `dexflag-demo.apk`——一个**能玩的 Unity 爬塔小游戏**（回合制战斗，点 [攻击] / [重击] 爬 5 层塔；**没做混淆 / 加壳**）。游戏主逻辑在 C#（IL2CPP），但 flag 落在 Java 层：一个厂商自定义插件类 `com.course.dexflag.FlagHolder` 运行时拼出 flag（界面/logcat 不显、通关也拿不到）。unzip apk → 取 `classes.dex` → jadx 反编译 → 在应用自己的包（`com.course.dexflag`）里找那个类、读它拼 flag 的逻辑、算出 flag。任务详情见 `作业/README.md`。

---

## 小结 & 下一章

没混淆 / 没壳时，dex 逆向就一句话——**jadx 一把过，反编译回可读 Java 直接读**。对 Unity 游戏，dex 里主要是启动器 + SDK + 应用自己那几个类（主逻辑在 IL2CPP 的 `.so`），要读的在应用自己的包。下一课 **6.2** 讲另一半：**dex 被加壳**了怎么办——`classes.dex` 只剩个加载器、真代码藏在运行时才解密加载的隐藏 dex 里，得先脱壳。
