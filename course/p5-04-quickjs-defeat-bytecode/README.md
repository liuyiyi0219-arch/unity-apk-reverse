# 5.4　QuickJS 破改字节码

> 状态：🟢 已完成（正文 + 真编译实证）｜Part 5 · puerts 的 QuickJS 那条路（对应 Lua 4.7）

---

## 一、本章的目的

上一节（5.3）是最轻的——只改了字节码文件的头。这一节上硬的：**魔改 opcode 编码 / VM**。游戏改了 QuickJS VM 的 **opcode 编号**（重排 / 换号），header 完全正常、`version` 对得上，标准运行时**能读进去**，但按标准 opcode 表解释每条指令 → 跑出垃圾、抛异常。破法只有一条正路：**恢复 opcode 映射（差分 / 逆 dispatch）**，让映射正确的解释器去跑。这和 Lua 4.7 是同一套。

> **这类在做什么**：QuickJS 把 JS 编成自己的字节码，每条指令 = **1 字节 opcode + 操作数**（opcode 编号由 VM 的 opcode 表定义，标准里 `push_atom_value`、`get_field`、`return` 各有固定编号）。游戏拿 QuickJS 源码**改自己的 VM**：把 opcode 编号重排（比如把 `push_atom_value` 和 `get_field` 的编号对调），用配套 `qjsc` 编字节码。游戏的 VM 认新编号，标准运行时不认。**关键：header 没动（version 正常）、操作数没动，只有 opcode 字段被换了号**——所以 5.3 那种"改 header"的招在这没用；反过来结构完全正常，**把映射表纠正过来就能解**，这是突破口。

---

## 二、操作指南

### 第 1 步 · 诊断：崩在"执行"而非"读头部"

标准运行时读魔改 opcode 字节码：**version 校验过了**（header 正常），进到执行阶段才出问题：
```
[qjbc-run] 执行异常：
TypeError: cannot read property 'FLAG{' of undefined
```
注意**崩的阶段**：不是 `JS_ReadObject` 的版本校验（5.3 那类），而是**执行到一半**——因为 opcode 被换了号，标准解释器把 `push_atom_value` 当成了 `get_field` 去跑，栈全乱了。**报错阶段本身就是分类依据**：拒在版本校验 = 改格式（5.3）；读得进去、崩/乱在执行 = 改 opcode（本章）。

### 第 2 步 · 差分恢复 opcode 映射（核心）

**思路**：opcode 只是被"换了号"、操作没变。拿**同一份源码**的标准编译产物和游戏的产物**逐字节比较**，就读出映射：

1. 标准 `qjsc` 编一份已知源码（`sample.js`）→ `sample.stock.qjbc`；
2. 游戏（魔改 VM）的 `qjsc` 编同一份 → `sample.op.qjbc`；
3. **同源 → 两份字节码除 4 字节校验和外结构完全一致，只有 opcode 字节被换了号、位置对齐**，逐字节比：位置 i 标准是 `4`(push_atom_value)、魔改是 `64` → **魔改编号 64 = push_atom_value**。

kit 带的 `diff-opcodes.py` 就是干这个（排除头部第 1–4 字节的校验和，其余每个差异字节直接读出置换）：
```
标准   4  →  魔改  64   (出现 3 次)   ← push_atom_value 被换成 64
标准  17  →  魔改  70   (出现 2 次)   ← dup 被换成 70
标准  40  →  魔改  14   (出现 1 次)   ← return 被换成 14
...
```

> **为什么同源两份能逐字节对齐**：opcode 重排只改**编号**、不改**指令长度**，也不动操作数——所以两份字节码每条指令都在同一偏移，差异只出现在 opcode 字节。这让差分极干净（不像变长指令要逐条 walk）。**另一条恢复途径**：逆 **native VM 的 dispatch**——QuickJS 主循环是个大 `switch(op)`（`JS_CallInternal`），在 `libpuerts.so` 里找到它，看每个 opcode 号跳到哪个 handler、对照标准 QuickJS handler 特征读出映射。差分更快，逆 dispatch 更全（不受样本覆盖限制）。

### 第 3 步 · 用映射正确的解释器跑（QuickJS 和 Lua 的差别在这）

拿到映射后要**把它用出去**。这里 QuickJS 和 Lua 有个**关键差别，必须讲清**：

- **Lua（4.7）字节码定长**（每条指令 32 位），所以可以直接**改字节码文件**里的 opcode 字段、换回标准，再用标准 lua 跑；
- **QuickJS 字节码变长**（每条指令长度不一），而且**读入时 VM 会按 opcode 走一遍做 atom 重定位**——所以实战里不是改文件，而是**把恢复出的映射用回解释器 / decoder**：改 VM 的 opcode 表（或 dispatch）、重编，得到一个"认这套编号"的运行时。

