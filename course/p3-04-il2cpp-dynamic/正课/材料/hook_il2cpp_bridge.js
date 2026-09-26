// ============================================================
// 3.4 · 路线 B：frida-il2cpp-bridge 名字定位 hook（推荐）
// 不依赖 RVA，跨 build 稳；自动处理值类型 / string / MethodInfo*。
//
// 准备：
//   npm i frida-il2cpp-bridge
//   用 frida-compile 把本文件打成单文件： frida-compile hook_il2cpp_bridge.js -o _compiled.js
//   frida -U -f com.course.demo -l _compiled.js
// （或用支持 import 的 frida 版本直接跑。）
// ============================================================
import "frida-il2cpp-bridge";

Il2Cpp.perform(() => {
    const image = Il2Cpp.domain.assembly("Assembly-CSharp").image;
    const GameLogic = image.class("GameLogic");
    const License   = image.class("License");

    // ---- CalcDamage：先跑原逻辑验证静态还原，再改返回值 ----
    GameLogic.method("CalcDamage").implementation = function (atk, def, critMul) {
        // this.method(...).invoke 调用原实现，拿到“未改前”的真实返回值
        const real = GameLogic.method("CalcDamage").invoke(atk, def, critMul);
        console.log(`[CalcDamage] atk=${atk} def=${def} crit=${critMul} => 原=${real}  改成 99999`);
        return 99999;                       // 伤害爆表
    };

    // ---- IsActivated：打印传入的激活码后恒 true ----
    License.method("IsActivated").implementation = function (code) {
        // code 是 Il2Cpp.String，.content 直接是 JS 字符串
        console.log(`[IsActivated] code="${code.content}" => true`);
        return true;
    };

    // ---- GoldReward：只观察 ----
    GameLogic.method("GoldReward").implementation = function (enemyLevel) {
        const gold = GameLogic.method("GoldReward").invoke(enemyLevel);
        console.log(`[GoldReward] level=${enemyLevel} => ${gold}`);
        return gold;
    };

    console.log("[+] hooks installed (frida-il2cpp-bridge 路线).");
});
