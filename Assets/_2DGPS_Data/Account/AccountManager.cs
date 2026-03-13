using System.Collections;
using System.Collections.Generic;
using NTPackage_old.Functions;
using SimpleJSON;
using UnityEngine;
using NTFunctions_old;
using GOA.Config;

namespace Rubik._2DGPS.Account
{
    public class AccountManager : LoadBehaviour
    {
        public Account Account;

        public static AccountManager instance;
        protected override void Awake()
        {
            base.Awake();
            if (AccountManager.instance != null)
            {
                NTLog.LogWarning("Only 1 instance allow");
                return;
            }
            AccountManager.instance = this;
        }

        public IEnumerator Login(string username, string password)
        {
            JSONNode jdata = new JSONObject();
            jdata["username"] = username;
            jdata["password"] = password;
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), SeverConfigs.GPS2D_Server + SeverConfigs._2DGPS_API_Account_Login, (data) =>
            {
                jdata = JSONNode.Parse(data.downloadHandler.text);
                this.Account = JsonUtility.FromJson<Account>(jdata["Data"]["Account"].ToString());
            });
        }

        public IEnumerator Register(string email, string username, string password)
        {
            JSONNode jdata = new JSONObject();
            jdata["email"] = email;
            jdata["username"] = username;
            jdata["password"] = password;
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), SeverConfigs.GPS2D_Server + SeverConfigs._2DGPS_API_Account_Register, (data) =>
            {
                jdata = JSONNode.Parse(data.downloadHandler.text);
                this.Account = JsonUtility.FromJson<Account>(jdata["Data"]["Account"].ToString());
            });
        }

        public IEnumerator LoginByDeviceID(string deviceID)
        {
            JSONNode jdata = new JSONObject();
            jdata["deviceID"] = deviceID;
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), SeverConfigs.GPS2D_Server + SeverConfigs._2DGPS_API_Account_LoginByDeviceID, (data) =>
            {
                jdata = JSONNode.Parse(data.downloadHandler.text);
                this.Account = JsonUtility.FromJson<Account>(jdata["Data"]["Account"].ToString());
            });
        }
    }
}