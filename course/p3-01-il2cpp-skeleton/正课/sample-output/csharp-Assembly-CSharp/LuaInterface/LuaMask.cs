using Il2CppDummyDll;

namespace LuaInterface;

[Token(Token = "0x200003F")]
public enum LuaMask
{
	[Token(Token = "0x4000071")]
	LUA_MASKCALL = 1,
	[Token(Token = "0x4000072")]
	LUA_MASKRET = 2,
	[Token(Token = "0x4000073")]
	LUA_MASKLINE = 4,
	[Token(Token = "0x4000074")]
	LUA_MASKCOUNT = 8
}
