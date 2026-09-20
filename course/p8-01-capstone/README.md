# 8.1　毕业考：逆穿 VampireSurvivors 五层全保护

> 状态：🟢 已完成（真·五层全保护靶包 + 全链真机验证）｜全课收官「总分总」的最后一个「总」

## 本章目标

这是全课收官——**不教新东西**，把前面每课练的招式，在**一个叠了五层保护的真游戏**上全用一遍。靶子是 `VampireSurvivors.final.apk`：一个能玩的吸血鬼幸存者 clone，但**正常玩通不了关**——每 20 秒陨石毁灭世界、小怪还打不死。要通关，你得**把这游戏逐层逆开、作弊改掉它的规则**，最后打赢本不可能的 BOSS，拿到它守着的 flag。

学完你能做到：拿到一个陌生的、多层加固的 Unity 游戏，知道**按什么顺序剥保护、每层用哪招、改哪里能作弊通关、怎么验证**。

## 五层地图（每层 = 前面一章的技法）

| 层 | 这游戏里长什么样 | 用哪招 |
|---|---|---|
| ① 签名校验 | 发货包已被重打包（签名对不上），一点【开始】就弹"检测到盗版/重打包，禁止开始"、进不去 | 绕签名校验（2.4） |
| ② metadata 加密 | `global-metadata.dat` 被 AES 加密，Il2CppDumper 直接认不了 | 解 metadata 加密（3.7） |
| ③ native 壳 | `libil2cpp.so` 的 `.text` 被加密、运行时自解密，IDA F5 出乱码 | 脱 native 壳（3.8） |
| ④ Lua 逻辑层 | 小怪数值在**加密 AB** 的 `balance.lua` 里，默认小怪**打不死** | 提 AB + 改 lua（7.x + 4.x） |
| ⑤ puerts 逻辑层 | 每 20s 陨石毁灭世界，逻辑在 `meteor.js`（跑在 puerts/V8） | 改 puerts JS（5.x） |

> **贯穿全课的认知**：C# + native 是引擎/框架，**脚本层（Lua / JS）才是游戏逻辑**。这游戏把「小怪能不能打死」放在 Lua、「陨石毁不毁灭世界」放在 puerts——逆真实游戏时，当前生效的玩法逻辑往往就在这些热更脚本层，而不是 il2cpp dump 里那份。

## 通关路线（一条"黑掉游戏"的链，层间依赖决定顺序）

1. **绕签名（第一关，进都进不去）**：你拿到的 `VampireSurvivors.final.apk` 是一个**被重打包过的包**——签名和游戏内硬编码的期望值对不上。一点【开始】选角色，就弹"检测到盗版 / 重打包，签名校验未通过，禁止开始"，**连游戏都进不去**。先绕过 `SignatureCheck.IsUntampered()`（patch il2cpp 里的返回、或 frida hook 让它恒真），才玩得起来。（后面你改 lua/js 重打包，签名还是不对，所以这关得先破。）
2. **解 metadata + 脱 native 壳（读懂游戏）**：`balance.lua` 那个加密 AB 的 **XOR key、AB 名**都在 C# 里，而 C# 在**加密 metadata + 加壳的 `libil2cpp.so`** 里——先解 metadata、脱 native 壳、dump C#，才读得到 key 和加载逻辑。
3. **提 + 改 Lua（让小怪能打死）**：从包里抠出加密 AB `gamelua.ab` → 用拿到的 XOR key 解密 → 改 `balance.lua`（让小怪不再无敌）→ 重打 AB。
4. **改 puerts（屏蔽陨石）**：抠出明文 `meteor.js` → 改掉"20s 毁灭"判断 → 打回去。
5. **重打包重签跑起来**：绕过签名门 → 活过陨石 → 杀 10 只小怪刷 BOSS → 打败 BOSS → 游戏**解密一张藏图**落到 app 外部目录 → `adb pull` → **flag 在这张 GIF 里**。

> **为什么正常玩拿不到 flag**：那张图只在"打赢 BOSS"时才解密落盘，而默认平衡下小怪打不死、陨石又会毁灭世界——**打不赢**。要打赢就必须逆穿全部五层去作弊。所以 flag 锁死在"逆向 + 作弊"后面，正常玩永远碰不到。

## 检查点

- 能对一个多层加固的 Unity 游戏做**保护 triage**、排出剥壳/解密的**正确顺序**（签名→metadata→native→AB/lua→puerts）。
- 会解 metadata 加密 + 脱 native 壳读到 C#，从中找出加密 AB 的 key 与加载逻辑。
- 会提取 + 改加密 AB 里的 `balance.lua`、改 puerts 的 `meteor.js`，重打包重签、绕过签名门把改后的游戏跑起来。
- 打赢本不可能的 BOSS，从解密落盘的 `reward.gif` 里取出 flag（`strings` 或按 GIF Comment 字段解析）。

## 动手

- **作业（逆穿五层、作弊通关、取 flag）**：下载 `作业/材料/VampireSurvivors.final.apk`，按上面的路线逆穿五层保护、作弊打赢 BOSS，从解密落盘的 `reward.gif` 取出 flag 交给批改器。任务详情见 `作业/README.md`。

## 小结

真实逆向的收官不是"多学一招"，是"把招式在一个真目标上按对的顺序串起来"。这一关把签名 / metadata / native / lua / puerts 五层全保护叠在一个能玩的游戏上——**逆向就是你的通关手段**，剥完五层、改掉规则、打赢守关的 BOSS，就毕业了。
