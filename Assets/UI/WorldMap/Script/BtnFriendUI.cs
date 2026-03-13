using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTFunctions_old;

namespace GOA.UIWorldMap{
    public class BtnFriendUI : BaseButton
    {
        protected override void OnClick()
        {
            base.OnClick();
            try
            {
                GOA.UIFriends.FriendUI friendUI = (GOA.UIFriends.FriendUI) UIManager.instance.GetPopupUIByCode(PopupCode.FriendUI);
                if(friendUI == null) return;
                friendUI.OnUI();
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e);
            }
        }

        protected override void OnSound()
        {
            Rubik.Common.AudioHelper.AudioCtrl.Instance.Play(Rubik.Common.AudioHelper.AudioName.UI_Button_Worldmap);
        }
    }
}
