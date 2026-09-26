# 4.8　Lua 破整体加密

> 状态：🟢 已完成（正文 + 正课演示 + 作业批改）｜`正课/lesson-4.8.exe`（AES/XXTEA/XOR 真解密）· `作业/check.exe`（找 key 解密 → flag）｜Lua 保护三破收官

---

## 一、本章的目的

实战最常见的一类 Lua 保护——**整体加密**：磁盘上的脚本（或整个 AssetBundle）是密文，运行时才解。方法论一句话：**整体加密的破法是「找 key + 认算法」，不是密码分析**。key 几乎永远是 **build-time 常量**，逆代码就能拿到；拿到 key + 对应算法离线解即可。

> 本章的算法谱系、key 来源、真实踩坑，全部来自逆向流水线积累的 **253 份真实游戏 recipe**（`lua-extract/cache`），不是编的。

---

## 二、操作指南

### 第 1 步 · 认算法

对 253 份真 recipe 统计（≈ 流行度），主流是 **AES / XXTEA / DES / TEA**：

| 算法 | 提及 | 说明 |
|---|---|---|
| **AES** | 615 | 最常见。多是 AES-256-CBC-PKCS7，key/iv 常从硬编码串**派生**（md5/sha256/PBKDF2） |
| **XXTEA** | 587 | tolua/cocos 生态经典，`key + sign` 前缀 |
| **DES / TEA** | 290 / 210 | 老牌分组密码 |
| **XOR** | 623 | 数字高但多是**组件**（叠在别的算法 / 压缩上）；纯单字节 XOR 是少数 |
| 压缩层 | LZ4/zlib/brotli/zstd | **先压缩再加密**——解密后还得解压才是明文 |
| 容器 | ZFile | key **运行时派生**，这类才真需要设备 |

> 真正通用的破法是**逆代码定位解密逻辑、拿到算法和 key**（第 2–3 步），对 AES/XXTEA/DES 都成立；已知明文攻击只在纯 XOR 上是捷径（见文末诚实定位）。

### 第 2 步 · 定位解密逻辑（在读取函数附近）—— 关键

算法和 key 都在**游戏读取那个加密文件的地方**：找到"读文件"的函数，解密就紧挨着它（读进密文 → 解密 → 交给 Lua VM / AssetBundle）。**定位读取点 = 同时拿到算法和 key 的位置**，这是整章的枢纽。

读取点在 dump.cs / Ghidra 里这么找：

- **Lua loader**：tolua 的 `LuaFileUtils.ReadFile` / `LoadFile`、xLua 的自定义 loader（`LuaEnv.AddLoader` / `customLoaders`）——它读完 bytes、`return` 给 VM 前那几行就是解密；
- **通用文件读**：`File.ReadAllBytes`、`AssetBundle.LoadFromFile` / `LoadFromMemory`、`TextAsset.bytes`、native `fopen`/`fread`/`AAssetManager_open`；
- **native VM 入口**：`luaL_loadbuffer` 的**调用者**——谁在调它、传进去的 buffer 从哪解密来。

顺读取点往下读几行，就会撞见一个 `AES.Decrypt(bytes,key,iv)` / `XXTEA(...)` / 一段 XOR 循环——**这一步同时确认了算法（第 1 步谱系里的哪个）和 key 的入口（那个 decrypt 调用的 key 参数）**。

### 第 3 步 · 从解密调用回溯拿 key（build-time 常量，静态可取）

key 是第 2 步那个 decrypt 调用的参数，回溯它的来源。真 recipe 里的分布：

- **C# 常量**：`HotfixManager.FileKey`、`AssetBundleEncryptKey`（`static readonly string`）；
- **`global-metadata.dat` 里的明文字符串字面量**（在某个 offset）；
- **native `.rodata`/`.data`**：`libxlua.so` / `libil2cpp.so` / `libcocos2dlua.so`；
- **从硬编码串派生**：`key=md5(secret)`、`iv=md5(ab_name)[8:24]`、PBKDF2(password,salt)；
- **从 native 表派生**：`key[0]=0x44, key[i]=kt2[i]^kt3[i]`；
- **运行时派生**（ZFile 容器）：只有这类才真需要设备 hook（4.4）。

> **扫 key 的范围要覆盖 `global-metadata.dat`**（il2cpp 的字符串字面量都在里面）。真实案例 **com.cyberjoy.x3**：key `leocool1!q2@w3#e` 就是 metadata 里的明文字符串字面量（@ offset `0xabfbc`）——把 metadata 一起扫就直接拿到，静态可解、不需设备。

### 第 4 步 · 照算法离线解密

拿到 key + 算法，离线解。三种真实方案（演示程序照搬真 recipe 参数真跑）：

- **① AES-256-CBC**（原型 `com.lighthouse.freeplay.ameuno`）：`key=md5(secret)` 的 32 位 hex 当 32 字节、`iv=md5(ab_name)[8:24]`（per-bundle IV）；secret 是 C# 常量 `AssetBundleEncryptKey`。实测 `secret=",lr,pbope#@^#982(@"` → key `0ce83aa3…` → 解出明文。
- **② XXTEA**（tolua/cocos 常见）：加密文件 = `sign` 前缀 + `XXTEA(原长度 + 明文)`；key/sign 逆 `libtolua.so` 的 `setXXTEAKeyAndSign` / `.rodata`。校验 sign 前缀 → 解 → 读回原长度。
- **③ 单字节 XOR**（原型 `com.global.szsslg`）：key=`0x5D`，来源 C# `AssetManager.BuildLuaXorKey()`。

