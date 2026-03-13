using System.Collections;
using System.Collections.Generic;
using Lean.Localization;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    float sessionTime = 0;
    void Awake()
    {

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        DontDestroyOnLoad(this.gameObject);
        sessionTime = Time.time;
          //  long.Parse(PlayerPrefs.GetString());
    }
    private void OnApplicationQuit()
    {
        var totalTime = Time.time- sessionTime;
        var totalPlayTime = float.Parse(PlayerPrefs.GetString("total_play_time","0"))+ totalTime;
        PlayerPrefs.SetString("total_play_time", totalPlayTime.ToString());
        AppsFlyerManager.TrackingEvent(AppsflyerEvents.sesion_length, "sesion_time", (int)totalTime);
        AppsFlyerManager.TrackingEvent(AppsflyerEvents.total_play_time, "total_play_time", (int)totalPlayTime);

    }
}
