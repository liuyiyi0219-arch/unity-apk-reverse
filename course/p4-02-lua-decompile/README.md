# 4.2　Lua 反编译

> 状态：🟢 已完成（正文 + 正课演示 + 作业批改）｜`正课/lesson-4.2.exe`（luac 真编译 + unluac 反编译，带调试 vs strip）· `作业/check.exe`（反编译 `mystery.ljbc`(LuaJIT) → 按字节表还原 → 交 flag）

---

## 一、本章的目的

明文源码直接读（4.1），但游戏常发的是**编译过的字节码**。这一课把字节码**反编译**回可读 Lua 源码——**分两套**：原生 Lua（`\x1bLua`）用 `unluac`，**LuaJIT（`\x1bLJ`）用 `luajit-decompiler-v2`**（两套完全不同，认错 magic 就白忙）。搞清：字节码结构、两套工具用法、**调试信息对还原度的决定性影响**、以及两边各自的版本坑。

> **为什么发字节码、为什么它好逆**：发字节码图省体积 + 轻度混淆 + 加载略快。但 Lua 字节码**保留了大量信息**：常量表（字符串 / 数字字面量原样存）、完整指令结构、以及**可选的调试信息**（局部变量名 + 行号）。所以反编译成功率很高——和 il2cpp 保留 metadata 一个道理：**编译没把语义抹掉，只是换了种表示**。

---

## 二、操作指南

### 第 1 步 · 认版本（magic 后第 5 字节）

```
1B 4C 75 61   (\x1bLua, magic)
51            ← 版本字节：51=5.1  53=5.3  54=5.4（LuaJIT 是 \x1bLJ）
00            format（0=官方；非 0 常是魔改，4.6）
```

> `unluac` 要对上版本，否则解不了或崩。LuaJIT（`\x1bLJ`）是另一套，换 `luajit-decompiler-v2` / `ljd`，**别拿 unluac 解**。

### 第 2 步 · unluac 反编译

原生 Lua 用 **unluac**（Java，一个 jar 打天下）：

```bash
java -jar unluac.jar Battle.luac > Battle.lua
```

### 第 3 步 · 看调试信息，决定还原度

同一份源码，编译时**带不带调试信息**，反编译结果天差地别。本课实测（Lua 5.1.5 luac 编 [`Battle.lua`](../demo/CourseDemo/Assets/GameLua/src/Gameplay/Battle.lua.bytes)）：

**① 带调试信息**（`luac`）→ **近源码级**，局部名全对：
```lua
function Battle.CalcDamage(atk, def, critMul)
  local base = atk * atk / (atk + def)
  return math.floor(base * (critMul or 1))
end
```

**② strip**（`luac -s`）→ **逻辑对、名字丢**：
```lua
function L1_1(A0_2, A1_2, A2_2)
  local L3_2 = A0_2 * A0_2
  local L4_2 = A0_2 + A1_2
  L3_2 = L3_2 / L4_2
  ...
  return math.floor(L3_2 * L5_2)
end
```
名字变成 `L0_1/A0_2`（unluac 造的）、临时变量摊开、`or` 展开成 if——**语义完全一致，可读性下降**。

> 这和 il2cpp「符号丢不丢」完全同理。**游戏发布多会 `-s` strip**（省体积 + 防逆），所以你常遇到"逻辑对、名字靠猜"。好在**常量表里的字符串照样在**（函数名、配置 key、报错文本）——是给变量重命名的金线索，别忽略。

### 第 4 步 · LuaJIT 字节码：换一套完全不同的反编译器

**LuaJIT（`\x1bLJ`）不是原生 Lua——字节码格式完全不同，`unluac` 解不了，要用专门的 LuaJIT 反编译器。** LuaJIT 在手游里非常常见（xLua/sLua 都能选它、性能好），别把它当原生 Lua。

