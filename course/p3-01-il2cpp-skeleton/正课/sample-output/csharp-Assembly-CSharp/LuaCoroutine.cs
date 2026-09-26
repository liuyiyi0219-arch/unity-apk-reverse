using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Il2CppDummyDll;
using LuaInterface;
using UnityEngine;

[Token(Token = "0x2000028")]
public static class LuaCoroutine
{
	[Token(Token = "0x2000029")]
	[CompilerGenerated]
	private sealed class _003CCoWaitForEndOfFrame_003Ed__8 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[Token(Token = "0x4000028")]
		[FieldOffset(Offset = "0x10")]
		private int _003C_003E1__state;

		[Token(Token = "0x4000029")]
		[FieldOffset(Offset = "0x18")]
		private object _003C_003E2__current;

		[Token(Token = "0x400002A")]
		[FieldOffset(Offset = "0x20")]
		public LuaFunction func;

		[Token(Token = "0x17000004")]
		private object System_002ECollections_002EGeneric_002EIEnumerator_003CSystem_002EObject_003E_002ECurrent
		{
			[Token(Token = "0x60001B5")]
			[Address(RVA = "0x645C7C", Offset = "0x641C7C", VA = "0x645C7C", Slot = "4")]
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		[Token(Token = "0x17000005")]
		private object System_002ECollections_002EIEnumerator_002ECurrent
		{
			[Token(Token = "0x60001B7")]
			[Address(RVA = "0x645CBC", Offset = "0x641CBC", VA = "0x645CBC", Slot = "7")]
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		[Token(Token = "0x60001B2")]
		[Address(RVA = "0x6459E8", Offset = "0x6419E8", VA = "0x6459E8")]
		[DebuggerHidden]
		public _003CCoWaitForEndOfFrame_003Ed__8(int _003C_003E1__state)
		{
		}

		[Token(Token = "0x60001B3")]
		[Address(RVA = "0x645BD0", Offset = "0x641BD0", VA = "0x645BD0", Slot = "5")]
		[DebuggerHidden]
		private void System_002EIDisposable_002EDispose()
		{
		}

		[Token(Token = "0x60001B4")]
		[Address(RVA = "0x645BD4", Offset = "0x641BD4", VA = "0x645BD4", Slot = "6")]
		private bool MoveNext()
		{
			return default(bool);
		}

		[Token(Token = "0x60001B6")]
		[Address(RVA = "0x645C84", Offset = "0x641C84", VA = "0x645C84", Slot = "8")]
		[DebuggerHidden]
		private void System_002ECollections_002EIEnumerator_002EReset()
		{
		}
	}

	[Token(Token = "0x200002A")]
	[CompilerGenerated]
	private sealed class _003CCoWaitForFixedUpdate_003Ed__6 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[Token(Token = "0x400002B")]
		[FieldOffset(Offset = "0x10")]
		private int _003C_003E1__state;

		[Token(Token = "0x400002C")]
		[FieldOffset(Offset = "0x18")]
		private object _003C_003E2__current;

		[Token(Token = "0x400002D")]
		[FieldOffset(Offset = "0x20")]
		public LuaFunction func;

		[Token(Token = "0x17000006")]
		private object System_002ECollections_002EGeneric_002EIEnumerator_003CSystem_002EObject_003E_002ECurrent
		{
			[Token(Token = "0x60001BB")]
			[Address(RVA = "0x645D70", Offset = "0x641D70", VA = "0x645D70", Slot = "4")]
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		[Token(Token = "0x17000007")]
		private object System_002ECollections_002EIEnumerator_002ECurrent
		{
			[Token(Token = "0x60001BD")]
			[Address(RVA = "0x645DB0", Offset = "0x641DB0", VA = "0x645DB0", Slot = "7")]
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		[Token(Token = "0x60001B8")]
		[Address(RVA = "0x645954", Offset = "0x641954", VA = "0x645954")]
		[DebuggerHidden]
		public _003CCoWaitForFixedUpdate_003Ed__6(int _003C_003E1__state)
		{
		}

		[Token(Token = "0x60001B9")]
		[Address(RVA = "0x645CC4", Offset = "0x641CC4", VA = "0x645CC4", Slot = "5")]
		[DebuggerHidden]
		private void System_002EIDisposable_002EDispose()
		{
		}

		[Token(Token = "0x60001BA")]
		[Address(RVA = "0x645CC8", Offset = "0x641CC8", VA = "0x645CC8", Slot = "6")]
		private bool MoveNext()
		{
			return default(bool);
		}

		[Token(Token = "0x60001BC")]
		[Address(RVA = "0x645D78", Offset = "0x641D78", VA = "0x645D78", Slot = "8")]
		[DebuggerHidden]
		private void System_002ECollections_002EIEnumerator_002EReset()
		{
		}
	}

