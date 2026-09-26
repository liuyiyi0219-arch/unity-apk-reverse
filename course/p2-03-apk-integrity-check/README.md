# 2.3　签名校验：游戏怎么发现你改了包

> 状态：🟢 已完成（正文 + 演示程序）｜演示程序：`正课/lesson-2.3.exe`

---

## 一、本章的目的

理解游戏运行时怎么**发现你改了包**——最主力的一招是**签名校验**。跟着走一遍，你会说清这套判断的完整链路（逆向时就按这条链去找它），并亲眼看到"改包重签 → 签名变 → 被发现"。

> 为什么改包一定会被发现：APK 必须签名才能装。你用 apktool 改完包（2.2），**拿不到开发者的原 key**，只能用自己的 key 重签——签名证书就变了（2.1 里你已经看到重签后指纹会变）。游戏就抓这一点：运行时**自查签名**、跟出厂时记下的值比，一变就知道被人动过。

---

## 二、操作指南

签名校验的完整链路——**游戏这么做，你逆向时就这么找它**：

### 第 1 步 · 取自己的签名证书

用 `PackageManager` 取自己的签名：

```java
getPackageInfo(pkg, GET_SIGNATURES)
```

> API 28+ 改用 `GET_SIGNING_CERTIFICATES` / `SigningInfo`，老设备回退 `GET_SIGNATURES`——分析新 / 老包时留意这个 API 差异。

### 第 2 步 · 算证书哈希

对取到的证书算哈希（`SHA-1` / `SHA-256`），常再 `Base64`。

### 第 3 步 · 和硬编码的期望值比对

和**包里硬编码的出厂期望值**比。我们靶子里这个值就是那把 debug key 的 `E2462490F7ED575E6F12A6AF2430B4CFEE370306`。

> 校验代码放哪：可能在 **java(dex)**，也可能在 **il2cpp 的 C#**——我们的靶就埋在 C# 的 [`SignatureCheck.cs`](../demo/CourseDemo/Assets/Scripts/Game/SignatureCheck.cs)（`GetSignatureSha1()` / `IsUntampered()` / `ExpectedSha1`）。期望值本身常被**字符串加密 / 藏进 native**，防你一眼定位。游戏常在**多处**校验，不止一个点。

### 第 4 步 · fail-closed

对不上、或取签名时异常，**一律当作被篡改**（宁可错杀）。

> 配套的自查还有：安装器来源（`getInstallerPackageName`，非商店安装可疑）、调试器 / 模拟器检测、文件 CRC 自校验等。

### 第 5 步 · 看它拦住重打包

把 2.2 重签过的包装到真机，游戏一跑就判定被改。真实靶 **VampireSurvivors** 里也埋了同一套 [`SignatureCheck`](../demo/VampireSurvivors/VampireSurvivorsClone/Assets/Scripts/Protection/SignatureCheck.cs)：原包启动 logcat 打 `untampered = True`；你 2.2 重打包它，重签后指纹变了，同一处就变 `False`。

---

## 三、检查点

- 能完整复述签名校验的四步（取签名 → 哈希 → 比对硬编码 → fail-closed）。
- 能解释"为什么 apktool 改包重签一定会被签名校验发现"。
- 知道校验代码可能藏在 dex 或 il2cpp C#、可能有多处、期望值可能被加密。

---

## 动手：运行演示程序

签名校验是运行时行为（要设备），但它的**判断逻辑能在电脑上模拟**。双击 **`正课/lesson-2.3.exe`**，它会：

1. 讲清校验的四步链路；
2. 把原 `CompleteDemo.apk` 当"官方包"，**现算它的签名指纹**（= 游戏硬编码的期望值）；
3. **模拟游戏的校验**，验两个包——
   - 原包 → 指纹一致 → **✔ 通过**；
   - `samples/CompleteDemo_repacked.apk`（事先用**另一把 key 重签**，模拟攻击者重打包）→ 指纹变了 → **✘ 检测到重打包**。

你会直观看到：**只要重签，指纹必变，这道校验就拦得住**。

---

## 小结 & 下一章

签名校验是改包路上第一道墙：**重签必被发现**。那有防守怎么办？下一课 **2.4**：**绕过签名校验**——不改签名（root + frida 运行时注入）/ patch 掉校验 / hook 取签名的接口。
