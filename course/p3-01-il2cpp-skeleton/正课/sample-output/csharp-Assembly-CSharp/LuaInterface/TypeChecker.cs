using System;
using Il2CppDummyDll;

namespace LuaInterface;

[Token(Token = "0x2000073")]
public static class TypeChecker
{
	[Token(Token = "0x4000117")]
	[FieldOffset(Offset = "0x0")]
	public static Type[] LuaValueTypeMap;

	[Token(Token = "0x4000118")]
	[FieldOffset(Offset = "0x8")]
	private static Type monoType;

	[Token(Token = "0x60006CF")]
	[Address(RVA = "0x683BE4", Offset = "0x67FBE4", VA = "0x683BE4")]
	static TypeChecker()
	{
	}

	[Token(Token = "0x60006D0")]
	[Address(RVA = "0x677968", Offset = "0x673968", VA = "0x677968")]
	public static bool IsValueType(Type t)
	{
		return default(bool);
	}

	[Token(Token = "0x60006D1")]
	[Address(RVA = "0x6841E4", Offset = "0x6801E4", VA = "0x6841E4")]
	public static bool CheckTypes(IntPtr L, int begin, Type type0)
	{
		return default(bool);
	}

	[Token(Token = "0x60006D2")]
	[Address(RVA = "0x684678", Offset = "0x680678", VA = "0x684678")]
	public static bool CheckTypes(IntPtr L, int begin, Type type0, Type type1)
	{
		return default(bool);
	}

	[Token(Token = "0x60006D3")]
	[Address(RVA = "0x68472C", Offset = "0x68072C", VA = "0x68472C")]
	public static bool CheckTypes(IntPtr L, int begin, Type type0, Type type1, Type type2)
	{
		return default(bool);
	}

	[Token(Token = "0x60006D4")]
	[Address(RVA = "0x684808", Offset = "0x680808", VA = "0x684808")]
	public static bool CheckTypes(IntPtr L, int begin, Type type0, Type type1, Type type2, Type type3)
	{
		return default(bool);
	}

	[Token(Token = "0x60006D5")]
	[Address(RVA = "0x684918", Offset = "0x680918", VA = "0x684918")]
	public static bool CheckTypes(IntPtr L, int begin, Type type0, Type type1, Type type2, Type type3, Type type4)
	{
		return default(bool);
	}

	[Token(Token = "0x60006D6")]
	[Address(RVA = "0x684A50", Offset = "0x680A50", VA = "0x684A50")]
	public static bool CheckTypes(IntPtr L, int begin, Type type0, Type type1, Type type2, Type type3, Type type4, Type type5)
	{
		return default(bool);
	}

	[Token(Token = "0x60006D7")]
	[Address(RVA = "0x684BBC", Offset = "0x680BBC", VA = "0x684BBC")]
	public static bool CheckTypes(IntPtr L, int begin, Type type0, Type type1, Type type2, Type type3, Type type4, Type type5, Type type6)
	{
		return default(bool);
	}

	[Token(Token = "0x60006D8")]
	[Address(RVA = "0x684D50", Offset = "0x680D50", VA = "0x684D50")]
	public static bool CheckTypes(IntPtr L, int begin, Type type0, Type type1, Type type2, Type type3, Type type4, Type type5, Type type6, Type type7)
	{
		return default(bool);
	}

	[Token(Token = "0x60006D9")]
	[Address(RVA = "0x684F18", Offset = "0x680F18", VA = "0x684F18")]
	public static bool CheckTypes(IntPtr L, int begin, Type type0, Type type1, Type type2, Type type3, Type type4, Type type5, Type type6, Type type7, Type type8)
	{
		return default(bool);
	}

	[Token(Token = "0x60006DA")]
	[Address(RVA = "0x685108", Offset = "0x681108", VA = "0x685108")]
	public static bool CheckTypes(IntPtr L, int begin, Type type0, Type type1, Type type2, Type type3, Type type4, Type type5, Type type6, Type type7, Type type8, Type type9)
	{
		return default(bool);
	}

	[Token(Token = "0x60006DB")]
	[Address(RVA = "0x685324", Offset = "0x681324", VA = "0x685324")]
	public static bool CheckTypes(IntPtr L, int begin, params Type[] types)
	{
		return default(bool);
	}

	[Token(Token = "0x60006DC")]
	[Address(RVA = "0x685400", Offset = "0x681400", VA = "0x685400")]
	public static bool CheckParamsType(IntPtr L, Type t, int begin, int count)
	{
		return default(bool);
	}

