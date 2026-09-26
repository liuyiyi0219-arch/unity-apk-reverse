using System;
using System.Collections;
using System.Collections.Generic;
using Il2CppDummyDll;

namespace LuaInterface;

[Token(Token = "0x200005F")]
public class LuaArrayTable : IDisposable, IEnumerable<object>, IEnumerable
{
	[Token(Token = "0x2000060")]
	private class Enumerator : IEnumerator<object>, IEnumerator, IDisposable
	{
		[Token(Token = "0x40000E3")]
		[FieldOffset(Offset = "0x10")]
		private LuaState state;

		[Token(Token = "0x40000E4")]
		[FieldOffset(Offset = "0x18")]
		private int index;

		[Token(Token = "0x40000E5")]
		[FieldOffset(Offset = "0x20")]
		private object current;

		[Token(Token = "0x40000E6")]
		[FieldOffset(Offset = "0x28")]
		private int top;

		[Token(Token = "0x1700002C")]
		public object Current
		{
			[Token(Token = "0x60005DD")]
			[Address(RVA = "0x675074", Offset = "0x671074", VA = "0x675074", Slot = "7")]
			get
			{
				return null;
			}
		}

		[Token(Token = "0x60005DC")]
		[Address(RVA = "0x674FF8", Offset = "0x670FF8", VA = "0x674FF8")]
		public Enumerator(LuaArrayTable list)
		{
		}

		[Token(Token = "0x60005DE")]
		[Address(RVA = "0x67507C", Offset = "0x67107C", VA = "0x67507C", Slot = "6")]
		public bool MoveNext()
		{
			return default(bool);
		}

		[Token(Token = "0x60005DF")]
		[Address(RVA = "0x675104", Offset = "0x671104", VA = "0x675104", Slot = "8")]
		public void Reset()
		{
		}

		[Token(Token = "0x60005E0")]
		[Address(RVA = "0x675118", Offset = "0x671118", VA = "0x675118", Slot = "5")]
		public void Dispose()
		{
		}
	}

	[Token(Token = "0x40000E1")]
	[FieldOffset(Offset = "0x10")]
	private LuaTable table;

	[Token(Token = "0x40000E2")]
	[FieldOffset(Offset = "0x18")]
	private LuaState state;

	[Token(Token = "0x1700002A")]
	public int Length
	{
		[Token(Token = "0x60005D6")]
		[Address(RVA = "0x674D1C", Offset = "0x670D1C", VA = "0x674D1C")]
		get
		{
			return default(int);
		}
	}

	[Token(Token = "0x1700002B")]
	public object this[int key]
	{
		[Token(Token = "0x60005D7")]
		[Address(RVA = "0x674D34", Offset = "0x670D34", VA = "0x674D34")]
		get
		{
			return null;
		}
		[Token(Token = "0x60005D8")]
		[Address(RVA = "0x674D4C", Offset = "0x670D4C", VA = "0x674D4C")]
		set
		{
		}
	}

	[Token(Token = "0x60005D4")]
	[Address(RVA = "0x674A6C", Offset = "0x670A6C", VA = "0x674A6C")]
	public LuaArrayTable(LuaTable table)
	{
	}

	[Token(Token = "0x60005D5")]
	[Address(RVA = "0x674CCC", Offset = "0x670CCC", VA = "0x674CCC", Slot = "4")]
	public void Dispose()
	{
	}

	[Token(Token = "0x60005D9")]
	[Address(RVA = "0x674D64", Offset = "0x670D64", VA = "0x674D64")]
	public void ForEach(Action<object> action)
	{
	}

	[Token(Token = "0x60005DA")]
	[Address(RVA = "0x674FA0", Offset = "0x670FA0", VA = "0x674FA0", Slot = "5")]
	public IEnumerator<object> GetEnumerator()
	{
		return null;
	}

	[Token(Token = "0x60005DB")]
	[Address(RVA = "0x675070", Offset = "0x671070", VA = "0x675070", Slot = "6")]
	private IEnumerator System_002ECollections_002EIEnumerable_002EGetEnumerator()
	{
		return null;
	}
}
