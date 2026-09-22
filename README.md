<div align="center">

# Unity APK 逆向入门

**动手扒开一个 Unity 手游 —— 把里面的 C# / Lua / 资源 / Java 一层层读出来。**

零基础也能跟着做：每章都有可运行的演示 + 自动批改的作业，做对点亮、像爬塔一样通关。

[![课程](https://img.shields.io/badge/课程-40_章-2ea44f)](#-课程内容40-章)
[![平台](https://img.shields.io/badge/平台-Windows_+_Android-informational)](#-快速开始)
[![引擎](https://img.shields.io/badge/引擎-Unity_IL2CPP-222)](#)
![形态](https://img.shields.io/badge/形态-可运行演示_+_自动批改-orange)
[![Stars](https://img.shields.io/github/stars/liuyiyi0219-arch/unity-apk-reverse?style=flat&logo=github)](https://github.com/liuyiyi0219-arch/unity-apk-reverse/stargazers)

</div>

## 🎮 同一场战斗，改一个数值前后

| 破解前 · 小怪吃 0 伤害，砍不动 | 破解后 · 一刀一个，秒杀 |
|:---:|:---:|
| <img src="assets/before.gif" width="380"> | <img src="assets/after.gif" width="380"> |

<div align="center"><sub>解开脚本层、把伤害/无敌开关的数值改掉、重打包，同一个游戏的战斗就完全不一样了。</sub></div>

---

## ✨ 你会学到

- **拆解 Unity APK 的五层结构** —— 认清 native / C# / 脚本 / 资源 / Java 各在哪
- **把游戏装上真机** —— 用 adb 上手一个真实的 Unity 手游
- **读出 C#** —— 从 il2cpp metadata 里挖出字符串、看类骨架
- **解出 Lua** —— 识别 Lua 的多种形态，XOR 爆破解出脚本
- **反编译 DEX** —— jadx 把 `classes.dex` 还原成可读 Java
- **提取资源** —— 从 AssetBundle 导出贴图

## 🚀 快速开始

去 [**Releases**](../../releases) 下最新包，解开后：

```
server.exe            ← 双击启动（本地课程加载器）
mods/course/          ← 课程内容
```

双击 `server.exe`，浏览器打开 **http://127.0.0.1:8770/** —— 看课、跑演示、交作业批改，全在网页里闭环，不用开终端。

## 📚 课程内容（40 章）

从装环境到毕业考，一条线走完 Unity 手游的每一层：

| 段 | 章数 | 学到 |
|---|---|---|
| 🧱 P0 · 地基 | 5 | adb / Frida / frida-gadget / unidbg —— 工具链与真机环境备齐 |
| 📦 P1 · APK 入门 | 2 | 认清 Unity APK 五层结构、把靶子游戏装上真机 |
| 🔧 P2 · APK 处理 | 4 | 下载、解包、签名校验、重打包绕过 |
| 🧩 P3 · C# / il2cpp | 8 | metadata 出骨架、还原 C#、动态 hook、热更 DLL、加固与脱壳 |
| 🌙 P4 · Lua | 9 | 认形态、反编译、配置还原、动态、protobuf 协议、破格式/字节码/加密、注入 |
| ⚡ P5 · puerts / JS | 6 | puerts、混淆、QuickJS 破格式/字节码/加密、注入 |
| ☕ P6 · DEX / Java | 2 | jadx 反编译、DEX 壳脱壳 |
| 🎨 P7 · 资源 | 3 | AssetBundle 导出、AB 加密、游戏数值配置 |
| 🏆 P8 · 毕业考 | 1 | 综合关：多层加固的靶包，从头拆到底 |

> 每章 = 一段可运行演示（正课）+ 一道自动批改的作业；做对点亮、像爬塔一样通关。

## 🛠 项目结构

| 目录 | 是什么 |
|---|---|
| `builder/` | 通用编译器：把课程目录编译成可加载的 mod 包 |
| `loader/` | 通用运行时：HTTP server + 内嵌网页 UI，`cd loader && go build` 即得 |
| `course/` | 课程内容：讲义、作业题面、靶包材料、塔定义 `maps/` |

> 仓库里只放源码与内容；编译产物（批改器、演示程序、靶包、工具）不进 git。
> **可直接运行的完整课程包在 [Releases](../../releases) 下载。**

## 📄 License

课程内容仅供**学习与授权测试**使用，允许个人学习与 Fork，禁止商业搬运/二次售卖。详见 [LICENSE](LICENSE)。

课程里所有逆向技术只用于学习和你有权分析的目标，请勿用于任何违法用途。