	[Token(Token = "0x60006DD")]
	[Address(RVA = "0x6854FC", Offset = "0x6814FC", VA = "0x6854FC")]
	private static bool IsNilType(Type t)
	{
		return default(bool);
	}

	[Token(Token = "0x60006DE")]
	[Address(RVA = "0x67E578", Offset = "0x67A578", VA = "0x67E578")]
	public static bool IsNullable(Type t)
	{
		return default(bool);
	}

	[Token(Token = "0x60006DF")]
	[Address(RVA = "0x685578", Offset = "0x681578", VA = "0x685578")]
	public static Type GetNullableType(Type t)
	{
		return null;
	}

	[Token(Token = "0x60006E0")]
	[Address(RVA = "0x684250", Offset = "0x680250", VA = "0x684250")]
	public static bool CheckType(IntPtr L, Type type, int pos)
	{
		return default(bool);
	}

	[Token(Token = "0x60006E1")]
	public static T ChangeType<T>(object temp, Type type)
	{
		return (T)null;
	}

	[Token(Token = "0x60006E2")]
	[Address(RVA = "0x685BF4", Offset = "0x681BF4", VA = "0x685BF4")]
	public static object ChangeType(object temp, Type type)
	{
		return null;
	}

	[Token(Token = "0x60006E3")]
	[Address(RVA = "0x6857A4", Offset = "0x6817A4", VA = "0x6857A4")]
	private static bool IsMatchUserData(IntPtr L, Type t, int pos)
	{
		return default(bool);
	}

	[Token(Token = "0x60006E4")]
	[Address(RVA = "0x685668", Offset = "0x681668", VA = "0x685668")]
	public static bool IsNumberType(Type t)
	{
		return default(bool);
	}

	[Token(Token = "0x60006E5")]
	[Address(RVA = "0x6859E0", Offset = "0x6819E0", VA = "0x6859E0")]
	public static bool IsUserTable(Type t, IntPtr L, int pos)
	{
		return default(bool);
	}

	[Token(Token = "0x60006E6")]
	public static bool CheckTypes<T1>(IntPtr L, int pos)
	{
		return default(bool);
	}

	[Token(Token = "0x60006E7")]
	public static bool CheckTypes<T1, T2>(IntPtr L, int pos)
	{
		return default(bool);
	}

	[Token(Token = "0x60006E8")]
	public static bool CheckTypes<T1, T2, T3>(IntPtr L, int pos)
	{
		return default(bool);
	}

	[Token(Token = "0x60006E9")]
	public static bool CheckTypes<T1, T2, T3, T4>(IntPtr L, int pos)
	{
		return default(bool);
	}

	[Token(Token = "0x60006EA")]
	public static bool CheckTypes<T1, T2, T3, T4, T5>(IntPtr L, int pos)
	{
		return default(bool);
	}

	[Token(Token = "0x60006EB")]
	public static bool CheckTypes<T1, T2, T3, T4, T5, T6>(IntPtr L, int pos)
	{
		return default(bool);
	}

	[Token(Token = "0x60006EC")]
	public static bool CheckTypes<T1, T2, T3, T4, T5, T6, T7>(IntPtr L, int pos)
	{
		return default(bool);
	}

	[Token(Token = "0x60006ED")]
	public static bool CheckTypes<T1, T2, T3, T4, T5, T6, T7, T8>(IntPtr L, int pos)
	{
		return default(bool);
	}

	[Token(Token = "0x60006EE")]
	public static bool CheckTypes<T1, T2, T3, T4, T5, T6, T7, T8, T9>(IntPtr L, int pos)
	{
		return default(bool);
	}

	[Token(Token = "0x60006EF")]
	public static bool CheckTypes<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(IntPtr L, int pos)
	{
		return default(bool);
	}

	[Token(Token = "0x60006F0")]
	public static bool CheckTypes<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(IntPtr L, int pos)
	{
		return default(bool);
	}

	[Token(Token = "0x60006F1")]
	public static bool CheckTypes<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(IntPtr L, int pos)
	{
		return default(bool);
	}

	[Token(Token = "0x60006F2")]
	public static bool CheckParamsType<T>(IntPtr L, int begin, int count)
	{
		return default(bool);
	}

	[Token(Token = "0x60006F3")]
	[Address(RVA = "0x685D48", Offset = "0x681D48", VA = "0x685D48")]
	public static bool CheckDelegateType(Type type, IntPtr L, int pos)
	{
		return default(bool);
	}

	[Token(Token = "0x60006F4")]
	[Address(RVA = "0x685E9C", Offset = "0x681E9C", VA = "0x685E9C")]
	public static bool CheckEnumType(Type type, IntPtr L, int pos)
	{
		return default(bool);
	}
}
