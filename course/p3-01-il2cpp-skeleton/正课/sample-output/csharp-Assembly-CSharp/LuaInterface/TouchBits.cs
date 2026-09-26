using Il2CppDummyDll;

namespace LuaInterface;

[Token(Token = "0x2000055")]
public class TouchBits
{
	[Token(Token = "0x40000AB")]
	public const int DeltaPosition = 1;

	[Token(Token = "0x40000AC")]
	public const int Position = 2;

	[Token(Token = "0x40000AD")]
	public const int RawPosition = 4;

	[Token(Token = "0x40000AE")]
	public const int ALL = 7;

	[Token(Token = "0x60003CB")]
	[Address(RVA = "0x65A994", Offset = "0x656994", VA = "0x65A994")]
	public TouchBits()
	{
	}
}
