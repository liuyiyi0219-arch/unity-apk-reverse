using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Il2CppDummyDll;
using UnityEngine;

[Token(Token = "0x2000002")]
public class GameBoot : MonoBehaviour
{
	[Token(Token = "0x2000004")]
	[CompilerGenerated]
	private sealed class _003CQuitAfterDelay_003Ed__2 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[Token(Token = "0x4000004")]
		[FieldOffset(Offset = "0x10")]
		private int _003C_003E1__state;

		[Token(Token = "0x4000005")]
		[FieldOffset(Offset = "0x18")]
		private object _003C_003E2__current;

		[Token(Token = "0x4000006")]
		[FieldOffset(Offset = "0x20")]
		public float seconds;

		[Token(Token = "0x17000001")]
		private object System_002ECollections_002EGeneric_002EIEnumerator_003CSystem_002EObject_003E_002ECurrent
		{
			[Token(Token = "0x600000B")]
			[Address(RVA = "0x6016B0", Offset = "0x5FD6B0", VA = "0x6016B0", Slot = "4")]
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		[Token(Token = "0x17000002")]
		private object System_002ECollections_002EIEnumerator_002ECurrent
		{
			[Token(Token = "0x600000D")]
			[Address(RVA = "0x6016F0", Offset = "0x5FD6F0", VA = "0x6016F0", Slot = "7")]
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		[Token(Token = "0x6000008")]
		[Address(RVA = "0x600F20", Offset = "0x5FCF20", VA = "0x600F20")]
		[DebuggerHidden]
		public _003CQuitAfterDelay_003Ed__2(int _003C_003E1__state)
		{
		}

		[Token(Token = "0x6000009")]
		[Address(RVA = "0x6012B4", Offset = "0x5FD2B4", VA = "0x6012B4", Slot = "5")]
		[DebuggerHidden]
		private void System_002EIDisposable_002EDispose()
		{
		}

		[Token(Token = "0x600000A")]
		[Address(RVA = "0x6012B8", Offset = "0x5FD2B8", VA = "0x6012B8", Slot = "6")]
		private bool MoveNext()
		{
			return default(bool);
		}

		[Token(Token = "0x600000C")]
		[Address(RVA = "0x6016B8", Offset = "0x5FD6B8", VA = "0x6016B8", Slot = "8")]
		[DebuggerHidden]
		private void System_002ECollections_002EIEnumerator_002EReset()
		{
		}
	}

	[Token(Token = "0x4000001")]
	[FieldOffset(Offset = "0x20")]
	public GameConfig config;

	[Token(Token = "0x6000001")]
	[Address(RVA = "0x5FFA24", Offset = "0x5FBA24", VA = "0x5FFA24")]
	private void Start()
	{
	}

	[Token(Token = "0x6000002")]
	[Address(RVA = "0x600D50", Offset = "0x5FCD50", VA = "0x600D50")]
	[IteratorStateMachine(typeof(_003CQuitAfterDelay_003Ed__2))]
	private IEnumerator QuitAfterDelay(float seconds)
	{
		return null;
	}

	[Token(Token = "0x6000003")]
	[Address(RVA = "0x600ADC", Offset = "0x5FCADC", VA = "0x600ADC")]
	private static void ShowToast(string text)
	{
	}

	[Token(Token = "0x6000004")]
	[Address(RVA = "0x5FFDF0", Offset = "0x5FBDF0", VA = "0x5FFDF0")]
	private static void AndroidLog(string msg)
	{
	}

	[Token(Token = "0x6000005")]
	[Address(RVA = "0x600F50", Offset = "0x5FCF50", VA = "0x600F50")]
	public GameBoot()
	{
	}
}
