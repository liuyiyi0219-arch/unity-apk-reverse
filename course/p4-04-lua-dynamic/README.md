# 4.4　Lua 动态 hook

> 状态：🟢 已完成（正文 + 演示程序 + 真跑 Lua monkey-patch）｜演示程序：`正课/lesson-4.4.exe`｜frida 脚本：`正课/材料/hook_lua_load.js`

---

## 一、本章的目的

4.1–4.3 是**静态**（拿到源码 / 数值）。这一课进**动态**：运行时给游戏注入自定义 Lua、hook 已有 Lua 函数——**改逻辑、抓调用、以及 dump 解密后的脚本**。这是 Lua 层的 3.4（对应 C# 的 Frida hook）。

> Frida 环境（通道 / 版本对齐）在 [0.2](../p0-02-frida-setup/README.md) 已调通，这一课只讲 Lua 层怎么 hook。动态 hook 也是 **4.5 抓 Protobuf 协议**的基础（在序列化 / 发送处 hook，抓明文包）。

---

## 二、操作指南

### 第 1 步 · Lua 级 monkey-patch（最自然）

Lua 是**动态语言**——函数就是值，运行时**直接重赋值**就完成 hook，天生适合。给真实的 [`Battle.lua`](../demo/CourseDemo/Assets/GameLua/src/Gameplay/Battle.lua.bytes) 挂 hook：

```lua
local orig = Battle.CalcDamage
Battle.CalcDamage = function(atk, def, crit)
    local real = orig(atk, def, crit)   -- 先跑原逻辑（观察/验证）
    print("[hook] atk="..atk.." def="..def.." 原返回="..real)
    return 99999                        -- 改返回值
end
```

本课实测（host 上真跑 Lua 5.1.5）：`CalcDamage(180,30,1.5)` 原返回 `231` → hook 后 `99999`。读参 + 改返回一次搞定，**比 3.4 hook C# 还简单**——不用管 native 地址 / 寄存器，Lua 层重赋值即可。

### 第 2 步 · hook `luaL_loadbuffer` dump 解密后的脚本（杀手锏）

Lua 动态最值钱的一招，**顺手破了加密**：

> 不管脚本在磁盘上是明文、luac、还是 AB 里 XOR/AES 加密的——**送进 `luaL_loadbuffer` 编译时，它一定是解密的明文**（VM 只能编译明文）。

所以 hook `luaL_loadbuffer`、把每次的 `buff/sz` 落盘，就把**每个被加载到的脚本**都拿到解密态——和 3.8"从内存偷解密后的 metadata"完全一个思路。我们 demo 的 Lua 是 AB 里 XOR 0x5A 加密的，但 hook 到这里已是明文：

```js
Interceptor.attach(Process.findModuleByName('libtolua.so').getExportByName('luaL_loadbuffer'), {
    onEnter(args) {
        const src = args[1].readByteArray(args[2].toInt32());   // 解密后的 chunk
        const name = args[3].readCString();
        new File('/data/local/tmp/lua_dump/'+name+'.lua','wb').write(src);   // 落盘
    }
});
```

```bash
frida -H 127.0.0.1:27042 -p $(adb shell pidof com.course.demo) -l 正课/材料/hook_lua_load.js   # 通道见 0.2
adb pull /data/local/tmp/lua_dump/
```

进游戏、脚本一加载就被 dump，**盘上加密白加了**。（xLua 换 `libxlua.so`；注意 Lua 版本对应的函数名：5.1 是 `luaL_loadbuffer`，5.3+ 是 `luaL_loadbufferx`。）

#### 这一招的两面：免破解，但只拿到「你跑到的」

**正面 —— 加密不用破**：脚本在盘上是明文、luac、还是 XOR/AES 加密都无所谓，送进 `luaL_loadbuffer` 时一定已解密。**遇到加密脚本，动态 hook 是最省事的一条路**——直接拿明文，密码学一点不用碰。

**另一面 —— dump 到的脚本可能不全**：游戏很少一次性把所有 Lua 都加载，大多**按游戏状态惰性加载**——进战斗才加载战斗脚本、开某活动才加载活动模块、点开某界面才 `require` 那个文件。所以 hook 只落盘**你这一局真正触发到的那些脚本**，没跑到的功能对应的脚本一个都不出现。**要拿得全，就把游戏各状态走一遍**（各玩法、各界面、各活动、登录到结算），把加载都触发出来；条件苛刻的冷门分支仍可能漏。

**所以和静态配着用**：

