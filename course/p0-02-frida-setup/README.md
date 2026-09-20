# 0.2　Frida：心智模型 + 把它调通

> 状态：🟢 已完成（正文 + 演示程序）｜演示程序：`正课/lesson-0.2.exe`（Frida 通道体检）
>
> 后面**每一层的动态调试**（hook C# 3.4 / hook Lua 4.4 / hook native / hook Java）都站在这一章的地基上。先把 Frida 调通、心智模型立起来，再谈各层怎么 hook。

---

## 一、本章的目的

把 **Frida** 在你的 Win 电脑 + 安卓真机之间**调通**（`frida-ps -U` 能列出进程 = 通了），并建立它的**心智模型**——搞清它到底在干什么，后面各层的 hook 才不是照抄咒语。

### 心智模型：Frida 到底是什么

一句话：**Frida 把一个 JS 引擎（GumJS）注入到目标进程里，让你的 JS 在目标进程内部跑**，再通过桥接去读改它的 native / Java / C# 世界。

- **JS ↔ 目标进程**的三座桥（后面各层各用一座）：
  - `Interceptor.attach(addr, …)` —— 挂 **native 函数**（.so 的导出或偏移），读改参数 / 返回值（hook native，接 3.9）；
  - `Java.use('类名')` / `Java.perform` —— 挂 **Java 类方法**（dex/SDK，hook 签名校验 `getPackageInfo`，接 2.4）；
  - `frida-il2cpp-bridge` —— 在 il2cpp runtime 上按类名/方法名挂 **C# 方法**（接 3.4）。
- **两种接入目标进程**的方式：
  - **frida-server**：一个跑在设备上的常驻程序，需 **root**。本课主用。
  - **gadget**：一个 `.so`，**打进 APK 里**随游戏一起加载，**免 root**（未加固包才好焊）——见 gadget 那节 / `gadget-repack` 工具。
- **两种启动目标**：
  - **spawn**（`-f 包名`）：由 Frida 把游戏**从头拉起**并在第一行代码前挂好——抢在早期校验 / 解密之前。
  - **attach**（`-p pid` / `-n 名`）：挂到**已经在跑**的进程——更稳，但错过了启动早期。

---

## 二、操作指南

### 第 1 步 · Windows 装 host 端 + 记住版本对齐

```bash
pip install frida-tools
frida --version          # 本机 17.4.4
```

> **★头号铁律：host 和 device 的 Frida 必须同一个版本。** 你电脑上 `frida --version` 是多少，设备上的 `frida-server` / `florida-server` 就得是同一版本，差一个小版本都连不上。（本课 `tools/frida/` 内置的 server 要和你 host 的 `frida-tools` 对齐；不一致就按 host 版本重下对应的 server。）

### 第 2 步 · Android 起 frida-server

设备已 root（0.0）。把**和 host 同版本**的 `frida-server`（arm64）推上去跑：

```bash
adb push frida-server /data/local/tmp/
adb shell su -c 'chmod 755 /data/local/tmp/frida-server'
adb shell su -c '/data/local/tmp/frida-server &'
```

> SELinux 是 Enforcing 时，vanilla frida-server 枚举进程会报 `system_server` 错——先 `adb shell su -c 'setenforce 0'`。
>
> **游戏检测 Frida**（闪退 / 找不到进程）时，把 `frida-server` 换成 **Florida**（去特征 fork，用法一样），见 [0.1](../p0-01-env-setup/README.md)。

### 第 3 步 · 建一条稳的通道（Windows 头号坑）

在 Windows 上，Frida 的 **USB 通道经常挂**：`frida -U -f 包名`（spawn）超时、`frida-ps -U` 枚举报错。**验证过的稳妥走法**是走 TCP + attach：

```bash
adb forward tcp:27042 tcp:27042           # 把设备的 frida 端口转到本地
frida-ps -H 127.0.0.1:27042               # 走 TCP 列进程（别用 -U）
frida -H 127.0.0.1:27042 -p <pid> -l x.js # attach 到已运行进程（别用 -f spawn）
```

> 记住两条：**用 `-H` + `adb forward` 而不是 `-U`**；**用 attach（`-p`）而不是 spawn（`-f`）**。需要抢启动早期（早期校验/解密）才用 spawn，那时先让游戏在前台起来再 attach 往往也够。

### 第 4 步 · 调通验证

```bash
frida-ps -H 127.0.0.1:27042
```

能**列出一屏进程**（含 `com.course.vampire` 之类）就说明 host ↔ device 通了。再挂一个**最小 hook** 证明端到端能改行为——例如 attach 到某个进程、`Interceptor.attach` 一个 libc 函数打印回调，能看到回调触发就算真通了。

> **枚举失败 ≠ 没调通**：`frida-ps` 列进程是靠**注入 `system_server`** 来枚举的，有的设备的 `system_server` 会拒绝 ptrace 写（报 `unable to perform ptrace pokedata: I/O error`），于是 `frida-ps` 失败——但这只卡"列全部进程"，**attach 到某个具名目标照样能 hook**。而后面所有动态章节都是 attach 具名目标（游戏进程），**不受这个影响**。所以看到 `frida-ps` 报这个错、但 attach + hook 能跑，就是通的。（这个错和"版本不对齐 / server 没跑"是两码事——那两种是真没通。）

---

## 三、检查点

- 能用自己的话说清 Frida 心智模型：注入 JS 引擎到目标进程 + 三座桥（Interceptor / Java.use / il2cpp-bridge）+ server vs gadget + spawn vs attach。
- host `frida --version` 和设备 `frida-server` 版本一致。
- `frida-ps -H 127.0.0.1:27042` 能列出设备进程。
- 能挂一个最小 hook 让回调触发（端到端通）。

---

## 动手：运行演示程序

双击 **`正课/lesson-0.2.exe`**（Frida 通道体检），它替你把上面的 checklist 跑一遍：查 host `frida-tools` 版本 → `adb devices` → 设备端 frida-server/Florida 在不在、版本对不对 → `adb forward` → `frida-ps` 能不能列进程 → 报一张 ✅/❌ 记分卡，缺哪步指回本章哪一段。

> Frida 在不同 host / 设备上环境差异大，体检程序把每一步的真实报错也贴出来，方便对症。跑不通时优先查两件事：**版本对齐**、**用 `-H`+forward+attach 而不是 `-U`+spawn**。

---

## 小结 & 下一章

Frida 通了、心智模型立起来了——这是后面所有动态章节的地基。各层怎么用它：**hook C#**（3.4，frida-il2cpp-bridge 按类名挂方法）、**hook Lua**（4.4）、**hook native**（.so 的 Interceptor）、**hook Java**（`getPackageInfo` 绕签名校验，2.4）、**免 root 注入**（gadget）。回到主线，下一部分 **1.1** 从分层结构开始。
