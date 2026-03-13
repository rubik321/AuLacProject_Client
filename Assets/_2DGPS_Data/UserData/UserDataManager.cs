using System.Collections;
using System.Collections.Generic;
using NTPackage.Functions;
using SimpleJSON;
using UnityEngine;
using NTPackage.Functions;
using GOA.Config;
using NTPackage.EventDispatcher;

namespace Rubik._2DGPS.UserData
{
    using Account;
    using NTFunctions_old;
    using UnityEngine.Purchasing.MiniJSON;

    public class UserDataManager : LoadBehaviour
    {
        public UserData UserData;

        public int TapTime = 0;

        public static UserDataManager instance;
        protected override void Awake()
        {
            base.Awake();
            if (UserDataManager.instance != null)
            {
                Debug.LogWarning("Only 1 instance allow");
                return;
            }
            UserDataManager.instance = this;
        }

        public override void LoadComponents()
        {
            base.LoadComponents();
        }


        public IEnumerator LoadData(){
            yield return null;
        }

        public void Init()
        {

        }

        public IEnumerator Login(int server)
        {
            if (AccountManager.instance.Account._id.Length > 0)
            {
                JSONNode jdata = new JSONObject();
                jdata["accountID"] = AccountManager.instance.Account._id;
                jdata["server"] = server;
                yield return APIManager.Instance.PostDataUrl(jdata.ToString(), SeverConfigs.GPS2D_Server + SeverConfigs._2DGPS_API_UserData_Login, (data) =>
                {
                    jdata = JSONNode.Parse(data.downloadHandler.text);
                    APIManager.Instance.BaseAPIRespone(jdata);
                });
            }
        }

        private IEnumerator Get()
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = this.UserData._id;
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), SeverConfigs.GPS2D_Server + SeverConfigs._2DGPS_API_UserData_Get, (data) =>
            {
                jdata = JSONNode.Parse(data.downloadHandler.text);
                APIManager.Instance.BaseAPIRespone(jdata);
            });
        }

        public void UpdateData(JSONNode jdata){
            this.UserData = JsonUtility.FromJson<UserData>(jdata.ToString());
        }

        public void UpdateCardTeam(JSONNode jdata){
            List<string> team = new List<string>();
            foreach (JSONNode item in jdata)
            {
                team.Add(item);
            }
            this.UserData.CardTeam = team.ToArray();
        }

        public void UpdateCurrencies(JSONNode jdata){
            foreach (JSONNode item in jdata)
            {
                CurrencyData currencyData = JsonUtility.FromJson<CurrencyData>(item.ToString());
                this.SetCurrency(currencyData.Type, currencyData.Amount);
            }
        }

        public string GetUserID()
        {
            return this.UserData._id;
        }

        public void AddCurrency(CurrencyType currencyType, int amount)
        {
            this.AddPropValue(currencyType.ToString(), amount);
        }

        public int GetCurrency(CurrencyType currencyType)
        {
            return GetPropValue(currencyType.ToString());
        }

        public void SetCurrency(CurrencyType currencyType, int amount)
        {
            this.SetPropValue(currencyType.ToString(), amount);
        }

        public int GetPropValue(string name)
        {
            return (int)this.UserData.GetType().GetField(name).GetValue(this.UserData);
        }

        public void SetPropValue(string name, int amount)
        {
            this.UserData.GetType().GetField(name).SetValue(this.UserData, amount);
        }

        public void AddPropValue(string name, int amount)
        {
            this.SetPropValue(name, this.GetPropValue(name) + amount);
        }
    }
}
