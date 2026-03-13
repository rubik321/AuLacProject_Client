using NTPackage.UI;
using Rubik.Quest;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rubik.UserProfile
{
    public class AchievementChangeItemUI : NTButtonEffect
    {
        public AchievementPlayer AvatarBorderData;

        public Image Icon;

        public AchievementTabUI achieTabUI;

        public Transform EquippedTransform;
        public Transform LockedTransform;

        public TextMeshProUGUI titleTxt;
        public StatusType Status;

        public void SetData(AchievementPlayer avatarBorderData, AchievementTabUI achieTabUI)
        {
            this.AvatarBorderData = avatarBorderData;
            this.achieTabUI = achieTabUI;
            this.UpdateData();
        }

        public void UpdateData()
        {
            this.Icon.sprite = AchievementManager.Instance.GetAchiementSprite(AvatarBorderData.Index);
            //Icon.SetNativeSize();
            Debug.Log(AchievementManager.Instance.AchievementBadgePlayer.Equip + "  -   " + (int)AvatarBorderData.Index);
            if (!AchievementManager.Instance.IsAchievementCompleted(AvatarBorderData.Index))
            {
                this.Status = StatusType.Locked;
            }
            else if(AchievementManager.Instance.AchievementBadgePlayer.Equip == (int)AvatarBorderData.Index)
            {
                this.Status = StatusType.Equipped;

            }
            else
            {
                this.Status = StatusType.None;
            }

            this.EquippedTransform.gameObject.SetActive(false);
            this.LockedTransform.gameObject.SetActive(false);
            if (this.Status == StatusType.Equipped)
            {
                this.EquippedTransform.gameObject.SetActive(true);
            }
            else if (this.Status == StatusType.Locked)
            {
                this.LockedTransform.gameObject.SetActive(true);
            }
            if (AchievementManager.Instance.AchievementBadgePlayer.Equip == (int)AvatarBorderData.Index)
            {
                OnClick();
            }
            titleTxt.text = AchievementManager.Instance.GetAchievementName(AvatarBorderData.Index); 
            // this.LockedTransform.gameObject.SetActive(true);
        }

        public void OnClick()
        {
          
            if (IsChose()) return;
            achieTabUI.AvatarChangeUI.SetBadge(this.AvatarBorderData.Index);
            this.achieTabUI.SelectFrame((int)this.AvatarBorderData.Index);
        }
    }

}
