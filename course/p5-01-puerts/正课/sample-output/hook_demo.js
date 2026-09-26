// 4.10 · JS 级 monkey-patch：hook 真实业务函数（和 4.4 的 Lua 同理，JS 也是动态语言）
const { Battle } = require("./Battle.js");
console.log("原始  CalcDamage(180,30,1.5) =", Battle.CalcDamage(180, 30, 1.5));

const orig = Battle.CalcDamage;
Battle.CalcDamage = function (atk, def, crit) {
    const real = orig(atk, def, crit);
    console.log(`[hook] 入参 atk=${atk} def=${def} crit=${crit} 原返回=${real} → 改成 99999`);
    return 99999;
};
console.log("hook后 CalcDamage(180,30,1.5) =", Battle.CalcDamage(180, 30, 1.5));
