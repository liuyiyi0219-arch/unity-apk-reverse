# 课程编译器（builder）· 通用组件

把**任意一门课**编译成「mod 包」，交给加载器（`../loader/server.exe`）加载运行。不绑具体课程——所有课程专属数据下沉到各课的 `maps/full.json`（塔定义，节点自带 `mode`）+ `course.json`（课程名字）。

## 三块通用组件 vs 课程内容

```
docs/course/
├── builder/          ← 编译器（本目录，通用）：gen.py 出网页数据 + build_mod.py 打包
├── loader/           ← 运行时（通用）：server + 内嵌 UI 壳（爬塔.html）
└── <course>/         ← 一门课（如 unity-apk-reverse）
    ├── p*/…          章节（正课 + 作业/check.exe + 材料）
    ├── maps/full.json  塔定义：节点/act/依赖 + 每节点 mode(env|flag|upload|file)
    ├── course.json     本课编译配置：full/free 的名字 + 塔 acts
    ├── tools/ demo/    演示 exe 用的共享 fixture（随 mod 打包）
    └── _web/           builder 生成的 dev 数据（dev 直跑用，可重生成）
```

## 编译

```bash
# 任意课编译成 mod 包 → <course>/mod-dist/mod(.zip)
python builder/build_mod.py --course <course> --zip --version 1.0.0

# 只刷 dev 数据（不打包）→ <course>/_web/，改完刷新即见
python builder/gen.py --course <course>
```

## 叠加课程（叠加课就是这么做的）

一门课的 `course.json` 里写 `"baseCourse": "../<别的课>"`，它就成了**叠加课**：章节 / tools / demo
先在本课文件夹找，找不到回退 baseCourse。用于叠加课——**本课文件夹里只放本课专属的
（塔 `maps/full.json` + `course.json` + 想改的那几章），共享的内容章和 fixture 全靠
baseCourse 指向基座课，不拷贝、不飘移**。想让某章在叠加课里不一样（比如更轻的环境检测），
就在本课文件夹里放一份同名章覆盖它即可（本课优先）。

例：`unity-apk-reverse-free/` 只有 `course.json` + `maps/full.json`，baseCourse 指 `../unity-apk-reverse`，
`build_mod.py --course unity-apk-reverse-free` 即出课程包，8 章从基座课解析。

## 运行

- **学员**：`loader/server.exe` + `mod.zip` 解开的 `mod/`（或 `mods/<课>/`）放同一目录，双击 server.exe → `http://127.0.0.1:8770/`。UI 壳在 server.exe 里内嵌，mod 只带数据+章节。
- **作者 dev**：在课程根（`cd <course>`）里跑 `../loader/server.exe`，cwd 有 `_web/maps.js` → 进「源码直跑」模式，改 README/maps 后 `gen.py` 刷新即见。

## 加新课

1. 建 `<course>/`，放章节（每章 `正课/` + `作业/{_src/check.go, 材料/}`）。
2. 写 `<course>/maps/full.json`（塔定义，节点带 `chapter` + `mode`）。
3. 写 `<course>/course.json`（full/free 名字 + free.acts）。
4. `python builder/build_mod.py --course <course> --zip` 即出包，同一套 loader 加载。

## 节点 mode（写在 maps/full.json 的节点里）

| mode | 批改怎么调 check.exe |
|---|---|
| `env` | 不填答案，直接跑（探本机/设备，如环境体检） |
| `flag` | 学生填拼好的 flag，当单参数传 |
| `upload` | 学生传一个文件（节点 `uploadName` 定扩展名，如重打包 APK） |
| `file` | 学生填答案文本，落成文件（节点 `answerName` 定文件名，默认 answer.txt） |
