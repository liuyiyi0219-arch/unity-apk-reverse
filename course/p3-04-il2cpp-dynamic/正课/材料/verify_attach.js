// ============================================================
// 3.4 · 真机验证脚本（attach 版）——本课在 Pixel 4 上实测通过
// 本机 spawn 超时，改用 attach 到已启动进程；直接用 NativeFunction 调方法，
// 既【验证静态还原】(GoldReward=10+lv*lv*3) 又【验证 hook】(改返回值)。
//
// 跑法（走 TCP，不走易挂的 USB）：
//   adb forward tcp:27042 tcp:27042
//   adb shell 'su -c "/data/local/tmp/frida-server &"'     # 版本要与 host frida 对齐
//   adb shell monkey -p com.course.demo 1                  # 先把游戏启动
//   frida -H 127.0.0.1:27042 -p $(adb shell pidof com.course.demo) -l verify_attach.js
// ============================================================
function run() {
    const mod = Process.findModuleByName('libil2cpp.so');   // frida 17 API
    if (mod === null) { setTimeout(run, 300); return; }
    const base = mod.base;
    console.log('[+] libil2cpp.so base = ' + base);

    // GoldReward(int enemyLevel, MethodInfo*) -> int  （纯算术，可从任意线程直接调）
    const gold = new NativeFunction(base.add(0x600E4C), 'int', ['int', 'pointer']);

    // 装 observe hook：读入参 + 读返回
    Interceptor.attach(base.add(0x600E4C), {
        onEnter(a) { this.lv = a[0].toInt32(); },
        onLeave(r) { console.log('  [hook] GoldReward(level=' + this.lv + ') -> ' + r.toInt32()); }
    });

    // 验证静态还原：10 + lv*lv*3
    console.log('[static] GoldReward(5)  = ' + gold(5, ptr(0)) + '   期望 85');
    console.log('[static] GoldReward(10) = ' + gold(10, ptr(0)) + '  期望 310');

    // 装 replace hook：把返回值改成 99999，再调一次
    Interceptor.attach(base.add(0x600E4C), { onLeave(r) { r.replace(ptr(99999)); } });
    console.log('[hook改] GoldReward(5)  = ' + gold(5, ptr(0)) + '   期望 99999');
}
run();

// 注：CalcDamage 用了 Mathf（会分配内存），从 frida 线程【直接调】需先
//   il2cpp_thread_attach(il2cpp_domain_get()) 注册线程，否则崩；
//   或直接用 frida-il2cpp-bridge（它自动处理线程/字符串/MethodInfo）。
//   若走 spawn 让游戏自己在启动时调 CalcDamage，则 Interceptor 观察不受此限。
