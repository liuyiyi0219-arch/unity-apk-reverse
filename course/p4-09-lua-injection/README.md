# 4.9　Lua 注入

> 状态：🟢 已完成（正文 + 真机注入靶）｜Part 4 · Lua 动态进阶

---

## 一、本章的目的

4.4 讲的是**hook 现有的东西**：hook `luaL_loadbuffer` 拿脚本、monkey-patch 已加载的函数。这一课更进一步——**往活着的 Lua 引擎里注入你自己的 Lua 代码**：抓到运行时的 `lua_State*`，用它 `luaL_dostring` 跑任意 Lua，直接读 VM 的运行时状态、调游戏的任意 Lua 函数。

> **为什么需要它**：有些东西**只活在运行时的 VM 里**——一个运行时才拼出来的全局、一个内存里的表、一段没落盘的逻辑。静态拆包（4.1/4.2）读不到，hook loadbuffer（4.4）也只拿到"加载时那份"。要读 VM 的**活状态 / 调它的活函数**，就得把代码注进那个正在跑的 `lua_State`。这是我们逆真实游戏（腾讯 Pandora/SLua 那类）时用过的实招。

---

## 二、原理：`lua_State` 是钥匙

Lua VM 的一切都挂在一个 `lua_State*`（全局表、栈、所有已加载的函数）。**拿到这个指针，就能对这个 VM 为所欲为**：`luaL_dostring(L, "任意 Lua")` 就在这个 VM 里跑、读它的 `_G`、调它的函数。

问题是：`lua_State*` 在进程内、但你手上没有它的引用。**怎么拿？——hook 一个高频、参数里带 `lua_State*` 的函数**：

- **`lua_pcall(lua_State* L, …)` / `lua_resume`**：游戏每帧、每个事件都在调（跑 Lua 逻辑）——`args[0]` 就是 `L`；
- **任意注册的 C 函数**（`lua_CFunction`）：签名 `int f(lua_State* L)`，被 Lua 调到时 `L` 就在参数里；
- **`luaL_loadbuffer` 的调用**（同 4.4 的 hook 点）也带 `L`。

**关键：这些函数是在 Lua 线程上被调的**——所以你在 hook 的 `onEnter` 里（同一个线程）拿 `L` 直接跑 Lua 是**线程安全**的（Lua 不是线程安全的，必须在它自己的线程上动它）。

---

## 三、破法：frida 抓 `lua_State` → 注入读全局

```js
const m = Process.getModuleByName('libxxx.so');   // 带 Lua VM 的库（本靶子 Unity 的 libtolua.so）
const luaL_loadstring = new NativeFunction(m.getExportByName('luaL_loadstring'), 'int', ['pointer','pointer']);
const lua_pcall       = new NativeFunction(m.getExportByName('lua_pcall'), 'int', ['pointer','int','int','int']);
const lua_tolstring   = new NativeFunction(m.getExportByName('lua_tolstring'), 'pointer', ['pointer','int','pointer']);

let done = false;
Interceptor.attach(m.getExportByName('lua_pcall'), {
  onEnter(args) {
    if (done) return; done = true;
    const L = args[0];                              // ← 抓到活着的 lua_State*（在 Lua 线程上）
    luaL_loadstring(L, Memory.allocUtf8String('return __secret'));  // 注入我们的 Lua
    lua_pcall(L, 0, 1, 0);
    console.log('[inject] ' + lua_tolstring(L, -1, NULL).readUtf8String());  // 读回全局值
  }
});
```

拿到 `L` 之后能做的远不止读一个全局：`dofile` 你的整套 Lua、hook 游戏 Lua 函数、改内存表、把结果通过某个 C 函数回传——**等于在游戏的 Lua 世界里拿到了 root**。

---

## 四、检查点

- 能说清 4.9（注入）和 4.4（hook 拿脚本）的区别：**4.4 拿"加载时那份"，4.9 把代码注进活着的 VM 读活状态 / 调活函数**。
- 能说清为什么 hook 高频函数就能拿到 `lua_State*`（它是那些函数的参数），以及为什么必须在 Lua 线程上注入（Lua 非线程安全）。
- 会用 frida hook `lua_pcall`（或注册的 C 函数）抓 `L`，`luaL_loadstring`+`lua_pcall` 注入 Lua、`lua_tolstring` 读回结果。

---

## 动手

- **作业（真机注入）**：`作业/材料/luainject.apk`——一个 **Unity + toLua** 的爬塔小游戏（主体 C# 壳、战斗逻辑跑在 `libtolua.so` 的 Lua VM 里），flag **只作为一个运行时 Lua 全局** `__secret` 活在 `lua_State` 里（脚本文本里没有明文、`strings` 抠不到；C# 每帧调 `lua_pcall` 跑心跳）。frida attach、hook `libtolua.so!lua_pcall` 抓 `L`、注入 `return __secret` 读出 flag（参考脚本 `作业/材料/inject_secret.js`）。任务详情见 `作业/README.md`。

---

## 小结 & 下一部分

Lua 层从静态（4.1–4.3）、hook/dump（4.4）、保护对抗（4.6–4.8）到**运行时注入**（4.9，抓 `lua_State` 注入任意 Lua）都齐了。脚本层另一半 puerts 在 Part 5——其中 **5.6** 是这一课的 puerts 版：抓 puerts 引擎句柄、往 JS 引擎注入代码（`Eval` 是后端无关 FFI 入口，V8/QuickJS 通用）。