| | 动态 hook `luaL_loadbuffer` | 静态从盘上 / AB 取（4.2） |
|---|---|---|
| 加密 | **不用破**，到手即明文 | 脚本加密时要先破（找 key / 算法） |
| 覆盖 | 只有**跑到的**脚本，惰性加载→可能不全 | **全量**（包内所有脚本都在盘上） |
| 拿全的成本 | 走遍游戏各状态触发加载 | 直接取，一次到位 |

一句话：**脚本加密重、只想要关键玩法那几段逻辑 → 动态 hook 免破解最快**；**要一份完整脚本清单、冷门分支也不能漏 → 静态取全**（加密就先破，或先用动态 dump 出 key/算法再回静态一次性解全量）。两条路互补，不是二选一。

### 第 3 步 · 注入代码

拿到 `luaL_loadbuffer` 入口，就能**改 buffer 注入**：在某脚本尾部追加一段 monkey-patch（给 `Battle.CalcDamage` 套 hook），或额外 `loadbuffer` 你自己的 Lua；也可 hook C# 的 `LuaState.DoString` 直接喂 Lua。效果 = 运行时改任意 Lua 逻辑，**APK 一个字节没动**。

> 静态（4.2）改不了、或脚本运行时下载 / 解密的 → 动态在运行时改行为、或先 dump 出来再静态分析，两者互补。

---

## 三、检查点

- 能用 Lua 级 monkey-patch hook 一个真实 Lua 函数，读参 + 改返回。
- 能说清 hook `luaL_loadbuffer` 为什么拿到的是**解密后**的脚本（送进 VM 前必已解密，加密不用破）。
- 能说清它的两面：**免破解**，但只 dump 到**跑到的**脚本（惰性加载→可能不全），要拿全得走遍各游戏状态；跟静态取全互补。
- 知道 Lua 版本 / 框架对应的 C API 名（`luaL_loadbuffer` vs `luaL_loadbufferx`，`libtolua` vs `libxlua`）。

---

## 动手

- **演示（`正课/lesson-4.4.exe`）**：双击 → 讲两条路 → 用 lua 5.1.5 加载真实 `Battle.lua` 真跑 monkey-patch（`231 → 99999`，读参 + 改返回，真执行）→ 展示 hook 脚本结构 → 讲杀手锏 `luaL_loadbuffer` 拦截为什么拿到的是解密后的脚本（顺手破 AB 加密），以及它的两面（免破解，但只 dump 到跑到的、惰性加载可能不全，要拿全得走遍游戏状态），给 `正课/材料/hook_lua_load.js` + 真机 recipe → 弹文件夹。
- **作业（真机）**：下载 `作业/材料/lua-loadbuffer-hook.apk`——一个 **Unity + toLua 爬塔小游戏**：游戏主体（UI + 回合循环）在 C#，**战斗数值逻辑全在 `battle.lua` 里**（伤害/出怪/奖励/通关码）；`battle.lua` **盘上 AES 加密**（`strings` 整个 apk 抠不到明文），第一次点 [攻击] 时才解密送进 `libtolua.so` 的 `luaL_loadbuffer`。装上（能玩：打怪爬 5 层塔）、attach frida、hook `libtolua.so!luaL_loadbuffer` 读它拿到的 buffer（解密后的明文），源码里那行 `local CLEAR_CODE = "FLAG{...}"` 就是答案。**通关也拿不到 flag**（只把通关码长度打日志），只有 hook loadbuffer 拿明文——这正是本课杀手锏：游戏主体 C#、玩法逻辑 lua，读玩法逻辑就得拿它的 lua，而 lua 加密时动态 hook 最省事。任务详情见 `作业/README.md`。

> host 上 monkey-patch 是**真跑真验证**（Lua 5.1.5 实执行）；native hook 给**脚本 + 真机 recipe**（frida 通道见 0.2），设备连上照挂即可。
>
> **几个坑**：LuaJIT 没有明文 `luaL_loadbuffer` 入口（走自己的加载路径，找 `lua_load`/JIT 内部）；hook 太晚脚本早加载完了（attach 尽早）；`findModuleByName` 找不到多是库没加载（game state 不对，不是失败）；注入的 Lua 语法错会静默失败（先在 host lua 跑通再注入）；monkey-patch 后目标又被 require 覆盖会失效（hook 加载点或在赋值后再打）。

---

## 小结 & 下一章

Lua 层的"读 / 改 / dump"都齐了：静态还原（4.2）、配表还原（4.3）、动态注入 + hook（4.4，还顺手用 `luaL_loadbuffer` 破了加密）。下一课 **4.5** 做一件高价值应用——**还原 Lua 层的 Protobuf 协议**（在 Lua 里抓 `.proto` + opcode），Lua 的"裸"部分收尾。
