# 4.3　Lua 配表还原

> 状态：🟢 已完成（正文 + 正课演示 + 作业批改）｜`正课/lesson-4.3.exe`（配表 lua → 策划 CSV，round-trip 字节验证）· `作业/check.exe`（还原 Weapon 配表 → flag）

---

## 一、本章的目的

把游戏的 **Lua 配表**（英雄 / 道具 / 技能 / 掉落等数值表）**还原回策划维护的原始 CSV**——一次拿到全套数值。这是"从成品反推策划数据"的通法，也是数值分析 / 竞品研究最想要的东西。核心手法：**复刻加载器读表导 CSV，而不是硬逆游戏的表加载逻辑**。

> **配表怎么来的（正反对称）**：策划用 Excel/CSV 维护数值 → 构建时 [`csv2lua`](../demo/tools/csv2lua.py) 转成 Lua 配表打进游戏。所以**配表 lua = 策划数据的机器形态**，逆回去（`lua2csv`）就拿到策划表，两条链严格对称。**为什么这么值**：一次拿到所有英雄的 Hp/Atk/Def、所有道具价格、技能、掉落率、商店价——策划表级别，可直接做平衡性分析、竞品数值对比。逆一个游戏，配表往往比代码更值钱。

---

## 二、操作指南

### 第 1 步 · 认形态（决定难度）

| | keyed / 带注解（**本课 demo**） | positional / 紧凑 |
|---|---|---|
| 数据行 | `{["Id"]=1,["Name"]="Knight",…}`（字段名在每行） | `{1,"Knight",1200,…}`（纯数组，无字段名） |
| 列名来源 | 每行自带 + HeaderTable + 注解 | **只在 header/index 里** |
| 还原难度 | 低（自描述） | 中（先解析 header 再对齐数据） |
| 典型 | csv2lua/ToLua 常见 | 省体积商业方案（`return {index,data,vExt}`，`vExt` 字符串池要展开） |

我们 demo 的 [`Hero.lua`](../demo/CourseDemo/Assets/GameLua/config/Hero.lua.bytes) 是 keyed 带注解，**自包含**——字段元信息（名 / 类型 / 中文说明）+ HeaderTable + DataTable 全在里面，连 EmmyLua 注解里的中文说明都留着：
```lua
--[[ auto generated from Hero.csv, don't modify it! ]]
---@field Id int @ 英雄ID       ← 字段名 + 类型 + 中文说明（= CSV 前三行！）
config.HeroHeaderTable = {["Id"]=0, ["Name"]=1, ...}         ← 字段 → 列序
config.HeroTable = { [1] = {["Id"]=1, ["Name"]="Knight", ["Hp"]=1200, ...}, ... }
```

### 第 2 步 · 复刻加载器读表

游戏运行时用一个 `LocalController`/`ConfigManager`（表加载器）读表，它带一堆外部依赖（网络、资源、单例），**没法单独跑起来**，逆它每行分支很绕。**巧解**：配表 lua 是自包含的结构化数据，只需复刻它读表的那一小段：

- 有 Lua 解释器 → `dofile` 配表拿到内存 table → 遍历 HeaderTable/DataTable；
- 没有也行 → **纯文本解析（正则）**抽字段元信息 + 数据行（本课走这条，零依赖）。

> "能复刻就复刻"的典型：目标是**拿到数据**，复刻它读表的那一小段就够，比逐分支跟那个 reader 省事得多。

### 第 3 步 · 导 CSV

`---@field` 注解 → CSV 前三行（字段名 / 类型 / 中文说明）；HeaderTable → 列序；DataTable → 数据行。按 HeaderTable 列序输出（`data` 常以主键为 key、非 1..n，别按 table 遍历顺序）。

### 第 4 步 · round-trip 字节验证（硬验收）

有原始 `Hero.csv`，就 `lua2csv(Hero.lua)` 还原出的 CSV 和原始**逐字节 diff**。本课实测 `Hero`(170B)、`Item`(110B) **都逐字节一致**——字段名、类型、中文说明、数值一个不差。不是"看着对"，是"字节相同"。

---

## 三、检查点

- 能把 keyed 形态的配表 lua 还原成 CSV，round-trip 验证字节一致。
- 能说清"复刻加载器 vs 硬逆加载器"，以及为什么前者省事。
- 见到 positional（`{index,data,vExt}`）形态，知道要先对 header、`vExt` 字符串池要展开。
- 知道按 HeaderTable 列序输出（主键 key ≠ 顺序）、浮点 / 字符串转义细节是 round-trip 对不上的常见原因。

---

## 动手

本课的动手分两块，**分开放**：

- **`正课/`** —— 走一遍参考解答。双击 `正课/lesson-4.3.exe`（纯文本解析，零 Lua 依赖）：看配表 `Hero.lua`（注解 + HeaderTable + DataTable）→ `lua2csv` 解析 `---@field` 注解拿字段/类型/中文说明 + 解析数据行 → 重建 CSV（Id/Name/Hp/Atk/Def/SkillId + 3 行英雄）→ 和原始 `Hero.csv` 逐字节 diff（一致）→ `Item` 表同样跑一遍（也一致）→ 弹文件夹。`正课/sample-output/` 放了配表 lua + 原始 CSV + 还原 CSV。
- **`作业/`** —— **自己动手 + 自动批改**。`作业/材料/weapon.lua` 是一份新的 keyed 带注解配表（**30 把武器**，字段行内乱序、主键 Id 非连续）：先复刻加载器读表把它还原（按 `---@field`/HeaderTable 列名对齐、遍历所有行），再从还原出的表里取 **X=Atk 最高武器的 Name（小写）** + **N=全表 Price 之和**，拼成 `FLAG{urev-4.3-<X>-<N>}` 交给 `作业/check.exe`——**列名对齐了、没漏行才算得对 🚩，错了提示常见坑**。flag 不在文件里明摆着，是从你还原对的数据里算出来的。任务详情见 `作业/README.md`。

> 建议：先做 `作业/`（真动手才学得会逆向），卡住了再翻 `正课/` 对答案。

> 真实表里字段常是**本地化 key**（`hero_name_001`），要顺带解本地化文件映射成人话（6.x）；配表 lua 若来自魔改 header 的 `.luac`，反编译可能残缺（先确认是完整的表，4.2）。

---

## 小结 & 下一章

你能把游戏成品里的配表 lua 反向还原成策划 CSV、round-trip 验证一致，拿到全套数值——逆向里性价比最高的产物之一。下一课 **4.4** 回到运行时：**Lua 动态**——注入代码 + hook Lua 函数，改逻辑 / 抓调用 / 验证还原。
