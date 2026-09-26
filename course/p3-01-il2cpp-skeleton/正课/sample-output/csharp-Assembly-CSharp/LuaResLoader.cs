using Il2CppDummyDll;
using LuaInterface;

[Token(Token = "0x2000030")]
public class LuaResLoader : LuaFileUtils
{
	[Token(Token = "0x60001E4")]
	[Address(RVA = "0x646958", Offset = "0x642958", VA = "0x646958")]
	public LuaResLoader()
	{
	}

	[Token(Token = "0x60001E5")]
	[Address(RVA = "0x6469C4", Offset = "0x6429C4", VA = "0x6469C4", Slot = "5")]
	public override byte[] ReadFile(string fileName)
	{
		return null;
	}

	[Token(Token = "0x60001E6")]
	[Address(RVA = "0x646CDC", Offset = "0x642CDC", VA = "0x646CDC", Slot = "6")]
	public override string FindFileError(string fileName)
	{
		return null;
	}

	[Token(Token = "0x60001E7")]
	[Address(RVA = "0x646B30", Offset = "0x642B30", VA = "0x646B30")]
	private byte[] ReadResourceFile(string fileName)
	{
		return null;
	}

	[Token(Token = "0x60001E8")]
	[Address(RVA = "0x646A0C", Offset = "0x642A0C", VA = "0x646A0C")]
	private byte[] ReadDownLoadFile(string fileName)
	{
		return null;
	}
}
