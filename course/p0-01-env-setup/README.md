# 0.1　环境配置 · 工具清单

> 状态：🟢 ｜ 把电脑上要用到的工具装齐。本课只做**离线扒包 + 装真机**，清单很短——五样就够。

---

## 一、本章的目的

本课这 8 章，动手用到的就下面这几样命令行工具，装一次、后面各章直接用。
（本课会用到 Il2CppDumper / Ghidra / frida / unluac / flatc… 一大批，那些到本课再装；这里不用。）

课程自带的小工具（`tools/` 目录，随课程发，演示程序自己调用）你不用管，比如精简 JRE、zipalign、apksigner。

---

## 二、操作指南（装这五样）

| 工具 | 用途 | 安装 | 验证 |
|---|---|---|---|
| **adb** | 把靶子游戏装到真机 / 连设备（0.0 已装） | 见 [0.0](../p0-00-prep/README.md) | `adb version` |
| **Python 3** | 解包 / 写脚本（3.1 grep metadata、4.1 XOR 爆破都靠它） | `winget install Python.Python.3.13` | `python --version` |
| **UnityPy** | 从 AssetBundle 导 Unity 资源（7.1 提贴图） | `pip install -U UnityPy`（需 ≥1.25.3） | `python -c "import UnityPy"` 不报错 |
| **Java (JDK)** | 跑 jadx（jadx 是 Java 写的） | `winget install EclipseAdoptium.Temurin.21.JDK` | `java -version` |
| **jadx** | DEX → Java 反编译（6.1） | 下 [jadx](https://github.com/skylot/jadx) 解压，把 `bin/` 加 PATH | `jadx --version` |

---

## 三、检查点

逐条敲验证命令，能打出版本号 / 正常输出即通过：

| # | 工具 | 验证命令 | 期望 |
|---|---|---|---|
| 1 | adb | `adb version` | 打出版本号 |
| 2 | Python | `python --version` | 打出版本号 |
| 3 | UnityPy | `python -c "import UnityPy"` | 无报错 |
| 4 | Java | `java -version` | 打出版本号 |
| 5 | jadx | `jadx --version` | 打出用法 / 版本 |

五样全绿，作业里的环境检查就过。

---

## 小结 & 下一章

工具备齐后，从 **[1.1 Unity APK 的分层结构](../p1-01-apk-layers/README.md)** 开始正式逆向：先认清 APK 五层，再把游戏装上真机，然后 C# / Lua / 资源 / Java 各扒一层。

> 想学动态 hook、脱壳、破加密、注入、打通关这些硬功夫？那是本课的内容，用到的 Il2CppDumper / Ghidra / frida / unluac 等一整套工具也在那里配。
