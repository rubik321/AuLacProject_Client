using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rubik.Common.AudioHelper;
using TMPro;
using Lean.Localization;
namespace Rubik.SettingUI
{
    using NTPackage.Functions;
    using NTPackage.UI;
    using Rubik.Account;
    using Rubik.DataCenter;
    using Rubik.Localization;
    using Rubik.Manager;
    using Rubik.SystemData;
    using Rubik.UI;
    using Rubik.UserDataPlayer;

    public class SettingUI : PopupUI
    {
        public NTButtonEffect soundCheck;
        public NTButtonEffect musicCheck;

        public TextMeshProUGUI UserId;
        public TextMeshProUGUI versionID;
        public TextMeshProUGUI textConfrim;
        public TextMeshProUGUI TextTitle;
        [SerializeField] TMP_Dropdown m_Dropdown;
        public GameObject confirmPopup;
        public UnityEngine.UI.Button btnYes;

        public NTButtonEffect BtnLinkAccount;
        public NTButtonEffect BtnLinkGooglePlay;
        public NTButtonEffect BtnLinkGameCenter;

        public NTButtonEffect BtnDeleteAccount;

        public NTButtonEffect BtnRestoreIAP;

        protected override void Start()
        {
            base.Start();

            //Add listener for when the value of the Dropdown changes, to take action
            m_Dropdown.onValueChanged.AddListener(delegate
            {

                DropdownValueChanged(m_Dropdown);
            });

            if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
            {
                this.BtnRestoreIAP.gameObject.SetActive(true);
            }
            else
            {
                this.BtnRestoreIAP.gameObject.SetActive(false);
            }

            if (Application.platform == RuntimePlatform.Android)
            {
                this.BtnLinkGooglePlay.gameObject.SetActive(true);
            }
            else
            {
                this.BtnLinkGooglePlay.gameObject.SetActive(false);
            }
            if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
            {
                this.BtnLinkGameCenter.gameObject.SetActive(true);
            }
            else
            {
                this.BtnLinkGameCenter.gameObject.SetActive(false);
            }
        }

        [SerializeField] bool isclick = false;
        void DropdownValueChanged(TMP_Dropdown change)
        {

            if (!isclick && change.value != 0)
            {
                isclick = true;
                return;
            }

            switch (change.value)
            {
                case 0:
                    Debug.Log("English");
                    LocalizationManager.Instance.SelectLanguage(LocalizationConfig.English, () =>
                    {
                        this.UpdateData();
                    });
                    break;
                case 1:
                    Debug.Log("Vietnamese");
                    LocalizationManager.Instance.SelectLanguage(LocalizationConfig.Vietnamese, () =>
                    {
                        this.UpdateData();
                    });
                    break;
                case 2:
                    Debug.Log("Chinese");
                    LocalizationManager.Instance.SelectLanguage(LocalizationConfig.Chinese, () =>
                    {
                        this.UpdateData();
                    });
                    break;
                case 3:
                    Debug.Log("German");
                    LocalizationManager.Instance.SelectLanguage(LocalizationConfig.German, () =>
                    {
                        this.UpdateData();
                    });

                    break;
            }
        }
        public void OnclickSound()
        {
            Rubik.Common.AudioHelper.AudioCtrl.Instance.Play(Rubik.Common.AudioHelper.AudioName.UI_Checkbox);
            try
            {
                AudioCtrl.Instance.ChangeStateEffect();
            }
            catch (System.Exception) { }
            NTLog.LogMessage("OnclickSound");
            this.UpdateData();
        }

        public void OnclickMusic()
        {
            Rubik.Common.AudioHelper.AudioCtrl.Instance.Play(Rubik.Common.AudioHelper.AudioName.UI_Checkbox);
            try
            {
                AudioCtrl.Instance.ChangeStateMusic();
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e);
            }
            NTLog.LogMessage("OnclickMusic");
            this.UpdateData();
        }

