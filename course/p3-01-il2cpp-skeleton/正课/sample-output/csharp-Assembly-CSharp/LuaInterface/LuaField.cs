using System;
using System.Reflection;
using Il2CppDummyDll;

namespace LuaInterface;

[Token(Token = "0x200007B")]
public sealed class LuaField
{
	[Token(Token = "0x4000134")]
	[FieldOffset(Offset = "0x10")]
	private FieldInfo field;

	[Token(Token = "0x4000135")]
	[FieldOffset(Offset = "0x18")]
	private Type kclass;

	[Token(Token = "0x6000714")]
	[Address(RVA = "0x6867FC", Offset = "0x6827FC", VA = "0x6867FC")]
	[NoToLua]
	public LuaField(FieldInfo info, Type t)
	{
	}

	[Token(Token = "0x6000715")]
	[Address(RVA = "0x686840", Offset = "0x682840", VA = "0x686840")]
	public int Get(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x6000716")]
	[Address(RVA = "0x686B60", Offset = "0x682B60", VA = "0x686B60")]
	public int Set(IntPtr L)
	{
		return default(int);
	}
}
