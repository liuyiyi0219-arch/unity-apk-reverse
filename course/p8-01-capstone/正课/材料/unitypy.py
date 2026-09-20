# -*- coding: utf-8 -*-
# 第 7.1 课（综合实战）辅助脚本：把【去 XOR 后的明文 UnityFS】里的 Lua TextAsset 全提出来。
#
#   extract <decrypted.ab> <outdir> <out.json>
#       用 UnityPy load 明文 UnityFS → 把每个 TextAsset 的源码写到 outdir/<name>，
#       并写一份 JSON 清单（对象数 / 类型分布 / 每个 TextAsset 的字节数 + sha256）。
#
# 这一步等价于 4.1 / 6.1 课的做法：Lua 被打进 AssetBundle，标准工具就是 UnityPy。
# 没装 UnityPy 就写 {unitypy:false}，Go 侧回退到仓库预生成的 extracted-lua/ 讲解。
#
# 红线：这里对 bundle 的一切操作都是【标准 UnityFS 处理】，不涉及任何厂商私有布局或算法。
import sys, os, json, hashlib
from collections import Counter


def _load_unitypy():
    import UnityPy
    UnityPy.config.FALLBACK_UNITY_VERSION = "2022.3.62f2"
    return UnityPy


def _script_bytes(d):
    s = getattr(d, "m_Script", b"")
    if isinstance(s, str):
        return s.encode("utf-8", "surrogateescape")
    return bytes(s)


def cmd_extract(inp, outdir, out):
    try:
        UnityPy = _load_unitypy()
    except Exception as e:
        data = {"unitypy": False, "import_error": str(e)}
        _write(out, data); print(json.dumps(data, ensure_ascii=False)); return 0
    import UnityPy as _u
    os.makedirs(outdir, exist_ok=True)
    env = _u.load(inp)
    objs = list(env.objects)
    texts = {}
    for o in objs:
        if o.type.name == "TextAsset":
            d = o.read()
            raw = _script_bytes(d)
            name = d.m_Name
            # UnityPy 有时把 .lua 的名字带上后缀，有时不带——统一补 .lua 方便阅读。
            fn = name if name.endswith(".lua") else name + ".lua"
            with open(os.path.join(outdir, fn), "wb") as f:
                f.write(raw)
            texts[fn] = {"bytes": len(raw), "sha256": hashlib.sha256(raw).hexdigest()}
    data = {"unitypy": True, "version": _u.__version__,
            "num_objects": len(objs),
            "types": dict(Counter(o.type.name for o in objs)),
            "textassets": texts}
    _write(out, data); print(json.dumps(data, ensure_ascii=False)); return 0


def _write(out, data):
    with open(out, "w", encoding="utf-8") as f:
        json.dump(data, f, ensure_ascii=False, indent=2)


def main():
    a = sys.argv[1:]
    if not a:
        print("usage: _unitypy.py extract <decrypted.ab> <outdir> <out.json>"); return 2
    if a[0] == "extract":
        return cmd_extract(a[1], a[2], a[3])
    print("unknown cmd", a[0]); return 2


if __name__ == "__main__":
    sys.exit(main())
