using System;
using Il2CppDummyDll;

namespace LuaInterface;

[Token(Token = "0x2000038")]
[AttributeUsage(AttributeTargets.Method)]
public sealed class LuaRenameAttribute : Attribute
{
	[Token(Token = "0x4000043")]
	[FieldOffset(Offset = "0x10")]
	public string Name;

	[Token(Token = "0x60001F0")]
	[Address(RVA = "0x647264", Offset = "0x643264", VA = "0x647264")]
	public LuaRenameAttribute()
	{
	}
}
