using System;
using Il2CppDummyDll;
using LuaInterface;

[Token(Token = "0x200001B")]
public class System_Collections_Generic_KeyValuePairWrap
{
	[Token(Token = "0x6000086")]
	[Address(RVA = "0x614FB4", Offset = "0x610FB4", VA = "0x614FB4")]
	public static void Register(LuaState L)
	{
	}

	[Token(Token = "0x6000087")]
	[Address(RVA = "0x614BC4", Offset = "0x610BC4", VA = "0x614BC4")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	private static int get_Key(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x6000088")]
	[Address(RVA = "0x614DBC", Offset = "0x610DBC", VA = "0x614DBC")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	private static int get_Value(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x6000089")]
	[Address(RVA = "0x6151A0", Offset = "0x6111A0", VA = "0x6151A0")]
	public System_Collections_Generic_KeyValuePairWrap()
	{
	}
}
