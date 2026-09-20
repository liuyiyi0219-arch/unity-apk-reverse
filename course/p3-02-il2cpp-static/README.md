# 3.2　C# 读伪 C

> 状态：🟢 已完成（正文 + 正课演示 + 作业批改）｜`正课/lesson-3.2.exe`（RVA→伪 C 现场反编译）· `作业/check.exe`（读裸伪 C 算返回值 → flag）｜参考产物：`正课/sample-output/pseudo-c.txt`

---

## 一、本章的目的

3.1 的骨架给了每个方法的 **RVA**，但方法体是桩 `return default`。这一课拿着 RVA 进 **Ghidra**，在 `libil2cpp.so` 里把方法**真身**找出来、读它的**伪 C**——第一次看到"逻辑到底怎么算"（伤害公式、激活码算法现形）。汇编不用会，能读 C 就行。

---

## 二、操作指南

### 第 1 步 · RVA 换算成 Ghidra 地址

Ghidra 导入 so 时给它一个 **imageBase**（本 demo = `0x100000`），所以：

```
Ghidra 地址 = imageBase + RVA = 0x100000 + 0x600DB8 = 0x700DB8
```

> RVA（Relative Virtual Address）是相对加载基址的偏移，是 3.1 → 3.2 的接力棒。**忘加 imageBase** 直接跳 `0x600DB8` 会落到没映射的地方。

### 第 2 步 · 跳过去反编译读伪 C

跳到这个地址、按 **F5**（反编译）就能读伪 C。若显示"未定义"，先 `createFunction` / 按 `F`（本课的 `DecompileRVA.java` 已处理 `getFunctionContaining==null` 就建函数）。

> 有的工具（Il2CppDumper 的 `script.json` / Ghidra 脚本）能把方法名一次性灌进 Ghidra，函数就不叫 `FUN_00700db8` 而直接叫 `GameLogic$$CalcDamage`——这叫**符号注入**，3.3 用。本章先看**不注入**、纯裸的样子，把 RVA 定位本身吃透。

### 第 3 步 · 降噪：认出三个运行时套路，跳过

裸伪 C 里有些东西**不是**游戏逻辑，是 IL2CPP 运行时的固定套路，认得出就跳过：

1. **类初始化守卫**（开头那坨 `if`）：`if (DAT_xxx=='\0') { FUN_...(PTR_...); DAT_xxx='\x01'; }` 是 `il2cpp_runtime_class_init`，保证静态构造器跑过，**和业务无关，略过**。
2. **参数被寄存器重排**：ARM64 把浮点参数放浮点寄存器（`v0…`）、整型放整型寄存器（`x0…`），Ghidra 按寄存器顺序命名，于是 `CalcDamage(int atk,int def,float critMul)` 的 `critMul` 跑到了 `param_1`。**按类型 + 用法对，别按位置死认**。末尾还有个隐形 `MethodInfo*`（实例方法开头还有 `this`），用不上就没显示。
3. **string 不是 char\***：`*(int*)(str+0x10)` 是 IL2CPP string 对象的**长度字段**，取字符 / 拼接走 helper（`get_Chars` / `String.Concat`）。看到"对指针 +0x10 读 int 当循环上界"，基本是遍历字符串。

### 第 4 步 · 读出业务公式

降噪后，三个埋点的算法就裸奔了：

```c
// CalcDamage (RVA 0x600DB8)
param_1 = (((float)param_2*(float)param_2)/(float)(param_3+param_2)) * param_1;  // atk*atk/(atk+def)*critMul
iVar1 = (int)param_1;   // Mathf.FloorToInt

// GoldReward (RVA 0x600E4C)
return param_1 * param_1 * 3 + 10;      // level*level*3 + 10

// License.IsActivated (RVA 0x600E5C) —— 校验算法整个暴露
lVar3 = FUN_009df6e0(param_1, "COURSE-2026", 0);   // String.Concat(code, salt)
do { sVar4 = sVar4*0x1f + get_Chars(lVar3,iVar5); iVar5++; } while (iVar5 < *(int*)(lVar3+0x10));
return sVar4 == 0x4a3b;                  // 滚动哈希×31，和魔数 0x4A3B 比
```

> `License` 逆到这一步，你既能**爆破**出满足 `==0x4A3B` 的 code，也能直接 **patch** 掉那个比较让它恒真——这正是逆向的价值：把编译后看不见的校验摊在阳光下。

### 第 5 步 · 原始 vs 伪 C，看还原了多少

