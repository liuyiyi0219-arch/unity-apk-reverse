using System;
using System.Runtime.InteropServices;
using Il2CppDummyDll;

namespace LuaInterface;

[Token(Token = "0x2000046")]
public class LuaDLL
{
	[Token(Token = "0x4000089")]
	[Il2CppDummyDll.FieldOffset(Offset = "0x0")]
	public static string version;

	[Token(Token = "0x400008A")]
	[Il2CppDummyDll.FieldOffset(Offset = "0x8")]
	public static int LUA_MULTRET;

	[Token(Token = "0x400008B")]
	[Il2CppDummyDll.FieldOffset(Offset = "0x10")]
	public static string[] LuaTypeName;

	[Token(Token = "0x400008C")]
	private const string LUADLL = "tolua";

	[PreserveSig]
	[Token(Token = "0x6000217")]
	[Address(RVA = "0x647E00", Offset = "0x643E00", VA = "0x647E00")]
	public static extern int luaopen_pb(IntPtr L);

	[PreserveSig]
	[Token(Token = "0x6000218")]
	[Address(RVA = "0x647E7C", Offset = "0x643E7C", VA = "0x647E7C")]
	public static extern int luaopen_ffi(IntPtr L);

	[PreserveSig]
	[Token(Token = "0x6000219")]
	[Address(RVA = "0x647EF8", Offset = "0x643EF8", VA = "0x647EF8")]
	public static extern int luaopen_bit(IntPtr L);

	[PreserveSig]
	[Token(Token = "0x600021A")]
	[Address(RVA = "0x647F74", Offset = "0x643F74", VA = "0x647F74")]
	public static extern int luaopen_struct(IntPtr L);

	[PreserveSig]
	[Token(Token = "0x600021B")]
	[Address(RVA = "0x647FF0", Offset = "0x643FF0", VA = "0x647FF0")]
	public static extern int luaopen_lpeg(IntPtr L);

	[PreserveSig]
	[Token(Token = "0x600021C")]
	[Address(RVA = "0x643ED4", Offset = "0x63FED4", VA = "0x643ED4")]
	public static extern int luaopen_socket_core(IntPtr L);

	[PreserveSig]
	[Token(Token = "0x600021D")]
	[Address(RVA = "0x643F50", Offset = "0x63FF50", VA = "0x643F50")]
	public static extern int luaopen_mime_core(IntPtr L);

	[PreserveSig]
	[Token(Token = "0x600021E")]
	[Address(RVA = "0x64806C", Offset = "0x64406C", VA = "0x64806C")]
	public static extern int luaopen_cjson(IntPtr L);

	[PreserveSig]
	[Token(Token = "0x600021F")]
	[Address(RVA = "0x6480E8", Offset = "0x6440E8", VA = "0x6480E8")]
	public static extern int luaopen_cjson_safe(IntPtr L);

	[Token(Token = "0x6000220")]
	[Address(RVA = "0x64379C", Offset = "0x63F79C", VA = "0x64379C")]
	public static int lua_upvalueindex(int i)
	{
		return default(int);
	}

	[PreserveSig]
	[Token(Token = "0x6000221")]
	[Address(RVA = "0x648164", Offset = "0x644164", VA = "0x648164")]
	public static extern void lua_close(IntPtr luaState);

	[PreserveSig]
	[Token(Token = "0x6000222")]
	[Address(RVA = "0x6481E0", Offset = "0x6441E0", VA = "0x6481E0")]
	public static extern IntPtr lua_newthread(IntPtr L);

	[PreserveSig]
	[Token(Token = "0x6000223")]
	[Address(RVA = "0x64825C", Offset = "0x64425C", VA = "0x64825C")]
	public static extern IntPtr lua_atpanic(IntPtr luaState, IntPtr panic);

	[PreserveSig]
	[Token(Token = "0x6000224")]
	[Address(RVA = "0x627328", Offset = "0x623328", VA = "0x627328")]
	public static extern int lua_gettop(IntPtr luaState);

	[PreserveSig]
	[Token(Token = "0x6000225")]
	[Address(RVA = "0x6482E0", Offset = "0x6442E0", VA = "0x6482E0")]
	public static extern void lua_settop(IntPtr luaState, int top);

	[PreserveSig]
	[Token(Token = "0x6000226")]
	[Address(RVA = "0x648364", Offset = "0x644364", VA = "0x648364")]
	public static extern void lua_pushvalue(IntPtr luaState, int idx);

	[PreserveSig]
	[Token(Token = "0x6000227")]
	[Address(RVA = "0x6483E8", Offset = "0x6443E8", VA = "0x6483E8")]
	public static extern void lua_remove(IntPtr luaState, int idx);

	[PreserveSig]
	[Token(Token = "0x6000228")]
	[Address(RVA = "0x64846C", Offset = "0x64446C", VA = "0x64846C")]
	public static extern void lua_insert(IntPtr luaState, int idx);

	[PreserveSig]
	[Token(Token = "0x6000229")]
	[Address(RVA = "0x6484F0", Offset = "0x6444F0", VA = "0x6484F0")]
	public static extern void lua_replace(IntPtr luaState, int index);

	[PreserveSig]
	[Token(Token = "0x600022A")]
	[Address(RVA = "0x648574", Offset = "0x644574", VA = "0x648574")]
	public static extern int lua_checkstack(IntPtr luaState, int extra);

	[PreserveSig]
	[Token(Token = "0x600022B")]
	[Address(RVA = "0x6485F8", Offset = "0x6445F8", VA = "0x6485F8")]
	public static extern void lua_xmove(IntPtr from, IntPtr to, int n);

	[PreserveSig]
	[Token(Token = "0x600022C")]
	[Address(RVA = "0x64868C", Offset = "0x64468C", VA = "0x64868C")]
	public static extern int lua_isnumber(IntPtr luaState, int idx);

	[PreserveSig]
	[Token(Token = "0x600022D")]
	[Address(RVA = "0x64870C", Offset = "0x64470C", VA = "0x64870C")]
	public static extern int lua_isstring(IntPtr luaState, int index);

	[PreserveSig]
	[Token(Token = "0x600022E")]
	[Address(RVA = "0x64878C", Offset = "0x64478C", VA = "0x64878C")]
	public static extern int lua_iscfunction(IntPtr luaState, int index);

	[PreserveSig]
	[Token(Token = "0x600022F")]
	[Address(RVA = "0x648810", Offset = "0x644810", VA = "0x648810")]
	public static extern int lua_isuserdata(IntPtr luaState, int stackPos);

	[PreserveSig]
	[Token(Token = "0x6000230")]
	[Address(RVA = "0x633B40", Offset = "0x62FB40", VA = "0x633B40")]
	public static extern LuaTypes lua_type(IntPtr luaState, int index);

	[Token(Token = "0x6000231")]
	[Address(RVA = "0x648894", Offset = "0x644894", VA = "0x648894")]
	public static string lua_typename(IntPtr luaState, LuaTypes type)
	{
		return null;
	}

