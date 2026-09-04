using System;
using Il2CppDummyDll;

namespace LuaInterface;

[Token(Token = "0x2000054")]
[NoToLua]
public static class LuaMisc
{
	[Token(Token = "0x60003C2")]
	[Address(RVA = "0x659450", Offset = "0x655450", VA = "0x659450")]
	public static string GetArrayRank(Type t)
	{
		return null;
	}

	[Token(Token = "0x60003C3")]
	[Address(RVA = "0x659660", Offset = "0x655660", VA = "0x659660")]
	public static string GetTypeName(Type t)
	{
		return null;
	}

	[Token(Token = "0x60003C4")]
	[Address(RVA = "0x65A0E4", Offset = "0x6560E4", VA = "0x65A0E4")]
	public static string[] GetGenericName(Type[] types, int offset, int count)
	{
		return null;
	}

	[Token(Token = "0x60003C5")]
	[Address(RVA = "0x65A230", Offset = "0x656230", VA = "0x65A230")]
	private static string CombineTypeStr(string space, string name)
	{
		return null;
	}

	[Token(Token = "0x60003C6")]
	[Address(RVA = "0x6597E4", Offset = "0x6557E4", VA = "0x6597E4")]
	private static string GetGenericName(Type t)
	{
		return null;
	}

	[Token(Token = "0x60003C7")]
	[Address(RVA = "0x65A2A4", Offset = "0x6562A4", VA = "0x65A2A4")]
	public static Delegate GetEventHandler(object obj, Type t, string eventName)
	{
		return null;
	}

	[Token(Token = "0x60003C8")]
	[Address(RVA = "0x659AD8", Offset = "0x655AD8", VA = "0x659AD8")]
	public static string GetPrimitiveStr(Type t)
	{
		return null;
	}

	[Token(Token = "0x60003C9")]
	[Address(RVA = "0x65A364", Offset = "0x656364", VA = "0x65A364")]
	public static double ToDouble(object obj)
	{
		return default(double);
	}

	[Token(Token = "0x60003CA")]
	[Address(RVA = "0x65A864", Offset = "0x656864", VA = "0x65A864")]
	public static Type GetExportBaseType(Type t)
	{
		return null;
	}
}
