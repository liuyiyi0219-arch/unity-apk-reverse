5.5 作业材料 · 破「整体加密的 QuickJS 字节码」

skills.qjbc.enc     —— 靶子：AES-128-CBC 整体加密的 QuickJS 字节码
libgame_rodata.bin  —— 从 native 抠出的 .rodata（算法指纹 + 解密函数名 + 16B key + 16B IV）
qjbc-run.exe        —— 标准 QuickJS 运行时（跑解密后的字节码验证）
qjs-aes-src/        —— 加密 recipe + 源 skills.js

破法：
  1. strings libgame_rodata.bin              → AES-128-CBC + cipher_key/cipher_iv（各 16B）
  2. openssl enc -d -aes-128-cbc -K <keyhex> -iv <ivhex> -in skills.qjbc.enc -out skills.qjbc
  3. strings skills.qjbc | grep FLAG          （或 qjbc-run.exe skills.qjbc）
  4. check.exe FLAG{...}
