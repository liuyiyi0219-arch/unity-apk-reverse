using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppDummyDll;
using UnityEngine;

namespace LuaInterface;

[Token(Token = "0x200005A")]
public class LuaState : LuaStatePtr, IDisposable
{
	[Token(Token = "0x40000BC")]
	[Il2CppDummyDll.FieldOffset(Offset = "0x20")]
	public ObjectTranslator translator;

	[Token(Token = "0x40000BD")]
	[Il2CppDummyDll.FieldOffset(Offset = "0x28")]
	public LuaReflection reflection;

	[Token(Token = "0x40000CA")]
	[Il2CppDummyDll.FieldOffset(Offset = "0x60")]
	public Action OnDestroy;

	[Token(Token = "0x40000CB")]
	[Il2CppDummyDll.FieldOffset(Offset = "0x68")]
	private Dictionary<string, WeakReference> funcMap;

	[Token(Token = "0x40000CC")]
	[Il2CppDummyDll.FieldOffset(Offset = "0x70")]
	private Dictionary<int, WeakReference> funcRefMap;

	[Token(Token = "0x40000CD")]
	[Il2CppDummyDll.FieldOffset(Offset = "0x78")]
	private Dictionary<long, WeakReference> delegateMap;

	[Token(Token = "0x40000CE")]
	[Il2CppDummyDll.FieldOffset(Offset = "0x80")]
	private List<GCRef> gcList;

	[Token(Token = "0x40000CF")]
	[Il2CppDummyDll.FieldOffset(Offset = "0x88")]
	private List<LuaBaseRef> subList;

	[Token(Token = "0x40000D0")]
	[Il2CppDummyDll.FieldOffset(Offset = "0x90")]
	private Dictionary<Type, int> metaMap;

	[Token(Token = "0x40000D1")]
	[Il2CppDummyDll.FieldOffset(Offset = "0x98")]
	private Dictionary<Enum, object> enumMap;

	[Token(Token = "0x40000D2")]
	[Il2CppDummyDll.FieldOffset(Offset = "0xA0")]
	private Dictionary<Type, LuaCSFunction> preLoadMap;

	[Token(Token = "0x40000D3")]
	[Il2CppDummyDll.FieldOffset(Offset = "0xA8")]
	private Dictionary<int, Type> typeMap;

	[Token(Token = "0x40000D4")]
	[Il2CppDummyDll.FieldOffset(Offset = "0xB0")]
	private HashSet<Type> genericSet;

	[Token(Token = "0x40000D5")]
	[Il2CppDummyDll.FieldOffset(Offset = "0xB8")]
	private HashSet<string> moduleSet;

	[Token(Token = "0x40000D6")]
	[Il2CppDummyDll.FieldOffset(Offset = "0x0")]
	private static LuaState mainState;

	[Token(Token = "0x40000D7")]
	[Il2CppDummyDll.FieldOffset(Offset = "0x8")]
	private static LuaState injectionState;

	[Token(Token = "0x40000D8")]
	[Il2CppDummyDll.FieldOffset(Offset = "0x10")]
	private static Dictionary<IntPtr, LuaState> stateMap;

	[Token(Token = "0x40000D9")]
	[Il2CppDummyDll.FieldOffset(Offset = "0xC0")]
	private int beginCount;

	[Token(Token = "0x40000DA")]
	[Il2CppDummyDll.FieldOffset(Offset = "0xC4")]
	private bool beLogGC;

	[Token(Token = "0x40000DB")]
	[Il2CppDummyDll.FieldOffset(Offset = "0xC5")]
	private bool bInjectionInited;

	[Token(Token = "0x40000DC")]
	[Il2CppDummyDll.FieldOffset(Offset = "0xC8")]
	private HashSet<Type> missSet;

	[Token(Token = "0x17000019")]
	public int ArrayMetatable
	{
		[Token(Token = "0x600045F")]
		[Address(RVA = "0x661468", Offset = "0x65D468", VA = "0x661468")]
		[CompilerGenerated]
		get
		{
			return default(int);
		}
		[Token(Token = "0x6000460")]
		[Address(RVA = "0x661470", Offset = "0x65D470", VA = "0x661470")]
		[CompilerGenerated]
		private set
		{
		}
	}

	[Token(Token = "0x1700001A")]
	public int DelegateMetatable
	{
		[Token(Token = "0x6000461")]
		[Address(RVA = "0x661478", Offset = "0x65D478", VA = "0x661478")]
		[CompilerGenerated]
		get
		{
			return default(int);
		}
		[Token(Token = "0x6000462")]
		[Address(RVA = "0x661480", Offset = "0x65D480", VA = "0x661480")]
		[CompilerGenerated]
		private set
		{
		}
	}

	[Token(Token = "0x1700001B")]
	public int TypeMetatable
	{
		[Token(Token = "0x6000463")]
		[Address(RVA = "0x661488", Offset = "0x65D488", VA = "0x661488")]
		[CompilerGenerated]
		get
		{
			return default(int);
		}
		[Token(Token = "0x6000464")]
		[Address(RVA = "0x661490", Offset = "0x65D490", VA = "0x661490")]
		[CompilerGenerated]
		private set
		{
		}
	}

	[Token(Token = "0x1700001C")]
	public int EnumMetatable
	{
		[Token(Token = "0x6000465")]
		[Address(RVA = "0x661498", Offset = "0x65D498", VA = "0x661498")]
		[CompilerGenerated]
		get
		{
			return default(int);
		}
		[Token(Token = "0x6000466")]
		[Address(RVA = "0x6614A0", Offset = "0x65D4A0", VA = "0x6614A0")]
		[CompilerGenerated]
		private set
		{
		}
	}

