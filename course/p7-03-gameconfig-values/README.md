# 7.3　策划数值 type-tree

> 状态：🟢 已完成（正文 + 真 SO 靶，UnityPy 真跑）｜第七部分（资源 / AB）· 策划数值（和美术资源正交）

---

## 一、本章的目的

**策划数值和美术资源是两回事。** 前面 7.1–7.3 讲的是**美术资源**（贴图 / 音频 / 模型，打包在 AssetBundle 里，一键导出）；这一章讲**策划数值**——属性表、经济曲线、掉落率这些**策划调的数字**。它们存在 `ScriptableObject`/`MonoBehaviour` 里，**光用提资源的工具解不出来**，得补上 **type-tree**。这一章单独把它拎出来讲。

> **为什么单开一章**：美术资源"丢进工具点导出"就完了（7.1）；策划数值不行——直接提只拿到一个**空壳**（`{m_Name: "gameconfig"}`，零字段值）。要拿真数值，得知道那段字节的**布局**（哪几个字节是 `maxLevel`、是 int 还是 float…）。这张布局表叫 **type-tree**，它不在资源里、要从别处（il2cpp）弄来。这是和美术资源完全不同的一条路，所以单列。

---

## 二、为什么直接提是空壳

一个 `ScriptableObject` 序列化进资源，字节长这样（本章靶子——爬塔的 `GameConfig`——实测 116 字节）：

```
[ 标准头 44 字节 ] [ 自定义字段的原始字节 72 字节 ]
  m_GameObject / m_Enabled / m_Script / m_Name        towerFloors, monsterHp, ..., reward
  ↑ 每个 SO 都一样，工具天生认得                        ↑ 一段"没有说明书"的字节
```

标准工具（UnityPy / AssetStudio）**只认标准头**——读到 `m_Name` 就停：
```
naive read err: Expected to read 116 bytes, but only read 44 bytes
{ m_Name: "gameconfig" }        ← 空壳，零字段值
```

**数值就在后面那 72 字节里，缺的只是布局（type-tree）**：字段名 + 类型 + 顺序。工具不知道"偏移 44 往后 4 字节是 `towerFloors`、是 int"，就只能停在 `m_Name`。

> **为什么布局会没**：发行版（IL2CPP release）打包 / `DisableWriteTypeTree` 会**把自定义脚本的 type-tree 从资源里剥掉**（省体积）。Unity 内建类型（Texture 等）的布局工具内置、不受影响；只有**游戏自己定义的脚本类**受影响。

---

## 三、type-tree 从哪来

策划数值类的布局由**游戏的 C# 代码**定义。发行版把它从资源里剥了，但代码还在——从 **il2cpp** 里能生成回来（这一步接第三部分）：

- `libil2cpp.so` + `global-metadata.dat` 喂 **Il2CppDumper** → 出 **DummyDll**（带字段名 + 类型 + 顺序的空壳程序集）；
- 或喂 **TypeTreeGenerator** 直接生成 type-tree；
- 把这张布局喂给 UnityPy / AssetStudioMod，它就能把 SO 解全；**或**照布局直接按偏移解那段原始字节（本章作业走这条，最直观）。

> **一句话**：策划数值 = **原始字节（在资源里）+ type-tree（从 il2cpp 来）**。缺一不可——所以解策划数值**要带 APK**（il2cpp 在里面），不是光有资源包 / `csharp.zip`（反编译源码不是编译后 DLL，喂不了生成器）能给的。

---

## 四、操作指南

1. **提取 SO 的原始字节**：UnityPy 载入资源 / bundle，拿到那个 `MonoBehaviour` 对象的 raw data（`obj.get_raw_data()`）——naive read 会在 44 字节处停（空壳），但**原始字节全都在**。
2. **拿到 type-tree**：从 il2cpp 生成 DummyDll / TypeTreeGenerator（第三部分），得到字段布局（名 + 类型 + 顺序）。
3. **按布局解字节**：跳过标准头（到自定义字段起点），按字段类型顺序读——`int32`/`float32` 各 4 字节小端；`string` = int32 长度 + utf8 字节 + 补零对齐到 4。解出真数值。
4. **交叉验证**：解出的数值可以和反编译的 C#（第三部分）/ Lua 传参（第四部分）对照——同一个数三处呼应就稳了。

---

## 五、检查点

- 能说清**策划数值 ≠ 美术资源**：美术一键导出，策划数值直接提只有 `m_Name` 空壳。
- 能解释空壳的原因：自定义脚本类的 type-tree 被发行版剥掉了，剩一段"没说明书"的原始字节。
- 知道 type-tree 从 **il2cpp** 来（`libil2cpp.so`+`global-metadata.dat` → Il2CppDumper DummyDll / TypeTreeGenerator），所以解策划数值**要带 APK**。
- 会按布局把原始字节解成真数值（`int32`/`float32`/`string` 的读法 + 对齐）。

---

## 动手

- **作业（type-tree 解出 flag 数值）**：下载 `作业/材料/gameconfig-kit.zip`——`configpack` 是爬塔（Tower Climb）游戏的**策划数值配置**（一个 AssetBundle，**type-tree 被剥掉**、模拟发行版），里面一个 `GameConfig`（ScriptableObject：塔层数 / 顶层怪物 HP·ATK / 暴击倍率 / 通关语 / 通关奖励）；`typetree.txt` 是它的字段布局（模拟从 il2cpp 生成的那张说明书）。用 UnityPy 抽出 SO 的原始字节（naive read 只有 `m_Name` 空壳）→ 照 `typetree.txt` 的布局解那段字节 → 里面 `reward`（通关奖励）字段就是 flag。任务详情见 `作业/README.md`。

---

## 小结 & 下一部分

策划数值是和美术资源正交的一条线：数值以原始字节存在 SO 里，**要 type-tree（从 il2cpp 来）才解得出**，光提资源只是空壳。这也是"要拿真数值就得带 APK 一起解"的原因。**资源 / 数值层到此收尾**（7.1–7.3 美术资源 + 7.3 策划数值），下一部分是综合实战（第八部分），把前面所有层串成一条流水线。
