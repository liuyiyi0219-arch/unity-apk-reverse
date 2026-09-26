# 3.8　native 破壳

> 状态：🟢 已完成（正文 + 正课演示 + 作业靶包）｜`作业/check.exe`（脱壳 → 读 flag）｜核心是"修复"

---

## 一、本章的目的

3.7 破的是 **metadata** 层加密。这一课破 **native 层**——`.so`（`libil2cpp.so` 或游戏自研 native 库）**本身被加壳**：`.text`（代码段）在盘上被加密，Ghidra/IDA 反出来是乱码、`strings` 抠不到，一 attach 可能还崩。

破法和 metadata 一样**不破密码学**：让程序自己在内存里把 `.text` 解密，然后把它**从内存 dump 出来**。但 native 壳多一步——**dump 出来的 ELF 结构是坏的（节头被抹、段地址是内存态），Ghidra 加载不了，必须先"修复"（SoFixer）**。这一课的核心就是这个"修复"。

---

## 二、认：`.text` 乱码 = native 壳

```bash
llvm-objdump -d lib/arm64-v8a/libxxx.so | head        # .text 反汇编是不是乱码
llvm-readelf -S lib/arm64-v8a/libxxx.so                # 节头是否被抹/异常
```

- **`.text` 反汇编是乱码**（非法指令一大片）→ 代码段被加密。
- **`readelf -S` 报错 / 节头缺失 / 段大小异常** → ELF 头被动过。
- **`.so` 体积异常大**（比引擎通常需要的大 2 倍以上）→ 壳把 payload 塞进去了。
- 常配一个 loader / 反调试库，或 `.init_array` 里有个在 `JNI_OnLoad` 之前跑的构造函数（它就是运行时解密器）。

> **熵别单用**：真壳的 `.text` 磁盘签名各不相同——易盾是**打桩→低熵 ~3.9**（不是高熵）、Virbox 是**清零→熵 ~0.05**、高熵加密壳才 ~7.5。低熵不代表没壳。看反汇编是不是乱码更准。

---

## 三、破：dump 内存里解密的 `.text` + SoFixer 修 ELF

### 第 1 步 · 让它跑起来（自解密）
壳的解密器（`.init_array` / `.preload` 构造函数）在**代码执行前**把 `.text` 在内存里解密。进程跑起来（进主界面），内存里的 `.text` 就是明文了。

### 第 2 步 · 从内存 dump 出 `.so`
```bash
cat /proc/<pid>/maps | grep libxxx.so     # 定位 .so 的映射区
# 纯 adb dd（对反注入隐形）：
adb shell su -c "dd if=/proc/<pid>/mem bs=1 skip=<起始> count=<大小> of=/data/local/tmp/dump.so"
```
或 frida 枚举模块拿 base + size 整块读出来。dump 出来的 `.text` 已是明文（能反出合法 ARM 指令），但**整个 ELF 是"内存态"**：节头（SHT）没了、program header 里段的 `p_vaddr/p_offset/p_filesz` 是内存布局，Ghidra 直接加载会失败。

