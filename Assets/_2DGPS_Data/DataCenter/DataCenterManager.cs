using System.Collections;
using System.Collections.Generic;
using NTPackage_old.Functions;
using SimpleJSON;
using UnityEngine;
using NTFunctions_old;
using GOA.Config;

namespace Rubik._2DGPS.DataCenter
{
    public class DataCenterManager : LoadBehaviour
    {
        public DataVersion DataVersion;
        public bool IsInit = false;

        public static DataCenterManager instance;
        protected override void Awake()
        {
            base.Awake();
            if (DataCenterManager.instance != null)
            {
                NTLog.LogWarning("Only 1 instance allow");
                return;
            }
            DataCenterManager.instance = this;
        }

        public override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadDataVersionData();
        }

        protected void LoadDataVersionData()
        {
            try
            {
                string data = this.GetData(DataName.DataVersion);
                if (data == null) this.DataVersion = new DataVersion();
                else this.DataVersion = JsonUtility.FromJson<DataVersion>(data);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString(), gameObject);
                this.DataVersion = new DataVersion();
            }

        }

        public IEnumerator CheckVersion()
        {
            this.LoadDataVersionData();
            JSONNode jdata = new JSONObject();
            jdata["dataVersion"] = JSONNode.Parse(JsonUtility.ToJson(this.DataVersion));
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), Rubik.Config.URL_Config.BASE_API_URL + SeverConfigs._2DGPS_DataCenter_CheckVersion, (data) =>
            {
                jdata = JSONNode.Parse(data.downloadHandler.text);
                DataVersion dataVersion = JsonUtility.FromJson<DataVersion>(jdata["Data"]["DataVersion"].ToString());
                if (jdata["Data"]["ServerGame"] != null)
                {
                    this.SetData(DataName.ServerGame, jdata["Data"]["ServerGame"].ToString());
                    this.DataVersion.ServerGame = dataVersion.ServerGame;
                }
                if (jdata["Data"]["CardData"] != null)
                {
                    this.SetData(DataName.CardData, jdata["Data"]["CardData"].ToString());
                    this.DataVersion.CardData = dataVersion.CardData;
                }
                if (jdata["Data"]["WorldMapLvData"] != null)
                {
                    this.SetData(DataName.WorldMapLvData, jdata["Data"]["WorldMapLvData"].ToString());
                    this.DataVersion.WorldMapLvData = dataVersion.WorldMapLvData;
                }
                if (jdata["Data"]["CampaignLvData"] != null)
                {
                    this.SetData(DataName.CampaignLvData, jdata["Data"]["CampaignLvData"].ToString());
                    this.DataVersion.CampaignLvData = dataVersion.CampaignLvData;
                }
                if (jdata["Data"]["GearData"] != null)
                {
                    this.SetData(DataName.GearData, jdata["Data"]["GearData"].ToString());
                    this.DataVersion.GearData = dataVersion.GearData;
                }
                if (jdata["Data"]["GearLvCost"] != null)
                {
                    this.SetData(DataName.GearLvCost, jdata["Data"]["GearLvCost"].ToString());
                    this.DataVersion.GearLvCost = dataVersion.GearLvCost;
                }
                if (jdata["Data"]["CharacterLvCost"] != null)
                {
                    this.SetData(DataName.CharacterLvCost, jdata["Data"]["CharacterLvCost"].ToString());
                    this.DataVersion.CharacterLvCost = dataVersion.CharacterLvCost;
                }
                if (jdata["Data"]["CardEvolveData"] != null)
                {
                    this.SetData(DataName.CardEvolveData, jdata["Data"]["CardEvolveData"].ToString());
                    this.DataVersion.CardEvolveData = dataVersion.CardEvolveData;
                }
                this.IsInit = true;
            });
            this.SetData(DataName.DataVersion, JsonUtility.ToJson(this.DataVersion));
        }

        public void SetData(DataName dataName, string data)
        {
            PlayerPrefs.SetString("GOA:" + dataName, data);
        }
        public void DelData(DataName dataName)
        {
            PlayerPrefs.DeleteKey("GOA:" + dataName);
        }

        public string GetData(DataName dataName)
        {
            string data = PlayerPrefs.GetString("GOA:" + dataName);
            if (data == null || data.Length == 0)
            {
                NTLog.LogError("Not found: " + dataName.ToString(), gameObject);
                return null;
            }
            return data;
        }

        public void ClearDataCenter()
        {
            this.IsInit = false;
            this.DelData(DataName.CardData);
            this.DelData(DataName.ServerGame);
        }

        public void Init(){
            
        }
    }
}

