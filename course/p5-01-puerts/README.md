# 5.1　puerts 多后端 / 明文 JS

> 状态：🟢 已完成（正文 + 演示程序 + 真 puerts 游戏靶）｜演示程序：`正课/lesson-5.1.exe`｜Part 5 · 脚本层的另一半

---

## 一、本章的目的

脚本层除了 Lua，另一大半是 **puerts**——用 **TypeScript/JS** 写游戏逻辑。这一部分单开一章讲它，因为 TS/JS 这条路和 Lua 很不一样，尤其是**它有多个 JS 后端，逆向面完全取决于用了哪个后端**。本章：认出 puerts、说清各后端的逆向形态、并逆一个**真 puerts 游戏**的 `libpuerts.so` + 脚本。

> **puerts 是什么**：`C#+native` 是引擎，脚本层写逻辑——Lua 是一条路，puerts（TS/JS）是另一条。链路 `TS → tsc 编成 JS → JS 后端执行`（puerts 桥接 C# ↔ JS）。识别：native 库 `libpuerts.so`、C# 侧 `Puerts`/`JsEnv`。

---

## 二、多后端 —— 这决定逆向面（本章核心）

puerts 支持多个 JS 后端，各自性能 / 安全 / 逆向形态不同：

| 后端 | 执行方式 | 性能 | 安全形态 | 逆向面 |
|---|---|---|---|---|
| **V8**（主流） | JIT 编译执行 | **好** | **只能明文 JS + 源码级混淆** | 抠出 JS → 去混淆 → 读逻辑 |
| **QuickJS** | 解释执行（字节码） | 差 | **可魔改 VM / 字节码混淆** | 逆 VM dispatch / 解字节码（同 4.7 思路） |
| **Node.js** | 带完整 Node + V8 | 好、包最重 | 同 V8（明文 JS） | 同 V8，实战罕见 |

### ★ V8 后端：生产环境一定是明文 JS

很多人以为可以把 JS 编成 V8 字节码（`produceCachedData` / bytenode / 商业版 "V8CC"）来防逆。**对 V8 后端，这在生产里不可控**：

- **JS 的反射特性依赖运行时的明文信息**（`Object.keys`、`for...in`、`fn.toString()`、`arguments`、按名访问属性…）。把源码编成字节码后，这些明文元信息还得在运行时可得，**没法真正抹掉**；
- V8 cachedData 还**死锁 V8 版本**（跨版本必 reject，见下），生产里维护成本高、不可控。

