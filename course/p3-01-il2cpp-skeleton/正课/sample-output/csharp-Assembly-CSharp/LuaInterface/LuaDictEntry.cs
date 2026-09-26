using System.Runtime.CompilerServices;
using Il2CppDummyDll;

namespace LuaInterface;

[Token(Token = "0x2000063")]
public struct LuaDictEntry<K, V>
{
	[Token(Token = "0x17000030")]
	public K Key
	{
		[Token(Token = "0x60005EF")]
		[CompilerGenerated]
		readonly get
		{
			return (K)null;
		}
		[Token(Token = "0x60005F0")]
		[CompilerGenerated]
		set
		{
		}
	}

	[Token(Token = "0x17000031")]
	public V Value
	{
		[Token(Token = "0x60005F1")]
		[CompilerGenerated]
		readonly get
		{
			return (V)null;
		}
		[Token(Token = "0x60005F2")]
		[CompilerGenerated]
		set
		{
		}
	}

	[Token(Token = "0x60005EE")]
	public LuaDictEntry(K key, V value)
	{
	}
}
