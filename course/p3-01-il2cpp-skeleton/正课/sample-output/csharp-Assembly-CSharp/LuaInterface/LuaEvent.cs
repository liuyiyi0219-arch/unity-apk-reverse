using System;
using Il2CppDummyDll;

namespace LuaInterface;

[Token(Token = "0x2000047")]
public class LuaEvent : IDisposable
{
	[Token(Token = "0x400008D")]
	[FieldOffset(Offset = "0x10")]
	protected LuaState luaState;

	[Token(Token = "0x400008E")]
	[FieldOffset(Offset = "0x18")]
	protected bool beDisposed;

	[Token(Token = "0x400008F")]
	[FieldOffset(Offset = "0x20")]
	private LuaTable self;

	[Token(Token = "0x4000090")]
	[FieldOffset(Offset = "0x28")]
	private LuaFunction _add;

	[Token(Token = "0x4000091")]
	[FieldOffset(Offset = "0x30")]
	private LuaFunction _remove;

	[Token(Token = "0x60002FD")]
	[Address(RVA = "0x64FA70", Offset = "0x64BA70", VA = "0x64FA70")]
	public LuaEvent(LuaTable table)
	{
	}

	[Token(Token = "0x60002FE")]
	[Address(RVA = "0x64FB5C", Offset = "0x64BB5C", VA = "0x64FB5C", Slot = "4")]
	public void Dispose()
	{
	}

	[Token(Token = "0x60002FF")]
	[Address(RVA = "0x64FBB0", Offset = "0x64BBB0", VA = "0x64FBB0")]
	private void Clear()
	{
	}

	[Token(Token = "0x6000300")]
	[Address(RVA = "0x64FBF8", Offset = "0x64BBF8", VA = "0x64FBF8")]
	public void Dispose(bool disposeManagedResources)
	{
	}

	[Token(Token = "0x6000301")]
	[Address(RVA = "0x64FCE4", Offset = "0x64BCE4", VA = "0x64FCE4")]
	public void Add(LuaFunction func, LuaTable obj)
	{
	}

	[Token(Token = "0x6000302")]
	[Address(RVA = "0x64FEF0", Offset = "0x64BEF0", VA = "0x64FEF0")]
	public void Remove(LuaFunction func, LuaTable obj)
	{
	}
}
