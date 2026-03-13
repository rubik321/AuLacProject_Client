using NTPackage.Functions;
using NTPackage.UI;
using Rubik.Account;
using Rubik.Manager;
using Rubik.UI;
using TMPro;
using UnityEngine;

namespace Rubik.Authen
{
    public class DeleteAccountUI : PopupUI
    {
        public TMP_InputField InputFieldUsername;
        public TMP_InputField InputFieldPassword;
        public TextMeshProUGUI TextMessage;

        public string Username;
        public string Password;

        public override void OnUI(object data = null, bool isDefaultSound = true)
        {
            base.OnUI(data, isDefaultSound);
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

        public void DeleteAccount()
        {
            StartCoroutine(AccountManager.Instance.IEDeleteAccount(this.Username, this.Password, Success));
        }

        public void Success(AuthenResponse authenResponse)
        {
            if (authenResponse.Status == 1)
            {
                this.TextMessage.text = Lean.Localization.LeanLocalization.GetTranslationText(authenResponse.Message ?? "");
                this.TextMessage.color = NTFunction.StringHexToColor(AccountConfig.SuccessColor);
                PopupManager.Instance.OnUI(PopupCode.MessageOptionPanel, null, (popupUI) =>
                {
                    MessageOptionPanel messageOptionPanel = popupUI as MessageOptionPanel;
                    messageOptionPanel.SetData(Lean.Localization.LeanLocalization.GetTranslationText("delete_account_title", "Delete Account"), Lean.Localization.LeanLocalization.GetTranslationText("delete_account_ask", "This action will permanently erase your account and all progress. This cannot be undone. Are you sure you wish to proceed?"));

                    messageOptionPanel.SetActionConfirm(() =>
                    {
                        popupUI.OffUI();
                        StartCoroutine(AccountManager.Instance.IEConfirmDeleteAccount((authenResponse) =>
                        {
                            if (authenResponse.Status == 1)
                            {
                                PopupManager.Instance.OffUI(PopupCode.DeleteAccountUI);
                                ServerManager.Instance.LogOut();
                            }
                            else
                            {
                                this.TextMessage.text = Lean.Localization.LeanLocalization.GetTranslationText(authenResponse.Message ?? "");
                                this.TextMessage.color = NTFunction.StringHexToColor(AccountConfig.ErrorColor);
                            }
                            PopupManager.Instance.OffUI(PopupCode.MessageOptionPanel);
                        }));
                    }, Lean.Localization.LeanLocalization.GetTranslationText("btn_confirm", "Confirm"));
                    messageOptionPanel.SetActionReject(() =>
                    {
                        popupUI.OffUI();
                    }, Lean.Localization.LeanLocalization.GetTranslationText("btn_cancel", "Cancel"));
                    messageOptionPanel.SetTimeConfirm();
                });

            }
            else
            {
                this.TextMessage.text = Lean.Localization.LeanLocalization.GetTranslationText(authenResponse.Message ?? "");
                this.TextMessage.color = NTFunction.StringHexToColor(AccountConfig.ErrorColor);
            }
        }
    }
}