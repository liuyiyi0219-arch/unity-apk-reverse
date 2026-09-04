using System.Collections.Generic;
using System.Runtime.InteropServices;
using Il2CppDummyDll;
using UnityEngine;

namespace LuaInterface;

[Token(Token = "0x2000049")]
public class LuaFileUtils
{
	[Token(Token = "0x4000098")]
	[Il2CppDummyDll.FieldOffset(Offset = "0x10")]
	public bool beZip;

	[Token(Token = "0x4000099")]
	[Il2CppDummyDll.FieldOffset(Offset = "0x18")]
	protected List<string> searchPaths;

	[Token(Token = "0x400009A")]
	[Il2CppDummyDll.FieldOffset(Offset = "0x20")]
	protected Dictionary<string, AssetBundle> zipMap;

	[Token(Token = "0x400009B")]
	[Il2CppDummyDll.FieldOffset(Offset = "0x0")]
	protected static LuaFileUtils instance;

	[Token(Token = "0x17000017")]
	public static LuaFileUtils Instance
	{
		[Token(Token = "0x6000309")]
		[Address(RVA = "0x650AE8", Offset = "0x64CAE8", VA = "0x650AE8")]
		get
		{
			return null;
		}
		[Token(Token = "0x600030A")]
		[Address(RVA = "0x650C78", Offset = "0x64CC78", VA = "0x650C78")]
		protected set
		{
		}
	}

	[Token(Token = "0x600030B")]
	[Address(RVA = "0x650B6C", Offset = "0x64CB6C", VA = "0x650B6C")]
	public LuaFileUtils()
	{
	}

	[Token(Token = "0x600030C")]
	[Address(RVA = "0x650CD0", Offset = "0x64CCD0", VA = "0x650CD0", Slot = "4")]
	public virtual void Dispose()
	{
	}

	[Token(Token = "0x600030D")]
	[Address(RVA = "0x650EB0", Offset = "0x64CEB0", VA = "0x650EB0")]
	public bool AddSearchPath(string path, [Optional] bool front)
	{
		return default(bool);
	}

	[Token(Token = "0x600030E")]
	[Address(RVA = "0x650FC8", Offset = "0x64CFC8", VA = "0x650FC8")]
	public bool RemoveSearchPath(string path)
	{
		return default(bool);
	}

	[Token(Token = "0x600030F")]
	[Address(RVA = "0x65105C", Offset = "0x64D05C", VA = "0x65105C")]
	public void AddSearchBundle(string name, AssetBundle bundle)
	{
	}

	[Token(Token = "0x6000310")]
	[Address(RVA = "0x6510C4", Offset = "0x64D0C4", VA = "0x6510C4")]
	public string FindFile(string fileName)
	{
		return null;
	}

	[Token(Token = "0x6000311")]
	[Address(RVA = "0x651284", Offset = "0x64D284", VA = "0x651284", Slot = "5")]
	public virtual byte[] ReadFile(string fileName)
	{
		return null;
	}

	[Token(Token = "0x6000312")]
	[Address(RVA = "0x6516C4", Offset = "0x64D6C4", VA = "0x6516C4", Slot = "6")]
	public virtual string FindFileError(string fileName)
	{
		return null;
	}

	[Token(Token = "0x6000313")]
	[Address(RVA = "0x6512D8", Offset = "0x64D2D8", VA = "0x6512D8")]
	private byte[] ReadZipFile(string fileName)
	{
		return null;
	}

	[Token(Token = "0x6000314")]
	[Address(RVA = "0x651BC4", Offset = "0x64DBC4", VA = "0x651BC4")]
	public static string GetOSDir()
	{
		return null;
	}
}
