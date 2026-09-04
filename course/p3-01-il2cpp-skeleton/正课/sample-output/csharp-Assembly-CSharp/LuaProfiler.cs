using System.Collections.Generic;
using Il2CppDummyDll;

[Token(Token = "0x200002F")]
public static class LuaProfiler
{
	[Token(Token = "0x400003D")]
	[FieldOffset(Offset = "0x0")]
	public static List<string> list;

	[Token(Token = "0x60001DF")]
	[Address(RVA = "0x644E88", Offset = "0x640E88", VA = "0x644E88")]
	public static void Clear()
	{
	}

	[Token(Token = "0x60001E0")]
	[Address(RVA = "0x646758", Offset = "0x642758", VA = "0x646758")]
	public static int GetID(string name)
	{
		return default(int);
	}

	[Token(Token = "0x60001E1")]
	[Address(RVA = "0x64683C", Offset = "0x64283C", VA = "0x64683C")]
	public static void BeginSample(int id)
	{
	}

	[Token(Token = "0x60001E2")]
	[Address(RVA = "0x6468BC", Offset = "0x6428BC", VA = "0x6468BC")]
	public static void EndSample()
	{
	}
}
