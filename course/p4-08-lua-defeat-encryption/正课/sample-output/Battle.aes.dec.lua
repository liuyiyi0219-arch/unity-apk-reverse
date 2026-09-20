-- 战斗伤害公式（后续 Lua 逆向章节的目标之一）。
local Battle = {}

-- 物理伤害：攻击力经防御减免（防御曲线），再乘暴击倍率。
---@param atk number 攻击力
---@param def number 防御力
---@param critMul number 暴击倍率，默认 1.0
---@return number 取整后的伤害
function Battle.CalcDamage(atk, def, critMul)
    local base = atk * atk / (atk + def)
    return math.floor(base * (critMul or 1.0))
end

return Battle