        public override void UpdateData(object data = null)
        {
            base.UpdateData(data);
            //this.TextTitle.text = "Setting";
            if (AudioCtrl.Instance.IsMuteMusic())
            {
                this.musicCheck.Unchose();
            }
            else
            {
                this.musicCheck.Chose();
            }
            if (AudioCtrl.Instance.IsMuteEffect())
            {
                this.soundCheck.Unchose();
            }
            else
            {
                this.soundCheck.Chose();
            }
            this.UserId.text = UserDataManager.Instance.GetUserID();
            versionID.text = Application.version;
            if (LocalizationManager.Instance.Language == LocalizationConfig.English)
            {
                isclick = true;
            }
            this.m_Dropdown.value = GetValueLangeage();

            if (AccountManager.Instance.IsLinkMyrk())
            {
                this.BtnLinkAccount.Unchose();
            }
            else
            {
                this.BtnLinkAccount.Chose();
            }
            if (AccountManager.Instance.IsLinkGooglePlay())
            {
                this.BtnLinkGooglePlay.Unchose();
            }
            else
            {
                this.BtnLinkGooglePlay.Chose();
            }

            if (AccountManager.Instance.IsLinkAccount())
            {
                this.BtnDeleteAccount.Unchose();
            }
            else
            {
                this.BtnDeleteAccount.Chose();
            }
        }
        int GetValueLangeage()
        {
            switch (LocalizationManager.Instance.Language)
            {
                case LocalizationConfig.English:
                    return 0;

                case LocalizationConfig.Vietnamese:
                    return 1;

                case LocalizationConfig.Chinese:
                    return 2;

                case LocalizationConfig.German:
                    return 3;

                default:
                    return 0;
            }
        }

        public void Btn_TERMS_OF_SERVICE()
        {
            Application.OpenURL(SystemManager.Instance.SystemData.TermsOfService_Url);
        }

        public void Btn_POLICIES_ON_PRIVACY()
        {
            Application.OpenURL(SystemManager.Instance.SystemData.PoliciesOnPrivacy_Url);
        }

        public void OnNoticQuitGame()
        {
            PopupManager.Instance.OnUI(PopupCode.MessagePanel, null, (popupUI) =>
            {
                MessagePanel messageUI = (MessagePanel)popupUI;
                messageUI.SetData(Lean.Localization.LeanLocalization.GetTranslationText("quit", "Quit"), Lean.Localization.LeanLocalization.GetTranslationText("quit_ask", "Are you sure you want to quit the game?"));
                messageUI.btnClose = () =>
                {
                    Application.Quit();
                };
            });
        }
        public void OnNoticLogout()
        {
            PopupManager.Instance.OnUI(PopupCode.MessagePanel, null, (popupUI) =>
            {
                MessagePanel messageUI = (MessagePanel)popupUI;
                messageUI.SetData(Lean.Localization.LeanLocalization.GetTranslationText("logout", "Logout"), Lean.Localization.LeanLocalization.GetTranslationText("logout_ask", "Are you sure you want to logout?"));
                messageUI.btnClose = () =>
                {
                    ServerManager.Instance.LogOut();
                    this.OffUI();
                };
            });
        }
        public void OnNoticDiscord()
        {
            //Application.OpenURL(SystemManager.Instance.SystemData.Discord_Url);
        }


        public void OnResetDay()
        {
            StartCoroutine(UserDataManager.Instance.IEResetDay(() =>
            {
                this.UpdateData();
            }));
        }

        public void _OnClickBtnLinkAccount()
        {
            if (AccountManager.Instance.IsLinkMyrk())
            {
                string str = Lean.Localization.LeanLocalization.GetTranslationText("linked_account_notic", "Your Myrk account {0} has been linked successfully.");
                str = string.Format(str, AccountManager.Instance.Account.Username);
                PopupManager.Instance.OnUI(PopupCode.MessagePanel, null, (popup) =>
                {
                    MessagePanel messagePanel = popup as MessagePanel;
                    messagePanel.SetData(Lean.Localization.LeanLocalization.GetTranslationText("link_account_title", "Link Account"), str);
                });
            }
            else
            {
                if (AccountManager.Instance.IsLogin())
                {
                    PopupManager.Instance.OnUI(PopupCode.LinkAccountUI);
                }
                else
                {
                    PopupManager.Instance.OnUI(PopupCode.MessagePanel, null, (popup) =>
                    {
                        MessagePanel messagePanel = popup as MessagePanel;
                        messagePanel.SetData(Lean.Localization.LeanLocalization.GetTranslationText("link_account_title", "Link Account"), Lean.Localization.LeanLocalization.GetTranslationText("link_account_not_login", "Please log in to continue."));
                    });
                }
            }
        }

