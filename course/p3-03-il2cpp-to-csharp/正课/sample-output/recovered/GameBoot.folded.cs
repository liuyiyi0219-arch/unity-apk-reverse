// ============================================================
// 逆向还原产物（3.3）—— 状态机/闭包折回原语法
// 演示“模式识别”这一步：把编译器生成的隐藏类折叠回 yield / lambda。
// 别把 <QuitAfterDelay>d__2 / <>c__DisplayClass3_0 当真类输出——它们是编译产物。
// ============================================================
using System.Collections;
using UnityEngine;

public partial class GameBoot : MonoBehaviour
{
    // ---- yield 折回 ----
    // 依据 <QuitAfterDelay>d__2.MoveNext（RVA 0x6012B8）的 switch(<>1__state)：
    //   state==0 分支: current = new WaitForSeconds(seconds); state=1; return true;
    //                  →  yield return new WaitForSeconds(seconds);
    //   state==1 分支: state=-1; 跑收尾逻辑（Application.Quit + killProcess）; return false;
    //                  →  yield 之后那段直到方法结束
    private IEnumerator QuitAfterDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);   // ← state 0→1
        Application.Quit();                          // ← state==1 恢复点起的收尾
        // 进程自杀（原文是 using(AndroidJavaClass "android.os.Process") + killProcess(myPid)）
    }

    // ---- lambda 折回 ----
    // <>c__DisplayClass3_0 捕获了 text / activity 两个局部；<ShowToast>b__0（RVA 0x600F58）是 lambda 体。
    // 把 b__0 内联回 runOnUiThread 的实参位置，DisplayClass 这个类不输出。
    private static void ShowToast(string text)
    {
        var player = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        var activity = player.GetStatic<AndroidJavaObject>("currentActivity");
        activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
        {
            // <ShowToast>b__0 的方法体：makeText(activity, text, 1).show()
            using (var toastClass = new AndroidJavaClass("android.widget.Toast"))
            {
                var toast = toastClass.CallStatic<AndroidJavaObject>("makeText", activity, text, 1);
                toast.Call("show");
            }
        }));
    }
}