### 第 3 步 · SoFixer 修 ELF（★核心）
[SoFixer](https://github.com/F8LEFT/SoFixer) 按 **program header** 重建一个 Ghidra 能读的 ELF：
- 把段的 **`p_offset` 归一到 `p_vaddr`**（内存态下两者相等），`p_filesz` = `p_memsz`；
- 按段重建一份节头表（`.text`/`.data`/`.dynsym`…）；
- dump 基址传对（`-m <dump 时的加载基址>`）。

```bash
SoFixer64 -m 0x<load_base> -s dump.so -o fixed.so
```

修好的 `fixed.so` 用 Ghidra/IDA 打开，`.text` 正常反汇编、函数/符号都在——回到普通的静态分析（3.2 读伪 C 那套）。

### 第 4 步 · 对抗反调试（需要时）
壳常检测 frida / ptrace / Magisk，检测到就崩或改行为。绕：
- 纯 `adb dd /proc/pid/mem`（不注入、不 attach，对反 frida 隐形）；
- Florida（改名 frida）、`SIGSTOP` 冻住再 dump；
- 断网 / `install -i com.android.vending` 骗过某些启动闸门（PairIP 那类）。

---

## 四、开源参考：ELFEncryTest 的 AES-CTR 加壳

本课作业的靶包用开源的 [**ELFEncryTest**](https://github.com/nuloperrito/ELFEncryTest) 加壳，它的做法就是一套标准 native 壳：

- Gradle 插件在 `stripDebugSymbols` 后用 `jelf` 解析 `.so`，把 **`.text` 段用 AES-CTR 加密**（key 焊在里面）；
- `.so` 里一个 `.preload` 段（不在 `.text`、不被加密）的**构造函数**在启动时跑，dlopen 一个辅助 `.so`（OpenSSL），把 `.text` 在内存里 AES-CTR 解密：`mprotect` 改可写 → 解密 → 改回 `r-x` → 刷 CPU cache。

> 它的一个限制（README 自己写的）：解密代码 + OpenSSL 依赖**必须放在另一个 `.so`**——因为这些代码要是编进主 `.so` 就会进 `.text`，而 `.text` 那时还是密文，一调就 SIGILL。这也是为什么很多壳有"主库 + 辅助解密库"两个 so。

**破它和上面完全一样**：不碰 AES，让它自解密、dump `.text`、SoFixer 修。加密用 XOR 还是 AES，对破法没区别。

---

## 五、真实环境的壳复杂得多（认指纹，不复刻）

demo 只加密了 `.text`。真实加固（以易盾 libil2cpp 为例）远不止这一层，**修复的活也多得多**：

- **`.text` 不是简单加密，而是打桩 / 原地编码**：要么缩成 ≤32B 桩（真码在 `.note.gnu.proc`，运行时填）、要么整段编码运行时原地解码——**低熵、熵判假阴**。
- **ELF 头 / 节头被抹或伪造**：dump 完 SHT 全没，SoFixer 要按 program header 全部重建（正是本课这步的加强版）。
- **导出符号名整表加密**：`il2cpp_*` 导出名全变乱码（条目数守恒）→ `frida-il2cpp-bridge` 按名解析直接废。
- **内存里 metadata 头被清零**：dump 出来还要手工把 `af1bb1fa`+version 盖回去。
- **指针是 vaddr 形式（base 0 未重定位）**：Il2CppDumper 要用 `mod_base=0` + `IsDumped` 补丁。
- 库分工别混：易盾 `libnesec`(加固壳) / `libNetHTProtect`(风控) / `libhtpcrash`(崩溃取证)是三条不同产品线。

一句话：**demo 让你练"dump + SoFixer 修复"这一刀；真实壳是这套的加强版，dump 完要修的东西更多、还要先过反调试。**

> 破 native 壳还有一个"层"要认对：有时 `libil2cpp.so` 没加壳，但里面的**注册结构 CodeRegistration / MetadataRegistration** 被打乱（腾讯 TP、FairGuard）——metadata 全明文却 Dumper `EndOfStream`。那不是 `.text` 壳，得 frida 调 live IL2CPP runtime API 或结构定位 CR/MR。

---

## 六、检查点

- 能一句话说清 native 壳破法：**不破密码学，dump 内存里解密的 `.text` + SoFixer 修 ELF**。
- 会认 native 壳（`.text` 乱码 / readelf 异常 / `.so` 异常大 / 有 `.init_array` 解密器）。
- 会用 `dd /proc/pid/mem`（或 frida）dump `.so`，用 SoFixer 按 program header 修好让 Ghidra 能读。
- 知道真实壳比 demo 复杂（打桩 / 编码 / 符号加密 / 头抹除 / 反调试）——修复的活更多。

---

## 动手

- **`正课/`** —— 走一遍脱壳闭环（造 `.text` 加密的 `.so` → 反汇编是乱码 → 内存里解密 → dump → SoFixer 修 → 反出函数）。
- **`作业/`** —— **自己动手（真机）**。下载 `作业/材料/native-shell.apk`：里面 `libelfencrytest.so` 用开源 ELFEncryTest **AES-CTR 加壳**了 `.text`，flag 藏在函数 `computeFlag` 的机器码里（静态是乱码）——它焊了一段 **SM4（国密 GB/T 32907）密文 + key**，运行时解密出 flag。装上跑起来 → 从内存 dump `libelfencrytest.so` → SoFixer 修 ELF → Ghidra 反 `computeFlag`，认出标准 SM4、抄下密文和 key、用标准 SM4 解出 flag → 交给 `作业/check.exe FLAG{...}`。任务详情见 `作业/README.md`。

> 靶包是**自造教学样本**——用开源 ELFEncryTest 的标准 AES-CTR native 壳加固，不复刻任何商业壳。

---

## 小结 & 下一章

native 壳破了：认（`.text` 乱码）→ 让它自解密 → dump → **SoFixer 修 ELF** → Ghidra。这一刀（dump + 修复）是所有 native 壳的通用打法，真实壳只是修复的活更多。native 仿真工具 **unidbg**（PC 上不用真机跑 `.so` 函数、脱壳、算 key）见 **0.4**。至此 **C# / IL2CPP 层** 配齐，下一部分 · **脚本层 Lua**。
