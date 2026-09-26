# 7.3 作业：type-tree 解 ScriptableObject 字段，读出 flag

## 题干

`configpack` 是爬塔（Tower Climb）游戏的**策划数值配置**——一个 AssetBundle（**type-tree 被剥掉**，模拟发行版），里面一个 `GameConfig`（ScriptableObject：塔层数 / 顶层怪物 HP·ATK / 暴击倍率 / 通关语 / 通关奖励）。直接提这个 SO 只拿到空壳 `{ m_Name: "gameconfig" }`——**数值在后面那段"没说明书"的原始字节里**。`typetree.txt` 就是那张说明书（字段布局，模拟从 il2cpp 生成）。你要抽出 SO 的原始字节、照 `typetree.txt` 布局解出字段，读 `reward`（通关奖励）字段里的 flag。

## 材料（`gameconfig-kit.zip`）

| 文件 | 是什么 |
|---|---|
| `configpack` | 爬塔策划数值配置 AssetBundle（type-tree 剥掉），里面一个 `GameConfig` |
| `typetree.txt` | `GameConfig` 的字段布局（int32/float32/string 顺序 + 标准头说明） |

## 怎么交

解出 `reward` 字段里的 flag，交给批改器：

```bash
check.exe FLAG{...}
```
