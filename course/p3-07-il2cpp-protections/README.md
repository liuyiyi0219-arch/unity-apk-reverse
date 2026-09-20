# 3.7　C# 破 metadata 加密

> 状态：🟢 已完成（正文 + 正课演示 + 作业批改）｜`作业/check.exe`（脱 AES metadata → flag）｜一个问题、三种方法

---

## 一、本章的目的

真实游戏保护 il2cpp,动的第一样常是 **`global-metadata.dat`**——把它**整体加密**。加密后 magic 变了、Il2CppDumper 拒收、`strings` 也抠不到里面的 C# 字符串。**成熟的对称加密(AES 这类)没有"已知明文一减就出"的漏洞**,你没法靠拼凑把它静态还原。

但你**不需要**破密码学。这一课把"脱 metadata 加密"这**一个问题**讲透,给**三条脱法**——各有取舍,场景不同选不同的:

| 方法 | 怎么做 | 取舍 |
|---|---|---|
| **① 静态找密钥** | 逆 `libil2cpp.so`,认出是 AES、把 **key + IV** 挖出来,写离线脚本解 | **不用设备**;通用算法时"逆算法"其实就是"找密钥" |
| **② unidbg 模拟执行** | 把 `libil2cpp.so` 丢进 unidbg(PC 上的 ARM 模拟器),直接**调它的解密函数**,喂密文出明文 | **跨版本兼容好**;不用懂算法、不用找 key |
| **③ 内存 dump** | 让 APK 跑起来,metadata 在内存里已被解成明文,从 RAM **扫出来** | **最方便**;不碰算法、不找 key |

三条路拿到的是**同一份明文 metadata**,喂回 3.1 的 Il2CppDumper,就回到明文流程。

---

## 二、认：字符串命中≈0，正文被整体加密

```bash
for s in UnityEngine mscorlib Assembly-CSharp; do
  printf '%-16s ' "$s"; grep -a -o "$s" global-metadata.dat | wc -l
done
xxd -l 4 global-metadata.dat     # magic
```

- **六个类型名命中≈0** → 正文被整体加密(不是明文、也不是只坏了头)。
- magic 不是 `AF1BB1FA`(被加密改掉了);想指认厂商见 §六。

> 一个 10 秒的省事出口:magic 变、命中≈0 时,先试**单字节 XOR**——有工作室只做全文单字节 XOR 躲裸扫描,XOR 一下 magic 就回 `AF1BB1FA`。**但真用了 AES 这类成熟算法就没这捷径了**,老老实实走下面三条。

---

## 三、① 静态找密钥（不用设备）

**通用算法,"逆算法"其实就是"找密钥"。** 你不用重写 AES——AES 全世界一套,`pip install pycryptodome` 就有。你要做的是**在 `libil2cpp.so` 里把 key 和 IV 挖出来**:

1. 解包 APK,Ghidra / IDA 打开 `libil2cpp.so`,定位 metadata 解密路径(从 `il2cpp_init` / 载入 metadata 的地方往里找,或搜 AES 的 S-box 常量表认出 AES)。
2. 看解密函数引用的**常量**:AES 的 **16 字节 key** + **16 字节 IV** 就编在附近(很多游戏就是这么把 key 焊死在 so 里)。
3. 拿 key/IV 写离线脚本解密整份 `global-metadata.dat`:

```python
from Crypto.Cipher import AES
d = open("global-metadata.dat","rb").read()
pt = AES.new(KEY, AES.MODE_CBC, IV).decrypt(d)   # KEY/IV 是你挖出来的
assert pt[:4] == b"\xAF\x1B\xB1\xFA"              # magic 回来了 = 解对了
```

**优点:全程零设备。** 缺点:key 换了 / 算法改了要重挖,跨版本得重来。

---

## 四、② unidbg 模拟执行（跨版本兼容好）

unidbg 是 PC 上的一个 **ARM/Android 模拟器**:它能把 `libil2cpp.so` 加载起来、**直接调用里面的 native 函数**,不用真机。

- 你**不用看懂算法、也不用找 key**——找到那个"输入密文、输出明文"的解密函数入口,把加密的 metadata 喂进去,让 unidbg 把它当真机一样执行,拿输出。
- **优点:跨版本兼容好。** 只要函数签名/入口没大变,游戏更新了、key 换了、算法微调了,你的调用脚本基本不动——因为是"让它自己算",而不是"你替它算"。
- 缺点:得把函数入口和参数对上,遇到函数依赖运行时环境(JNI、其它 so)时要补桩。

