# unidbg-emulate-kit —— 用 arm64 unidbg 模拟执行一段 bin，跑出 flag

## 里面有什么

| 文件 | 是什么 |
|---|---|
| `flag_gen.bin` | 一段 **arm64 机器码**（76 字节）。里面没有明文 flag（`strings` 抠不到）——flag 是它**跑起来**才算出来、写进输出缓冲的。 |
| `unidbg-run/` | 一个能直接跑的 **unidbg Maven 工程**（`pom.xml` + `BinEmu.java`）。 |

## 怎么跑

`flag_gen.bin` 是裸机器码（不是 ELF/.so），所以用 unidbg 的**低层路**：mmap 一段可执行内存写进码 → eFunc 从码地址跑到 `ret` → 读输出缓冲。`BinEmu.java` 已经写好这套，直接：

```bash
cd unidbg-run
mvn -q compile exec:java -Dexec.args="../flag_gen.bin"
# 输出： FLAG = FLAG{...}
```

需要 JDK 11+ 和 Maven（unidbg 自动从 jitpack 拉）。把打印的 `FLAG{...}` 交给批改器。

> 不想装 Java？unidbg 底层就是 Unicorn，等价的 12 行 Python（`pip install unicorn`）也能跑出同一个 flag——见 `答案.md`。但这一章练的就是 unidbg，推荐走上面的工程。
