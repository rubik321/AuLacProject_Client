using AppsFlyerSDK;
using System.Collections.Generic;
using UnityEngine;
public enum AppsflyerEvents
{
    player_level,
    money_spent,
    evolvel_echo,
    sesion_length,
    total_play_time,
    level_player_quit,
    player_join_clan,
    donate_clan,
    attack_clan_boss,
    attack_astral_gate,
    capture_fortress,
    player_start_tutorial,
    player_finish_tutorial,
    tutorial_players_stuck,
    arena_battle_start,
    arena_battle_end,
    purchase_data,
    gems_spent,
    ads_watched,
    daily_login,
    equipment_upgraded,
    android_install,
    ios_install

}
public class AppsFlyerManager : MonoBehaviour
{
    private string devKey = "djCX3JGnpqx4GP8UFPbTb7";
#if UNITY_IOS
    private string appID = "YOUR_IOS_APP_ID";
#endif

    void Start()
    {
        AppsFlyer.setIsDebug(true);
#if UNITY_IOS
        AppsFlyer.initSDK(devKey, appID, this);
#elif UNITY_ANDROID
        AppsFlyer.initSDK(devKey, null, this);
#endif
        AppsFlyer.startSDK();
    }

    // Callback khi có dữ liệu attribution
    public void onConversionDataSuccess(string conversionData)
    {
        Debug.Log("Attribution data: " + conversionData);
    }

    public void onConversionDataFail(string error)
    {
        Debug.Log("Attribution error: " + error);
    }

    public void onAppOpenAttribution(string attributionData)
    {
        Debug.Log("App opened via attribution link: " + attributionData);
    }

    public void onAppOpenAttributionFailure(string error)
    {
        Debug.Log("Attribution open fail: " + error);
    }
    public static void SendEvent(string eventName, Dictionary<string, string> eventValues = null)
    {
        if (eventValues == null)
            eventValues = new Dictionary<string, string>();

        Debug.Log($"[AppsFlyer] Sending event: {eventName}");
        AppsFlyer.sendEvent(eventName, eventValues);
    }
    public static void TrackingAds(string _place)
    {
        var eventValues = new Dictionary<string, string>()
            {
                { "Watch_reward_ads","1" },
                { "Watch_reward_place",_place }
            }
         ;

        SendEvent(AppsflyerEvents.ads_watched.ToString(), eventValues);
    }
    public static void TrackingEvent(AppsflyerEvents eventName, int count,int total)
    {
        var eventValues = new Dictionary<string, string>()
            {
                { "Count",count.ToString() },
                { "total",total.ToString() }
            }
         ;

        SendEvent(eventName.ToString(), eventValues);
    }
    public static void TrackingEvent(AppsflyerEvents eventName, int number)
    {
        var eventValues = new Dictionary<string, string>()
            {
                { "number",number.ToString() },
               
            }
         ;

        SendEvent(eventName.ToString(), eventValues);
    }
    public static void TrackingEvent(AppsflyerEvents eventName,string parName ,int number)
    {
        var eventValues = new Dictionary<string, string>()
            {
                { parName,number.ToString() },

            }
         ;

        SendEvent(eventName.ToString(), eventValues);
    }
    public static void TrackingArenaEnd( string result)
    {
        var eventValues = new Dictionary<string, string>()
            {
                { "result",result },
          
            }
         ;

        SendEvent(AppsflyerEvents.arena_battle_end.ToString(), eventValues);
    }
    public static void TrackingPurchases(string id,string platform)
    {
        var eventValues = new Dictionary<string, string>()
            {
                { "platform",platform },
                { "id",id },
            }

         ;

        SendEvent(AppsflyerEvents.purchase_data.ToString(), eventValues);
    }
    public static void TrackingLevel(int level)
    {
        var eventValues = new Dictionary<string, string>()
            {
               
                { "Level",level.ToString() }
            }
         ;

        SendEvent(AppsflyerEvents.player_level.ToString(), eventValues);
    }
  
}