> unidbg 的完整用法(加载 so、定位函数、补环境、调用)见 **0.4**（工具章）;本课只需知道它是脱 metadata 加密的第二条路,以及它"跨版本省事"的价值。

---

## 五、③ 内存 dump（最方便）

最省事的一条:**根本不碰算法,也不找 key。**

- il2cpp 引擎**只认明文 metadata**。加密壳把磁盘的 `global-metadata.dat` 加密了,但游戏一启动,壳**必须**把它解密成标准格式送进引擎——否则引擎自己也跑不起来。
- 所以**明文一定在某刻出现在进程内存里**(magic 变回 `AF1BB1FA`)。让游戏跑到主界面、metadata 解好,扫内存把它 dump 出来即可。

```bash
adb shell monkey -p <pkg> 1                                     # 等进主界面
frida -H 127.0.0.1:27042 -p $(adb shell pidof <pkg>) -l dump_metadata.js
adb pull /data/local/tmp/dumped-global-metadata.dat
```

`dump_metadata.js` 扫 `r--` 内存找 `AF 1B B1 FA` magic,命中就整块 dump。**优点:不管 key 多复杂、算法多绕,你都绕过去了,最方便。** 缺点:要真机(或模拟器)、要能注入(反调试见 3.8),dump 时机要等解密完成。

> 不想注入也行:`cat /proc/<pid>/maps` 定位 metadata 映射区,纯 `adb dd /proc/<pid>/mem` 读出来——对反 frida / 反注入隐形。

---

## 六、认对之后：喂回 Il2CppDumper + magic 指认厂商

三条路任一条拿到**明文** `global-metadata.dat` + 磁盘的 `libil2cpp.so` → **Il2CppDumper** → `dump.cs`,回到 3.1–3.3。

确认是加密之后,magic 值能指认厂商:`0x1357FEDA`+`CODEPHIL` 串=HybridCLR 商业版/CODEPHIL;ASCII `"LIKE"`=LIAPP;`MHY\0`=米哈游;每 build 随机=Virbox/FairGuard。

---

## 七、检查点

- 能说清**为什么不用破密码学**:引擎只认明文,三条路(找密钥/模拟执行/内存 dump)都不重写算法。
- 三条路的**取舍**说得出:静态找密钥不用设备、unidbg 跨版本好、内存 dump 最方便——什么场景选哪条。
- 会用字符串命中测试认"正文整体加密"(命中≈0),会拿 magic 指认厂商。
- 拿到明文 metadata 后能喂回 Il2CppDumper,验证 `dump.cs` 正常、接回 3.1。

---

## 动手

- **`正课/`** —— 走一遍脱法闭环(造 AES 加密 metadata → Dumper 拒 → 三条路各演示恢复明文 → Dumper 接受出 dump.cs)。
- **`作业/`** —— **自己动手 + 自动批改**。下载 `作业/材料/metadata-enc-kit.zip`:`metadata-encrypted.apk` 的 `global-metadata.dat` 被**整体加密**(具体什么算法、密钥藏在哪,**你自己去认、去挖**——这正是静态找密钥要练的),flag 是 metadata 里的一个 C# 字符串,密钥编进了 `libil2cpp.so`。**三条路任选一条做通**(kit 里带了 `decrypt_skeleton.py` 给静态法、`dump_metadata.js` 给内存 dump 法),拿到 flag 交给 `作业/check.exe FLAG{...}`。任务详情见 `作业/README.md`。

> 靶包是**自造教学样本**——一个能玩的爬塔 mini-game（`com.course.hdrflag`，装上真机点 [攻击]/[重击] 打怪），用一套成熟对称算法（AES-128-CBC）整体加密 metadata、密钥焊进 `libil2cpp.so`,运行时由改过的 `MetadataLoader` 解回明文（所以游戏正常跑），是真实"metadata 整体加密"的等效再现,不含任何真实游戏数据。flag 是通关码金库 `Vault37.GetClearCode` 返回的明文字符串,玩通关只把它的长度打日志、不显示明文——想拿到它,得脱掉 metadata 加密后 `strings | grep FLAG`。

---

## 小结 & 下一章

脱 metadata 加密就这一个问题、三条路:**静态找密钥(零设备)/ unidbg 模拟执行(跨版本)/ 内存 dump(最方便)**——AES 这类成熟算法没有静态捷径,但你不用破它,绕过去就行。内存 dump 的硬核(反调试对抗 + SoFixer 修复)在 **3.8 native 壳**;unidbg 的完整用法见 **0.4**。