	[PreserveSig]
	[Token(Token = "0x6000232")]
	[Address(RVA = "0x648914", Offset = "0x644914", VA = "0x648914")]
	public static extern int lua_equal(IntPtr luaState, int idx1, int idx2);

	[PreserveSig]
	[Token(Token = "0x6000233")]
	[Address(RVA = "0x6489A8", Offset = "0x6449A8", VA = "0x6489A8")]
	public static extern int lua_rawequal(IntPtr luaState, int idx1, int idx2);

	[PreserveSig]
	[Token(Token = "0x6000234")]
	[Address(RVA = "0x648A3C", Offset = "0x644A3C", VA = "0x648A3C")]
	public static extern int lua_lessthan(IntPtr luaState, int idx1, int idx2);

	[PreserveSig]
	[Token(Token = "0x6000235")]
	[Address(RVA = "0x633D4C", Offset = "0x62FD4C", VA = "0x633D4C")]
	public static extern double lua_tonumber(IntPtr luaState, int idx);

	[Token(Token = "0x6000236")]
	[Address(RVA = "0x648AD0", Offset = "0x644AD0", VA = "0x648AD0")]
	public static int lua_tointeger(IntPtr luaState, int idx)
	{
		return default(int);
	}

	[Token(Token = "0x6000237")]
	[Address(RVA = "0x633DCC", Offset = "0x62FDCC", VA = "0x633DCC")]
	public static bool lua_toboolean(IntPtr luaState, int idx)
	{
		return default(bool);
	}

	[Token(Token = "0x6000238")]
	[Address(RVA = "0x648C44", Offset = "0x644C44", VA = "0x648C44")]
	public static IntPtr lua_tolstring(IntPtr luaState, int index, out int strLen)
	{
		return default(IntPtr);
	}

	[Token(Token = "0x6000239")]
	[Address(RVA = "0x648D44", Offset = "0x644D44", VA = "0x648D44")]
	public static int lua_objlen(IntPtr luaState, int idx)
	{
		return default(int);
	}

	[PreserveSig]
	[Token(Token = "0x600023A")]
	[Address(RVA = "0x648E28", Offset = "0x644E28", VA = "0x648E28")]
	public static extern IntPtr lua_tocfunction(IntPtr luaState, int idx);

	[PreserveSig]
	[Token(Token = "0x600023B")]
	[Address(RVA = "0x648EAC", Offset = "0x644EAC", VA = "0x648EAC")]
	public static extern IntPtr lua_touserdata(IntPtr luaState, int idx);

	[PreserveSig]
	[Token(Token = "0x600023C")]
	[Address(RVA = "0x648F30", Offset = "0x644F30", VA = "0x648F30")]
	public static extern IntPtr lua_tothread(IntPtr L, int idx);

	[PreserveSig]
	[Token(Token = "0x600023D")]
	[Address(RVA = "0x648FB0", Offset = "0x644FB0", VA = "0x648FB0")]
	public static extern IntPtr lua_topointer(IntPtr L, int idx);

	[PreserveSig]
	[Token(Token = "0x600023E")]
	[Address(RVA = "0x649034", Offset = "0x645034", VA = "0x649034")]
	public static extern void lua_pushnil(IntPtr luaState);

	[PreserveSig]
	[Token(Token = "0x600023F")]
	[Address(RVA = "0x6404AC", Offset = "0x63C4AC", VA = "0x6404AC")]
	public static extern void lua_pushnumber(IntPtr luaState, double number);

	[Token(Token = "0x6000240")]
	[Address(RVA = "0x6275E8", Offset = "0x6235E8", VA = "0x6275E8")]
	public static void lua_pushinteger(IntPtr L, int n)
	{
	}

	[Token(Token = "0x6000241")]
	[Address(RVA = "0x6490B0", Offset = "0x6450B0", VA = "0x6490B0")]
	public static void lua_pushlstring(IntPtr luaState, byte[] str, int size)
	{
	}

	[PreserveSig]
	[Token(Token = "0x6000242")]
	[Address(RVA = "0x62764C", Offset = "0x62364C", VA = "0x62764C")]
	public static extern void lua_pushstring(IntPtr luaState, string str);

	[PreserveSig]
	[Token(Token = "0x6000243")]
	[Address(RVA = "0x649214", Offset = "0x645214", VA = "0x649214")]
	public static extern void lua_pushcclosure(IntPtr luaState, IntPtr fn, int n);

	[PreserveSig]
	[Token(Token = "0x6000244")]
	[Address(RVA = "0x6492A8", Offset = "0x6452A8", VA = "0x6492A8")]
	public static extern void lua_pushboolean(IntPtr luaState, int value);

	[Token(Token = "0x6000245")]
	[Address(RVA = "0x627584", Offset = "0x623584", VA = "0x627584")]
	public static void lua_pushboolean(IntPtr luaState, bool value)
	{
	}

	[PreserveSig]
	[Token(Token = "0x6000246")]
	[Address(RVA = "0x64932C", Offset = "0x64532C", VA = "0x64932C")]
	public static extern void lua_pushlightuserdata(IntPtr luaState, IntPtr udata);

	[PreserveSig]
	[Token(Token = "0x6000247")]
	[Address(RVA = "0x6493B0", Offset = "0x6453B0", VA = "0x6493B0")]
	public static extern int lua_pushthread(IntPtr L);

	[Token(Token = "0x6000248")]
	[Address(RVA = "0x64942C", Offset = "0x64542C", VA = "0x64942C")]
	public static void lua_gettable(IntPtr L, int idx)
	{
	}

	[Token(Token = "0x6000249")]
	[Address(RVA = "0x64957C", Offset = "0x64557C", VA = "0x64957C")]
	public static void lua_getfield(IntPtr L, int idx, string key)
	{
	}

	[PreserveSig]
	[Token(Token = "0x600024A")]
	[Address(RVA = "0x649700", Offset = "0x645700", VA = "0x649700")]
	public static extern void lua_rawget(IntPtr luaState, int idx);

	[PreserveSig]
	[Token(Token = "0x600024B")]
	[Address(RVA = "0x649784", Offset = "0x645784", VA = "0x649784")]
	public static extern void lua_rawgeti(IntPtr luaState, int idx, int n);

	[PreserveSig]
	[Token(Token = "0x600024C")]
	[Address(RVA = "0x649818", Offset = "0x645818", VA = "0x649818")]
	public static extern void lua_createtable(IntPtr luaState, int narr, int nrec);

	[Token(Token = "0x600024D")]
	[Address(RVA = "0x6498AC", Offset = "0x6458AC", VA = "0x6498AC")]
	public static IntPtr lua_newuserdata(IntPtr luaState, int size)
	{
		return default(IntPtr);
	}

	[PreserveSig]
	[Token(Token = "0x600024E")]
	[Address(RVA = "0x649994", Offset = "0x645994", VA = "0x649994")]
	public static extern int lua_getmetatable(IntPtr luaState, int objIndex);

	[PreserveSig]
	[Token(Token = "0x600024F")]
	[Address(RVA = "0x649A18", Offset = "0x645A18", VA = "0x649A18")]
	public static extern void lua_getfenv(IntPtr luaState, int idx);