	[Token(Token = "0x1700001D")]
	public int IterMetatable
	{
		[Token(Token = "0x6000467")]
		[Address(RVA = "0x6614A8", Offset = "0x65D4A8", VA = "0x6614A8")]
		[CompilerGenerated]
		get
		{
			return default(int);
		}
		[Token(Token = "0x6000468")]
		[Address(RVA = "0x6614B0", Offset = "0x65D4B0", VA = "0x6614B0")]
		[CompilerGenerated]
		private set
		{
		}
	}

	[Token(Token = "0x1700001E")]
	public int EventMetatable
	{
		[Token(Token = "0x6000469")]
		[Address(RVA = "0x6614B8", Offset = "0x65D4B8", VA = "0x6614B8")]
		[CompilerGenerated]
		get
		{
			return default(int);
		}
		[Token(Token = "0x600046A")]
		[Address(RVA = "0x6614C0", Offset = "0x65D4C0", VA = "0x6614C0")]
		[CompilerGenerated]
		private set
		{
		}
	}

	[Token(Token = "0x1700001F")]
	public int PackBounds
	{
		[Token(Token = "0x600046B")]
		[Address(RVA = "0x6614C8", Offset = "0x65D4C8", VA = "0x6614C8")]
		[CompilerGenerated]
		get
		{
			return default(int);
		}
		[Token(Token = "0x600046C")]
		[Address(RVA = "0x6614D0", Offset = "0x65D4D0", VA = "0x6614D0")]
		[CompilerGenerated]
		private set
		{
		}
	}

	[Token(Token = "0x17000020")]
	public int UnpackBounds
	{
		[Token(Token = "0x600046D")]
		[Address(RVA = "0x6614D8", Offset = "0x65D4D8", VA = "0x6614D8")]
		[CompilerGenerated]
		get
		{
			return default(int);
		}
		[Token(Token = "0x600046E")]
		[Address(RVA = "0x6614E0", Offset = "0x65D4E0", VA = "0x6614E0")]
		[CompilerGenerated]
		private set
		{
		}
	}

	[Token(Token = "0x17000021")]
	public int PackRay
	{
		[Token(Token = "0x600046F")]
		[Address(RVA = "0x6614E8", Offset = "0x65D4E8", VA = "0x6614E8")]
		[CompilerGenerated]
		get
		{
			return default(int);
		}
		[Token(Token = "0x6000470")]
		[Address(RVA = "0x6614F0", Offset = "0x65D4F0", VA = "0x6614F0")]
		[CompilerGenerated]
		private set
		{
		}
	}

	[Token(Token = "0x17000022")]
	public int UnpackRay
	{
		[Token(Token = "0x6000471")]
		[Address(RVA = "0x6614F8", Offset = "0x65D4F8", VA = "0x6614F8")]
		[CompilerGenerated]
		get
		{
			return default(int);
		}
		[Token(Token = "0x6000472")]
		[Address(RVA = "0x661500", Offset = "0x65D500", VA = "0x661500")]
		[CompilerGenerated]
		private set
		{
		}
	}

	[Token(Token = "0x17000023")]
	public int PackRaycastHit
	{
		[Token(Token = "0x6000473")]
		[Address(RVA = "0x661508", Offset = "0x65D508", VA = "0x661508")]
		[CompilerGenerated]
		get
		{
			return default(int);
		}
		[Token(Token = "0x6000474")]
		[Address(RVA = "0x661510", Offset = "0x65D510", VA = "0x661510")]
		[CompilerGenerated]
		private set
		{
		}
	}

	[Token(Token = "0x17000024")]
	public int PackTouch
	{
		[Token(Token = "0x6000475")]
		[Address(RVA = "0x661518", Offset = "0x65D518", VA = "0x661518")]
		[CompilerGenerated]
		get
		{
			return default(int);
		}
		[Token(Token = "0x6000476")]
		[Address(RVA = "0x661520", Offset = "0x65D520", VA = "0x661520")]
		[CompilerGenerated]
		private set
		{
		}
	}

	[Token(Token = "0x17000025")]
	public bool LogGC
	{
		[Token(Token = "0x6000477")]
		[Address(RVA = "0x661528", Offset = "0x65D528", VA = "0x661528")]
		get
		{
			return default(bool);
		}
		[Token(Token = "0x6000478")]
		[Address(RVA = "0x661530", Offset = "0x65D530", VA = "0x661530")]
		set
		{
		}
	}

	[Token(Token = "0x17000026")]
	public object this[string fullPath]
	{
		[Token(Token = "0x60004FF")]
		[Address(RVA = "0x66E664", Offset = "0x66A664", VA = "0x66E664")]
		get
		{
			return null;
		}
		[Token(Token = "0x6000500")]
		[Address(RVA = "0x66E7A4", Offset = "0x66A7A4", VA = "0x66E7A4")]
		set
		{
		}
	}

	[Token(Token = "0x6000479")]
	[Address(RVA = "0x661554", Offset = "0x65D554", VA = "0x661554")]
	public LuaState()
	{
	}

	[Token(Token = "0x600047A")]
	[Address(RVA = "0x6690D4", Offset = "0x6650D4", VA = "0x6690D4")]
	private void OpenBaseLibs()
	{
	}

	[Token(Token = "0x600047B")]
	[Address(RVA = "0x669550", Offset = "0x665550", VA = "0x669550")]
	private void InitLuaPath()
	{
	}

	[Token(Token = "0x600047C")]
	[Address(RVA = "0x6699DC", Offset = "0x6659DC", VA = "0x6699DC")]
	private void OpenBaseLuaLibs()
	{
	}

