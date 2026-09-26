using System;
using System.Runtime.InteropServices;
using Il2CppDummyDll;

namespace LuaInterface;

[Token(Token = "0x2000067")]
public sealed class LuaUnityLibs
{
	[Token(Token = "0x600060A")]
	[Address(RVA = "0x67621C", Offset = "0x67221C", VA = "0x67621C")]
	public static void OpenLibs(IntPtr L)
	{
	}

	[Token(Token = "0x600060B")]
	[Address(RVA = "0x67644C", Offset = "0x67244C", VA = "0x67644C")]
	public static void OpenLuaLibs(IntPtr L)
	{
	}

	[Token(Token = "0x600060C")]
	[Address(RVA = "0x676234", Offset = "0x672234", VA = "0x676234")]
	private static void InitMathf(IntPtr L)
	{
	}

	[Token(Token = "0x600060D")]
	[Address(RVA = "0x676344", Offset = "0x672344", VA = "0x676344")]
	private static void InitLayer(IntPtr L)
	{
	}

	[Token(Token = "0x600060E")]
	[Address(RVA = "0x675970", Offset = "0x671970", VA = "0x675970")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	private static int PerlinNoise(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x600060F")]
	[Address(RVA = "0x676898", Offset = "0x672898", VA = "0x676898")]
	private static void SetOutMethods(IntPtr L, string table, [Optional] LuaCSFunction getOutFunc)
	{
	}

	[Token(Token = "0x6000610")]
	[Address(RVA = "0x675AC4", Offset = "0x671AC4", VA = "0x675AC4")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	private static int GetOutVector3(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x6000611")]
	[Address(RVA = "0x675B80", Offset = "0x671B80", VA = "0x675B80")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	private static int GetOutVector2(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x6000612")]
	[Address(RVA = "0x675C3C", Offset = "0x671C3C", VA = "0x675C3C")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	private static int GetOutVector4(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x6000613")]
	[Address(RVA = "0x675CF8", Offset = "0x671CF8", VA = "0x675CF8")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	private static int GetOutColor(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x6000614")]
	[Address(RVA = "0x675DB4", Offset = "0x671DB4", VA = "0x675DB4")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	private static int GetOutQuaternion(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x6000615")]
	[Address(RVA = "0x675E70", Offset = "0x671E70", VA = "0x675E70")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	private static int GetOutRay(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x6000616")]
	[Address(RVA = "0x675F2C", Offset = "0x671F2C", VA = "0x675F2C")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	private static int GetOutBounds(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x6000617")]
	[Address(RVA = "0x675FE8", Offset = "0x671FE8", VA = "0x675FE8")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	private static int GetOutRaycastHit(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x6000618")]
	[Address(RVA = "0x6760A4", Offset = "0x6720A4", VA = "0x6760A4")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	private static int GetOutTouch(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x6000619")]
	[Address(RVA = "0x676160", Offset = "0x672160", VA = "0x676160")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	private static int GetOutLayerMask(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x600061A")]
	[Address(RVA = "0x67698C", Offset = "0x67298C", VA = "0x67698C")]
	public LuaUnityLibs()
	{
	}
}
