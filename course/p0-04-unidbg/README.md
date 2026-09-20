# 0.4　unidbg：PC 上仿真跑 native（工具介绍）

> 状态：🟢 已完成（工具介绍 + 一份真跑 unidbg 的作业）
>
> 这一课是**工具备选**，放在环境部分——你不一定马上用得上，但碰到"加壳 SO 里的加密函数"或"静态解不动的 metadata"时，它是天选。常规明文包用不到，先知道有这么个东西、大概怎么用就行。

---

## 一、unidbg 是什么

[**zhkl0228/unidbg**](https://github.com/zhkl0228/unidbg)（Java，国内主导）——一个 **Android Native 模拟执行框架**：在 **PC 上无侵入地跑一个 arm64 `.so` 里的函数**，不用真机、不进设备防护、不被反调试干扰。

- 底层 CPU 用 **Unicorn / Dynarmic** 模拟 ARM；
- 上层补齐了 **JNI**（JavaVM/JNIEnv）、**Android linker**（`loadLibrary` 解依赖）、**syscall**、**内联 hook**、**指令/读/写三路 trace**。
- 定位：**静态分析的延伸**（不是 Frida 真机动态、也不是 Ghidra 纯静态）——把一个 native 函数当**黑盒**在 PC 上跑起来、抓输入输出。国内看雪/吾爱圈子做 Android native 仿真基本就用它。

---

## 二、什么时候用它（决策）

- **只想知道一个加密函数"在算什么"**（签名 / AB-key / 网络协议加密），不需要好看的 C 伪代码 → unidbg **黑盒调 + trace + 认常量**，比硬啃 OLLVM 混淆的 CFG 快得多。
- **静态 dumper 解不动壳里的 metadata**（CodeRegistration 被打乱 / metadata 加密）→ unidbg 加载壳 SO、`callJNI_OnLoad` 让它**在仿真内存里自解密自注册**，再从仿真内存 dump 明文 metadata（和 3.8 "内存偷明文"同思路，只是内存在 PC 的模拟器里，无真机）。
- **跨版本兼容好**：让它自己算，游戏更新了 key/算法微调，调用脚本基本不动（这也是 3.7 metadata 加密的第二条脱法）。
- **不适用**：想给 Ghidra/IDA 一个能 F5 的清爽函数、给同事讲解 → 那走静态去混淆，不是 unidbg。

---

## 三、怎么用（最小骨架）

Maven 依赖 `com.github.zhkl0228:unidbg-android`。核心就四步：

```java
// 1. 建 64 位模拟器
AndroidEmulator emulator = AndroidEmulatorBuilder.for64Bit()
        .addBackendFactory(new Unicorn2Factory(true)).build();
// 2. 建 VM、设 linker 解析器（Android API 级别）
VM vm = emulator.createDalvikVM();
emulator.getMemory().setLibraryResolver(new AndroidResolver(23));
// 3. 加载壳 SO，跑它的 JNI_OnLoad（解依赖 + 初始化 + 壳自解密）
DalvikModule dm = vm.loadLibrary(new File("libgame.so"), true);
dm.callJNI_OnLoad(emulator);
// 4. 黑盒调目标函数：给已知输入,抓输出/内存/寄存器轨迹
Number ret = dm.getModule().callFunction(emulator, offset, arg1, arg2);
```

拿到输出后，用 trace（指令/内存/寄存器）+ 常量识别（S-box / 轮常量 / 魔数）认出它是哪种**标准算法**，再用标准实现离线复现，**多样本逐字节对齐**验证（不是"跑通没报错"，是"字节相同"）。

---

## 四、能干到什么程度（一个脱敏案例）

某加壳手游的 **AssetBundle 被一段 native 生成的 keystream 加密**，磁盘上连 `UnityFS` 魔数都被抹了、OLLVM 混淆静态啃不动。把**生成 keystream 的函数丢进 unidbg 黑盒调**、喂固定输入抓 keystream + 轨迹，认出常量后发现它其实是**标准 ZUC-128**（3GPP 公开序列密码），只是 keystream 按 32 位字**小端**发射——**不是自研**。据此标准 ZUC 离线复现，**200/200 个 bundle 字节对齐通过**。

> **可迁移的教训**：厂商宣称的"自研加密"，很多时候是**标准算法换了个字节序 / 参数 / 常量表**。黑盒仿真抓输入输出、比对标准算法常量，比硬啃混淆快得多。判"成功"永远看**字节对齐**，不看"看起来像"。（已脱敏：不含真实厂商 / 游戏 / 地址 / 密钥。）

---

## 五、检查点

- 能说清 unidbg 的定位：**PC 上无侵入跑 native**，是静态分析的延伸（对比 Frida 真机 / Ghidra 纯静态）。
- 知道两个杀手锏：**黑盒调加密函数抓算法**、**`callJNI_OnLoad` 让壳自解密再 dump metadata**（无真机）。
- 会用"黑盒调 + trace + 认常量"把一个加密函数认成某种标准算法，并用**多样本字节对齐**验证复现。

---

## 动手：用 arm64 unidbg 模拟执行一段 bin

- **`作业/`** —— 真跑一次 unidbg。材料给你一段 **arm64 机器码** `flag_gen.bin`（不是 ELF，就是一段码），flag 被编码存在码里、**跑起来**才解出来（`strings` 抠不到明文）。你用现成的 unidbg 工程把它模拟执行起来（`mmap 可执行内存写码 → eFunc 跑到 ret → 读输出缓冲`），读出 flag。材料里 `unidbg-run/` 是能直接 `mvn` 跑的工程，harness 已写好。任务详情见 `作业/README.md`。

> 作业工程首次 `mvn` 会从 **jitpack 联网**拉 unidbg 依赖。离线机在有网时先跑一次 `mvn compile` 预热本地 `~/.m2` 仓库，之后断网也能反复跑。

> 这份作业练的是 unidbg 跑「一段裸码」的低层路。跑「一个壳 SO」的高层路（`loadLibrary + callJNI_OnLoad + callFunction`）见本仓库 `tools/*-unidbg/` 的 harness，方法档 `docs/research/ollvm/04-unidbg-emulation.md` 有完整 Java 示例 + unidbg vs D-810/Triton 的选型。

---

## 小结

unidbg 给了你一条**无真机**的 native 分析路：黑盒调加密函数抓算法、或让壳自解密再 dump metadata——静态啃不动、又不想上真机 Frida 时的高阶备选。它是 3.7（metadata 加密的第二条脱法）和 3.8（native 壳）的工具补充，需要时回来看这一课。
