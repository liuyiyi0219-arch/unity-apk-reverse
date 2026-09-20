# 4.7　Lua 破改字节码

> 状态：🟢 已完成（正文 + 演示程序 + 真编译实证）｜演示程序：`正课/lesson-4.7.exe`

---

## 一、本章的目的

一类更硬的 Lua 保护——**改字节码 / 魔改 VM**：游戏改了 Lua VM 的 **opcode 编码**（重排 / 换号），header 完全正常，但标准反编译器解出来是垃圾甚至崩溃。破法只有一条正路：**恢复 opcode 映射（差分 / 逆 `luaV_execute`）+ 改反编译器**，把字节码换回标准再反编译。（注意：**opcode 重排 hook/dump 绕不过**——`loadbuffer` 拿到的还是魔改字节码，见第 4 步。）

> **这类在做什么**：Lua 5.1 每条指令 32 位，**低 6 位是 opcode**（0–37 共 38 个），标准映射固定（`0=MOVE, 22=JMP, 30=RETURN`…）。游戏拿 Lua 源码**改自己的 VM**：把 opcode 编号重排，用配套 `luac` 编译。游戏的 VM 认得新编号，标准反编译器不认。**关键：header 没动、常量表没动，只有指令的 opcode 字段变了**——所以 4.6 那种"改 header"的招在这没用；反过来字节码结构完全正常，**把映射表纠正过来就能解**，这是突破口。

---

## 二、操作指南

### 第 1 步 · 诊断：崩在"解码之后"

stock unluac 解魔改字节码：
```
ArrayIndexOutOfBoundsException: Index -130042 out of bounds for length 6
    at unluac.decompile.ControlFlowHandler.find_reverse_targets(...)   ← decompile 阶段，不是 parse
```
注意**崩的位置**：不是 `parse`（header 解析，4.6 那类），而是 `decompile/ControlFlowHandler`（控制流分析）——因为 `JMP` 被换成了别的、跳转目标算出来是天文数字。**报错位置本身就是分类依据。**

### 第 2 步 · 差分分析恢复 opcode 映射（核心）

**思路**：opcode 只是被"换了号"、操作没变。拿**同一份源码**的标准编译产物和游戏的产物**逐条指令对位比较**，就读出映射：

1. 标准 `luac` 编一份源码 → 标准 opcode 序列；
2. 游戏（魔改 VM）编出的同源文件 → 魔改 opcode 序列；
3. 按位置对齐逐条比：位置 i 标准是 `JMP(22)`、魔改是 `30` → **魔改编号 30 = JMP**。

实测（我们的 `Battle.lua`）推出：`map[0]=GETGLOBAL, map[1]=ADD, map[12]=LOADK, map[22]=RETURN, map[30]=JMP`。

> **原理 · Lua 引擎的关键代码 `luaV_execute`（读懂它才知道"改的是什么"）**：Lua VM 跑字节码的核心是一个大循环——取一条 32 位指令 → 低 6 位 opcode 查跳转表 → 跳对应 handler。标准 Lua 5.1 的 `lvm.c` 里长这样（简化）：
> ```c
> void luaV_execute(lua_State *L, int nexeccalls) {
>   for (;;) {
>     Instruction i = *pc++;              // 取指
>     switch (GET_OPCODE(i)) {            // GET_OPCODE(i) = i & 0x3f，就是低 6 位
>       case OP_MOVE:      /* 0  */ ... break;
>       case OP_LOADK:     /* 1  */ ... break;
>       case OP_GETGLOBAL: /* 5  */ ... break;   // 用常量当 key 查 _G
>       case OP_ADD:       /* 12 */ ... break;
>       case OP_JMP:       /* 22 */ dojump(L, pc, GETARG_sBx(i)); break;  // pc 加偏移
>       case OP_RETURN:    /* 30 */ ... return;  // 函数收尾
>       /* ... 共 38 个 opcode ... */
>     }
>   }
> }
> ```
> **魔改 VM 动的就是这张「opcode 号 → handler」的对应**：厂商把 `lopcodes.h` 的枚举重排、配一套同样重排的 `luac`，于是它发出的字节码里 30 号其实跳 ADD、12 号其实跳 RETURN……标准 VM 按标准表跳就崩 / 出垃圾。
>
> **另一条恢复途径 —— 逆 `luaV_execute` 拿全表**：在 `libxlua.so`/`libtolua.so` 里定位这个大 switch / 跳转表，看每个 opcode 号跳到哪段 handler，对照标准 handler 的指纹（`OP_JMP` 的 `pc += sBx`、`OP_RETURN` 的栈收尾、`OP_GETGLOBAL` 用常量查全局表）读出「魔改号 → 标准号」。差分分析更快，**逆 dispatch 更全——不受样本覆盖限制，是差分覆盖不全时的权威兜底**。

