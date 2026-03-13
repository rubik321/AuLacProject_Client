using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class Utils
{
    /// <summary>
    /// Use to delay invoke method from Scriptable Objects
    /// <para> <paramref name="action"/>: Method to invoke </para>
    /// <para> <paramref name="delay"/>: Method is invoked after delay, by seconds </para>
    /// <para> <paramref name="followTimeScale"/>: Should delay time followtimeScale </para>
    /// <para> <paramref name="isCrossScene"/>: If current scene changed and method isn't invoke yet, stop invoking </para>
    /// </summary>
    public static void DelayInvoke(this MonoBehaviour source, UnityAction action, float delay, bool followTimeScale = true)
    {
        source.StartCoroutine(DelayInvokeCoroutine(action, delay, followTimeScale));
    }

    private static IEnumerator DelayInvokeCoroutine(UnityAction action, float delay, bool followTimeScale = true)
    {
        if (followTimeScale)
        {
            yield return new WaitForSeconds(delay);
        }
        else
        {
            yield return new WaitForSecondsRealtime(delay);
        }
        action?.Invoke();
    }
}
