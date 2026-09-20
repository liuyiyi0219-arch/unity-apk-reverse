# 0.3　Frida gadget：免 root 注入

> 状态：🟢 已完成（正文，工具真跑）｜工具：`tools/gadget-repack/gadget_repack.py`
>
> 0.2 讲的是 **root + frida-server** 那条路；这一课讲**免 root** 的另一条——把 gadget 焊进包里。

---

## 一、本章的目的

把 **frida-gadget**（一个 `.so`）静态焊进一个**未加固**的 APK，产物能装到**任何手机**（无需 root、无需 frida-server、无需运行时 attach）——app 自己在启动时把 gadget 加载进自己的进程。之后你就能在**没 root 的机器**上 hook 它的 C#/Lua/native/Java。

> 两种跑法：**listen 模式**（gadget 在进程内监听 `127.0.0.1:27042`，host `frida -U Gadget` 连上去交互）；**script 模式**（gadget 启动时自动跑一段打包好的 JS，完全自包含、离线生效——真正把逻辑焊进包里）。
>
> 适用范围：**未加固包**。有的加固包会在 native 层检测到多出来的 `.so` 或变了的签名然后自杀——那些改用 frida-server attach（不额外往包里塞 `.so`，见 [0.2](../p0-02-frida-setup/README.md)）、或先脱壳（[3.8](../p3-08-unpack-native/README.md)）再处理。用前先跑一遍加固分诊。

---

## 二、操作指南

### 第 1 步 · 分诊：确认未加固

跑一遍保护 triage（3.7）：metadata magic + native 库名。没有加固库、magic 明文 → 这条路能走。

### 第 2 步 · 注入 gadget（纯 smali，不碰资源）

工具 `gadget_repack.py` 的做法（也是手工时的正确姿势）：

- **`apktool d -r`**：只反 dex 成 smali，`resources.arsc` / `AndroidManifest` 保持二进制原样——**全程不跑 aapt2 重编资源**，绕开重打包失败的头号原因（资源 ID 漂移 / aapt2 报错）。
- **给 app 的 `Application` 类加一个 `<clinit>` 静态初始化块**，里面 `System.loadLibrary(gadget)`。类的静态初始化块是这个类最早跑的 Java 代码，所以 **gadget 在 `onCreate` / 任何 Java 层反篡改逻辑之前就加载了**。没有 Application 类就退回启动 Activity 的 `onCreate`。
- **Manifest 能不碰就不碰**：只有在 `extractNativeLibs=false`（.so 要页对齐免压缩）或 listen 模式缺 INTERNET 权限时才改它。

### 第 3 步 · 重签 + 装

`apktool b` → zipalign → apksigner **v1+v2+v3 全签**（少签一个某些 Android 版本会判篡改）。XAPK/split：注入进 base split，**所有 split 用同一个 key** 重签（签名不一致 = `INSTALL_FAILED`），用 `adb install-multiple` 装。

### 第 4 步 · 用

```bash
adb install -r out.apk
adb shell monkey -p <pkg> -c android.intent.category.LAUNCHER 1
frida -U Gadget -l hook.js          # listen 模式连上去（script 模式则自包含、不用连）
```

### 第 4.5 步 · 架构对齐：hook arm64 库就用 arm64 机

gadget 是个 `.so`，分 **arm64 / arm / x86_64 / x86**——下**和 app 实际跑的 CPU 一致**的那个（看 APK `lib/` 下有哪些 ABI）。**要 hook arm64 的游戏库（`libil2cpp.so` 这些），就在真 arm64 机、或 arm64 模拟器上跑**（Apple 芯片上 Android Studio 的 arm64 系统镜像、云真机等）。

