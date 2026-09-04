-- 框架启动：初始化全局 config 表，载入所有 CSV 转出来的配置表。
-- 参考 W3 Client/Binary/Src/Framework/Boot 的职责精简而来。

config = config or {}

-- 载入配表（每个文件往 config 挂 XxxTable / XxxHeaderTable）
require("Hero")
require("Item")

-- 按主键取一行配置
function config.Get(tableName, id)
    local t = config[tableName .. "Table"]
    return t and t[id] or nil
end

print(string.format("[Boot] config loaded: Hero=%d Item=%d", #config.HeroTable, #config.ItemTable))