	[Token(Token = "0x200002B")]
	[CompilerGenerated]
	private sealed class _003CCoWaitForSeconds_003Ed__4 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[Token(Token = "0x400002E")]
		[FieldOffset(Offset = "0x10")]
		private int _003C_003E1__state;

		[Token(Token = "0x400002F")]
		[FieldOffset(Offset = "0x18")]
		private object _003C_003E2__current;

		[Token(Token = "0x4000030")]
		[FieldOffset(Offset = "0x20")]
		public float sec;

		[Token(Token = "0x4000031")]
		[FieldOffset(Offset = "0x28")]
		public LuaFunction func;

		[Token(Token = "0x17000008")]
		private object System_002ECollections_002EGeneric_002EIEnumerator_003CSystem_002EObject_003E_002ECurrent
		{
			[Token(Token = "0x60001C1")]
			[Address(RVA = "0x645E74", Offset = "0x641E74", VA = "0x645E74", Slot = "4")]
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		[Token(Token = "0x17000009")]
		private object System_002ECollections_002EIEnumerator_002ECurrent
		{
			[Token(Token = "0x60001C3")]
			[Address(RVA = "0x645EB4", Offset = "0x641EB4", VA = "0x645EB4", Slot = "7")]
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		[Token(Token = "0x60001BE")]
		[Address(RVA = "0x6458C0", Offset = "0x6418C0", VA = "0x6458C0")]
		[DebuggerHidden]
		public _003CCoWaitForSeconds_003Ed__4(int _003C_003E1__state)
		{
		}

		[Token(Token = "0x60001BF")]
		[Address(RVA = "0x645DB8", Offset = "0x641DB8", VA = "0x645DB8", Slot = "5")]
		[DebuggerHidden]
		private void System_002EIDisposable_002EDispose()
		{
		}

		[Token(Token = "0x60001C0")]
		[Address(RVA = "0x645DBC", Offset = "0x641DBC", VA = "0x645DBC", Slot = "6")]
		private bool MoveNext()
		{
			return default(bool);
		}

		[Token(Token = "0x60001C2")]
		[Address(RVA = "0x645E7C", Offset = "0x641E7C", VA = "0x645E7C", Slot = "8")]
		[DebuggerHidden]
		private void System_002ECollections_002EIEnumerator_002EReset()
		{
		}
	}

	[Token(Token = "0x200002C")]
	[CompilerGenerated]
	private sealed class _003CCoWrap_003Ed__13 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[Token(Token = "0x4000032")]
		[FieldOffset(Offset = "0x10")]
		private int _003C_003E1__state;

		[Token(Token = "0x4000033")]
		[FieldOffset(Offset = "0x18")]
		private object _003C_003E2__current;

		[Token(Token = "0x4000034")]
		[FieldOffset(Offset = "0x20")]
		public LuaFunction func;

		[Token(Token = "0x1700000A")]
		private object System_002ECollections_002EGeneric_002EIEnumerator_003CSystem_002EObject_003E_002ECurrent
		{
			[Token(Token = "0x60001C7")]
			[Address(RVA = "0x645F70", Offset = "0x641F70", VA = "0x645F70", Slot = "4")]
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		[Token(Token = "0x1700000B")]
		private object System_002ECollections_002EIEnumerator_002ECurrent
		{
			[Token(Token = "0x60001C9")]
			[Address(RVA = "0x645FB0", Offset = "0x641FB0", VA = "0x645FB0", Slot = "7")]
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		[Token(Token = "0x60001C4")]
		[Address(RVA = "0x645B2C", Offset = "0x641B2C", VA = "0x645B2C")]
		[DebuggerHidden]
		public _003CCoWrap_003Ed__13(int _003C_003E1__state)
		{
		}

		[Token(Token = "0x60001C5")]
		[Address(RVA = "0x645EBC", Offset = "0x641EBC", VA = "0x645EBC", Slot = "5")]
		[DebuggerHidden]
		private void System_002EIDisposable_002EDispose()
		{
		}

		[Token(Token = "0x60001C6")]
		[Address(RVA = "0x645EC0", Offset = "0x641EC0", VA = "0x645EC0", Slot = "6")]
		private bool MoveNext()
		{
			return default(bool);
		}

		[Token(Token = "0x60001C8")]
		[Address(RVA = "0x645F78", Offset = "0x641F78", VA = "0x645F78", Slot = "8")]
		[DebuggerHidden]
		private void System_002ECollections_002EIEnumerator_002EReset()
		{
		}
	}

	[Token(Token = "0x200002D")]
	[CompilerGenerated]
	private sealed class _003CCoYield_003Ed__10 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[Token(Token = "0x4000035")]
		[FieldOffset(Offset = "0x10")]
		private int _003C_003E1__state;

		[Token(Token = "0x4000036")]
		[FieldOffset(Offset = "0x18")]
		private object _003C_003E2__current;

		[Token(Token = "0x4000037")]
		[FieldOffset(Offset = "0x20")]
		public object o;