	[Token(Token = "0x6000250")]
	[Address(RVA = "0x649A9C", Offset = "0x645A9C", VA = "0x649A9C")]
	public static void lua_settable(IntPtr L, int idx)
	{
	}

	[Token(Token = "0x6000251")]
	[Address(RVA = "0x649BEC", Offset = "0x645BEC", VA = "0x649BEC")]
	public static void lua_setfield(IntPtr L, int idx, string key)
	{
	}

	[PreserveSig]
	[Token(Token = "0x6000252")]
	[Address(RVA = "0x649D70", Offset = "0x645D70", VA = "0x649D70")]
	public static extern void lua_rawset(IntPtr luaState, int idx);

	[PreserveSig]
	[Token(Token = "0x6000253")]
	[Address(RVA = "0x649DF4", Offset = "0x645DF4", VA = "0x649DF4")]
	public static extern void lua_rawseti(IntPtr luaState, int tableIndex, int index);

	[PreserveSig]
	[Token(Token = "0x6000254")]
	[Address(RVA = "0x649E88", Offset = "0x645E88", VA = "0x649E88")]
	public static extern void lua_setmetatable(IntPtr luaState, int objIndex);

	[PreserveSig]
	[Token(Token = "0x6000255")]
	[Address(RVA = "0x649F0C", Offset = "0x645F0C", VA = "0x649F0C")]
	public static extern int lua_setfenv(IntPtr luaState, int stackPos);

	[PreserveSig]
	[Token(Token = "0x6000256")]
	[Address(RVA = "0x649F90", Offset = "0x645F90", VA = "0x649F90")]
	public static extern void lua_call(IntPtr luaState, int nArgs, int nResults);

	[PreserveSig]
	[Token(Token = "0x6000257")]
	[Address(RVA = "0x64A024", Offset = "0x646024", VA = "0x64A024")]
	public static extern int lua_pcall(IntPtr luaState, int nArgs, int nResults, int errfunc);

	[PreserveSig]
	[Token(Token = "0x6000258")]
	[Address(RVA = "0x64A0C0", Offset = "0x6460C0", VA = "0x64A0C0")]
	public static extern int lua_cpcall(IntPtr L, IntPtr func, IntPtr ud);

	[PreserveSig]
	[Token(Token = "0x6000259")]
	[Address(RVA = "0x64A154", Offset = "0x646154", VA = "0x64A154")]
	public static extern int lua_yield(IntPtr L, int nresults);

	[PreserveSig]
	[Token(Token = "0x600025A")]
	[Address(RVA = "0x64A1D8", Offset = "0x6461D8", VA = "0x64A1D8")]
	public static extern int lua_resume(IntPtr L, int narg);

	[PreserveSig]
	[Token(Token = "0x600025B")]
	[Address(RVA = "0x64A25C", Offset = "0x64625C", VA = "0x64A25C")]
	public static extern int lua_status(IntPtr L);

	[PreserveSig]
	[Token(Token = "0x600025C")]
	[Address(RVA = "0x64A2D8", Offset = "0x6462D8", VA = "0x64A2D8")]
	public static extern int lua_gc(IntPtr luaState, LuaGCOptions what, int data);

	[PreserveSig]
	[Token(Token = "0x600025D")]
	[Address(RVA = "0x64A36C", Offset = "0x64636C", VA = "0x64A36C")]
	public static extern int lua_next(IntPtr luaState, int index);

	[PreserveSig]
	[Token(Token = "0x600025E")]
	[Address(RVA = "0x64A3F0", Offset = "0x6463F0", VA = "0x64A3F0")]
	public static extern void lua_concat(IntPtr luaState, int n);

	[Token(Token = "0x600025F")]
	[Address(RVA = "0x6437FC", Offset = "0x63F7FC", VA = "0x6437FC")]
	public static void lua_pop(IntPtr luaState, int amount)
	{
	}

	[Token(Token = "0x6000260")]
	[Address(RVA = "0x64A474", Offset = "0x646474", VA = "0x64A474")]
	public static void lua_newtable(IntPtr luaState)
	{
	}

	[Token(Token = "0x6000261")]
	[Address(RVA = "0x64A4D0", Offset = "0x6464D0", VA = "0x64A4D0")]
	public static void lua_register(IntPtr luaState, string name, LuaCSFunction func)
	{
	}

	[Token(Token = "0x6000262")]
	[Address(RVA = "0x64A544", Offset = "0x646544", VA = "0x64A544")]
	public static void lua_pushcfunction(IntPtr luaState, LuaCSFunction func)
	{
	}

	[Token(Token = "0x6000263")]
	[Address(RVA = "0x64A694", Offset = "0x646694", VA = "0x64A694")]
	public static bool lua_isfunction(IntPtr luaState, int n)
	{
		return default(bool);
	}

	[Token(Token = "0x6000264")]
	[Address(RVA = "0x64A704", Offset = "0x646704", VA = "0x64A704")]
	public static bool lua_istable(IntPtr luaState, int n)
	{
		return default(bool);
	}

	[Token(Token = "0x6000265")]
	[Address(RVA = "0x64A774", Offset = "0x646774", VA = "0x64A774")]
	public static bool lua_islightuserdata(IntPtr luaState, int n)
	{
		return default(bool);
	}

	[Token(Token = "0x6000266")]
	[Address(RVA = "0x64A7E4", Offset = "0x6467E4", VA = "0x64A7E4")]
	public static bool lua_isnil(IntPtr luaState, int n)
	{
		return default(bool);
	}

	[Token(Token = "0x6000267")]
	[Address(RVA = "0x64A854", Offset = "0x646854", VA = "0x64A854")]
	public static bool lua_isboolean(IntPtr luaState, int n)
	{
		return default(bool);
	}

	[Token(Token = "0x6000268")]
	[Address(RVA = "0x64A8C4", Offset = "0x6468C4", VA = "0x64A8C4")]
	public static bool lua_isthread(IntPtr luaState, int n)
	{
		return default(bool);
	}

	[Token(Token = "0x6000269")]
	[Address(RVA = "0x64A934", Offset = "0x646934", VA = "0x64A934")]
	public static bool lua_isnone(IntPtr luaState, int n)
	{
		return default(bool);
	}

	[Token(Token = "0x600026A")]
	[Address(RVA = "0x64A9A4", Offset = "0x6469A4", VA = "0x64A9A4")]
	public static bool lua_isnoneornil(IntPtr luaState, int n)
	{
		return default(bool);
	}

	[Token(Token = "0x600026B")]
	[Address(RVA = "0x64A5F8", Offset = "0x6465F8", VA = "0x64A5F8")]
	public static void lua_setglobal(IntPtr luaState, string name)
	{
	}

	[Token(Token = "0x600026C")]
	[Address(RVA = "0x64AA14", Offset = "0x646A14", VA = "0x64AA14")]
	public static void lua_getglobal(IntPtr luaState, string name)
	{
	}

