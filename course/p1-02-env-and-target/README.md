# 1.2　认识靶子 + 工具链

> 状态：🟢 已完成（正文 + 演示程序）｜靶子：`demo/build/CompleteDemo.apk`｜演示程序：`正课/lesson-1.2.exe`

---

## 一、本章的目的

认识全课要逆的那个**完整靶子** `CompleteDemo.apk`——它比 1.1 的裸包多了什么、这些东西各归后面哪一章逆；顺带把会用到的**工具链**过一眼混个脸熟。

> 靶子我们已经做好、当产物给你——**不用装 Unity、不用自己 build**。它怎么造的属于工程侧，不占你的学习路径。

---

## 二、操作指南

先在真机上把它当普通游戏玩一遍，再拆开看结构。

### 第 1 步 · 装上真机，跑一遍看它活着

`CompleteDemo.apk` 是个**能装能跑的完整 Unity 游戏**。先用 0.0 配好的 adb 装上、启动，看它真的在跑：

```bash
adb install -r ../demo/build/CompleteDemo.apk
adb shell monkey -p com.course.demo -c android.intent.category.LAUNCHER 1   # 启动
adb logcat -s GameBoot Unity                                                # 另开一个终端看输出
```

这个 demo 没有花哨的画面，它的"游戏内容"直接打进 **logcat**。你会看到：

```
GameBoot: [GameBoot] signature sha1 = ...
GameBoot: [GameBoot] untampered = True
GameBoot: [GameBoot] demo damage = 107, gold = 85
GameBoot: [GameBoot] license(FREE-TRIAL) = False
Unity  : [Main] Knight (atk=180) hits Mage (def=30) for 231 damage
```

> **记住这几个数**——C# 层伤害 `107`、金币 `85`、license 判定，Lua 层战斗结算 `231`，全是游戏跑起来算出来的。**后面几章要做的，就是不运行游戏、只从二进制 / 加密脚本里把这些值和它们的公式还原出来**，再回到这里对答案。C# 埋点由 `GameBoot` tag 打（用 `GameConfig` 的 100/40），Lua 战斗由 Unity tag 打（用 Hero 配表的 180/30）——**同一个伤害公式、两层各跑一次**，两层都在运行。
>
> 顺带你会注意到：签名正常时 `untampered = True`、照常进游戏；等 2.4 你重打包后再装，这里会变 `False`、弹 Toast"检测到应用被篡改"并退出——那是签名校验在拦。

### 第 2 步 · 并排解开两个包，看完整靶多了什么

把 `BareDemo.apk`（裸包）和 `CompleteDemo.apk`（完整靶）都解开，对比：

| | 裸包 BareDemo | 完整靶 CompleteDemo |
|---|---|---|
| `libil2cpp.so`（C# 代码） | 8.5 MB | **11.7 MB**（多了真实游戏逻辑） |
| 脚本层 `assets/gamelua.ab` | 没有 | **有**（加密打包的 Lua） |
| 游戏自己的类 | 只有 `HelloWorld` | `GameLogic` / `License` / `GameConfig` / … |
| 保护 | 无 | 签名校验、Lua 加密 |

> 一句话：完整靶 = 裸包 + 真实 C# 逻辑 + 脚本层（加密 Lua）+ 数值 + 保护。

### 第 3 步 · 认埋点清单（每层埋了什么、归哪章逆）

每一层都放了"看得见的靶点"，后面对应章节逐个逆。**这些靶点在 demo 工程里都有源码**——逆向的目标，就是从编译 / 加密后的产物把它们还原出来。先翻一眼源码"答案"，逆到时好对照：

| 层 | 埋了什么（源码） | 归哪章逆 |
|---|---|---|
| **C#** | [`GameLogic.cs`](../demo/CourseDemo/Assets/Scripts/Game/GameLogic.cs)（伤害 / 金币公式）、[`License.cs`](../demo/CourseDemo/Assets/Scripts/Game/License.cs)（激活码校验）、[`GameConfig.cs`](../demo/CourseDemo/Assets/Scripts/Game/GameConfig.cs)（数值定义） | 3.1–3.4 还原 + hook |
| **脚本层 Lua** | [`Battle.lua`](../demo/CourseDemo/Assets/GameLua/src/Gameplay/Battle.lua.bytes)（战斗逻辑）+ [`Hero`](../demo/CourseDemo/Assets/GameLua/config/Hero.lua.bytes) / [`Item`](../demo/CourseDemo/Assets/GameLua/config/Item.lua.bytes) 配表（ToLua，打进加密 AB） | 4.1–4.8 |
| **资源** | [`GameConfig.asset`](../demo/CourseDemo/Assets/GameData/GameConfig.asset)（带数值的 ScriptableObject） | 6.1 |

