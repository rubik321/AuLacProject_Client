using System.Collections;
using System.Collections.Generic;
using NTPackage.Functions;
using NTPackage.UI;
using Rubik.Account;
using Rubik.Banner;
using Rubik.Manager;
using Rubik.PlayerLand;
using Rubik.UserDataPlayer;
using Rubik.UserProfile;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rubik.Myrk.StartingUI
{
    public class StartingUI : PopupUI
    {
        public TextMeshProUGUI DisplayName;
        public Image LandUI;

        public override void OnUI(object data = null, bool isDefaultSound = true)
        {
            base.OnUI(data, false);
            if(!AccountManager.Instance.IsLinkAccount()){
                BannerManager.Instance.AddTopBanner(BannerTopConfig.WarrningGuest, Lean.Localization.LeanLocalization.GetTranslationText("link_account_warrning", "You are playing as a guest. Please link your account to avoid data loss. Go to Settings (top-right of the screen) and tap the Link button."));
            }
        }

        public override void ScriptOffUI()
        {
            base.ScriptOffUI();
            BannerManager.Instance.RemoveTopBanner(BannerTopConfig.WarrningGuest);
        }

        public override void UpdateData(object data = null)
        {
            base.UpdateData(data);
            this.LandUI.sprite = PlayerLandManager.Instance.GetLandSprite(PlayerLandManager.Instance.GetPlayerLandSelected().Index);
            this.DisplayName.text = UserDataManager.Instance.GetDisplayName();
        }

        public void OnclickStart(){
            StartCoroutine(ServerManager.Instance.IEJoinGame());
            this.OffUI();
        }

        public void OnclickSetting(){
            PopupManager.Instance.OnUI(PopupCode.SettingUI);
        }
    }
}