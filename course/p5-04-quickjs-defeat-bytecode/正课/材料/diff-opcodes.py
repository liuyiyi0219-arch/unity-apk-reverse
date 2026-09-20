#!/usr/bin/env python3
# 5.5 差分恢复 opcode 映射：同一份源码用【标准 qjsc】和【游戏魔改 qjsc】各编一份字节码，
# 逐字节比对——同源 → 除 4 字节校验和外，只有 opcode 字节被换了号，位置对齐，直接读出置换。
# 用法: diff-opcodes.py sample.stock.qjbc sample.op.qjbc
import sys
s=open(sys.argv[1],"rb").read(); o=open(sys.argv[2],"rb").read()
if len(s)!=len(o):
    print("警告：两文件不等长，可能不是同源或魔改不止重排"); 
perm={}
for off,(x,y) in enumerate(zip(s,o)):
    if x!=y and not (1<=off<=4):     # 排除头部第 1..4 字节的校验和
        perm[(x,y)]=perm.get((x,y),0)+1
print("恢复出的 opcode 置换（标准编号 → 魔改编号，按出现次数排序）:")
for (x,y),c in sorted(perm.items(),key=lambda kv:-kv[1]):
    print(f"  标准 {x:3d}  →  魔改 {y:3d}   (在样本里出现 {c} 次)")
print("\n把 secret.op.qjbc 每条指令的 opcode 按【魔改→标准】换回，或用带此映射的运行时(qjbc-run-op)跑。")
