using System.Collections;
using System.Collections.Generic;
using NTPackage.Functions;
using NTPackage.UI;
using Rubik.Account;
using Rubik.Manager;
using TMPro;
using UnityEngine;

namespace Rubik.Authen
{
    public class LoginTab : NTBehaviour
    {

        public TMP_InputField InputFieldUsername;
        public TMP_InputField InputFieldPassword;
        public TextMeshProUGUI TextMessage;
        public NTButtonEffect BtnRemember;

        public string Username;
        public string Password;

        public void OnUI()
        {
            if (AccountManager.Instance.IsAutoLoginSelection())
                this.BtnRemember.Chose();
            else
                this.BtnRemember.Unchose();
            this.InputFieldUsername.text = "";
            this.InputFieldPassword.text = "";
            this.TextMessage.text = "";
        }

        public void _OnChangeUsername(string text)
        {
            this.Username = text;
        }

        public void _OnChangePassword(string text)
        {
            this.Password = text;
        }

        public void Login()
        {
            if(this.Username.Length == 0 || this.Password.Length == 0)
            {
                this.TextMessage.text = Lean.Localization.LeanLocalization.GetTranslationText("login_username_password_empty", "Username and password cannot be empty");
                this.TextMessage.color = NTFunction.StringHexToColor(AccountConfig.ErrorColor);
                return;
            }
            ServerManager.Instance.LoginByUsernamePassword(this.Username, this.Password, Success);
        }

        public void Success(AuthenResponse authenResponse)
        {
            if (authenResponse.Status == 1)
            {
                PopupManager.Instance.OffUI(PopupCode.LoginUI);
                this.TextMessage.text = Lean.Localization.LeanLocalization.GetTranslationText(authenResponse.Message ?? "");
                this.TextMessage.color = NTFunction.StringHexToColor(AccountConfig.SuccessColor);
            }
            else
            {
                this.TextMessage.text = Lean.Localization.LeanLocalization.GetTranslationText(authenResponse.Message ?? "");
                this.TextMessage.color = NTFunction.StringHexToColor(AccountConfig.ErrorColor);
            }
        }

        public void _OnChangeRemember(){
            if (!AccountManager.Instance.IsAutoLoginSelection())
            {
                this.BtnRemember.Chose();
                AccountManager.Instance.SetIsAutoLoginSelection(true);
            }
            else
            {
                this.BtnRemember.Unchose();
                AccountManager.Instance.SetIsAutoLoginSelection(false);
            }
           
               
        }
        
    }
}