	[Token(Token = "0x600026D")]
	[Address(RVA = "0x6479D4", Offset = "0x6439D4", VA = "0x6479D4")]
	public static string lua_ptrtostring(IntPtr str, int len)
	{
		return null;
	}

	[Token(Token = "0x600026E")]
	[Address(RVA = "0x633BC4", Offset = "0x62FBC4", VA = "0x633BC4")]
	public static string lua_tostring(IntPtr luaState, int index)
	{
		return null;
	}

	[Token(Token = "0x600026F")]
	[Address(RVA = "0x64AAB0", Offset = "0x646AB0", VA = "0x64AAB0")]
	public static IntPtr lua_open()
	{
		return default(IntPtr);
	}

	[Token(Token = "0x6000270")]
	[Address(RVA = "0x64AB64", Offset = "0x646B64", VA = "0x64AB64")]
	public static void lua_getregistry(IntPtr L)
	{
	}

	[Token(Token = "0x6000271")]
	[Address(RVA = "0x64ABF0", Offset = "0x646BF0", VA = "0x64ABF0")]
	public static int lua_getgccount(IntPtr L)
	{
		return default(int);
	}

	[PreserveSig]
	[Token(Token = "0x6000272")]
	[Address(RVA = "0x64AC4C", Offset = "0x646C4C", VA = "0x64AC4C")]
	public static extern int lua_getstack(IntPtr L, int level, ref Lua_Debug ar);

	[PreserveSig]
	[Token(Token = "0x6000273")]
	[Address(RVA = "0x64AD64", Offset = "0x646D64", VA = "0x64AD64")]
	public static extern int lua_getinfo(IntPtr L, string what, ref Lua_Debug ar);

	[PreserveSig]
	[Token(Token = "0x6000274")]
	[Address(RVA = "0x64AE90", Offset = "0x646E90", VA = "0x64AE90")]
	public static extern string lua_getlocal(IntPtr L, ref Lua_Debug ar, int n);

	[PreserveSig]
	[Token(Token = "0x6000275")]
	[Address(RVA = "0x64AFB8", Offset = "0x646FB8", VA = "0x64AFB8")]
	public static extern string lua_setlocal(IntPtr L, ref Lua_Debug ar, int n);

	[PreserveSig]
	[Token(Token = "0x6000276")]
	[Address(RVA = "0x64B0E0", Offset = "0x6470E0", VA = "0x64B0E0")]
	public static extern string lua_getupvalue(IntPtr L, int funcindex, int n);

	[PreserveSig]
	[Token(Token = "0x6000277")]
	[Address(RVA = "0x64B18C", Offset = "0x64718C", VA = "0x64B18C")]
	public static extern string lua_setupvalue(IntPtr L, int funcindex, int n);

	[PreserveSig]
	[Token(Token = "0x6000278")]
	[Address(RVA = "0x64B238", Offset = "0x647238", VA = "0x64B238")]
	public static extern int lua_sethook(IntPtr L, LuaHookFunc func, int mask, int count);

	[PreserveSig]
	[Token(Token = "0x6000279")]
	[Address(RVA = "0x64B2DC", Offset = "0x6472DC", VA = "0x64B2DC")]
	public static extern LuaHookFunc lua_gethook(IntPtr L);

	[PreserveSig]
	[Token(Token = "0x600027A")]
	[Address(RVA = "0x64B388", Offset = "0x647388", VA = "0x64B388")]
	public static extern int lua_gethookmask(IntPtr L);

	[PreserveSig]
	[Token(Token = "0x600027B")]
	[Address(RVA = "0x64B404", Offset = "0x647404", VA = "0x64B404")]
	public static extern int lua_gethookcount(IntPtr L);

	[PreserveSig]
	[Token(Token = "0x600027C")]
	[Address(RVA = "0x64B480", Offset = "0x647480", VA = "0x64B480")]
	public static extern void luaL_openlibs(IntPtr luaState);

	[Token(Token = "0x600027D")]
	[Address(RVA = "0x64B4FC", Offset = "0x6474FC", VA = "0x64B4FC")]
	public static int abs_index(IntPtr L, int i)
	{
		return default(int);
	}

	[Token(Token = "0x600027E")]
	[Address(RVA = "0x64B5A4", Offset = "0x6475A4", VA = "0x64B5A4")]
	public static int luaL_getn(IntPtr luaState, int i)
	{
		return default(int);
	}

	[PreserveSig]
	[Token(Token = "0x600027F")]
	[Address(RVA = "0x64B68C", Offset = "0x64768C", VA = "0x64B68C")]
	public static extern int luaL_getmetafield(IntPtr luaState, int stackPos, string field);

	[Token(Token = "0x6000280")]
	[Address(RVA = "0x64B73C", Offset = "0x64773C", VA = "0x64B73C")]
	public static int luaL_callmeta(IntPtr L, int stackPos, string field)
	{
		return default(int);
	}

	[Token(Token = "0x6000281")]
	[Address(RVA = "0x64B86C", Offset = "0x64786C", VA = "0x64B86C")]
	public static int luaL_argerror(IntPtr L, int narg, string extramsg)
	{
		return default(int);
	}

	[Token(Token = "0x6000282")]
	[Address(RVA = "0x64B9FC", Offset = "0x6479FC", VA = "0x64B9FC")]
	public static int luaL_typerror(IntPtr L, int stackPos, string tname, [Optional] string t2)
	{
		return default(int);
	}

	[Token(Token = "0x6000283")]
	[Address(RVA = "0x64BB3C", Offset = "0x647B3C", VA = "0x64BB3C")]
	public static string luaL_checklstring(IntPtr L, int numArg, out int len)
	{
		return null;
	}

	[Token(Token = "0x6000284")]
	[Address(RVA = "0x64BC28", Offset = "0x647C28", VA = "0x64BC28")]
	public static string luaL_optlstring(IntPtr L, int narg, string def, out int len)
	{
		return null;
	}

	[Token(Token = "0x6000285")]
	[Address(RVA = "0x633C70", Offset = "0x62FC70", VA = "0x633C70")]
	public static double luaL_checknumber(IntPtr L, int stackPos)
	{
		return default(double);
	}

	[Token(Token = "0x6000286")]
	[Address(RVA = "0x64BCEC", Offset = "0x647CEC", VA = "0x64BCEC")]
	public static double luaL_optnumber(IntPtr L, int idx, double def)
	{
		return default(double);
	}

	[Token(Token = "0x6000287")]
	[Address(RVA = "0x64BD94", Offset = "0x647D94", VA = "0x64BD94")]
	public static int luaL_checkinteger(IntPtr L, int stackPos)
	{
		return default(int);
	}

	[Token(Token = "0x6000288")]
	[Address(RVA = "0x64BE60", Offset = "0x647E60", VA = "0x64BE60")]
	public static int luaL_optinteger(IntPtr L, int idx, int def)
	{
		return default(int);
	}

	[Token(Token = "0x6000289")]
	[Address(RVA = "0x633E30", Offset = "0x62FE30", VA = "0x633E30")]
	public static bool luaL_checkboolean(IntPtr luaState, int index)
	{
		return default(bool);
	}

