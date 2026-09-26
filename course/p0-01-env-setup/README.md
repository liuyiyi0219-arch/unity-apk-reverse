# 0.1　环境配置 · 工具清单

> 状态：🟢 ｜ 把电脑上要用到的逆向工具装齐。后面各章"动手"要用什么，都在这里。

---

## 一、本章的目的

逆向要用一批命令行工具。它们分两类：

- **课程自带（`tools/` 目录，随课程发）**：小、版本敏感的，演示程序直接调用，你直接用。
- **需要自己装**：大 / 官方 / 系统级的，按下面装一次。

本章把"需要自己装"的这批装齐。Windows 上大多能用一条 `winget install <ID>` 装好，给不出 winget 的走官方下载页。

---

## 二、操作指南

### A. 课程自带（`tools/`，直接用）

这批随课程发，演示程序会自己调用，你了解一下位置即可：

| 工具 | 用途 | 位置 |
|---|---|---|
| **apktool** | 解包 / 重打包 APK（manifest、资源、dex 变可读可改，再打回可跑） | `tools/apktool.jar` |
| **精简 JRE** | 跑 apktool / apksigner 等 Java 工具 | `tools/jre/` |
| **zipalign** | 重打包后做 4 字节对齐 | `tools/buildtools/zipalign.exe` |
| **apksigner** | 给重打包后的 APK 签名 | `tools/buildtools/apksigner.jar` |
| **course.keystore** | 课程统一的重签名 key（`storepass/keypass=course123`，alias `coursekey`） | `tools/buildtools/` |
| **frida-server** | 设备端注入引擎，动态调试用（push 到 `/data/local/tmp/`；**版本必须和 host frida-tools 对齐**） | `tools/frida/`（arm64；心智模型 + 怎么调通见 **[0.2](../p0-02-frida-setup/README.md)**） |

### B. 先装运行时（很多工具依赖它们）

先把这三个装上，后面的分析工具才能跑：

| 工具 | 用途 | 安装 | 验证 |
|---|---|---|---|
| **Java (JDK)** | apktool / jadx / Ghidra / unluac 用 | `winget install EclipseAdoptium.Temurin.21.JDK` | `java -version` |
| **.NET SDK** | Il2CppDumper / ILSpy 用 | `winget install Microsoft.DotNet.SDK.8` | `dotnet --version` |
| **Python 3** | UnityPy / frida / 各种脚本用 | `winget install Python.Python.3.13` | `python --version` |

### C. 装静态分析工具

处理 C# / native / DEX 的一批：

| 工具 | 用途 | 安装 | 验证 |
|---|---|---|---|
| **Il2CppDumper** | `libil2cpp.so` + metadata → 类/方法骨架（dump.cs）+ DummyDLL | 下 release 解压：https://github.com/lxraa/Il2CppDumper（我们的 fork，支持 metadata v32–v39 / Unity 6，含 PR#903 修复） | 跑一次出 `dump.cs` |
| **Cpp2IL** | il2cpp 反编译备选 | 下 `Cpp2IL-*-Windows.exe`：https://github.com/SamboyCoding/Cpp2IL | `Cpp2IL.exe --help` |
| **Ghidra** | 反汇编 / 伪 C，读 native | 解压跑 `ghidraRun.bat`：https://ghidra-sre.org | 能启动 |
| **SoFixer** | 把内存 dump 出的 `.so` 按 program header 重建 section header，得到 Ghidra/IDA 能加载的 ELF | 课程已带 `vendor/sofixer/build/SoFixer64.exe`（我们的 fork，含 `.eh_frame` SHT 重建：https://github.com/lxraa/SoFixer） | `SoFixer64.exe -h` |
| **ILSpy / ilspycmd** | 反编译 DummyDLL / .NET 程序集 | `dotnet tool install -g ilspycmd` | `ilspycmd --version` |
| **jadx** | DEX → Java | 解压跑 `bin/jadx.bat`：https://github.com/skylot/jadx | `jadx --version` |
| **unidbg** | PC 上仿真跑壳内 native `.so`：黑盒调加密函数抓算法 / `callJNI_OnLoad` 让壳自解密再 dump metadata（无真机，3.10） | Maven 项目依赖 `com.github.zhkl0228:unidbg-android`（需 Java）：https://github.com/zhkl0228/unidbg；本仓库 harness 在 `tools/*-unidbg/` | `mvn -q compile` 通过 |

