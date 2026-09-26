# 通用课程编译器 · 数据生成：从 <course>/maps/*.json + 各章 README/作业 生成网页数据
# （maps.js / content.js / manifest.js / manifest.json）。不绑任何一门课——吃 --course <目录>。
#
# 叠加课程（course.json 有 baseCourse）：章节/fixture 先在本课文件夹找，找不到回退 baseCourse。
# 用于「叠加课」——只放本课专属的（塔 + 改过的环境章），共享内容章靠 baseCourse 指向基座课，
# 不拷贝、不飘移。
#
# 节点的 grade 模式（env/flag/upload/file）、上传/答案文件名，由 maps/*.json 的节点自带字段声明：
#   node.mode / node.uploadName / node.answerName
import json, glob, os, re, argparse

FLAG_RE = re.compile(r'FLAG\{[a-z0-9]+-[^"\'} ]*\}')


def course_roots(course_root):
    """本课根 + baseCourse 根（若有）——章节/fixture 的搜索路径，本课优先。"""
    roots = [os.path.abspath(course_root)]
    cj = os.path.join(course_root, "course.json")
    if os.path.exists(cj):
        base = json.load(open(cj, encoding="utf-8")).get("baseCourse")
        if base:
            roots.append(os.path.abspath(os.path.join(course_root, base)))
    return roots

def chapter_dir(roots, ch):
    """章节目录：搜索路径里第一个存在的 <root>/<ch>；都没有则返回本课下的（可能不存在）。"""
    for r in roots:
        d = os.path.join(r, ch)
        if os.path.isdir(d):
            return d
    return os.path.join(roots[0], ch)

def fixture_dir(roots, sub):
    """fixture（tools/、demo/build 等）：搜索路径里第一个存在的；都没有返回 None。"""
    for r in roots:
        d = os.path.join(r, sub)
        if os.path.isdir(d):
            return d
    return None


def chapter_flag(roots, ch):
    """从 <章>/作业/_src/check.go（新布局）抓嵌着的 flag；旧布局 作业/check.go 兜底。"""
    cd = chapter_dir(roots, ch)
    cg = os.path.join(cd, "作业", "_src", "check.go")
    if not os.path.exists(cg):
        cg = os.path.join(cd, "作业", "check.go")
    if not os.path.exists(cg):
        return None
    m = FLAG_RE.search(open(cg, encoding="utf-8", errors="ignore").read())
    return m.group(0) if m else None

def read(path):
    try:
        return open(path, encoding="utf-8").read()
    except Exception:
        return ""

def dump_js(var, obj, path):
    with open(path, "w", encoding="utf-8") as w:
        w.write("// 自动生成，勿手改。源在 maps/*.json 和各章 README/作业。改完跑 builder/gen.py。\n")
        w.write("window.%s = %s;\n" % (var, json.dumps(obj, ensure_ascii=False)))

def readme_usage(roots, ch):
    rp = os.path.join(chapter_dir(roots, ch), "作业", "README.md")
    if not os.path.exists(rp):
        return ""
    for ln in open(rp, encoding="utf-8", errors="ignore").read().splitlines():
        if "check.exe" in ln:
            return ln.strip().lstrip("`").strip()
    return ""

def demo_exe(roots, ch):
    """章 正课/ 下的演示 exe（相对它所在的章根，加载时按 chapters/<ch>/正课/ 找）。"""
    dg = sorted(glob.glob(os.path.join(chapter_dir(roots, ch), "正课", "*.exe")))
    if not dg:
        return None
    return (ch + "/正课/" + os.path.basename(dg[0]))


def load_maps_from_files(roots, writeback=True):
    """读本课 maps/*.json，用 check.go 的真 flag 回填节点（可写回源 json）。maps 只从本课根读。"""
    maps = {}
    for f in sorted(glob.glob(os.path.join(roots[0], "maps", "*.json"))):
        d = json.load(open(f, encoding="utf-8"))
        changed = False
        for n in d["nodes"]:
            if n.get("type") == "final":
                if n.get("flag", "") != "":
                    n["flag"] = ""; changed = True
                continue
            fl = chapter_flag(roots, n.get("chapter", ""))
            if fl and n.get("flag", "") != fl:
                n["flag"] = fl; changed = True
        if changed and writeback:
            json.dump(d, open(f, "w", encoding="utf-8"), ensure_ascii=False, indent=2)
        maps[d["id"]] = d
    return maps


