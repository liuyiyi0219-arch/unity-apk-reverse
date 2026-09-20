"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Battle = void 0;
class Battle {
    static CalcDamage(atk, def, critMul = 1.0) {
        const base = atk * atk / (atk + def);
        return Math.floor(base * critMul);
    }
    static DailyReward(day) {
        const gold = 100 + day * 50;
        return day % 7 === 0 ? gold * 2 : gold;
    }
}
exports.Battle = Battle;
