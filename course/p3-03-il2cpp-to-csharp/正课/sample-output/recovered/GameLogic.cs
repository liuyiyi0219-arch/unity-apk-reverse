// ============================================================
// 逆向还原产物（3.3 的“真 C#”）—— 从 3.2 的伪 C 翻译回来
// 来源：libil2cpp.so RVA 0x600DB8 / 0x600E4C（符号注入 + 模式识别）
// 说明：签名/类名来自 metadata（无损）；方法体逻辑高保真；
//       局部变量名（baseDmg 等）是逆向补的、非原文。
// ============================================================
using UnityEngine;

public static class GameLogic
{
    // 伪 C: critMul = (atk*atk)/(def+atk) * critMul; (int)+INFINITY 判断 = FloorToInt
    public static int CalcDamage(int atk, int def, float critMul)
    {
        float baseDmg = (float)atk * atk / (atk + def);
        return Mathf.FloorToInt(baseDmg * critMul);
    }

    // 伪 C: param_1*param_1*3 + 10
    public static int GoldReward(int enemyLevel)
    {
        return 10 + enemyLevel * enemyLevel * 3;
    }
}
