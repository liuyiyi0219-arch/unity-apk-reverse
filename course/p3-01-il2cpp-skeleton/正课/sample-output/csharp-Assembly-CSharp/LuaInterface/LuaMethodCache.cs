using System;
using System.Collections.Generic;
using System.Reflection;
using Il2CppDummyDll;

namespace LuaInterface;

[Token(Token = "0x200004D")]
public static class LuaMethodCache
{
	[Token(Token = "0x40000A3")]
	[FieldOffset(Offset = "0x0")]
	public static Dictionary<Type, Dictionary<string, List<MethodInfo>>> dict;

	[Token(Token = "0x60003AC")]
	[Address(RVA = "0x658874", Offset = "0x654874", VA = "0x658874")]
	private static MethodInfo GetMethod(Type t, string name, Type[] ts)
	{
		return null;
	}

	[Token(Token = "0x60003AD")]
	[Address(RVA = "0x658CB4", Offset = "0x654CB4", VA = "0x658CB4")]
	public static object CallSingleMethod(string name, object obj, params object[] args)
	{
		return null;
	}

	[Token(Token = "0x60003AE")]
	[Address(RVA = "0x658D50", Offset = "0x654D50", VA = "0x658D50")]
	public static object CallMethod(string name, object obj, params object[] args)
	{
		return null;
	}
}
