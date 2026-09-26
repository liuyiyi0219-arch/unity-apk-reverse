using Il2CppDummyDll;

namespace LuaInterface;

[Token(Token = "0x2000056")]
public class RaycastBits
{
	[Token(Token = "0x40000AF")]
	public const int Collider = 1;

	[Token(Token = "0x40000B0")]
	public const int Normal = 2;

	[Token(Token = "0x40000B1")]
	public const int Point = 4;

	[Token(Token = "0x40000B2")]
	public const int Rigidbody = 8;

	[Token(Token = "0x40000B3")]
	public const int Transform = 16;

	[Token(Token = "0x40000B4")]
	public const int ALL = 31;

	[Token(Token = "0x60003CC")]
	[Address(RVA = "0x65A99C", Offset = "0x65699C", VA = "0x65A99C")]
	public RaycastBits()
	{
	}
}
