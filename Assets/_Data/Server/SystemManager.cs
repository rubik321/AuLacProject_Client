using System.Collections;
using System.Collections.Generic;
using NTPackage.Functions;
using NTPackage.UI;
using Rubik.Server;
using Rubik.UI;
using SimpleJSON;
using UnityEngine;

namespace Rubik.SystemData
{
    [System.Serializable]
    public class SystemData
    {
        public string[] Versions;
        public int GameVersion;
        public string API_Url;
        public string Socket_Url;
        public string AppStore_Url;
        public string GooglePlay_Url;
        public string IOS_Addressable;
        public string Android_Addressable;
        public string Discord_Url;
        public string TermsOfService_Url;
        public string PoliciesOnPrivacy_Url;

    }

    public class SystemConfig
    {
        public const string URL_HOST = "http://15.235.180.137:7040";
        public const string Hung_URL_HOST = "http://167.71.202.159:7040";
        public const string API_GET_SYSTEM = "/api/system/get_system_data";
    }
    public class SystemManager : NTBehaviour
    {
        public SystemData SystemData = new SystemData();

        public bool IsLoad = false;

        public static SystemManager Instance;
        protected override void Awake()
        {
            base.Awake();
            if (SystemManager.Instance != null)
            {
                NTLog.LogError("Only 1 Instance allow");
                return;
            }
            SystemManager.Instance = this;
        }

        public IEnumerator IEInit()
        {
            yield return this.IEGetSystemData();
            while (!this.IsLoad)
            {
                yield return new WaitForSeconds(0.2f);
                if (this.IsLoad)
                {
                    break;
                }
                else
                {
                    PopupManager.Instance.OnUI(PopupCode.MessageOptionPanel, null, (PopupUI popupUI) =>
                    {
                        MessageOptionPanel messageOptionPanel = popupUI as MessageOptionPanel;
                        messageOptionPanel.IsBlockClickScreenDim = true;
                        messageOptionPanel.SetData(Lean.Localization.LeanLocalization.GetTranslationText("get_system_fail_title", "System Data Failed"), Lean.Localization.LeanLocalization.GetTranslationText("get_system_fail_des", "System data failed. Check your connection and try again. If the problem persists, please contact support."));
                        messageOptionPanel.SetActionConfirm(() =>
                        {
                            StartCoroutine(this.IEGetSystemData());
                        }, Lean.Localization.LeanLocalization.GetTranslationText("btn_try_again", "Try Again"));
                        messageOptionPanel.SetActionReject(() =>
                        {
                            Application.Quit();
                        }, Lean.Localization.LeanLocalization.GetTranslationText("btn_exit", "Exit"));
                    });
                }
            }
        }

        public IEnumerator IEGetSystemData()
        {
            this.IsLoad = false;
            JSONNode jdata = new JSONObject();
            jdata["version"] = Application.version;
            jdata["platform"] = (int)Application.platform;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(jdata.ToString(), SystemConfig.Hung_URL_HOST + SystemConfig.API_GET_SYSTEM, (data) =>
            {
                jdata = JSONNode.Parse(data.downloadHandler.text);
                this.SystemData = JsonUtility.FromJson<SystemData>(jdata["Data"].ToString());
                this.IsLoad = true;
            });
        }
    }
}