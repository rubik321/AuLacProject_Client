using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTFunctions_old;
using GOA.UserData;

namespace GOA.UIWorldMap{
    using UserData;
    
    public class BtnAccountUI : BaseButton
    {
        protected override void OnClick()
        {
            base.OnClick();
            try
            {
                GOA.UIProfile.ProfileUI profileUI = (GOA.UIProfile.ProfileUI) UIManager.instance.GetPopupUIByCode(PopupCode.ProfileUI);
                profileUI.OnUI(UserData.Instance.data);
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
