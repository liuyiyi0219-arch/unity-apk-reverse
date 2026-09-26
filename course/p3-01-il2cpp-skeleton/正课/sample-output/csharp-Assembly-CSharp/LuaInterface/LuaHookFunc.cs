using System;
using Il2CppDummyDll;

namespace LuaInterface;

[Token(Token = "0x2000045")]
public delegate void LuaHookFunc(IntPtr L, ref Lua_Debug ar);
