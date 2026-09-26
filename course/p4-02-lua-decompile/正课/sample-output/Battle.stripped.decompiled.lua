local L0_1, L1_1
L0_1 = {}

function L1_1(A0_2, A1_2, A2_2)
  local L3_2, L4_2, L5_2
  L3_2 = A0_2 * A0_2
  L4_2 = A0_2 + A1_2
  L3_2 = L3_2 / L4_2
  L4_2 = math
  L4_2 = L4_2.floor
  L5_2 = A2_2 or L5_2
  if not A2_2 then
    L5_2 = 1
  end
  L5_2 = L3_2 * L5_2
  return L4_2(L5_2)
end

L0_1.CalcDamage = L1_1
return L0_1
