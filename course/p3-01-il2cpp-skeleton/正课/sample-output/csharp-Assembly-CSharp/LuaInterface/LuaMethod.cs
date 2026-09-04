using System;
using System.Collections.Generic;
using System.Reflection;
using Il2CppDummyDll;

namespace LuaInterface;

[Token(Token = "0x200007C")]
public sealed class LuaMethod
{
	[Token(Token = "0x4000136")]
	[FieldOffset(Offset = "0x10")]
	private MethodInfo method;

	[Token(Token = "0x4000137")]
	[FieldOffset(Offset = "0x18")]
	private List<Type> list;

	[Token(Token = "0x4000138")]
	[FieldOffset(Offset = "0x20")]
	private Type kclass;

	[Token(Token = "0x6000717")]
	[Address(RVA = "0x687018", Offset = "0x683018", VA = "0x687018")]
	[NoToLua]
	public LuaMethod(MethodInfo md, Type t, Type[] types)
	{
	}

	[Token(Token = "0x6000718")]
	[Address(RVA = "0x68711C", Offset = "0x68311C", VA = "0x68711C")]
	public int Call(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x6000719")]
	[Address(RVA = "0x687494", Offset = "0x683494", VA = "0x687494")]
	public void Destroy()
	{
	}
}
