using System;
using System.Collections;
using System.Collections.Generic;
using Il2CppDummyDll;
using UnityEngine;

namespace LuaInterface;

[Token(Token = "0x2000070")]
public static class ToLua
{
	[Token(Token = "0x2000071")]
	public delegate object LuaTableToVar(IntPtr L, int pos);

	[Token(Token = "0x2000072")]
	public delegate void LuaPushVarObject(IntPtr L, object o);

	[Token(Token = "0x4000114")]
	[FieldOffset(Offset = "0x0")]
	private static Type monoType;

	[Token(Token = "0x4000115")]
	[FieldOffset(Offset = "0x8")]
	public static LuaTableToVar[] ToVarMap;

	[Token(Token = "0x4000116")]
	[FieldOffset(Offset = "0x10")]
	public static Dictionary<Type, LuaPushVarObject> VarPushMap;

	[Token(Token = "0x6000643")]
	[Address(RVA = "0x679BE4", Offset = "0x675BE4", VA = "0x679BE4")]
	static ToLua()
	{
	}

	[Token(Token = "0x6000644")]
	[Address(RVA = "0x67A0A0", Offset = "0x6760A0", VA = "0x67A0A0")]
	public static void OpenLibs(IntPtr L)
	{
	}

	[Token(Token = "0x6000645")]
	[Address(RVA = "0x67A5B0", Offset = "0x6765B0", VA = "0x67A5B0")]
	private static void AddLuaLoader(IntPtr L)
	{
	}