所以**用 V8 后端的 puerts 游戏，脚本层一定是明文 JS**（打进包里的 `.js`）。安全只能加在**源码层**——用 JS 混淆器（改名 / 控制流平坦化 / 字符串数组 / 死代码），代表作 [`javascript-obfuscator`](https://github.com/javascript-obfuscator/javascript-obfuscator)。**逆 V8 puerts = 抠出那份（可能被混淆的）JS，去混淆、读逻辑**（webcrack / 手工还原控制流）。

### QuickJS 后端：这条才谈得上"字节码 / 魔改 VM"

QuickJS 是**解释执行**（把 JS 编成 QuickJS 自己的字节码再解释）。正因为有一个自己实现的解释器，厂商**可以魔改这个 VM**（改字节码编码 / opcode，同 Lua 4.7 那套），或发 `qjsc` 编的字节码——所以"字节码混淆 / 魔改 VM"在 QuickJS 上才成立。代价是**性能差**，很多重逻辑游戏不选它。逆法回到"逆 VM dispatch 恢复映射"（参见 4.7）。

---

## 三、逆 V8 puerts（本章主线，配套真游戏）

### 第 1 步 · 认 puerts + 定后端

`libpuerts.so` 在，就是 puerts；再看同目录有没有 `libv8`/`libnode` 之类判后端（V8 / Node / QuickJS）。C# 侧 `JsEnv`。

### 第 2 步 · 抠出 JS（V8 = 明文）

JS 打进包里（`Resources`/`StreamingAssets` 里的 `.js`，或 bundle 进 C#）。抠出来直接就是可读文本——**如果没混淆，直接读**；混淆了，就是下一步。

### 第 3 步 · 混淆了就去混淆

没混淆直接读（本课作业就是这种）。真实游戏一般会上 `javascript-obfuscator`（改名 + 字符串数组编码 + 控制流平坦化）——那就去混淆：`webcrack` 一把过，或手工还原字符串数组 + 反平坦化。**这层防守 + 去混淆的细节，单开一课 5.2 讲。**

### 第 4 步 · 动态 hook（JS 是动态语言，最省事）

JS 和 Lua 一样运行时可重赋值函数 = hook（同 4.4）：
```js
const orig = Battle.CalcDamage;
Battle.CalcDamage = (atk, def, crit) => { orig(atk, def, crit); return 99999; };
```
或 native 侧 hook `libpuerts.so` 的加载点 / V8 `Script::Compile` 拿源码（frida，通道见 [0.2](../p0-02-frida-setup/README.md)）。

> **万一真碰到 V8 字节码**（少数强行上 cachedData 的）：第一件事是**定 V8 版本**（cachedData 头内嵌 VersionHash，跨版本必 reject）——puerts 2.x≈V8 11.8.172、3.x≈13.6.233.17。对上版本才能用同版本 `d8`/`node --print-bytecode` 反汇编，或 **View8**（`suleram/View8`）反编译。但 cachedData 里字符串仍明文，`strings` 先捞骨架最快。

---

## 四、检查点

- 能识别 puerts（`libpuerts.so`/`JsEnv`），并说清**逆向面取决于后端**：V8 / QuickJS / Node。
- 能说清**为什么 V8 后端生产里一定是明文 JS**（反射依赖运行时明文，字节码不可控 + 版本死锁），安全只能加**源码级混淆**（javascript-obfuscator）。
- 会抠出 puerts 游戏的 JS、对 javascript-obfuscator 那类做基本去混淆（字符串数组 / 反平坦化），读出逻辑。
- 知道 QuickJS 后端才谈得上"字节码 / 魔改 VM"（性能差，逆法同 4.7）。

---

## 动手

- **演示（`正课/lesson-5.1.exe`，需 node）**：TS → JS → 展示明文 JS；跑一段被 `javascript-obfuscator` 混淆过的 JS，演示字符串数组还原 + 反平坦化把它读回来；node 真跑 JS monkey-patch（`231 → 99999`）；讲多后端对照表。
- **作业（真 puerts 游戏，明文）**：下载配套的 **V8 puerts 游戏 APK**（`puerts-plain-demo.apk`）——一个能玩的**爬塔 mini-game**：C# 壳管界面，**战斗逻辑全在 `battle.js`**（脚本层是**明文 JS**、没做混淆），通关也只显示"通关码长度"不显 flag。unzip apk、认 `libpuerts.so`（V8 后端）、从 `assets/bin/Data/` 翻出脚本 JS 直接读，里面那行 flag 就是答案。**拆开就能看到，正是"V8 = 明文 JS"这一课的体感**（加了源码级混淆、要去混淆才读到的版本 → 5.2）。任务详情见 `作业/README.md`。

---

## 小结 & 下一部分

puerts 是脚本层的另一半：TS/JS，**逆向面看后端**——V8（主流、高性能）在生产里一定是**明文 JS + 源码级混淆**（逆 = 抠 JS + 去混淆）；QuickJS 解释执行、才谈得上魔改 VM / 字节码（同 4.7）。puerts 5.x 分两课：**5.1 明文 JS**（拆包即见，本课）+ **5.2 源码级混淆**（javascript-obfuscator，去混淆）。下一课 **5.2** 就看真实 V8 puerts 唯一那层防守——源码混淆——怎么破。
