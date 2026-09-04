using System;
using System.Reflection;
using Il2CppDummyDll;

namespace LuaInterface;

[Token(Token = "0x200007D")]
public sealed class LuaProperty
{
	[Token(Token = "0x4000139")]
	[FieldOffset(Offset = "0x10")]
	private PropertyInfo property;

	[Token(Token = "0x400013A")]
	[FieldOffset(Offset = "0x18")]
	private Type kclass;

	[Token(Token = "0x600071A")]
	[Address(RVA = "0x687514", Offset = "0x683514", VA = "0x687514")]
	[NoToLua]
	public LuaProperty(PropertyInfo prop, Type t)
	{
	}

	[Token(Token = "0x600071B")]
	[Address(RVA = "0x687558", Offset = "0x683558", VA = "0x687558")]
	public int Get(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x600071C")]
	[Address(RVA = "0x687870", Offset = "0x683870", VA = "0x687870")]
	public int Set(IntPtr L)
	{
		return default(int);
	}
}
