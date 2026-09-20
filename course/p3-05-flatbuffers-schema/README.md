# 3.5　C# 反推 FlatBuffers 协议

> 状态：🟢 已完成（正文 + 正课演示 + 作业批改）｜`正课/lesson-3.5.exe`（flatc round-trip 实证）· `作业/check.exe`（反推 .fbs → flatc --python → 用 .py 读 mail.bin → flag）｜参考产物：`正课/sample-output/`

---

## 一、本章的目的

游戏用 **FlatBuffers** 定义网络协议 / 数据文件，你手里只有 il2cpp dump、没有 `.fbs`。这一课把 `.fbs` **无损反推**回来——拿到每个字段的**名字 / 类型 / id / 默认值 / 顺序**，之后就能自己造包、解包、改协议。这是 3.1–3.3（读懂 C#）的一个高价值**应用**。

> **为什么 FlatBuffers 能"完美"反推**：开发者写一份 `.fbs`，用 flatc 编译成 C# 代码，游戏调生成的 `Create*` / 访问器收发包。而 flatc 生成的代码**把 schema 每条信息都焊死在方法里**（下表），il2cpp 又一个符号都不丢（3.1）——所以能逐字段无损重建、还能用 flatc 做字节级验证。

| schema 信息 | 焊在生成代码的哪 |
|---|---|
| 字段**名** | 访问器属性名 `Id/Name/Hp`、`Create` 参数名、`AddX` 方法名 |
| 字段**类型** | 访问器里的 `GetInt/GetUint/GetFloat/__string/…` + cast |
| 字段 **id / 顺序** | `__offset(N)` 常量、`AddX` 的 vtable 槽位 |
| **默认值** | 访问器三元 `o!=0 ? … : 默认`、`AddX` 第三参 |
| **root_type** | 哪个类型有 `GetRootAsX` / `FinishXBuffer` |
| **namespace / enum** | 生成代码的 `namespace`、独立的 enum 类型 |

> 和 protobuf（脚本层，4.5）对比：protobuf 的 wire 上只有 tag 号、没字段名，反推得连蒙带猜；FlatBuffers 生成代码**连字段名都在**，反推更彻底。

---

## 二、操作指南

### 第 1 步 · 在 dump.cs 圈出协议类型

搜 **`IFlatbufferObject`**（所有 FlatBuffers 表都实现它）、`FlatBufferBuilder`、`__offset`、`GetRootAs`，把游戏的**全部协议类型**一网打尽。

> `Google.FlatBuffers` 运行时（`Table`/`ByteBuffer`/`__offset` 的实现）也在 dump 里，那是**引擎侧**，不是协议，别混进来。dump 里没有 `//` 注释（编译丢了），但属性名 / 方法名 / `__offset` 常量 / 字面量默认值都在，线索齐全。

### 第 2 步 · 逐字段反推（对着生成代码）

一个 `BattleSync` 表的生成代码（= dump.cs 里的样子）：
```csharp
public struct BattleSync : IFlatbufferObject {
  public CmdType Cmd { get { int o = __p.__offset(4);  return o!=0 ? (CmdType)__p.bb.GetSbyte(o+__p.bb_pos) : CmdType.Sync; } }
  public uint Frame  { get { int o = __p.__offset(6);  return o!=0 ? __p.bb.GetUint(o+__p.bb_pos) : (uint)0; } }
  public PlayerState? Players(int j) { int o = __p.__offset(8); return o!=0 ? …__indirect(__p.__vector(o)+j*4)… : null; }
  public long Seed   { get { int o = __p.__offset(10); return o!=0 ? __p.bb.GetLong(o+__p.bb_pos) : (long)0; } }
  public static void AddCmd(FlatBufferBuilder b, CmdType cmd) { b.AddSbyte(0, (sbyte)cmd, 3); }   // 槽位0，默认3
}
```

逐条抠：
1. **字段名** = 访问器属性名 / `Create` 参数名 / `AddX` 方法名（il2cpp 里是 `get_Cmd`，去掉 `get_`）。
2. **字段 id / 顺序**：`__offset(N)` → `id=(N-4)/2`（`4→id0`、`6→id1`…）；或读 `AddCmd(){AddSbyte(0,…)}` 的**第一个参数** = 槽位 = 字段序。
3. **类型**：看 `Get*`/cast——`GetInt→int`、`GetUint→uint`、`GetLong→long`、`GetFloat→float`、`GetSbyte→sbyte`、`Get→bool`、`__string→string`；`(CmdType)…GetSbyte`→**enum**；`__indirect+__assign`→**嵌套 table**；`__vector+__indirect`→**vector of table**，`__vector` 不带 `__indirect`→vector of 标量。`string`/`vector`/`table` 都是**间接 offset**，不是内联标量。
4. **默认值**：以 `AddX` **第三个参数**为准（写入路径真正用的默认，`AddSbyte(0,(sbyte)cmd,3)`→默认 `3=Sync`）；访问器三元的 `: 默认` 是读路径。
5. **root_type / enum / namespace**：有 `GetRootAsBattleSync` 的 → `root_type BattleSync;`；独立 `enum CmdType : sbyte {…}` 名字/值/底层类型全在；生成代码 `namespace Course.Net` = `.fbs` 的 namespace。

