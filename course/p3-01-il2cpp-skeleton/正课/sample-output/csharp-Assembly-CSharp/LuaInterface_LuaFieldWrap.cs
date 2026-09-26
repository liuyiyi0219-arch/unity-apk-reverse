using System;
using Il2CppDummyDll;
using LuaInterface;

[Token(Token = "0x2000013")]
public class LuaInterface_LuaFieldWrap
{
	[Token(Token = "0x6000033")]
	[Address(RVA = "0x603BFC", Offset = "0x5FFBFC", VA = "0x603BFC")]
	public static void Register(LuaState L)
	{
	}

	[Token(Token = "0x6000034")]
	[Address(RVA = "0x6038AC", Offset = "0x5FF8AC", VA = "0x6038AC")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	private static int Get(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x6000035")]
	[Address(RVA = "0x603A54", Offset = "0x5FFA54", VA = "0x603A54")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	private static int Set(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x6000036")]
	[Address(RVA = "0x603DF4", Offset = "0x5FFDF4", VA = "0x603DF4")]
	public LuaInterface_LuaFieldWrap()
	{
	}
}
