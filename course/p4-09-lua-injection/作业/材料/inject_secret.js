// 4.9 · hook libtolua.so!lua_pcall 抓活着的 lua_State*，注入 Lua 读运行时全局 __secret（=flag）
//
// 原理：游戏每帧走一次 lua_pcall（C# 的 heartbeat），onEnter 的 args[0] 就是当前 lua_State*。
// 抓到它以后，用同一个 state：luaL_loadstring(L,"return __secret") 把一段 chunk 压栈编译，
// lua_pcall(L,0,1,0) 跑它，lua_tolstring(L,-1,&len) 把返回值（flag）读回来。
//
// 用法： frida -U -f com.course.lua49 -l inject_secret.js   （或 attach 已在跑的进程）
//   通道见 0.2（本靶设备用 florida attach）。lua_pcall 每帧触发，随时 attach 都抓得到。

function hook() {
    var m = Process.findModuleByName('libtolua.so');
    if (m === null) { setTimeout(hook, 300); return; }

    var p_pcall       = m.findExportByName('lua_pcall');
    var p_loadstring  = m.findExportByName('luaL_loadstring');
    var p_tolstring   = m.findExportByName('lua_tolstring');
    var p_settop      = m.findExportByName('lua_settop');
    if (!p_pcall || !p_loadstring || !p_tolstring || !p_settop) {
        send('[-] missing export(s): pcall=' + p_pcall + ' loadstring=' + p_loadstring +
             ' tolstring=' + p_tolstring + ' settop=' + p_settop);
        return;
    }

    // int  luaL_loadstring(lua_State*, const char* s)
    var luaL_loadstring = new NativeFunction(p_loadstring, 'int', ['pointer', 'pointer']);
    // int  lua_pcall(lua_State*, int nargs, int nresults, int errfunc)
    var lua_pcall = new NativeFunction(p_pcall, 'int', ['pointer', 'int', 'int', 'int']);
    // const char* lua_tolstring(lua_State*, int idx, size_t* len)
    var lua_tolstring = new NativeFunction(p_tolstring, 'pointer', ['pointer', 'int', 'pointer']);
    // void lua_settop(lua_State*, int idx)
    var lua_settop = new NativeFunction(p_settop, 'void', ['pointer', 'int']);

    var done = false;
    var busy = false;   // 防重入：我们注入的 lua_pcall 会再次触发本 hook
    var code = Memory.allocUtf8String('return __secret');

    Interceptor.attach(p_pcall, {
        onEnter: function (args) {
            if (done || busy) return;
            var L = args[0];
            if (L.isNull()) return;
            busy = true;
            try {
                // 在同一个活着的 lua_State 上注入：编译并跑 "return __secret"
                var rc = luaL_loadstring(L, code);           // 压入 chunk（栈顶多 1）
                if (rc === 0) {
                    var pr = lua_pcall(L, 0, 1, 0);          // 跑它，返回值压栈顶
                    if (pr === 0) {
                        var s = lua_tolstring(L, -1, NULL);
                        if (!s.isNull()) {
                            var flag = s.readUtf8String();
                            send('[+] __secret (flag) = ' + flag);
                            done = true;
                        }
                    } else {
                        send('[-] injected pcall rc=' + pr);
                    }
                    lua_settop(L, -2);                       // 弹掉我们压的返回值，别扰乱游戏栈
                } else {
                    send('[-] luaL_loadstring rc=' + rc);
                }
            } catch (e) {
                send('[-] inject err ' + e);
            } finally {
                busy = false;
            }
        }
    });
    send('[*] hooked libtolua.so!lua_pcall @ ' + p_pcall + ' —— 等一帧 heartbeat 触发即读回 __secret');
}
hook();
