# 3.8 作业：脱 native 壳，读出 flag

## 题干

一个 APK 里的 native 库被加壳了：静态 Ghidra 反出来是乱码、`strings` 抠不到。flag 藏在其中一个函数 **`computeFlag`** 的机器码里——它焊了一段 **SM4（国密 GB/T 32907）密文 + 128 位 key**，运行时用标准 SM4 解密出 flag，**只存在于加密的 `.text`**（不在 `.rodata`、不在任何明文串里）。启动时壳会在内存里把 `.text` 自解密回来，所以 App 能正常跑。目标：把它脱出来，认出 SM4，算出 flag。

## 靶包

点面板 **⬇ 下载材料** 拿到 `native-shell.apk`（package `com.chico.elfencrytest`，自造教学样本）：

| 文件 | 是什么 |
|---|---|
| `lib/arm64-v8a/libelfencrytest.so` | 用开源 [ELFEncryTest](https://github.com/nuloperrito/ELFEncryTest) 把 `.text` 段 AES-CTR 加密了。启动时 `.so` 里的 `.preload` 构造函数会 dlopen `libcryutil.so` 把 `.text` 在内存里解密回来 |

## 怎么交

回爬塔这个节点 → **✍ 做作业** → 填 flag → **▶ 运行批改**。