### 第 3 步 · 拼成 .fbs

逐字段填一遍：
```fbs
table BattleSync {
  cmd:CmdType = Sync;    // id0, GetSbyte+cast, 默认 3
  frame:uint;            // id1, GetUint
  players:[PlayerState]; // id2, __vector+__indirect
  seed:long;             // id3, GetLong
}
```
`PlayerState` 同理（`正课/sample-output/generated-original/` 有完整生成代码）。合起来就是 `net.reconstructed.fbs`。

### 第 4 步 · flatc round-trip 字节验证（最硬的验收）

"看着像"不算数。验收标准：**反推出的 `.fbs` 用 flatc 重新生成的 C#，和原始生成代码逐字节一致**：

```bash
flatc --csharp -o gen_original     net.fbs               # 原始（= dump 里的 specimen）
flatc --csharp -o gen_roundtrip    net.reconstructed.fbs # 反推产物再生成
diff -r gen_original gen_roundtrip                        # 空 = 完全一致
```

本课实跑：**diff 为空**——反推的 `.fbs` 和原始 schema 在 wire 格式 + API 上**完全等价**。比对 protobuf 更强：连生成代码都逐字节相同。

### 第 5 步 · 拿 .fbs 生成任意语言的类，写自己的工具

`.fbs` 是**语言中立**的——反推回来后，你不必绑在游戏的 C# 上，用 flatc 生成**顺手语言**的类，处理协议：

```bash
flatc --python net.fbs      # 生成 Python 类
flatc --cpp    net.fbs      # C++
flatc --go     net.fbs      # Go
flatc --rust   net.fbs      # Rust
```

有了这些类，就能用你喜欢的语言**造包 / 解包 / 改字段**——写抓包器、MITM 代理、协议模糊测试、脚本 bot 都行。这才是反推 `.fbs` 的真正价值：**从游戏的一份 C# schema，拿到一把能在任何语言里收发这游戏协议的钥匙**。

---

## 三、检查点

- 能在 dump.cs 里靠 `IFlatbufferObject`/`__offset` 圈出全部协议类型（排除 `Google.FlatBuffers` 引擎侧）。
- 能从一段生成代码认出字段**名/类型/id/默认值**，写出可编译的 `.fbs`（`id=(N-4)/2`、默认以 `AddX` 第三参为准、`string`/`vector`/`table` 是间接 offset）。
- 能用 flatc round-trip **字节验证**反推正确，而不是"看着像"。
- 知道 `.fbs` 语言中立：`flatc --python/--cpp/--go/--rust` 能生成任意语言的类，用顺手的语言写自己的协议工具（造包 / 解包 / 抓包 / bot）。

---

## 动手

本课的动手分两块，**分开放**：

- **`正课/`** —— 走一遍参考解答。双击 `正课/lesson-3.5.exe`（需装 flatc，见 [0.1](../p0-01-env-setup/README.md)）：看答案 `net.fbs` → flatc 真生成 C#（specimen = dump.cs 的样子）→ 对着 `BattleSync`/`PlayerState` 逐条抠名字/id/类型/默认值 → 看只凭生成代码重建的 `net.reconstructed.fbs` → 现场 `flatc` 两份 → `diff` 证明逐字节一致 → 弹文件夹对比。
- **`作业/`** —— **自己动手 + 自动批改**（AI + 数据）。下载 `作业/材料/mail-kit.zip`：里面 `MailMsg_generated.cs` 是 flatc 从一份你看不到的 `.fbs` 生成的 C#（邮件消息表 `MailMsg`），`mail.bin` 是用那份 schema 序列化的一段真实数据。三步：① 用 AI 把 `.fbs` 反推出来；② `flatc --python` 编成 `MailMsg.py`；③ 上传这个 `.py`。批改器**用你的 `.py` 去读 `mail.bin`**，5 个字段全读对 → 吐 flag 🚩，读串了告诉你差在哪。schema 错一个字段（顺序/类型/名字），同一段字节就读不出正确值——这就是最硬的验证。任务详情见 `作业/README.md`。

> 建议：先做 `作业/`（真动手才学得会反推），卡住了再翻 `正课/` 对答案。

> 遇到 FlatBuffers 的 **`struct`**（定长内联，无 `__offset`、无默认、不可选）生成代码形态不同，按定长布局反推；本课两处 demo 全是 `table`。（`正课/sample-output/` 有两份 `.fbs` + 两套生成代码，不想跑也能直接看。）

---

## 小结 & 下一章

你把一个 FlatBuffers 网络协议从 il2cpp 生成代码**无损反推**回 `.fbs`，并用 flatc **字节验证**了正确性——从此能造包、解包、改协议。C# 层的"读 / 改 / 应用"技能到此配齐。下一课 **3.6** 讲 **C# 热更（HybridCLR / ILRuntime）**——你 dump 出的 C# 里可能**根本没有游戏逻辑**，因为它在运行时加载的热更 DLL 里。
