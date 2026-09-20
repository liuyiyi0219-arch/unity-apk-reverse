# 3.6　C# 热更 DLL

> 状态：🟢 已完成（正文 + 演示程序）｜演示程序：`正课/lesson-3.6.exe`｜参考产物：`正课/sample-output/`

---

## 一、本章的目的

搞清一件可能颠覆前几课的事：当游戏用 **C# 热更**（HybridCLR / ILRuntime）时，**核心游戏逻辑根本不在 `libil2cpp.so` / `global-metadata.dat` 里**——它在运行时加载的**热更 DLL**（标准 .NET 程序集）里。跟着做一遍，你能识别热更、找到热更 DLL、（必要时解密）、用 dnSpy / ilspycmd 反编译成**近乎源码级的干净 C#**。

> **工程动机**：iOS 禁止下发可执行原生代码，但游戏要频繁改数值 / 修 bug / 上活动，又不想每次强更 + 过审。于是把易变逻辑放进热更层，运行时从服务器 / 本地加载。il2cpp 是 AOT（本身不支持热更 C#），业界在它之上补出热更能力：**HybridCLR**（原 huatuo，主流，给 il2cpp 加完整 CLR 解释器）、**ILRuntime**（较老，纯 IL 解释器）。两者的热更 DLL 都是**标准 ECMA-335 托管程序集**。（注意：**xLua hotfix** 是用 Lua 补丁替换 C# 方法，属脚本层 Lua，4.x 讲，不是这里的 C# 热更。）

### 这对逆向意味着什么（关键）

你按 3.1 dump il2cpp，拿到的是 **AOT 部分**（引擎 + 框架 + 桥接 / 占位）。真正的战斗 / 数值 / 活动逻辑可能整个不在里面，或只剩空壳。所以逆向重心**转移**：从"逆 il2cpp"变成"**找到热更 DLL**"。**好消息**：热更 DLL 是托管 IL，dnSpy/ILSpy 直接反编译成近源码级干净 C#。

| | il2cpp AOT（3.1–3.3） | 热更 DLL（本章） |
|---|---|---|
| 形态 | native 机器码 + metadata | 标准 .NET 托管程序集 |
| 反编译 | Dumper + Ghidra + 翻译，伪 C 有损 | dnSpy/ILSpy 直接，**近源码** |
| 变量名 / 结构 | 丢 | **基本保留** |
| 难点 | 读懂 native | **找到** DLL（可能加密 / 运行时下载） |

> 有点反直觉：**热更游戏一旦拿到 DLL，反而是最好逆的**——难点从"读懂 native"变成"找到（可能被加密 / 下载的）DLL"。

---

## 二、操作指南

### 第 1 步 · 识别游戏是否用了 C# 热更

- **AOT dump 里业务逻辑缺失 / 空壳**：dump.cs 里找不到该有的战斗 / 数值类，或它们是空方法——**最强信号**。
- **libil2cpp 里有热更运行时痕迹**：字符串 `HybridCLR.RuntimeApi`、`LoadMetadataForAOTAssembly`、`ILRuntime` 等。
- **资源里有程序集**：`assets/` / `StreamingAssets/` 里有 `*.dll` / `*.dll.bytes` / 可疑 `*.bytes`，或 AB 里打包了程序集。
- **metadata magic 变了**：HybridCLR **商业版**会把 `global-metadata.dat`（il2cpp 的 **AOT 元数据**，不是热更 DLL 本身）加密，magic 从 `AF 1B B1 FA` 变 `DA FE 57 13`（`0x1357FEDA`）——那是加固，运行时 dump（3.8）。

### 第 2 步 · 找到热更 DLL

- **APK 内置**：`assets/` / `StreamingAssets/` 里的 `.bytes`，或打进 AssetBundle；解包 APK（2.2）就能拿到。
- **运行时下载**：首启从 CDN 拉（热更本意）——APK 里根本没有，需**设备首启（甚至登录 / 进战斗）后**去 dump 落地目录 / 抓包（这也是有些包"死活找不到逻辑"的原因）。

### 第 3 步 · 认出它、（必要时）解密

明文 .NET 程序集头是 `MZ`（`4D 5A`），往后不远有 metadata 签名 **`BSJB`**。头是 `MZ`+`BSJB` = 明文程序集，直接进第 4 步。头不是 → 被加密 / 压在 AB 里。**真实加密分布是这三档**：

