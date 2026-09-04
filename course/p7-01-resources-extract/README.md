# 7.1　AssetBundle 提取

> 状态：🟢 已完成（正文 + 真 AssetBundle 靶，UnityPy / AssetStudioMod 真跑）｜第七部分（资源 / AB）

---

## 一、本章的目的

Unity 游戏的美术 / 音频 / 字体 / 模型 / 文本，绝大多数打包在 **AssetBundle** 里——这是 Unity 的**通用资源打包容器**（热更、按需下载、OBB 的资源都塞这）。这一部分就围绕它：**资源怎么塞进这个容器、容器怎么解、多个容器之间怎么关联**。本章讲**裸 AB**（没加密的），下一课 7.2 讲加密。

> **AssetBundle 是什么**：一个把若干 Unity 资源对象序列化打包成的文件（`.ab`/`.unity3d`/无扩展名，标准容器是 **UnityFS** 魔数）。构建时把资源"归到某个 bundle 名下"就打进对应的 AB。一个游戏往往有**成百上千个 AB**，加一份 **manifest**（记录哪个 AB 依赖哪个）。

---

## 二、容器里有什么 + 怎么解

### AB 容器 = 一堆序列化对象

一个 AB 解开后是一组 Unity 对象，每个对象有个**本地编号 PathID**：

| 对象类型 | 是什么 | 导出 |
|---|---|---|
| **Texture2D / Sprite** | 贴图 / UI（**图里可能写着东西**） | PNG |
| **AudioClip / Font / Mesh** | 音频 / 字体 / 模型 | wav / TTF / obj |
| **TextAsset** | 配表 / JSON / 有时是 lua | 原始字节 |
| **GameObject / MonoBehaviour** | prefab / 挂在上面的组件 | 结构 + 引用 |

> 容器结构（细节留给 7.2）：`UnityFS header → BlocksInfo（block 目录 + 对象清单）→ 压缩数据块`。裸 AB 三段都明文，标准工具直接读。

### 两个工具（裸 AB 提取的主力）

**AssetStudioMod**（`aelurum/AssetStudio`，经典 AssetStudio 的活跃 fork——**原版已停维护**）：GUI 拖进去，左边按类型列对象、右边预览、选中导出；带 CLI：

```bash
AssetStudioModCLI <bundle 或整个 bundle 目录> -t tex2d -o out    # 只导贴图
AssetStudioModCLI <目录> -o out                                   # 全导
```

**UnityPy**（Python 库，脚本化 / 精确控制 / 接自己的流水线）：

```python
import UnityPy
env = UnityPy.load("rewardpack")                 # 吃单个 bundle、或一整个目录
for obj in env.objects:
    if obj.type.name in ("Texture2D", "Sprite"):
        obj.read().image.save(f"{obj.read().m_Name}.png")
```

> 遇到**非标准魔数的自研容器**（不是标准 `UnityFS`），换支持它的 fork（如 `Escartem/AnimeStudio`）。

---

## 三、★容器之间怎么关联（依赖）

**一个 AB 不一定自包含**。举例：一个 `hero.prefab` 打在 A 包里，它用的贴图 / 材质打在 B 包里。A 包里那个 prefab 对贴图的引用，是一个**指向 B 包某个 PathID 的指针**——A 包自己没有这张贴图的字节。

- **构建时**：Unity 把资源分到不同 bundle，跨 bundle 的引用记进 **manifest**（每个 AB 的 `m_Dependencies`：我依赖哪些别的 AB）。
- **运行时**：加载 A 包前，**必须先加载它依赖的 B 包**，否则那个引用解析成 null（贴图丢失）。
- **逆向时**：只拿到 A 包，prefab 的贴图引用会是一个**指向你没有的包的 PathID（断链指针）**。要把 prefab 连同美术完整还原，得**把它依赖的 AB 一起拿到、一起喂给工具**——AssetStudioMod 加载**整个 bundle 目录**、UnityPy `load` **整个目录**，PathID 才能跨包解上。

一句话：**AB 之间靠 PathID + manifest 依赖串起来；解一个游戏的资源常常要解一整套 AB，不是单个。**

---

## 四、检查点

- 能说清 AssetBundle 是什么：Unity 的通用资源打包容器（UnityFS），资源按 bundle 名打进去，一个游戏成百上千个 AB + manifest。
- 会认容器里的对象类型（Texture2D/Sprite/AudioClip/Font/Mesh/TextAsset/prefab）、每个有 PathID。
- 会用 **AssetStudioMod**（GUI/CLI，AssetStudio 活跃 fork）或 **UnityPy**（脚本）把裸 AB 里的对象导出成 PNG/wav/TTF/字节。
- **能说清容器间的依赖**：跨 bundle 引用是指向别的 AB 的 PathID，逆向要把依赖的 AB 一起喂工具（加载整个目录）才连得上，否则断链。

---

## 动手

- **作业（从 AB 提图读 flag）**：下载 `作业/材料/ab-image-kit.zip` 里的 AssetBundle `rewardpack`——爬塔（Tower Climb）游戏的**通关奖励包**（没加密的标准 `UnityFS` 容器）。里面有一张"第 30 层 · 通关奖励"横幅贴图，**flag 就写在图上**（是图片像素，`strings` 抠不到）。用 AssetStudioMod（GUI 拖进去 / `AssetStudioModCLI rewardpack -t tex2d -o out`）或 UnityPy 把 `Texture2D` 导成 PNG，打开图片读那行 `FLAG{...}` 交上来。任务详情见 `作业/README.md`。

---

## 小结 & 下一章

资源这层的核心是 **AssetBundle 这个容器**：资源序列化打包进 UnityFS、用 AssetStudioMod / UnityPy 一键解、多个 AB 靠 PathID + manifest 依赖串起来（解一套、不是解一个）。这些都是**没加密**时的情况。下一章 **7.2** 讲另一半：真实游戏怎么给 AB **加密**——而且很特别，**几乎不做整包对称加密**（运行时按需加载、全量解密吃帧率），而是只加密容器最关键的一小段（部分加密）——认出来、恢复 key、解开、再回来提资源。