	[Token(Token = "0x600047D")]
	[Address(RVA = "0x669A68", Offset = "0x665A68", VA = "0x669A68")]
	public void Start()
	{
	}

	[Token(Token = "0x600047E")]
	[Address(RVA = "0x669CA0", Offset = "0x665CA0", VA = "0x669CA0")]
	public int OpenLibs(LuaCSFunction open)
	{
		return default(int);
	}

	[Token(Token = "0x600047F")]
	[Address(RVA = "0x669CC8", Offset = "0x665CC8", VA = "0x669CC8")]
	public void BeginPreLoad()
	{
	}

	[Token(Token = "0x6000480")]
	[Address(RVA = "0x669E70", Offset = "0x665E70", VA = "0x669E70")]
	public void EndPreLoad()
	{
	}

	[Token(Token = "0x6000481")]
	[Address(RVA = "0x669F00", Offset = "0x665F00", VA = "0x669F00")]
	public void AddPreLoad(string name, LuaCSFunction func, Type type)
	{
	}

	[Token(Token = "0x6000482")]
	[Address(RVA = "0x66A110", Offset = "0x666110", VA = "0x66A110")]
	public void AddPreLoad(string name, LuaCSFunction func)
	{
	}

	[Token(Token = "0x6000483")]
	[Address(RVA = "0x66A190", Offset = "0x666190", VA = "0x66A190")]
	public int BeginPreModule(string name)
	{
		return default(int);
	}

	[Token(Token = "0x6000484")]
	[Address(RVA = "0x66A340", Offset = "0x666340", VA = "0x66A340")]
	public void EndPreModule(int reference)
	{
	}

	[Token(Token = "0x6000485")]
	[Address(RVA = "0x66A3B8", Offset = "0x6663B8", VA = "0x66A3B8")]
	public void EndPreModule(IntPtr L, int reference)
	{
	}

	[Token(Token = "0x6000486")]
	[Address(RVA = "0x66A430", Offset = "0x666430", VA = "0x66A430")]
	public void BindPreModule(Type t, LuaCSFunction func)
	{
	}

	[Token(Token = "0x6000487")]
	[Address(RVA = "0x66A498", Offset = "0x666498", VA = "0x66A498")]
	public LuaCSFunction GetPreModule(Type t)
	{
		return null;
	}

	[Token(Token = "0x6000488")]
	[Address(RVA = "0x66964C", Offset = "0x66564C", VA = "0x66964C")]
	public bool BeginModule(string name)
	{
		return default(bool);
	}

	[Token(Token = "0x6000489")]
	[Address(RVA = "0x669734", Offset = "0x665734", VA = "0x669734")]
	public void EndModule()
	{
	}

	[Token(Token = "0x600048A")]
	[Address(RVA = "0x66A508", Offset = "0x666508", VA = "0x66A508")]
	private void BindTypeRef(int reference, Type t)
	{
	}

	[Token(Token = "0x600048B")]
	[Address(RVA = "0x66A5F0", Offset = "0x6665F0", VA = "0x66A5F0")]
	public Type GetClassType(int reference)
	{
		return null;
	}

	[Token(Token = "0x600048C")]
	[Address(RVA = "0x6613BC", Offset = "0x65D3BC", VA = "0x6613BC")]
	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	public static int Collect(IntPtr L)
	{
		return default(int);
	}

	[Token(Token = "0x600048D")]
	[Address(RVA = "0x66A6C4", Offset = "0x6666C4", VA = "0x66A6C4")]
	public static bool GetInjectInitState(int index)
	{
		return default(bool);
	}

	[Token(Token = "0x600048E")]
	[Address(RVA = "0x66A7D0", Offset = "0x6667D0", VA = "0x66A7D0")]
	private string GetToLuaTypeName(Type t)
	{
		return null;
	}

	[Token(Token = "0x600048F")]
	[Address(RVA = "0x66A85C", Offset = "0x66685C", VA = "0x66A85C")]
	public int BeginClass(Type t, Type baseType, [Optional] string name)
	{
		return default(int);
	}

	[Token(Token = "0x6000490")]
	[Address(RVA = "0x66ACCC", Offset = "0x666CCC", VA = "0x66ACCC")]
	public void EndClass()
	{
	}

	[Token(Token = "0x6000491")]
	[Address(RVA = "0x66AD28", Offset = "0x666D28", VA = "0x66AD28")]
	public int BeginEnum(Type t)
	{
		return default(int);
	}

	[Token(Token = "0x6000492")]
	[Address(RVA = "0x66AE98", Offset = "0x666E98", VA = "0x66AE98")]
	public void EndEnum()
	{
	}

	[Token(Token = "0x6000493")]
	[Address(RVA = "0x66AEF4", Offset = "0x666EF4", VA = "0x66AEF4")]
	public void BeginStaticLibs(string name)
	{
	}

	[Token(Token = "0x6000494")]
	[Address(RVA = "0x66AFAC", Offset = "0x666FAC", VA = "0x66AFAC")]
	public void EndStaticLibs()
	{
	}

	[Token(Token = "0x6000495")]
	[Address(RVA = "0x66AC0C", Offset = "0x666C0C", VA = "0x66AC0C")]
	public void RegFunction(string name, LuaCSFunction func)
	{
	}

	[Token(Token = "0x6000496")]
	[Address(RVA = "0x66B008", Offset = "0x667008", VA = "0x66B008")]
	public void RegVar(string name, LuaCSFunction get, LuaCSFunction set)
	{
	}

	[Token(Token = "0x6000497")]
	[Address(RVA = "0x66B0FC", Offset = "0x6670FC", VA = "0x66B0FC")]
	public void RegConstant(string name, double d)
	{
	}

