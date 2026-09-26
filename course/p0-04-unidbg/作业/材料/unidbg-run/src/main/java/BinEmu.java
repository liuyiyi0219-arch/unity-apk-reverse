import com.github.unidbg.AndroidEmulator;
import com.github.unidbg.linux.android.AndroidEmulatorBuilder;
import com.github.unidbg.linux.android.AndroidResolver;
import com.github.unidbg.memory.Memory;
import com.github.unidbg.pointer.UnidbgPointer;

import java.io.File;
import java.nio.file.Files;

// 0.4 作业：把一段裸 arm64 机器码 bin 丢进 unidbg 模拟执行，读出它写进 out 缓冲的 flag。
//
// 手上没有 ELF（.so），就是一段码——所以走 unidbg 的低层路，不是 loadLibrary：
//   ① mmap 一段【可执行】内存，写进机器码 + 码后紧跟的编码数据；
//   ② mmap 一段读写内存当输出缓冲；
//   ③ eFunc(码地址, 出缓冲)：把出缓冲塞进 x0、设好返回哨兵、从码地址一路跑到 ret；
//   ④ 读出缓冲 —— 就是 bin 在 arm64 上跑出来的 flag。
//
// 跑：  mvn -q compile exec:java -Dexec.args="../flag_gen.bin"
public class BinEmu {
    static final int PROT_READ = 1, PROT_WRITE = 2, PROT_EXEC = 4;

    public static void main(String[] args) throws Exception {
        byte[] blob = Files.readAllBytes(new File(args[0]).toPath());

        AndroidEmulator emulator = AndroidEmulatorBuilder.for64Bit()
                .setProcessName("urev.unidbg.binemu").build();
        Memory memory = emulator.getMemory();
        memory.setLibraryResolver(new AndroidResolver(23));
        emulator.createDalvikVM();                       // 建基础运行时（linker / JNI 环境）

        UnidbgPointer code = memory.mmap(0x1000, PROT_READ | PROT_WRITE | PROT_EXEC);
        code.write(0, blob, 0, blob.length);             // 机器码 + 码后编码数据一起写进去
        UnidbgPointer out = memory.mmap(0x1000, PROT_READ | PROT_WRITE);

        // gen(char* out@x0)：x0 = 出缓冲；eFunc 跑到 ret 停
        emulator.eFunc(code.peer, (Number) Long.valueOf(out.peer));

        System.out.println("FLAG = " + out.getString(0));
        emulator.close();
    }
}
