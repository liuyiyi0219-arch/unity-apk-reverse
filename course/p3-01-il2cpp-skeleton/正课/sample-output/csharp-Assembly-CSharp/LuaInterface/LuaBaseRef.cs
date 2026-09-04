using System;
using Il2CppDummyDll;

namespace LuaInterface;

[Token(Token = "0x2000039")]
public abstract class LuaBaseRef : IDisposable
{
	[Token(Token = "0x4000044")]
	[FieldOffset(Offset = "0x10")]
	public string name;

	[Token(Token = "0x4000045")]
	[FieldOffset(Offset = "0x18")]
	protected int reference;

	[Token(Token = "0x4000046")]
	[FieldOffset(Offset = "0x20")]
	protected LuaState luaState;

	[Token(Token = "0x4000047")]
	[FieldOffset(Offset = "0x28")]
	protected ObjectTranslator translator;

	[Token(Token = "0x4000048")]
	[FieldOffset(Offset = "0x30")]
	protected bool beDisposed;

	[Token(Token = "0x4000049")]
	[FieldOffset(Offset = "0x34")]
	protected int count;

	[Token(Token = "0x400004A")]
	[FieldOffset(Offset = "0x38")]
	public bool IsAlive;

	[Token(Token = "0x60001F1")]
	[Address(RVA = "0x64726C", Offset = "0x64326C", VA = "0x64726C")]
	public LuaBaseRef()
	{
	}

	[Token(Token = "0x60001F2")]
	[Address(RVA = "0x6472B0", Offset = "0x6432B0", VA = "0x6472B0", Slot = "1")]
	~LuaBaseRef()
	{
	}

	[Token(Token = "0x60001F3")]
	[Address(RVA = "0x647348", Offset = "0x643348", VA = "0x647348", Slot = "5")]
	public virtual void Dispose()
	{
	}

	[Token(Token = "0x60001F4")]
	[Address(RVA = "0x64738C", Offset = "0x64338C", VA = "0x64738C")]
	public void AddRef()
	{
	}

	[Token(Token = "0x60001F5")]
	[Address(RVA = "0x64739C", Offset = "0x64339C", VA = "0x64739C", Slot = "6")]
	public virtual void Dispose(bool disposeManagedResources)
	{
	}

	[Token(Token = "0x60001F6")]
	[Address(RVA = "0x64746C", Offset = "0x64346C", VA = "0x64746C")]
	public void Dispose(int generation)
	{
	}

	[Token(Token = "0x60001F7")]
	[Address(RVA = "0x64748C", Offset = "0x64348C", VA = "0x64748C")]
	public LuaState GetLuaState()
	{
		return null;
	}

	[Token(Token = "0x60001F8")]
	[Address(RVA = "0x647494", Offset = "0x643494", VA = "0x647494")]
	public void Push()
	{
	}

	[Token(Token = "0x60001F9")]
	[Address(RVA = "0x6474B4", Offset = "0x6434B4", VA = "0x6474B4", Slot = "2")]
	public override int GetHashCode()
	{
		return default(int);
	}

	[Token(Token = "0x60001FA")]
	[Address(RVA = "0x6474BC", Offset = "0x6434BC", VA = "0x6474BC", Slot = "7")]
	public virtual int GetReference()
	{
		return default(int);
	}

	[Token(Token = "0x60001FB")]
	[Address(RVA = "0x6474C4", Offset = "0x6434C4", VA = "0x6474C4", Slot = "0")]
	public override bool Equals(object o)
	{
		return default(bool);
	}

	[Token(Token = "0x60001FC")]
	[Address(RVA = "0x647574", Offset = "0x643574", VA = "0x647574")]
	private static bool CompareRef(LuaBaseRef a, LuaBaseRef b)
	{
		return default(bool);
	}

	[Token(Token = "0x60001FD")]
	[Address(RVA = "0x644E84", Offset = "0x640E84", VA = "0x644E84")]
	public static bool operator ==(LuaBaseRef a, LuaBaseRef b)
	{
		return default(bool);
	}

	[Token(Token = "0x60001FE")]
	[Address(RVA = "0x644924", Offset = "0x640924", VA = "0x644924")]
	public static bool operator !=(LuaBaseRef a, LuaBaseRef b)
	{
		return default(bool);
	}
}
