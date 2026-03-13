using System.Collections;
using System.Collections.Generic;
using NTPackage.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Rubik.UserProfile
{
    public class FrameChangeItemUI : NTButtonEffect
    {
        public AvatarBorderData AvatarBorderData;

        public Image Icon;

        public FrameTabUI FrameTabUI;

        public Transform EquippedTransform;
        public Transform LockedTransform;

        public StatusType Status;

        public void SetData(AvatarBorderData avatarBorderData, FrameTabUI frameTabUI)
        {
            this.AvatarBorderData = avatarBorderData;
            this.FrameTabUI = frameTabUI;
            this.UpdateData();
        }

        public void UpdateData()
        {
            this.Icon.sprite = UserProfileManager.Instance.GetAvatarBorderSprite(this.AvatarBorderData.Index);
            if (this.FrameTabUI.IndexSelectedFrame == this.AvatarBorderData.Index)
            {
                this.Status = StatusType.Equipped;
            }
            else
            {
                this.Status = StatusType.None;
                if (!UserProfileManager.Instance.IsAvatarBorderAvailable(this.AvatarBorderData.Index)) this.Status = StatusType.Locked;
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
            this.FrameTabUI.SelectFrame(this.AvatarBorderData.Index);
        }
    }
}