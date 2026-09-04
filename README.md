# Unity APK 逆向入门

动手扒开一个 **Unity 手游**：从装好环境、把游戏装上真机，到解开 APK，把里面的 **C# / Lua / 资源 / Java** 各扒一层读出来。零基础也能跟着做——每一章都有可运行的演示 + 自动批改的作业（做对点亮，像爬塔一样通关）。

## 直接玩（推荐）

去 [**Releases**](../../releases) 下最新包，解开后：

```
server.exe            ← 双击启动（本地课程加载器）
mods/course/          ← 课程内容
```

双击 `server.exe`，浏览器开 `http://127.0.0.1:8770/` —— 课程、演示、作业批改全在网页里闭环，不用开终端。

## 课程内容（8 章）

| 段 | 章 | 学到 |
|---|---|---|
| 地基 | 环境准备 · 工具清单 | 装好 adb / python / UnityPy / java / jadx |
| APK 入门 | APK 五层结构 · 装游戏到真机 | 认清 Unity APK 的五层，把靶子游戏装上手机 |
| 读懂各层 | C# 出骨架 | 从 il2cpp metadata 里读出字符串 |
| | Lua 认形态 | 识别 Lua 的多种形态，XOR 爆破解出脚本 |
| | DEX 反编译 | jadx 把 classes.dex 还原成 Java |
| | 资源提取 | 从 AssetBundle 导出贴图 |

C# / Lua / 资源 / Java 四大层各一个上手速通，建立「我能扒 Unity 游戏」的手感。

## 从源码构建

课程用一套通用引擎驱动，任意课程目录都能编译成可加载的包：

```bash
python builder/build_mod.py --course course --zip   # → course/mod-dist/mod.zip
cd loader && go build -o server.exe .                # 编译加载器
```

- `builder/` —— 通用编译器（课程 → mod 包），见 `builder/README.md`
- `loader/` —— 通用运行时（HTTP server + 内嵌网页 UI）
- `course/` —— 课程内容（8 章 + 塔定义 `maps/full.json`）

> 演示用的靶包 / 工具（`course/tools`、`course/demo`）体积较大，不进 git；直接玩请下 Releases 里的成品包。

## License

课程内容仅供学习与授权测试使用。
