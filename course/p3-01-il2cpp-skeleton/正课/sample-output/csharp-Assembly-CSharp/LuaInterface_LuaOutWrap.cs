using System;
using Il2CppDummyDll;
using LuaInterface;

[Token(Token = "0x2000015")]
public class LuaInterface_LuaOutWrap
{
	[Token(Token = "0x600003B")]
	[Address(RVA = "0x60462C", Offset = "0x60062C", VA = "0x60462C")]
	public static void Register(LuaState L)
	{
	}

	[Token(Token = "0x600003C")]
	[Address(RVA = "0x604358", Offset = "0x600358", VA = "0x604358")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	private static int LuaOpen_ToLua_Out(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x600003D")]
	private static void RawSetOutType<T>(IntPtr L)
	{
	}

	[Token(Token = "0x600003E")]
	[Address(RVA = "0x6046E8", Offset = "0x6006E8", VA = "0x6046E8")]
	public LuaInterface_LuaOutWrap()
	{
	}
}
