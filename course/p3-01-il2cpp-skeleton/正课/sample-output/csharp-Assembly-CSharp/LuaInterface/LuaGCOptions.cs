using Il2CppDummyDll;

namespace LuaInterface;

[Token(Token = "0x200003C")]
public enum LuaGCOptions
{
	[Token(Token = "0x400005C")]
	LUA_GCSTOP,
	[Token(Token = "0x400005D")]
	LUA_GCRESTART,
	[Token(Token = "0x400005E")]
	LUA_GCCOLLECT,
	[Token(Token = "0x400005F")]
	LUA_GCCOUNT,
	[Token(Token = "0x4000060")]
	LUA_GCCOUNTB,
	[Token(Token = "0x4000061")]
	LUA_GCSTEP,
	[Token(Token = "0x4000062")]
	LUA_GCSETPAUSE,
	[Token(Token = "0x4000063")]
	LUA_GCSETSTEPMUL
}
