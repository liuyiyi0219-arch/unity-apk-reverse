using System;
using Il2CppDummyDll;

namespace LuaInterface;

[Token(Token = "0x2000076")]
public static class DelegateTraits<T>
{
	[Token(Token = "0x4000122")]
	[FieldOffset(Offset = "0x0")]
	private static DelegateFactory.DelegateCreate _Create;

	[Token(Token = "0x6000700")]
	public static void Init(DelegateFactory.DelegateCreate func)
	{
	}

	[Token(Token = "0x6000701")]
	public static Delegate Create(LuaFunction func)
	{
		return null;
	}

	[Token(Token = "0x6000702")]
	public static Delegate Create(LuaFunction func, LuaTable self)
	{
		return null;
	}
}
