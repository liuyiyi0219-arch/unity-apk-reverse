# 3.4　C# 动态 hook

> 状态：🟢 已完成（正文 + 演示程序 + **真机实测通过**）｜演示程序：`正课/lesson-3.4.exe`｜hook 脚本：`正课/材料/hook_by_rva.js`、`正课/材料/hook_il2cpp_bridge.js`、`正课/材料/verify_attach.js`

---

## 一、本章的目的

3.1–3.3 是**静态**——把代码读懂。这一课进**动态**：程序**跑起来时**，用 Frida 挂到那些 C# 方法上，**读它的参数 / 返回值**、**改它的返回值**。你能：验证静态还原对不对（hook `CalcDamage` 打印实参，和你逆的公式对）、直接改逻辑（让 `License.IsActivated` 恒真、`CalcDamage` 返回爆炸伤害），全程**不改 APK 一个字节**。

> Frida 环境（心智模型、Win/Android 配置、通道）在 **[0.2](../p0-02-frida-setup/README.md)** 已调通，这一课只讲"怎么挂 C# 方法"。
>
> 静态之后为什么还要动态——三个静态给不了的：① **验证**（逆出的公式对不对，hook 一跑便知，静态×动态互证是最扎实的闭环）；② **改行为**（运行时改返回值即可，APK 没动、签名没变，2.3 的签名校验根本不触发）；③ **抓运行时才有的值**（解密后的字符串、内存里的 key，静态看不到，hook 一挂就现形——3.8/4.x 的脱壳、抓 key 都靠它）。

---

## 二、操作指南

### 第 1 步 · 定位方法（两条路线，日常首选 B）

每个 C# 方法编译成 `libil2cpp.so` 里**一个 native 函数**（3.1 讲过），地址就是那个 **RVA**。所以"hook C# 方法" = "hook 一个 native 函数"。

**路线 A · RVA 直定位**（简单，但脆）：
```js
const base = Process.findModuleByName('libil2cpp.so').base;
Interceptor.attach(base.add(0x600DB8), { /* CalcDamage */ });   // 模块基址 + RVA = 运行时地址
```
> 缺点：RVA 每次 build 都变，换包就得用 3.1 重取；读浮点参数 / 字符串很麻烦（第 2 步）。
>
> 重取 RVA 时按 3.1 用 **Il2CppDumper ≥ 6.7.x** 跑（靶包 metadata 是 version 31，v29+ 都走它；Il2CppInspector-2021.1 遇 v31 报 `not of a supported version (31)`）。跑通照样打 `WARNING: find JNI_OnLoad` / `ERROR: This file may be protected.` 两行——常规输出，有 `dump.cs` 即成。

