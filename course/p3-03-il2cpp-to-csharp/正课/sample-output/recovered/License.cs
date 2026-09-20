// ============================================================
// 逆向还原产物（3.3）—— 从 RVA 0x600E5C 的伪 C 翻译回来
// 关键点：
//   · sVar4 = sVar4*0x1f + sVar1  →  sum = sum*31 + c
//   · sVar4 是 short（16-bit）→ 反推出源码的 & 0xFFFF 截断
//   · String.Concat(code, PTR) 的字面量参数 → salt 常量 "COURSE-2026"
//   · return sVar4 == 0x4a3b  →  == 0x4A3B
// sum/c/Salt 这些名字是逆向补的；魔数 0x4A3B、salt 串是从 binary 读到的真值。
// ============================================================
public static class License
{
    private const string Salt = "COURSE-2026";

    public static bool IsActivated(string code)
    {
        if (string.IsNullOrEmpty(code)) return false;
        int sum = 0;
        foreach (char c in (code + Salt))
            sum = (sum * 31 + c) & 0xFFFF;
        return sum == 0x4A3B;
    }
}
