-- 4.4 · Lua 级 monkey-patch 演示：hook 真实业务函数 Battle.CalcDamage
package.path = "sample-output/?.lua;" .. package.path
local Battle = require("Battle")

-- 原始调用
print(string.format("原始  CalcDamage(180,30,1.5) = %d", Battle.CalcDamage(180, 30, 1.5)))

-- ---- 注入 hook（Lua 是动态的，直接重赋值函数即可）----
local orig = Battle.CalcDamage
Battle.CalcDamage = function(atk, def, crit)
    local real = orig(atk, def, crit)            -- 先跑原逻辑（验证/观察）
    print(string.format("[hook] 入参 atk=%d def=%d crit=%.1f 原返回=%d → 改成 99999", atk, def, crit, real))
    return 99999                                  -- 改返回值
end

-- hook 后调用
print(string.format("hook后 CalcDamage(180,30,1.5) = %d", Battle.CalcDamage(180, 30, 1.5)))