	[Token(Token = "0x600028A")]
	[Address(RVA = "0x64BEFC", Offset = "0x647EFC", VA = "0x64BEFC")]
	public static void luaL_checkstack(IntPtr L, int space, string mes)
	{
	}

	[Token(Token = "0x600028B")]
	[Address(RVA = "0x64BFC8", Offset = "0x647FC8", VA = "0x64BFC8")]
	public static void luaL_checktype(IntPtr L, int narg, LuaTypes t)
	{
	}

	[Token(Token = "0x600028C")]
	[Address(RVA = "0x64C074", Offset = "0x648074", VA = "0x64C074")]
	public static void luaL_checkany(IntPtr L, int narg)
	{
	}

	[PreserveSig]
	[Token(Token = "0x600028D")]
	[Address(RVA = "0x64C124", Offset = "0x648124", VA = "0x64C124")]
	public static extern int luaL_newmetatable(IntPtr luaState, string meta);

	[Token(Token = "0x600028E")]
	[Address(RVA = "0x64C1C4", Offset = "0x6481C4", VA = "0x64C1C4")]
	public static IntPtr luaL_checkudata(IntPtr L, int ud, string tname)
	{
		return default(IntPtr);
	}

	[PreserveSig]
	[Token(Token = "0x600028F")]
	[Address(RVA = "0x64C320", Offset = "0x648320", VA = "0x64C320")]
	public static extern void luaL_where(IntPtr luaState, int level);

	[Token(Token = "0x6000290")]
	[Address(RVA = "0x6273A4", Offset = "0x6233A4", VA = "0x6273A4")]
	public static int luaL_throw(IntPtr L, string message)
	{
		return default(int);
	}

	[PreserveSig]
	[Token(Token = "0x6000291")]
	[Address(RVA = "0x64C420", Offset = "0x648420", VA = "0x64C420")]
	public static extern int luaL_ref(IntPtr luaState, int t);

	[PreserveSig]
	[Token(Token = "0x6000292")]
	[Address(RVA = "0x64C4A4", Offset = "0x6484A4", VA = "0x64C4A4")]
	public static extern void luaL_unref(IntPtr luaState, int registryIndex, int reference);

	[PreserveSig]
	[Token(Token = "0x6000293")]
	[Address(RVA = "0x64C538", Offset = "0x648538", VA = "0x64C538")]
	public static extern int luaL_loadfile(IntPtr luaState, string filename);

	[Token(Token = "0x6000294")]
	[Address(RVA = "0x64C5D8", Offset = "0x6485D8", VA = "0x64C5D8")]
	public static int luaL_loadbuffer(IntPtr luaState, byte[] buff, int size, string name)
	{
		return default(int);
	}

	[PreserveSig]
	[Token(Token = "0x6000295")]
	[Address(RVA = "0x64C718", Offset = "0x648718", VA = "0x64C718")]
	public static extern int luaL_loadstring(IntPtr luaState, string chunk);

	[PreserveSig]
	[Token(Token = "0x6000296")]
	[Address(RVA = "0x64AAFC", Offset = "0x646AFC", VA = "0x64AAFC")]
	public static extern IntPtr luaL_newstate();

	[PreserveSig]
	[Token(Token = "0x6000297")]
	[Address(RVA = "0x64C7B8", Offset = "0x6487B8", VA = "0x64C7B8")]
	public static extern IntPtr luaL_gsub(IntPtr luaState, string str, string pattern, string replacement);

	[PreserveSig]
	[Token(Token = "0x6000298")]
	[Address(RVA = "0x64C898", Offset = "0x648898", VA = "0x64C898")]
	public static extern IntPtr luaL_findtable(IntPtr luaState, int idx, string fname, [Optional] int szhint);

	[Token(Token = "0x6000299")]
	[Address(RVA = "0x64BAD0", Offset = "0x647AD0", VA = "0x64BAD0")]
	public static string luaL_typename(IntPtr luaState, int stackPos)
	{
		return null;
	}

	[Token(Token = "0x600029A")]
	[Address(RVA = "0x64C950", Offset = "0x648950", VA = "0x64C950")]
	public static bool luaL_dofile(IntPtr luaState, string fileName)
	{
		return default(bool);
	}

	[Token(Token = "0x600029B")]
	[Address(RVA = "0x64C9F8", Offset = "0x6489F8", VA = "0x64C9F8")]
	public static bool luaL_dostring(IntPtr luaState, string chunk)
	{
		return default(bool);
	}

	[Token(Token = "0x600029C")]
	[Address(RVA = "0x64CAA0", Offset = "0x648AA0", VA = "0x64CAA0")]
	public static void luaL_getmetatable(IntPtr luaState, string meta)
	{
	}

	[Token(Token = "0x600029D")]
	[Address(RVA = "0x64CB3C", Offset = "0x648B3C", VA = "0x64CB3C")]
	public static int lua_ref(IntPtr luaState)
	{
		return default(int);
	}

	[Token(Token = "0x600029E")]
	[Address(RVA = "0x64CBC8", Offset = "0x648BC8", VA = "0x64CBC8")]
	public static void lua_getref(IntPtr luaState, int reference)
	{
	}

	[Token(Token = "0x600029F")]
	[Address(RVA = "0x64CC64", Offset = "0x648C64", VA = "0x64CC64")]
	public static void lua_unref(IntPtr luaState, int reference)
	{
	}

	[PreserveSig]
	[Token(Token = "0x60002A0")]
	[Address(RVA = "0x64CD00", Offset = "0x648D00", VA = "0x64CD00")]
	public static extern void tolua_openlibs(IntPtr L);

	[PreserveSig]
	[Token(Token = "0x60002A1")]
	[Address(RVA = "0x64CD7C", Offset = "0x648D7C", VA = "0x64CD7C")]
	public static extern void tolua_openint64(IntPtr L);

	[PreserveSig]
	[Token(Token = "0x60002A2")]
	[Address(RVA = "0x64CDF8", Offset = "0x648DF8", VA = "0x64CDF8")]
	public static extern int tolua_openlualibs(IntPtr L);

	[PreserveSig]
	[Token(Token = "0x60002A3")]
	[Address(RVA = "0x64CE74", Offset = "0x648E74", VA = "0x64CE74")]
	public static extern IntPtr tolua_tag();

	[PreserveSig]
	[Token(Token = "0x60002A4")]
	[Address(RVA = "0x64CEDC", Offset = "0x648EDC", VA = "0x64CEDC")]
	public static extern void tolua_newudata(IntPtr luaState, int val);

	[PreserveSig]
	[Token(Token = "0x60002A5")]
	[Address(RVA = "0x643860", Offset = "0x63F860", VA = "0x643860")]
	public static extern int tolua_rawnetobj(IntPtr luaState, int obj);

	[PreserveSig]
	[Token(Token = "0x60002A6")]
	[Address(RVA = "0x64CF60", Offset = "0x648F60", VA = "0x64CF60")]
	public static extern bool tolua_pushudata(IntPtr L, int index);

