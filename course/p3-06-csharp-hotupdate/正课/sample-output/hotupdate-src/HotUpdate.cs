// HotUpdate.cs —— 热更程序集里的游戏逻辑。
// 这份代码【不在】AOT 的 libil2cpp.so / global-metadata.dat 里，
// 而是运行时从热更 DLL 加载（HybridCLR 解释执行 / ILRuntime 解释）。
// 运营可以在线下发新版本，绕过商店审核、避免强更。
using System;

namespace Course.Hotfix
{
    public static class HotfixLogic
    {
        // 运营下发的“新伤害公式 v2”——把防御从线性减免改成饱和曲线 + 暴击加成。
        public static int CalcDamageV2(int atk, int def, float crit)
        {
            float reduction = def / (def + 100f);
            float dmg = atk * (1f - reduction) * crit;
            return (int)dmg;
        }

        // 签到奖励：第 7 天翻倍。
        public static int DailyReward(int day)
        {
            int baseGold = 100 + day * 50;
            return (day % 7 == 0) ? baseGold * 2 : baseGold;
        }
    }
}
