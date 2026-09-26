using System;
using Il2CppDummyDll;
using LuaInterface;

[Token(Token = "0x2000011")]
public class LuaInterface_EventObjectWrap
{
	[Token(Token = "0x600002B")]
	[Address(RVA = "0x603150", Offset = "0x5FF150", VA = "0x603150")]
	public static void Register(LuaState L)
	{
	}

	[Token(Token = "0x600002C")]
	[Address(RVA = "0x602D48", Offset = "0x5FED48", VA = "0x602D48")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	private static int op_Subtraction(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x600002D")]
	[Address(RVA = "0x602F4C", Offset = "0x5FEF4C", VA = "0x602F4C")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	private static int op_Addition(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x600002E")]
	[Address(RVA = "0x603348", Offset = "0x5FF348", VA = "0x603348")]
	public LuaInterface_EventObjectWrap()
	{
	}
}
