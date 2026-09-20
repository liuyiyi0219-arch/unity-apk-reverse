sigcheck —— 重打包/签名校验 关键代码（自制讲解样例，演示主流加固的通用做法）

■ 目的：把"重打包校验到底怎么做"读明白——盯签名证书的 tbsCertificate 指纹，
  native 自解 zip + 手写 DER 状态机取证书，双路交叉佐证，不符就踢桌面+exit。

■ 读代码顺序（都在 sigcheck.cpp 里）：
  1) der_parse / tlv_len_in_bounds      手写 DER TLV 解析（含 32 位长度域溢出防护）
  2) pkcs7_extract_certificate / asn1_locate_tbs   从签名块定位到 tbsCertificate
  3) ApkZip                             不经 PackageManager，自己读 zip 找 META-INF/*.RSA
  4) verify_apk_signature               双路取证书 → 只哈希 tbsCertificate → 比对
  5) punish_kick_home_and_exit          不符时的统一处置

■ 构建 + 运行（PC 上）：
    g++ -std=c++17 -O2 -o sigcheck sigcheck.cpp
    # 或 cmake -B build && cmake --build build
    ./sigcheck sample-signed.apk x            # 先看 observed 指纹
    ./sigcheck sample-signed.apk <observed>   # 用它当 expected → MATCH
  （sample-signed.apk 是配套的 v1 签名样例包，签名块存成 stored 好让 PC 版直接读；
    真机上 native 接真 zlib，能读 deflate 压过的签名块。）

■ flag 就藏在 sigcheck.cpp 的注释里——读到"定位 tbsCertificate"那段就看到了。
