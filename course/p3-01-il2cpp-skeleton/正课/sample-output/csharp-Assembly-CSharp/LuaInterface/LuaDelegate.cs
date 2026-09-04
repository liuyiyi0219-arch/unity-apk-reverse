using System.Reflection;
using Il2CppDummyDll;

namespace LuaInterface;

[Token(Token = "0x2000053")]
public class LuaDelegate
{
	[Token(Token = "0x40000A8")]
	[FieldOffset(Offset = "0x10")]
	public LuaFunction func;

	[Token(Token = "0x40000A9")]
	[FieldOffset(Offset = "0x18")]
	public LuaTable self;

	[Token(Token = "0x40000AA")]
	[FieldOffset(Offset = "0x20")]
	public MethodInfo method;

	[Token(Token = "0x60003BA")]
	[Address(RVA = "0x65913C", Offset = "0x65513C", VA = "0x65913C")]
	public LuaDelegate(LuaFunction func)
	{
	}

	[Token(Token = "0x60003BB")]
	[Address(RVA = "0x65916C", Offset = "0x65516C", VA = "0x65916C")]
	public LuaDelegate(LuaFunction func, LuaTable self)
	{
	}

	[Token(Token = "0x60003BC")]
	[Address(RVA = "0x6591B0", Offset = "0x6551B0", VA = "0x6591B0", Slot = "4")]
	public virtual void Dispose()
	{
	}

	[Token(Token = "0x60003BD")]
	[Address(RVA = "0x659254", Offset = "0x655254", VA = "0x659254", Slot = "0")]
	public override bool Equals(object o)
	{
		return default(bool);
	}

	[Token(Token = "0x60003BE")]
	[Address(RVA = "0x659368", Offset = "0x655368", VA = "0x659368")]
	private static bool CompareLuaDelegate(LuaDelegate a, LuaDelegate b)
	{
		return default(bool);
	}

	[Token(Token = "0x60003BF")]
	[Address(RVA = "0x659364", Offset = "0x655364", VA = "0x659364")]
	public static bool operator ==(LuaDelegate a, LuaDelegate b)
	{
		return default(bool);
	}

	[Token(Token = "0x60003C0")]
	[Address(RVA = "0x659430", Offset = "0x655430", VA = "0x659430")]
	public static bool operator !=(LuaDelegate a, LuaDelegate b)
	{
		return default(bool);
	}

	[Token(Token = "0x60003C1")]
	[Address(RVA = "0x659448", Offset = "0x655448", VA = "0x659448", Slot = "2")]
	public override int GetHashCode()
	{
		return default(int);
	}
}
