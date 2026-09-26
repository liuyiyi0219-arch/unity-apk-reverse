# 3.3　C# 还原真 C#

> 状态：🟢 已完成（正文 + 演示程序）｜演示程序：`正课/lesson-3.3.exe`｜参考产物：`正课/sample-output/`

---

## 一、本章的目的

3.2 读到了逻辑，但满屏 `FUN_`/`DAT_`/`param_N`。这一课把它翻成**带名字、可编译**的真 C#。核心技能是 IL2CPP 逆向的看家本领：**认出每种 C# 语法编译后的固定形态**（yield、泛型、lambda、属性、事件…），把它们折回源码。

### 先说清边界：反编译是**有损**的

编译是**单向压缩**，一路在丢信息：

- **丢名字**：局部变量名、参数名、注释全没了（`atk` → `param_2`）；
- **丢结构**：`for`/`while`/`foreach`/三元被拆成基本块 + 跳转，反编译器只能**猜**着重组；
- **丢语法糖**：`yield`/`async`/`lambda`/`using`/`lock` 被改写成状态机、闭包类、try-finally——那行糖没了，只剩展开后的样子；
- **丢部分类型**：`var`/`nameof`/内联常量编译期就化开了。

所以**"逆出逐字节一致的原文"不可能**。目标是**语义等价、可编译、可读**——不是复刻原文。

### 但 IL2CPP 的还原率**远高于**纯 C/C++

同样是 native 机器码，逆纯 C/C++ 常常靠猜；IL2CPP 是 **C# 的 AOT 编译**，留了三样纯 native 没有的东西：

| | 纯 C/C++ 逆向 | IL2CPP 逆向 |
|---|---|---|
| 符号（名字） | strip 后全无，靠猜 | **metadata 明文全有** |
| 类型 | 靠推断，常错 | **完整保留**（klass / 偏移 / 泛型） |
| 语法结构 | 优化后千变万化 | **固定 pattern，可套模板** |
| 还原产物 | 伪 C，勉强读 | **可编译的真 C#** |

**所以 IL2CPP 逆向的核心技能 = 认 pattern。**

---

## 二、操作指南

把伪 C 翻真 C# **分三步，每步性质不同，别一锅烩**：

### 第 1 步 · 符号注入（机械、确定性）

Il2CppDumper 除了 `dump.cs` 还产 **`script.json`**（`RVA → 方法名 / 类型 / 字段偏移` 的完整映射）。喂给配套 Ghidra/IDA 脚本，把 3.2 那坨匿名伪 C 一次性**重命名 + 套结构**：

```
FUN_00700db8      →  GameLogic$$CalcDamage
DAT_00cbca8b      →  GameLogic__cctor_finished   (类初始化标志)
*(param_1 + 0x40) →  this->_soundSources         (套上 struct 布局后)
```

纯机械、脚本干的。做完伪 C 从"匿名"变"具名"，可读性陡增。

### 第 2 步 · 逐行 idiom 翻译（局部、逐句）

把每行 il2cpp 惯用法翻回 C#：类初始化守卫删掉、`*(this+0x40)`→字段名、`get_X/set_X`→属性、vtable 间接调用→`x.Foo()`、`op_Addition`→`a+b`、`String.Format+装箱`→字符串内插……都是**行级的局部替换**（对着第三节的 (A)(B)(C)(E)(F)）。做完，一个"没有语法糖的普通方法"就还原了。

### 第 3 步 · 语法糖还原（结构级，**单独一步**）← 重点

遇到编译器生成的隐藏类（`<Method>d__N`、`<>c__DisplayClassN_M`…）**不能**按第 2 步逐行翻——它们是 `yield`/`async`/`lambda` 被编译器**改写成的整套状态机 / 闭包**，横跨"kicker 方法 + 隐藏类 + MoveNext 状态机"多个部分。要单独走一套（对着第三节的 (D)）：

