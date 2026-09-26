# 5.6　puerts 注入

> 状态：🟢 已完成（正文 + 真机注入靶）｜Part 5 收尾 · 脚本层动态注入（puerts 版，对应 Lua 4.9）

---

## 一、本章的目的

前面 5.1–5.5 都是**静态读脚本**（V8 明文/去混淆、QuickJS 解字节码）。这一课是运行时——**往活着的 puerts 引擎里注入你自己的代码**：抓到 puerts 的 JS 环境句柄，用它 eval 任意 JS，直接读 VM 的运行时状态、调游戏的任意 JS 函数。这是 Lua 4.9（往 `lua_State` 注入）的 puerts 版，套路一模一样。

**本章放在 5.x 最后，是因为它对两个后端通用**：puerts 有**统一的 JS 注入入口**——不管游戏用 V8 还是 QuickJS 后端，注入入口是**同一个 native 导出符号**。所以一份注入脚本 V8 / QuickJS 都能用，正好收尾整个 puerts 部分。

> **为什么需要它**：有些东西**只活在运行时的 JS VM 里**——一个运行时才拼出来的全局、内存里的对象、没落盘的逻辑。静态拆包读不到。要读 VM 的活状态、调它的活函数，就得把代码注进那个正在跑的引擎。

---

## 二、原理：puerts 的注入入口是后端无关的（本章核心）

puerts 桥接 C# ↔ JS，native 层（`libpuerts.so`）暴露一套**稳定的 C ABI（FFI 层，puerts 叫 "Default" 后端）**给 C# 调。**这套 ABI 是后端无关的**——后端（V8 / QuickJS / Node）是**运行时参数**，不是不同的符号。源码为证（`Puerts/Runtime/Src/Default/Native/PuertsDLL.cs`）：

```csharp
[DllImport("puerts")] IntPtr CreateJSEngine(int backendType);   // 后端是"参数"
[DllImport("puerts")] int    GetLibBackend(IntPtr isolate);     // 查当前后端
[DllImport("puerts")] IntPtr Eval(IntPtr isolate, byte[] code, string path);   // 同一个符号
[DllImport("puerts")] IntPtr GetStringFromResult(IntPtr resultInfo, out int len);
```
且 `Backend.cs` 里 `enum BackendType { V8, QuickJS = 2, NodeJS, ... }` + `class BackendQuickJS`。

**结论**：`Eval` 是 puerts 的通用 FFI 注入入口，**QuickJS 后端的 `libpuerts.so`（qjspkg）导出同一个 `Eval` 符号**。第一个参数 puerts 命名成 `isolate`（V8 历史命名），实际是它**后端无关的 JSEngine 句柄**——V8 后端下包着 `v8::Isolate`，QuickJS 后端下包着 `JSContext`。**一份注入脚本，两个后端通用**。

> **两种 puerts 模式，入口都通用**：
> - **FFI / "Default" 模式**（本课 demo 用的反射后端）：`libpuerts.so` 导出 `Eval` / `GetStringFromResult`，C# 用 `DllImport` 调。后端无关。
> - **IL2Cpp 静态 pesapi 模式**：用 `pesapi_eval(pesapi_env, code, …)`——puerts 的嵌入 API（pesapi = Puerts Embedding API），**本身就是后端抽象层**。也后端无关。
>
> 两种模式的注入入口都不是 V8 特有——都是 puerts 通用的。

---

## 三、破法：frida 抓句柄 → 注入读 `globalThis` 全局（V8 / QuickJS 同一份脚本）

本靶子的高频入口是 `libpuerts.so` 的 **C 导出 `Eval(void* isolate, const char* code, const char* path)`**——`env.Eval(...)` 最终落到它（App 每帧 eval `__tick(n)`）。hook 它、第一次触发时 `args[0]` 就是活着的**引擎句柄**（在 JS/Unity 主线程上，跑 JS 安全）；然后**用同一个 `Eval` 把你自己的 JS 喂进去**，返回值用 `GetStringFromResult` 读回：

