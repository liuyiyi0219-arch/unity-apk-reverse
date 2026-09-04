using Il2CppDummyDll;
using LuaInterface;
using UnityEngine;

[Token(Token = "0x200000C")]
public class LuaManager : MonoBehaviour
{
	[Token(Token = "0x4000014")]
	[FieldOffset(Offset = "0x20")]
	private LuaState lua;

	[Token(Token = "0x4000015")]
	[FieldOffset(Offset = "0x28")]
	private LuaLooper loop;

	[Token(Token = "0x600001A")]
	[Address(RVA = "0x601E08", Offset = "0x5FDE08", VA = "0x601E08")]
	private void Awake()
	{
	}

	[Token(Token = "0x600001B")]
	[Address(RVA = "0x601F24", Offset = "0x5FDF24", VA = "0x601F24")]
	private void OnDestroy()
	{
	}

	[Token(Token = "0x600001C")]
	[Address(RVA = "0x602024", Offset = "0x5FE024", VA = "0x602024")]
	public LuaManager()
	{
	}
}