- **工具：[luajit-decompiler-v2](https://github.com/marsinator358/luajit-decompiler-v2)**（C++，维护中）。老的 Python 版 `ljd` 早就烂了、bug 一堆、**别用**；v2 修好了、还支持 goto + strip（丢了局部名/upvalue 也能启发式还原）。
  ```bash
  luajit-decompiler-v2.exe game.ljbc          # 或把文件/文件夹拖到 exe 上
  # 反编译出的 .lua 默认落到 exe 同目录的 output/ 文件夹
  # 常用：-i 忽略调试信息  -e 只解某扩展名  --manifest 写批量结果  -f 覆盖
  ```

- **★版本死锁（LuaJIT 最大的坑）**：`\x1bLJ` 后面第 4 字节是**版本字节**——`01`=LuaJIT **2.0**、`02`=**2.1**。**2.0 和 2.1 的字节码互不兼容**，反编译器/运行时必须对上版本，解错版本直接崩或出垃圾。再叠一层 **GC64**（64 位指针模式，2.1 常开）——字节码布局又不一样。所以拿到 `\x1bLJ`：**先看版本字节、判是不是 GC64，再选对工具/模式**。
  ```
  1B 4C 4A   02   ...      ← \x1bLJ + 0x02 = LuaJIT 2.1
             ^版本字节：01=2.0  02=2.1
  ```

- **认版本 = 认工具链**：和原生 Lua「51/53/54 要对上 unluac 版本」一个道理，只是 LuaJIT 的坑更深（2.0↔2.1 完全不通 + GC64）。**先分清 原生 Lua vs LuaJIT、再分清 LuaJIT 2.0 vs 2.1/GC64，才动手。**

> 一句话对照：**`\x1bLua` → unluac（对 5.1/5.3/5.4 版本）；`\x1bLJ` → luajit-decompiler-v2（对 2.0/2.1 + GC64）。** 两套工具、两套坑，认错 magic 就白忙。

---

## 三、检查点

- 能把一份原生 `luac` 字节码用 unluac 反编译成可读 Lua 源码。
- 能从 magic 认出 **原生 Lua（`\x1bLua`）vs LuaJIT（`\x1bLJ`）**，各用哪套反编译器（unluac / luajit-decompiler-v2）。
- 说得清两套的版本坑：原生 Lua 的 51/53/54 要对上 unluac；**LuaJIT 的 2.0/2.1 字节码互不兼容 + GC64**，要对上 luajit-decompiler-v2。
- 说清"带调试 vs strip"对还原度的影响；会用常量表里的字符串给 strip 后的变量重命名。

---

## 动手

本课的动手分两块，**分开放**：

- **`正课/`** —— 走一遍参考解答。双击 `正课/lesson-4.2.exe`（需 luac 5.1.5 + unluac.jar + java）：看源码答案 `Battle.lua` → 用 luac 5.1.5 真编出**带调试** + **strip** 两版（看 magic `1B 4C 75 61 51`）→ unluac 反编译带调试版（近源码，逐行对，名字全对）→ 反编译 strip 版（名字变 `L0_1/A0_2`，逻辑对、名字丢）→ 讲版本坑 → 弹文件夹。
- **`作业/`** —— **自己动手 + 自动批改**。`作业/材料/mystery.ljbc` 是一份 strip 过的 **LuaJIT 2.1 字节码**（自造样本，一段"字符串编码进字节表、运行时解"的常见写法）：用 `luajit-decompiler-v2` 反编译它（`\x1bLJ` 是 LuaJIT，**别用 unluac**），读懂它那张字节表的解码逻辑、按逻辑还原出 flag，交给 `作业/check.exe FLAG{...}`——**答对了吐 flag 🚩，答错提示"这是 LuaJIT、别用 unluac"**。任务详情见 `作业/README.md`。

> 建议：先做 `作业/`（真动手才学得会），卡住了再翻 `正课/` 对答案。

> **本课作业用的是 LuaJIT**（`\x1bLJ`）：把 `mystery.ljbc` 拖进 `luajit-decompiler-v2.exe`（`~/.reverse-tools/luajit-decompiler-v2/`），输出落 `output/`。先看版本字节（`01`=2.0 / `02`=2.1）+ 是否 GC64，工具对上版本才解得动——这是 LuaJIT 特有的坑，比原生 Lua 深。原生 Lua（`\x1bLua` → unluac）那条路，`正课/lesson-4.2.exe` 演示过了。

> **版本坑（实战血泪）**：`unluac` 必须对上 Lua 版本，**版本不对或魔改就崩**——本课 `luac 5.1.5`（ToLua 用的）干净还原；但 `Lua 5.4.7` 编的喂标准 `unluac.jar` 会 `ArrayIndexOutOfBoundsException`；魔改 header（`format` 非 0）报 `non-standard lua format`（4.6 改工具破）。所以**崩了先怀疑版本 / 魔改、行为测**（拿实际能否解出正确指令为准）；判"成功"看能否反出可读源码。

---

## 小结 & 下一章

你能把 luac 字节码反编译回源码了，也懂了调试信息决定还原度、版本决定工具。下一课 **4.3** 做一件高价值**应用**——把 **Lua 配表还原成策划 CSV**（`Hero.lua`/`Item.lua` 那种数值表 → 可读表格），这是"从成品反推策划数据"的通法。