	[PreserveSig]
	[Token(Token = "0x60002A7")]
	[Address(RVA = "0x64CFEC", Offset = "0x648FEC", VA = "0x64CFEC")]
	public static extern bool tolua_pushnewudata(IntPtr L, int metaRef, int index);

	[PreserveSig]
	[Token(Token = "0x60002A8")]
	[Address(RVA = "0x64D088", Offset = "0x649088", VA = "0x64D088")]
	public static extern int tolua_beginpcall(IntPtr L, int reference);

	[PreserveSig]
	[Token(Token = "0x60002A9")]
	[Address(RVA = "0x64C3A4", Offset = "0x6483A4", VA = "0x64C3A4")]
	public static extern void tolua_pushtraceback(IntPtr L);

	[PreserveSig]
	[Token(Token = "0x60002AA")]
	[Address(RVA = "0x64D10C", Offset = "0x64910C", VA = "0x64D10C")]
	public static extern void tolua_getvec2(IntPtr luaState, int stackPos, out float x, out float y);

	[PreserveSig]
	[Token(Token = "0x60002AB")]
	[Address(RVA = "0x64D1A8", Offset = "0x6491A8", VA = "0x64D1A8")]
	public static extern void tolua_getvec3(IntPtr luaState, int stackPos, out float x, out float y, out float z);

	[PreserveSig]
	[Token(Token = "0x60002AC")]
	[Address(RVA = "0x64D254", Offset = "0x649254", VA = "0x64D254")]
	public static extern void tolua_getvec4(IntPtr luaState, int stackPos, out float x, out float y, out float z, out float w);

	[PreserveSig]
	[Token(Token = "0x60002AD")]
	[Address(RVA = "0x64D308", Offset = "0x649308", VA = "0x64D308")]
	public static extern void tolua_getclr(IntPtr luaState, int stackPos, out float r, out float g, out float b, out float a);

	[PreserveSig]
	[Token(Token = "0x60002AE")]
	[Address(RVA = "0x64D3BC", Offset = "0x6493BC", VA = "0x64D3BC")]
	public static extern void tolua_getquat(IntPtr luaState, int stackPos, out float x, out float y, out float z, out float w);

	[PreserveSig]
	[Token(Token = "0x60002AF")]
	[Address(RVA = "0x64D470", Offset = "0x649470", VA = "0x64D470")]
	public static extern int tolua_getlayermask(IntPtr luaState, int stackPos);

	[PreserveSig]
	[Token(Token = "0x60002B0")]
	[Address(RVA = "0x64D4F4", Offset = "0x6494F4", VA = "0x64D4F4")]
	public static extern void tolua_pushvec2(IntPtr luaState, float x, float y);

	[PreserveSig]
	[Token(Token = "0x60002B1")]
	[Address(RVA = "0x64D588", Offset = "0x649588", VA = "0x64D588")]
	public static extern void tolua_pushvec3(IntPtr luaState, float x, float y, float z);

	[PreserveSig]
	[Token(Token = "0x60002B2")]
	[Address(RVA = "0x64D62C", Offset = "0x64962C", VA = "0x64D62C")]
	public static extern void tolua_pushvec4(IntPtr luaState, float x, float y, float z, float w);

	[PreserveSig]
	[Token(Token = "0x60002B3")]
	[Address(RVA = "0x64D6D8", Offset = "0x6496D8", VA = "0x64D6D8")]
	public static extern void tolua_pushquat(IntPtr luaState, float x, float y, float z, float w);

	[PreserveSig]
	[Token(Token = "0x60002B4")]
	[Address(RVA = "0x64D784", Offset = "0x649784", VA = "0x64D784")]
	public static extern void tolua_pushclr(IntPtr luaState, float r, float g, float b, float a);

	[PreserveSig]
	[Token(Token = "0x60002B5")]
	[Address(RVA = "0x64D830", Offset = "0x649830", VA = "0x64D830")]
	public static extern void tolua_pushlayermask(IntPtr luaState, int mask);

	[PreserveSig]
	[Token(Token = "0x60002B6")]
	[Address(RVA = "0x64D8B4", Offset = "0x6498B4", VA = "0x64D8B4")]
	public static extern bool tolua_isint64(IntPtr luaState, int stackPos);

	[PreserveSig]
	[Token(Token = "0x60002B7")]
	[Address(RVA = "0x64D940", Offset = "0x649940", VA = "0x64D940")]
	public static extern long tolua_toint64(IntPtr luaState, int stackPos);

	[Token(Token = "0x60002B8")]
	[Address(RVA = "0x64D9C4", Offset = "0x6499C4", VA = "0x64D9C4")]
	public static long tolua_checkint64(IntPtr L, int stackPos)
	{
		return default(long);
	}

	[PreserveSig]
	[Token(Token = "0x60002B9")]
	[Address(RVA = "0x64DA90", Offset = "0x649A90", VA = "0x64DA90")]
	public static extern void tolua_pushint64(IntPtr luaState, long n);

	[PreserveSig]
	[Token(Token = "0x60002BA")]
	[Address(RVA = "0x64DB14", Offset = "0x649B14", VA = "0x64DB14")]
	public static extern bool tolua_isuint64(IntPtr luaState, int stackPos);

	[PreserveSig]
	[Token(Token = "0x60002BB")]
	[Address(RVA = "0x64DBA0", Offset = "0x649BA0", VA = "0x64DBA0")]
	public static extern ulong tolua_touint64(IntPtr luaState, int stackPos);

	[Token(Token = "0x60002BC")]
	[Address(RVA = "0x64DC24", Offset = "0x649C24", VA = "0x64DC24")]
	public static ulong tolua_checkuint64(IntPtr L, int stackPos)
	{
		return default(ulong);
	}

	[PreserveSig]
	[Token(Token = "0x60002BD")]
	[Address(RVA = "0x64DCF0", Offset = "0x649CF0", VA = "0x64DCF0")]
	public static extern void tolua_pushuint64(IntPtr luaState, ulong n);

	[PreserveSig]
	[Token(Token = "0x60002BE")]
	[Address(RVA = "0x64DD70", Offset = "0x649D70", VA = "0x64DD70")]
	public static extern void tolua_setindex(IntPtr L);

	[PreserveSig]
	[Token(Token = "0x60002BF")]
	[Address(RVA = "0x64DDEC", Offset = "0x649DEC", VA = "0x64DDEC")]
	public static extern void tolua_setnewindex(IntPtr L);

	[PreserveSig]
	[Token(Token = "0x60002C0")]
	[Address(RVA = "0x64DE68", Offset = "0x649E68", VA = "0x64DE68")]
	public static extern int toluaL_ref(IntPtr L);

	[PreserveSig]
	[Token(Token = "0x60002C1")]
	[Address(RVA = "0x64DEE4", Offset = "0x649EE4", VA = "0x64DEE4")]
	public static extern void toluaL_unref(IntPtr L, int reference);

	[PreserveSig]
	[Token(Token = "0x60002C2")]
	[Address(RVA = "0x64DF64", Offset = "0x649F64", VA = "0x64DF64")]
	public static extern IntPtr tolua_getmainstate(IntPtr L);