1. **识别**：类名带 `<>` + 实现 `IEnumerator`/含 `MoveNext` + 带 `[IteratorStateMachine]`/`[AsyncStateMachine]` → 命中语法糖；
2. **配对**：把隐藏类和它的 kicker 父方法（去掉 `<>` 后同名）配上；
3. **解码**：读 `MoveNext` 的 `switch(<>1__state)`，每个 `current=X; state=N; return true` 解成一句 `yield return X`（async 按 `await` 边界解）；
4. **折叠 + 丢弃**：输出原始 `yield`/`async`/`lambda` 语法，**丢掉隐藏类本身**（源码里没有它）。

> **为什么必须单列一步**：第 2 步是"看懂一行"，第 3 步是"看懂一个跨类的状态机"——性质完全不同。混在一起做，几乎必然把状态机原样吐出来（IL2CPP 翻译最常见的错误）。语法糖还原是一项**独立能力**，当成独立步骤专门处理，还原质量都很高。
>
> **实操坑**：有些反编译器把 `<` `>` 转义成 Unicode（`<>c__DisplayClass7_0` 显示成 `_003C_003E_c__DisplayClass7_0`）——识别语法糖前先脚本转回标准写法，否则名字对不上、配不到 kicker。

---

## 三、C# 语法编译后长什么样（对着翻）

按"你写的 C# → 编译后的形态 → 怎么折回"看。标 ⭐ 的用**我们 demo 的真实标本**（[`GameBoot.cs`](../demo/CourseDemo/Assets/Scripts/Game/GameBoot.cs) 里就有）。(A) 是降噪、(B)(C)(E)(F) 是第 2 步逐行翻译、**(D) 是第 3 步语法糖还原——单独处理**。

### (A) 运行时套路——先降噪（3.2 已讲）
类初始化守卫、内存屏障、末尾隐形 `MethodInfo*`、GC 写屏障——**全是运行时噪声，翻译时删掉**。

### (B) 成员访问
| 你写的 | 编译后 | 折回 |
|---|---|---|
| `this._field` | `*(this + 0x40)`（偏移查骨架字段表） | `this._field` |
| `obj.Prop`（属性） | `get_Prop()` / `set_Prop(v)` + `<Prop>k__BackingField` | `T Prop { get; set; }` |
| `clip.Play()`（虚方法） | `(**(this+0x10 → klass+0x118))(this,…)` vtable 调用 | `clip.Play()` |
| `handler.Handle(e)`（接口） | `il2cpp_runtime_invoke(handler, IID, slot)` | `handler.Handle(e)` |
| `list[i]`（索引器） | `get_Item(list,i)` / `set_Item(list,i,v)` | `list[i]` |
| `a + b`（运算符重载） | `op_Addition(a,b)`（`op_Equality`/`op_Implicit`…） | `a + b` |

### (C) 泛型——IL2CPP 反而**更好认**
IL2CPP 是 AOT，对泛型**部分特化**：值类型每个 `T` 生成独立 body（`List<int>` 和 `List<float>` 是两份），引用类型共享 body（靠 `MethodInfo*` 区分）。所以 dump.cs 里直接看到 `List<int>`、`Dictionary<string, GameConfig>` 这样**带具体类型参数**的条目——泛型没被 Java 那样"擦除"。

```csharp
var list = new List<AudioClip>(); list.Add(clip);
// 编译后：List<AudioClip>__ctor(); List<AudioClip>__Add(list, clip, MethodInfo);
// 折回：  照写 new List<AudioClip>() + .Add(clip)
```

### (D) 编译器生成类——状态机是大头（重点）

