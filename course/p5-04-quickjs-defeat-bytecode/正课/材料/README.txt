5.4 作业材料 · 破「改字节码/魔改 VM 的 QuickJS 字节码」

secret.op.qjbc    —— 靶子：opcode 被重排的 QuickJS 字节码（跑起来算法拼出 flag）
sample.js         —— 已知源码（差分用）
sample.stock.qjbc —— sample.js 的标准 qjsc 产物
sample.op.qjbc    —— sample.js 的魔改 qjsc 产物
diff-opcodes.py   —— 差分助手（逐字节比、排除校验和、读出 opcode 置换）
qjbc-run.exe      —— 标准 QuickJS 运行时（跑 secret.op 会崩在执行阶段）
qjbc-run-op.exe   —— 映射正确的运行时（对应游戏 libpuerts.so 里的 QuickJS）
qjs-op-src/       —— 魔改 recipe（opcode 互换的那几行）

破法：
  1. qjbc-run.exe secret.op.qjbc            → 执行异常（version 正常、崩在执行 = 改 opcode）
  2. python diff-opcodes.py sample.stock.qjbc sample.op.qjbc   → 读出 opcode 置换
  3. qjbc-run-op.exe secret.op.qjbc         → FLAG{...}
  4. check.exe FLAG{...}