	[Token(Token = "0x6000498")]
	[Address(RVA = "0x66B178", Offset = "0x667178", VA = "0x66B178")]
	public void RegConstant(string name, bool flag)
	{
	}

	[Token(Token = "0x6000499")]
	[Address(RVA = "0x669BD0", Offset = "0x665BD0", VA = "0x669BD0")]
	private int GetFuncRef(string name)
	{
		return default(int);
	}

	[Token(Token = "0x600049A")]
	[Address(RVA = "0x66B510", Offset = "0x667510", VA = "0x66B510")]
	public static LuaState Get(IntPtr ptr)
	{
		return null;
	}

	[Token(Token = "0x600049B")]
	[Address(RVA = "0x66A660", Offset = "0x666660", VA = "0x66A660")]
	public static ObjectTranslator GetTranslator(IntPtr ptr)
	{
		return null;
	}

	[Token(Token = "0x600049C")]
	[Address(RVA = "0x66B568", Offset = "0x667568", VA = "0x66B568")]
	public static LuaReflection GetReflection(IntPtr ptr)
	{
		return null;
	}

	[Token(Token = "0x600049D")]
	[Address(RVA = "0x66B5CC", Offset = "0x6675CC", VA = "0x66B5CC")]
	public void DoString(string chunk, [Optional] string chunkName)
	{
	}

	[Token(Token = "0x600049E")]
	public T DoString<T>(string chunk, [Optional] string chunkName)
	{
		return (T)null;
	}

	[Token(Token = "0x600049F")]
	[Address(RVA = "0x66B760", Offset = "0x667760", VA = "0x66B760")]
	private byte[] LoadFileBuffer(string fileName)
	{
		return null;
	}

	[Token(Token = "0x60004A0")]
	[Address(RVA = "0x66B824", Offset = "0x667824", VA = "0x66B824")]
	private string LuaChunkName(string name)
	{
		return null;
	}

	[Token(Token = "0x60004A1")]
	[Address(RVA = "0x669A30", Offset = "0x665A30", VA = "0x669A30")]
	public void DoFile(string fileName)
	{
	}

	[Token(Token = "0x60004A2")]
	public T DoFile<T>(string fileName)
	{
		return (T)null;
	}

	[Token(Token = "0x60004A3")]
	[Address(RVA = "0x66B8BC", Offset = "0x6678BC", VA = "0x66B8BC")]
	public void Require(string fileName)
	{
	}

	[Token(Token = "0x60004A4")]
	public T Require<T>(string fileName)
	{
		return (T)null;
	}

	[Token(Token = "0x60004A5")]
	[Address(RVA = "0x66979C", Offset = "0x66579C", VA = "0x66979C")]
	public void InitPackagePath()
	{
	}

	[Token(Token = "0x60004A6")]
	[Address(RVA = "0x66BAAC", Offset = "0x667AAC", VA = "0x66BAAC")]
	private string ToPackagePath(string path)
	{
		return null;
	}

	[Token(Token = "0x60004A7")]
	[Address(RVA = "0x669900", Offset = "0x665900", VA = "0x669900")]
	public void AddSearchPath(string fullPath)
	{
	}

	[Token(Token = "0x60004A8")]
	[Address(RVA = "0x66BCB8", Offset = "0x667CB8", VA = "0x66BCB8")]
	public void RemoveSeachPath(string fullPath)
	{
	}

	[Token(Token = "0x60004A9")]
	[Address(RVA = "0x651E5C", Offset = "0x64DE5C", VA = "0x651E5C")]
	public int BeginPCall(int reference)
	{
		return default(int);
	}

	[Token(Token = "0x60004AA")]
	[Address(RVA = "0x651EC8", Offset = "0x64DEC8", VA = "0x651EC8")]
	public void PCall(int args, int oldTop)
	{
	}

	[Token(Token = "0x60004AB")]
	[Address(RVA = "0x651FB4", Offset = "0x64DFB4", VA = "0x651FB4")]
	public void EndPCall(int oldTop)
	{
	}

	[Token(Token = "0x60004AC")]
	[Address(RVA = "0x6537B4", Offset = "0x64F7B4", VA = "0x6537B4")]
	public void PushArgs(object[] args)
	{
	}

	[Token(Token = "0x60004AD")]
	[Address(RVA = "0x66BD90", Offset = "0x667D90", VA = "0x66BD90")]
	private void CheckNull(LuaBaseRef lbr, string fmt, object arg0)
	{
	}

	[Token(Token = "0x60004AE")]
	[Address(RVA = "0x66BE10", Offset = "0x667E10", VA = "0x66BE10")]
	private bool PushLuaTable(string fullPath, [Optional] bool checkMap)
	{
		return default(bool);
	}

	[Token(Token = "0x60004AF")]
	[Address(RVA = "0x66B208", Offset = "0x667208", VA = "0x66B208")]
	private bool PushLuaFunction(string fullPath, [Optional] bool checkMap)
	{
		return default(bool);
	}

	[Token(Token = "0x60004B0")]
	[Address(RVA = "0x66BFA0", Offset = "0x667FA0", VA = "0x66BFA0")]
	private void RemoveFromGCList(int reference)
	{
	}

	[Token(Token = "0x60004B1")]
	[Address(RVA = "0x66C10C", Offset = "0x66810C", VA = "0x66C10C")]
	public LuaFunction GetFunction(string name, [Optional] bool beLogMiss)
	{
		return null;
	}

	[Token(Token = "0x60004B2")]
	[Address(RVA = "0x66C618", Offset = "0x668618", VA = "0x66C618")]
	private LuaBaseRef TryGetLuaRef(int reference)
	{
		return null;
	}

