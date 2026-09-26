// ============================================================
// 4.4 · Frida native hook：拦 luaL_loadbuffer（ToLua/libtolua.so，Lua 5.1.5）
// 用途：
//   ① dump 解密后的脚本 —— 每个 Lua chunk 在【编译前】都过 luaL_loadbuffer，
//      而此刻它已经是解密的明文（AB 的 XOR/整体加密在更早的加载层就解掉了）。
//      → 一次 hook，把游戏所有 Lua 脚本（解密态）落盘。相当于 3.8 之于 metadata。
//   ② 注入代码 —— 改写 buffer，或额外 loadbuffer 你自己的 Lua。
//
// frida 通道见 3.4（adb forward + attach；本机 spawn 超时用 attach）：
//   frida -H 127.0.0.1:27042 -p $(adb shell pidof com.course.demo) -l hook_lua_load.js
//   adb pull /data/local/tmp/lua_dump/
// ============================================================

// int luaL_loadbuffer(lua_State *L, const char *buff, size_t sz, const char *name)
function hook() {
    const mod = Process.findModuleByName('libtolua.so'); // xLua 换成 libxlua.so
    if (mod === null) { setTimeout(hook, 500); return; }
    const addr = mod.getExportByName('luaL_loadbuffer'); // 5.1；5.3+ 是 luaL_loadbufferx
    let n = 0;
    Interceptor.attach(addr, {
        onEnter(args) {
            const buff = args[1];
            const sz = args[2].toInt32();
            const name = args[3].isNull() ? ('chunk_' + (n++)) : args[3].readCString();
            const src = buff.readByteArray(sz);
            // 落盘：每个脚本一份（解密态）
            const safe = name.replace(/[^\w.\-]/g, '_');
            const f = new File('/data/local/tmp/lua_dump/' + safe + '.lua', 'wb');
            f.write(src); f.close();
            send('[dump] ' + name + '  (' + sz + ' bytes)');

            // ② 注入示例：给某个脚本尾部追加一段 hook（monkey-patch）——可选
            // if (name.indexOf('Battle') >= 0) {
            //     const patch = '\n local o=Battle.CalcDamage; Battle.CalcDamage=function(a,b,c)'
            //                 + ' print("[injected] "..a..","..b) return 99999 end\n';
            //     // 用一个新 buffer 替换 args[1]/args[2] 即可注入（略）
            // }
        }
    });
    send('[+] hooked luaL_loadbuffer @ ' + addr + '，等待脚本加载…（进游戏触发）');
}
hook();
