using System;
using System.Collections.Generic;
using System.Reflection;
using Il2CppDummyDll;

namespace LuaInterface;

[Token(Token = "0x200007A")]
public sealed class LuaConstructor
{
	[Token(Token = "0x4000132")]
	[FieldOffset(Offset = "0x10")]
	private ConstructorInfo method;

	[Token(Token = "0x4000133")]
	[FieldOffset(Offset = "0x18")]
	private List<Type> list;

	[Token(Token = "0x6000711")]
	[Address(RVA = "0x686430", Offset = "0x682430", VA = "0x686430")]
	[NoToLua]
	public LuaConstructor(ConstructorInfo func, Type[] types)
	{
	}

	[Token(Token = "0x6000712")]
	[Address(RVA = "0x6864E8", Offset = "0x6824E8", VA = "0x6864E8")]
	public int Call(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x6000713")]
	[Address(RVA = "0x68677C", Offset = "0x68277C", VA = "0x68677C")]
	public void Destroy()
	{
	}
}
