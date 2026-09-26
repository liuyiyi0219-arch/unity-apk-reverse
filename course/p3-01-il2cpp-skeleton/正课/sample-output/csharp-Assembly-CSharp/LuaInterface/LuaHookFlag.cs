using Il2CppDummyDll;

namespace LuaInterface;

[Token(Token = "0x200003E")]
public enum LuaHookFlag
{
	[Token(Token = "0x400006B")]
	LUA_HOOKCALL,
	[Token(Token = "0x400006C")]
	LUA_HOOKRET,
	[Token(Token = "0x400006D")]
	LUA_HOOKLINE,
	[Token(Token = "0x400006E")]
	LUA_HOOKCOUNT,
	[Token(Token = "0x400006F")]
	LUA_HOOKTAILRET
}
