using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
//using BestHTTP;
using System;
using Colyseus;
using SimpleJSON;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using Rubik.Manager;
using Rubik.UI;
using JWT;
using JWT.Serializers;
using JWT.Algorithms;
using NTPackage.Functions;
using System.Security.Cryptography;
using System.Text;
using NTPackage.UI;
using Rubik.Common.AudioHelper;
using Rubik.Banner;

namespace Rubik.Server
{
    public class API_Error
    {
        public const string SUCCESS = "SUCCESS";
        public const string DB_ERROR = "DB_ERROR";
        public const string UNKNOWN = "UNKNOWN";

        public const string USERDATA_NOT_FOUND_USER = "USERDATA_NOT_FOUND_USER";
        public const string USERDATA_NOT_ENOUGH_ITEM = "USERDATA_NOT_ENOUGH_ITEM";
        public const string USERDATA_SAME_OLD_NAME = "USERDATA_SAME_OLD_NAME";

        public const string ACCOUNT_WRONG_USERNAME_OR_PASSWORD = "ACCOUNT_WRONG_USERNAME_OR_PASSWORD";
        public const string ACCOUNT_NOT_FOUND = "ACCOUNT_NOT_FOUND";
        public const string ACCOUNT_GOOGLE_LINKED = "ACCOUNT_GOOGLE_LINKED";
        public const string ACCOUNT_GOOGLE_LINKED_BY_OTHER_ACCOUNT = "ACCOUNT_GOOGLE_LINKED_BY_OTHER_ACCOUNT";
    }

    public class APIConfig
    {
        public const string KeyTokenAPI = "KeyTokenAPI";
    }

    public class APIManager : MonoBehaviour
    {
        public static APIManager Instance;

        private void Awake()
        {
            if (APIManager.Instance != null)
            {
                Debug.LogError("Only 1 Instance allow");
                return;
            }

            APIManager.Instance = this;
        }
        public IEnumerator PostDataUrl(string json, string url, Action<UnityWebRequest> callback, bool isShowLoading = true, string token = "")
        {
            //panelLock.SetActive(true);
            if (isShowLoading)
            {
                this.ShowLoadingPanel();
            }
            var uwr = new UnityWebRequest(url, "POST");
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            NTLog.LogMessage("Post:" + url);
            uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            uwr.SetRequestHeader("Content-Type", "application/json");
            if (token.Length == 0) token = this.GenerateToken();
            uwr.SetRequestHeader("Authorization", "Token " + token);
            //Debug.Log(jsonAttackMod);
            //Send the request then wait here until it returns
            yield return uwr.SendWebRequest();
            if (isShowLoading)
            {
                this.HideLoadingPanel();
            }

            if (uwr.result == UnityWebRequest.Result.ConnectionError)
            {
                NTPackage.Functions.NTLog.LogWarning(JsonUtility.ToJson(("Post Error", json, url, uwr.error)), gameObject);
                var mess = Lean.Localization.LeanLocalization.GetTranslationText("error_while_sending", "Error While Sending: ");
                string title = Lean.Localization.LeanLocalization.GetTranslationText("error_while_sending_title", "Error");
                BannerManager.Instance.AddTopBanner(BannerTopConfig.ConnectUnstable, mess);
                // this.ShowNotification(title, mess, false);
                // AudioCtrl.Instance.Play(AudioName.UI_Popup_Panel_Error);
            }
            else
            {
                JSONNode info = JSON.Parse(uwr.downloadHandler.text);
                NTPackage.Functions.NTLog.LogMessage(("Post send: " + json, "Post url: " + url, gameObject, uwr.downloadHandler.text).ToString(), gameObject);
                string resultLG = info["Status"];
                if (resultLG == "0")
                {
                    string mess = info["Error"]["Message"];
                    mess = Lean.Localization.LeanLocalization.GetTranslationText(mess, mess);
                    NTPackage.Functions.NTLog.LogWarning(JsonUtility.ToJson(("Post Error:" + url, mess)), gameObject);
                    string title = Lean.Localization.LeanLocalization.GetTranslationText("error_title", "Error");
                    this.ShowNotification(title, mess, false);
                    AudioCtrl.Instance.Play(AudioName.UI_Popup_Panel_Error);
                }
                else
                {
                    NTPackage.Functions.NTLog.LogMessage(JsonUtility.ToJson(("Post Recive", json, url, uwr.downloadHandler.text)), gameObject);
                    callback(uwr);
                }
            }
            this.HideLoadingPanel();
        }

        public IEnumerator GetDataUrl(string url, string key, string data, Action<UnityWebRequest> callback)
        {

            var uwr = UnityWebRequest.Get(url);
            uwr.SetRequestHeader(key, data);
            uwr.SetRequestHeader("Content-Type", "application/json");
            yield return uwr.SendWebRequest();
            callback(uwr);
        }

        private void ShowNotification(string title, string mess, bool isDefaultSound = true)
        {
            PopupManager.Instance.OnUI(PopupCode.MessagePanel, null, (PopupUI popupUI) =>
            {
                popupUI.GetComponent<MessagePanel>().SetData(title, mess);
            }, isDefaultSound);
        }

        private void HideLoadingPanel()
        {
            HUDCanvas.Instance.HideLoadingPanel();
        }
        private void ShowLoadingPanel()
        {
            HUDCanvas.Instance.ShowLoadingPanel();
        }

        public string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }

        [ContextMenu("Token")]
        public string GenerateToken()
        {
            var payload = new Dictionary<string, object>
        {
            { "Section", /*UserDataManager.Instance.GetUserID() +*/ NTFunction.GenerateId() },
        };

            IJwtAlgorithm algorithm = new HMACSHA256Algorithm(); // symmetric
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            var token = encoder.Encode(payload, APIConfig.KeyTokenAPI);
            return token;
            // return "";
        }
    }
}