	[Token(Token = "0x60004B3")]
	[Address(RVA = "0x66C750", Offset = "0x668750", VA = "0x66C750")]
	public LuaFunction GetFunction(int reference)
	{
		return null;
	}

	[Token(Token = "0x60004B4")]
	[Address(RVA = "0x66C900", Offset = "0x668900", VA = "0x66C900")]
	public LuaTable GetTable(string fullPath, [Optional] bool beLogMiss)
	{
		return null;
	}

	[Token(Token = "0x60004B5")]
	[Address(RVA = "0x66CD80", Offset = "0x668D80", VA = "0x66CD80")]
	public LuaTable GetTable(int reference)
	{
		return null;
	}

	[Token(Token = "0x60004B6")]
	[Address(RVA = "0x66CEB0", Offset = "0x668EB0", VA = "0x66CEB0")]
	public LuaThread GetLuaThread(int reference)
	{
		return null;
	}

	[Token(Token = "0x60004B7")]
	[Address(RVA = "0x66CFE0", Offset = "0x668FE0", VA = "0x66CFE0")]
	public LuaDelegate GetLuaDelegate(LuaFunction func)
	{
		return null;
	}

	[Token(Token = "0x60004B8")]
	[Address(RVA = "0x66D108", Offset = "0x669108", VA = "0x66D108")]
	public LuaDelegate GetLuaDelegate(LuaFunction func, LuaTable self)
	{
		return null;
	}

	[Token(Token = "0x60004B9")]
	[Address(RVA = "0x66D26C", Offset = "0x66926C", VA = "0x66D26C")]
	public void AddLuaDelegate(LuaDelegate target, LuaFunction func)
	{
	}

	[Token(Token = "0x60004BA")]
	[Address(RVA = "0x66D334", Offset = "0x669334", VA = "0x66D334")]
	public void AddLuaDelegate(LuaDelegate target, LuaFunction func, LuaTable self)
	{
	}

	[Token(Token = "0x60004BB")]
	[Address(RVA = "0x66D43C", Offset = "0x66943C", VA = "0x66D43C")]
	public bool CheckTop()
	{
		return default(bool);
	}

	[Token(Token = "0x60004BC")]
	[Address(RVA = "0x652730", Offset = "0x64E730", VA = "0x652730")]
	public void Push(bool b)
	{
	}

	[Token(Token = "0x60004BD")]
	[Address(RVA = "0x652380", Offset = "0x64E380", VA = "0x652380")]
	public void Push(double d)
	{
	}

	[Token(Token = "0x60004BE")]
	[Address(RVA = "0x652564", Offset = "0x64E564", VA = "0x652564")]
	public void Push(uint un)
	{
	}

	[Token(Token = "0x60004BF")]
	[Address(RVA = "0x652418", Offset = "0x64E418", VA = "0x652418")]
	public void Push(int n)
	{
	}

	[Token(Token = "0x60004C0")]
	[Address(RVA = "0x66D4FC", Offset = "0x6694FC", VA = "0x66D4FC")]
	public void Push(short s)
	{
	}

	[Token(Token = "0x60004C1")]
	[Address(RVA = "0x66D56C", Offset = "0x66956C", VA = "0x66D56C")]
	public void Push(ushort us)
	{
	}

	[Token(Token = "0x60004C2")]
	[Address(RVA = "0x6525FC", Offset = "0x64E5FC", VA = "0x6525FC")]
	public void Push(long l)
	{
	}

	[Token(Token = "0x60004C3")]
	[Address(RVA = "0x652694", Offset = "0x64E694", VA = "0x652694")]
	public void Push(ulong ul)
	{
	}

	[Token(Token = "0x60004C4")]
	[Address(RVA = "0x6527C8", Offset = "0x64E7C8", VA = "0x6527C8")]
	public void Push(string str)
	{
	}

	[Token(Token = "0x60004C5")]
	[Address(RVA = "0x652860", Offset = "0x64E860", VA = "0x652860")]
	public void Push(IntPtr p)
	{
	}

	[Token(Token = "0x60004C6")]
	[Address(RVA = "0x652BE0", Offset = "0x64EBE0", VA = "0x652BE0")]
	public void Push(Vector3 v3)
	{
	}

	[Token(Token = "0x60004C7")]
	[Address(RVA = "0x652C90", Offset = "0x64EC90", VA = "0x652C90")]
	public void Push(Vector2 v2)
	{
	}

	[Token(Token = "0x60004C8")]
	[Address(RVA = "0x652D30", Offset = "0x64ED30", VA = "0x652D30")]
	public void Push(Vector4 v4)
	{
	}

	[Token(Token = "0x60004C9")]
	[Address(RVA = "0x652EA0", Offset = "0x64EEA0", VA = "0x652EA0")]
	public void Push(Color clr)
	{
	}

	[Token(Token = "0x60004CA")]
	[Address(RVA = "0x652DE8", Offset = "0x64EDE8", VA = "0x652DE8")]
	public void Push(Quaternion q)
	{
	}

	[Token(Token = "0x60004CB")]
	[Address(RVA = "0x65301C", Offset = "0x64F01C", VA = "0x65301C")]
	public void Push(Ray ray)
	{
	}

	[Token(Token = "0x60004CC")]
	[Address(RVA = "0x6531A4", Offset = "0x64F1A4", VA = "0x6531A4")]
	public void Push(Bounds bound)
	{
	}

	[Token(Token = "0x60004CD")]
	[Address(RVA = "0x65332C", Offset = "0x64F32C", VA = "0x65332C")]
	public void Push(RaycastHit hit)
	{
	}

	[Token(Token = "0x60004CE")]
	[Address(RVA = "0x6534B4", Offset = "0x64F4B4", VA = "0x6534B4")]
	public void Push(Touch touch)
	{
	}

