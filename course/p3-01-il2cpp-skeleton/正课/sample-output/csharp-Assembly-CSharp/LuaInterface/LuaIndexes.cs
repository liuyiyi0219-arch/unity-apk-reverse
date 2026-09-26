using Il2CppDummyDll;

namespace LuaInterface;

[Token(Token = "0x2000040")]
public class LuaIndexes
{
	[Token(Token = "0x4000075")]
	[FieldOffset(Offset = "0x0")]
	public static int LUA_REGISTRYINDEX;

	[Token(Token = "0x4000076")]
	[FieldOffset(Offset = "0x4")]
	public static int LUA_ENVIRONINDEX;

	[Token(Token = "0x4000077")]
	[FieldOffset(Offset = "0x8")]
	public static int LUA_GLOBALSINDEX;

	[Token(Token = "0x6000205")]
	[Address(RVA = "0x647844", Offset = "0x643844", VA = "0x647844")]
	public LuaIndexes()
	{
	}
}
