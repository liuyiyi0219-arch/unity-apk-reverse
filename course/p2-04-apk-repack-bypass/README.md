# 2.4　绕过签名校验（破）

> 状态：🟢 已完成（正文 + 演示程序）｜演示程序：`正课/lesson-2.4.exe`、绕过脚本：`正课/材料/hook_sig.js`

---

## 一、本章的目的

2.2 你把包改了重打包，2.3 游戏用**签名校验**发现了它（重签 → 指纹变 → 被判篡改 → 拒启动 / 闪退）。这一课教你**让它照样过**——三条绕过路，按场景选一条。

> 这是"改包能跑"之后的最后一道坎：重打包本身（2.2）已经会跑，但带签名校验的游戏会拦你，所以要过这道校验。

---

## 二、操作指南

三条绕过路，按你的目的选：

### 路线 ① · 不改签名：root + frida 运行时注入（最省事）

如果你只是想**运行时改行为**（改数值、抓调用），根本不用重打包——**root 设备 + Frida 运行时注入**即可：APK 一个字节没动、签名不变，**签名校验根本不触发**。

> 这也是为什么后面很多动态章节走 frida 而不是改包。要持久改文件（改资源、改 smali 逻辑）才需要重打包，那才会撞上签名校验。

### 路线 ② · patch 掉校验

定位校验函数，改成**恒返回"通过"**：

- 校验在 **java 层** → 在 apktool 解出的 **smali** 里改（返回值改成 `true` / 跳过比对），再重打包（2.2 的链）；
- 校验在 **il2cpp 的 C#**（我们靶子的 [`SignatureCheck.IsUntampered`](../demo/CourseDemo/Assets/Scripts/Game/SignatureCheck.cs) 就在这）→ 找到方法的 RVA，在 `libil2cpp.so` 里 patch（3.x 讲怎么定位 RVA）。

> 游戏常在**多处**校验，只 patch 一处会被别处拦——找全。

### 路线 ③ · frida hook 取签名的接口（不改包）

运行时 hook 系统取签名的调用，让它**永远返回原始签名**——`ApkSignatureKiller` 一类工具就是这思路。核心：hook `PackageManager.getPackageInfo`，把返回里的 signatures 换回官方的：

```javascript
// 正课/材料/hook_sig.js —— 让签名校验永远拿到“原始签名”
Java.perform(function () {
    var PM = Java.use('android.app.ApplicationPackageManager');
    var ORIG = '308203...';  // 原始签名证书的 hex（从官方包取，见 2.1/2.3）
    PM.getPackageInfo.overload('java.lang.String', 'int').implementation =
        function (pkg, flags) {
            var info = this.getPackageInfo(pkg, flags);
            if ((flags & 0x40) !== 0 && info.signatures.value) {   // GET_SIGNATURES
                var Sig = Java.use('android.content.pm.Signature');
                info.signatures.value = [Sig.$new(ORIG)];          // 塞回原始签名
            }
            return info;
        };
});
```

跑：`frida -U -f com.course.demo -l 正课/材料/hook_sig.js`（用 `-f` spawn 抢在校验前；本课 `tools/frida/` 已带匹配版 frida-server）。

> 本机设备 SELinux 是 Enforcing，vanilla frida-server 枚举进程会报 `system_server` 错——跑前先 `adb shell su -c 'setenforce 0'`，或改用 **Florida**（去特征 frida）。frida 通道排障（`-H` + `adb forward` + attach）见 3.4。

---

## 三、检查点

- 能说清三条绕过路各自适用什么场景（不改包运行时改 / patch 校验 / hook 取签名接口）。
- 能用 `正课/材料/hook_sig.js` 让一个被重打包、签名指纹已变的包**照样过**签名校验、正常进游戏。
- 知道校验常在多处，patch / hook 要找全。

---

## 动手：运行演示程序

双击 **`正课/lesson-2.4.exe`**（全程课程自带工具，零安装），它会：

1. 快速重演 2.2 的重打包：apktool 解 `CompleteDemo` → 改 app 名 `HACKED!` → `apktool b` → zipalign → apksigner 重签，出改版包；
2. **对比签名指纹**——原包 vs 改版包，你会看到指纹变了；
3. 讲清：这个改版包装到 `CompleteDemo` 上会**弹窗"检测到被篡改"并退出**（2.3 埋的签名校验真拦，已实拍验证）；
4. 给出三条绕过路。

**绕过脚本**：本文件夹的 `正课/材料/hook_sig.js`（hook `getPackageInfo`、内嵌原始签名证书，让校验永远通过）。frida 就绪时 `frida -U -f com.course.demo -l 正课/材料/hook_sig.js`，hook 上后被篡改的包也不弹窗、正常进游戏。

> **真靶练习**：真实靶 `VampireSurvivors` 里埋的是同一套签名校验（启动打 `untampered=True`）。你 2.2 重打包它 → 启动变 `False` 被拦 → 用路线 ②/③ 绕过，就是一次完整的攻防。

> **关于 frida LIVE**：frida 在不同 host / 设备上环境差异大。本课把重打包做成**稳定可复现的演示程序**，frida 绕过给**现成脚本 + 步骤**——跑不通时先按 3.4 排障（Florida / 端口 / 版本对齐 / `-H` + adb forward）。

---

## 小结 & 下一章

APK 层通关：拿包（2.1）→ 重打包（2.2）→ 签名校验（2.3）→ 绕过校验（2.4）——你能拿到、解开、改、打回去跑、并绕过签名校验了。下一章 **3.1** 正式进 C# / IL2CPP 层，从 `dump.cs` 出骨架。
