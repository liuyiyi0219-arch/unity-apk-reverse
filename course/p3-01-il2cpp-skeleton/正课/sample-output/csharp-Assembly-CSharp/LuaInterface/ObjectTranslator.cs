using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Il2CppDummyDll;
using UnityEngine;

namespace LuaInterface;

[Token(Token = "0x200006C")]
public class ObjectTranslator
{
	[Token(Token = "0x200006D")]
	private class DelayGC
	{
		[Token(Token = "0x4000110")]
		[FieldOffset(Offset = "0x10")]
		public int id;

		[Token(Token = "0x4000111")]
		[FieldOffset(Offset = "0x18")]
		public UnityEngine.Object obj;

		[Token(Token = "0x4000112")]
		[FieldOffset(Offset = "0x20")]
		public float time;

		[Token(Token = "0x600063D")]
		[Address(RVA = "0x677E9C", Offset = "0x673E9C", VA = "0x677E9C")]
		public DelayGC(int id, UnityEngine.Object obj, float time)
		{
		}
	}

	[Token(Token = "0x200006E")]
	private class CompareObject : IEqualityComparer<object>
	{
		[Token(Token = "0x600063E")]
		[Address(RVA = "0x6783B4", Offset = "0x6743B4", VA = "0x6783B4", Slot = "4")]
		public new bool Equals(object x, object y)
		{
			return default(bool);
		}

		[Token(Token = "0x600063F")]
		[Address(RVA = "0x6783C0", Offset = "0x6743C0", VA = "0x6783C0", Slot = "5")]
		public int GetHashCode(object obj)
		{
			return default(int);
		}

		[Token(Token = "0x6000640")]
		[Address(RVA = "0x677898", Offset = "0x673898", VA = "0x677898")]
		public CompareObject()
		{
		}
	}

	[Token(Token = "0x400010B")]
	[FieldOffset(Offset = "0x18")]
	public readonly Dictionary<object, int> objectsBackMap;

	[Token(Token = "0x400010C")]
	[FieldOffset(Offset = "0x20")]
	public readonly LuaObjectPool objects;

	[Token(Token = "0x400010D")]
	[FieldOffset(Offset = "0x28")]
	private List<DelayGC> gcList;

	[Token(Token = "0x400010E")]
	[FieldOffset(Offset = "0x30")]
	private Action<object, int> removeInvalidObject;

	[Token(Token = "0x400010F")]
	[FieldOffset(Offset = "0x0")]
	private static ObjectTranslator _translator;

	[Token(Token = "0x17000036")]
	public bool LogGC
	{
		[Token(Token = "0x600062B")]
		[Address(RVA = "0x6776AC", Offset = "0x6736AC", VA = "0x6776AC")]
		[CompilerGenerated]
		get
		{
			return default(bool);
		}
		[Token(Token = "0x600062C")]
		[Address(RVA = "0x6776B4", Offset = "0x6736B4", VA = "0x6776B4")]
		[CompilerGenerated]
		set
		{
		}
	}

	[Token(Token = "0x600062D")]
	[Address(RVA = "0x6776C0", Offset = "0x6736C0", VA = "0x6776C0")]
	public ObjectTranslator()
	{
	}

	[Token(Token = "0x600062E")]
	[Address(RVA = "0x6778A0", Offset = "0x6738A0", VA = "0x6778A0")]
	public int AddObject(object obj)
	{
		return default(int);
	}

	[Token(Token = "0x600062F")]
	[Address(RVA = "0x6779A8", Offset = "0x6739A8", VA = "0x6779A8")]
	public static ObjectTranslator Get(IntPtr L)
	{
		return null;
	}

	[Token(Token = "0x6000630")]
	[Address(RVA = "0x6779F0", Offset = "0x6739F0", VA = "0x6779F0")]
	private void RemoveObject(object o, int udata)
	{
	}

	[Token(Token = "0x6000631")]
	[Address(RVA = "0x677A9C", Offset = "0x673A9C", VA = "0x677A9C")]
	public void RemoveObject(int udata)
	{
	}

	[Token(Token = "0x6000632")]
	[Address(RVA = "0x677BC0", Offset = "0x673BC0", VA = "0x677BC0")]
	public object GetObject(int udata)
	{
		return null;
	}

	[Token(Token = "0x6000633")]
	[Address(RVA = "0x677BD8", Offset = "0x673BD8", VA = "0x677BD8")]
	public void Destroy(int udata)
	{
	}

	[Token(Token = "0x6000634")]
	[Address(RVA = "0x677CFC", Offset = "0x673CFC", VA = "0x677CFC")]
	public void DelayDestroy(int id, float time)
	{
	}

	[Token(Token = "0x6000635")]
	[Address(RVA = "0x677EE4", Offset = "0x673EE4", VA = "0x677EE4")]
	public bool Getudata(object o, out int index)
	{
		return default(bool);
	}

	[Token(Token = "0x6000636")]
	[Address(RVA = "0x677F54", Offset = "0x673F54", VA = "0x677F54")]
	public void Destroyudata(int udata)
	{
	}

	[Token(Token = "0x6000637")]
	[Address(RVA = "0x677F6C", Offset = "0x673F6C", VA = "0x677F6C")]
	public void SetBack(int index, object o)
	{
	}

	[Token(Token = "0x6000638")]
	[Address(RVA = "0x677F84", Offset = "0x673F84", VA = "0x677F84")]
	private bool RemoveFromGCList(int id)
	{
		return default(bool);
	}

	[Token(Token = "0x6000639")]
	[Address(RVA = "0x6780A0", Offset = "0x6740A0", VA = "0x6780A0")]
	private void DestroyUnityObject(int udata, UnityEngine.Object obj)
	{
	}

	[Token(Token = "0x600063A")]
	[Address(RVA = "0x6781CC", Offset = "0x6741CC", VA = "0x6781CC")]
	public void Collect()
	{
	}

	[Token(Token = "0x600063B")]
	[Address(RVA = "0x678308", Offset = "0x674308", VA = "0x678308")]
	public void StepCollect()
	{
	}

	[Token(Token = "0x600063C")]
	[Address(RVA = "0x678328", Offset = "0x674328", VA = "0x678328")]
	public void Dispose()
	{
	}
}
