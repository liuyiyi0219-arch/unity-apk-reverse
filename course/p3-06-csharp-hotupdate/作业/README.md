# 3.6 作业：flag 藏在热更 DLL 里

## 题干

给你一个 C# 热更靶包，它同时是个能玩的爬塔 mini-game（装上真机点 [攻击]/[重击] 打怪爬塔）。战斗/UI 在主体 il2cpp 里，但主体里还埋了一个**热更加载器** `HotfixBoot`，运行时从 `StreamingAssets`（打进 APK 的 `assets/HotUpdate.dll.bytes`）读一份托管 DLL、加载执行。运营下发的逻辑和它内嵌的字符串都在那份热更 DLL 里。**flag 就在其中**（主体 metadata 里搜不到），把它挖出来读。

## 靶包

点面板 **⬇ 下载材料** 拿到 `hotfixflag.apk`。

## 怎么交

塔里点这个节点 → **✍ 做作业** → 填你读到的 flag → **▶ 运行批改**。