> **别在 x86_64 模拟器上跑 arm64 gadget**：BlueStacks / LDPlayer / Nox / Genymotion 这些 x86_64 模拟器靠 **libhoudini**（ARM→x86 翻译层）跑 arm64 库，而 Frida 工作在 x86_64 域，**探测 / 注入不进 houdini 翻译出来的 ARM 代码**。实测症状：注入就崩（`Unsupported Android linker` → SIGABRT）／ Frida v14+ 报 `frida_gadget_detect_location: assertion failed (our_range != null)`／ Java 和 x86 native 能 hook，但 `libil2cpp.so` 这些 arm 库 **Frida 直接看不见**，`--realm=emulated` 又报 "process is not using emulation"。硬凑：把 Frida 降到 **12.11.x** + `--realm=emulated` 有时能行，但强依赖"模拟器 + Android x86 版本 + Frida 版本"的组合，不可靠——**要稳就用 arm64 环境**。（社区多年老问题：frida issues [#283](https://github.com/frida/frida-core/issues/283)/[#1556](https://github.com/frida/frida/issues/1556)/[#3421](https://github.com/frida/frida/issues/3421)、frida-il2cpp-bridge [#613](https://github.com/vfsfitvnm/frida-il2cpp-bridge/issues/613)）

### 第 5 步 · 过签名检测（重签必换签名）

重打包必然换签名（你没有原厂私钥）。**"过签名检测" = 让 app 不去比、或比的时候拿到假的正版签名**（不是伪造正版签名，那伪造不了）。按检测在哪层：

- **Java 层自校验**（`getPackageInfo(...GET_SIGNATURES)`/`getSigningInfo`，或自读 APK 算 hash）→ 再加一段 smali，hook 这些调用点返回硬编码的正版签名字节，或把校验方法 patch 成 `return true`（有现成的开源补丁模块）。
- **DEX / 资源完整性**（存了各 dex 的 CRC 比对）→ 定位比对逻辑 patch 掉。（ART 那句 `ClassLoaderContext ... checksum mismatch` 是系统跳过 oat 的无害警告，不是 app 自校验。）
- **native 层自校验**（自读 `/data/app/.../base.apk` 重算 v2/v3 签名块 hash）→ 最硬：在 IDA/Ghidra 定位那个"读 APK 算 hash 比对"的函数，native patch（nop/改跳转），或 hook `openat`/`read` 把读到的内容重定向到正版 APK。
- **Play Integrity / SafetyNet**（结论由 Google 服务端签发）→ 重签名 + 非 Play 来源会挂在 `MEETS_DEVICE_INTEGRITY`，静态抹不掉；带这个的目标只能走 root 隐藏方案。

---

## 三、检查点

- 能说清 gadget vs frida-server：gadget 免 root（焊进包，app 自加载）、server 需 root（常驻设备）。
- 能用 `gadget_repack.py` 把一个未加固包注入 gadget、重签、装上，`frida -U Gadget` 连得上。
- 知道注入点是 `Application.<clinit>` 里 `System.loadLibrary`（抢在 Java 反篡改前）、要 v1+v2+v3 全签、split 同 key。
- 会按"签名检测在哪层"选对绕过（Java hook getSignatures / native patch / Play Integrity 走 root）。
- 知道 gadget `.so` 要**架构对齐**：hook arm64 游戏库就用 arm64 机 / arm64 模拟器；**x86_64 模拟器（houdini 翻译 arm64）上 Frida 看不见 / 注入不进 arm 库**。

---

## 动手：用 gadget-repack 工具

`tools/gadget-repack/gadget_repack.py`（工业级引擎，已在真机验证过未加固包）：

```bash
python gadget_repack.py in.apk -o out.apk                              # listen 模式
python gadget_repack.py in.apk -o out.apk --mode script --script x.js  # script 模式（自包含）
python gadget_repack.py in.xapk -o out_dir/                            # split：一组重签的 split
```

gadget `.so` 从 frida releases 下对应**版本 + 架构**（版本要和 host frida 一致，同 0.2 的对齐铁律；架构要和 app 实际跑的 CPU 一致——见第 4.5 步，arm64 库别在 x86_64 模拟器上跑）。工具会先做加固分诊，加固包直接拒绝。深度方法 + 签名检测分层见 `tools/gadget-repack/README.md`。

---

## 小结 & 下一步

Frida 的两条路齐了：**0.2 root + frida-server**（常驻、能 spawn 抢早期）、**0.3 gadget 免 root**（焊进包、可分发、逻辑常驻，代价是换签名要处理签名检测）。有 root 优先 server，目标机不能 root（或要把逻辑分发出去）就走 gadget。回主线继续各层逆向。