**路线 B · 按名字定位**（[frida-il2cpp-bridge](https://github.com/vfsfitvnm/frida-il2cpp-bridge)，推荐）：
```js
const GameLogic = Il2Cpp.domain.assembly("Assembly-CSharp").image.class("GameLogic");
GameLogic.method("CalcDamage").implementation = function (atk, def, crit) { ... };
```
> 优点：不依赖 RVA（跨 build 稳）、自动处理值类型 / `string` / `MethodInfo*`、参数直接是可读的值。代价：多一个依赖、启动稍慢。**日常首选 B，快速一次性验证用 A。**

### 第 2 步 · 读参数（ARM64 调用约定的坑）

3.2 讲过参数被寄存器打乱，裸 hook 会实打实撞上。以 `CalcDamage(int atk, int def, float critMul)` 为例：

- **整型在整型寄存器**：`atk`→`x0`、`def`→`x1`，裸 `Interceptor` 里就是 `args[0]`、`args[1]`；
- **浮点在浮点寄存器**：`critMul`→`v0/s0`，**不在 `args[]` 里**！裸 hook 要读 `this.context.s0`；
- **末尾隐形 `MethodInfo*`**：签名之外还有一个，别数错位；**实例方法**：`args[0]` 是 `this`，真参数从 `args[1]` 起；
- **`string` 不是 C 字符串**：是 Il2CppString 对象（`+0x10` 存长度、`+0x14` 起 UTF-16）。

> 这些坑正是"路线 B 更省心"的原因：浮点、`string`、`this`、`MethodInfo*` 全被 bridge 封装好（`.content` 直接给字符串）。裸 `Interceptor` 适合"全整型参数、只看返回值"的方法（如 `IsActivated` 只改返回 bool）。

### 第 3 步 · 改返回 / 验证（对着埋点）

本文件夹两份现成脚本，针对 demo 的 [`GameLogic`](../demo/CourseDemo/Assets/Scripts/Game/GameLogic.cs) / [`License`](../demo/CourseDemo/Assets/Scripts/Game/License.cs)：

- **`正课/材料/hook_by_rva.js`**（路线 A）：用 3.1 的 RVA（`CalcDamage=0x600DB8`…）`Interceptor.attach`；`CalcDamage` 打印 `x0/x1`、`onLeave` 把返回 `ret.replace(99999)`；`IsActivated` `onLeave` `ret.replace(1)`——任何激活码都通过。
- **`正课/材料/hook_il2cpp_bridge.js`**（路线 B，推荐）：`Il2Cpp.perform` 按 `Assembly-CSharp → GameLogic/License → method` 定位；`CalcDamage` 先 `invoke` 原逻辑打印真实返回（验证静态还原），再 `return 99999`；`IsActivated` 打印 `code.content` 后 `return true`。换 build 也能用。

### 第 4 步 · 跑起来（attach 到游戏进程）

用 [0.2](../p0-02-frida-setup/README.md) 的稳妥通道：**先把游戏起到前台，再 attach**（Windows 上 spawn 常超时）：

```bash
adb forward tcp:27042 tcp:27042
adb shell monkey -p com.course.demo 1
frida -H 127.0.0.1:27042 -p $(adb shell pidof com.course.demo) -l 正课/材料/verify_attach.js
```

真机实测（Pixel 4，`GoldReward` 静态还原 = `10 + lv*lv*3`）：
```
[+] libil2cpp.so base = 0x7b13c0c000
[static] GoldReward(5)  = 85     期望 85     ✓ 静态还原正确
[static] GoldReward(10) = 310    期望 310    ✓
（装 replace hook 后）GoldReward(5) = 99999   ✓ hook 改返回值生效
```
**一次跑通同时证明两件事**：你 3.1–3.3 逆出来的公式**是对的**（返回值分毫不差），且 hook **能改**它——全程 APK 一字节没动。完整记录见 `sample-output-live.txt`。

> **一个 C# 特有的坑**：从 frida 线程直接 `NativeFunction` 调"会分配内存"的方法（如 `CalcDamage` 用了 `Mathf`）会崩——先 `il2cpp_thread_attach(il2cpp_domain_get())` 把线程注册进 runtime，或直接用 frida-il2cpp-bridge（它全帮你处理）；纯算术方法（如 `GoldReward`）无此限。想"观察游戏自己在启动时的调用"（Interceptor 看 `CalcDamage` 入参）要 spawn 抢在 `GameBoot.Start` 前——本机 spawn 超时，故用 attach + 自调 `GoldReward` 达到等效验证。

---

## 三、检查点

- 能说清"C# 方法 = native 函数"，并把一个 RVA 换成运行时 hook 地址（`模块基址 + RVA`）。
- 能用两条路线（RVA / bridge）各写一个 hook，读入参、改返回值。
- 能讲出裸 `Interceptor` 读浮点参数（`s0`）/ 字符串（Il2CppString `+0x10`）/ `MethodInfo*` 的坑，以及 bridge 为什么省心。
- 知道 frida 线程直接调"会分配内存"的方法要先 `il2cpp_thread_attach`（或用 bridge）。

---

## 动手：运行演示程序

双击 **`正课/lesson-3.4.exe`**：从 3.1 真 dump.cs 取三个方法的 RVA、讲"运行时基址 + RVA = hook 地址" → 把真实 RVA 填进 `正课/材料/hook_by_rva.js`（现场产出可用脚本）→ 讲 ARM64 参数坑（`critMul` 不在 `args[]`、`string` 是 Il2CppString、末尾 `MethodInfo*`）→ 两条路线对比（RVA vs bridge）→ 预期效果（入参被打印、伤害恒 99999、`IsActivated` 恒真）→ 弹文件夹，frida 就绪时照跑。

> **作业**（`作业/`）：材料 `il2cppflag34.apk` 是一个能点着玩的**爬塔回合制小游戏**（uGUI 按钮 `[攻击]`/`[重击]`，打穿 5 层 Boss 通关）。flag 是游戏里 `Vault34`（"开发者内测通关码金库"）**运行时 AES-128-CBC 解密**出来的——打穿 Boss 通关时 `Vault34.GetClearCode` 会内部调一次 `Validate(BuildFlag())`，flag 作为**入参**路过 `Validate`（`strings` 抠不到、界面也不显示）。hook `Vault34.Validate`（本次 `RVA: 0x591F88`）在 `onEnter` 读 `args[0]`，把塔打通关就拿到 flag，交给 `作业/check.exe <flag>`。详情见 `作业/README.md`。

---

## 小结 & 下一章

你打通了**静态 ↔ 动态**：3.1–3.3 把方法读懂、拿到 RVA / 签名，3.4 在运行时把它们挂起来验证、改写。C# 层的"读 + 改"技能到此完整。下一课 **3.5** 做一件 C# 层的高价值**应用**——从 il2cpp 生成代码还原 **FlatBuffers 协议（.fbs）**，之后进入 IL2CPP 的**保护与脱壳**主线（3.7–3.9）。