### 第 3 步 · 改反编译器 `OpcodeMap.java`（真实 before/after）

unluac 的 opcode 映射就在 `unluac/decompile/OpcodeMap.java` 的 `case LUA51:`，一行一个，**按恢复出的映射改那几行**：
```java
// 修改前（upstream 标准）     →    修改后（按恢复的映射）
map[0]  = Op.MOVE;                map[0]  = Op.GETGLOBAL;   // ← 魔改 VM 重排
map[5]  = Op.GETGLOBAL;          map[5]  = Op.MOVE;
map[12] = Op.ADD;                map[12] = Op.LOADK;
map[22] = Op.JMP;                map[22] = Op.RETURN;
map[30] = Op.RETURN;             map[30] = Op.JMP;
```
**只改映射表**——指令格式、常量表、控制流分析全不用动。重新 `javac`（同 4.6）再解 → **完整还原** `Battle.CalcDamage`。（完整 before/after + diff 见 `正课/sample-output/patch/`。）

> **一个必须知道的后果：这个 jar 不能通用。** 改完 opcode 表的 jar，**解标准字节码反而会崩**（本课实测对称报错）。所以魔改 opcode 的反编译器是"**一游戏一份**"的，别 drop-in 替换团队共用的 stock jar、也别拿 A 游戏的 jar 解 B 游戏（记忆里踩过的坑）——工程上按游戏归档 `unluac-<game>.jar`。

### 第 4 步 · 为什么"运行时 dump"绕不过 opcode 重排

整体加密（4.8）能靠 4.4 的 `luaL_loadbuffer` hook 拿到解密后的明文——但 **opcode 重排不行，hook 拿不到明文代码**：

- 游戏发的、VM 吃的、你 hook `loadbuffer` 拿到的，**是同一份魔改字节码**（VM 本来就认这套编号）。dump 出来和盘上那份一模一样，**还是重排的、读不了**；
- Lua VM **直接执行字节码、运行时没有源码 / AST 可 dump**——想读逻辑，只能**恢复 opcode 映射**（第 2–3 步：差分 / 逆 `luaV_execute`）把它换回标准再反编译。

> 唯一 dump 有用的情况：魔改字节码**外面又叠了一层加密**——那 hook 到的是"解密后的魔改字节码"，只去掉了加密层，**opcode 还是重排的**，仍要恢复映射。加密那一层归 4.8。

---

## 三、检查点

- 能从"崩在 decompile 阶段而非 parse 阶段"判断出是改字节码类。
- 能用差分分析（同源对位比较）恢复 opcode 映射表。
- 能改 `OpcodeMap.java` 重编译出配套反编译器，并解出正确源码。
- 知道这种 jar 一游戏一份、不能通用（按游戏归档）；也能说清 opcode 重排为什么 hook/dump 绕不过（拿到的还是魔改字节码，没有明文代码可 dump）。

---

## 动手

