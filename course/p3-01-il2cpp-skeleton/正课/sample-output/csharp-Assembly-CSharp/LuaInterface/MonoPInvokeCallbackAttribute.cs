using System;
using Il2CppDummyDll;

namespace LuaInterface;

[Token(Token = "0x2000033")]
[AttributeUsage(AttributeTargets.Method)]
public sealed class MonoPInvokeCallbackAttribute : Attribute
{
	[Token(Token = "0x60001EB")]
	[Address(RVA = "0x64723C", Offset = "0x64323C", VA = "0x64723C")]
	public MonoPInvokeCallbackAttribute(Type type)
	{
	}
}