对照[你写的源码](../demo/CourseDemo/Assets/Scripts/Game/GameLogic.cs)（[License.cs](../demo/CourseDemo/Assets/Scripts/Game/License.cs)）：

| 方法 | 你写的（原始 C#） | 逆出来的（伪 C 核心行） | 还原度 |
|---|---|---|---|
| CalcDamage | `atk*atk/(atk+def)*critMul`，`Mathf.FloorToInt` | `(param_2*param_2)/(param_3+param_2)*param_1`；`(int)` | 公式**完全可读**，只是参数名没了、顺序被重排 |
| GoldReward | `10 + level*level*3` | `param_1*param_1*3 + 10` | **一模一样** |
| License.IsActivated | `sum=(sum*31+c)&0xFFFF; sum==0x4A3B` | `sVar4=sVar4*0x1f+c; sVar4==0x4a3b` | 算法**完全可读**（`&0xFFFF`→`short` 截断，salt 拼接→`String.Concat`） |

> 结论：伪 C 丢了名字（`FUN_`/`param_N`）、混入运行时噪声、参数顺序被打乱——但**真正的算法逻辑完整保留**。3.2 已经能让你**看懂**任何方法怎么算；3.3 再把它翻回带名字、可编译的真 C#。

> 有时那段伪 C 不是业务公式，而是一套**具名密码算法**（TEA / XXTEA / AES 这类）。认法是抓**特征魔数常量**——比如 XXTEA 的 `delta = 0x9E3779B9`、`6 + 52/n` 的轮数、`key[(p&3)^e]` 取子键——拿这个常量搜一下就知道算法名 + 标准实现，再对着伪 C 核对轮函数即可，不必逐行硬读。本章作业就藏着这么一个（第 **4.8** 章会正面讲 XXTEA 的解密）。

---

## 三、检查点

- 能把一个 RVA 换算成 Ghidra 地址（`imageBase + RVA`），跳过去反编译出伪 C。
- 能在裸伪 C 里认出三处运行时噪声（类初始化守卫、寄存器重排的参数、string +0x10 长度字段），并读出真正的业务公式。
- 能指着 `License.IsActivated` 的伪 C 说清激活码算法，并说出两条打法（爆破 / patch 比较）。

---

## 动手

本课的动手分两块，**分开放**：

- **`正课/`** —— 走一遍参考解答。双击 `正课/lesson-3.2.exe`：列出三个埋点的 RVA → 演示 `0x600DB8 → 0x700DB8` 换算 →（装了 Ghidra 可选现场 headless 反编译，约 5 分钟；不想等就用 `正课/sample-output/pseudo-c.txt` 预生成的）→ 三个方法逐个读伪 C、标出三处噪声 → 原始 vs 伪 C 并排 → 弹文件管理器。
- **`作业/`** —— **自己动手 + 自动批改**。材料 `il2cppflag32.apk` 是一个能点着玩的**爬塔回合制小游戏**（uGUI 按钮 `[攻击]`/`[重击]`/`[下一层]`，打穿 5 层 Boss 通关），但 flag 藏在游戏里 `RewardVault.GetClearCode` 这个"开发者内测通关码金库"方法的伪 C 逻辑里（不是明文，`strings` 抠不到；把塔玩通关也不显示它——只把它的长度打进 logcat）。dump 出骨架、按 RVA 定位 `RewardVault.GetClearCode`、读懂那段"内嵌编码字节 + XXTEA 解码循环"、照逻辑把 flag 还原出来，交给 `作业/check.exe <flag>`——**答对了吐 flag 🚩，答错给定向提示**。任务详情见 `作业/README.md`。

> 建议：先做 `作业/`（真读一遍伪 C 才学得会降噪），卡住了再翻 `正课/` 对答案。

> **为什么默认给样本**：headless 分析这份 12MB 的 `libil2cpp.so` 要约 275 秒（含 Decompiler / Stack / GCC 异常处理等 pass）再反编译。为了双击就能看效果，默认展示预生成的伪 C，想验真可选实况跑。大 so 首次分析几分钟属正常。

---

## 小结 & 下一章

你第一次读到了方法的**真实逻辑**——伤害公式、金币公式、激活码算法全在裸伪 C 里现形，但还是 `FUN_`/`param_N` 的匿名形态。下一课 **3.3**：把符号注入进去、补上类型，把这份伪 C **翻译成带名字、可编译的真 C#**，完成"骨架 → 真源码"的最后一步。