```js
const m = Process.getModuleByName('libpuerts.so');
const Eval = new NativeFunction(m.getExportByName('Eval'), 'pointer', ['pointer','pointer','pointer']);
const GetStringFromResult = new NativeFunction(m.getExportByName('GetStringFromResult'), 'pointer', ['pointer','pointer']);
let done = false;
Interceptor.attach(m.getExportByName('Eval'), { onEnter(a) {
  if (done) return; done = true;
  const handle = a[0];                                        // ← 抓到引擎句柄（V8 isolate / QuickJS ctx）
  const holder = Eval(handle,
      Memory.allocUtf8String("'EXFIL:'+globalThis.__secret"),  // 注入我们的 JS
      Memory.allocUtf8String('inj'));
  const len = Memory.alloc(4);
  console.log('[inject] ' + GetStringFromResult(holder, len).readUtf8String());  // 读回 = flag
}});
```

**这份脚本对 V8 和 QuickJS 后端都成立**：`Eval` 是同一个符号、句柄语义相同、`GetStringFromResult` 同一个读回口——唯一区别是注入的 JS 在哪个 VM 里跑（V8 或 QuickJS）。本课 demo 是 V8 后端的 puerts App；换成 QuickJS 后端的 puerts App，**同一份 `inject.js` 原样就能用**（`GetLibBackend` 可现场确认后端）。

和 4.9 完全对称：Lua 抓 `lua_State`、`luaL_loadstring`+`lua_pcall` 注入、`lua_tolstring` 读回；puerts 抓引擎句柄、`Eval` 注入、`GetStringFromResult` 读回。（不同 puerts 版本导出名可能有别；这份是**真机验证过**的，完整版见作业。）

拿到句柄后能做的远不止读一个全局：eval 整套 JS、hook 游戏 JS 函数、读内存对象、mock 后端——**等于在游戏的 JS 世界拿到 root**。

---

## 四、检查点

- 能说清注入（读运行时 VM 状态 / 调活函数）和静态读脚本（5.1–5.5）的区别。
- 能说清**为什么 puerts 的注入入口后端无关**：`Eval` 是 FFI 层同一个符号，后端是 `CreateJSEngine(backendType)` 的参数（V8 / QuickJS / Node 共用），"isolate" 只是 V8 历史命名的后端无关句柄。
- 会用 frida hook 高频的 `Eval`、抓句柄、注入 JS 读 `globalThis` 的运行时全局并回传——并知道**同一份脚本 V8 / QuickJS 通用**。
- 认得它和 4.9（Lua 注入）是同一套：抓引擎句柄 → 注入 eval。

---

## 动手

- **作业（真机注入）**：下载 `作业/材料/puerts-inject-kit.zip` 里 `puerts-inject.apk`——一个 puerts 爬塔小游戏（C# 壳 + `battle.js` 战斗逻辑，主循环每帧 `env.Eval("__tick(n)")`），flag **只作为运行时 JS 全局** `globalThis.__secret` 活在引擎里（落包 `battle.js` 只有编码字节表、没明文、`strings` 抠不到）。frida attach、hook 高频的 `Eval` 抓句柄、注入 JS 读 `globalThis.__secret`。这份 demo 是 V8 后端；**同一套 hook 换 QuickJS 后端的 puerts App 原样适用**（源码已证 `Eval` 是通用符号）。任务详情见 `作业/README.md`。

---

## 小结 & 下一部分

puerts 收尾在动态注入：抓引擎句柄 → 注入代码，**V8 / QuickJS 通用**（同一个 `Eval` FFI 入口，后端只是 `CreateJSEngine` 的参数）。和 Lua 4.9 是同一套"抓引擎句柄 → 注入代码"的活。**整个脚本层收尾**：Lua（4.x）+ puerts 两条后端路（V8 静态 5.1–5.2 / QuickJS 5.3–5.5）+ 动态注入（4.9 Lua、5.6 puerts）。下一部分转 Java/DEX（第六部分）。