	[PreserveSig]
	[Token(Token = "0x60002C3")]
	[Address(RVA = "0x64DFE0", Offset = "0x649FE0", VA = "0x64DFE0")]
	public static extern int tolua_getvaluetype(IntPtr L, int stackPos);

	[PreserveSig]
	[Token(Token = "0x60002C4")]
	[Address(RVA = "0x64E064", Offset = "0x64A064", VA = "0x64E064")]
	public static extern bool tolua_createtable(IntPtr L, string fullPath, [Optional] int szhint);

	[PreserveSig]
	[Token(Token = "0x60002C5")]
	[Address(RVA = "0x64E118", Offset = "0x64A118", VA = "0x64E118")]
	public static extern bool tolua_pushluatable(IntPtr L, string fullPath);

	[PreserveSig]
	[Token(Token = "0x60002C6")]
	[Address(RVA = "0x64E1BC", Offset = "0x64A1BC", VA = "0x64E1BC")]
	public static extern bool tolua_beginmodule(IntPtr L, string name);

	[PreserveSig]
	[Token(Token = "0x60002C7")]
	[Address(RVA = "0x64E260", Offset = "0x64A260", VA = "0x64E260")]
	public static extern void tolua_endmodule(IntPtr L);

	[PreserveSig]
	[Token(Token = "0x60002C8")]
	[Address(RVA = "0x64E2DC", Offset = "0x64A2DC", VA = "0x64E2DC")]
	public static extern bool tolua_beginpremodule(IntPtr L, string fullPath, [Optional] int szhint);

	[PreserveSig]
	[Token(Token = "0x60002C9")]
	[Address(RVA = "0x64E38C", Offset = "0x64A38C", VA = "0x64E38C")]
	public static extern void tolua_endpremodule(IntPtr L, int reference);

	[PreserveSig]
	[Token(Token = "0x60002CA")]
	[Address(RVA = "0x64E410", Offset = "0x64A410", VA = "0x64E410")]
	public static extern bool tolua_addpreload(IntPtr L, string path);

	[PreserveSig]
	[Token(Token = "0x60002CB")]
	[Address(RVA = "0x64E4B0", Offset = "0x64A4B0", VA = "0x64E4B0")]
	public static extern int tolua_beginclass(IntPtr L, string name, int baseMetaRef, [Optional] int reference);

	[PreserveSig]
	[Token(Token = "0x60002CC")]
	[Address(RVA = "0x64E568", Offset = "0x64A568", VA = "0x64E568")]
	public static extern void tolua_endclass(IntPtr L);

	[PreserveSig]
	[Token(Token = "0x60002CD")]
	[Address(RVA = "0x64E5E4", Offset = "0x64A5E4", VA = "0x64E5E4")]
	public static extern void tolua_function(IntPtr L, string name, IntPtr fn);

	[PreserveSig]
	[Token(Token = "0x60002CE")]
	[Address(RVA = "0x64E68C", Offset = "0x64A68C", VA = "0x64E68C")]
	public static extern IntPtr tolua_tocbuffer(string name, int sz);

	[PreserveSig]
	[Token(Token = "0x60002CF")]
	[Address(RVA = "0x64E728", Offset = "0x64A728", VA = "0x64E728")]
	public static extern void tolua_freebuffer(IntPtr buffer);

	[PreserveSig]
	[Token(Token = "0x60002D0")]
	[Address(RVA = "0x64E7A4", Offset = "0x64A7A4", VA = "0x64E7A4")]
	public static extern void tolua_variable(IntPtr L, string name, IntPtr get, IntPtr set);

	[PreserveSig]
	[Token(Token = "0x60002D1")]
	[Address(RVA = "0x64E854", Offset = "0x64A854", VA = "0x64E854")]
	public static extern void tolua_constant(IntPtr L, string name, double val);

	[PreserveSig]
	[Token(Token = "0x60002D2")]
	[Address(RVA = "0x64E8FC", Offset = "0x64A8FC", VA = "0x64E8FC")]
	public static extern int tolua_beginenum(IntPtr L, string name);

	[PreserveSig]
	[Token(Token = "0x60002D3")]
	[Address(RVA = "0x64E99C", Offset = "0x64A99C", VA = "0x64E99C")]
	public static extern void tolua_endenum(IntPtr L);

	[PreserveSig]
	[Token(Token = "0x60002D4")]
	[Address(RVA = "0x64EA18", Offset = "0x64AA18", VA = "0x64EA18")]
	public static extern void tolua_beginstaticclass(IntPtr L, string name);

	[PreserveSig]
	[Token(Token = "0x60002D5")]
	[Address(RVA = "0x64EAB0", Offset = "0x64AAB0", VA = "0x64EAB0")]
	public static extern void tolua_endstaticclass(IntPtr L);

	[PreserveSig]
	[Token(Token = "0x60002D6")]
	[Address(RVA = "0x64EB2C", Offset = "0x64AB2C", VA = "0x64EB2C")]
	public static extern int tolua_require(IntPtr L, string fileName);

	[PreserveSig]
	[Token(Token = "0x60002D7")]
	[Address(RVA = "0x64EBCC", Offset = "0x64ABCC", VA = "0x64EBCC")]
	public static extern int tolua_getmetatableref(IntPtr L, int pos);

	[PreserveSig]
	[Token(Token = "0x60002D8")]
	[Address(RVA = "0x64EC50", Offset = "0x64AC50", VA = "0x64EC50")]
	public static extern void tolua_setflag(int bit, bool flag);

	[PreserveSig]
	[Token(Token = "0x60002D9")]
	[Address(RVA = "0x64ECD4", Offset = "0x64ACD4", VA = "0x64ECD4")]
	public static extern bool tolua_isvptrtable(IntPtr L, int index);

	[Token(Token = "0x60002DA")]
	[Address(RVA = "0x627480", Offset = "0x623480", VA = "0x627480")]
	public static int toluaL_exception(IntPtr L, Exception e)
	{
		return default(int);
	}

	[Token(Token = "0x60002DB")]
	[Address(RVA = "0x633EFC", Offset = "0x62FEFC", VA = "0x633EFC")]
	public static int toluaL_exception(IntPtr L, Exception e, object o, string msg)
	{
		return default(int);
	}

	[PreserveSig]
	[Token(Token = "0x60002DC")]
	[Address(RVA = "0x64C654", Offset = "0x648654", VA = "0x64C654")]
	public static extern int tolua_loadbuffer(IntPtr luaState, byte[] buff, int size, string name);

	[PreserveSig]
	[Token(Token = "0x60002DD")]
	[Address(RVA = "0x648BB8", Offset = "0x644BB8", VA = "0x648BB8")]
	public static extern bool tolua_toboolean(IntPtr luaState, int index);

	[PreserveSig]
	[Token(Token = "0x60002DE")]
	[Address(RVA = "0x648B34", Offset = "0x644B34", VA = "0x648B34")]
	public static extern int tolua_tointeger(IntPtr luaState, int idx);

