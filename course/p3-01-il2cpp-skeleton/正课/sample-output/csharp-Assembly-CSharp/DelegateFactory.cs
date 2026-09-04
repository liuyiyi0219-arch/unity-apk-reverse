using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Il2CppDummyDll;
using LuaInterface;

[Token(Token = "0x200000D")]
public class DelegateFactory
{
	[Token(Token = "0x200000E")]
	public delegate Delegate DelegateCreate(LuaFunction func, LuaTable self, bool flag);

	[Token(Token = "0x4000016")]
	[Il2CppDummyDll.FieldOffset(Offset = "0x0")]
	public static Dictionary<Type, DelegateCreate> dict;

	[Token(Token = "0x4000017")]
	[Il2CppDummyDll.FieldOffset(Offset = "0x8")]
	private static DelegateFactory factory;

	[Token(Token = "0x600001D")]
	[Address(RVA = "0x60202C", Offset = "0x5FE02C", VA = "0x60202C")]
	public static void Init()
	{
	}

	[Token(Token = "0x600001E")]
	[Address(RVA = "0x602078", Offset = "0x5FE078", VA = "0x602078")]
	public static void Register()
	{
	}

	[Token(Token = "0x600001F")]
	[Address(RVA = "0x6020F0", Offset = "0x5FE0F0", VA = "0x6020F0")]
	public static Delegate CreateDelegate(Type t, [Optional] LuaFunction func)
	{
		return null;
	}

	[Token(Token = "0x6000020")]
	[Address(RVA = "0x602314", Offset = "0x5FE314", VA = "0x602314")]
	public static Delegate CreateDelegate(Type t, LuaFunction func, LuaTable self)
	{
		return null;
	}

	[Token(Token = "0x6000021")]
	[Address(RVA = "0x60254C", Offset = "0x5FE54C", VA = "0x60254C")]
	public static Delegate RemoveDelegate(Delegate obj, LuaFunction func)
	{
		return null;
	}

	[Token(Token = "0x6000022")]
	[Address(RVA = "0x6026AC", Offset = "0x5FE6AC", VA = "0x6026AC")]
	public static Delegate RemoveDelegate(Delegate obj, Delegate dg)
	{
		return null;
	}

	[Token(Token = "0x6000023")]
	[Address(RVA = "0x60289C", Offset = "0x5FE89C", VA = "0x60289C")]
	public DelegateFactory()
	{
	}
}
