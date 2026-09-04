using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Il2CppDummyDll;
using UnityEngine;

namespace LuaInterface;

[Token(Token = "0x200004A")]
public class LuaFunction : LuaBaseRef
{
	[Token(Token = "0x200004B")]
	protected struct FuncData
	{
		[Token(Token = "0x40000A0")]
		[Il2CppDummyDll.FieldOffset(Offset = "0x0")]
		public int oldTop;

		[Token(Token = "0x40000A1")]
		[Il2CppDummyDll.FieldOffset(Offset = "0x4")]
		public int stackPos;

		[Token(Token = "0x6000367")]
		[Address(RVA = "0x651E54", Offset = "0x64DE54", VA = "0x651E54")]
		public FuncData(int top, int stack)
		{
		}
	}

	[Token(Token = "0x400009C")]
	[Il2CppDummyDll.FieldOffset(Offset = "0x3C")]
	protected int oldTop;

	[Token(Token = "0x400009D")]
	[Il2CppDummyDll.FieldOffset(Offset = "0x40")]
	private int argCount;

	[Token(Token = "0x400009E")]
	[Il2CppDummyDll.FieldOffset(Offset = "0x44")]
	private int stackPos;

	[Token(Token = "0x400009F")]
	[Il2CppDummyDll.FieldOffset(Offset = "0x48")]
	private Stack<FuncData> stack;

	[Token(Token = "0x6000315")]
	[Address(RVA = "0x651C1C", Offset = "0x64DC1C", VA = "0x651C1C")]
	public LuaFunction(int reference, LuaState state)
	{
	}

	[Token(Token = "0x6000316")]
	[Address(RVA = "0x651CD4", Offset = "0x64DCD4", VA = "0x651CD4", Slot = "5")]
	public override void Dispose()
	{
	}

	[Token(Token = "0x6000317")]
	public T ToDelegate<T>() where T : class
	{
		return null;
	}

	[Token(Token = "0x6000318")]
	[Address(RVA = "0x651CDC", Offset = "0x64DCDC", VA = "0x651CDC", Slot = "8")]
	public virtual int BeginPCall()
	{
		return default(int);
	}

	[Token(Token = "0x6000319")]
	[Address(RVA = "0x64FDAC", Offset = "0x64BDAC", VA = "0x64FDAC")]
	public void PCall()
	{
	}

	[Token(Token = "0x600031A")]
	[Address(RVA = "0x64FE74", Offset = "0x64BE74", VA = "0x64FE74")]
	public void EndPCall()
	{
	}

	[Token(Token = "0x600031B")]
	[Address(RVA = "0x652020", Offset = "0x64E020", VA = "0x652020")]
	public void Call()
	{
	}

	[Token(Token = "0x600031C")]
	public void Call<T1>(T1 arg1)
	{
	}

	[Token(Token = "0x600031D")]
	public void Call<T1, T2>(T1 arg1, T2 arg2)
	{
	}

