using Il2CppDummyDll;

namespace LuaInterface;

[Token(Token = "0x200004E")]
public class GCRef
{
	[Token(Token = "0x40000A4")]
	[FieldOffset(Offset = "0x10")]
	public int reference;

	[Token(Token = "0x40000A5")]
	[FieldOffset(Offset = "0x18")]
	public string name;

	[Token(Token = "0x60003B0")]
	[Address(RVA = "0x658F50", Offset = "0x654F50", VA = "0x658F50")]
	public GCRef(int reference, string name)
	{
	}
}
