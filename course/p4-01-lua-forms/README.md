# 4.1　Lua 认形态

> 状态：🟢 已完成（正文 + 演示程序）｜演示程序：`正课/lesson-4.1.exe`（Lua 形态扫描器）｜第四部分开篇

---

## 一、本章的目的

第四部分开篇——脚本层 Lua 是很多 Unity 手游**游戏逻辑的真正所在**（战斗数值、活动、配表全在这），也是本课最厚的一部分。这一课先认**形态**：游戏用没用 Lua、哪个桥接框架、**原生 Lua 还是 LuaJIT**、脚本在 APK 里怎么打包、怎么定位。这是脚本层的**地图**，像 3.7 之于 il2cpp。

> **为什么游戏逻辑在 Lua**（回顾 1.1）：`C# + native` 是引擎 / 框架，**脚本层才是游戏逻辑**。选 Lua 的动机：热更避免强更、策划配表、逻辑与引擎分离。所以逆一个 Lua 游戏，**真正想要的逻辑往往不在 il2cpp dump 里、而在 Lua 脚本里**（和 3.6 的 C# 热更同源）。

---

## 二、操作指南：定位 Lua 的工作流

### 第 1 步 · 扫 native 库 → 用没用 Lua、哪个框架、原生还是 JIT

`lib/arm64-v8a/` 下看有没有 Lua 运行时库：

| 框架 | Lua 运行时 | native 库 | C# 侧特征 |
|---|---|---|---|
| **ToLua** | 原生 Lua 5.1.5 | `libtolua.so` | `LuaState` / `LuaFunction` / `LuaFileUtils`（**本课 demo**） |
| **xLua** | Lua 5.3/5.4 或 LuaJIT | `libxlua.so` | `LuaEnv` / `DoString` / `hotfix` |
| **sLua / uLua** | 原生 Lua / LuaJIT | `libslua.so` 等 | `LuaSvr` |
| puerts | 其实是 **JS/TS** | `libpuerts.so` | `JsEnv`（脚本层另一半，5.1） |

> 加固 / 广告 SDK 的 native `.so`（各家名字不同）不是 Lua 运行时，别当框架。库存在也 ≠ 该方案激活（可能编了没用），行为 / 资源一起看。

### 第 2 步 · 扫资源，找 Lua 载体

搜 `*.lua` / `*.luac` / `*.lua.bytes` / `gamelua*.ab` / 可疑 `.bytes`。

### 第 3 步 · 看 magic，定形态

| magic / 长相 | 是什么 | 怎么办 |
|---|---|---|
| 可读文本 | **① 明文源码** | 直接读（最好的情况） |
| `1B 4C 75 61`（`\x1bLua`） | **② 原生 luac 字节码** | 反编译（4.2，unluac） |
| `1B 4C 4A`（`\x1bLJ`） | **② LuaJIT 字节码** | 反编译（luajit-decompiler-v2；**别拿 unluac 解**） |
| `UnityFS` | **③ 打进 AssetBundle** | UnityPy 解 AB 拿 TextAsset（6.1） |
| 别的 / 乱码 | **④ 加密**（整包 XOR/AES、魔改 magic/header） | 找 key 解（4.8）/ 改工具（4.6–4.7） |
| APK 里根本没有 | **⑤ CDN 下载** | 设备首启后抓落地目录 / 抓包 |

> **原生 Lua vs LuaJIT 是关键分叉**（决定后面用哪套工具）：magic 不同（`\x1bLua` vs `\x1bLJ`）、反编译器不同（unluac vs ljd）、坑不同（LuaJIT 有**版本死锁**：2.0 和 2.1 字节码不兼容，工具要对版本）。**先分清再动手。**

### 第 4 步 · 看 C# 侧，确认从哪加载

搜 `LuaState` / `DoString` / `DoFile` / `require` 的路径——Lua 从哪读、加没加载。（`xLua hotfix` 是"Lua 补丁替换 C# 方法"，逻辑可能散在 `Resources` / `persistentDataPath`。）

### 第 5 步 · 形态判定 + 规划路线

真实游戏常**叠加**几种形态。判定后规划：明文直接读 / 字节码反编译（4.2）/ 解 AB（6.1）/ 解密（4.8）。

> 我们 demo 的形态 = **① 明文源码 → ③ 打进 UnityFS AB → ④ 整包 XOR 0x5A**（真靶 `VampireSurvivors` 也是同一套 ToLua + XOR-AB，它的 `balance.lua` 打进 `gamelua.ab`、XorKey=0x5A——3.9 脱壳时在 dump.cs 的 `LuaBundleConst` 里就把这把 key 露出来了）。

---

## 三、检查点

- 能对一个 Unity 包判断：用没用 Lua、哪个框架、**原生 Lua 还是 LuaJIT**、脚本打包形态。
- 能认出 `\x1bLua` / `\x1bLJ` / `UnityFS` / 明文文本 / 加密 这几种 magic，别把 LuaJIT 当原生 Lua 解。
- 能规划提取路线（明文直接读 / 字节码反编译 / 解 AB / 解密 / CDN 抓）。
- 知道"库存在≠方案激活"、"`.bytes` 不一定是 Lua"、"AB 里可能是 luac 不是源码"。

---

## 动手：运行演示程序

双击 **`正课/lesson-4.1.exe`**（Lua 形态扫描器），对真实的 `CompleteDemo.apk`：扫 native 库 → 命中 `libtolua.so`（ToLua / 原生 Lua 5.1.5）→ 扫资源找到 `assets/gamelua.ab` → 看 magic（`0F 34 33 2E…` 不是 `UnityFS` → 试 XOR `0x5A` → `UnityFS` = 加密的 AB）→ 解 AB 看里面（5 个 Lua TextAsset，明文源码：[`Battle.lua`](../demo/CourseDemo/Assets/GameLua/src/Gameplay/Battle.lua.bytes) 有 `CalcDamage` 伤害公式、[`Hero`](../demo/CourseDemo/Assets/GameLua/config/Hero.lua.bytes)/`Item` 配表）→ 形态判定（ToLua + 明文源码 + UnityFS AB + XOR → 解 AB + 去 XOR 就能读，无需反编译字节码）→ 弹文件夹。

> AB 里的 Lua 是 **LZ4 压缩**的（un-XOR 后直接搜 `function` 搜不到），要 **UnityPy** 解 AssetBundle 才能拿到 TextAsset（6.1 的活）。`正课/sample-output/extracted-lua/` 放了解好的 5 个 Lua 源文件。

---

## 小结 & 下一章

你有了一张 Lua 形态地图 + 一套定位工作流：框架（ToLua/xLua）、运行时（原生/JIT）、打包（源码/字节码/AB/加密/CDN）。下一课 **4.2** 正式动手——**Lua 反编译**：把 `luac` 字节码还原成源码（明文源码直接读，字节码才是本事）。
