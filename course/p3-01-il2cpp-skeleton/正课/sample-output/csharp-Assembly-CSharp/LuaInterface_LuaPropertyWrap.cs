using System;
using Il2CppDummyDll;
using LuaInterface;

[Token(Token = "0x2000016")]
public class LuaInterface_LuaPropertyWrap
{
	[Token(Token = "0x600003F")]
	[Address(RVA = "0x604A40", Offset = "0x600A40", VA = "0x604A40")]
	public static void Register(LuaState L)
	{
	}

	[Token(Token = "0x6000040")]
	[Address(RVA = "0x6046F0", Offset = "0x6006F0", VA = "0x6046F0")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	private static int Get(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x6000041")]
	[Address(RVA = "0x604898", Offset = "0x600898", VA = "0x604898")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	private static int Set(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x6000042")]
	[Address(RVA = "0x604C38", Offset = "0x600C38", VA = "0x604C38")]
	public LuaInterface_LuaPropertyWrap()
	{
	}
}
