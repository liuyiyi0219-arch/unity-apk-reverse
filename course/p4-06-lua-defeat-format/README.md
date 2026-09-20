# 4.6　Lua 破改格式

> 状态：🟢 已完成（正文 + 演示程序 + 真编译 unluac 实证）｜演示程序：`正课/lesson-4.6.exe`

---

## 一、本章的目的

Lua 保护里最轻、也最直白的一类——**改格式**：魔改 luac header（改 `format` 字节、增删 / 调换字段），让标准反编译器不认。手法直白但很有代表性：**读报错 → 在反编译器源码里找到那个校验 → 改它 → 重新编译 → 解开**。这是逆向里"工具不趁手就改工具"的典型，也是 4.7 的基础。

> **这类保护在防什么**：`luac` 开头 12 字节是 header，声明版本 / format / 字长。反编译器**先解 header 再解指令**——header 对不上就当场拒绝、连指令都不看。游戏于是做最省事的防守：**动 header**（改 `format` 字节 / 增删字段 / 调换顺序）。**指令和常量表完全没动**——所以这是 Lua 保护里最轻的一类，文件本体是标准字节码，只是"报了个假身份证"。
>
> **本课碰两种变体，别混**：
> - **演示**（`正课/lesson-4.6.exe`）走更狠的一个真实变体——**xLua format=1**：`format` 字节设成 `1`，且**整个省略 `sizeof(Instruction)` 那一字节**（指令仍是标准 4 字节），布局整体前移，标准 unluac 一读就错位，得**改 unluac 的 header 解析**才反编译得动（下面第二节演示的就是这个）：
> ```
> 标准 5.1 头 (12B): 1b 4c 75 61 51 00 01 04 08 04 08 00
> xlua fmt1 头 (11B): 1b 4c 75 61 51 01 01 04 08    08 00   ← fmt=1 + ins 字节没了、后面整体前移
> ```
> - **作业**（`Reward.tampered.luac`）是**最轻的单字节变体**：只有 `format` 字节被改（`00 → 0x42`）、12 字节布局**原封不动**。它**不用改 unluac**——把那个字节改回 `00`、用 kit 的 Lua 5.1 运行时**跑它**即可（flag 是脚本运行时用算法拼出来的，所以是"改回去 + 跑"，不是"反编译读源码"）。

---

## 二、操作指南：改反编译器（两处 patch）

### 第 1 步 · 读报错定位

stock unluac 喂这个文件：
```
IllegalStateException: The input chunk reports a non-standard lua format: 1
    at unluac.parse.LHeaderType.parse_format(LHeaderType.java:87)
```
**报错直接给了类名 + 行号**——反编译器是开源的，顺着去看就行。

### 第 2 步 · 改 `parse_format`：接受 format=1

```java
// 修改前（upstream）              →   修改后
int format = 0xFF & buffer.get();
if(format != 0) {                     if(format != 0 && format != 1) {   // ← 放行 format=1
    throw new IllegalStateException("... non-standard lua format: " + format);
}
```

### 第 3 步 · 改 `parse_instruction_size`：format=1 时跳过那个字节

```java
// 修改前（upstream）                    →   修改后
protected void parse_instruction_size(...) {   protected void parse_instruction_size(...) {
    int instructionSize = 0xFF&buffer.get();       if(s.format == 1) return;   // ← xLua fmt1 省略了这字节，不读，避免后续全部错位
    if(instructionSize != 4) throw ...;            int instructionSize = 0xFF&buffer.get();
}                                                  if(instructionSize != 4) throw ...;
                                               }
```

> **只改这两处**——因为指令 / 常量表根本没被动，header 一对齐，剩下的标准逻辑照常工作。`正课/sample-output/patch/` 附了完整教学材料：两个方法的 `.before.java`/`.after.java`、可 `git apply` 的 unified diff、以及 `LHeaderType.stock.java`（原版）vs `LHeaderType.patched.java`（打过 patch）两份完整源码对照。

### 第 4 步 · 重新编译（unluac 纯 Java，秒级）

```bash
find src -name "*.java" > sources.txt
javac -d build/classes @sources.txt
jar cfm unluac-patched.jar src/MANIFEST.MF -C build/classes .
```

### 第 5 步 · 解开

```
$ java -jar unluac-patched.jar Battle.xluafmt1.luac
function Battle.CalcDamage(atk, def, critMul)
  local base = atk * atk / (atk + def)
  return math.floor(base * (critMul or 1))
end
```
**完整源码回来了**——这类保护的强度就到这：认出 format 变体（一次性活）后就是确定性重放，同批文件一把全解。真实语料里更狠一档是**魔改 opcode**（不是改 header、是改字节码语义），得靠差分分析补出映射表——留到 4.7。

---

## 三、检查点

- 能读 unluac 的报错定位到源码里的校验点。
- 能改 header 解析逻辑（接受新 format / 跳过被删字段）并重新编译出可用的 jar。
- 能解出可读源码并和原始对照验证，而不是只看"header 过了"。
- 知道只改 `format` 不改布局会导致后续字段**整体错位**（报一个看似无关的错，如 `invalid code for lua number integrality: 8`）——**报错换了个地方 = 布局还没对齐**。

---

## 动手

- **演示（`正课/lesson-4.6.exe`，需 JDK）**：用真 `luac 5.1.5` 编标准 luac 再造 xlua-fmt1 变体、**逐字节对比两个 header** → 真跑 stock `unluac.jar` 碰壁（`non-standard lua format: 1`，带类名行号）→ 并排展示 `parse_format` 的**真实修改前/后代码**（从 `正课/sample-output/patch/` 读真文件）→ 同样展示 `parse_instruction_size` 的前/后 + 指向完整 diff → `javac` 133 源文件 + `jar` 打包产出本课自己的 `unluac-patched.jar` → 用自建 jar 反编译成功、和原始 `Battle.lua` 对照 → 方法论小结。
- **作业**：下载 `作业/材料/reward-luac-kit.zip`——`Reward.tampered.luac` 的 header 被改过（改格式），标准 lua/unluac 不认。对比标准 Lua 5.1 头认出被改的字节、改回去，再用 kit 里带的 **Lua 5.1 运行时**跑它——flag 是脚本**运行时用算法拼出来的**（`strings` 抠不到），header 不修好跑不起来就拿不到。kit 里还配了一个**真 Unity+xLua 游戏 `xlua-demo.apk`**，它的 `libxlua.so` 被真魔改（VM 期望 `format=0x42`）——抠出 `libxlua.so` 逆它、定位 header 校验，就能看清"改格式"在真实游戏 native 层长什么样（大家实战就是这么逆 `libxlua.so` 找逻辑）。任务详情见 `作业/README.md`。

> **方法论：对付一个没见过的 header 变体**——① 拿一份标准的做对照（同版本 `luac` 自己编一个，`xxd` 看头）② 逐字节 diff（哪个字节变了、长度变了几字节）③ 顺着报错进源码（开源、带类名行号）④ 改校验 / 布局 → 重编译 → 试解 ⑤ 验成功看输出（解出可读合理源码才算）。演示程序把 patched jar 编到**本课目录**，不动共享的 `vendor/unluac/unluac.jar`（stock 版留着做对照）。

---

## 小结 & 下一章

改格式是最轻的一类：**指令没动，只是 header 撒谎**——读报错、改两行、重编译就破了。你也顺带学会了通法：**工具不认就改工具**（反编译器都是开源的）。下一课 **4.7** 对付更狠的：**改字节码 / 魔改 VM**——opcode 编码被重排，header 修好也没用，得逆出指令映射或走运行时 dump。
