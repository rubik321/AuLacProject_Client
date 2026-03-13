using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NTFunctions_old
{
    public class PopupLv
    {
        public float GetValue(string name){
            try
            {
                return (float) this.GetType().GetField(name).GetValue(this);
            }
            catch (System.Exception)
            {
                return 0;
            }
            
        }
        //Popup
        public static float ViewInforMobUI = 1;
        public static float MenuUI = 1;
        public static float FriendUI = 1;
        public static float ShopUI = 1;
        public static float OutpostUI = 1;
        public static float SystemUI = 1;
        public static float PlayerShopUI = 1;
        public static float NeutralShopUI = 1;
        public static float WanderingDealerUI = 1;
        public static float LandEventUI = 1;
        public static float ProfileUI = 1;
        public static float BlacksmithUI = 1;

        public static float ClassesUI = 2;
        public static float DailyIncomeUI = 2;
        public static float ChatUI = 2;
        public static float PortalUnlookUI = 2;
        public static float PortalTeleportUI = 2;
        public static float LandEventPopupUI = 2;
        public static float RankLandEventUI = 2;
        public static float Blacksmith_UpgradeUI = 2;
        public static float Blacksmith_UpStarUI = 2;
        public static float Blacksmith_FusionUI = 2;
        public static float Blacksmith_FragmentUI = 2;

        public static float AddFriendUI = 3;
        public static float InforBuildingUI = 3;
        public static float InforItemInAppUI = 3;
        public static float InforItemShopUI = 3;
        public static float NoticUseKeyOpenPortalUI = 3;

        public static float PortalLookUI = 4;
        public static float PopupRewardUI = 4;
        public static float PopupBuffPlayerAdminUI = 4;
        public static float LandEventDescUI = 4;


        //Notic
        public static float FriendRequestUI = 5;
        public static float NoticPrivacyPolicyUI = 5;
        public static float NoticLogoutUI = 5;
        public static float NoticQuitGameUI = 5;
        public static float NoticAddFriendUI = 5;
        public static float NoticFriendDeniedUI = 5;
        public static float NoticPortalKeyUI = 5;
    }
}
