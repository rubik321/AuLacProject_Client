using System.Collections;
using System.Collections.Generic;
using NTPackage.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Rubik.UserProfile
{
    public class AvatarChangeItemUI : NTButtonEffect
    {
        public AvatarData AvatarData;

        public Image Icon;

        public AvatarTabUI AvatarTabUI;

        public Transform EquippedTransform;
        public Transform LockedTransform;

        public StatusType Status;

        public void SetData(AvatarData avatarData, AvatarTabUI avatarTabUI)
        {
            this.AvatarData = avatarData;
            this.AvatarTabUI = avatarTabUI;
            this.UpdateData();
        }

        public void UpdateData()
        {
            this.Icon.sprite = UserProfileManager.Instance.GetAvatarSprite(this.AvatarData.Index);
            if (UserProfileManager.Instance.AvatarPlayer.Current == this.AvatarData.Index){
                this.Status = StatusType.Equipped;
            }
            else{
                this.Status = StatusType.None;
                if (!UserProfileManager.Instance.IsAvatarAvailable(this.AvatarData.Index)) this.Status = StatusType.Locked;
            }

            this.EquippedTransform.gameObject.SetActive(false);
            this.LockedTransform.gameObject.SetActive(false);
            if(this.Status == StatusType.Equipped){
                this.EquippedTransform.gameObject.SetActive(true);
            } else if(this.Status == StatusType.Locked){
                this.LockedTransform.gameObject.SetActive(true);
            }
        }

        public void OnClick()
        {
            if (IsChose()) return;
           this.AvatarTabUI.SelectAvatar(this.AvatarData.Index);
        }
    }
}