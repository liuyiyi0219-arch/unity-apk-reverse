local Battle = {}

function Battle.CalcDamage(atk, def, critMul)
  local base = atk * atk / (atk + def)
  return math.floor(base * (critMul or 1))
end

return Battle