### 第 5 步 · 用 lua-token 密度验证

判解密成没成，看 **lua-token 密度**（`local`/`function`/`return`/`require`/`--[[`）。加密的通常是 lua **源码文本**，解对了就是一堆 Lua 关键词。

> 真实案例 **com.global.szsslg**（单字节 XOR key=`0x5D`）：加密的是 lua 源码，用 lua-token 密度一判就中。本课演示实测复现——同一份解密结果，lua-token 判 `true`、而字节码 magic（`\x1bLua`）判 `false`：源码本来就不以 `\x1bLua` 开头，所以密度判才是对的尺子。

### 第 6 步 · 解密不是终点

- **先解密再解压**：数据常先 gzip/lz4/brotli 压缩再加密，解出来还要解压才是明文；
- **里层可能还有保护**：解开整体加密后可能还是改格式（4.6）/ 魔改字节码（4.7）的 luac，或直接是明文源码；
- **验收标准**：能读出源码 / 能反编译，不是"解出来没报错"。

---

## 三、检查点

- 能说清"整体加密破法 = 定位解密逻辑 + 认算法 + 拿 key"，并列出真实算法谱系（AES/XXTEA/DES/TEA/XOR）。
- **会从"读取加密文件的函数"（`LuaFileUtils.ReadFile` / `File.ReadAllBytes` / `AssetBundle.Load*` / `luaL_loadbuffer` 调用者）追到紧挨着它的解密调用**——算法和 key 入口都在那。
- 知道 key 是 build-time 常量、藏在 C#/metadata/native，能说出至少 3 个来源，且找 key 会连 `global-metadata.dat` 一起扫（x3 教训）。
- 用 **lua-token 密度**验证解密（szsslg 案例：加密的是源码、不以 `\x1bLua` 开头，密度判才是对的尺子）。
- 知道解密后可能还有压缩层 / 里层保护，验收看能否读出源码。

---

## 动手

本课的动手分两块，**分开放**：

- **`正课/`** —— 走一遍参考解答。双击 `正课/lesson-4.8.exe`（三种加密都是**真算法真解密**，Go crypto/aes + 手写 XXTEA，参数照搬真实 recipe）：253 recipe 算法谱系 → AES-256-CBC（ameuno：常量 md5 派生 key/iv → 真 AES 解 → lua-token 验证）→ XXTEA（key+sign）→ XOR（lua-token 密度判对）→ key 藏哪（x3 在 metadata 明文）。
- **`作业/`** —— **自己动手 + 自动批改**（找 flag）。`作业/材料/lua-aes-kit.zip` 里 `skills.luac.enc` 是一份 **AES 对称加密**的 luac（`strings` 抠不到）——整体加密没有可爆破的密码学漏洞，只能**找 key + 认算法**。`strings libgame_rodata.bin` 认出算法 `AES-128-CBC`、在 `BuildSkillCipherKey` 旁找 16B key/IV → AES-128-CBC 解密 → 得标准 luac → 反编译（4.2）或 `strings` 它，**代码里那个 string 就是 flag** → `作业/check.exe FLAG{...}`。任务详情见 `作业/README.md`。

> 建议：先做 `作业/`（真动手才学得会逆向），卡住了再翻 `正课/` 对答案。

> **已知明文攻击的诚实定位**：XOR 有"密文 ⊕ 明文 = 密钥"的性质，配 luac 固定头能算 key——但**只对纯 XOR 有效**，对 AES/XXTEA/DES 无用，且真实里主要用在边角（派生 key 的一步 / key 在加密 metadata 里读不到时的后备）。所以**先逆代码找 key（主线）**；纯 XOR 且 key 一时找不到才用爆破当捷径，且**爆破要用 lua-token 打分**（真 recipe 就是"256 候选按字符集 + 关键词密度打分"），不是 magic。

---

## 小结 & 下一章（Lua 保护三破收官）

整体加密的正解是**定位解密逻辑（在读取加密文件的函数附近）→ 认出算法 → 从 decrypt 调用回溯拿 key**（AES/XXTEA/DES/TEA/XOR + 压缩层 + 容器），是逆代码而非对着密文做密码分析。key 是 build-time 常量，藏在 C#/metadata/native，静态可取；验证用 **lua-token 密度**；扫 key 覆盖 metadata。加上前两课，**Lua 保护四类全破**：改格式（4.6）· 改字节码 / 魔改 VM（4.7）· 整体加密（4.8）· 外加万能运行时 dump（4.4）。

**Lua 层的破解三件套（格式 / 字节码 / 加密）到此收齐**。下一章 **4.9** 讲 Lua 运行时注入（hook 拿明文）；之后再进 **第五部分 · puerts（TS/JS）**（从 **5.1** 起）单开来讲脚本层的另一半。
