# 1.1　Unity APK 的分层结构

> 状态：🟢 已完成（正文 + 演示程序）｜标本：`demo/build/BareDemo.apk`｜演示程序：`正课/lesson-1.1.exe`

---

## 一、本章的目的

拿到一个 Unity 手游 APK，能顺着"**游戏逻辑到底在哪**"把它拆成五层——**java · C#（IL2CPP）· native · 脚本层 · resources**，说清每个关键文件属于哪层、从哪层切入。这张地图是全课的底座。

本章跟着做一遍，会得到：一张五层地图、一条启动链、以及一个核心手感——**在上万个引擎类里认出"你自己写的那几个类"**。

> 本章用最干净的标本：一个几乎是空的 Unity 游戏 `BareDemo.apk`，只有一个自己写的类 `HelloWorld`，别的全是引擎自带的。它是**纯 C# 游戏**（没有脚本层），正好看最基础的骨架；脚本层等 1.2 的 v1 靶才出现。怎么造的放 1.2 讲。

---

## 二、操作指南

对着 `BareDemo.apk` 一步步走。（不想敲命令就直接跑 `正课/lesson-1.1.exe`，它把这几步真做一遍。）

### 第 1 步 · 把 APK 当 ZIP 解开

APK 就是个 zip。把它复制一份改名 `.zip` 解压，或用解压软件直接打开，先认通用结构：

| 文件 / 目录 | 是什么 |
|---|---|
| `AndroidManifest.xml` | App 身份证：包名、入口 Activity、权限 |
| `classes.dex` | 编译后的 Java/Kotlin 字节码（可能有 `classes2.dex`…） |
| `lib/<abi>/*.so` | 原生库，按 CPU 架构分（`arm64-v8a` / `armeabi-v7a`） |
| `assets/` | 开发者塞的任意文件 |
| `res/` + `resources.arsc` | 安卓 UI 资源（图标 / 布局） |
| `META-INF/` | 签名 |

> 这些是安卓通识，任何 App 都一样，还不分引擎。真正的问题从下一步开始。
>
> 如果解出来发现缺 `lib/` 或缺 `assets/bin/Data`：这个包是 split APK / XAPK，`lib` 和数据被拆进了 `config.arm64_v8a.apk`、`UnityDataAssetPack.apk` 等分包——把**所有分包**都解开再看（获取方式见 2.1）。

### 第 2 步 · 看 `classes.dex`，确认逻辑不在 java 层

反编译 `classes.dex`，看到的基本是**启动入口 + 第三方 SDK（分析 / 广告 / 渠道）+ 可能的加固壳**。

> 游戏逻辑通常不在 dex 层，所以看一眼确认性质即可，往下走。

### 第 3 步 · 进 `lib/`，找到 `libil2cpp.so`

`lib/arm64-v8a/` 里两个大文件：`libunity.so`（引擎本体，C++）和 **`libil2cpp.so`**（关键）。

> Unity 用 **IL2CPP** 后端把所有 C#（引擎框架 + 你的代码）先转成 C++、再编成机器码，全塞进 `libil2cpp.so`。所以"C# 逻辑"物理上是一个 native `.so`。

### 第 4 步 · 认出它的搭档 `global-metadata.dat`

在 `assets/bin/Data/Managed/Metadata/global-metadata.dat`。**逆 IL2CPP 必须 `libil2cpp.so` + `global-metadata.dat` 两个一起拿。**

> IL2CPP 把代码和元信息拆两块存：`libil2cpp.so` 是机器码（逻辑本身），`global-metadata.dat` 是每个类叫什么、有哪些方法/字段、字符串常量等元信息。运行时靠 metadata 才知道类型长什么样；缺一个就还原不出可读代码（3.1 会看到）。原理见文末官方 *IL2CPP internals*。
>
> 确认是不是 IL2CPP：有 `libil2cpp.so` + `global-metadata.dat` 就是（本课主线）。若看到的是 `assets/bin/Data/Managed/*.dll` 一堆明文 DLL + `libmono*.so`，那是 Mono，直接用 dnSpy 反编译，不用本课这套。

### 第 5 步 · 在引擎海洋里认出"你的代码"

在 `global-metadata.dat` 里数可读字符串：会有**上万条**（`MonoBehaviour`、`GameObject`、`Vector3`… 全是引擎和 .NET 基础类库），而你自己写的 `HelloWorld` 只出现 **1 次**。两条定位抓手：

- 游戏 C# 几乎都在 **`Assembly-CSharp.dll`** 程序集（引擎在 `UnityEngine.*.dll`）；
- 编译时源码路径也写进 metadata，能直接搜到 `\Assets\Scripts\HelloWorld.cs`。

> 哪怕是空游戏，`libil2cpp.so` 也有 8.9 MB、metadata 1.5 MB——因为把整个引擎 + BCL 都编进去了。所以逆向第一难点不是"看文件"，而是**从上万引擎/BCL 类里挑出游戏自己写的那部分**。这个手感贯穿全课，3.1 出骨架第一件事就干这个。

