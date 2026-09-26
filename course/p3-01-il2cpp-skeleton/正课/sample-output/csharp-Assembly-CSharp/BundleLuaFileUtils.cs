using System.Collections.Generic;
using Il2CppDummyDll;
using LuaInterface;
using UnityEngine;

[Token(Token = "0x200000A")]
public class BundleLuaFileUtils : LuaFileUtils
{
	[Token(Token = "0x400000E")]
	[FieldOffset(Offset = "0x28")]
	private readonly AssetBundle bundle;

	[Token(Token = "0x400000F")]
	[FieldOffset(Offset = "0x30")]
	private readonly Dictionary<string, string> srcMap;

	[Token(Token = "0x4000010")]
	[FieldOffset(Offset = "0x38")]
	private readonly Dictionary<string, string> configMap;

	[Token(Token = "0x6000016")]
	[Address(RVA = "0x6017D8", Offset = "0x5FD7D8", VA = "0x6017D8")]
	public BundleLuaFileUtils(AssetBundle bundle)
	{
	}

	[Token(Token = "0x6000017")]
	[Address(RVA = "0x6019E0", Offset = "0x5FD9E0", VA = "0x6019E0")]
	private static string StripBytes(string s)
	{
		return null;
	}

	[Token(Token = "0x6000018")]
	[Address(RVA = "0x601AC0", Offset = "0x5FDAC0", VA = "0x601AC0", Slot = "5")]
	public override byte[] ReadFile(string fileName)
	{
		return null;
	}

	[Token(Token = "0x6000019")]
	[Address(RVA = "0x601C44", Offset = "0x5FDC44", VA = "0x601C44")]
	public static BundleLuaFileUtils Install()
	{
		return null;
	}
}
