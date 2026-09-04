using Il2CppDummyDll;
using UnityEngine;

[Token(Token = "0x2000005")]
[CreateAssetMenu(fileName = "GameConfig", menuName = "Course/GameConfig")]
public class GameConfig : ScriptableObject
{
	[Token(Token = "0x4000007")]
	[FieldOffset(Offset = "0x18")]
	public int maxLevel;

	[Token(Token = "0x4000008")]
	[FieldOffset(Offset = "0x1C")]
	public int baseAtk;

	[Token(Token = "0x4000009")]
	[FieldOffset(Offset = "0x20")]
	public int baseDef;

	[Token(Token = "0x400000A")]
	[FieldOffset(Offset = "0x24")]
	public float critMultiplier;

	[Token(Token = "0x400000B")]
	[FieldOffset(Offset = "0x28")]
	public string welcomeText;

	[Token(Token = "0x600000E")]
	[Address(RVA = "0x6016F8", Offset = "0x5FD6F8", VA = "0x6016F8")]
	public GameConfig()
	{
	}
}
