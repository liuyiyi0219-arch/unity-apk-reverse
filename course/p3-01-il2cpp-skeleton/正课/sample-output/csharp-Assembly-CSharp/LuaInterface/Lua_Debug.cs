using System;
using Il2CppDummyDll;

namespace LuaInterface;

[Token(Token = "0x2000043")]
public struct Lua_Debug
{
	[Token(Token = "0x400007E")]
	[FieldOffset(Offset = "0x0")]
	public int eventcode;

	[Token(Token = "0x400007F")]
	[FieldOffset(Offset = "0x8")]
	public IntPtr _name;

	[Token(Token = "0x4000080")]
	[FieldOffset(Offset = "0x10")]
	public IntPtr _namewhat;

	[Token(Token = "0x4000081")]
	[FieldOffset(Offset = "0x18")]
	public IntPtr _what;

	[Token(Token = "0x4000082")]
	[FieldOffset(Offset = "0x20")]
	public IntPtr _source;

	[Token(Token = "0x4000083")]
	[FieldOffset(Offset = "0x28")]
	public int currentline;

	[Token(Token = "0x4000084")]
	[FieldOffset(Offset = "0x2C")]
	public int nups;

	[Token(Token = "0x4000085")]
	[FieldOffset(Offset = "0x30")]
	public int linedefined;

	[Token(Token = "0x4000086")]
	[FieldOffset(Offset = "0x34")]
	public int lastlinedefined;

	[Token(Token = "0x4000087")]
	[FieldOffset(Offset = "0x38")]
	public byte[] _short_src;

	[Token(Token = "0x4000088")]
	[FieldOffset(Offset = "0x40")]
	public int i_ci;

	[Token(Token = "0x17000011")]
	public string namewhat
	{
		[Token(Token = "0x6000209")]
		[Address(RVA = "0x647AC4", Offset = "0x643AC4", VA = "0x647AC4")]
		get
		{
			return null;
		}
	}

	[Token(Token = "0x17000012")]
	public string name
	{
		[Token(Token = "0x600020A")]
		[Address(RVA = "0x647ACC", Offset = "0x643ACC", VA = "0x647ACC")]
		get
		{
			return null;
		}
	}

	[Token(Token = "0x17000013")]
	public string what
	{
		[Token(Token = "0x600020B")]
		[Address(RVA = "0x647AD4", Offset = "0x643AD4", VA = "0x647AD4")]
		get
		{
			return null;
		}
	}

	[Token(Token = "0x17000014")]
	public string source
	{
		[Token(Token = "0x600020C")]
		[Address(RVA = "0x647ADC", Offset = "0x643ADC", VA = "0x647ADC")]
		get
		{
			return null;
		}
	}

	[Token(Token = "0x17000015")]
	public string short_src
	{
		[Token(Token = "0x600020E")]
		[Address(RVA = "0x647B24", Offset = "0x643B24", VA = "0x647B24")]
		get
		{
			return null;
		}
	}

	[Token(Token = "0x6000208")]
	[Address(RVA = "0x6478B8", Offset = "0x6438B8", VA = "0x6478B8")]
	private string tostring(IntPtr p)
	{
		return null;
	}

	[Token(Token = "0x600020D")]
	[Address(RVA = "0x647AE4", Offset = "0x643AE4", VA = "0x647AE4")]
	private int GetShortSrcLen(byte[] str)
	{
		return default(int);
	}
}
