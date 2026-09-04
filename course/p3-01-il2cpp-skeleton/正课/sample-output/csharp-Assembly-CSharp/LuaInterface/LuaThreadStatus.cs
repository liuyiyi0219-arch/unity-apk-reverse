using Il2CppDummyDll;

namespace LuaInterface;

[Token(Token = "0x200003D")]
public enum LuaThreadStatus
{
	[Token(Token = "0x4000065")]
	LUA_YIELD = 1,
	[Token(Token = "0x4000066")]
	LUA_ERRRUN,
	[Token(Token = "0x4000067")]
	LUA_ERRSYNTAX,
	[Token(Token = "0x4000068")]
	LUA_ERRMEM,
	[Token(Token = "0x4000069")]
	LUA_ERRERR
}
