# 3.7 作业：脱 metadata 的整体加密，三条路任选一条

## 题干

一个 APK 里的 `global-metadata.dat` 被**整体加密**了：magic 不是 `AF1BB1FA`、`strings` 抠不到 flag。flag 是 metadata 里的一个 C# 字符串。目标：把它解回明文，读出里面的 flag。**具体是什么加密算法、密钥藏在哪，得你自己认、自己挖**。

## 靶包

点面板 **⬇ 下载材料** 拿到 `metadata-enc-kit.zip`，解开有：

| 文件 | 是什么 |
|---|---|
| `metadata-encrypted.apk` | 靶包（package `com.course.hdrflag`）。`global-metadata.dat` 被整体加密；加密用的**密钥（可能还有 IV）作为常量编进了 `libil2cpp.so`** |
| `decrypt_skeleton.py` | 静态找密钥法的离线解密脚本骨架——Ghidra 挖出 key/iv 填进 `KEY`/`IV` 再跑；也可加 `--scan` 让它自己从 `.rodata` 扫出来（顺带演示静态明文 key 有多脆） |
| `dump_metadata.js` | 内存 dump 法的 frida 脚本 |

## 怎么交

三条路拿到的是同一份明文 metadata、同一个 flag。回爬塔这个节点 → **✍ 做作业** → 填 flag → **▶ 运行批改**。
