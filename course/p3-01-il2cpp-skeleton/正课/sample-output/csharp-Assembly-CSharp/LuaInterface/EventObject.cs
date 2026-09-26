using System;
using Il2CppDummyDll;

namespace LuaInterface;

[Token(Token = "0x2000058")]
public class EventObject
{
	[Token(Token = "0x40000B9")]
	[FieldOffset(Offset = "0x10")]
	[NoToLua]
	public EventOp op;

	[Token(Token = "0x40000BA")]
	[FieldOffset(Offset = "0x18")]
	[NoToLua]
	public Delegate func;

	[Token(Token = "0x40000BB")]
	[FieldOffset(Offset = "0x20")]
	[NoToLua]
	public Type type;

	[Token(Token = "0x60003CD")]
	[Address(RVA = "0x65A9A4", Offset = "0x6569A4", VA = "0x65A9A4")]
	[NoToLua]
	public EventObject(Type t)
	{
	}

	[Token(Token = "0x60003CE")]
	[Address(RVA = "0x65A9D4", Offset = "0x6569D4", VA = "0x65A9D4")]
	public static EventObject operator +(EventObject a, Delegate b)
	{
		return null;
	}

	[Token(Token = "0x60003CF")]
	[Address(RVA = "0x65AA00", Offset = "0x656A00", VA = "0x65AA00")]
	public static EventObject operator -(EventObject a, Delegate b)
	{
		return null;
	}
}
