using UnityEngine;
using AppsFlyerSDK;

public class AppsFlyerInit : MonoBehaviour, IAppsFlyerConversionData
{
    // Thay bằng DevKey và AppID của bạn
    private string devKey = "djCX3JGnpqx4GP8UFPbTb7";
#if UNITY_IOS
    private string appID = "YOUR_IOS_APP_ID";
#endif

    void Start()
    {
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
}