### 第 6 步 · 画出五层地图 + 启动链

把上面认到的东西归位成五层：

| 层 | 物理产物 | 角色 / 放什么 | 逆向章 |
|---|---|---|---|
| **java（dex）** | `classes.dex` | 启动入口、第三方 SDK、可能的加固壳 | 5.1 |
| **C#（IL2CPP）** | `libil2cpp.so` 里的托管代码 + `global-metadata.dat` | 引擎/框架的 C#；**纯 C# 游戏的逻辑也在这** | 3.x |
| **native** | `libil2cpp.so` / `libunity.so` / `libmain.so` | 引擎本体 + il2cpp runtime + 脚本 VM + 保护层 | 3.2、3.9 |
| **脚本层** | lua / js(puerts) / C# 热更 DLL + 各自运行时 | **游戏逻辑**（热更；本标本无，v1 才有） | 4.x |
| **resources** | `assets/bin/Data`、AssetBundle、`res/`、`.arsc` | 场景 / 贴图音频 / 数值 | 6.x |

**启动链**：安卓从 `classes.dex` 的 `UnityPlayerActivity` 起步 → `libmain.so` → `libunity.so`（引擎）→ `libil2cpp.so`（C#）→ 引擎再拉起脚本运行时、加载脚本层。

> **核心认知：C#+native 合起来是"引擎/框架"，脚本层才是"游戏逻辑"。** 纯 C# 游戏逻辑和引擎都在 C#（本标本）；带热更的游戏把逻辑抽到脚本层（lua / js / C# 热更），`libil2cpp.so` 只剩引擎框架。所以如果在 `libil2cpp.so` 里找不到业务逻辑，就去脚本 / 热更包找。
>
> `libil2cpp.so` 在表里同时占 C# 行和 native 行——它**就是** C# 编译成的那个 native `.so`：从托管代码看是 C#，从二进制文件看是 native。
>
> 一条连带规律：脚本 / 资源 / AB 的加密 key 常是 `libil2cpp.so` / native 里的**编译期常量**，所以解脚本或资源经常得先啃 native。

---

## 三、检查点

- 拿一个**没见过**的 Unity APK，能说出每个关键文件属于哪层、作用是什么。
- 说清"为什么逆 IL2CPP 要 so + metadata 一起"，以及"纯 C# 游戏 vs 有脚本层的游戏，逻辑分别在哪"。
- 给出至少两种"从引擎堆里定位游戏代码"的抓手（`Assembly-CSharp` / `Assets/` 源码路径串）。
- 会区分 IL2CPP（`libil2cpp.so` + `global-metadata.dat`）和 Mono（明文 `.dll` + `libmono*.so`）。

---

## 动手：运行演示程序

双击 **`正课/lesson-1.1.exe`**，它会：

- 把真实的 `BareDemo.apk` **当压缩包真解压**，弹出文件管理器让你看里面的文件；
- 按回车一步步演示分层、"认出你的代码"（在文件管理器里高亮 `global-metadata.dat`）、启动链；
- 屏幕上所有数字（文件数、各层大小、metadata 文字条数、`HelloWorld` 出现几次、源码路径）都是**现场算出来的**。

看完结尾会问要不要清理解压文件：想留着逛就回车（在 `_extracted_BareDemo/`），想删输 `y`。源码就是本文件夹的 `main.go`。

---

## 参考资料

原理读官方——逆向工具都是复用同一套 IL2CPP 结构，懂原理才不把工具当黑箱。

- Unity 官方 **"An introduction to IL2CPP internals"** 系列（Josh Peterson）：
  - [toolchain](https://unity.com/blog/engine-platform/an-introduction-to-ilcpp-internals) · [A tour of generated code](https://unity.com/blog/engine-platform/il2cpp-internals-a-tour-of-generated-code) · [IL2CPP Overview (Manual 2022.3)](https://docs.unity3d.com/2022.3/Documentation/Manual/IL2CPP.html)
- 逆向实战走查：
  - [Reverse engineering a Unity-based Android game（palant.info）](https://palant.info/2021/02/18/reverse-engineering-a-unity-based-android-game/)
  - [IL2CPP Reverse Engineering（katyscode）](https://katyscode.wordpress.com/2020/06/24/il2cpp-part-1/)
  - [Reverse Engineering a Unity IL2CPP Game（dev.moe / Coxxs）](https://dev.moe/en/3043)

---

## 小结 & 下一章

你现在有了一张五层地图，抓住了核心：**C#+native 是引擎，脚本层才是游戏逻辑**，逆向第一难点是"在引擎里认出游戏代码"。下一章 **1.2** 装齐工具链，把这个裸标本升级成带真实埋点（C# 公式 / 校验 + Lua + 资源 + 全套保护）的 **v1 完整靶**——后面所有章节都逆它。
