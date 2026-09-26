using Il2CppDummyDll;

namespace LuaInterface;

[Token(Token = "0x200003B")]
public enum LuaTypes
{
	[Token(Token = "0x4000051")]
	LUA_TNONE = -1,
	[Token(Token = "0x4000052")]
	LUA_TNIL,
	[Token(Token = "0x4000053")]
	LUA_TBOOLEAN,
	[Token(Token = "0x4000054")]
	LUA_TLIGHTUSERDATA,
	[Token(Token = "0x4000055")]
	LUA_TNUMBER,
	[Token(Token = "0x4000056")]
	LUA_TSTRING,
	[Token(Token = "0x4000057")]
	LUA_TTABLE,
	[Token(Token = "0x4000058")]
	LUA_TFUNCTION,
	[Token(Token = "0x4000059")]
	LUA_TUSERDATA,
	[Token(Token = "0x400005A")]
	LUA_TTHREAD
}
