using Il2CppDummyDll;

namespace LuaInterface;

[Token(Token = "0x2000068")]
public struct LuaValueType
{
	[Token(Token = "0x40000F3")]
	public const int None = 0;

	[Token(Token = "0x40000F4")]
	public const int Vector3 = 1;

	[Token(Token = "0x40000F5")]
	public const int Quaternion = 2;

	[Token(Token = "0x40000F6")]
	public const int Vector2 = 3;

	[Token(Token = "0x40000F7")]
	public const int Color = 4;

	[Token(Token = "0x40000F8")]
	public const int Vector4 = 5;

	[Token(Token = "0x40000F9")]
	public const int Ray = 6;

	[Token(Token = "0x40000FA")]
	public const int Bounds = 7;

	[Token(Token = "0x40000FB")]
	public const int Touch = 8;

	[Token(Token = "0x40000FC")]
	public const int LayerMask = 9;

	[Token(Token = "0x40000FD")]
	public const int RaycastHit = 10;

	[Token(Token = "0x40000FE")]
	public const int Int64 = 11;

	[Token(Token = "0x40000FF")]
	public const int UInt64 = 12;

	[Token(Token = "0x4000100")]
	public const int Max = 64;

	[Token(Token = "0x4000101")]
	[FieldOffset(Offset = "0x0")]
	private int type;

	[Token(Token = "0x600061B")]
	[Address(RVA = "0x676994", Offset = "0x672994", VA = "0x676994")]
	public LuaValueType(int value)
	{
	}

	[Token(Token = "0x600061C")]
	[Address(RVA = "0x67699C", Offset = "0x67299C", VA = "0x67699C")]
	public static implicit operator int(LuaValueType mask)
	{
		return default(int);
	}

	[Token(Token = "0x600061D")]
	[Address(RVA = "0x6769A0", Offset = "0x6729A0", VA = "0x6769A0")]
	public static implicit operator LuaValueType(int intVal)
	{
		return default(LuaValueType);
	}

	[Token(Token = "0x600061E")]
	[Address(RVA = "0x6769A8", Offset = "0x6729A8", VA = "0x6769A8", Slot = "3")]
	public override string ToString()
	{
		return null;
	}
}