1. **明文 / 直接运行时 dump（最常见）**：HybridCLR **不强制**加密热更 DLL。社区版常是明文 `.bytes`；即便商业版把 metadata 加密了，游戏一跑起来解密后的程序集就在内存里，用 `frida-il2cpp-bridge` 走标准 il2cpp API 一次 dump 全出。依据：`docs/research/hybridclr-bridge-test.md`（商业版样本 `com.allstarunion.lastfurry` 实测 105 assembly / 25,174 类全出，等同 Il2CppDumper，**完全绕开加密**）。
2. **商业版"加密虚拟机"（不是裸 XOR/AES）**：HybridCLR 商业版（Code Hardening）用自研加密 VM——per-build 随机、OLLVM 平坦化的字节置换密码，`vmSeed` 编进 native、`metadataSeed`/`key` 可随热更动态换。**静态几乎解不了**，务实解法只能运行时 dump。依据：官方 [hybridclr.cn `business/basicencryption`](https://www.hybridclr.cn/en/docs/business/basicencryption) + `docs/research/il2cpp-magic-0x1357FEDA-investigation.md`。
3. **AES 装进自加密 AssetBundle**：真实例 **vm3.global**（Century Games）——`assembly-csharp.dll` 等以 **CDPH-AES** 加密在 `game.unity3d` 里，IV `ac346728b005d60c195fc06860a02d52`，key 运行时派生、未能静态复原。依据：`.claude/skills/hybridclr-il2cpp-unpack/SKILL.md` + `il2cpp-extract` cache 的 `com.vm3.global` fail.md。

> **key 在哪（对比 4.8 的 Lua）**：和 Lua"找到一把 key 就通杀"不同，HybridCLR 热更 DLL **没有统一的一把 key**——社区版明文=无 key；商业版真正防线是 per-build 随机的加密 VM，光有 key 也解不了；AES-in-AB 那种 key 运行时派生、静态抠不出。**统一可靠的拿法是"游戏跑起来后运行时 dump"**，一把绕开所有加密——和 3.8 之后 il2cpp 脱壳同一条路。

### 第 4 步 · 反编译

明文 / dump 出的 DLL 用 `ilspycmd HotUpdate.dll` 或 dnSpy 打开 → **干净 C#**，直接读。同一个方法：il2cpp 要 Dumper+Ghidra+翻译（3.1–3.3 三课），热更 DLL **一步到源码级**。

> **托管 DLL 的字符串是 UTF-16LE**：想在反编译前先扫一眼里面的串，用 `strings -e l`（`-e l` 指定 UTF-16LE 编码）就能捞到明文；或者直接在 ILSpy / `ilspycmd` 的反编译结果里读——托管程序集反出来就是带完整字符串的干净 C#。

---

## 三、检查点

- 能说清"热更游戏的逻辑不在 il2cpp、在托管 DLL 里"，并列出识别信号（AOT dump 空壳、热更运行时字符串、资源里的程序集、metadata magic 变）。
- 能从一个 `.bytes` 认出 / 解出 .NET 程序集（`MZ`+`BSJB`），用 ilspycmd 反编译成 C#。
- 能讲出热更 DLL 加密的真实三档（明文/dump、商业版加密 VM、AES-in-AB），知道**统一可靠的拿法是运行时 dump**，别假设"单字节 XOR"。
- 不把 **DHE**（Differential Hybrid Execution，差分混合执行）当加密——它是 HybridCLR 的**执行模型**（改过的类走解释器、没改的走 AOT），跟加密无关；DLL 加不加密是商业版 Code Hardening 的独立开关。

---

## 动手：运行演示程序

demo 造了个真实的热更程序集 `HotUpdate.dll`（源码 [`正课/sample-output/hotupdate-src/HotUpdate.cs`](正课/sample-output/hotupdate-src/HotUpdate.cs)，`Course.Hotfix.HotfixLogic`：运营下发的新伤害公式 `CalcDamageV2` + 签到 `DailyReward`），**故意用最简单的单字节 XOR** 打包成 `HotUpdate.dll.bytes` 当"资源"。

> ⚠️ **demo 的 XOR 只是教学演示**——用来把"识别 → 解密 → 反编译"这条链讲清楚（一眼看懂、不用外部 key）；**它不代表真实分布**。真实热更 DLL 几乎没人用裸单字节 XOR，实际是上文三档：多为明文 / 运行时 dump，商业版是自研加密 VM（静态解不了），或 AES 装进加密 AB（如 vm3.global）。

双击 **`正课/lesson-3.6.exe`**：看资源（`HotUpdate.dll.bytes` 头 `17 00 CA 5A`，不是 `MZ`）→ 试 XOR `0x5A` → 头变回 `4D 5A`+后面 `BSJB` = 确认是 .NET 程序集 → `ilspycmd` 反成干净 C# → 和原始源码 `hotupdate-src/HotUpdate.cs` 并排（近乎一致）→ 对比 il2cpp（同方法 il2cpp 要三课、热更 DLL 一步到源码级）→ 弹文件夹。

---

## 作业

**`作业/`** —— 自己动手 + 自动批改。靶包 `材料/hotfixflag.apk` 是一个能玩的爬塔 mini-game（`com.course.hotfixflag`，装上真机点 [攻击]/[重击] 打怪爬塔）：战斗/UI 在主体 il2cpp 里，但主体只埋了个热更加载器 `HotfixBoot`，真正下发的逻辑和 **flag 在热更 DLL**（`assets/HotUpdate.dll.bytes`，标准 .NET 托管程序集）里——dump 主体 metadata 搜不到 flag。按面包屑（`HotUpdate.dll.bytes` / `Course.Hotfix.HotfixLogic`）从 APK 取出那份 DLL，用 dnSpy/ILSpy 反编译读出 flag，交给 `作业/check.exe FLAG{...}`。任务详情见 `作业/README.md`。

---

## 小结 & 下一章

你知道了：热更游戏的逻辑藏在**标准托管 DLL** 里，怎么识别、怎么找、怎么解、怎么读（而且读起来最轻松）。C# 层的"裸"技能到此全配齐：**3.1–3.3 AOT 还原 · 3.4 动态 hook · 3.5 协议还原 · 3.6 热更定位**。下一课 **3.7** 起进 IL2CPP 的**保护与脱壳**——metadata 加密、native 壳、反调试，把前面遇到的"加固"逐个拆掉。
