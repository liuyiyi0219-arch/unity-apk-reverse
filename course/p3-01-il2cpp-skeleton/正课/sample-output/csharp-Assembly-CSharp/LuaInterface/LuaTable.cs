using Il2CppDummyDll;

namespace LuaInterface;

[Token(Token = "0x200005E")]
public class LuaTable : LuaBaseRef
{
	[Token(Token = "0x17000027")]
	public object this[string key]
	{
		[Token(Token = "0x60005B0")]
		[Address(RVA = "0x673948", Offset = "0x66F948", VA = "0x673948")]
		get
		{
			return null;
		}
		[Token(Token = "0x60005B1")]
		[Address(RVA = "0x673AB8", Offset = "0x66FAB8", VA = "0x673AB8")]
		set
		{
		}
	}

	[Token(Token = "0x17000028")]
	public object this[int key]
	{
		[Token(Token = "0x60005B2")]
		[Address(RVA = "0x673C28", Offset = "0x66FC28", VA = "0x673C28")]
		get
		{
			return null;
		}
		[Token(Token = "0x60005B3")]
		[Address(RVA = "0x673D80", Offset = "0x66FD80", VA = "0x673D80")]
		set
		{
		}
	}

	[Token(Token = "0x17000029")]
	public int Length
	{
		[Token(Token = "0x60005B4")]
		[Address(RVA = "0x673ED8", Offset = "0x66FED8", VA = "0x673ED8")]
		get
		{
			return default(int);
		}
	}

	[Token(Token = "0x60005AF")]
	[Address(RVA = "0x673910", Offset = "0x66F910", VA = "0x673910")]
	public LuaTable(int reference, LuaState state)
	{
	}

	[Token(Token = "0x60005B5")]
	public T RawGetIndex<T>(int index)
	{
		return (T)null;
	}

	[Token(Token = "0x60005B6")]
	public void RawSetIndex<T>(int index, T value)
	{
	}

	[Token(Token = "0x60005B7")]
	public V RawGet<K, V>(K key)
	{
		return (V)null;
	}

	[Token(Token = "0x60005B8")]
	public void RawSet<K, V>(K key, V arg)
	{
	}

	[Token(Token = "0x60005B9")]
	public T GetTable<T>(string key)
	{
		return (T)null;
	}

	[Token(Token = "0x60005BA")]
	public void SetTable<T>(string key, T arg)
	{
	}

	[Token(Token = "0x60005BB")]
	[Address(RVA = "0x673F34", Offset = "0x66FF34", VA = "0x673F34")]
	public LuaFunction RawGetLuaFunction(string key)
	{
		return null;
	}

	[Token(Token = "0x60005BC")]
	[Address(RVA = "0x6740A4", Offset = "0x6700A4", VA = "0x6740A4")]
	public LuaFunction GetLuaFunction(string key)
	{
		return null;
	}

	[Token(Token = "0x60005BD")]
	[Address(RVA = "0x674214", Offset = "0x670214", VA = "0x674214")]
	private bool BeginCall(string name, int top)
	{
		return default(bool);
	}

	[Token(Token = "0x60005BE")]
	[Address(RVA = "0x6742A0", Offset = "0x6702A0", VA = "0x6742A0")]
	public void Call(string name)
	{
	}

	[Token(Token = "0x60005BF")]
	public void Call<T1>(string name, T1 arg1)
	{
	}

	[Token(Token = "0x60005C0")]
	public void Call<T1, T2>(string name, T1 arg1, T2 arg2)
	{
	}

	[Token(Token = "0x60005C1")]
	public void Call<T1, T2, T3>(string name, T1 arg1, T2 arg2, T3 arg3)
	{
	}

	[Token(Token = "0x60005C2")]
	public void Call<T1, T2, T3, T4>(string name, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
	{
	}

	[Token(Token = "0x60005C3")]
	public void Call<T1, T2, T3, T4, T5>(string name, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
	{
	}

	[Token(Token = "0x60005C4")]
	public void Call<T1, T2, T3, T4, T5, T6>(string name, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
	{
	}

	[Token(Token = "0x60005C5")]
	public R1 Invoke<R1>(string name)
	{
		return (R1)null;
	}

	[Token(Token = "0x60005C6")]
	public R1 Invoke<T1, R1>(string name, T1 arg1)
	{
		return (R1)null;
	}

	[Token(Token = "0x60005C7")]
	public R1 Invoke<T1, T2, R1>(string name, T1 arg1, T2 arg2)
	{
		return (R1)null;
	}

	[Token(Token = "0x60005C8")]
	public R1 Invoke<T1, T2, T3, R1>(string name, T1 arg1, T2 arg2, T3 arg3)
	{
		return (R1)null;
	}

	[Token(Token = "0x60005C9")]
	public R1 Invoke<T1, T2, T3, T4, R1>(string name, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
	{
		return (R1)null;
	}

	[Token(Token = "0x60005CA")]
	public R1 Invoke<T1, T2, T3, T4, T5, R1>(string name, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
	{
		return (R1)null;
	}

	[Token(Token = "0x60005CB")]
	public R1 Invoke<T1, T2, T3, T4, T5, T6, R1>(string name, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
	{
		return (R1)null;
	}

	[Token(Token = "0x60005CC")]
	[Address(RVA = "0x6743CC", Offset = "0x6703CC", VA = "0x6743CC")]
	public string GetStringField(string name)
	{
		return null;
	}

	[Token(Token = "0x60005CD")]
	[Address(RVA = "0x674524", Offset = "0x670524", VA = "0x674524")]
	public void AddTable(string name)
	{
	}

	[Token(Token = "0x60005CE")]
	[Address(RVA = "0x67468C", Offset = "0x67068C", VA = "0x67468C")]
	public object[] ToArray()
	{
		return null;
	}

	[Token(Token = "0x60005CF")]
	[Address(RVA = "0x674958", Offset = "0x670958", VA = "0x674958", Slot = "3")]
	public override string ToString()
	{
		return null;
	}

	[Token(Token = "0x60005D0")]
	[Address(RVA = "0x674A14", Offset = "0x670A14", VA = "0x674A14")]
	public LuaArrayTable ToArrayTable()
	{
		return null;
	}

	[Token(Token = "0x60005D1")]
	[Address(RVA = "0x674AC0", Offset = "0x670AC0", VA = "0x674AC0")]
	public LuaDictTable ToDictTable()
	{
		return null;
	}

	[Token(Token = "0x60005D2")]
	public LuaDictTable<K, V> ToDictTable<K, V>()
	{
		return null;
	}

	[Token(Token = "0x60005D3")]
	[Address(RVA = "0x674B6C", Offset = "0x670B6C", VA = "0x674B6C")]
	public LuaTable GetMetaTable()
	{
		return null;
	}
}
