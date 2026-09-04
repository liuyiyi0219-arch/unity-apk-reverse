-- 游戏入口：读英雄配置，用伤害公式算一次战斗，打印结果。
-- 由 LuaManager 在 require("Framework.Boot") 之后 require("Main")。

local Battle = require("Gameplay.Battle")

local hero = config.Get("Hero", 1)    -- Knight
local target = config.Get("Hero", 3)  -- Mage
local dmg = Battle.CalcDamage(hero.Atk, target.Def, 1.5)

print(string.format("[Main] %s (atk=%d) hits %s (def=%d) for %d damage",
    hero.Name, hero.Atk, target.Name, target.Def, dmg))
