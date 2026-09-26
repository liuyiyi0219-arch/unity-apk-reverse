import sys, os, time, frida
print("frida", frida.__version__)
pid = int(sys.argv[1])
# -l <script.js>：注入脚本路径；不带则默认读同目录的 inject_secret.js
js_path = os.path.join(os.path.dirname(os.path.abspath(__file__)), "inject_secret.js")
if "-l" in sys.argv:
    js_path = sys.argv[sys.argv.index("-l") + 1]
js = open(js_path, "r", encoding="utf-8").read()
dev = frida.get_usb_device(timeout=10)
sess = dev.attach(pid)
scr = sess.create_script(js)
got = {"flag": None}
def on_msg(m, data):
    if m.get("type")=="send":
        print("[frida]", m["payload"])
        if "flag) = " in str(m["payload"]):
            got["flag"] = m["payload"].split("flag) = ",1)[1]
    elif m.get("type")=="error":
        print("[err]", m.get("description"))
scr.on("message", on_msg)
scr.load()
t0=time.time()
while time.time()-t0 < 12 and not got["flag"]:
    time.sleep(0.3)
print("RESULT_FLAG=", got["flag"])
