using Il2CppDummyDll;

namespace LuaInterface;

[Token(Token = "0x2000041")]
public class LuaRIDX
{
	[Token(Token = "0x4000078")]
	[FieldOffset(Offset = "0x10")]
	public int LUA_RIDX_MAINTHREAD;

	[Token(Token = "0x4000079")]
	[FieldOffset(Offset = "0x14")]
	public int LUA_RIDX_GLOBALS;

	[Token(Token = "0x400007A")]
	[FieldOffset(Offset = "0x18")]
	public int LUA_RIDX_PRELOAD;

	[Token(Token = "0x400007B")]
	[FieldOffset(Offset = "0x1C")]
	public int LUA_RIDX_LOADED;

	[Token(Token = "0x6000207")]
	[Address(RVA = "0x6478A4", Offset = "0x6438A4", VA = "0x6478A4")]
	public LuaRIDX()
	{
	}
}