	[Token(Token = "0x60004CF")]
	[Address(RVA = "0x6524B4", Offset = "0x64E4B4", VA = "0x6524B4")]
	public void PushLayerMask(LayerMask mask)
	{
	}

	[Token(Token = "0x60004D0")]
	[Address(RVA = "0x653610", Offset = "0x64F610", VA = "0x653610")]
	public void Push(LuaByteBuffer bb)
	{
	}

	[Token(Token = "0x60004D1")]
	[Address(RVA = "0x66D5DC", Offset = "0x6695DC", VA = "0x66D5DC")]
	public void PushByteBuffer(byte[] buffer)
	{
	}

	[Token(Token = "0x60004D2")]
	[Address(RVA = "0x6538FC", Offset = "0x64F8FC", VA = "0x6538FC")]
	public void PushByteBuffer(byte[] buffer, int len)
	{
	}

	[Token(Token = "0x60004D3")]
	[Address(RVA = "0x6528CC", Offset = "0x64E8CC", VA = "0x6528CC")]
	public void Push(LuaBaseRef lbr)
	{
	}

	[Token(Token = "0x60004D4")]
	[Address(RVA = "0x66D714", Offset = "0x669714", VA = "0x66D714")]
	private void PushUserData(object o, int reference)
	{
	}

	[Token(Token = "0x60004D5")]
	[Address(RVA = "0x652BA4", Offset = "0x64EBA4", VA = "0x652BA4")]
	public void Push(Array array)
	{
	}

	[Token(Token = "0x60004D6")]
	[Address(RVA = "0x652A88", Offset = "0x64EA88", VA = "0x652A88")]
	public void Push(Type t)
	{
	}

	[Token(Token = "0x60004D7")]
	[Address(RVA = "0x66D818", Offset = "0x669818", VA = "0x66D818")]
	public void Push(Delegate ev)
	{
	}

	[Token(Token = "0x60004D8")]
	[Address(RVA = "0x66D828", Offset = "0x669828", VA = "0x66D828")]
	public object GetEnumObj(Enum e)
	{
		return null;
	}

	[Token(Token = "0x60004D9")]
	[Address(RVA = "0x652B44", Offset = "0x64EB44", VA = "0x652B44")]
	public void Push(Enum e)
	{
	}

	[Token(Token = "0x60004DA")]
	[Address(RVA = "0x66D8CC", Offset = "0x6698CC", VA = "0x66D8CC")]
	public void Push(IEnumerator iter)
	{
	}

	[Token(Token = "0x60004DB")]
	[Address(RVA = "0x6529F0", Offset = "0x64E9F0", VA = "0x6529F0")]
	public void Push(UnityEngine.Object obj)
	{
	}

	[Token(Token = "0x60004DC")]
	[Address(RVA = "0x66D938", Offset = "0x669938", VA = "0x66D938")]
	public void Push(TrackedReference tracker)
	{
	}

	[Token(Token = "0x60004DD")]
	[Address(RVA = "0x652958", Offset = "0x64E958", VA = "0x652958")]
	public void PushVariant(object obj)
	{
	}

	[Token(Token = "0x60004DE")]
	[Address(RVA = "0x653748", Offset = "0x64F748", VA = "0x653748")]
	public void PushObject(object obj)
	{
	}

	[Token(Token = "0x60004DF")]
	public void PushSealed<T>(T o)
	{
	}

	[Token(Token = "0x60004E0")]
	public void PushValue<T>(T v) where T : struct
	{
	}

	[Token(Token = "0x60004E1")]
	public void PushGeneric<T>(T o)
	{
	}

	[Token(Token = "0x60004E2")]
	[Address(RVA = "0x66D9A4", Offset = "0x6699A4", VA = "0x66D9A4")]
	private Vector3 ToVector3(int stackPos)
	{
		return default(Vector3);
	}

	[Token(Token = "0x60004E3")]
	[Address(RVA = "0x653DD8", Offset = "0x64FDD8", VA = "0x653DD8")]
	public Vector3 CheckVector3(int stackPos)
	{
		return default(Vector3);
	}

	[Token(Token = "0x60004E4")]
	[Address(RVA = "0x65401C", Offset = "0x65001C", VA = "0x65401C")]
	public Quaternion CheckQuaternion(int stackPos)
	{
		return default(Quaternion);
	}

	[Token(Token = "0x60004E5")]
	[Address(RVA = "0x654270", Offset = "0x650270", VA = "0x654270")]
	public Vector2 CheckVector2(int stackPos)
	{
		return default(Vector2);
	}

	[Token(Token = "0x60004E6")]
	[Address(RVA = "0x654498", Offset = "0x650498", VA = "0x654498")]
	public Vector4 CheckVector4(int stackPos)
	{
		return default(Vector4);
	}

	[Token(Token = "0x60004E7")]
	[Address(RVA = "0x6546EC", Offset = "0x6506EC", VA = "0x6546EC")]
	public Color CheckColor(int stackPos)
	{
		return default(Color);
	}

	[Token(Token = "0x60004E8")]
	[Address(RVA = "0x654924", Offset = "0x650924", VA = "0x654924")]
	public Ray CheckRay(int stackPos)
	{
		return default(Ray);
	}

	[Token(Token = "0x60004E9")]
	[Address(RVA = "0x654CF8", Offset = "0x650CF8", VA = "0x654CF8")]
	public Bounds CheckBounds(int stackPos)
	{
		return default(Bounds);
	}

	[Token(Token = "0x60004EA")]
	[Address(RVA = "0x65500C", Offset = "0x65100C", VA = "0x65500C")]
	public LayerMask CheckLayerMask(int stackPos)
	{
		return default(LayerMask);
	}

