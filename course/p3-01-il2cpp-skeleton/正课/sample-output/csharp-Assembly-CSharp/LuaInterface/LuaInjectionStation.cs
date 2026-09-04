using Il2CppDummyDll;

namespace LuaInterface;

[Token(Token = "0x2000079")]
public class LuaInjectionStation
{
	[Token(Token = "0x400012D")]
	public const byte NOT_INJECTION_FLAG = 0;

	[Token(Token = "0x400012E")]
	public const byte INVALID_INJECTION_FLAG = byte.MaxValue;

	[Token(Token = "0x400012F")]
	[FieldOffset(Offset = "0x0")]
	private static int cacheSize;

	[Token(Token = "0x4000130")]
	[FieldOffset(Offset = "0x8")]
	private static byte[] injectionFlagCache;

	[Token(Token = "0x4000131")]
	[FieldOffset(Offset = "0x10")]
	private static LuaFunction[] injectFunctionCache;

	[Token(Token = "0x600070B")]
	[Address(RVA = "0x685FE4", Offset = "0x681FE4", VA = "0x685FE4")]
	static LuaInjectionStation()
	{
	}

	[Token(Token = "0x600070C")]
	[Address(RVA = "0x6860A4", Offset = "0x6820A4", VA = "0x6860A4")]
	[NoToLua]
	public static byte GetInjectFlag(int index)
	{
		return default(byte);
	}

	[Token(Token = "0x600070D")]
	[Address(RVA = "0x6861A8", Offset = "0x6821A8", VA = "0x6861A8")]
	[NoToLua]
	public static LuaFunction GetInjectionFunction(int index)
	{
		return null;
	}

	[Token(Token = "0x600070E")]
	[Address(RVA = "0x686224", Offset = "0x682224", VA = "0x686224")]
	public static void CacheInjectFunction(int index, byte injectFlag, LuaFunction func)
	{
	}

	[Token(Token = "0x600070F")]
	[Address(RVA = "0x68632C", Offset = "0x68232C", VA = "0x68632C")]
	public static void Clear()
	{
	}

	[Token(Token = "0x6000710")]
	[Address(RVA = "0x686428", Offset = "0x682428", VA = "0x686428")]
	public LuaInjectionStation()
	{
	}
}