	[Token(Token = "0x600031E")]
	public void Call<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3)
	{
	}

	[Token(Token = "0x600031F")]
	public void Call<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
	{
	}

	[Token(Token = "0x6000320")]
	public void Call<T1, T2, T3, T4, T5>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
	{
	}

	[Token(Token = "0x6000321")]
	public void Call<T1, T2, T3, T4, T5, T6>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
	{
	}

	[Token(Token = "0x6000322")]
	public void Call<T1, T2, T3, T4, T5, T6, T7>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
	{
	}

	[Token(Token = "0x6000323")]
	public void Call<T1, T2, T3, T4, T5, T6, T7, T8>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
	{
	}

	[Token(Token = "0x6000324")]
	public void Call<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
	{
	}

	[Token(Token = "0x6000325")]
	public R1 Invoke<R1>()
	{
		return (R1)null;
	}

	[Token(Token = "0x6000326")]
	public R1 Invoke<T1, R1>(T1 arg1)
	{
		return (R1)null;
	}

	[Token(Token = "0x6000327")]
	public R1 Invoke<T1, T2, R1>(T1 arg1, T2 arg2)
	{
		return (R1)null;
	}

	[Token(Token = "0x6000328")]
	public R1 Invoke<T1, T2, T3, R1>(T1 arg1, T2 arg2, T3 arg3)
	{
		return (R1)null;
	}

	[Token(Token = "0x6000329")]
	public R1 Invoke<T1, T2, T3, T4, R1>(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
	{
		return (R1)null;
	}

	[Token(Token = "0x600032A")]
	public R1 Invoke<T1, T2, T3, T4, T5, R1>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
	{
		return (R1)null;
	}

	[Token(Token = "0x600032B")]
	public R1 Invoke<T1, T2, T3, T4, T5, T6, R1>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
	{
		return (R1)null;
	}

	[Token(Token = "0x600032C")]
	public R1 Invoke<T1, T2, T3, T4, T5, T6, T7, R1>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
	{
		return (R1)null;
	}

	[Token(Token = "0x600032D")]
	public R1 Invoke<T1, T2, T3, T4, T5, T6, T7, T8, R1>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
	{
		return (R1)null;
	}

	[Token(Token = "0x600032E")]
	public R1 Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, R1>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
	{
		return (R1)null;
	}

	[Token(Token = "0x600032F")]
	[Address(RVA = "0x652048", Offset = "0x64E048", VA = "0x652048")]
	[Obsolete("LuaFunction.LazyCall() is obsolete.Use LuaFunction.Invoke()")]
	public object[] LazyCall(params object[] args)
	{
		return null;
	}

	[Token(Token = "0x6000330")]
	[Address(RVA = "0x652328", Offset = "0x64E328", VA = "0x652328")]
	public void CheckStack(int args)
	{
	}

	[Token(Token = "0x6000331")]
	[Address(RVA = "0x652344", Offset = "0x64E344", VA = "0x652344")]
	public bool IsBegin()
	{
		return default(bool);
	}

	[Token(Token = "0x6000332")]
	[Address(RVA = "0x652354", Offset = "0x64E354", VA = "0x652354")]
	public void Push(double num)
	{
	}

	[Token(Token = "0x6000333")]
	[Address(RVA = "0x6523EC", Offset = "0x64E3EC", VA = "0x6523EC")]
	public void Push(int n)
	{
	}

	[Token(Token = "0x6000334")]
	[Address(RVA = "0x652484", Offset = "0x64E484", VA = "0x652484")]
	public void PushLayerMask(LayerMask n)
	{
	}

	[Token(Token = "0x6000335")]
	[Address(RVA = "0x652538", Offset = "0x64E538", VA = "0x652538")]
	public void Push(uint un)
	{
	}

	[Token(Token = "0x6000336")]
	[Address(RVA = "0x6525D0", Offset = "0x64E5D0", VA = "0x6525D0")]
	public void Push(long num)
	{
	}

	[Token(Token = "0x6000337")]
	[Address(RVA = "0x652668", Offset = "0x64E668", VA = "0x652668")]
	public void Push(ulong un)
	{
	}

	[Token(Token = "0x6000338")]
	[Address(RVA = "0x652700", Offset = "0x64E700", VA = "0x652700")]
	public void Push(bool b)
	{
	}

	[Token(Token = "0x6000339")]
	[Address(RVA = "0x65279C", Offset = "0x64E79C", VA = "0x65279C")]
	public void Push(string str)
	{
	}

	[Token(Token = "0x600033A")]
	[Address(RVA = "0x652834", Offset = "0x64E834", VA = "0x652834")]
	public void Push(IntPtr ptr)
	{
	}

	[Token(Token = "0x600033B")]
	[Address(RVA = "0x64FD80", Offset = "0x64BD80", VA = "0x64FD80")]
	public void Push(LuaBaseRef lbr)
	{
	}

	[Token(Token = "0x600033C")]
	[Address(RVA = "0x65292C", Offset = "0x64E92C", VA = "0x65292C")]
	public void Push(object o)
	{
	}

	[Token(Token = "0x600033D")]
	[Address(RVA = "0x6529C4", Offset = "0x64E9C4", VA = "0x6529C4")]
	public void Push(UnityEngine.Object o)
	{
	}

	[Token(Token = "0x600033E")]
	[Address(RVA = "0x652A5C", Offset = "0x64EA5C", VA = "0x652A5C")]
	public void Push(Type t)
	{
	}

	[Token(Token = "0x600033F")]
	[Address(RVA = "0x652B18", Offset = "0x64EB18", VA = "0x652B18")]
	public void Push(Enum e)
	{
	}

	[Token(Token = "0x6000340")]
	[Address(RVA = "0x652B78", Offset = "0x64EB78", VA = "0x652B78")]
	public void Push(Array array)
	{
	}

	[Token(Token = "0x6000341")]
	[Address(RVA = "0x652BB4", Offset = "0x64EBB4", VA = "0x652BB4")]
	public void Push(Vector3 v3)
	{
	}

	[Token(Token = "0x6000342")]
	[Address(RVA = "0x652C64", Offset = "0x64EC64", VA = "0x652C64")]
	public void Push(Vector2 v2)
	{
	}

	[Token(Token = "0x6000343")]
	[Address(RVA = "0x652D04", Offset = "0x64ED04", VA = "0x652D04")]
	public void Push(Vector4 v4)
	{
	}

	[Token(Token = "0x6000344")]
	[Address(RVA = "0x652DBC", Offset = "0x64EDBC", VA = "0x652DBC")]
	public void Push(Quaternion quat)
	{
	}

	[Token(Token = "0x6000345")]
	[Address(RVA = "0x652E74", Offset = "0x64EE74", VA = "0x652E74")]
	public void Push(Color clr)
	{
	}

	[Token(Token = "0x6000346")]
	[Address(RVA = "0x652F2C", Offset = "0x64EF2C", VA = "0x652F2C")]
	public void Push(Ray ray)
	{
	}

	[Token(Token = "0x6000347")]
	[Address(RVA = "0x6530B4", Offset = "0x64F0B4", VA = "0x6530B4")]
	public void Push(Bounds bounds)
	{
	}

	[Token(Token = "0x6000348")]
	[Address(RVA = "0x65323C", Offset = "0x64F23C", VA = "0x65323C")]
	public void Push(RaycastHit hit)
	{
	}

	[Token(Token = "0x6000349")]
	[Address(RVA = "0x6533C4", Offset = "0x64F3C4", VA = "0x6533C4")]
	public void Push(Touch t)
	{
	}

	[Token(Token = "0x600034A")]
	[Address(RVA = "0x65354C", Offset = "0x64F54C", VA = "0x65354C")]
	public void Push(LuaByteBuffer buffer)
	{
	}

	[Token(Token = "0x600034B")]
	public void PushValue<T>(T value) where T : struct
	{
	}

	[Token(Token = "0x600034C")]
	[Address(RVA = "0x653684", Offset = "0x64F684", VA = "0x653684")]
	public void PushObject(object o)
	{
	}

	[Token(Token = "0x600034D")]
	public void PushSealed<T>(T o)
	{
	}

	[Token(Token = "0x600034E")]
	public void PushGeneric<T>(T t)
	{
	}

	[Token(Token = "0x600034F")]
	[Address(RVA = "0x65219C", Offset = "0x64E19C", VA = "0x65219C")]
	public void PushArgs(object[] args)
	{
	}

	[Token(Token = "0x6000350")]
	[Address(RVA = "0x653820", Offset = "0x64F820", VA = "0x653820")]
	public void PushByteBuffer(byte[] buffer, [Optional] int len)
	{
	}

	[Token(Token = "0x6000351")]
	[Address(RVA = "0x653970", Offset = "0x64F970", VA = "0x653970")]
	public double CheckNumber()
	{
		return default(double);
	}

	[Token(Token = "0x6000352")]
	[Address(RVA = "0x653AA4", Offset = "0x64FAA4", VA = "0x653AA4")]
	public bool CheckBoolean()
	{
		return default(bool);
	}

	[Token(Token = "0x6000353")]
	[Address(RVA = "0x653BDC", Offset = "0x64FBDC", VA = "0x653BDC")]
	public string CheckString()
	{
		return null;
	}

	[Token(Token = "0x6000354")]
	[Address(RVA = "0x653D10", Offset = "0x64FD10", VA = "0x653D10")]
	public Vector3 CheckVector3()
	{
		return default(Vector3);
	}

	[Token(Token = "0x6000355")]
	[Address(RVA = "0x653F54", Offset = "0x64FF54", VA = "0x653F54")]
	public Quaternion CheckQuaternion()
	{
		return default(Quaternion);
	}

	[Token(Token = "0x6000356")]
	[Address(RVA = "0x6541A8", Offset = "0x6501A8", VA = "0x6541A8")]
	public Vector2 CheckVector2()
	{
		return default(Vector2);
	}

	[Token(Token = "0x6000357")]
	[Address(RVA = "0x6543D0", Offset = "0x6503D0", VA = "0x6543D0")]
	public Vector4 CheckVector4()
	{
		return default(Vector4);
	}

	[Token(Token = "0x6000358")]
	[Address(RVA = "0x654624", Offset = "0x650624", VA = "0x654624")]
	public Color CheckColor()
	{
		return default(Color);
	}

	[Token(Token = "0x6000359")]
	[Address(RVA = "0x654834", Offset = "0x650834", VA = "0x654834")]
	public Ray CheckRay()
	{
		return default(Ray);
	}

	[Token(Token = "0x600035A")]
	[Address(RVA = "0x654C08", Offset = "0x650C08", VA = "0x654C08")]
	public Bounds CheckBounds()
	{
		return default(Bounds);
	}

	[Token(Token = "0x600035B")]
	[Address(RVA = "0x654F40", Offset = "0x650F40", VA = "0x654F40")]
	public LayerMask CheckLayerMask()
	{
		return default(LayerMask);
	}

	[Token(Token = "0x600035C")]
	[Address(RVA = "0x65512C", Offset = "0x65112C", VA = "0x65512C")]
	public long CheckLong()
	{
		return default(long);
	}

	[Token(Token = "0x600035D")]
	[Address(RVA = "0x655274", Offset = "0x651274", VA = "0x655274")]
	public ulong CheckULong()
	{
		return default(ulong);
	}

	[Token(Token = "0x600035E")]
	[Address(RVA = "0x6553BC", Offset = "0x6513BC", VA = "0x6553BC")]
	public Delegate CheckDelegate()
	{
		return null;
	}

	[Token(Token = "0x600035F")]
	[Address(RVA = "0x6555E8", Offset = "0x6515E8", VA = "0x6555E8")]
	public object CheckVariant()
	{
		return null;
	}

	[Token(Token = "0x6000360")]
	[Address(RVA = "0x65567C", Offset = "0x65167C", VA = "0x65567C")]
	public char[] CheckCharBuffer()
	{
		return null;
	}

	[Token(Token = "0x6000361")]
	[Address(RVA = "0x6557B0", Offset = "0x6517B0", VA = "0x6557B0")]
	public byte[] CheckByteBuffer()
	{
		return null;
	}

	[Token(Token = "0x6000362")]
	[Address(RVA = "0x6558E4", Offset = "0x6518E4", VA = "0x6558E4")]
	public object CheckObject(Type t)
	{
		return null;
	}

	[Token(Token = "0x6000363")]
	[Address(RVA = "0x655A24", Offset = "0x651A24", VA = "0x655A24")]
	public LuaFunction CheckLuaFunction()
	{
		return null;
	}

	[Token(Token = "0x6000364")]
	[Address(RVA = "0x655B58", Offset = "0x651B58", VA = "0x655B58")]
	public LuaTable CheckLuaTable()
	{
		return null;
	}

	[Token(Token = "0x6000365")]
	[Address(RVA = "0x655C8C", Offset = "0x651C8C", VA = "0x655C8C")]
	public LuaThread CheckLuaThread()
	{
		return null;
	}

	[Token(Token = "0x6000366")]
	public T CheckValue<T>()
	{
		return (T)null;
	}
}
