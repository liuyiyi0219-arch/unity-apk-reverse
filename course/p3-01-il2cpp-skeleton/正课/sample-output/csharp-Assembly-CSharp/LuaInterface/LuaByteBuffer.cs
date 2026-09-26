using System;
using System.IO;
using System.Runtime.CompilerServices;
using Il2CppDummyDll;

namespace LuaInterface;

[Token(Token = "0x200004F")]
public struct LuaByteBuffer
{
	[Token(Token = "0x40000A6")]
	[FieldOffset(Offset = "0x0")]
	public byte[] buffer;

	[Token(Token = "0x17000018")]
	public int Length
	{
		[Token(Token = "0x60003B6")]
		[Address(RVA = "0x659124", Offset = "0x655124", VA = "0x659124")]
		[CompilerGenerated]
		readonly get
		{
			return default(int);
		}
		[Token(Token = "0x60003B7")]
		[Address(RVA = "0x65912C", Offset = "0x65512C", VA = "0x65912C")]
		[CompilerGenerated]
		private set
		{
		}
	}

	[Token(Token = "0x60003B1")]
	[Address(RVA = "0x658F88", Offset = "0x654F88", VA = "0x658F88")]
	public LuaByteBuffer(IntPtr source, int len)
	{
	}

	[Token(Token = "0x60003B2")]
	[Address(RVA = "0x659040", Offset = "0x655040", VA = "0x659040")]
	public LuaByteBuffer(byte[] buf)
	{
	}

	[Token(Token = "0x60003B3")]
	[Address(RVA = "0x659074", Offset = "0x655074", VA = "0x659074")]
	public LuaByteBuffer(byte[] buf, int len)
	{
	}

	[Token(Token = "0x60003B4")]
	[Address(RVA = "0x65909C", Offset = "0x65509C", VA = "0x65909C")]
	public LuaByteBuffer(MemoryStream stream)
	{
	}

	[Token(Token = "0x60003B5")]
	[Address(RVA = "0x6590FC", Offset = "0x6550FC", VA = "0x6590FC")]
	public static implicit operator LuaByteBuffer(MemoryStream stream)
	{
		return default(LuaByteBuffer);
	}
}
