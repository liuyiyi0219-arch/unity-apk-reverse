import ghidra.app.script.GhidraScript;
import ghidra.app.decompiler.DecompInterface;
import ghidra.app.decompiler.DecompileResults;
import ghidra.program.model.address.Address;
import ghidra.program.model.listing.Function;

public class DecompileRVA extends GhidraScript {
    public void run() throws Exception {
        String[] args = getScriptArgs();
        DecompInterface di = new DecompInterface();
        di.openProgram(currentProgram);
        for (String a : args) {
            long rva = Long.decode(a);
            Address addr = currentProgram.getImageBase().add(rva);
            println("========== RVA " + a + " (ghidra " + addr + ") ==========");
            Function f = getFunctionContaining(addr);
            if (f == null) f = createFunction(addr, null);
            if (f == null) { println("(no function)"); continue; }
            DecompileResults res = di.decompileFunction(f, 90, monitor);
            if (res != null && res.getDecompiledFunction() != null)
                println(res.getDecompiledFunction().getC());
            else
                println("(decompile failed)");
        }
    }
}
