# 3.1　C# 出骨架

> 状态：🟢 已完成（正文 + 演示程序）｜演示程序：`正课/lesson-3.1.exe`｜参考产物：`正课/sample-output/`

---

## 一、本章的目的

用 **Il2CppDumper** 从明文 `libil2cpp.so` + `global-metadata.dat` 反出**骨架**：两份产物——`dump.cs`（一整份类型 / 方法 / RVA 清单）和**按模块拆好的 C#**（DummyDll → ilspycmd）。跟着做一遍，你会拿到全部类型的签名 + 地址，并看清"骨架"是什么、不是什么。

> 本章用明文的 `CompleteDemo`。真实靶 `VampireSurvivors` 的 metadata 是加密的（脱它是 3.7 / 3.8），这里先在明文包上把骨架流程走通。

---

## 二、操作指南

### 第 1 步 · 抽两个文件

从 `CompleteDemo.apk` 里解出 **`libil2cpp.so`** + **`global-metadata.dat`**（`assets/bin/Data/Managed/Metadata/`）——逆 il2cpp 必须两者齐。

> 为什么两个：`libil2cpp.so` 是编译后的机器码，`global-metadata.dat` 存每个类叫什么、有哪些方法 / 字段 / 字符串。Il2CppDumper 把两者一拼才反得出可读签名（1.1 讲过）。

### 第 2 步 · 跑 Il2CppDumper

```bash
Il2CppDumper.exe libil2cpp.so global-metadata.dat out
```

它先在 so 里定位 CodeRegistration / MetadataRegistration（代码与元数据的"总入口"），再对着 metadata 还原每个类型 / 方法，产出 **`dump.cs`** + **`DummyDll/`** + `script.json`。

> 跑不出来时对症处理：**报 "not valid metadata"** = metadata 被加密（是壳，走 3.7），不是本章明文流程；**版本对不上**（老 Dumper 不认 v31 / Unity 2022.3+）→ 用支持该版本的构建（本课的支持到 v39）；**导 DummyDll 那步 OOM**（10 万+方法的大包）→ 换大内存机；**CR 找到但 MR=0** → 换 `Il2CppInspector.CLI --dll-out`。

> **靶包 metadata 是 version 31（Unity 2021.x）**：认版本用 **Il2CppDumper ≥ 6.7.x**（本课 fork 支持到 v39），metadata v29 及以上都走它。Il2CppInspector-2021.1 遇 v31 会报 `not of a supported version (31)`，换 Il2CppDumper 即可解。
>
> Il2CppDumper 跑通时照样会打 `WARNING: find JNI_OnLoad` 和 `ERROR: This file may be protected.` 两行——这是它的常规输出；判成功以产物为准，`dump.cs` + `DummyDll/` 正常产出即成。

### 第 3 步 · 读 dump.cs，定位埋点 + 拿 RVA

在 `dump.cs`（本 demo 10 万+行）里搜 `GameLogic`，看到签名 + **RVA**：

```csharp
public static class GameLogic // TypeDefIndex: 1397
{
    // RVA: 0x600DB8 Offset: 0x5FCDB8 VA: 0x600DB8
    public static int CalcDamage(int atk, int def, float critMul) { }
}
```

> `Assembly-CSharp.dll` 里的就是游戏自己写的 C#（`UnityEngine.*` 是引擎）——所以 `GameLogic / License / GameConfig / SignatureCheck / GameBoot` 一眼能从引擎堆里挑出来，这就是 1.1 "认出 game vs engine" 落到实处。

### 第 4 步 · 拆成按类型的 C#

```bash
ilspycmd -p Assembly-CSharp.dll -o csharp_out
```

把 DummyDll 反成**一个类一个 `.cs`**，每方法带 `[Address(RVA=...)]`，按类型浏览比在 10 万行里搜更顺手。

### 第 5 步 · 原始 vs 还原，看骨架还原了多少

靶子是自己写的，把[**你写的源码**](../demo/CourseDemo/Assets/Scripts/Game/GameLogic.cs)和还原产物摆一起：

```csharp
// 你写的（有真逻辑）
public static int CalcDamage(int atk, int def, float critMul) {
    float baseDmg = (float)atk * atk / (atk + def);
    return Mathf.FloorToInt(baseDmg * critMul);
}
// 逆出来的（只剩壳）
[Address(RVA = "0x600DB8", Offset = "0x5FCDB8", VA = "0x600DB8")]
public static int CalcDamage(int atk, int def, float critMul) { return default(int); }
```

| 维度 | 还原情况 |
|---|---|
| 类名 / 方法名 / 参数 / 返回值 / 类结构 | **完全一致** ✓（这些在 metadata 里明文存着） |
| 方法体（真正的算法） | **丢了** → 桩 `return default`（metadata 不存逻辑，逻辑在 native 机器码里） |
| RVA 地址 | **多出来的线索** → 告诉你这段逻辑在 `libil2cpp.so` 的哪个偏移 |

> 一句话：**骨架 = 忠实的"外壳" + 逻辑的"藏宝图（RVA）"，但没有"内容"**。够你**定位**任何方法；要**看懂它怎么算**，顺 RVA 去 native 读（3.2）、再翻回真 C#（3.3）。

---

## 三、检查点

- 能跑出 `dump.cs`，并在里面定位埋点类和它的 **RVA**。
- 能把 `DummyDll` 反编译成按类型拆好的 C#，说清"这是骨架（签名 + RVA），不是方法体"。
- 能指着 `CalcDamage` 说清：签名 / 类结构完全还原、方法体丢了、RVA 是去 native 找逻辑的线索。

---

## 动手：运行演示程序

双击 **`正课/lesson-3.1.exe`**（需先装 Il2CppDumper + ilspycmd，见 [0.1](../p0-01-env-setup/README.md)），它一步步真跑：抽两个文件 → 跑 Il2CppDumper 出 `dump.cs`/`DummyDll` → 在 dump.cs 里定位 `GameLogic` 看 RVA → `ilspycmd` 拆成按类型 C# → 原始 vs 还原并排对比 → 弹文件管理器摊开两份产物。

（`正课/sample-output/` 放了现成的 `dump.cs` + 拆好的 C#，不想跑也能直接看。）

> **作业**（`作业/`）：材料 `il2cppflag.apk` 是一个能点着玩的**爬塔回合制小游戏**（uGUI 按钮 `[攻击]`/`[重击]`，打穿 5 层 Boss 通关）。flag 是游戏里 `Vault31`（"开发者内测通关码金库"）的一个**明文 string 字面量**字段——玩通关也不显示（只把它的长度打进 logcat）。dump 出骨架、在 `dump.cs` 里定位 `Vault31`、去 `stringliteral.json`（或 `strings global-metadata.dat | grep FLAG`）取出那个明文串，交给 `作业/check.exe <flag>`。详情见 `作业/README.md`。

---

## 小结 & 下一章

你拿到骨架了：类型 / 方法签名 + RVA + 按模块拆好的 C#，但方法体还是桩。下一课 **3.2**：拿着 RVA 进 Ghidra，把方法在 native 里找到、读它的伪 C。
