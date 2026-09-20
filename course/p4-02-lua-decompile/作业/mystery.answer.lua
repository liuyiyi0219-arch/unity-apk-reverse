-- Buff 系统：同名叠加、封顶、总加成结算。
-- （这是作业答案源码，只用来生成 mystery.luac；不发给学员。）
local Buff = {}

local MAX_STACK = 5

-- 施加一个 buff：列表里有同 id 就叠加（封顶 MAX_STACK），没有就新建。
function Buff.Apply(list, id, power)
    for i = 1, #list do
        local b = list[i]
        if b.id == id then
            b.stack = b.stack + 1
            if b.stack > MAX_STACK then
                b.stack = MAX_STACK
            end
            b.power = power
            return b.stack
        end
    end
    list[#list + 1] = { id = id, stack = 1, power = power }
    return 1
end

-- 结算总加成：sum(power * stack)。
function Buff.TotalBonus(list)
    local sum = 0
    for i = 1, #list do
        local b = list[i]
        sum = sum + b.power * b.stack
    end
    return sum
end

return Buff