本课配套的 **`qjbc-run-op.exe` 就是这样的运行时**（opcode 表按恢复的映射重排、重编）——它就是游戏 `libpuerts.so` 里那个 QuickJS 的等价物。用它跑魔改字节码：
```
$ qjbc-run.exe    secret.op.qjbc     # 标准表：执行异常（opcode 对不上）
$ qjbc-run-op.exe secret.op.qjbc     # 映射正确的表：FLAG{...}
```
flag 是脚本**运行时用算法拼出来的**（`String.fromCharCode` + 分段拼接），`strings` 抠不到——opcode 不恢复对就跑不出来。

> **改 VM 表 = 改 reversing 工具**：这正对应 Lua 4.7 里"改反编译器 `OpcodeMap.java` 再重编"——都是**把恢复出的映射应用回工具**。区别只是 Lua 定长可以顺便改文件、QuickJS 变长走改解释器这条。恢复出的映射（`qjs-op-src/` 里那几行 opcode 互换）就是游戏 VM 和你的工具都要认的那张表。

### 第 4 步 · 为什么 hook/dump 绕不过 opcode 重排

和 Lua 4.7 一样：整体加密（5.5）能靠 hook `JS_Eval`/`external.doBuffer` 拿到解密后的字节码——但 **opcode 重排 hook 拿不到明文代码**：

- 游戏发的、VM 吃的、你 hook 拿到的，**是同一份魔改字节码**（VM 本来就认这套编号）。dump 出来和盘上那份一样，**还是重排的、读不了**；
- QuickJS **直接解释字节码、运行时没有 JS 源码 / AST 可 dump**——想读逻辑，只能**恢复 opcode 映射**（差分 / 逆 dispatch）把它换回标准语义再读。

---

## 三、检查点

- 能从"version 校验过了、崩在执行阶段"判断出是改 opcode 类（区别于 5.3 的改格式，拒在版本校验）。
- 能用差分分析（同源逐字节比较、排除校验和）恢复 opcode 映射表。
- 能说清 QuickJS 和 Lua 的差别：Lua 定长可直接改文件换回 opcode；QuickJS 变长 + 读入做 atom 重定位，所以把映射用回**解释器 / VM 表**（重编运行时），或逆 `libpuerts.so` 的 dispatch。
- 知道 opcode 重排 hook/dump 绕不过（拿到的还是魔改字节码、没有明文可 dump），也能说清和 Lua 4.7 是同一套。

---

## 动手

- **作业（和 4.7 同套路，找 flag）**：下载 `作业/材料/qjbc-opcode-kit.zip`——`secret.op.qjbc` 跑起来会用算法拼出 flag，但 opcode 被真重排（header/version 正常，标准运行时 `qjbc-run.exe` 崩在执行阶段）。用 `diff-opcodes.py` 对样本对 `sample.stock.qjbc`↔`sample.op.qjbc` 做差分恢复 opcode 映射（或逆 `qjbc-run-op.exe`/游戏 `libpuerts.so` 的 dispatch）→ 用映射正确的运行时 `qjbc-run-op.exe` 跑 `secret.op.qjbc` → 读出 flag。`qjbc-run-op.exe` 的 opcode 表被**真魔改成同一套映射**，就是能跑这套字节码的那个 VM（对应游戏 `libpuerts.so` 里的 QuickJS）。任务详情见 `作业/README.md`。

> **几个坑**：差分只能恢复"样本里出现过的" opcode（样本没用到的推不出）→ 用覆盖面广的样本，或直接逆 VM dispatch；有的魔改**不止重排**、还改指令长度/操作数布局（那要动指令解码，比换号重）——先确认只是"换号"（同源两份定长对齐、只差 opcode 字节，就是纯重排）；魔改 VM 的运行时"一游戏一份"、不能通用（用 A 游戏的表解 B 游戏会崩）。

---

## 小结 & 下一章

改 opcode 这类的破法核心是**"opcode 只是换了号、语义没变"**：同源差分恢复映射 → 把映射用回解释器 / VM 表。和 Lua 4.7 同一套，差别只在 Lua 定长可改文件、QuickJS 变长走改运行时（+ 逆 dispatch）。（opcode 重排 hook/dump 绕不过——拿到的还是魔改字节码。）下一课 **5.5** 破最后一类、也是最常见的一类——**整体加密的 QuickJS 字节码**（找 key 解密），对应 Lua 4.8。