        public void _OnNoticDeleteAccount()
        {
            if (AccountManager.Instance.IsLinkMyrk())
            {
                PopupManager.Instance.OnUI(PopupCode.DeleteAccountUI);
            }
            else
            {
                PopupManager.Instance.OnUI(PopupCode.MessagePanel, null, (popup) =>
                {
                    MessagePanel messagePanel = popup as MessagePanel;
                    messagePanel.SetData(Lean.Localization.LeanLocalization.GetTranslationText("delete_account_title", "Delete Account"), Lean.Localization.LeanLocalization.GetTranslationText("delete_account_not_link", "Please link your account to continue!"));
                });
            }
        }

        public void _OnRestoreIAP()
        {
            PopupManager.Instance.OnUI(PopupCode.MessageOptionPanel, null, (popup) =>
            {
                MessageOptionPanel optionPanel = popup as MessageOptionPanel;
                optionPanel.SetData(Lean.Localization.LeanLocalization.GetTranslationText("restore_iap_title", "Restore Purchases"), Lean.Localization.LeanLocalization.GetTranslationText("restore_iap_content", "Are you sure you want to restore your purchases?"));
                optionPanel.SetActionConfirm(() =>
                {
                    IAPManager.Instance.RestorePurchases();
                }, Lean.Localization.LeanLocalization.GetTranslationText("btn_confirm", "Confirm"));
                optionPanel.SetActionReject(() =>
                {
                    optionPanel.OffUI();
                }, Lean.Localization.LeanLocalization.GetTranslationText("btn_cancel", "Cancel"));
                optionPanel.SetTimeConfirm();
            });
        }

