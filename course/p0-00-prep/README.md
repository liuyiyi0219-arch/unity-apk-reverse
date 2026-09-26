# 0.0　前序：机器与设备准备

> 状态：🟢 已完成（正文 + 设备体检演示程序）｜演示程序：`正课/lesson-0.0.exe`
>
> 全课最靠前的一章，先做完。

---

## 一、本章的目的

你需要两样东西：**一台 Windows 电脑（Win10 及以上）** 和 **一台已 root 的安卓真机**。电脑跑工具、通过 USB 连手机调试；手机是逆向的落点。

逆向不只是静态看代码，还要**在游戏运行时观察和修改它**——注入调试器、读游戏进程的内存、翻它的私有数据。这些都需要 **root 权限**，所以手机必须 root。

本章带你：装好电脑上的 adb、打开手机的调试开关、把手机 root、再用 adb 确认 root 成功。

---

## 二、操作指南

### A. Windows 装 adb

`adb` 是电脑和手机通信的工具。

1. 下载 platform-tools：`https://dl.google.com/android/repository/platform-tools-latest-windows.zip`
2. 解压到固定目录（如 `D:\tools\platform-tools\`），把该目录加入系统 PATH。
3. 新开终端，敲 `adb version`，打出版本号即成功。

### B. 打开手机的调试开关

1. **开发者选项**：`设置 → 关于手机 → 版本号`，连点 7 次。
2. **USB 调试**：`设置 → 系统 → 开发者选项 → USB 调试`，打开。
3. 数据线连电脑，手机弹**"允许 USB 调试吗？"**，勾"一律允许"→ 确定。
4. 电脑上敲：
   ```bash
   adb devices
   ```
   看到一行 `<序列号>    device` 即连接成功。

### C. Root 手机（用 Magisk）

刷机步骤按机型走，去 **Magisk 中文网** 找你手机对应的教程跟着做：

> **https://magiskcn.com/**

大致流程（具体以 magiskcn 对应教程为准）：

1. **解锁 Bootloader**（会清空数据，先备份）。
2. 提取本机对应版本的 `boot.img`。
3. 用手机上的 Magisk App 给 `boot.img` 打补丁，得到 `magisk_patched.img`。
4. `adb reboot bootloader` 进 fastboot，`fastboot flash boot magisk_patched.img`，重启。
5. 重启后 Magisk App 显示已安装（有版本号）即 root 成功。

### D. 验证 root

```bash
adb shell su -c id
```

输出里有 **`uid=0(root)`** 就成了（第一次跑，手机上点 Magisk 弹出的授权框"允许"）。

再认一下架构和系统版本，后面选工具版本要用：

```bash
adb shell getprop ro.product.cpu.abi          # CPU 架构，多为 arm64-v8a
adb shell getprop ro.build.version.release    # 安卓版本
```

---

## 三、Checklist

逐条勾，每条都能用 adb 敲出预期输出才算过。（也可以直接跑 `正课/lesson-0.0.exe`，它替你全过一遍。）

| # | 检查项 | 验证命令 | 期望 |
|---|---|---|---|
| 1 | Windows 是 Win10+ | `设置→关于` | 版本 ≥ 10 |
| 2 | adb 装好、在 PATH | `adb version` | 打出版本号 |
| 3 | 开发者选项 + USB 调试已开 | —（手机上） | 开关为开 |
| 4 | 设备已连、已授权 | `adb devices` | 有一行 `… device` |
| 5 | 已 root | `adb shell su -c id` | 输出含 `uid=0(root)` |
| 6 | 认得架构 | `adb shell getprop ro.product.cpu.abi` | 通常 `arm64-v8a` |
| 7 | 认得安卓版本 | `adb shell getprop ro.build.version.release` | 打出版本号 |

全部 ✅ 后，逆向工作台就绪。

---

## 动手：运行设备体检程序

打开 **`正课/lesson-0.0.exe`**，它自动跑一遍上面的 checklist：找 adb → `adb devices` → `adb shell su -c id` 验 root → 读架构/版本，最后打一张 ✅/❌ 记分卡，缺哪项对应回本章哪一段都告诉你。（纯 adb 只读命令，不改设备。）

---

## 小结 & 下一章

有了一台已 root、能 adb 调试的安卓真机 + 一台 Win10+ 电脑，工作台就绪。工具清单见 **[0.1 环境配置](../p0-01-env-setup/README.md)**，然后从 **[1.1 APK 的分层结构](../p1-01-apk-layers/README.md)** 开始。
