using System;
using System.Collections.Generic;
using System.Collections;
using NTPackage_old.Functions;
using Rubik.KingFish.Card;
using SimpleJSON;
using Sirenix.OdinInspector;
using UnityEngine;
using NTFunctions_old;
using GOA.Config;

namespace Rubik._2DGPS.Campaign
{
    using DataCenter;
    using UserData;

    public class CampaignManager : LoadBehaviour
    {

        public static CampaignManager instance;
        protected override void Awake()
        {
            base.Awake();
            if (CampaignManager.instance != null)
            {
                NTLog.LogWarning("Only 1 instance allow");
                return;
            }
            CampaignManager.instance = this;
        }

        public List<CampaignLvData> CampaignLvDatas;

        public IEnumerator LoadData()
        {
            this.LoadCardData();
            yield return null;
        }

        public void LoadCardData()
        {
            this.CampaignLvDatas = new List<CampaignLvData>();
            JSONNode jdata = JSONNode.Parse(DataCenterManager.instance.GetData(DataName.CampaignLvData));
            foreach (JSONNode item in jdata)
            {
                CampaignLvData worldMapLvData = JsonUtility.FromJson<CampaignLvData>(item.ToString());
                this.CampaignLvDatas.Add(worldMapLvData);
            }
        }

        [Button]
        public void PassCampaign(int level, Action<List<CurrencyData>> callback = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.instance.UserData._id;
            jdata["level"] = level;
            StartCoroutine(APIManager.Instance.PostDataUrl(jdata.ToString(), SeverConfigs.GPS2D_Server + SeverConfigs._2DGPS_API_Campaign_PassCampaign, (data) =>
                {
                    JSONNode jdata = JSONNode.Parse(data.downloadHandler.text);
                    APIManager.Instance.BaseAPIRespone(jdata);
                    List<CurrencyData> rewards = new List<CurrencyData>();
                    foreach (JSONNode item in jdata["Data"]["Rewards"])
                    {
                        try
                        {
                            CurrencyData currencyData = JsonUtility.FromJson<CurrencyData>(item.ToString());
                            rewards.Add(currencyData);
                        }
                        catch (System.Exception e)
                        {
                            NTLog.LogError(e.ToString(), gameObject);
                        }
                    }
                    UserDataManager.instance.UserData.CampaignLv = jdata["Data"]["CampaignLv"];
                    callback?.Invoke(rewards);
                }
            ));
        }
    }
}