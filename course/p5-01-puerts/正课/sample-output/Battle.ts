// puerts 脚本层：游戏逻辑写在 TS 里，编译成 JS 后跑在 V8 上。
export class Battle {
    // 物理伤害：攻击力经防御减免，再乘暴击倍率。
    static CalcDamage(atk: number, def: number, critMul: number = 1.0): number {
        const base = atk * atk / (atk + def);
        return Math.floor(base * critMul);
    }
    static DailyReward(day: number): number {
        const gold = 100 + day * 50;
        return day % 7 === 0 ? gold * 2 : gold;
    }
}
