using System;
using System.Collections;
using System.Collections.Generic;
using NTPackage;
using NTPackage.Functions;
using NTPackage.UI;
using Rubik.Config;
using Rubik.Manager;
using Rubik.Server;
using Rubik.UI;
using Rubik.UserDataPlayer;
using SimpleJSON;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Rubik.PlayerMail
{
    using System.Linq;
    using Rubik.Server;

    public class PlayerMailConfig
    {
        public const string API_GetMail = "/api/2D_GPS/player_mail/get_mail";
        public const string API_ReadMail = "/api/2D_GPS/player_mail/read_mail";
        public const string API_RecieveMail = "/api/2D_GPS/player_mail/recieve_mail";
        public const string API_DeleteMail = "/api/2D_GPS/player_mail/delete_mail";
    }
    public class PlayerMailManager : NTBehaviour
    {
        public NTDictionary<string, PlayerMail> PlayerMails;

        public bool IsNoticMail = false;

        public static PlayerMailManager instance;
        protected override void Awake()
        {
            base.Awake();
            if (PlayerMailManager.instance != null)
            {
                NTLog.LogWarning("Only 1 instance allow");
                return;
            }
            PlayerMailManager.instance = this;
        }

        #region Function

        public void Init()
        {
            this.PlayerMails = new NTDictionary<string, PlayerMail>();
        }

        public void Logout()
        {
            this.PlayerMails = new NTDictionary<string, PlayerMail>();
        }

        public void UpdatePlayerMail(PlayerMail[] playerMails)
        {
            foreach (PlayerMail item in playerMails)
            {
                if (this.PlayerMails.Get(item._id) == null)
                {
                    this.PlayerMails.Add(item._id, item);
                    this.PlayerMails.Get(item._id).UpdateData(item);
                }
                else
                {
                    this.PlayerMails.Get(item._id).UpdateData(item);
                }
            }
            PlayerMailManager.instance.UpdateNotic();
        }

        public void UpdateNotic()
        {
            this.IsNoticMail = false;
            foreach (PlayerMail item in this.PlayerMails.ToList())
            {
                if (item.Attached.IsEmpty())
                {
                    if (item.IsDelete == false && !item.IsRead)
                    {
                        this.IsNoticMail = true;
                        break;
                    }
                }
                else
                {
                    if (!item.IsRecieve && item.IsDelete == false)
                    {
                        this.IsNoticMail = true;
                        break;
                    }
                }
            }
        }

        public void DeleteAllMail(Action callback)
        {
            PopupManager.Instance.OnUI(PopupCode.MessageOptionPanel, null, (popupUI) =>
            {
                MessageOptionPanel messageOptionPanel = popupUI as MessageOptionPanel;
                messageOptionPanel.SetData(Lean.Localization.LeanLocalization.GetTranslationText("text_mail_delete_all", "Delete All Mail"), Lean.Localization.LeanLocalization.GetTranslationText("text_mail_delete_all_content", "Are you sure you want to delete all mail?"));
                messageOptionPanel.SetActionConfirm(() =>
                {
                    List<string> mailIDList = new List<string>();
                    foreach (PlayerMail item in this.PlayerMails.ToList())
                    {
                        if (item.IsDelete) continue;
                        if (!item.Attached.IsEmpty() && !item.IsRecieve) continue;
                        mailIDList.Add(item._id);
                    }
                    if (mailIDList.Count == 0) return;
                    JSONNode jdata = new JSONObject();
                    jdata["userID"] = UserDataManager.Instance.GetUserID();
                    jdata["playerMailIDs"] = mailIDList.ToArray();
                    StartCoroutine(this.DeleteMail(mailIDList.ToArray(), callback));
                }, Lean.Localization.LeanLocalization.GetTranslationText("btn_confirm", "Confirm"));
                messageOptionPanel.SetActionReject(() =>
                {
                    popupUI.OffUI();
                }, Lean.Localization.LeanLocalization.GetTranslationText("btn_cancel", "Cancel"));
            });
        }

        public void RecieveAllMail(Action callback)
        {
            List<string> mailIDList = new List<string>();
            foreach (PlayerMail item in this.PlayerMails.ToList())
            {
                if (item.IsDelete) continue;
                if (item.IsRecieve) continue;
                if (item.Attached.IsEmpty()) continue;
                mailIDList.Add(item._id);
            }
            if (mailIDList.Count == 0) return;
            StartCoroutine(this.RecieveMail(mailIDList.ToArray(), callback));
        }

        #endregion

        #region API

        [Button]
        public IEnumerator GetMail(Action callback)
        {
            this.PlayerMails = new NTDictionary<string, PlayerMail>();
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + PlayerMailConfig.API_GetMail, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                PlayerMailManager.instance.UpdateNotic();
                callback?.Invoke();
            });
        }

        public IEnumerator ReadMail(string mailID, Action callback)
        {
            PlayerMail playerMail = this.PlayerMails.Get(mailID);
            if (playerMail == null)
            {
                yield break;
            }
            else
            {
                if (!playerMail.IsRead) yield break;
                if (!playerMail.IsRecieve) yield break;
                if (playerMail.IsDelete) yield break;
                if (!playerMail.Attached.IsEmpty()) yield break;
            }

            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            jdata["playerMailID"] = mailID;
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + PlayerMailConfig.API_ReadMail, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                PlayerMailManager.instance.UpdateNotic();
                callback?.Invoke();
            });
        }

        [Button]
        public IEnumerator RecieveMail(string[] mailIDs, Action callback)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            jdata["playerMailIDs"] = mailIDs;
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + PlayerMailConfig.API_RecieveMail, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                PlayerMailManager.instance.UpdateNotic();
                callback?.Invoke();
            });
        }

        [Button]
        public IEnumerator DeleteMail(string[] mailIDs, Action callback)
        {
            List<string> mailIDList = new List<string>();
            foreach (string mailID in mailIDs)
            {
                PlayerMail playerMail = this.PlayerMails.Get(mailID);
                if (playerMail != null)
                {
                    if (!playerMail.Attached.IsEmpty() && !playerMail.IsRecieve)
                    {
                        HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("text_mail_not_recieve"));
                        continue;
                    }
                    mailIDList.Add(mailID);
                }
            }
            if (mailIDList.Count == 0) yield break;
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            jdata["playerMailIDs"] = mailIDList.ToArray();
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + PlayerMailConfig.API_DeleteMail, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                PlayerMailManager.instance.UpdateNotic();
                callback?.Invoke();
            });
        }

        #endregion

        #region Getter
        public bool IsEmpty()
        {
            if ((this.PlayerMails.ToList().FindAll(a => a.IsDelete == false)).Count == 0) return true;
            return false;
        }

        public PlayerMail GetPlayerMail(string mailID)
        {
            return this.PlayerMails.Get(mailID);
        }

        public bool CanRecieveMail()
        {
            bool isCanRecieve = false;
            foreach (PlayerMail item in this.PlayerMails.ToList())
            {
                if (!item.IsRecieve && !item.IsDelete && !item.Attached.IsEmpty())
                {
                    isCanRecieve = true;
                    break;
                }
            }
            return isCanRecieve;
        }

        public bool CanDeleteMail()
        {
            bool IsEmpty = true;
            bool isCanDelete = true;
            foreach (PlayerMail item in this.PlayerMails.ToList())
            {
                if (!item.IsRecieve && !item.IsDelete && item.Attached.IsEmpty())
                {
                    isCanDelete = false;
                    break;
                }
                if (!item.IsDelete) IsEmpty = false;
            }
            return isCanDelete && !IsEmpty;
        }

        #endregion
    }
}
