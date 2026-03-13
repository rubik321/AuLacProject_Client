using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTFunctions_old;
using TMPro;
using GOA.UserData;

namespace GOA.UIMenu{
    using UserData;

    public class AccountTabUI : TabUI
    {
        public TextMeshProUGUI textUserName;
        public TextMeshProUGUI textEmail;

        public override void UpdateData()
        {
            base.UpdateData();
            try
            {
                this.textUserName.text = UserData.Instance.data.UserName;   
            }
            catch (System.Exception)
            {
                
            }
        }

        public void OnNoticPrivacyPolicy(){
            NoticPrivacyPolicyUI noticPrivacyPolicyUI = (NoticPrivacyPolicyUI) UIManager.instance.GetPopupUIByCode(PopupCode.NoticPrivacyPolicyUI);
            if(noticPrivacyPolicyUI == null) return;
            noticPrivacyPolicyUI.OnUI();
        }
        public void OnNoticQuitGame(){
            NoticQuitGameUI noticQuitGameUI = (NoticQuitGameUI) UIManager.instance.GetPopupUIByCode(PopupCode.NoticQuitGameUI);
            if(noticQuitGameUI == null) return;
            noticQuitGameUI.OnUI();
        }
        public void OnNoticLogout(){
            NoticLogoutUI noticLogoutUI = (NoticLogoutUI) UIManager.instance.GetPopupUIByCode(PopupCode.NoticLogoutUI);
            if(noticLogoutUI == null) return;
            noticLogoutUI.OnUI();
        }
    }
}
