# 通用课程编译器 · 打包：把一门课编译成「mod 包」——学员买课后发这个包，加载器（loader/server.exe）加载它。
# 不绑任何一门课——吃 --course <目录>，读该课的 maps/full.json + course.json。UI 壳由加载器内嵌，不进 mod。
#
# 叠加课程（course.json 有 baseCourse，如叠加课）：章节/fixture 先在本课找、找不到回退 baseCourse——
# 共享内容章不拷贝、不飘移。叠加课 = 一个独立的叠加课，正常 --course 编译即可，无 --free 特例。
#
# 用法：python build_mod.py --course <课程目录> [--zip] [--version 1.0.0] [输出目录]
import os, sys, glob, json, shutil, datetime, argparse, subprocess

HERE = os.path.dirname(os.path.abspath(__file__))
sys.path.insert(0, HERE)
import gen


def _sh(cmd, cwd):
    return subprocess.run(cmd, cwd=cwd, shell=True, capture_output=True, text=True)


def main():
    ap = argparse.ArgumentParser(description="通用课程打包器")
    ap.add_argument("--course", required=True, help="课程根目录（含 maps/、course.json、各章）")
    ap.add_argument("--zip", action="store_true")
    ap.add_argument("--version", default="1.0.0")
    ap.add_argument("out_base", nargs="?", default=None)
    a = ap.parse_args()

    course_root = os.path.abspath(a.course)
    roots = gen.course_roots(course_root)            # [本课, (baseCourse)]
    cfg = json.load(open(os.path.join(course_root, "course.json"), encoding="utf-8"))
    out_base = a.out_base or os.path.join(course_root, "mod-dist")
    mod = os.path.join(out_base, "mod")

    # 1) 清 + 建
    if os.path.exists(mod):
        shutil.rmtree(mod)
    os.makedirs(os.path.join(mod, "web"))
    os.makedirs(os.path.join(mod, "chapters"))

    # 2) web/：只放数据（UI 壳由加载器内嵌，不进 mod）
    _, _, manifest = gen.generate(roots, os.path.join(mod, "web"), writeback=False)
    chapters = sorted({e["chapter"] for e in manifest.values() if e.get("chapter")})
    print(f"① web/ 已装（{len(manifest)} 节点；UI 壳走加载器内嵌）")

    # 3) chapters/：每章批改器 + 材料 + 演示（章节按 roots 解析：本课优先，回退 baseCourse）
    nbuilt = nmat = ndemo = 0
    for ch in chapters:
        src = gen.chapter_dir(roots, ch)
        hw = os.path.join(src, "作业")
        if not os.path.isdir(hw):
            continue
        dst_hw = os.path.join(mod, "chapters", ch, "作业")
        os.makedirs(dst_hw, exist_ok=True)
        exe = os.path.join(hw, "check.exe")
        src_dir = os.path.join(hw, "_src")
        if not os.path.exists(exe):
            if os.path.exists(os.path.join(src_dir, "check.go")):
                b = _sh("go build -buildvcs=false -o ../check.exe .", src_dir)
            elif os.path.exists(os.path.join(hw, "check.go")):
                b = _sh("go build -buildvcs=false -o check.exe .", hw)
            else:
                b = None
            if b and b.returncode != 0:
                print(f"  ⚠ {ch} check.exe 编译失败：{b.stderr.strip()[:200]}")
        if os.path.exists(exe):
            shutil.copy2(exe, os.path.join(dst_hw, "check.exe")); nbuilt += 1
        mats = os.path.join(hw, "材料")
        if os.path.isdir(mats):
            shutil.copytree(mats, os.path.join(dst_hw, "材料"), ignore=shutil.ignore_patterns('_*', '__pycache__', '*.pyc')); nmat += 1
        zk = os.path.join(src, "正课")
        if os.path.isdir(zk):
            dz = os.path.join(mod, "chapters", ch, "正课")
            got = False
            for e in glob.glob(os.path.join(zk, "*.exe")):
                os.makedirs(dz, exist_ok=True); shutil.copy2(e, os.path.join(dz, os.path.basename(e))); got = True
            for sub in ("sample-output", "材料"):
                sp = os.path.join(zk, sub)
                if os.path.isdir(sp):
                    os.makedirs(dz, exist_ok=True); shutil.copytree(sp, os.path.join(dz, sub), ignore=shutil.ignore_patterns('_*', '__pycache__', '*.pyc')); got = True
            if got:
                ndemo += 1
    print(f"② chapters/ 已装：{nbuilt} 个 check.exe · {nmat} 章带材料 · {ndemo} 章带演示")

    # 4) fixture：buildtools + demo/build 靶包 + tools 工具（按 roots 解析：本课优先，回退 baseCourse）
    bt = gen.fixture_dir(roots, os.path.join("tools", "buildtools"))
    if bt:
        shutil.copytree(bt, os.path.join(mod, "tools", "buildtools"))
    demo = gen.fixture_dir(roots, os.path.join("demo", "build"))
    if demo:
        shutil.copytree(demo, os.path.join(mod, "chapters", "demo", "build"), ignore=shutil.ignore_patterns('_*', '__pycache__', '*.pyc', 'frida'))
    tools = gen.fixture_dir(roots, "tools")
    if tools:
        shutil.copytree(tools, os.path.join(mod, "chapters", "tools"), ignore=shutil.ignore_patterns('_*', '__pycache__', '*.pyc', 'frida'))
    print("③ fixture 已打包（buildtools + demo/build 靶包 + tools 工具）")

    # 5) mod.json
    tcfg = cfg["full"]
    meta = {
        "name": tcfg["name"], "version": a.version, "description": tcfg["description"],
        "tier": cfg.get("tier", "full"), "course": cfg.get("id", ""),
        "chapters": len(chapters), "nodes": len(manifest),
        "built": datetime.datetime.now().strftime("%Y-%m-%d %H:%M"),
        "engine": cfg.get("engine", "urev-tower"),
    }
    json.dump(meta, open(os.path.join(mod, "mod.json"), "w", encoding="utf-8"), ensure_ascii=False, indent=2)
    print(f"④ mod.json：{meta['name']} v{a.version}（{len(chapters)} 章 / {len(manifest)} 节点）")

    if a.zip:
        zp = os.path.join(out_base, "mod")
        shutil.make_archive(zp, "zip", mod)
        print(f"⑤ 打包：{zp}.zip（{os.path.getsize(zp + '.zip')/1024/1024:.1f} MB）")

    print(f"\nmod 包已生成：{mod}")
    print("   发课：把 mod.zip 解开的 mod/（或 mods/<课>/）放到 loader/server.exe 旁边即可。")


if __name__ == "__main__":
    main()
