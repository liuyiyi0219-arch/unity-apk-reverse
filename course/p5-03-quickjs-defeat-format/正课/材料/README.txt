5.3 作业材料 · 破「改格式的 QuickJS 字节码」

secret.fmt.qjbc   —— 靶子：被魔改 version 头的 QuickJS 字节码（跑起来算法拼出 flag）
qjbc-run.exe      —— 标准 QuickJS 字节码运行时（认标准版本 0x1b）
qjbc-run-mod.exe  —— 魔改运行时（认 0x42 的那个 VM，对应真实游戏 libpuerts.so 里的 QuickJS）
qjs-mod-src/      —— 魔改 recipe（一行 BC_VERSION 改动）+ qjbc-run.c 源

破法：
  1. qjbc-run.exe secret.fmt.qjbc        → SyntaxError: invalid version (66 expected=27)
  2. 把文件第 1 字节 0x42 改回 0x1b（十六进制编辑器，或 printf '\x1b' | dd ... seek=0 count=1 conv=notrunc）
  3. qjbc-run.exe secret.fixed.qjbc      → FLAG{...}
  4. check.exe FLAG{...}

（也可以直接 qjbc-run-mod.exe secret.fmt.qjbc —— 魔改运行时本来就认 0x42。）
