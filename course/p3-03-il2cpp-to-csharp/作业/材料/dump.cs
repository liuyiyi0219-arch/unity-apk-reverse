// ============================================================================
// 这是 Il2CppDumper / 反编译器出的「骨架」——一段含 yield 的协程被编译器去掉语法糖后，
// 变成了下面这台状态机（编译器生成的 <RevealFlag>d__1，字段名都是 <>-mangled 的）。
// 直接读它很难看出在算什么：状态在 <>1__state 里跳、变量散在几个 <..>5__N 字段里、
// 解码逻辑被拆进 MoveNext 的每一次调用。
//
// 作业：把这台状态机【还原】回原来那段可读的协程（yield return ...），
// 看懂它的解码算法，算出它一路 yield 出来的字符拼成的 flag。
// （这一步就是「骨架 → 真 C#」，通常喂给 AI 做——见作业 README。）
// ============================================================================

public class Puzzle33 : MonoBehaviour
{
    // RVA: 0x0 —— 编译器把协程体搬进了下面的状态机，这里只 new 一个它出来。
    public System.Collections.Generic.IEnumerator<char> RevealFlag()
    {
        return new <RevealFlag>d__1(0) { <>4__this = this };
    }

    [CompilerGenerated]
    private sealed class <RevealFlag>d__1 : IEnumerator<char>, IEnumerator, IDisposable
    {
        // 这几个常量被编译器内联进了 MoveNext 的算式里（源码里它们是类内的字面量）。
        private const uint K0 = 0xDEADBEEFu;
        private const uint K1 = 0x01234567u;
        private const uint K2 = 0x89ABCDEFu;
        private const uint K3 = 0x0BADF00Du;
        private const uint DELTA = 0x9E3779B9u;

        private int <>1__state;
        private char <>2__current;
        public Puzzle33 <>4__this;
        private byte[] <enc>5__1;
        private int <off>5__2;
        private ulong <blk>5__3;
        private int <j>5__4;

        [DebuggerHidden]
        public <RevealFlag>d__1(int <>1__state)
        {
            this.<>1__state = <>1__state;
        }

        [DebuggerHidden]
        void IDisposable.Dispose() { }

        private bool MoveNext()
        {
            switch (<>1__state)
            {
                default:
                    return false;
                case 0:
                    <>1__state = -1;
                    <enc>5__1 = new byte[]
                    {
                        0x1D, 0xF7, 0xE5, 0xD8, 0x60, 0x1F, 0x60, 0x80, 0xE9, 0xB1,
                        0x68, 0x10, 0x75, 0xD3, 0x17, 0xF0, 0x11, 0xD0, 0x28, 0x59,
                        0x16, 0x0B, 0xF0, 0x61, 0xF2, 0xB5, 0xC4, 0xCE, 0x5D, 0x41,
                        0xA0, 0xA7
                    };
                    <off>5__2 = 0;
                    <j>5__4 = 0;
                    break;
                case 1:
                    <>1__state = -1;
                    <j>5__4++;
                    break;
            }
            while (<off>5__2 < <enc>5__1.Length)
            {
                if (<j>5__4 == 0)
                {
                    uint v0 = (uint)(<enc>5__1[<off>5__2] | (<enc>5__1[<off>5__2 + 1] << 8) | (<enc>5__1[<off>5__2 + 2] << 16) | (<enc>5__1[<off>5__2 + 3] << 24));
                    uint v1 = (uint)(<enc>5__1[<off>5__2 + 4] | (<enc>5__1[<off>5__2 + 5] << 8) | (<enc>5__1[<off>5__2 + 6] << 16) | (<enc>5__1[<off>5__2 + 7] << 24));
                    uint sum = 0xC6EF3720u;
                    for (int r = 0; r < 32; r++)
                    {
                        v1 -= (((v0 << 4) + K2) ^ (v0 + sum) ^ ((v0 >> 5) + K3));
                        v0 -= (((v1 << 4) + K0) ^ (v1 + sum) ^ ((v1 >> 5) + K1));
                        sum -= DELTA;
                    }
                    <blk>5__3 = (ulong)v0 | ((ulong)v1 << 32);
                }
                if (<j>5__4 < 8)
                {
                    if (<off>5__2 + <j>5__4 >= 29)
                    {
                        return false;
                    }
                    <>2__current = (char)(byte)(<blk>5__3 >> (<j>5__4 * 8));
                    <>1__state = 1;
                    return true;
                }
                <off>5__2 += 8;
                <j>5__4 = 0;
            }
            return false;
        }

        char IEnumerator<char>.Current
        {
            [DebuggerHidden] get { return <>2__current; }
        }

        object IEnumerator.Current
        {
            [DebuggerHidden] get { return <>2__current; }
        }

        [DebuggerHidden]
        void IEnumerator.Reset() { throw new NotSupportedException(); }
    }
}
