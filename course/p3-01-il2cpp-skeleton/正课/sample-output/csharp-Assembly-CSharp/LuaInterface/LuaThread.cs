using System;
using Il2CppDummyDll;

namespace LuaInterface;

[Token(Token = "0x2000066")]
public class LuaThread : LuaBaseRef
{
	[Token(Token = "0x6000600")]
	[Address(RVA = "0x67571C", Offset = "0x67171C", VA = "0x67571C")]
	public LuaThread(int reference, LuaState state)
	{
	}

	[Token(Token = "0x6000601")]
	[Address(RVA = "0x675758", Offset = "0x671758", VA = "0x675758")]
	protected int Resume(IntPtr L, int nArgs)
	{
		return default(int);
	}

	[Token(Token = "0x6000602")]
	[Address(RVA = "0x6758E4", Offset = "0x6718E4", VA = "0x6758E4")]
	public int Resume()
	{
		return default(int);
	}

	[Token(Token = "0x6000603")]
	public int Resume<T1>(T1 arg1)
	{
		return default(int);
	}

	[Token(Token = "0x6000604")]
	public int Resume<T1, T2>(T1 arg1, T2 arg2)
	{
		return default(int);
	}

	[Token(Token = "0x6000605")]
	public int Resume<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3)
	{
		return default(int);
	}

	[Token(Token = "0x6000606")]
	public int Resume<R1>(out R1 ret1)
	{
		return default(int);
	}

	[Token(Token = "0x6000607")]
	public int Resume<T1, R1>(T1 arg1, out R1 ret1)
	{
		return default(int);
	}

	[Token(Token = "0x6000608")]
	public int Resume<T1, T2, R1>(T1 arg1, T2 arg2, out R1 ret1)
	{
		return default(int);
	}

	[Token(Token = "0x6000609")]
	public int Resume<T1, T2, T3, R1>(T1 arg1, T2 arg2, T3 arg3, out R1 ret1)
	{
		return default(int);
	}
}
