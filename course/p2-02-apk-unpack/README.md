# 2.2　重打包：解包 → 改 → 打回去能跑

> 状态：🟢 已完成（正文 + 演示程序）｜演示程序：`正课/lesson-2.2.exe`（用课程自带 Java + apktool + zipalign + apksigner，零安装）

---

## 一、本章的目的

把一个 APK 解开、改一处、再**重新打包成能装能跑的 APK**——这是"改包"的完整链路：**解包 → 改 → 回编译 → 对齐 → 签名 → 装上验证**。

跟着做一遍，你会亲手把一个 demo 包的名字改成 `HACKED!`、打回去、装到真机上看到它真的变了。这条链路是后面很多动手环节的基本功（改个值验证你理解对不对，是逆向里最直接的反馈）。

> 为什么不用 unzip 直接改：`AndroidManifest.xml` 是编译过的二进制 AXML、`resources.arsc` 和界面也是二进制，unzip 出来是一堆字节**读不了改不了**；就算改了字节压回 zip，签名坏了、没对齐，**装不上也跑不起来**。所以 unzip 只用来**看结构**（1.1），改包得用 **apktool** 这条链。

---

## 二、操作指南

对一个 demo 包（如 `CompleteDemo.apk`）走一遍。课程 `tools/` 里 Java / apktool / zipalign / apksigner 都塞好了，**零安装**。下面命令里的 `apktool` / `zipalign` / `apksigner` 是简写，实际按课程自带工具这样调（或把 `tools/`、`tools/buildtools/` 加进 PATH 再用简写）：

> - `apktool …` → `java -jar tools/apktool.jar …`
> - `zipalign …` → `tools/buildtools/zipalign.exe …`
> - `apksigner …` → `java -jar tools/buildtools/apksigner.jar …`（keystore 在 `tools/buildtools/course.keystore`）

### 第 1 步 · apktool 解包

`apktool d` 把二进制反编译成**可读可改**的形态：

```bash
apktool d CompleteDemo.apk -o work
```

- `AndroidManifest.xml` → 可读 XML（能改包名、权限、入口、app 名）
- `res/` → 资源解成可读文件（含 `strings.xml` 里的 app 名）
- `classes.dex` → 反汇编成 **smali**（java 层代码，可改）
- `lib/`、`assets/` → 原样拷出（`libil2cpp.so` / `global-metadata.dat` / `gamelua.ab` 都在这）

> 对 Unity IL2CPP 游戏，游戏主料（native so、metadata、lua AB）apktool 是**原样拷**、不反编译——改它们是 3.x / 4.x 的活。apktool 在本章的价值是"改 manifest / 资源 / smali + 打回去能跑"这条链。

### 第 2 步 · 改一处

在解出的**可读**文件里改。最直观的是 app 名——改 `res/values/strings.xml`（或 manifest 的 `android:label` 指向的字符串）里的 app 名为 `HACKED!`。

> 也可以改 smali（java 层逻辑）、改资源、改 manifest 权限。改 native / metadata / lua 不在这一步（那几层 apktool 只原样拷）。

### 第 3 步 · 回编译

```bash
apktool b work -o CompleteDemo_repacked.apk
```

> 想往 APK 里**手动塞一个文件**（如作业里塞 `change.txt`）：用 `apktool` 解包加文件再打回，或 python `zipfile` 直接写入（`ZipFile(apk,"a").write("change.txt")`）——两条都能把文件写进 APK。

### 第 4 步 · 4 字节对齐

```bash
zipalign -p -f 4 CompleteDemo_repacked.apk CompleteDemo_aligned.apk
```

> `.so` 要页对齐，否则新系统装不上 / 跑不稳。

### 第 5 步 · 重签名

开发者原 key 拿不到，用**自己的 key** 重签（开 v2/v3，老系统才认 v1）：

```bash
apksigner sign --ks course.keystore --ks-pass pass:course123 CompleteDemo_aligned.apk
```

> 重签用的是你自己的 key，**签名指纹就变了**——游戏若有签名校验，一跑就发现被改（这正是 2.3 / 2.4 的主题）。
>
> demo 靶包的 `AndroidManifest.xml` 是**占位文本**（明文 XML，不是编译后的 binary AXML），apksigner 从中读不到 minSdkVersion——签名时显式加 `--min-sdk-version 21` 即可签成、`apksigner verify` 通过：`apksigner sign --min-sdk-version 21 --ks course.keystore ...`。

### 第 6 步 · 装上验证

```bash
adb install -r CompleteDemo_aligned.apk
```

装上后看桌面图标名——变成 `HACKED!` 就说明整条重打包链走通了。

> split / XAPK 包先把所有分包解开 / 合并再动，别只解 base（1.1 的坑）。

---

## 三、检查点

- 说得清 unzip 和 apktool 各自用在什么场景（看结构 vs 改包）。
- 能用 apktool 解出一个包，找到**可读的** `AndroidManifest.xml`、smali、以及 `lib/assets` 里的 Unity 核心料。
- 能走完 `apktool b → zipalign → apksigner` 三步，把改过的包打成**装得上、跑得起来**的 APK。
- 装上后能看到自己的改动生效（app 名变 `HACKED!`）。

---

## 动手：运行演示程序

双击 **`正课/lesson-2.2.exe`**，它用课程自带工具**真跑**一遍完整重打包：

1. 先给你看 unzip 出来的 `AndroidManifest.xml` **二进制原样**（前几十字节 hex）——一眼看出"读不了改不了"；
2. **真跑 `apktool d`** 解包（你会看到 Baksmaling、Decoding resources… 的真实输出），弹文件管理器让你逛可读产物；
3. **改一处**（把 app 名改成 `HACKED!`）；
4. **真跑 `apktool b` → `zipalign` → `apksigner`**（都是课程自带工具），产出重打包好的 APK；
5. 告诉你 `adb install -r` 装上后就能看到 `HACKED!`——并点明"你重签了，指纹变了"，引到 2.3。

一直按回车看完（结尾问是否清理产物）。

---

## 小结 & 下一章

你会把包解开、改一处、重打包成能跑的 APK 了。但你重签后**指纹变了**——下一课 **2.3**：游戏怎么用**签名校验**发现你改了包。