def build_content(roots, maps):
    chapters = {n["chapter"] for d in maps.values() for n in d["nodes"] if n.get("chapter")}
    content = {}
    for ch in sorted(chapters):
        cd = chapter_dir(roots, ch)
        has_check = os.path.exists(os.path.join(cd, "作业", "check.exe")) or \
                    os.path.exists(os.path.join(cd, "作业", "_src", "check.go")) or \
                    os.path.exists(os.path.join(cd, "作业", "check.go"))
        content[ch] = {
            "lesson": read(os.path.join(cd, "README.md")),
            "homework": read(os.path.join(cd, "作业", "README.md")),
            "answer": read(os.path.join(cd, "作业", "答案.md")),
            "hasCheck": has_check,
        }
    return content


def build_manifest(roots, maps):
    manifest = {}
    for d in maps.values():
        for n in d.get("nodes", []):
            ch = n.get("chapter", "")
            nid = n["id"]
            if not ch or nid in manifest:
                continue
            cd = chapter_dir(roots, ch)
            cwd = ch + "/作业"                       # 相对 chapters/（加载时 server 在 mod/chapters/<ch>/作业 跑）
            has_check = os.path.exists(os.path.join(cd, "作业", "check.exe")) or \
                        os.path.exists(os.path.join(cd, "作业", "check.go"))
            mode = n.get("mode", "file")
            entry = {"chapter": ch, "cwd": cwd, "hasCheck": has_check, "mode": mode,
                     "usage": readme_usage(roots, ch), "demo": demo_exe(roots, ch)}
            if mode == "file":
                entry["answerName"] = n.get("answerName", "answer.txt")
            if mode == "upload":
                entry["uploadName"] = n.get("uploadName", "_upload.apk")
            matdir = os.path.join(cd, "作业", "材料")
            mats = glob.glob(os.path.join(matdir, "*.apk")) or glob.glob(os.path.join(matdir, "*.zip")) or \
                   glob.glob(os.path.join(matdir, "*.cs")) or glob.glob(os.path.join(matdir, "*.dat"))
            if mats:
                entry["material"] = "材料/" + os.path.basename(sorted(mats)[0])
            manifest[nid] = entry
    return manifest


def generate(roots, out_dir, maps=None, writeback=True):
    """生成 maps.js / content.js / manifest.js / manifest.json 到 out_dir。
    roots = [本课根, (baseCourse根)]；maps=None → 从本课 maps/*.json 读。"""
    if isinstance(roots, str):
        roots = course_roots(roots)
    os.makedirs(out_dir, exist_ok=True)
    if maps is None:
        maps = load_maps_from_files(roots, writeback=writeback)
    content = build_content(roots, maps)
    manifest = build_manifest(roots, maps)
    dump_js("TOWER_MAPS", maps, os.path.join(out_dir, "maps.js"))
    dump_js("TOWER_CONTENT", content, os.path.join(out_dir, "content.js"))
    dump_js("TOWER_MANIFEST", manifest, os.path.join(out_dir, "manifest.js"))
    json.dump(manifest, open(os.path.join(out_dir, "manifest.json"), "w", encoding="utf-8"),
              ensure_ascii=False, indent=1)
    return maps, content, manifest


if __name__ == "__main__":
    ap = argparse.ArgumentParser(description="通用课程数据生成器")
    ap.add_argument("--course", required=True, help="课程根目录（含 maps/ 和各章；叠加课含 course.json 的 baseCourse）")
    ap.add_argument("--out", default=None, help="输出目录（默认 <course>/_web，dev 直跑用）")
    a = ap.parse_args()
    course = os.path.abspath(a.course)
    roots = course_roots(course)
    out = a.out or os.path.join(course, "_web")
    maps, content, manifest = generate(roots, out)
    print("manifest: %d 章 | 有demo %d" % (
        len(manifest), sum(1 for e in manifest.values() if e["demo"])))
    print("maps:", list(maps.keys()), "| chapters:", len(content),
          "| roots:", [os.path.basename(r) for r in roots], "| out:", out)
