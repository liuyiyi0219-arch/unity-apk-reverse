# 2.1　拿到 APK：下载源 + 验真

> 状态：🟢 已完成（正文 + 演示程序）｜演示程序：`正课/lesson-2.1.exe`

---

## 一、本章的目的

逆向第一步是拿到目标游戏**正确、未被篡改**的那个 APK。跟着做一遍，你会知道从哪几类源下载、各自取舍，以及拿到后怎么**验真**——形态 / 包名版本 / 签名指纹。

> 为什么"拿包"要单独讲：① 要**对的版本**(分析某版本、做版本对比得精确到那个 version)；② 要**没被重打包**(小站的包可能被塞广告、改签名，逆出来的不作数)；③ 要**完整**(现代包常拆成 base + 一堆 config 分包，少拿一块就缺 so 或资源)。

---

## 二、操作指南

### 第 1 步 · 选源下包（三类，按优先级）

- **官方渠道（最权威）**：官网 / 官方 CDN 直接给 APK 下载（国内渠道服、出海官网、预约站）。没经任何第三方重打包的一手包，能拿到就优先用，有时还能拿到内测 / PTR 包。
- **应用商店 · 设备侧（商店原包）**：真机上让 Google Play / Aurora Store（开源 Play 客户端，免账号）/ 厂商商店装好，再取安装包（含所有分包）——`adb pull` 连电脑取，或手机上用 **MT 管理器**从已装应用一键"提取安装包"。是商店原包、版本准。
- **Web 下载站（免设备，最省事）**：APKPure / APKCombo / APKMirror / Aptoide 网页或直链下。快、能选历史版本、可脚本化。

> 三方站个别包被二次打包，所以从这类源下的**务必走第 4 步验签名**。另外：部分游戏用 Google Play 的**应用完整性保护**，三方站的重打包 / 合并包常常装不上或装上闪退——遇到这种走 Play 原包 / MT 提取即可（这类完整性保护细节后面章节讲）。

### 第 2 步 · 认形态，split 要全解

单个 APK 直接用；**split / XAPK / .apks**（`base.apk` + `config.*.apk`）要**全解或合并**——`lib/` 和资源常被拆进 `config.arm64_v8a.apk`、`UnityDataAssetPack.apk` 等分包，全下全解才不缺 so / 资源。

### 第 3 步 · 读包名 + 版本

从 `AndroidManifest.xml` 读 `package` / `versionName`，确认是**对的游戏、对的版本**。

### 第 4 步 · 对签名指纹

签名证书的哈希 = **签名者身份指纹**。同一个人签的包，指纹一样；别人重打包、用自己的 key 重签，指纹就变了。跟官方对得上 = 一手包；对不上 = 被二次打包。

> 指纹从 v1 签名（`META-INF/*.RSA` 里的 X.509 证书）算得。只有 v2/v3 签名（无 v1）的新包，用 `apksigner verify --print-certs` 看真实证书。

### 第 5 步 · 装到真机

- **单个 APK**：`adb install game.apk`
- **split / XAPK / .apks（多个 apk）**：不能只装 base，要**一次装齐所有分包**：
  ```bash
  adb install-multiple base.apk split_config.arm64_v8a.apk split_config.xxhdpi.apk ...
  ```
  XAPK / .apks 本质是个 zip——先解压出里面所有 `.apk`，再 `install-multiple`。手机上更省事：用 **SAI（Split APKs Installer）** 或 **MT 管理器**，选中 XAPK / .apks 一键装。

> XAPK 有时带 **OBB 数据**：把里面的 `*.obb` 推到设备 `/sdcard/Android/obb/<包名>/`。另外某些包用 Google Play 完整性保护，要求"来自 Play"才肯跑，装时加 `-i com.android.vending` 伪装安装来源：`adb install-multiple -i com.android.vending base.apk split_*.apk`。

---

## 三、检查点

- 说得出三类下载源各自的优缺点，以及官方渠道为什么最稳。
- 能对一个下到的包：判断形态（别漏 split）、读出包名 / 版本、对比签名指纹。
- 会把单 APK / split / XAPK / .apks 装到真机（split 包用 `adb install-multiple` 装齐所有分包）。

---

## 动手：运行演示程序

下载要联网 / 真机没法在程序里替你下；但"**验真**"能做成工具。双击 **`正课/lesson-2.1.exe`**，它把几个 demo 包逐个验一遍：

- **有效性 / 形态 / 文件数**；
- **包名 + 版本**（自己解 `AndroidManifest.xml` 读出来）；
- **签名指纹**（从 v1 签名抠证书算哈希）——你会看到几个 demo 包**指纹相同**（都是我们同一把 key 签的）；到 **2.4** 你把某个包重打包重签，指纹就变。

它想让你记住的：**指纹一样 = 同一个签名者；对不上 = 被人重打包过**——这正是 2.3「签名校验」里游戏自查有没有被改的原理。

---

## 参考资料

**Web 下载站**：[APKPure](https://apkpure.com) · [APKCombo](https://apkcombo.com) · [APKMirror](https://www.apkmirror.com) · [Aptoide](https://en.aptoide.com)

**应用商店 · 设备侧**：[Google Play](https://play.google.com) · [Aurora Store（开源）](https://auroraoss.com)（[源码](https://gitlab.com/AuroraOSS/AuroraStore)）· [MT 管理器（手机上提取安装包）](https://mt2.cn)

**验真工具**：[apksigner · verify --print-certs（看真实签名证书）](https://developer.android.com/tools/apksigner)

> 下载站的可用性 / 域名常变，用前留意是否官方；能拿到官方渠道包就优先官方。

---

## 小结 & 下一章

你会挑源、会验真了。下一课 **2.2**：把拿到的包解开，取出各层产物。
