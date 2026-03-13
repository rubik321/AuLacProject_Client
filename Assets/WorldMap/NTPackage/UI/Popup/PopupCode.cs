using System;
using UnityEngine;

namespace NTFunctions_old
{
    public class PopupCodeParser
    {
        public static PopupCode FromString(string name)
        {
            try
            {
                return (PopupCode)Enum.Parse(typeof(PopupCode), name);
            }
            catch (System.Exception)
            {
               return PopupCode.Unknown;
            }
            //name = name.ToLower();
        }
    }

    public enum PopupCode
    {
        Unknown = 0,

        ViewInforMobUI = 1,
        ChatUI = 2,
        AddFriendUI = 3,
        FriendRequestUI = 4,
        ProfileUI = 5,
        MenuUI = 6,
        NoticPrivacyPolicyUI = 7,
        NoticLogoutUI = 8,
        NoticQuitGameUI = 9,
        SystemUI = 10,
        ClassesUI = 11,
        FriendUI = 12,
        NoticAddFriendUI = 13,
        NoticFriendDeniedUI = 14,
        DailyIncomeUI = 15,
        ShopUI = 16,
        InforItemInAppUI = 17,
        InforItemShopUI = 18,
        PortalLookUI = 19,
        NoticPortalKeyUI = 20,
        OutpostUI = 21,
        PortalUnlookUI = 22,
        PortalTeleportUI = 23,
        NoticUseKeyOpenPortalUI = 24,
        DailyRewardUI = 25,
        PopupRewardUI = 26,
        PopupBuffPlayerAdminUI = 27,
        LevelUpUI = 28,
        NeutralShopUI = 29,
        WanderingDealerUI = 30,
        InforBuildingUI = 31,
        PlayerShopUI = 32,
        LandEventUI = 33,
        LandEventPopupUI = 34,
        RankLandEventUI = 35,
        LandEventDescUI = 36,
        BlacksmithUI,
        Blacksmith_UpgradeUI,
        Blacksmith_UpStarUI,
        Blacksmith_FusionUI,
        Blacksmith_FragmentUI,
    }
}