**⭐ yield 迭代器** —— demo 的 `GameBoot.QuitAfterDelay` 就是活标本。你写的：
```csharp
private IEnumerator QuitAfterDelay(float seconds) {
    yield return new WaitForSeconds(seconds);
    Application.Quit();
}
```
编译后 dump.cs 多出隐藏类：
```csharp
[IteratorStateMachine(typeof(GameBoot.<QuitAfterDelay>d__2))]
private IEnumerator QuitAfterDelay(float seconds) { }   // kicker，body 只是 new 状态机
private sealed class GameBoot.<QuitAfterDelay>d__2 : IEnumerator<object>, IDisposable {
    private int    <>1__state;      // 当前状态
    private object <>2__current;    // yield 出去的值
    public  float  seconds;         // 捕获的参数
    private bool MoveNext() { … }   // switch(<>1__state)，逻辑全在这
}
```
**怎么认**：`<方法名>d__N` + 实现 `IEnumerator` + 有 `<>1__state`/`<>2__current` + `MoveNext` 是 switch。
**怎么折回**：`MoveNext` 里每个 `<>2__current=X; <>1__state=N; return true;` = 一句 `yield return X;`；`<>1__state=-1; return false;` = 迭代结束。把 case 顺次串起来就是原 `yield` 方法体。**别把 `<QuitAfterDelay>d__2` 当真类输出**。

**async/await** —— 同样 `<Method>d__N`，字段是 `AsyncTaskMethodBuilder <>t__builder` + `TAwaiter <>u__1`，每个 `AwaitUnsafeOnCompleted(...)` 是一个 `await` 边界，折回 `async`/`await` 线性流。

**⭐ lambda / 闭包** —— demo 的 `ShowToast` 里 `runOnUiThread(() => {...})` 捕获 `text`/`activity`，编译成 `GameBoot.<>c__DisplayClass3_0`：
```csharp
private sealed class GameBoot.<>c__DisplayClass3_0 {
    public string text;                  // 捕获的外层局部
    public AndroidJavaObject activity;
    internal void <ShowToast>b__0() { … }// lambda 的方法体
}
```
**怎么认**：`<>c__DisplayClassN_M`（有捕获）或 `<>c`（无捕获、单例），配 `<方法名>b__M`。
**怎么折回**：把 `<ShowToast>b__0` 的方法体**内联回 lambda 出现处**，捕获字段就是被闭包捕获的局部。输出 `() => { … }`，**不输出 DisplayClass 这个类**。

**其它**：匿名类型 `new {…}` → `<>f__AnonymousTypeN`；record（C# 9+）→ 有 `EqualityContract` + 自动 `ToString`/`Equals`/`Deconstruct` → 折成 `record class Point(int X, int Y);`；event → `add_OnX`/`remove_OnX`（`Delegate.Combine`/`Remove` 的 CAS 循环）→ 折成一行 `event`。

### (E) 语句糖——try/finally 一族
| 你写的 | 编译后 | 认法 |
|---|---|---|
| `using (x) {…}` | `try{…} finally{ x?.Dispose(); }` | 变量是 IDisposable + finally 里 Dispose |
| `lock (o) {…}` | `Monitor.Enter(o,&taken); try{…} finally{ if(taken) Monitor.Exit(o); }` | Monitor.Enter/Exit 夹 try-finally |
| `foreach (v in c)` | `e=c.GetEnumerator(); try{ while(e.MoveNext()) v=e.Current; } finally{ e.Dispose(); }` | GetEnumerator+MoveNext+Dispose |
| `$"Hi {name}"` | `String.Format(lit, new object[]{ box(name) })`（或 C#10 `DefaultInterpolatedStringHandler`） | String.Format + 装箱数组 |
| `Interlocked.Increment(ref x)` | LL/SC 循环（`ExclusiveMonitorPass`） | 原子循环命中 Interlocked 签名 |

### (F) 常量与字符串
字符串字面量存 metadata 的 StringLiteral 表，伪 C 里是 `StringLiteral_9299`——用 Il2CppDumper 的 `stringliteral.json` 映射回真串。`nameof(x)` 编译期就变成普通字符串字面量，看到字面量正好等于某字段/参数名时，**优先折回 `nameof`**。

