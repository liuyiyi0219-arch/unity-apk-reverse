// ============================================================
// 3.4 · 路线 A：RVA 直定位 hook（对着 CompleteDemo 的 C# 埋点）
// 用法：frida -U -f com.course.demo -l hook_by_rva.js
//
// RVA 来自 3.1 的 dump.cs（每次 build 会变，仅对当前这个包有效）。
// 演示程序 lesson-3.4.exe 会把当前包的真实 RVA 填进这里。
// ============================================================

// GameLogic / License 三个静态方法的 RVA（16 进制）
const RVA = {
    CalcDamage:  0x600DB8,   // static int CalcDamage(int atk, int def, float critMul)
    GoldReward:  0x600E4C,   // static int GoldReward(int enemyLevel)
    IsActivated: 0x600E5C,   // static bool IsActivated(string code)
};

function hookWhenReady() {
    // frida 17 移除了 Module.findBaseAddress，用 Process.findModuleByName(...).base
    const mod = Process.findModuleByName('libil2cpp.so');
    if (mod === null) { setTimeout(hookWhenReady, 500); return; } // 等库加载
    const base = mod.base;
    console.log('[+] libil2cpp.so base = ' + base);

    // ---- CalcDamage：读入参 + 改返回值 ----
    // 坑：atk/def 是整型→x0/x1(args[0]/args[1])；critMul 是 float→s0，不在 args[] 里！
    Interceptor.attach(base.add(RVA.CalcDamage), {
        onEnter(args) {
            this.atk = args[0].toInt32();
            this.def = args[1].toInt32();
            // 浮点参数从浮点寄存器读（ARM64）
            this.crit = this.context.s0;   // 若架构不同/读不到，可省
        },
        onLeave(ret) {
            console.log(`[CalcDamage] atk=${this.atk} def=${this.def} crit=${this.crit} ` +
                        `=> 原=${ret.toInt32()}  改成 99999`);
            ret.replace(ptr(99999));       // 伤害爆表
        }
    });

    // ---- IsActivated：恒返回 true（任何激活码都通过）----
    Interceptor.attach(base.add(RVA.IsActivated), {
        onLeave(ret) {
            if (ret.toInt32() === 0) console.log('[IsActivated] 原=false -> 改 true');
            ret.replace(ptr(1));
        }
    });

    // ---- GoldReward：只观察，不改 ----
    Interceptor.attach(base.add(RVA.GoldReward), {
        onEnter(args) { this.lv = args[0].toInt32(); },
        onLeave(ret)  { console.log(`[GoldReward] level=${this.lv} => ${ret.toInt32()}`); }
    });

    console.log('[+] hooks installed (RVA 路线).');
}

hookWhenReady();
