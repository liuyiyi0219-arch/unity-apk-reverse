using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Il2CppDummyDll;

namespace LuaInterface;

[Token(Token = "0x2000048")]
public class LuaException : Exception
{
	[Token(Token = "0x4000092")]
	[Il2CppDummyDll.FieldOffset(Offset = "0x0")]
	public static Exception luaStack;

	[Token(Token = "0x4000093")]
	[Il2CppDummyDll.FieldOffset(Offset = "0x8")]
	public static string projectFolder;

	[Token(Token = "0x4000094")]
	[Il2CppDummyDll.FieldOffset(Offset = "0x10")]
	public static int InstantiateCount;

	[Token(Token = "0x4000095")]
	[Il2CppDummyDll.FieldOffset(Offset = "0x14")]
	public static int SendMsgCount;

	[Token(Token = "0x4000096")]
	[Il2CppDummyDll.FieldOffset(Offset = "0x18")]
	public static IntPtr L;

	[Token(Token = "0x4000097")]
	[Il2CppDummyDll.FieldOffset(Offset = "0x90")]
	protected string _stack;

	[Token(Token = "0x17000016")]
	public override string StackTrace
	{
		[Token(Token = "0x6000303")]
		[Address(RVA = "0x64FF8C", Offset = "0x64BF8C", VA = "0x64FF8C", Slot = "8")]
		get
		{
			return null;
		}
	}

	[Token(Token = "0x6000304")]
	[Address(RVA = "0x64FF94", Offset = "0x64BF94", VA = "0x64FF94")]
	public LuaException(string msg, [Optional] Exception e, [Optional] int skip)
	{
	}

	[Token(Token = "0x6000305")]
	[Address(RVA = "0x650874", Offset = "0x64C874", VA = "0x650874")]
	public static Exception GetLastError()
	{
		return null;
	}

	[Token(Token = "0x6000306")]
	[Address(RVA = "0x6501D4", Offset = "0x64C1D4", VA = "0x6501D4")]
	public static void ExtractFormattedStackTrace(StackTrace trace, StringBuilder sb, [Optional] StackTrace skip)
	{
	}

	[Token(Token = "0x6000307")]
	[Address(RVA = "0x6508E4", Offset = "0x64C8E4", VA = "0x6508E4")]
	public static void Init(IntPtr L0)
	{
	}
}
