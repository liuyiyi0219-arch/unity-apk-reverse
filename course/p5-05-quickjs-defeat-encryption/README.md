# 5.5　QuickJS 破整体加密

> 状态：🟢 已完成（正文 + 真加解密实证）｜Part 5 · puerts 的 QuickJS 那条路（对应 Lua 4.8）

---

## 一、本章的目的

QuickJS 三节的最后一类、也是最常见的一类：**整体加密**。游戏把整份 QuickJS 字节码（`qjsc -b` 产物）用**对称加密**（AES / XXTEA / XOR…）裹起来落盘，运行时在 native 里解密再喂给 `JS_ReadObject`。这类**没有密码学漏洞可爆破**，正解是**找 key + 认算法**——key 是构建期常量，藏在 native/C#/rodata，静态就能拿到。拿到 key 离线解密，就回到普通 QuickJS 字节码。这和 Lua 4.8 是同一套。

> **和前两节的区别**：5.3 改 header、5.4 改 opcode，都是"字节码本身被动了手脚"；5.5 是**字节码没动、外面裹了一层加密**。解开这层，里面就是标准 QuickJS 字节码（`strings` 直接能看到里面的字符串字面量，因为 QuickJS 的字符串常量以明文 atom 存在字节码里）。

---

## 二、操作指南（整体加密 = 找 key + 认算法，不破密码）

### 第 1 步 · 认出"被加密"

落盘的 `.qjbc` 首字节不是标准 `0x1b`（`BC_VERSION`）、且整体高熵、`strings` 抠不到任何可读串 → 大概率被整体加密（不是 5.3/5.4 那种局部改）。

### 第 2 步 · 认算法 + 找 key（核心）

解密逻辑在 native（`libpuerts.so` 外面那层 loader，或游戏 `il2cpp`/C#）。**静态**就能拿：

1. `strings` native 库 / 抠出的 `.rodata` → 认出算法指纹（`AES-128-CBC`、`XXTEA`、`ChaCha20`…）和解密函数名（`SkillCipher::Decrypt`、`BuildSkillCipherKey`、`LoadEncryptedBytecode`…）；
2. 在 key 构造函数附近找 **16B key + 16B IV**（AES-128-CBC 的话），或 XXTEA 的 128-bit key；
3. key/IV 是**构建期常量**——逆代码或翻 `.rodata` 静态就能拿到，不用跑起来。

> **为什么不破密码**：整体加密用的是标准对称算法（AES 等），**密文本身没有可爆破的弱点**。能拿到明文，是因为**客户端得自己解密才能跑**——所以 key 一定在包里（native 常量 / C# 字段 / metadata / rodata）。**本地能拿到的 key，一定是静态可复现的**。方法论永远是"找 key + 认算法"，不是对密文做密码分析。

### 第 3 步 · 离线解密 → 回到标准字节码

用拿到的 key/IV + 对应算法离线解：
```
openssl enc -d -aes-128-cbc -K <keyhex> -iv <ivhex> -in skills.qjbc.enc -out skills.qjbc
```
解出来首字节应是 `0x1b`（标准 `BC_VERSION`）——确认解对了。

### 第 4 步 · 读明文字节码里的逻辑 / flag

解出的标准 QuickJS 字节码：
- **`strings` 直接读**——QuickJS 把字符串字面量以明文 atom 存在字节码里，`strings skills.qjbc` 就能看到里面的字符串（本课 flag 就是这么一个字符串）；
- 或用标准运行时 `qjbc-run.exe` 跑它，看它打印什么。

> **动态兜底**：懒得找 key，也可以 hook native 解密函数的**输出**（解密后、喂给 `JS_ReadObject` 之前）dump 明文字节码——这对"整体加密"有效（和 5.4 的 opcode 重排不同，加密只是外面一层，dump 到的是解密后的标准字节码）。但 key 是静态常量，找 key 更干净、可离线复现。

---

## 三、检查点

- 能认出"整体加密"（落盘首字节非 `0x1b`、高熵、`strings` 无可读串），区别于 5.3/5.4 的局部改动。
- 能说清整体加密的正解是**找 key + 认算法**（不破密码）：key 是构建期常量，藏 native/rodata/C#，静态可取。
- 会从 `.rodata` 认出算法指纹 + key/IV，用 openssl 离线 AES 解密，确认解出标准字节码（首字节 `0x1b`）。
- 会从解密后的字节码用 `strings` / 运行读出里面的 flag（QuickJS 字符串字面量明文存在字节码里）。

---

## 动手

- **作业（和 4.8 同套路，找 flag）**：下载 `作业/材料/qjbc-aes-kit.zip`——`skills.qjbc.enc` 是一份被 **AES-128-CBC 整体加密**的 QuickJS 字节码（`strings` 抠不到任何可读串）；`libgame_rodata.bin` 是从 native 抠出的 `.rodata`，含算法指纹 `AES-128-CBC` + 解密函数名 + 16B key + 16B IV。`strings` rodata 认出算法、找到 key/IV → `openssl` AES-128-CBC 解密 → 得标准 `.qjbc`（首字节 `0x1b`）→ `strings` 解出的字节码（或用 kit 带的 `qjbc-run.exe` 跑），里面那行 `FLAG{...}` 就是答案。任务详情见 `作业/README.md`。

---

## 小结 & 下一章

整体加密的破法就一句话——**找 key + 认算法，不破密码**。key 是构建期常量（藏 native/rodata/C#），静态可取；拿到离线解，就回到标准 QuickJS 字节码（`strings` 即读里面的字符串）。**QuickJS 三节收尾**：5.3 改格式（修 version 头）/ 5.4 改字节码（差分恢复 opcode）/ 5.5 整体加密（找 key 解密）——和 Lua 4.6/4.7/4.8 完全同构。至此 puerts 的两条后端路（V8 静态 5.1–5.2 / QuickJS 5.3–5.5）都走完了。下一课 **5.6** 是脚本层**动态注入**的收尾章——往活着的 puerts 引擎注入代码，**V8 和 QuickJS 通用**（同一个 `Eval` 入口，抓引擎句柄→注入），也是 Lua 4.9 的 puerts 版。
