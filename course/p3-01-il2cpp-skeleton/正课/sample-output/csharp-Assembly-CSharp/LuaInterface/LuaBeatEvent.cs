using System;
using Il2CppDummyDll;

namespace LuaInterface;

[Token(Token = "0x200003A")]
public class LuaBeatEvent : IDisposable
{
	[Token(Token = "0x400004B")]
	[FieldOffset(Offset = "0x10")]
	protected LuaState luaState;

	[Token(Token = "0x400004C")]
	[FieldOffset(Offset = "0x18")]
	protected bool beDisposed;

	[Token(Token = "0x400004D")]
	[FieldOffset(Offset = "0x20")]
	private LuaTable self;

	[Token(Token = "0x400004E")]
	[FieldOffset(Offset = "0x28")]
	private LuaFunction _add;

	[Token(Token = "0x400004F")]
	[FieldOffset(Offset = "0x30")]
	private LuaFunction _remove;

	[Token(Token = "0x60001FF")]
	[Address(RVA = "0x6463D8", Offset = "0x6423D8", VA = "0x6463D8")]
	public LuaBeatEvent(LuaTable table)
	{
	}

	[Token(Token = "0x6000200")]
	[Address(RVA = "0x646684", Offset = "0x642684", VA = "0x646684", Slot = "4")]
	public void Dispose()
	{
	}

	[Token(Token = "0x6000201")]
	[Address(RVA = "0x6475D4", Offset = "0x6435D4", VA = "0x6475D4")]
	private void Clear()
	{
	}

	[Token(Token = "0x6000202")]
	[Address(RVA = "0x64761C", Offset = "0x64361C", VA = "0x64761C")]
	public void Dispose(bool disposeManagedResources)
	{
	}

	[Token(Token = "0x6000203")]
	[Address(RVA = "0x6476EC", Offset = "0x6436EC", VA = "0x6476EC")]
	public void Add(LuaFunction func, LuaTable obj)
	{
	}

	[Token(Token = "0x6000204")]
	[Address(RVA = "0x647798", Offset = "0x643798", VA = "0x647798")]
	public void Remove(LuaFunction func, LuaTable obj)
	{
	}
}