- **演示（`正课/lesson-4.7.exe`，需 luac 5.1.5 + JDK）**：真跑 luac 编标准字节码，再用内置 luac walker **遍历所有函数原型、按置换重写每条指令的 opcode**（模拟魔改 VM 产物）→ 真跑 stock unluac（解标准 ✔ / 解魔改 ✘ 崩在 `ControlFlowHandler`）→ 逐条对位比较**现场推出 opcode 映射表** → 展示 `OpcodeMap.java` 真实修改前/后代码 → 从 patch 过的源码副本 `javac` 出 `unluac-vmmod.jar` 反编译成功、和原始对照 → 反证不通用（patched jar 解标准字节码崩）→ 路线 B 小结。
- **作业（和 4.6 同套路，找 flag）**：下载 `作业/材料/opcode-vm-kit.zip`——`secret.opmod.luac` 跑起来会用算法拼出 flag，但 opcode 被真重排（header 正常，标准 lua 报 `bad code`）。恢复 opcode 映射（**差分** `reward.vmmod.luac`↔锚点，或**逆** kit 里 `xlua-op-demo.apk` 的 `libxlua.so` 的 `luaV_execute`）→ 把 `secret.opmod.luac` 每条指令的 opcode 换回标准 → 用 kit 带的 Lua 5.1 跑 → 读出 flag。**换 opcode 这一步用 kit 里的 `remap_luac.py`**（Lua 5.1 字节码重写器：递归遍历所有 proto、把每条指令低 6 位 opcode 按你给的映射换掉）——把你恢复出的映射填进 `mapping.template.json`，跑 `python remap_luac.py secret.opmod.luac secret.fixed.luac mapping.json`，再 `lua5.1/lua.exe secret.fixed.luac`。`xlua-op-demo.apk` 的引擎被**真魔改成同一套映射**，就是能跑这套字节码的那个 VM。任务详情见 `作业/README.md`。

> **几个坑**：差分只能恢复"样本里出现过的" opcode（样本没用到 `MOVE` 就推不出它的新号）→ 用覆盖面广的样本集（含循环/闭包/比较/表操作），或直接逆 VM dispatch 表。**本课作业实测就有这个缺口**：差分锚点 `reward` 覆盖不到 `secret` 用的 `GETGLOBAL / SUB / TAILCALL / SETLIST` 这几个号——只靠差分把它们当恒等换回去，`secret.fixed.luac` **会在运行时崩**（栈上索引到没赋值的寄存器）。补法两条：读 `secret` 自身指令的操作数语义定死（`iABx` 用常量当名 = `GETGLOBAL`；`NEWTABLE + LOADK×N + ?` = `SETLIST`；减 7 出干净可打印 ASCII 才是 `SUB`），或逆 `luaV_execute` dispatch 一次拿全 38 个。映射不全会在别的文件上崩，先在多样本验证映射闭合；有的魔改**不止重排**、还改指令位宽/参数布局（那要动指令解码，比改映射表重）——先确认只是"换号"。演示程序把源码复制到本课工作区再 patch/编译，**不动共享的 `vendor/unluac/`**（4.6 同纪律）。
>
> **LuaJIT 特别提醒**：`\x1bLJ` 是另一套（字节码格式 / opcode 集不同、**没有 `luaL_loadbuffer` 明文入口**、2.0/2.1 版本死锁），魔改 LuaJIT 要改 `luajit-decompiler-v2`/`ljd`，比改 unluac 硬得多。判运行时永远第一步。

---

## 小结 & 下一章

改字节码这类的破法核心是**"opcode 只是换了号、语义没变"**：差分对位恢复映射 → 改反编译器映射表 → 重编译。（opcode 重排 hook/dump 绕不过——拿到的还是魔改字节码。）代价是**配套 jar 一游戏一份**。下一课 **4.8** 破最后一类、也是最常见的一类——**整体加密的 Lua**（找 key 解密）。
