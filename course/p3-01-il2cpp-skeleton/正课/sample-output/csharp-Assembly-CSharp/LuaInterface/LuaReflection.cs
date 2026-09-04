using System;
using System.Collections.Generic;
using System.Reflection;
using Il2CppDummyDll;

namespace LuaInterface;

[Token(Token = "0x200007E")]
public class LuaReflection : IDisposable
{
	[Token(Token = "0x400013B")]
	[FieldOffset(Offset = "0x10")]
	public List<Assembly> list;

	[Token(Token = "0x400013C")]
	[FieldOffset(Offset = "0x0")]
	private static LuaReflection _reflection;

	[Token(Token = "0x600071D")]
	[Address(RVA = "0x68A4D8", Offset = "0x6864D8", VA = "0x68A4D8")]
	public LuaReflection()
	{
	}

	[Token(Token = "0x600071E")]
	[Address(RVA = "0x68A7A4", Offset = "0x6867A4", VA = "0x68A7A4")]
	public static void OpenLibs(IntPtr L)
	{
	}

	[Token(Token = "0x600071F")]
	[Address(RVA = "0x68ACD0", Offset = "0x686CD0", VA = "0x68ACD0")]
	public static LuaReflection Get(IntPtr L)
	{
		return null;
	}

	[Token(Token = "0x6000720")]
	[Address(RVA = "0x687C6C", Offset = "0x683C6C", VA = "0x687C6C")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	private static int OpenReflectionLibs(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x6000721")]
	[Address(RVA = "0x687E44", Offset = "0x683E44", VA = "0x687E44")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	private static int FindType(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x6000722")]
	[Address(RVA = "0x687FD0", Offset = "0x683FD0", VA = "0x687FD0")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	private static int LoadAssembly(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x6000723")]
	[Address(RVA = "0x68AD18", Offset = "0x686D18", VA = "0x68AD18")]
	private static void PushLuaMethod(IntPtr L, MethodInfo md, Type t, Type[] types)
	{
	}

	[Token(Token = "0x6000724")]
	[Address(RVA = "0x688160", Offset = "0x684160", VA = "0x688160")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	private static int GetMethod(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x6000725")]
	[Address(RVA = "0x68AE28", Offset = "0x686E28", VA = "0x68AE28")]
	private static void PushLuaConstructor(IntPtr L, ConstructorInfo func, Type[] types)
	{
	}

	[Token(Token = "0x6000726")]
	[Address(RVA = "0x688494", Offset = "0x684494", VA = "0x688494")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	private static int GetConstructor(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x6000727")]
	[Address(RVA = "0x688800", Offset = "0x684800", VA = "0x688800")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	private static int GetTypeMethod(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x6000728")]
	[Address(RVA = "0x68AF54", Offset = "0x686F54", VA = "0x68AF54")]
	private static void PushLuaProperty(IntPtr L, PropertyInfo p, Type t)
	{
	}

	[Token(Token = "0x6000729")]
	[Address(RVA = "0x6892A4", Offset = "0x6852A4", VA = "0x6892A4")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	private static int GetProperty(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x600072A")]
	[Address(RVA = "0x68B05C", Offset = "0x68705C", VA = "0x68B05C")]
	private static void PushLuaField(IntPtr L, FieldInfo f, Type t)
	{
	}

	[Token(Token = "0x600072B")]
	[Address(RVA = "0x689E20", Offset = "0x685E20", VA = "0x689E20")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	private static int GetField(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x600072C")]
	[Address(RVA = "0x68A1EC", Offset = "0x6861EC", VA = "0x68A1EC")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	private static int CreateInstance(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x600072D")]
	[Address(RVA = "0x68A5D8", Offset = "0x6865D8", VA = "0x68A5D8")]
	private bool LoadAssembly(string name)
	{
		return default(bool);
	}

	[Token(Token = "0x600072E")]
	[Address(RVA = "0x68B164", Offset = "0x687164", VA = "0x68B164", Slot = "4")]
	public void Dispose()
	{
	}
}
