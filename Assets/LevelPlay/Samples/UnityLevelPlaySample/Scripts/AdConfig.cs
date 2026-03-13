public static class AdConfig
{
    public static string AppKey => GetAppKey();
    public static string BannerAdUnitId => GetBannerAdUnitId();
    public static string InterstitalAdUnitId => GetInterstitialAdUnitId();
    public static string RewardedVideoAdUnitId => GetRewardedVideoAdUnitId();

    static string GetAppKey()
    {
        #if UNITY_ANDROID
            return "2332538e5";
#elif UNITY_IPHONE
            return "23b7dbe7d";
#else
            return "85460dcd";
#endif
    }

    static string GetBannerAdUnitId()
    {
        #if UNITY_ANDROID
            return "thnfvcsog13bhn08";
        #elif UNITY_IPHONE
            return "iep3rxsyp9na3rw8";
        #else
            return "unexpected_platform";
        #endif
    }
    static string GetInterstitialAdUnitId()
    {
        #if UNITY_ANDROID
            return "aeyqi3vqlv6o8sh9";
        #elif UNITY_IPHONE
            return "wmgt0712uuux8ju4";
        #else
            return "unexpected_platform";
        #endif
    }

    static string GetRewardedVideoAdUnitId()
    {
        #if UNITY_ANDROID
            return "3aesd4g232smum86";
#elif UNITY_IPHONE
            return "1t90o27jn9nvswmz";
#else
            return "76yy3nay3ceui2a3";
#endif
    }
}