### D. 装脚本层 / 资源 / 协议工具

| 工具 | 用途 | 安装 | 验证 |
|---|---|---|---|
| **unluac** | Lua 字节码 → 源码 | 需 Java：https://github.com/lxraa/unluac（我们的 fork，修过若干解码 bug） | `java -jar unluac.jar` |
| **AssetStudioMod (CLI)** | 提 Unity 资源（贴图 / 音频 / mesh） | 下 `AssetStudioModCLI.exe`：https://github.com/aelurum/AssetStudio | `AssetStudioModCLI.exe --help` |
| **UnityPy** | Python 解 AssetBundle / 资源 | `pip install -U UnityPy`（取最新，需 ≥1.25.3） | `python -c "import UnityPy"` |
| **flatc** | 编译 / 反推 FlatBuffers 协议 | 下 release：https://github.com/google/flatbuffers/releases | `flatc --version` |
| **protoc** | 编译 / 反推 Protobuf 协议 | 下 release：https://github.com/protocolbuffers/protobuf/releases | `protoc --version` |

### E. 装动态分析工具

| 工具 | 用途 | 安装 | 验证 |
|---|---|---|---|
| **adb (platform-tools)** | 连真机：装包 / 拉包 / logcat（0.0 已装） | 见 [0.0](../p0-00-prep/README.md) | `adb version` |
| **frida-tools (host)** | 电脑端 frida：`frida` / `frida-ps` / `frida-trace`（心智模型 + 怎么调通见 **[0.2](../p0-02-frida-setup/README.md)**） | `pip install frida-tools`（版本和设备端一致） | `frida --version` |
| **Florida (device)** | frida 的去特征 fork，设备端注入引擎；游戏检测 frida 时用它代替普通 frida-server | 下 `florida-server-*-android-arm64`：https://github.com/Ylarod/Florida，push 到设备 `/data/local/tmp/` 跑 | 设备上起后 host `frida-ps -U` 能列进程 |
| **frida-gadget (.so)** | 免 root 注入：焊进未加固 APK 随游戏自加载（0.3） | 下对应版本 `frida-gadget-*-android-arm64.so`：frida releases（**和 host frida 同版本**）；由 `tools/gadget-repack/gadget_repack.py` 注入 | 注入后 `frida -U Gadget` 连得上 |

---

## 三、检查点

按你要装的那几个，逐条敲验证命令，能打出版本号 / 正常输出即通过：

| # | 工具 | 验证命令 | 期望 |
|---|---|---|---|
| 1 | Java | `java -version` | 打出版本号 |
| 2 | .NET SDK | `dotnet --version` | 打出版本号 |
| 3 | Python | `python --version` | 打出版本号 |
| 4 | ILSpy | `ilspycmd --version` | 打出版本号 |
| 5 | jadx | `jadx --version` | 打出版本号 |
| 6 | SoFixer | `vendor/sofixer/build/SoFixer64.exe -h` | 打出用法 |
| 7 | UnityPy | `python -c "import UnityPy"` | 无报错 |
| 8 | flatc | `flatc --version` | 打出版本号 |
| 9 | protoc | `protoc --version` | 打出版本号 |
| 10 | frida | `frida --version` | 版本号，和设备端一致 |
| 11 | adb | `adb version` | 打出版本号 |
| 12 | Florida | 设备上起 florida-server 后 `frida-ps -U` | 列出进程 |
| 13 | unidbg | `tools/*-unidbg/` 里 `mvn -q compile` | 编译通过（高阶，用到再装） |
| 14 | frida-gadget | 注入后 `frida -U Gadget` | 连得上（0.3，免 root 用到再装） |

全部通过后，工具链就绪。课程自带的那批演示程序直接调用，不占你的清单。

---

## 小结 & 下一章

工具备齐后，各章"动手"用到什么回这里查。从 **[1.1 Unity APK 的分层结构](../p1-01-apk-layers/README.md)** 开始正式逆向。
