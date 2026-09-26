using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;

[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: TargetFramework(".NETStandard,Version=v2.1", FrameworkDisplayName = ".NET Standard 2.1")]
[assembly: AssemblyCompany("HotUpdate")]
[assembly: AssemblyConfiguration("Release")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: AssemblyInformationalVersion("1.0.0+ebbb6efc0e723674cc736c6cd2f51e212a3cfd26")]
[assembly: AssemblyProduct("HotUpdate")]
[assembly: AssemblyTitle("HotUpdate")]
[assembly: AssemblyVersion("1.0.0.0")]
namespace Course.Hotfix;

public static class HotfixLogic
{
	public static int CalcDamageV2(int atk, int def, float crit)
	{
		float num = (float)def / ((float)def + 100f);
		return (int)((float)atk * (1f - num) * crit);
	}

	public static int DailyReward(int day)
	{
		int num = 100 + day * 50;
		if (day % 7 != 0)
		{
			return num;
		}
		return num * 2;
	}
}
