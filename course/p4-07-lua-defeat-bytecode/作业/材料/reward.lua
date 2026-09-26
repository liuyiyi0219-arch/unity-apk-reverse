-- 掉落结算：按权重从奖池里挑价值最高的物品，做 count 次。
local Reward = {}

-- 从 pool 里挑 count 件：每件都在池中扫一遍，取 weight*luck 最大的。
---@param pool  table  物品池，元素形如 {id=.., weight=..}
---@param count number 抽取次数
---@param luck  number 幸运系数（放大权重）
---@return table 抽到的物品 id 列表
function Reward.Roll(pool, count, luck)
    local result = {}
    for i = 1, count do
        local pick = pool[1]
        local best = 0
        for j = 1, #pool do
            local item = pool[j]
            local weight = item.weight * luck
            if weight > best then
                pick = item
                best = weight
            end
        end
        result[i] = pick.id
    end
    return result
end

return Reward
