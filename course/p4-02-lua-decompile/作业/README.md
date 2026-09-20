# 4.2 作业：反编译 LuaJIT 字节码，读出 flag

## 题干

材料是爬塔游戏的 **battle 脚本编成的 LuaJIT 字节码**，flag 编码在字节码里的一张字节表里——`strings` 抠不到，必须真反编译回源码、再按逻辑还原才读得到。把它反编译回来，还原出 flag。

## 材料

下载 `luajit-bytecode-kit.zip`，里面 `mystery.ljbc` 是爬塔 battle 脚本编成的 **strip 过的 LuaJIT 2.1 字节码**（自造样本，一小段"字符串编码进字节表、运行时解"的常见写法）。

## 怎么交

```bash
check.exe FLAG{...}
```

对了它吐 flag 🚩；错了会提示"这是 LuaJIT、别用 unluac"。
