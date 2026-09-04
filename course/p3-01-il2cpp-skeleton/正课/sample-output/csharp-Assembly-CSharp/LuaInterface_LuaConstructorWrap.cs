using System;
using Il2CppDummyDll;
using LuaInterface;

[Token(Token = "0x2000012")]
public class LuaInterface_LuaConstructorWrap
{
	[Token(Token = "0x600002F")]
	[Address(RVA = "0x6036AC", Offset = "0x5FF6AC", VA = "0x6036AC")]
	public static void Register(LuaState L)
	{
	}

	[Token(Token = "0x6000030")]
	[Address(RVA = "0x603350", Offset = "0x5FF350", VA = "0x603350")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	private static int Call(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x6000031")]
	[Address(RVA = "0x6034F8", Offset = "0x5FF4F8", VA = "0x6034F8")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	private static int Destroy(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x6000032")]
	[Address(RVA = "0x6038A4", Offset = "0x5FF8A4", VA = "0x6038A4")]
	public LuaInterface_LuaConstructorWrap()
	{
	}
}
