# 5.3　QuickJS 破改格式

> 状态：🟢 已完成（正文 + 真编译实证）｜Part 5 · puerts 的 QuickJS 那条路（对应 Lua 4.6）

---

## 一、本章的目的

5.1–5.2 讲的是 **V8 后端**——生产里一定是明文 JS，逆向面是"抠 JS + 去混淆"。这一节切到 **QuickJS 后端**：它是**解释执行**（把 JS 编成 QuickJS 自己的字节码再解释），所以厂商**能魔改这个 VM**——正因为如此，QuickJS 这条路和 **Lua 完全同构**，接下来三节（5.3/5.4/5.5）就是 Lua 4.6/4.7/4.8 的 QuickJS 版：

| 这一节 | 对应 Lua | 干的事 |
|---|---|---|
| **5.3（本章）** | 4.6 改格式 | 魔改字节码 **header 格式**（version 字段），标准 VM 拒 → 修回标准 → 跑 |
| 5.4 | 4.7 改字节码 | 魔改 **opcode 编码**，标准 VM 跑出垃圾 → 恢复映射 → 换回 → 跑 |
| 5.5 | 4.8 整体加固 | **对称加密**整份字节码 → 找 key → 解密 → 读 flag |

本章是最轻的一类：**只动了字节码文件的头**，指令本身没变。破法一句话——**认出被改的头字段、改回标准、用标准运行时跑**。

> **QuickJS 字节码是什么**：`qjsc -b` 把 JS 编成一份原始字节码（`JS_WriteObject` 的产物）。文件头固定布局：**第 1 字节 = `BC_VERSION`（标准值 27 = `0x1b`）**，紧接 4 字节校验和，然后是 atom 表 + 函数体。游戏拿 QuickJS 源码**改自己的 VM**：把 `BC_VERSION` 改成一个非标准值（比如 `0x42`），用配套 `qjsc` 编字节码。游戏的 VM 认这个新版本号，标准 `qjs`/`JS_ReadObject` 一看版本对不上就拒。**关键：只有头 1 个字节变了，校验和和指令流没动**——所以改回标准值就能用标准运行时跑。

---

## 二、操作指南

### 第 1 步 · 诊断：拒在"读头部"阶段

标准运行时读魔改字节码，第一步就报：
```
SyntaxError: invalid version (66 expected=27)
```
注意**报错位置**：是 `JS_ReadObject` 的**版本校验**（读头部第 1 字节），还没进指令解码。报错文本直接把两个数摊开了——`66`（=`0x42`，文件里的值）vs `27`（=`0x1b`，标准值）。**报错本身就是分类依据**：拒在版本校验 = 改格式类（本章）；能读进去、跑出垃圾/崩在执行 = 改 opcode 类（5.4）。

### 第 2 步 · 认出被改的头字节

QuickJS 字节码头部布局（`JS_WriteObject` 写、`JS_ReadObject` 读）：

| 偏移 | 长度 | 字段 | 标准值 |
|---|---|---|---|
| 0 | 1 | `BC_VERSION` | **`0x1b`（27）** |
| 1 | 4 | 校验和（over 头后的 body） | 随内容 |
| 5.. | leb128 | atom 表长度 + atoms | 随内容 |

对照标准值，文件第 1 字节是 `0x42` 而不是 `0x1b`——**这就是被改的字节**。

> **为什么改回来不破坏校验和**：QuickJS 的校验和**不覆盖 version 和 checksum 这两个字段本身**（只算它们后面的 body）。所以改 version 字节不影响校验和；把 `0x42` 改回 `0x1b`，校验和照样对得上。（源码 `quickjs.c`：写头处注释 `don't include version and checksum fields in checksum`。）

### 第 3 步 · 改回标准值，用标准运行时跑

把文件第 1 字节 `0x42` 改成 `0x1b`（十六进制编辑器改 1 个字节，或 `printf '\x1b' | dd of=file bs=1 seek=0 count=1 conv=notrunc`），然后用标准 QuickJS 运行时跑：
```
$ qjbc-run  secret.fmt.qjbc          # 没修：SyntaxError: invalid version (66 expected=27)
$ # 改第 1 字节 0x42 → 0x1b
$ qjbc-run  secret.fixed.qjbc        # 修好：FLAG{...}
```
flag 是脚本**运行时用算法拼出来的**（`String.fromCharCode` + 分段 `join`），不是明文常量，`strings` 抠不到——**头不修好跑不起来，也就拿不到**。

> **另一条路：逆引擎确认它要哪个版本号。** 和 Lua 4.6 逆 `libxlua.so` 一样：真实游戏里这套魔改 QuickJS 编进 `libpuerts.so`（QuickJS 后端）。在里面找 `JS_ReadObject` 的版本校验那处常量（`cmp` 立即数就是它要的版本号），就知道该把头改成几。改一个常量的魔改，逆一处 `cmp` 就够。

### 第 4 步 · 或者改运行时的校验（改引擎那半）

对称地，也可以**改标准运行时去接受魔改的版本号**（改 `quickjs.c` 里 `#define BC_VERSION`，重编）——这正是游戏那半做的事，本课配套的魔改运行时就是这么来的（一行改动：`BC_VERSION 27 → 0x42`）。工程上：**改文件头**适合手上只有一份字节码要读；**改运行时**适合你要批量跑很多份同款魔改字节码。

---

## 三、检查点

- 能从"拒在 `JS_ReadObject` 版本校验（invalid version）而非执行阶段"判断出是改格式类（区别于 5.4 的改 opcode）。
- 知道 QuickJS 字节码头布局：第 1 字节 `BC_VERSION`（标准 `0x1b`=27）+ 4 字节校验和，且校验和不覆盖头两字段。
- 会认出被改的 version 字节、改回标准值、用标准运行时跑出 flag。
- 知道另一条路是逆 `libpuerts.so` 里 QuickJS 的版本校验常量（同 Lua 4.6 逆 `libxlua.so`）。

---

## 动手

- **作业（和 4.6 同套路，找 flag）**：下载 `作业/材料/qjbc-format-kit.zip`——`secret.fmt.qjbc` 跑起来会用算法拼出 flag，但字节码 header 的 version 字段被魔改（第 1 字节是 `0x42` 不是标准 `0x1b`），标准运行时报 `invalid version`。把第 1 字节改回 `0x1b`、用 kit 带的标准运行时 `qjbc-run.exe` 跑 → 读出 flag。kit 里还带了**魔改运行时** `qjbc-run-mod.exe`（就是那个认 `0x42` 的 VM，对应真实游戏 `libpuerts.so` 里的 QuickJS）和魔改源码 recipe（`qjs-mod-src/`，一行 `BC_VERSION` 改动）。任务详情见 `作业/README.md`。

---

## 小结 & 下一章

QuickJS 是 puerts 的另一条路——解释执行、VM 可魔改，所以和 Lua 同构。改格式这类最轻：**只动头部的 version 字段**，认出来改回标准值就通（校验和不覆盖它、指令流没变）。破法两条对称的路——**改字节码头**（手上一份要读）或**改运行时校验**（批量跑同款）。下一课 **5.4** 上硬的：魔改 **opcode 编码**（同 Lua 4.7）——header 正常、标准 VM 能读进去但跑出垃圾，得恢复 opcode 映射。
