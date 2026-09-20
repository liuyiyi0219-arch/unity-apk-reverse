# 0.4 作业：用 arm64 unidbg 模拟执行一段 bin，跑出 flag

## 题干

材料里给你一段 **arm64 机器码** `flag_gen.bin`（76 字节，不是 ELF，就是一段码）。它里面**没有明文 flag**——`strings flag_gen.bin` 只会看到乱码。flag 是这段码**跑起来**才算出来、写进它的输出缓冲的。

你要做的：**在 PC 上用 unidbg（arm64）把这段 bin 模拟执行起来**，读出它写出来的 flag。全程不用真机——这正是 unidbg 的用处：把一段 native 码当黑盒在 PC 上跑，抓它的输出。

> 这段码就是个 leaf 函数 `gen(char* out)`：入参 `x0` 是输出缓冲，函数把 flag 逐字节解码写进 `out`、然后 `ret`。你给它一段可执行内存 + 一段输出缓冲，从码地址跑到 `ret`，读缓冲即可。

## 材料（`unidbg-emulate-kit.zip`）

| 文件 | 是什么 |
|---|---|
| `flag_gen.bin` | 76 字节 arm64 机器码，跑起来才吐 flag（明文不在文件里） |
| `unidbg-run/` | 现成的 unidbg Maven 工程（`pom.xml` + `BinEmu.java`），`mvn` 一跑就出 flag |

## 怎么交

```bash
cd unidbg-run
mvn -q compile exec:java -Dexec.args="../flag_gen.bin"     # 打印 FLAG = FLAG{...}
check.exe FLAG{...}
```

对了它吐通关信息；`strings` 抠不到、随便填也过不了——flag 只有真把 bin 跑起来才拿得到。
