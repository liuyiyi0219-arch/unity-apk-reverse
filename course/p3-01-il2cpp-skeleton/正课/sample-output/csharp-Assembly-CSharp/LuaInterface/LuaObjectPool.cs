using System;
using System.Collections.Generic;
using Il2CppDummyDll;

namespace LuaInterface;

[Token(Token = "0x200006A")]
public class LuaObjectPool
{
	[Token(Token = "0x200006B")]
	private class PoolNode
	{
		[Token(Token = "0x4000108")]
		[FieldOffset(Offset = "0x10")]
		public int index;

		[Token(Token = "0x4000109")]
		[FieldOffset(Offset = "0x18")]
		public object obj;

		[Token(Token = "0x600062A")]
		[Address(RVA = "0x67700C", Offset = "0x67300C", VA = "0x67700C")]
		public PoolNode(int index, object obj)
		{
		}
	}

	[Token(Token = "0x4000103")]
	[FieldOffset(Offset = "0x10")]
	private List<PoolNode> list;

	[Token(Token = "0x4000104")]
	[FieldOffset(Offset = "0x18")]
	private PoolNode head;

	[Token(Token = "0x4000105")]
	[FieldOffset(Offset = "0x20")]
	private int count;

	[Token(Token = "0x4000106")]
	[FieldOffset(Offset = "0x24")]
	private int collectStep;

	[Token(Token = "0x4000107")]
	[FieldOffset(Offset = "0x28")]
	private int collectedIndex;

	[Token(Token = "0x17000035")]
	public object this[int i]
	{
		[Token(Token = "0x6000622")]
		[Address(RVA = "0x677044", Offset = "0x673044", VA = "0x677044")]
		get
		{
			return null;
		}
	}

	[Token(Token = "0x6000621")]
	[Address(RVA = "0x676DF0", Offset = "0x672DF0", VA = "0x676DF0")]
	public LuaObjectPool()
	{
	}

	[Token(Token = "0x6000623")]
	[Address(RVA = "0x6770C4", Offset = "0x6730C4", VA = "0x6770C4")]
	public void Clear()
	{
	}

	[Token(Token = "0x6000624")]
	[Address(RVA = "0x677140", Offset = "0x673140", VA = "0x677140")]
	public int Add(object obj)
	{
		return default(int);
	}

	[Token(Token = "0x6000625")]
	[Address(RVA = "0x6772C0", Offset = "0x6732C0", VA = "0x6772C0")]
	public object TryGetValue(int index)
	{
		return null;
	}

	[Token(Token = "0x6000626")]
	[Address(RVA = "0x677340", Offset = "0x673340", VA = "0x677340")]
	public object Remove(int pos)
	{
		return null;
	}

	[Token(Token = "0x6000627")]
	[Address(RVA = "0x677420", Offset = "0x673420", VA = "0x677420")]
	public object Destroy(int pos)
	{
		return null;
	}

	[Token(Token = "0x6000628")]
	[Address(RVA = "0x6774CC", Offset = "0x6734CC", VA = "0x6774CC")]
	public void StepCollect(Action<object, int> collectListener)
	{
	}

	[Token(Token = "0x6000629")]
	[Address(RVA = "0x6775F4", Offset = "0x6735F4", VA = "0x6775F4")]
	public object Replace(int pos, object o)
	{
		return null;
	}
}
