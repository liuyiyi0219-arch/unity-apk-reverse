using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppDummyDll;
using LuaInterface;
using UnityEngine;
using UnityEngine.SceneManagement;

[Token(Token = "0x2000027")]
public class LuaClient : MonoBehaviour
{
	[Token(Token = "0x4000020")]
	[Il2CppDummyDll.FieldOffset(Offset = "0x20")]
	protected LuaState luaState;

	[Token(Token = "0x4000021")]
	[Il2CppDummyDll.FieldOffset(Offset = "0x28")]
	protected LuaLooper loop;

	[Token(Token = "0x4000022")]
	[Il2CppDummyDll.FieldOffset(Offset = "0x30")]
	protected LuaFunction levelLoaded;

	[Token(Token = "0x4000023")]
	[Il2CppDummyDll.FieldOffset(Offset = "0x38")]
	protected bool openLuaSocket;

	[Token(Token = "0x4000024")]
	[Il2CppDummyDll.FieldOffset(Offset = "0x39")]
	protected bool beZbStart;

	[Token(Token = "0x4000025")]
	[Il2CppDummyDll.FieldOffset(Offset = "0x40")]
	private LuaTable profiler;

	[Token(Token = "0x17000003")]
	public static LuaClient Instance
	{
		[Token(Token = "0x600018A")]
		[Address(RVA = "0x643994", Offset = "0x63F994", VA = "0x643994")]
		[CompilerGenerated]
		get
		{
			return null;
		}
		[Token(Token = "0x600018B")]
		[Address(RVA = "0x6439DC", Offset = "0x63F9DC", VA = "0x6439DC")]
		[CompilerGenerated]
		protected set
		{
		}
	}

	[Token(Token = "0x600018C")]
	[Address(RVA = "0x643A34", Offset = "0x63FA34", VA = "0x643A34", Slot = "4")]
	protected virtual LuaFileUtils InitLoader()
	{
		return null;
	}

	[Token(Token = "0x600018D")]
	[Address(RVA = "0x643A3C", Offset = "0x63FA3C", VA = "0x643A3C", Slot = "5")]
	protected virtual void LoadLuaFiles()
	{
	}

	[Token(Token = "0x600018E")]
	[Address(RVA = "0x643A48", Offset = "0x63FA48", VA = "0x643A48", Slot = "6")]
	protected virtual void OpenLibs()
	{
	}

	[Token(Token = "0x600018F")]
	[Address(RVA = "0x643D3C", Offset = "0x63FD3C", VA = "0x643D3C")]
	public void OpenZbsDebugger([Optional] string ip)
	{
	}

	[Token(Token = "0x6000190")]
	[Address(RVA = "0x6438EC", Offset = "0x63F8EC", VA = "0x6438EC")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	private static int LuaOpen_Socket_Core(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x6000191")]
	[Address(RVA = "0x643940", Offset = "0x63F940", VA = "0x643940")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	private static int LuaOpen_Mime_Core(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x6000192")]
	[Address(RVA = "0x643BE8", Offset = "0x63FBE8", VA = "0x643BE8")]
	protected void OpenLuaSocket()
	{
	}

	[Token(Token = "0x6000193")]
	[Address(RVA = "0x643FCC", Offset = "0x63FFCC", VA = "0x643FCC")]
	protected void OpenCJson()
	{
	}

	[Token(Token = "0x6000194")]
	[Address(RVA = "0x644150", Offset = "0x640150", VA = "0x644150", Slot = "7")]
	protected virtual void CallMain()
	{
	}

	[Token(Token = "0x6000195")]
	[Address(RVA = "0x6441C8", Offset = "0x6401C8", VA = "0x6441C8", Slot = "8")]
	protected virtual void StartMain()
	{
	}

	[Token(Token = "0x6000196")]
	[Address(RVA = "0x644268", Offset = "0x640268", VA = "0x644268")]
	protected void StartLooper()
	{
	}

	[Token(Token = "0x6000197")]
	[Address(RVA = "0x6442E4", Offset = "0x6402E4", VA = "0x6442E4", Slot = "9")]
	protected virtual void Bind()
	{
	}

	[Token(Token = "0x6000198")]
	[Address(RVA = "0x644664", Offset = "0x640664", VA = "0x644664")]
	protected void Init()
	{
	}

	[Token(Token = "0x6000199")]
	[Address(RVA = "0x644720", Offset = "0x640720", VA = "0x644720")]
	protected void Awake()
	{
	}

	[Token(Token = "0x600019A")]
	[Address(RVA = "0x644810", Offset = "0x640810", VA = "0x644810", Slot = "10")]
	protected virtual void OnLoadFinished()
	{
	}

	[Token(Token = "0x600019B")]
	[Address(RVA = "0x644848", Offset = "0x640848", VA = "0x644848")]
	private void OnLevelLoaded(int level)
	{
	}

	[Token(Token = "0x600019C")]
	[Address(RVA = "0x64493C", Offset = "0x64093C", VA = "0x64493C")]
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
	}

	[Token(Token = "0x600019D")]
	[Address(RVA = "0x644970", Offset = "0x640970", VA = "0x644970", Slot = "11")]
	public virtual void Destroy()
	{
	}

	[Token(Token = "0x600019E")]
	[Address(RVA = "0x644D3C", Offset = "0x640D3C", VA = "0x644D3C")]
	protected void OnDestroy()
	{
	}

	[Token(Token = "0x600019F")]
	[Address(RVA = "0x644D48", Offset = "0x640D48", VA = "0x644D48")]
	protected void OnApplicationQuit()
	{
	}

	[Token(Token = "0x60001A0")]
	[Address(RVA = "0x644D54", Offset = "0x640D54", VA = "0x644D54")]
	public static LuaState GetMainState()
	{
		return null;
	}

	[Token(Token = "0x60001A1")]
	[Address(RVA = "0x644DA0", Offset = "0x640DA0", VA = "0x644DA0")]
	public LuaLooper GetLooper()
	{
		return null;
	}

	[Token(Token = "0x60001A2")]
	[Address(RVA = "0x644DA8", Offset = "0x640DA8", VA = "0x644DA8")]
	public void AttachProfiler()
	{
	}

	[Token(Token = "0x60001A3")]
	[Address(RVA = "0x644B90", Offset = "0x640B90", VA = "0x644B90")]
	public void DetachProfiler()
	{
	}

	[Token(Token = "0x60001A4")]
	[Address(RVA = "0x644F20", Offset = "0x640F20", VA = "0x644F20")]
	public LuaClient()
	{
	}
}
