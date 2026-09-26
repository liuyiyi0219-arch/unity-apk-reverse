using System.Runtime.CompilerServices;
using Il2CppDummyDll;
using LuaInterface;
using UnityEngine;

[Token(Token = "0x200002E")]
public class LuaLooper : MonoBehaviour
{
	[Token(Token = "0x400003C")]
	[FieldOffset(Offset = "0x38")]
	public LuaState luaState;

	[Token(Token = "0x1700000E")]
	public LuaBeatEvent UpdateEvent
	{
		[Token(Token = "0x60001D0")]
		[Address(RVA = "0x646148", Offset = "0x642148", VA = "0x646148")]
		[CompilerGenerated]
		get
		{
			return null;
		}
		[Token(Token = "0x60001D1")]
		[Address(RVA = "0x646150", Offset = "0x642150", VA = "0x646150")]
		[CompilerGenerated]
		private set
		{
		}
	}

	[Token(Token = "0x1700000F")]
	public LuaBeatEvent LateUpdateEvent
	{
		[Token(Token = "0x60001D2")]
		[Address(RVA = "0x646158", Offset = "0x642158", VA = "0x646158")]
		[CompilerGenerated]
		get
		{
			return null;
		}
		[Token(Token = "0x60001D3")]
		[Address(RVA = "0x646160", Offset = "0x642160", VA = "0x646160")]
		[CompilerGenerated]
		private set
		{
		}
	}

	[Token(Token = "0x17000010")]
	public LuaBeatEvent FixedUpdateEvent
	{
		[Token(Token = "0x60001D4")]
		[Address(RVA = "0x646168", Offset = "0x642168", VA = "0x646168")]
		[CompilerGenerated]
		get
		{
			return null;
		}
		[Token(Token = "0x60001D5")]
		[Address(RVA = "0x646170", Offset = "0x642170", VA = "0x646170")]
		[CompilerGenerated]
		private set
		{
		}
	}

	[Token(Token = "0x60001D6")]
	[Address(RVA = "0x646178", Offset = "0x642178", VA = "0x646178")]
	private void Start()
	{
	}

	[Token(Token = "0x60001D7")]
	[Address(RVA = "0x6462E0", Offset = "0x6422E0", VA = "0x6462E0")]
	private LuaBeatEvent GetEvent(string name)
	{
		return null;
	}

	[Token(Token = "0x60001D8")]
	[Address(RVA = "0x6464BC", Offset = "0x6424BC", VA = "0x6464BC")]
	private void ThrowException()
	{
	}

	[Token(Token = "0x60001D9")]
	[Address(RVA = "0x646560", Offset = "0x642560", VA = "0x646560")]
	private void Update()
	{
	}

	[Token(Token = "0x60001DA")]
	[Address(RVA = "0x6465E0", Offset = "0x6425E0", VA = "0x6465E0")]
	private void LateUpdate()
	{
	}

	[Token(Token = "0x60001DB")]
	[Address(RVA = "0x646630", Offset = "0x642630", VA = "0x646630")]
	private void FixedUpdate()
	{
	}

	[Token(Token = "0x60001DC")]
	[Address(RVA = "0x644C50", Offset = "0x640C50", VA = "0x644C50")]
	public void Destroy()
	{
	}

	[Token(Token = "0x60001DD")]
	[Address(RVA = "0x6466D8", Offset = "0x6426D8", VA = "0x6466D8")]
	private void OnDestroy()
	{
	}

	[Token(Token = "0x60001DE")]
	[Address(RVA = "0x646750", Offset = "0x642750", VA = "0x646750")]
	public LuaLooper()
	{
	}
}
