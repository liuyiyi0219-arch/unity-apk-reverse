using Il2CppDummyDll;

[Token(Token = "0x2000008")]
public static class SignatureCheck
{
	[Token(Token = "0x400000D")]
	public const string ExpectedSha1 = "E2462490F7ED575E6F12A6AF2430B4CFEE370306";

	[Token(Token = "0x6000012")]
	[Address(RVA = "0x600100", Offset = "0x5FC100", VA = "0x600100")]
	public static string GetSignatureSha1()
	{
		return null;
	}

	[Token(Token = "0x6000013")]
	[Address(RVA = "0x600A94", Offset = "0x5FCA94", VA = "0x600A94")]
	public static bool IsUntampered()
	{
		return default(bool);
	}
}
