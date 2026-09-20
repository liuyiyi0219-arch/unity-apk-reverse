# 8.1 毕业考：逆穿 VampireSurvivors 五层全保护，作弊通关拿 flag

## 题干

`VampireSurvivors.final.apk` 是一个能玩的吸血鬼幸存者游戏，叠了**五层保护**：① 开始按钮签名校验、② metadata AES 加密、③ native `.so` 壳、④ 小怪数值在加密 AB 的 `balance.lua` 里（默认**打不死**）、⑤ 每 20s 陨石毁灭世界（逻辑在 puerts 的 `meteor.js`）。

**正常玩通不了关**——陨石会毁灭世界，小怪打不死。你要**逐层逆开这游戏、作弊改掉它的规则**，打赢本不可能的最终 BOSS。打赢时游戏会解密一张藏图落到 app 外部目录，**flag 就藏在那张图里**。

## 材料

| 文件 | 是什么 |
|---|---|
| `VampireSurvivors.final.apk` | 叠了五层全保护的真游戏靶包（55MB）。**这是个被重打包过的包**——装上一点【开始】就弹"检测到盗版/重打包，禁止开始"，先破签名门才玩得起来（第一关） |

## 怎么交

逆穿五层、作弊打赢 BOSS → `adb pull` 出游戏解密落盘的 `reward.gif`（在 `Android/data/com.course.vampire/files/`）→ `strings` 或按 GIF Comment 字段取出 flag：

```bash
check.exe FLAG{...}
```

对了它吐通关信息；没真逆穿+作弊通关拿到那张图，随便填过不了。