        public void _OnClickBtnLinkGooglePlay()
        {
            if (!AccountManager.Instance.IsLinkGooglePlay())
            {
                PopupManager.Instance.OnUI(PopupCode.MessageOptionPanel, null, (popup) =>
                {
                    MessageOptionPanel messageOptionPanel = popup as MessageOptionPanel;
                    messageOptionPanel.SetData(Lean.Localization.LeanLocalization.GetTranslationText("link_google_play_title", "Link Google Play"), Lean.Localization.LeanLocalization.GetTranslationText("link_google_play_content", "Are you sure you want to link your Google Play account?"));
                    messageOptionPanel.SetActionConfirm(() =>
                    {
                        AccountManager.Instance.LinkGooglePlay((result) =>
                        {
                            if (result)
                            {
                                messageOptionPanel.OffUI();
                            }
                            this.UpdateData();
                        });
                    }, Lean.Localization.LeanLocalization.GetTranslationText("btn_confirm", "Confirm"));
                    messageOptionPanel.SetActionReject(() =>
                    {
                        messageOptionPanel.OffUI();
                    }, Lean.Localization.LeanLocalization.GetTranslationText("btn_cancel", "Cancel"));
                });
            }
            else
            {
                // PopupManager.Instance.OnUI(PopupCode.MessageOptionPanel, null, (PopupUI popupUI) =>
                // {
                //     MessageOptionPanel messageOptionPanel = popupUI as MessageOptionPanel;
                //     string googlePlayName = AccountManager.Instance.Account.GooglePlayName;
                //     string str = string.Format(Lean.Localization.LeanLocalization.GetTranslationText("linked_google_play_notic", "Your Google Play account {0} has been linked successfully."), googlePlayName);
                //     messageOptionPanel.SetData(Lean.Localization.LeanLocalization.GetTranslationText("link_google_play_title", "Link Google Play"), str);
                //     messageOptionPanel.SetActionConfirm(() =>
                //     {
                //         messageOptionPanel.OffUI();
                //     }, Lean.Localization.LeanLocalization.GetTranslationText("btn_close", "Close"));
                //     messageOptionPanel.SetActionReject(() =>
                //     {
                //         StartCoroutine(NTFunction.WaitSecond(0.5f, () =>
                //         {
                //             PopupManager.Instance.OnUI(PopupCode.MessageOptionPanel, null, (PopupUI popupUI) =>
                //             {
                //                 MessageOptionPanel messageOptionPanel = popupUI as MessageOptionPanel;
                //                 string str = string.Format(Lean.Localization.LeanLocalization.GetTranslationText("unlink_google_play_notic", "Are you sure you want to unlink your Google Play account {0}?"), googlePlayName);
                //                 messageOptionPanel.SetData(Lean.Localization.LeanLocalization.GetTranslationText("unlink_google_play_title", "Unlink Google Play"), str);
                //                 messageOptionPanel.SetActionConfirm(() =>
                //                 {
                //                     messageOptionPanel.OffUI();
                //                     AccountManager.Instance.UnLinkGooglePlay((result) =>
                //                     {
                //                         if (result)
                //                         {
                //                             messageOptionPanel.OffUI();
                //                         }
                //                         this.UpdateData();
                //                     });
                //                 }, Lean.Localization.LeanLocalization.GetTranslationText("btn_unlink", "Unlink"));
                //                 messageOptionPanel.SetActionReject(() =>
                //                 {
                //                     messageOptionPanel.OffUI();

                //                 }, Lean.Localization.LeanLocalization.GetTranslationText("btn_cancel", "Cancel"));
                //             });
                //         }));

                //     }, Lean.Localization.LeanLocalization.GetTranslationText("btn_unlink", "Unlink"));
                // });
                PopupManager.Instance.OnUI(PopupCode.MessagePanel, null, (popup) =>
                {
                    MessagePanel messagePanel = popup as MessagePanel;
                    string googlePlayName = AccountManager.Instance.Account.GooglePlayName;
                    string str = string.Format(Lean.Localization.LeanLocalization.GetTranslationText("linked_google_play_notic", "Your Google Play account {0} has been linked successfully."), googlePlayName);
                    messagePanel.SetData(Lean.Localization.LeanLocalization.GetTranslationText("link_google_play_title", "Link Google Play"), str);
                });
            }
        }

        public void _OnClickBtnLinkGameCenter()
        {
            if (!AccountManager.Instance.IsLinkGameCenter())
            {
                PopupManager.Instance.OnUI(PopupCode.MessageOptionPanel, null, (popup) =>
                {
                    MessageOptionPanel messageOptionPanel = popup as MessageOptionPanel;
                    messageOptionPanel.SetData(Lean.Localization.LeanLocalization.GetTranslationText("link_game_center_title", "Link Game Center"), Lean.Localization.LeanLocalization.GetTranslationText("link_game_center_content", "Are you sure you want to link your Game Center account?"));
                    messageOptionPanel.SetActionConfirm(() =>
                    {
                        AccountManager.Instance.LinkGameCenter((result) =>
                        {
                            if (result)
                            {
                                messageOptionPanel.OffUI();
                            }
                        });
                    }, Lean.Localization.LeanLocalization.GetTranslationText("btn_confirm", "Confirm"));
                    messageOptionPanel.SetActionReject(() =>
                    {
                        messageOptionPanel.OffUI();
                    }, Lean.Localization.LeanLocalization.GetTranslationText("btn_cancel", "Cancel"));
                });
            }
            else
            {
                PopupManager.Instance.OnUI(PopupCode.MessagePanel, null, (popup) =>
                {
                    MessagePanel messagePanel = popup as MessagePanel;
                    string appleName = AccountManager.Instance.Account.AppleName;
                    string str = string.Format(Lean.Localization.LeanLocalization.GetTranslationText("linked_game_center_notic", "Your Game Center account {0} has been linked successfully."), appleName);
                    messagePanel.SetData(Lean.Localization.LeanLocalization.GetTranslationText("link_game_center_title", "Link Game Center"), str);
                });
            }
        }
    }
}