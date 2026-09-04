using Il2CppDummyDll;

namespace LuaInterface;

[Token(Token = "0x2000069")]
public static class LuaValueTypeName
{
	[Token(Token = "0x4000102")]
	[FieldOffset(Offset = "0x0")]
	public static string[] names;

	[Token(Token = "0x600061F")]
	[Address(RVA = "0x676AE4", Offset = "0x672AE4", VA = "0x676AE4")]
	static LuaValueTypeName()
	{
	}

	[Token(Token = "0x6000620")]
	[Address(RVA = "0x676A00", Offset = "0x672A00", VA = "0x676A00")]
	public static string Get(int type)
	{
		return null;
	}
}