> 完整工程见 [`demo/CourseDemo/`](../demo/CourseDemo/)（C# 埋点在 `Assets/Scripts/Game/`，Lua 在 `Assets/GameLua/`）。签名校验的源码是 [`SignatureCheck.cs`](../demo/CourseDemo/Assets/Scripts/Game/SignatureCheck.cs)，游戏入口 [`GameBoot.cs`](../demo/CourseDemo/Assets/Scripts/Game/GameBoot.cs) 把这些串起来。
>
> 还有两个"应用级"靶点——**FlatBuffers 数据协议**（3.5）和 **Lua 层 protobuf 网络协议**（4.5）——写到那两章时再补进靶子。

### 第 4 步 · 认保护清单（叠了什么、现在有没有）

真实游戏不会让你轻松逆。完整靶的保护**分两批上**：现在就有的（能自写的），和到各自章节才叠的（需要壳的）：

| 保护 | 手法 | 现状 | 在哪讲 |
|---|---|---|---|
| **签名校验** | `PackageManager` 取签名 hash 比对（[`SignatureCheck.cs`](../demo/CourseDemo/Assets/Scripts/Game/SignatureCheck.cs)） | ✅ 已有 | 2.3 / 2.4 |
| **Lua 加密** | 打成加密 AssetBundle（`gamelua.ab`） | ✅ 已有 | 4.6–4.8 |
| AB 部分加密 | 自造：固定加密前 16KB | 到 6.2 叠 | 6.2 / 6.3 |
| metadata 加密 | 开源手法（XOR / AES） | 到 3.7 叠 | 3.7 / 3.8 |
| native 壳 | 开源 ELF packer | 到 3.9 叠 | 3.9 |

> 所以现在解开完整靶，只会看到签名校验 + Lua 加密两样，其余到对应章节才叠上——这是刻意的，方便一步步来。全课**只造教学等效手法，不复刻任何商业方案**（如某厂的 blockinfo AB 加密）。

### 第 5 步 · 把工具链过一眼（先混脸熟，用到再细讲）

| 工具 | 干什么 | 归哪章 |
|---|---|---|
| **Il2CppDumper** | 从 `libil2cpp.so` + metadata 出类 / 方法骨架（`dump.cs`） | 3.1 |
| **Ghidra** | 看 native 汇编 / 伪 C | 3.2 |
| **Frida** | 运行时 hook、抓参改值、dump | 3.4 起 |
| **AssetStudio / UnityPy** | 提资源、导数值 | 6.1 |
| **unluac** | Lua 字节码还原源码 | 4.2 |
| **SoFixer** | 把内存 dump 出的 `.so` 修好能加载 | 3.9 |
| **flatc / protoc** | 编译 / 反推序列化协议 | 3.5 / 4.5 |
| **zipalign / apksigner** | 重打包对齐 + 重签名 | 2.4 |

> 这些的官方下载 / 安装见 [0.1](../p0-01-env-setup/README.md)。课程自带的那批演示程序直接调，不用你装。

---

## 三、检查点

- 能把 `CompleteDemo.apk` 装上真机跑起来，在 `adb logcat -s GameBoot Unity` 里看到伤害 `107` / 金币 `85` / license 判定 / Lua 战斗 `231` 的输出。
- 能说出完整靶比裸包多了哪三类东西（真实 C# 逻辑 / 脚本层加密 Lua / 保护）。
- 能对着这几个埋点（`GameLogic` / `License` / `gamelua.ab` / `GameConfig` / 签名校验），说出各归后面哪一章逆。
- 知道保护是分批叠的：现在只有签名校验 + Lua 加密，其余到 3.7–3.9、6.2 才上。

---

## 动手：运行演示程序

双击 **`正课/lesson-1.2.exe`**，它会：

- 把裸包和完整靶**都解压**、弹文件管理器让你并排看；
- 现场对比：完整靶的 `libil2cpp.so` 大了多少、多出来的 `gamelua.ab`、多出来的游戏类（在两个包的 metadata 里各数一遍）；
- 把 `gamelua.ab` 的头 8 字节 **XOR 0x5A 解一下**，让你看到它变回 `UnityFS`——"哦，原来 Lua 是被加密的"；
- 最后把这些"多出来的东西"映射到后面各章。

一直按回车看完即可（结尾会问要不要清理解压的文件）。

> **demo 策略**（贯穿全课）：一个**完整靶**（本课这个）埋点 + 全套保护，最后 7.1 综合解穿；此外**每章配一个最小 demo**，讲某个技法时单拉一个最小靶子只演示那一件事（比如 AB 加密就单拉一个 AB、只加密 16KB、演示解密），干净可复现。完整靶是"最终 boss"，中间各章先用更小的靶把单个技法讲清。

---

## 小结 & 下一章

你认识了全课的靶子：**完整靶 = 裸包 + 真实 C# 逻辑 + 脚本层(加密 Lua) + 数值 + 保护**，也知道每块归哪章逆。下一课 **2.1**：真实逆向的第一步——怎么拿到一个游戏的 APK。
