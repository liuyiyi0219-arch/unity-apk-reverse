# 6.2　DEX 破壳

> 状态：🟢 已完成（正文 + 真 Unity APK 靶，脱壳→jadx 真跑）｜第六部分（Java / DEX）

---

## 一、本章的目的

6.1 是没壳的情况——jadx 一把过。这一课是另一半：**dex 被加壳（加固）**了。加壳后 `classes.dex` 里**看不到真代码**，只剩一个**加载器**：真 dex 被加密藏在别处（`assets` 数据文件 / native `.so` 里），运行时才解密、用 `DexClassLoader` 加载进内存。直接 jadx `classes.dex` 只看到加载器、看不到逻辑。破法：**先脱壳**（把真 dex 弄出来），再回到 6.1 的 jadx。

> **壳在做什么**：把应用真正的 `classes.dex` 加密打包成一个数据文件（放 `assets/` 或拼进 `.so`），换上一个壳自带的 `classes.dex`——里面是壳的 `Application` 加载器 + 一堆 stub。App 启动时加载器把真 dex 解密到内存 / 私有目录，再 `DexClassLoader`/`InMemoryDexClassLoader` 加载。**磁盘上的 `classes.dex` 是假的，真 dex 只在运行时出现。**

---

## 二、先认出"有壳"

反编译 `classes.dex` 后，这些信号说明有壳：

- **应用自己的类不见了**：该有的业务类 / 自定义 Activity 在 `classes.dex` 里找不到，只有一个陌生的 `Application` 子类 + 一批 stub / 加载器类。
- **有个加载器**：`Application.attachBaseContext` / `onCreate` 里 `System.loadLibrary(...)` + 读一个 `assets` 数据文件 + `DexClassLoader`/`InMemoryDexClassLoader`/`DexFile` + 反射 `loadClass`。
- **`AndroidManifest` 的 `application:name`** 指向那个壳 Application（不是应用自己的）。
- **`assets/` 里有个高熵大文件**（加密的真 dex），或真 dex 拼在某个 `.so` 里。

---

## 三、两条脱壳路

### 路 A · 静态脱壳（key 是构建期常量时）

加载器要能解密，**解密算法 + key 就在包里**——简单壳直接把 key 写在加载器 Java / native 里。步骤：

1. jadx `classes.dex` 读加载器：认出**算法 + key + 藏 dex 的资产名**（比如 `assets/xxx` + `XOR key` / `AES key`）；
2. 从 APK 里抽出那个加密资产，用读到的算法 + key **离线解密** → 得到真 dex（解出来首 8 字节应是 dex magic `64 65 78 0a`）；
3. jadx 解出的真 dex → 读逻辑 / 找 flag（回到 6.1）。

**这条零设备**，适合 key 静态可取的壳（本课作业就是这种）。

### 路 B · 动态 dump（通用，key 不静态时）

商业加固（key 运行时才生成 / native 里算 / 每次不同）静态拿不到 key——那就**让壳自己把真 dex 解到内存，再 dump 出来**。这是通用脱壳法：

- **hook 类加载点**：frida hook `DexClassLoader`/`InMemoryDexClassLoader` 构造、或 `DexFile.openDexFile*` / `defineClass`——壳调它们时参数里就是解密后的 dex（内存 buffer 或落地文件），dump 下来；
- **内存扫描**：壳把真 dex 解到内存后，`SIGSTOP` 冻住进程，扫 `/proc/PID/mem` 找 dex header 指纹（`dex\n035` / `64 65 78 0a`）把整段抠出来（配合完整性/反调试对抗）；
- dump 出的真 dex 再 jadx。

> **真实分布**（本仓库 266 份 java 解包 recipe）：**≈92% 的包 `classes.dex` 是明文、根本没壳**（6.1 那条直接过）。真见过的 DEX 壳只有一种第三方加固方案，形态就是"真 dex 加密在专有 `assets` 文件、磁盘 `classes.dex` 只剩 ~30 个 stub"，脱它要**路 B 动态 dump**（等壳的 native 把 dex 解到内存、`SIGSTOP`+内存扫描抠出，需真机 + 对抗完整性保护）。commercial 加固基本都走路 B。DEX 壳指纹目录 + 各家思路见方法论文档 `docs/methods/protection-fingerprints.md`。

---

## 四、检查点

- 能认出"有壳"：应用自己的类不见了、只剩壳 `Application` + 加载器（`loadLibrary` + 读 assets + `DexClassLoader`），`assets` 里有高熵大文件。
- 能说清壳的原理：真 dex 加密藏起来、磁盘 `classes.dex` 是假的，运行时才解密加载——所以直接 jadx 看不到逻辑。
- 会走**路 A 静态脱壳**：从加载器读出算法 + key + 资产名，离线解密真 dex（验 dex magic），再 jadx。
- 知道**路 B 动态 dump** 是通用法（key 不静态时）：hook `DexClassLoader`/`defineClass` 或 `SIGSTOP`+内存扫 dex 指纹，需真机；真实语料里 DEX 壳极少、脱这类要路 B。

---

## 动手

- **作业（脱壳 → 反编译找 flag）**：下载 `作业/材料/dex-shell-kit.zip` 里 `dexshell-demo.apk`——一个**能玩的 Unity 爬塔小游戏**（回合制战斗，点 [攻击] / [重击] 爬 5 层塔），Java 层加了 dex 壳。真正的 flag 类 `com.course.dexshell.SecretFlag` **不在 `classes.dex` 里**：它被编成单独的 dex、XOR 加密塞进 `assets/secret.bin`，运行时由 `classes.dex` 里的加载器解密 + `DexClassLoader` 加载（把塔玩通关也拿不到 flag）。jadx `classes.dex` 只看到加载器 `com.course.dexshell.Loader`（里面有 XOR key + 资产名）→ 抽 `assets/secret.bin`、按 key 解密 → 得真 dex → jadx → 读 `SecretFlag.getFlag()` 的 flag（走**路 A 静态脱壳**，零设备；这也是路 B 动态 dump 的静态版对照）。任务详情见 `作业/README.md`。

---

## 小结 & 下一部分

有壳的破法就一句话——**先脱壳、再回 6.1 的 jadx**。壳把真 dex 加密藏起来、磁盘 `classes.dex` 只剩加载器：key 静态可取就**离线解密**（路 A），不可取就**让壳自己解到内存再 dump**（路 B，通用，需真机）。真实语料里 dex 壳极少（≈92% 明文），碰到的多是那一种要动态 dump 的 DEX 加固。**代码层（C# / 脚本 / Java）到此全部过完**，下一部分转资源 / AB（第七部分）。