	[Token(Token = "0x60004EB")]
	[Address(RVA = "0x6551F4", Offset = "0x6511F4", VA = "0x6551F4")]
	public long CheckLong(int stackPos)
	{
		return default(long);
	}

	[Token(Token = "0x60004EC")]
	[Address(RVA = "0x65533C", Offset = "0x65133C", VA = "0x65533C")]
	public ulong CheckULong(int stackPos)
	{
		return default(ulong);
	}

	[Token(Token = "0x60004ED")]
	[Address(RVA = "0x653CA4", Offset = "0x64FCA4", VA = "0x653CA4")]
	public string CheckString(int stackPos)
	{
		return null;
	}

	[Token(Token = "0x60004EE")]
	[Address(RVA = "0x655484", Offset = "0x651484", VA = "0x655484")]
	public Delegate CheckDelegate(int stackPos)
	{
		return null;
	}

	[Token(Token = "0x60004EF")]
	[Address(RVA = "0x655744", Offset = "0x651744", VA = "0x655744")]
	public char[] CheckCharBuffer(int stackPos)
	{
		return null;
	}

	[Token(Token = "0x60004F0")]
	[Address(RVA = "0x655878", Offset = "0x651878", VA = "0x655878")]
	public byte[] CheckByteBuffer(int stackPos)
	{
		return null;
	}

	[Token(Token = "0x60004F1")]
	public T[] CheckNumberArray<T>(int stackPos) where T : struct
	{
		return null;
	}

	[Token(Token = "0x60004F2")]
	[Address(RVA = "0x6559B0", Offset = "0x6519B0", VA = "0x6559B0")]
	public object CheckObject(int stackPos, Type type)
	{
		return null;
	}

	[Token(Token = "0x60004F3")]
	[Address(RVA = "0x66DB3C", Offset = "0x669B3C", VA = "0x66DB3C")]
	public object CheckVarObject(int stackPos, Type type)
	{
		return null;
	}

	[Token(Token = "0x60004F4")]
	[Address(RVA = "0x6521D4", Offset = "0x64E1D4", VA = "0x6521D4")]
	public object[] CheckObjects(int oldTop)
	{
		return null;
	}

	[Token(Token = "0x60004F5")]
	[Address(RVA = "0x655AEC", Offset = "0x651AEC", VA = "0x655AEC")]
	public LuaFunction CheckLuaFunction(int stackPos)
	{
		return null;
	}

	[Token(Token = "0x60004F6")]
	[Address(RVA = "0x655C20", Offset = "0x651C20", VA = "0x655C20")]
	public LuaTable CheckLuaTable(int stackPos)
	{
		return null;
	}

	[Token(Token = "0x60004F7")]
	[Address(RVA = "0x655D54", Offset = "0x651D54", VA = "0x655D54")]
	public LuaThread CheckLuaThread(int stackPos)
	{
		return null;
	}

	[Token(Token = "0x60004F8")]
	public T CheckValue<T>(int stackPos)
	{
		return (T)null;
	}

	[Token(Token = "0x60004F9")]
	[Address(RVA = "0x655610", Offset = "0x651610", VA = "0x655610")]
	public object ToVariant(int stackPos)
	{
		return null;
	}

	[Token(Token = "0x60004FA")]
	[Address(RVA = "0x66DBB0", Offset = "0x669BB0", VA = "0x66DBB0")]
	public void CollectRef(int reference, string name, [Optional] bool isGCThread)
	{
	}

	[Token(Token = "0x60004FB")]
	[Address(RVA = "0x66E0E4", Offset = "0x66A0E4", VA = "0x66E0E4")]
	public void DelayDispose(LuaBaseRef br)
	{
	}

	[Token(Token = "0x60004FC")]
	[Address(RVA = "0x66E1B0", Offset = "0x66A1B0", VA = "0x66E1B0")]
	public int Collect()
	{
		return default(int);
	}

	[Token(Token = "0x60004FD")]
	[Address(RVA = "0x66E434", Offset = "0x66A434", VA = "0x66E434")]
	public void StepCollect()
	{
	}

	[Token(Token = "0x60004FE")]
	[Address(RVA = "0x66E450", Offset = "0x66A450", VA = "0x66E450")]
	public void RefreshDelegateMap()
	{
	}

	[Token(Token = "0x6000501")]
	[Address(RVA = "0x66EAD0", Offset = "0x66AAD0", VA = "0x66EAD0")]
	public void NewTable(string fullPath)
	{
	}

	[Token(Token = "0x6000502")]
	[Address(RVA = "0x66ECF0", Offset = "0x66ACF0", VA = "0x66ECF0")]
	public LuaTable NewTable([Optional] int narr, [Optional] int nrec)
	{
		return null;
	}

	[Token(Token = "0x6000503")]
	[Address(RVA = "0x66EDC0", Offset = "0x66ADC0", VA = "0x66EDC0")]
	public void ReLoad(string moduleFileName)
	{
	}

	[Token(Token = "0x6000504")]
	[Address(RVA = "0x66EFAC", Offset = "0x66AFAC", VA = "0x66EFAC")]
	public int GetMetaReference(Type t)
	{
		return default(int);
	}

	[Token(Token = "0x6000505")]
	[Address(RVA = "0x66F020", Offset = "0x66B020", VA = "0x66F020")]
	public int GetMissMetaReference(Type t)
	{
		return default(int);
	}

	[Token(Token = "0x6000506")]
	[Address(RVA = "0x66F20C", Offset = "0x66B20C", VA = "0x66F20C")]
	private Type GetBaseType(Type t)
	{
		return null;
	}