	[Token(Token = "0x6000646")]
	[Address(RVA = "0x6783F0", Offset = "0x6743F0", VA = "0x6783F0")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	private static int Panic(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x6000647")]
	[Address(RVA = "0x67847C", Offset = "0x67447C", VA = "0x67847C")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	private static int Print(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x6000648")]
	[Address(RVA = "0x678A24", Offset = "0x674A24", VA = "0x678A24")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	private static int Loader(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x6000649")]
	[Address(RVA = "0x678D6C", Offset = "0x674D6C", VA = "0x678D6C")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	public static int DoFile(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x600064A")]
	[Address(RVA = "0x679148", Offset = "0x675148", VA = "0x679148")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	public static int LoadFile(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x600064B")]
	[Address(RVA = "0x6793AC", Offset = "0x6753AC", VA = "0x6793AC")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	private static int IsNull(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x600064C")]
	[Address(RVA = "0x679494", Offset = "0x675494", VA = "0x679494")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	private static int BufferToString(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x600064D")]
	[Address(RVA = "0x6797A8", Offset = "0x6757A8", VA = "0x6797A8")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	private static int GetClassType(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x600064E")]
	[Address(RVA = "0x67994C", Offset = "0x67594C", VA = "0x67994C")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	private static int TableToArray(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x600064F")]
	[Address(RVA = "0x679B18", Offset = "0x675B18", VA = "0x679B18")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	public static int op_ToString(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x6000650")]
	[Address(RVA = "0x67AF64", Offset = "0x676F64", VA = "0x67AF64")]
	public static string ToString(IntPtr L, int stackPos)
	{
		return null;
	}

	[Token(Token = "0x6000651")]
	[Address(RVA = "0x67A730", Offset = "0x676730", VA = "0x67A730")]
	public static object ToObject(IntPtr L, int stackPos)
	{
		return null;
	}

	[Token(Token = "0x6000652")]
	[Address(RVA = "0x67B06C", Offset = "0x67706C", VA = "0x67B06C")]
	public static LuaFunction ToLuaFunction(IntPtr L, int stackPos)
	{
		return null;
	}

	[Token(Token = "0x6000653")]
	[Address(RVA = "0x67B134", Offset = "0x677134", VA = "0x67B134")]
	public static LuaTable ToLuaTable(IntPtr L, int stackPos)
	{
		return null;
	}

	[Token(Token = "0x6000654")]
	[Address(RVA = "0x67B1FC", Offset = "0x6771FC", VA = "0x67B1FC")]
	public static LuaThread ToLuaThread(IntPtr L, int stackPos)
	{
		return null;
	}

	[Token(Token = "0x6000655")]
	[Address(RVA = "0x67B2C4", Offset = "0x6772C4", VA = "0x67B2C4")]
	public static Vector3 ToVector3(IntPtr L, int stackPos)
	{
		return default(Vector3);
	}

	[Token(Token = "0x6000656")]
	[Address(RVA = "0x67B354", Offset = "0x677354", VA = "0x67B354")]
	public static Vector4 ToVector4(IntPtr L, int stackPos)
	{
		return default(Vector4);
	}

	[Token(Token = "0x6000657")]
	[Address(RVA = "0x67B3E8", Offset = "0x6773E8", VA = "0x67B3E8")]
	public static Vector2 ToVector2(IntPtr L, int stackPos)
	{
		return default(Vector2);
	}

	[Token(Token = "0x6000658")]
	[Address(RVA = "0x67B464", Offset = "0x677464", VA = "0x67B464")]
	public static Quaternion ToQuaternion(IntPtr L, int stackPos)
	{
		return default(Quaternion);
	}

	[Token(Token = "0x6000659")]
	[Address(RVA = "0x67B4F8", Offset = "0x6774F8", VA = "0x67B4F8")]
	public static Color ToColor(IntPtr L, int stackPos)
	{
		return default(Color);
	}

	[Token(Token = "0x600065A")]
	[Address(RVA = "0x67B58C", Offset = "0x67758C", VA = "0x67B58C")]
	public static Ray ToRay(IntPtr L, int stackPos)
	{
		return default(Ray);
	}

	[Token(Token = "0x600065B")]
	[Address(RVA = "0x67B834", Offset = "0x677834", VA = "0x67B834")]
	public static Bounds ToBounds(IntPtr L, int stackPos)
	{
		return default(Bounds);
	}

	[Token(Token = "0x600065C")]
	[Address(RVA = "0x67BA04", Offset = "0x677A04", VA = "0x67BA04")]
	public static LayerMask ToLayerMask(IntPtr L, int stackPos)
	{
		return default(LayerMask);
	}

	[Token(Token = "0x600065D")]
	[Address(RVA = "0x67BA7C", Offset = "0x677A7C", VA = "0x67BA7C")]
	public static object ToVarObject(IntPtr L, int stackPos)
	{
		return null;
	}

	[Token(Token = "0x600065E")]
	[Address(RVA = "0x67BEB0", Offset = "0x677EB0", VA = "0x67BEB0")]
	public static object ToVarObject(IntPtr L, int stackPos, Type t)
	{
		return null;
	}

	[Token(Token = "0x600065F")]
	[Address(RVA = "0x67BD7C", Offset = "0x677D7C", VA = "0x67BD7C")]
	public static object ToVarTable(IntPtr L, int stackPos)
	{
		return null;
	}

	[Token(Token = "0x6000660")]
	public static T? ToNullable<T>(IntPtr L, int stackPos) where T : struct
	{
		return null;
	}

	[Token(Token = "0x6000661")]
	[Address(RVA = "0x67BFF0", Offset = "0x677FF0", VA = "0x67BFF0")]
	private static object ToObjectVec3(IntPtr L, int stackPos)
	{
		return null;
	}

	[Token(Token = "0x6000662")]
	[Address(RVA = "0x67C088", Offset = "0x678088", VA = "0x67C088")]
	private static object ToObjectQuat(IntPtr L, int stackPos)
	{
		return null;
	}

	[Token(Token = "0x6000663")]
	[Address(RVA = "0x67C120", Offset = "0x678120", VA = "0x67C120")]
	private static object ToObjectColor(IntPtr L, int stackPos)
	{
		return null;
	}

	[Token(Token = "0x6000664")]
	[Address(RVA = "0x67C1B8", Offset = "0x6781B8", VA = "0x67C1B8")]
	private static object ToObjectVec4(IntPtr L, int stackPos)
	{
		return null;
	}

	[Token(Token = "0x6000665")]
	[Address(RVA = "0x67C250", Offset = "0x678250", VA = "0x67C250")]
	private static object ToObjectVec2(IntPtr L, int stackPos)
	{
		return null;
	}

	[Token(Token = "0x6000666")]
	[Address(RVA = "0x67C2DC", Offset = "0x6782DC", VA = "0x67C2DC")]
	private static object ToObjectRay(IntPtr L, int stackPos)
	{
		return null;
	}

	[Token(Token = "0x6000667")]
	[Address(RVA = "0x67C380", Offset = "0x678380", VA = "0x67C380")]
	private static object ToObjectLayerMask(IntPtr L, int stackPos)
	{
		return null;
	}

	[Token(Token = "0x6000668")]
	[Address(RVA = "0x67C410", Offset = "0x678410", VA = "0x67C410")]
	private static object ToObjectBounds(IntPtr L, int stackPos)
	{
		return null;
	}

	[Token(Token = "0x6000669")]
	[Address(RVA = "0x67C4B4", Offset = "0x6784B4", VA = "0x67C4B4")]
	public static LuaFunction CheckLuaFunction(IntPtr L, int stackPos)
	{
		return null;
	}

	[Token(Token = "0x600066A")]
	[Address(RVA = "0x67C5B4", Offset = "0x6785B4", VA = "0x67C5B4")]
	public static LuaTable CheckLuaTable(IntPtr L, int stackPos)
	{
		return null;
	}

	[Token(Token = "0x600066B")]
	[Address(RVA = "0x67C6B4", Offset = "0x6786B4", VA = "0x67C6B4")]
	public static LuaThread CheckLuaThread(IntPtr L, int stackPos)
	{
		return null;
	}

	[Token(Token = "0x600066C")]
	[Address(RVA = "0x67C7B4", Offset = "0x6787B4", VA = "0x67C7B4")]
	public static LuaBaseRef CheckLuaBaseRef(IntPtr L, int stackPos)
	{
		return null;
	}

	[Token(Token = "0x600066D")]
	[Address(RVA = "0x67C980", Offset = "0x678980", VA = "0x67C980")]
	public static string CheckString(IntPtr L, int stackPos)
	{
		return null;
	}

	[Token(Token = "0x600066E")]
	[Address(RVA = "0x67CB94", Offset = "0x678B94", VA = "0x67CB94")]
	public static IntPtr CheckIntPtr(IntPtr L, int stackPos)
	{
		return default(IntPtr);
	}

	[Token(Token = "0x600066F")]
	[Address(RVA = "0x67ACD0", Offset = "0x676CD0", VA = "0x67ACD0")]
	public static Type CheckMonoType(IntPtr L, int stackPos)
	{
		return null;
	}

	[Token(Token = "0x6000670")]
	[Address(RVA = "0x67CC6C", Offset = "0x678C6C", VA = "0x67CC6C")]
	public static IEnumerator CheckIter(IntPtr L, int stackPos)
	{
		return null;
	}

	[Token(Token = "0x6000671")]
	[Address(RVA = "0x67A804", Offset = "0x676804", VA = "0x67A804")]
	public static object CheckObject(IntPtr L, int stackPos)
	{
		return null;
	}

	[Token(Token = "0x6000672")]
	[Address(RVA = "0x67CE5C", Offset = "0x678E5C", VA = "0x67CE5C")]
	public static object CheckObject(IntPtr L, int stackPos, Type type)
	{
		return null;
	}

	[Token(Token = "0x6000673")]
	public static object CheckObject<T>(IntPtr L, int stackPos) where T : class
	{
		return null;
	}

	[Token(Token = "0x6000674")]
	[Address(RVA = "0x67D084", Offset = "0x679084", VA = "0x67D084")]
	public static Vector3 CheckVector3(IntPtr L, int stackPos)
	{
		return default(Vector3);
	}

	[Token(Token = "0x6000675")]
	[Address(RVA = "0x67D1FC", Offset = "0x6791FC", VA = "0x67D1FC")]
	public static Quaternion CheckQuaternion(IntPtr L, int stackPos)
	{
		return default(Quaternion);
	}

	[Token(Token = "0x6000676")]
	[Address(RVA = "0x67D38C", Offset = "0x67938C", VA = "0x67D38C")]
	public static Vector2 CheckVector2(IntPtr L, int stackPos)
	{
		return default(Vector2);
	}

	[Token(Token = "0x6000677")]
	[Address(RVA = "0x67D4F0", Offset = "0x6794F0", VA = "0x67D4F0")]
	public static Vector4 CheckVector4(IntPtr L, int stackPos)
	{
		return default(Vector4);
	}

	[Token(Token = "0x6000678")]
	[Address(RVA = "0x67D680", Offset = "0x679680", VA = "0x67D680")]
	public static Color CheckColor(IntPtr L, int stackPos)
	{
		return default(Color);
	}

	[Token(Token = "0x6000679")]
	[Address(RVA = "0x67D7C0", Offset = "0x6797C0", VA = "0x67D7C0")]
	public static Ray CheckRay(IntPtr L, int stackPos)
	{
		return default(Ray);
	}

	[Token(Token = "0x600067A")]
	[Address(RVA = "0x67D90C", Offset = "0x67990C", VA = "0x67D90C")]
	public static Bounds CheckBounds(IntPtr L, int stackPos)
	{
		return default(Bounds);
	}

	[Token(Token = "0x600067B")]
	[Address(RVA = "0x67DA58", Offset = "0x679A58", VA = "0x67DA58")]
	public static LayerMask CheckLayerMask(IntPtr L, int stackPos)
	{
		return default(LayerMask);
	}

	[Token(Token = "0x600067C")]
	public static T CheckValue<T>(IntPtr L, int stackPos) where T : struct
	{
		return (T)null;
	}

	[Token(Token = "0x600067D")]
	public static T? CheckNullable<T>(IntPtr L, int stackPos) where T : struct
	{
		return null;
	}

	[Token(Token = "0x600067E")]
	[Address(RVA = "0x67DB74", Offset = "0x679B74", VA = "0x67DB74")]
	public static object CheckVarObject(IntPtr L, int stackPos, Type t)
	{
		return null;
	}

	[Token(Token = "0x600067F")]
	[Address(RVA = "0x67E648", Offset = "0x67A648", VA = "0x67E648")]
	public static UnityEngine.Object CheckUnityObject(IntPtr L, int stackPos, Type type)
	{
		return null;
	}

	[Token(Token = "0x6000680")]
	[Address(RVA = "0x67E940", Offset = "0x67A940", VA = "0x67E940")]
	public static TrackedReference CheckTrackedReference(IntPtr L, int stackPos, Type type)
	{
		return null;
	}

	[Token(Token = "0x6000681")]
	[Address(RVA = "0x67AA24", Offset = "0x676A24", VA = "0x67AA24")]
	public static object[] CheckObjectArray(IntPtr L, int stackPos)
	{
		return null;
	}

	[Token(Token = "0x6000682")]
	public static T[] CheckObjectArray<T>(IntPtr L, int stackPos) where T : class
	{
		return null;
	}

	[Token(Token = "0x6000683")]
	public static T[] CheckStructArray<T>(IntPtr L, int stackPos) where T : struct
	{
		return null;
	}

	[Token(Token = "0x6000684")]
	[Address(RVA = "0x67EC28", Offset = "0x67AC28", VA = "0x67EC28")]
	public static char[] CheckCharBuffer(IntPtr L, int stackPos)
	{
		return null;
	}

	[Token(Token = "0x6000685")]
	[Address(RVA = "0x67EDE0", Offset = "0x67ADE0", VA = "0x67EDE0")]
	public static byte[] CheckByteBuffer(IntPtr L, int stackPos)
	{
		return null;
	}

	[Token(Token = "0x6000686")]
	public static T[] CheckNumberArray<T>(IntPtr L, int stackPos) where T : struct
	{
		return null;
	}

	[Token(Token = "0x6000687")]
	[Address(RVA = "0x67EFF4", Offset = "0x67AFF4", VA = "0x67EFF4")]
	public static bool[] CheckBoolArray(IntPtr L, int stackPos)
	{
		return null;
	}

	[Token(Token = "0x6000688")]
	[Address(RVA = "0x67F2C4", Offset = "0x67B2C4", VA = "0x67F2C4")]
	public static string[] CheckStringArray(IntPtr L, int stackPos)
	{
		return null;
	}

	[Token(Token = "0x6000689")]
	[Address(RVA = "0x67F614", Offset = "0x67B614", VA = "0x67F614")]
	public static object CheckGenericObject(IntPtr L, int stackPos, Type type, out Type ArgType)
	{
		return null;
	}

	[Token(Token = "0x600068A")]
	[Address(RVA = "0x67F7B0", Offset = "0x67B7B0", VA = "0x67F7B0")]
	public static object CheckGenericObject(IntPtr L, int stackPos, Type type, out Type t1, out Type t2)
	{
		return null;
	}

	[Token(Token = "0x600068B")]
	[Address(RVA = "0x67F988", Offset = "0x67B988", VA = "0x67F988")]
	public static object CheckGenericObject(IntPtr L, int stackPos, Type type)
	{
		return null;
	}

	[Token(Token = "0x600068C")]
	[Address(RVA = "0x67FACC", Offset = "0x67BACC", VA = "0x67FACC")]
	public static object[] ToParamsObject(IntPtr L, int stackPos, int count)
	{
		return null;
	}

	[Token(Token = "0x600068D")]
	public static T[] ToParamsObject<T>(IntPtr L, int stackPos, int count)
	{
		return null;
	}

	[Token(Token = "0x600068E")]
	[Address(RVA = "0x67FBF4", Offset = "0x67BBF4", VA = "0x67FBF4")]
	public static string[] ToParamsString(IntPtr L, int stackPos, int count)
	{
		return null;
	}

	[Token(Token = "0x600068F")]
	public static T[] ToParamsNumber<T>(IntPtr L, int stackPos, int count) where T : struct
	{
		return null;
	}

	[Token(Token = "0x6000690")]
	[Address(RVA = "0x67FCF4", Offset = "0x67BCF4", VA = "0x67FCF4")]
	public static char[] ToParamsChar(IntPtr L, int stackPos, int count)
	{
		return null;
	}

	[Token(Token = "0x6000691")]
	[Address(RVA = "0x67FDE0", Offset = "0x67BDE0", VA = "0x67FDE0")]
	public static bool[] CheckParamsBool(IntPtr L, int stackPos, int count)
	{
		return null;
	}

	[Token(Token = "0x6000692")]
	public static T[] CheckParamsNumber<T>(IntPtr L, int stackPos, int count) where T : struct
	{
		return null;
	}

	[Token(Token = "0x6000693")]
	[Address(RVA = "0x67FED0", Offset = "0x67BED0", VA = "0x67FED0")]
	public static char[] CheckParamsChar(IntPtr L, int stackPos, int count)
	{
		return null;
	}

	[Token(Token = "0x6000694")]
	[Address(RVA = "0x67FFBC", Offset = "0x67BFBC", VA = "0x67FFBC")]
	public static string[] CheckParamsString(IntPtr L, int stackPos, int count)
	{
		return null;
	}

	[Token(Token = "0x6000695")]
	public static T[] CheckParamsObject<T>(IntPtr L, int stackPos, int count)
	{
		return null;
	}

	[Token(Token = "0x6000696")]
	[Address(RVA = "0x6800BC", Offset = "0x67C0BC", VA = "0x6800BC")]
	public static char[] ToCharBuffer(IntPtr L, int stackPos)
	{
		return null;
	}

	[Token(Token = "0x6000697")]
	[Address(RVA = "0x6801E0", Offset = "0x67C1E0", VA = "0x6801E0")]
	public static byte[] ToByteBuffer(IntPtr L, int stackPos)
	{
		return null;
	}

	[Token(Token = "0x6000698")]
	public static T[] ToNumberArray<T>(IntPtr L, int stackPos) where T : struct
	{
		return null;
	}

	[Token(Token = "0x6000699")]
	[Address(RVA = "0x680364", Offset = "0x67C364", VA = "0x680364")]
	public static bool[] ToBoolArray(IntPtr L, int stackPos)
	{
		return null;
	}

	[Token(Token = "0x600069A")]
	[Address(RVA = "0x6805B0", Offset = "0x67C5B0", VA = "0x6805B0")]
	public static string[] ToStringArray(IntPtr L, int stackPos)
	{
		return null;
	}

	[Token(Token = "0x600069B")]
	[Address(RVA = "0x68087C", Offset = "0x67C87C", VA = "0x68087C")]
	public static object[] ToObjectArray(IntPtr L, int stackPos)
	{
		return null;
	}

	[Token(Token = "0x600069C")]
	public static T[] ToObjectArray<T>(IntPtr L, int stackPos) where T : class
	{
		return null;
	}

	[Token(Token = "0x600069D")]
	public static T[] ToStructArray<T>(IntPtr L, int stackPos) where T : struct
	{
		return null;
	}

	[Token(Token = "0x600069E")]
	[Address(RVA = "0x680A98", Offset = "0x67CA98", VA = "0x680A98")]
	public static void Push(IntPtr L, Vector3 v3)
	{
	}

	[Token(Token = "0x600069F")]
	[Address(RVA = "0x680B18", Offset = "0x67CB18", VA = "0x680B18")]
	public static void Push(IntPtr L, Vector2 v2)
	{
	}

	[Token(Token = "0x60006A0")]
	[Address(RVA = "0x680B88", Offset = "0x67CB88", VA = "0x680B88")]
	public static void Push(IntPtr L, Vector4 v4)
	{
	}

	[Token(Token = "0x60006A1")]
	[Address(RVA = "0x680C10", Offset = "0x67CC10", VA = "0x680C10")]
	public static void Push(IntPtr L, Quaternion q)
	{
	}

	[Token(Token = "0x60006A2")]
	[Address(RVA = "0x680C98", Offset = "0x67CC98", VA = "0x680C98")]
	public static void Push(IntPtr L, Color clr)
	{
	}

	[Token(Token = "0x60006A3")]
	[Address(RVA = "0x680D20", Offset = "0x67CD20", VA = "0x680D20")]
	public static void Push(IntPtr L, Ray ray)
	{
	}

	[Token(Token = "0x60006A4")]
	[Address(RVA = "0x680E64", Offset = "0x67CE64", VA = "0x680E64")]
	public static void Push(IntPtr L, Bounds bound)
	{
	}

	[Token(Token = "0x60006A5")]
	[Address(RVA = "0x680FB4", Offset = "0x67CFB4", VA = "0x680FB4")]
	public static void Push(IntPtr L, RaycastHit hit)
	{
	}

	[Token(Token = "0x60006A6")]
	[Address(RVA = "0x681234", Offset = "0x67D234", VA = "0x681234")]
	public static void Push(IntPtr L, RaycastHit hit, int flag)
	{
	}

	[Token(Token = "0x60006A7")]
	[Address(RVA = "0x681534", Offset = "0x67D534", VA = "0x681534")]
	public static void Push(IntPtr L, Touch t)
	{
	}

	[Token(Token = "0x60006A8")]
	[Address(RVA = "0x6815C8", Offset = "0x67D5C8", VA = "0x6815C8")]
	public static void Push(IntPtr L, Touch t, int flag)
	{
	}

	[Token(Token = "0x60006A9")]
	[Address(RVA = "0x681868", Offset = "0x67D868", VA = "0x681868")]
	public static void PushLayerMask(IntPtr L, LayerMask l)
	{
	}

	[Token(Token = "0x60006AA")]
	[Address(RVA = "0x6818E8", Offset = "0x67D8E8", VA = "0x6818E8")]
	public static void Push(IntPtr L, LuaByteBuffer bb)
	{
	}

	[Token(Token = "0x60006AB")]
	[Address(RVA = "0x681954", Offset = "0x67D954", VA = "0x681954")]
	public static void PushByteBuffer(IntPtr L, byte[] buffer)
	{
	}

	[Token(Token = "0x60006AC")]
	[Address(RVA = "0x67AEB8", Offset = "0x676EB8", VA = "0x67AEB8")]
	public static void Push(IntPtr L, Array array)
	{
	}

	[Token(Token = "0x60006AD")]
	[Address(RVA = "0x681AE0", Offset = "0x67DAE0", VA = "0x681AE0")]
	public static void Push(IntPtr L, LuaBaseRef lbr)
	{
	}

	[Token(Token = "0x60006AE")]
	[Address(RVA = "0x67A938", Offset = "0x676938", VA = "0x67A938")]
	public static void Push(IntPtr L, Type t)
	{
	}

	[Token(Token = "0x60006AF")]
	[Address(RVA = "0x681BA0", Offset = "0x67DBA0", VA = "0x681BA0")]
	public static void Push(IntPtr L, Delegate ev)
	{
	}

	[Token(Token = "0x60006B0")]
	[Address(RVA = "0x681C4C", Offset = "0x67DC4C", VA = "0x681C4C")]
	public static void Push(IntPtr L, EventObject ev)
	{
	}

	[Token(Token = "0x60006B1")]
	[Address(RVA = "0x681CF8", Offset = "0x67DCF8", VA = "0x681CF8")]
	public static void Push(IntPtr L, IEnumerator iter)
	{
	}

	[Token(Token = "0x60006B2")]
	[Address(RVA = "0x681DE0", Offset = "0x67DDE0", VA = "0x681DE0")]
	public static void Push(IntPtr L, Enum e)
	{
	}

	[Token(Token = "0x60006B3")]
	public static void PushOut<T>(IntPtr L, LuaOut<T> lo)
	{
	}

	[Token(Token = "0x60006B4")]
	[Address(RVA = "0x681E6C", Offset = "0x67DE6C", VA = "0x681E6C")]
	public static void PushStruct(IntPtr L, object o)
	{
	}

	[Token(Token = "0x60006B5")]
	public static void PushValue<T>(IntPtr L, T v) where T : struct
	{
	}

	[Token(Token = "0x60006B6")]
	public static void PusNullable<T>(IntPtr L, T? v) where T : struct
	{
	}

	[Token(Token = "0x60006B7")]
	[Address(RVA = "0x6819C0", Offset = "0x67D9C0", VA = "0x6819C0")]
	public static void PushUserData(IntPtr L, object o, int reference)
	{
	}

	[Token(Token = "0x60006B8")]
	[Address(RVA = "0x6820EC", Offset = "0x67E0EC", VA = "0x6820EC")]
	private static int LuaPCall(IntPtr L, LuaCSFunction func)
	{
		return default(int);
	}

	[Token(Token = "0x60006B9")]
	[Address(RVA = "0x682068", Offset = "0x67E068", VA = "0x682068")]
	public static int LoadPreType(IntPtr L, Type type)
	{
		return default(int);
	}

	[Token(Token = "0x60006BA")]
	[Address(RVA = "0x68224C", Offset = "0x67E24C", VA = "0x68224C")]
	private static void PushUserObject(IntPtr L, object o)
	{
	}

	[Token(Token = "0x60006BB")]
	[Address(RVA = "0x68115C", Offset = "0x67D15C", VA = "0x68115C")]
	public static void Push(IntPtr L, UnityEngine.Object obj)
	{
	}

	[Token(Token = "0x60006BC")]
	[Address(RVA = "0x682304", Offset = "0x67E304", VA = "0x682304")]
	public static void Push(IntPtr L, TrackedReference obj)
	{
	}

	[Token(Token = "0x60006BD")]
	public static void PushSealed<T>(IntPtr L, T o)
	{
	}

	[Token(Token = "0x60006BE")]
	[Address(RVA = "0x6823AC", Offset = "0x67E3AC", VA = "0x6823AC")]
	public static void PushObject(IntPtr L, object o)
	{
	}

	[Token(Token = "0x60006BF")]
	[Address(RVA = "0x682504", Offset = "0x67E504", VA = "0x682504")]
	public static void Push(IntPtr L, nil obj)
	{
	}

	[Token(Token = "0x60006C0")]
	[Address(RVA = "0x68255C", Offset = "0x67E55C", VA = "0x68255C")]
	public static void Push(IntPtr L, object obj)
	{
	}

	[Token(Token = "0x60006C1")]
	[Address(RVA = "0x6834AC", Offset = "0x67F4AC", VA = "0x6834AC")]
	public static void SetBack(IntPtr L, int stackPos, object o)
	{
	}

	[Token(Token = "0x60006C2")]
	[Address(RVA = "0x683584", Offset = "0x67F584", VA = "0x683584")]
	public static int Destroy(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x60006C3")]
	[Address(RVA = "0x683630", Offset = "0x67F630", VA = "0x683630")]
	public static void CheckArgsCount(IntPtr L, string method, int count)
	{
	}

	[Token(Token = "0x60006C4")]
	[Address(RVA = "0x683728", Offset = "0x67F728", VA = "0x683728")]
	public static void CheckArgsCount(IntPtr L, int count)
	{
	}

	[Token(Token = "0x60006C5")]
	[Address(RVA = "0x683810", Offset = "0x67F810", VA = "0x683810")]
	public static Delegate CheckDelegate(Type t, IntPtr L, int stackPos)
	{
		return null;
	}

	[Token(Token = "0x60006C6")]
	public static Delegate CheckDelegate<T>(IntPtr L, int stackPos)
	{
		return null;
	}
}