	[PreserveSig]
	[Token(Token = "0x60002DF")]
	[Address(RVA = "0x648CB0", Offset = "0x644CB0", VA = "0x648CB0")]
	public static extern IntPtr tolua_tolstring(IntPtr luaState, int index, out int strLen);

	[PreserveSig]
	[Token(Token = "0x60002E0")]
	[Address(RVA = "0x649178", Offset = "0x645178", VA = "0x649178")]
	public static extern void tolua_pushlstring(IntPtr luaState, byte[] str, int size);

	[PreserveSig]
	[Token(Token = "0x60002E1")]
	[Address(RVA = "0x648DA8", Offset = "0x644DA8", VA = "0x648DA8")]
	public static extern int tolua_objlen(IntPtr luaState, int stackPos);

	[PreserveSig]
	[Token(Token = "0x60002E2")]
	[Address(RVA = "0x649910", Offset = "0x645910", VA = "0x649910")]
	public static extern IntPtr tolua_newuserdata(IntPtr luaState, int size);

	[PreserveSig]
	[Token(Token = "0x60002E3")]
	[Address(RVA = "0x64B94C", Offset = "0x64794C", VA = "0x64B94C")]
	public static extern int tolua_argerror(IntPtr luaState, int narg, string extramsg);

	[PreserveSig]
	[Token(Token = "0x60002E4")]
	[Address(RVA = "0x64ED60", Offset = "0x64AD60", VA = "0x64ED60")]
	public static extern int tolua_error(IntPtr L, string msg);

	[PreserveSig]
	[Token(Token = "0x60002E5")]
	[Address(RVA = "0x649650", Offset = "0x645650", VA = "0x649650")]
	public static extern int tolua_getfield(IntPtr L, int idx, string key);

	[PreserveSig]
	[Token(Token = "0x60002E6")]
	[Address(RVA = "0x649CC0", Offset = "0x645CC0", VA = "0x649CC0")]
	public static extern int tolua_setfield(IntPtr L, int idx, string key);

	[PreserveSig]
	[Token(Token = "0x60002E7")]
	[Address(RVA = "0x6494F8", Offset = "0x6454F8", VA = "0x6494F8")]
	public static extern int tolua_gettable(IntPtr luaState, int idx);

	[PreserveSig]
	[Token(Token = "0x60002E8")]
	[Address(RVA = "0x649B68", Offset = "0x645B68", VA = "0x649B68")]
	public static extern int tolua_settable(IntPtr luaState, int idx);

	[PreserveSig]
	[Token(Token = "0x60002E9")]
	[Address(RVA = "0x64B608", Offset = "0x647608", VA = "0x64B608")]
	public static extern int tolua_getn(IntPtr luaState, int stackPos);

	[PreserveSig]
	[Token(Token = "0x60002EA")]
	[Address(RVA = "0x647958", Offset = "0x643958", VA = "0x647958")]
	public static extern int tolua_strlen(IntPtr str);

	[PreserveSig]
	[Token(Token = "0x60002EB")]
	[Address(RVA = "0x64EE00", Offset = "0x64AE00", VA = "0x64EE00")]
	public static extern void tolua_pushcfunction(IntPtr L, IntPtr fn);

	[Token(Token = "0x60002EC")]
	[Address(RVA = "0x64EE84", Offset = "0x64AE84", VA = "0x64EE84")]
	public static void tolua_pushcfunction(IntPtr luaState, LuaCSFunction func)
	{
	}

	[Token(Token = "0x60002ED")]
	[Address(RVA = "0x64EF34", Offset = "0x64AF34", VA = "0x64EF34")]
	public static string tolua_findtable(IntPtr L, int idx, string name, [Optional] int size)
	{
		return null;
	}

	[Token(Token = "0x60002EE")]
	[Address(RVA = "0x64F018", Offset = "0x64B018", VA = "0x64F018")]
	public static IntPtr tolua_atpanic(IntPtr L, LuaCSFunction func)
	{
		return default(IntPtr);
	}

	[PreserveSig]
	[Token(Token = "0x60002EF")]
	[Address(RVA = "0x64F0C8", Offset = "0x64B0C8", VA = "0x64F0C8")]
	public static extern IntPtr tolua_buffinit(IntPtr luaState);

	[PreserveSig]
	[Token(Token = "0x60002F0")]
	[Address(RVA = "0x64F144", Offset = "0x64B144", VA = "0x64F144")]
	public static extern void tolua_addlstring(IntPtr b, string str, int l);

	[PreserveSig]
	[Token(Token = "0x60002F1")]
	[Address(RVA = "0x64F1EC", Offset = "0x64B1EC", VA = "0x64F1EC")]
	public static extern void tolua_addstring(IntPtr b, string s);

	[PreserveSig]
	[Token(Token = "0x60002F2")]
	[Address(RVA = "0x64F284", Offset = "0x64B284", VA = "0x64F284")]
	public static extern void tolua_addchar(IntPtr b, byte s);

	[PreserveSig]
	[Token(Token = "0x60002F3")]
	[Address(RVA = "0x64F308", Offset = "0x64B308", VA = "0x64F308")]
	public static extern void tolua_pushresult(IntPtr b);

	[PreserveSig]
	[Token(Token = "0x60002F4")]
	[Address(RVA = "0x64F384", Offset = "0x64B384", VA = "0x64F384")]
	public static extern int tolua_update(IntPtr L, float deltaTime, float unscaledDelta);

	[PreserveSig]
	[Token(Token = "0x60002F5")]
	[Address(RVA = "0x64F418", Offset = "0x64B418", VA = "0x64F418")]
	public static extern int tolua_lateupdate(IntPtr L);

	[PreserveSig]
	[Token(Token = "0x60002F6")]
	[Address(RVA = "0x64F494", Offset = "0x64B494", VA = "0x64F494")]
	public static extern int tolua_fixedupdate(IntPtr L, float fixedTime);

	[PreserveSig]
	[Token(Token = "0x60002F7")]
	[Address(RVA = "0x64F520", Offset = "0x64B520", VA = "0x64F520")]
	public static extern void tolua_regthis(IntPtr L, IntPtr get, IntPtr set);

	[PreserveSig]
	[Token(Token = "0x60002F8")]
	[Address(RVA = "0x64F5B4", Offset = "0x64B5B4", VA = "0x64F5B4")]
	public static extern int tolua_where(IntPtr L, int level);

	[Token(Token = "0x60002F9")]
	[Address(RVA = "0x64F638", Offset = "0x64B638", VA = "0x64F638")]
	public static void tolua_bindthis(IntPtr L, LuaCSFunction get, LuaCSFunction set)
	{
	}

	[PreserveSig]
	[Token(Token = "0x60002FA")]
	[Address(RVA = "0x64F71C", Offset = "0x64B71C", VA = "0x64F71C")]
	public static extern int tolua_getclassref(IntPtr L, int pos);

	[Token(Token = "0x60002FB")]
	[Address(RVA = "0x64F7A0", Offset = "0x64B7A0", VA = "0x64F7A0")]
	public LuaDLL()
	{
	}
}