	[Token(Token = "0x6000507")]
	[Address(RVA = "0x66F260", Offset = "0x66B260", VA = "0x66F260")]
	private Type GetSpecialGenericType(Type t)
	{
		return null;
	}

	[Token(Token = "0x6000508")]
	[Address(RVA = "0x66F338", Offset = "0x66B338", VA = "0x66F338")]
	private void CloseBaseRef()
	{
	}

	[Token(Token = "0x6000509")]
	[Address(RVA = "0x66F3F4", Offset = "0x66B3F4", VA = "0x66F3F4", Slot = "4")]
	public void Dispose()
	{
	}

	[Token(Token = "0x600050A")]
	[Address(RVA = "0x66FB70", Offset = "0x66BB70", VA = "0x66FB70", Slot = "2")]
	public override int GetHashCode()
	{
		return default(int);
	}

	[Token(Token = "0x600050B")]
	[Address(RVA = "0x66FB78", Offset = "0x66BB78", VA = "0x66FB78", Slot = "0")]
	public override bool Equals(object o)
	{
		return default(bool);
	}

	[Token(Token = "0x600050C")]
	[Address(RVA = "0x651DDC", Offset = "0x64DDDC", VA = "0x651DDC")]
	public static bool operator ==(LuaState a, LuaState b)
	{
		return default(bool);
	}

	[Token(Token = "0x600050D")]
	[Address(RVA = "0x66A760", Offset = "0x666760", VA = "0x66A760")]
	public static bool operator !=(LuaState a, LuaState b)
	{
		return default(bool);
	}

	[Token(Token = "0x600050E")]
	[Address(RVA = "0x66FC60", Offset = "0x66BC60", VA = "0x66FC60")]
	public void PrintTable(string name)
	{
	}

	[Token(Token = "0x600050F")]
	[Address(RVA = "0x66DD5C", Offset = "0x669D5C", VA = "0x66DD5C")]
	protected void Collect(int reference, string name, bool beThread)
	{
	}

	[Token(Token = "0x6000510")]
	[Address(RVA = "0x66B61C", Offset = "0x66761C", VA = "0x66B61C")]
	protected void LuaLoadBuffer(byte[] buffer, string chunkName)
	{
	}

	[Token(Token = "0x6000511")]
	protected T LuaLoadBuffer<T>(byte[] buffer, string chunkName)
	{
		return (T)null;
	}

	[Token(Token = "0x6000512")]
	[Address(RVA = "0x67006C", Offset = "0x66C06C", VA = "0x67006C")]
	public bool BeginCall(string name, int top, bool beLogMiss)
	{
		return default(bool);
	}

	[Token(Token = "0x6000513")]
	[Address(RVA = "0x670174", Offset = "0x66C174", VA = "0x670174")]
	public void Call(int nArgs, int errfunc, int top)
	{
	}

	[Token(Token = "0x6000514")]
	[Address(RVA = "0x670278", Offset = "0x66C278", VA = "0x670278")]
	public void Call(string name, bool beLogMiss)
	{
	}

	[Token(Token = "0x6000515")]
	public void Call<T>(string name, T arg1, bool beLogMiss)
	{
	}

	[Token(Token = "0x6000516")]
	public void Call<T1, T2>(string name, T1 arg1, T2 arg2, bool beLogMiss)
	{
	}

	[Token(Token = "0x6000517")]
	public void Call<T1, T2, T3>(string name, T1 arg1, T2 arg2, T3 arg3, bool beLogMiss)
	{
	}

	[Token(Token = "0x6000518")]
	public void Call<T1, T2, T3, T4>(string name, T1 arg1, T2 arg2, T3 arg3, T4 arg4, bool beLogMiss)
	{
	}

	[Token(Token = "0x6000519")]
	public void Call<T1, T2, T3, T4, T5>(string name, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, bool beLogMiss)
	{
	}

	[Token(Token = "0x600051A")]
	public void Call<T1, T2, T3, T4, T5, T6>(string name, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, bool beLogMiss)
	{
	}

	[Token(Token = "0x600051B")]
	public R1 Invoke<R1>(string name, bool beLogMiss)
	{
		return (R1)null;
	}

	[Token(Token = "0x600051C")]
	public R1 Invoke<T1, R1>(string name, T1 arg1, bool beLogMiss)
	{
		return (R1)null;
	}

	[Token(Token = "0x600051D")]
	public R1 Invoke<T1, T2, R1>(string name, T1 arg1, T2 arg2, bool beLogMiss)
	{
		return (R1)null;
	}

	[Token(Token = "0x600051E")]
	public R1 Invoke<T1, T2, T3, R1>(string name, T1 arg1, T2 arg2, T3 arg3, bool beLogMiss)
	{
		return (R1)null;
	}

	[Token(Token = "0x600051F")]
	public R1 Invoke<T1, T2, T3, T4, R1>(string name, T1 arg1, T2 arg2, T3 arg3, T4 arg4, bool beLogMiss)
	{
		return (R1)null;
	}

	[Token(Token = "0x6000520")]
	public R1 Invoke<T1, T2, T3, T4, T5, R1>(string name, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, bool beLogMiss)
	{
		return (R1)null;
	}

	[Token(Token = "0x6000521")]
	public R1 Invoke<T1, T2, T3, T4, T5, T6, R1>(string name, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, bool beLogMiss)
	{
		return (R1)null;
	}

	[Token(Token = "0x6000522")]
	[Address(RVA = "0x661C84", Offset = "0x65DC84", VA = "0x661C84")]
	private void InitTypeTraits()
	{
	}

	[Token(Token = "0x6000523")]
	[Address(RVA = "0x664034", Offset = "0x660034", VA = "0x664034")]
	private void InitStackTraits()
	{
	}
}