### 还原度分级（诚实标注）
| 层次 | 还原度 | 说明 |
|---|---|---|
| 类/方法签名、字段、继承 | **近乎无损** | metadata 明文，1:1 |
| 方法体的算法逻辑 | **高保真** | 公式/分支/循环语义都在，变量名丢、控制流可能重构 |
| 语法糖（yield/async/lambda/using/lock） | **可折回** | 认出 pattern 就能还原；极复杂状态机给注释 + 尽力 |
| 局部变量名 / 注释 / 私有实现细节 | **丢失** | 只能靠语义重命名 |

逆出来的 C# **能编译、能读、语义等价**——够你分析算法、改逻辑、抽协议（3.5）。别拿"和原文一模一样"当验收标准。

---

## 四、检查点

- 能说清"为什么 il2cpp 比纯 C/C++ 好逆"（符号 + 类型 + 固定 pattern 三条）。
- 看到 `<Xxx>d__N`/`<>c__DisplayClassN_M` 能立刻认出是 yield/async/lambda，并折回原语法（不把它当真类输出、不逐字翻 MoveNext）。
- 能把 3.2 的一个方法伪 C 翻成可编译真 C#，标注哪些是"还原"、哪些是"丢失后补的名字"。
- 记得先符号注入（`script.json`）再翻译、字符串接 `stringliteral.json`、泛型保留类型实参。

---

## 动手：运行演示程序

双击 **`正课/lesson-3.3.exe`**：讲边界 + il2cpp 高还原率三条 → **第 1 步**符号注入前后（`FUN_00700db8` → `GameLogic$$CalcDamage`）→ **第 2 步**逐行翻 `CalcDamage` 并和 `GameLogic.cs` 逐行对 → **第 3 步 ⭐ 语法糖还原**：从真 dump.cs 现场抠 `GameBoot.<QuitAfterDelay>d__2` 状态机（+ 真 `MoveNext`）折回 coroutine、抠 `<>c__DisplayClass3_0` 折回 `runOnUiThread(() => …)` → 语法特征速查表一屏过 → 打开产物文件夹。

程序**刻意把逐行翻译和语法糖还原分开演**——这正是本章核心：**语法糖还原是单独一步**。（`正课/sample-output/` 有真 dump.cs 抠出的状态机骨架、`MoveNext` 伪 C、翻好的真 C#。）

> **作业**（`作业/`）：材料 `dump.cs` 是爬塔通关揭示奖励码的协程 `Puzzle33.RevealFlag()` 被 IL2CPP 编成的**状态机**（`<RevealFlag>d__1`，字段全是 `<>`-mangled、解码逻辑散在 `MoveNext` 里，直接读没法看）。把它**还原成可读的 C#**、跑通那段解码逻辑算出 flag，交给 `作业/check.exe <flag>`。详情见 `作业/README.md`。

---

## 参考资料

- **[sharplab.io](https://sharplab.io)** —— 在线看任意 C# 片段编译成 IL / 展开成状态机，是"认 pattern"最好的练手场：写一段 `yield`/`async`/`lambda`，切 IL 或 "C#" 视图，就能看到 `<>d__N`/`<>c__DisplayClass` 长什么样。
- 编译器生成类命名规范（`<Method>d__N` 迭代器/异步、`<>c__DisplayClassN_M` 闭包、`<>c` 无捕获单例、`<>f__AnonymousTypeN` 匿名类型、`<X>k__BackingField` 自动属性）：C# 编译器规范 / Roslyn 源码。
- Il2CppDumper 的 `script.json` / `stringliteral.json` —— 符号注入与字符串还原的数据源（[0.1](../p0-01-env-setup/README.md)）。

---

## 小结 & 下一章

你把骨架 + 伪 C 翻成了**可编译、可读、语义等价的真 C#**——三步：**① 符号注入（机械）② 逐行 idiom 翻译（局部）③ 语法糖还原（结构级，单独一步）**。第 3 步是重点也最易翻错：编译器生成类要"识别→配对→解码→折叠丢弃"。**静态**还原 C# 层到此通关。下一课 **3.4** 进**动态**：用 Frida 在运行时 hook 这些 C# 方法，改返回值 / 抓参数，验证静态还原对不对、或直接改逻辑。