		[Token(Token = "0x4000038")]
		[FieldOffset(Offset = "0x28")]
		public LuaFunction func;

		[Token(Token = "0x1700000C")]
		private object System_002ECollections_002EGeneric_002EIEnumerator_003CSystem_002EObject_003E_002ECurrent
		{
			[Token(Token = "0x60001CD")]
			[Address(RVA = "0x646100", Offset = "0x642100", VA = "0x646100", Slot = "4")]
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		[Token(Token = "0x1700000D")]
		private object System_002ECollections_002EIEnumerator_002ECurrent
		{
			[Token(Token = "0x60001CF")]
			[Address(RVA = "0x646140", Offset = "0x642140", VA = "0x646140", Slot = "7")]
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		[Token(Token = "0x60001CA")]
		[Address(RVA = "0x645A98", Offset = "0x641A98", VA = "0x645A98")]
		[DebuggerHidden]
		public _003CCoYield_003Ed__10(int _003C_003E1__state)
		{
		}

		[Token(Token = "0x60001CB")]
		[Address(RVA = "0x645FB8", Offset = "0x641FB8", VA = "0x645FB8", Slot = "5")]
		[DebuggerHidden]
		private void System_002EIDisposable_002EDispose()
		{
		}

		[Token(Token = "0x60001CC")]
		[Address(RVA = "0x645FBC", Offset = "0x641FBC", VA = "0x645FBC", Slot = "6")]
		private bool MoveNext()
		{
			return default(bool);
		}

		[Token(Token = "0x60001CE")]
		[Address(RVA = "0x646108", Offset = "0x642108", VA = "0x646108", Slot = "8")]
		[DebuggerHidden]
		private void System_002ECollections_002EIEnumerator_002EReset()
		{
		}
	}

	[Token(Token = "0x4000026")]
	[FieldOffset(Offset = "0x0")]
	private static MonoBehaviour mb;

	[Token(Token = "0x4000027")]
	[FieldOffset(Offset = "0x8")]
	private static string strCo;

	[Token(Token = "0x60001A5")]
	[Address(RVA = "0x644378", Offset = "0x640378", VA = "0x644378")]
	public static void Register(LuaState state, MonoBehaviour behaviour)
	{
	}

	[Token(Token = "0x60001A6")]
	[Address(RVA = "0x644F28", Offset = "0x640F28", VA = "0x644F28")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	private static int _WaitForSeconds(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x60001A7")]
	[Address(RVA = "0x645844", Offset = "0x641844", VA = "0x645844")]
	[IteratorStateMachine(typeof(_003CCoWaitForSeconds_003Ed__4))]
	private static IEnumerator CoWaitForSeconds(float sec, LuaFunction func)
	{
		return null;
	}

	[Token(Token = "0x60001A8")]
	[Address(RVA = "0x6450FC", Offset = "0x6410FC", VA = "0x6450FC")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	private static int WaitForFixedUpdate(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x60001A9")]
	[Address(RVA = "0x6458E8", Offset = "0x6418E8", VA = "0x6458E8")]
	[IteratorStateMachine(typeof(_003CCoWaitForFixedUpdate_003Ed__6))]
	private static IEnumerator CoWaitForFixedUpdate(LuaFunction func)
	{
		return null;
	}

	[Token(Token = "0x60001AA")]
	[Address(RVA = "0x645284", Offset = "0x641284", VA = "0x645284")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	private static int WaitForEndOfFrame(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x60001AB")]
	[Address(RVA = "0x64597C", Offset = "0x64197C", VA = "0x64597C")]
	[IteratorStateMachine(typeof(_003CCoWaitForEndOfFrame_003Ed__8))]
	private static IEnumerator CoWaitForEndOfFrame(LuaFunction func)
	{
		return null;
	}

	[Token(Token = "0x60001AC")]
	[Address(RVA = "0x64540C", Offset = "0x64140C", VA = "0x64540C")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	private static int Yield(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x60001AD")]
	[Address(RVA = "0x645A10", Offset = "0x641A10", VA = "0x645A10")]
	[IteratorStateMachine(typeof(_003CCoYield_003Ed__10))]
	private static IEnumerator CoYield(object o, LuaFunction func)
	{
		return null;
	}

	[Token(Token = "0x60001AE")]
	[Address(RVA = "0x6455BC", Offset = "0x6415BC", VA = "0x6455BC")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	private static int StopCoroutine(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x60001AF")]
	[Address(RVA = "0x64579C", Offset = "0x64179C", VA = "0x64579C")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	private static int WrapLuaCoroutine(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x60001B0")]
	[Address(RVA = "0x645AC0", Offset = "0x641AC0", VA = "0x645AC0")]
	[IteratorStateMachine(typeof(_003CCoWrap_003Ed__13))]
	private static IEnumerator CoWrap(LuaFunction func)
	{
		return null;
	}
}
