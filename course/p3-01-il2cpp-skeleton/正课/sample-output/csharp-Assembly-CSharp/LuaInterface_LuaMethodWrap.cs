using System;
using Il2CppDummyDll;
using LuaInterface;

[Token(Token = "0x2000014")]
public class LuaInterface_LuaMethodWrap
{
	[Token(Token = "0x6000037")]
	[Address(RVA = "0x604158", Offset = "0x600158", VA = "0x604158")]
	public static void Register(LuaState L)
	{
	}

	[Token(Token = "0x6000038")]
	[Address(RVA = "0x603DFC", Offset = "0x5FFDFC", VA = "0x603DFC")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	private static int Destroy(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x6000039")]
	[Address(RVA = "0x603FB0", Offset = "0x5FFFB0", VA = "0x603FB0")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	private static int Call(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x600003A")]
	[Address(RVA = "0x604350", Offset = "0x600350", VA = "0x604350")]
	public LuaInterface_LuaMethodWrap()
	{
	}
}
