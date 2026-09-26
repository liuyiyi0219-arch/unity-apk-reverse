using System;
using System.Collections;
using System.Collections.Generic;
using Il2CppDummyDll;

namespace LuaInterface;

[Token(Token = "0x2000061")]
public class LuaDictTable : IDisposable, IEnumerable<DictionaryEntry>, IEnumerable
{
	[Token(Token = "0x2000062")]
	private class Enumerator : IEnumerator<DictionaryEntry>, IEnumerator, IDisposable
	{
		[Token(Token = "0x40000E9")]
		[FieldOffset(Offset = "0x10")]
		private LuaState state;

		[Token(Token = "0x40000EA")]
		[FieldOffset(Offset = "0x18")]
		private DictionaryEntry current;

		[Token(Token = "0x40000EB")]
		[FieldOffset(Offset = "0x28")]
		private int top;

		[Token(Token = "0x1700002E")]
		public DictionaryEntry Current
		{
			[Token(Token = "0x60005E9")]
			[Address(RVA = "0x675560", Offset = "0x671560", VA = "0x675560", Slot = "4")]
			get
			{
				return default(DictionaryEntry);
			}
		}

		[Token(Token = "0x1700002F")]
		private object System_002ECollections_002EIEnumerator_002ECurrent
		{
			[Token(Token = "0x60005EA")]
			[Address(RVA = "0x67556C", Offset = "0x67156C", VA = "0x67556C", Slot = "7")]
			get
			{
				return null;
			}
		}

		[Token(Token = "0x60005E8")]
		[Address(RVA = "0x6754DC", Offset = "0x6714DC", VA = "0x6754DC")]
		public Enumerator(LuaDictTable list)
		{
		}

		[Token(Token = "0x60005EB")]
		[Address(RVA = "0x6755C8", Offset = "0x6715C8", VA = "0x6755C8", Slot = "6")]
		public bool MoveNext()
		{
			return default(bool);
		}

		[Token(Token = "0x60005EC")]
		[Address(RVA = "0x67566C", Offset = "0x67166C", VA = "0x67566C", Slot = "8")]
		public void Reset()
		{
		}

		[Token(Token = "0x60005ED")]
		[Address(RVA = "0x675674", Offset = "0x671674", VA = "0x675674", Slot = "5")]
		public void Dispose()
		{
		}
	}

	[Token(Token = "0x40000E7")]
	[FieldOffset(Offset = "0x10")]
	private LuaTable table;

	[Token(Token = "0x40000E8")]
	[FieldOffset(Offset = "0x18")]
	private LuaState state;

	[Token(Token = "0x1700002D")]
	public object this[string key]
	{
		[Token(Token = "0x60005E3")]
		[Address(RVA = "0x675210", Offset = "0x671210", VA = "0x675210")]
		get
		{
			return null;
		}
		[Token(Token = "0x60005E4")]
		[Address(RVA = "0x675228", Offset = "0x671228", VA = "0x675228")]
		set
		{
		}
	}

	[Token(Token = "0x60005E1")]
	[Address(RVA = "0x674B18", Offset = "0x670B18", VA = "0x674B18")]
	public LuaDictTable(LuaTable table)
	{
	}

	[Token(Token = "0x60005E2")]
	[Address(RVA = "0x6751C0", Offset = "0x6711C0", VA = "0x6751C0", Slot = "4")]
	public void Dispose()
	{
	}

	[Token(Token = "0x60005E5")]
	[Address(RVA = "0x675240", Offset = "0x671240", VA = "0x675240")]
	public Hashtable ToHashtable()
	{
		return null;
	}

	[Token(Token = "0x60005E6")]
	[Address(RVA = "0x675484", Offset = "0x671484", VA = "0x675484", Slot = "5")]
	public IEnumerator<DictionaryEntry> GetEnumerator()
	{
		return null;
	}

	[Token(Token = "0x60005E7")]
	[Address(RVA = "0x67555C", Offset = "0x67155C", VA = "0x67555C", Slot = "6")]
	private IEnumerator System_002ECollections_002EIEnumerable_002EGetEnumerator()
	{
		return null;
	}
}
[Token(Token = "0x2000064")]
public class LuaDictTable<K, V> : IDisposable, IEnumerable<LuaDictEntry<K, V>>, IEnumerable
{
	[Token(Token = "0x2000065")]
	private class Enumerator : IEnumerator<LuaDictEntry<K, V>>, IEnumerator, IDisposable
	{
		[Token(Token = "0x40000F0")]
		[FieldOffset(Offset = "0x0")]
		private LuaState state;

		[Token(Token = "0x40000F1")]
		[FieldOffset(Offset = "0x0")]
		private LuaDictEntry<K, V> current;

		[Token(Token = "0x40000F2")]
		[FieldOffset(Offset = "0x0")]
		private int top;

		[Token(Token = "0x17000033")]
		public LuaDictEntry<K, V> Current
		{
			[Token(Token = "0x60005FB")]
			get
			{
				return default(LuaDictEntry<K, V>);
			}
		}

		[Token(Token = "0x17000034")]
		private object System_002ECollections_002EIEnumerator_002ECurrent
		{
			[Token(Token = "0x60005FC")]
			get
			{
				return null;
			}
		}

		[Token(Token = "0x60005FA")]
		public Enumerator(LuaDictTable<K, V> list)
		{
		}

		[Token(Token = "0x60005FD")]
		public bool MoveNext()
		{
			return default(bool);
		}

		[Token(Token = "0x60005FE")]
		public void Reset()
		{
		}

		[Token(Token = "0x60005FF")]
		public void Dispose()
		{
		}
	}

	[Token(Token = "0x40000EE")]
	[FieldOffset(Offset = "0x0")]
	private LuaTable table;

	[Token(Token = "0x40000EF")]
	[FieldOffset(Offset = "0x0")]
	private LuaState state;

	[Token(Token = "0x17000032")]
	public V this[K key]
	{
		[Token(Token = "0x60005F5")]
		get
		{
			return (V)null;
		}
		[Token(Token = "0x60005F6")]
		set
		{
		}
	}

	[Token(Token = "0x60005F3")]
	public LuaDictTable(LuaTable table)
	{
	}

	[Token(Token = "0x60005F4")]
	public void Dispose()
	{
	}

	[Token(Token = "0x60005F7")]
	public Dictionary<K, V> ToDictionary()
	{
		return null;
	}

	[Token(Token = "0x60005F8")]
	public IEnumerator<LuaDictEntry<K, V>> GetEnumerator()
	{
		return null;
	}

	[Token(Token = "0x60005F9")]
	private IEnumerator System_002ECollections_002EIEnumerable_002EGetEnumerator()
	{
		return null;
	}
}
