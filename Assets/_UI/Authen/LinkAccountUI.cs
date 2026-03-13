using System.Collections;
using System.Collections.Generic;
using NTPackage.Functions;
using NTPackage.UI;
using Rubik.Account;
using Rubik.Manager;
using Rubik.UI;
using TMPro;
using UnityEngine;

namespace Rubik.Authen
{
    public class LinkAccountUI : PopupUI
    {

        public TMP_InputField InputFieldUsername;
        public TMP_InputField InputFieldPassword;
        public TMP_InputField InputFieldRePassword;

        public TextMeshProUGUI TextMessage;

        public string Username;
        public string Password;
        public string RePassword;

        public override void OnUI(object data = null, bool isDefaultSound = true)
        {
            base.OnUI(data, isDefaultSound);
            this.InputFieldUsername.text = "";
            this.InputFieldPassword.text = "";
            this.InputFieldRePassword.text = "";
            this.TextMessage.text = "";
        }

        public override void OffUI(){
            base.OffUI();
            PopupManager.Instance.OnUI(PopupCode.SettingUI);
        }

        public void _OnChangeUsername(string text)
        {
            this.Username = text;
            this.CheckUsername();
        }

        public void _OnChangePassword(string text)
        {
            this.Password = text;
            this.CheckPassword();
        }

        public void _OnChangeRePassword(string text)
        {
            this.RePassword = text;
            this.CheckRePassword();
        }

        public void LinkAccount()
        {
            if (!this.CheckUsername()) return;
            if (!this.CheckPassword()) return;
            if (!this.CheckRePassword()) return;
            StartCoroutine(AccountManager.Instance.IELinkAccount(this.Username, this.Password, Success));
        }

        public void Success(AuthenResponse authenResponse)
        {
            if (authenResponse.Status == 1)
            {
                this.TextMessage.color = NTFunction.StringHexToColor(AccountConfig.SuccessColor);
                this.TextMessage.text = Lean.Localization.LeanLocalization.GetTranslationText(authenResponse.Message);
                StartCoroutine(NTFunction.WaitSecond(1, () =>
                {
                    this.OffUI();
                }));
            }
            else
            {
                this.TextMessage.color = NTFunction.StringHexToColor(AccountConfig.ErrorColor);
                this.TextMessage.text = Lean.Localization.LeanLocalization.GetTranslationText(authenResponse.Message);
            }
        }

        public bool CheckUsername()
        {
            if (this.Username == null || this.Username.Length == 0)
            {
                this.TextMessage.color = NTFunction.StringHexToColor(AccountConfig.ErrorColor);
                this.TextMessage.text = Lean.Localization.LeanLocalization.GetTranslationText("username_required", "Username is required");
                return false;
            }
            if (this.Username.Length < 6)
            {
                this.TextMessage.color = NTFunction.StringHexToColor(AccountConfig.ErrorColor);
                this.TextMessage.text = Lean.Localization.LeanLocalization.GetTranslationText("username_too_short", "Username must be at least 6 characters");
                return false;
            }
            if (!AccountConfig.ValidPattern.IsMatch(this.Username))
            {
                this.TextMessage.color = NTFunction.StringHexToColor(AccountConfig.ErrorColor);
                this.TextMessage.text = Lean.Localization.LeanLocalization.GetTranslationText("username_invalid", "Username allow only letters, numbers, underscore");
                return false;
            }
            this.TextMessage.text = "";
            return true;
        }

        public bool CheckPassword()
        {
            if (this.Password == null || this.Password.Length == 0)
            {
                this.TextMessage.color = NTFunction.StringHexToColor(AccountConfig.ErrorColor);
                this.TextMessage.text = Lean.Localization.LeanLocalization.GetTranslationText("password_required", "Password is required");
                return false;
            }
            if (this.Password.Length < 6)
            {
                this.TextMessage.color = NTFunction.StringHexToColor(AccountConfig.ErrorColor);
                this.TextMessage.text = Lean.Localization.LeanLocalization.GetTranslationText("password_too_short", "Password must be at least 6 characters");
                return false;
            }
            if (this.Password.Length < 6)
            {
                this.TextMessage.color = NTFunction.StringHexToColor(AccountConfig.ErrorColor);
                this.TextMessage.text = Lean.Localization.LeanLocalization.GetTranslationText("password_too_short", "Password must be at least 6 characters");
                return false;
            }
            this.TextMessage.text = "";
            return true;
        }

        public bool CheckRePassword()
        {
            if (this.Password != this.RePassword)
            {
                this.TextMessage.color = NTFunction.StringHexToColor(AccountConfig.ErrorColor);
                this.TextMessage.text = Lean.Localization.LeanLocalization.GetTranslationText("password_and_repassword_not_same", "Password and Repassword are not the same");
                return false;
            }
            if (this.RePassword == null || this.RePassword.Length == 0)
            {
                this.TextMessage.color = NTFunction.StringHexToColor(AccountConfig.ErrorColor);
                this.TextMessage.text = Lean.Localization.LeanLocalization.GetTranslationText("repassword_required", "Repassword is required");
                return false;
            }
            this.TextMessage.text = "";
            return true;
        }


    }
}