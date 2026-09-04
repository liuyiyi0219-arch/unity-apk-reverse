using System;
using Il2CppDummyDll;

namespace LuaInterface;

[Token(Token = "0x2000077")]
public static class StackTraits<T>
{
	[Token(Token = "0x4000123")]
	[FieldOffset(Offset = "0x0")]
	public static Action<IntPtr, T> Push;

	[Token(Token = "0x4000124")]
	[FieldOffset(Offset = "0x0")]
	public static Func<IntPtr, int, T> Check;

	[Token(Token = "0x4000125")]
	[FieldOffset(Offset = "0x0")]
	public static Func<IntPtr, int, T> To;

	[Token(Token = "0x6000703")]
	public static void Init(Action<IntPtr, T> push, Func<IntPtr, int, T> check, Func<IntPtr, int, T> to)
	{
	}

	[Token(Token = "0x6000704")]
	private static Action<IntPtr, T> SelectPush()
	{
		return null;
	}

	[Token(Token = "0x6000705")]
	private static void PushValue(IntPtr L, T o)
	{
	}

	[Token(Token = "0x6000706")]
	private static void PushObject(IntPtr L, T o)
	{
	}

	[Token(Token = "0x6000707")]
	private static void PushArray(IntPtr L, T array)
	{
	}

	[Token(Token = "0x6000708")]
	private static T DefaultTo(IntPtr L, int pos)
	{
		return (T)null;
	}

	[Token(Token = "0x6000709")]
	private static T DefaultCheck(IntPtr L, int stackPos)
	{
		return (T)null;
	}
}
