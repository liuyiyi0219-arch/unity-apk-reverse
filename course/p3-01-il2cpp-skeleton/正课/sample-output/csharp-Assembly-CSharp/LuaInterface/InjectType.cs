using System;
using Il2CppDummyDll;

namespace LuaInterface;

[Token(Token = "0x2000078")]
[Flags]
public enum InjectType
{
	[Token(Token = "0x4000127")]
	None = 0,
	[Token(Token = "0x4000128")]
	After = 1,
	[Token(Token = "0x4000129")]
	Before = 2,
	[Token(Token = "0x400012A")]
	Replace = 4,
	[Token(Token = "0x400012B")]
	ReplaceWithPreInvokeBase = 8,
	[Token(Token = "0x400012C")]
	ReplaceWithPostInvokeBase = 0x10
}
