using System.Collections;
using System.Collections.Generic;
using NTPackage.EventDispatcher;
using NTPackage.Functions;
using NTPackage.UI;
using Rubik.Format;
using Rubik.ItemPlayer;
using Rubik.Quest;
using Rubik.UserDataPlayer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Rubik.UserProfile
{

    public class AvatarChangeUI : PopupUI
    {
        
        public AvatarPlayerUI AvatarPlayerUI;
        public TextMeshProUGUI TextName;
        public TextMeshProUGUI TextLevel;
        public TextMeshProUGUI TextExp;
        public TextMeshProUGUI badgeTxt,badgeTitleTxt,badgeDesTXt;
        public NTButtonEffect BtnBuy;
        public ItemDataBarUI Price;
        public NTButtonEffect BtnEquip;
        public NTButtonEffect BtnLock;
        public TextMeshProUGUI ConditionText;

        public MultiTabUI MultiTabUI;


        public override void LoadComponents()
        {
            base.LoadComponents();
            this.popupCode = PopupCode.AvatarChangeUI;
        }

        public override void OnUI(object data = null, bool isDefaultSound = true)
        {
            base.OnUI(data, isDefaultSound);
            this.MultiTabUI.OnUI();
            this.AvatarPlayerUI.SetData(UserProfileManager.Instance.AvatarPlayer.Current, UserProfileManager.Instance.AvatarBorderPlayer.Current);
            this.UpdateData();
            EventListenerManager.instance.Register(EventCode.ChangeDisplayName, "AvatarChangeUI", (data)=>{
                this.UpdateData();
            });
        }

        public override void UpdateData(object data = null){
            base.UpdateData(data);
            this.TextName.text = UserDataManager.Instance.UserData.DisplayName;
            (long exp, long expNext, int level) = UserDataManager.Instance.GetPlayerLevel();
            this.TextLevel.text = (level+1).ToString();
            this.TextExp.text = $"{FormatData.GetFriendlyShortNumber(exp)}/{FormatData.GetFriendlyShortNumber(expNext)}";
            badgeTxt.text = "";
            badgeTitleTxt.text = "";
            badgeDesTXt.text = "";
            badgeTitleTxt.gameObject.SetActive(false);
            if (AchievementManager.Instance.AchievementBadgePlayer.Equip > -1)
            {
                badgeTxt.text = AchievementManager.Instance.GetAchievementName(((AchievementPlayerIndex)AchievementManager.Instance.AchievementBadgePlayer.Equip));
               
            }
        }
        public void SetBadge(AchievementPlayerIndex current) {
            badgeTitleTxt.gameObject.SetActive(true);
            badgeTitleTxt.GetComponentInChildren<Image>().sprite = AchievementManager.Instance.GetAchiementSprite(current);
            badgeTitleTxt.text =AchievementManager.Instance.GetAchievementName(current);
            badgeDesTXt.text = AchievementManager.Instance.GetAchievementDesc(current);
        }
        public void SetBadgeOff() {
            badgeTitleTxt.gameObject.SetActive(false);
            
            badgeTitleTxt.text="";
            badgeDesTXt.text ="";
        }

        public void OnclickChangeName(){
            PopupManager.Instance.OnUI(PopupCode.NameChangeUI);
        }
    }
}
