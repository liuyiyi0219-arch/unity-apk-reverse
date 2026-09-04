using System;
using Il2CppDummyDll;

namespace LuaInterface;

[Token(Token = "0x2000074")]
public static class TypeTraits<T>
{
	[Token(Token = "0x4000119")]
	[FieldOffset(Offset = "0x0")]
	public static Func<IntPtr, int, bool> Check;

	[Token(Token = "0x400011A")]
	[FieldOffset(Offset = "0x0")]
	public static Type type;

	[Token(Token = "0x400011B")]
	[FieldOffset(Offset = "0x0")]
	public static bool IsValueType;

	[Token(Token = "0x400011C")]
	[FieldOffset(Offset = "0x0")]
	public static bool IsArray;

	[Token(Token = "0x400011D")]
	[FieldOffset(Offset = "0x0")]
	private static string typeName;

	[Token(Token = "0x400011E")]
	[FieldOffset(Offset = "0x0")]
	private static int nilType;

	[Token(Token = "0x400011F")]
	[FieldOffset(Offset = "0x0")]
	private static int metaref;

	[Token(Token = "0x60006F5")]
	public static void Init(Func<IntPtr, int, bool> check)
	{
	}

	[Token(Token = "0x60006F6")]
	public static string GetTypeName()
	{
		return null;
	}

	[Token(Token = "0x60006F7")]
	public static int GetLuaReference(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x60006F8")]
	private static bool DefaultCheck(IntPtr L, int pos)
	{
		return default(bool);
	}

	[Token(Token = "0x60006F9")]
	private static bool IsNilType()
	{
		return default(bool);
	}

	[Token(Token = "0x60006FA")]
	private static bool IsUserData(IntPtr L, int pos)
	{
		return default(bool);
	}

	[Token(Token = "0x60006FB")]
	private static bool IsUserTable(IntPtr L, int pos)
	{
		return default(bool);
	}
}
