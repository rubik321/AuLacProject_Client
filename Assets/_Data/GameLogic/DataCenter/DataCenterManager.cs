using System.Collections;
using System.Collections.Generic;
using NTPackage.Functions;
using SimpleJSON;
using UnityEngine;
using System;
using Rubik.Manager;
using Rubik.Config;
using Rubik.UI;
using Rubik.SystemData;

namespace Rubik.DataCenter
{
using Rubik.AddressablesLoader;
    public class DataCenterConfig
    {
        public const string API_DataCenter_CheckVersion = "/api/2D_GPS/data_center/check_version";
        public const string API_DataCenter_GetTimeServer = "/api/2D_GPS/data_center/get_time_server";
    }

    public class DataCenterManager : NTBehaviour
    {
        public DataVersion DataVersion;

        public DataCenterHolder DataCenterHolder;

        public static DataCenterManager Instance;
        protected override void Awake()
        {
            base.Awake();
            if (DataCenterManager.Instance != null)
            {
                NTLog.LogWarning("Only 1 instance allow");
                return;
            }
            DataCenterManager.Instance = this;
        }

        public override void LoadComponents()
        {
            base.LoadComponents();
        }

        protected void LoadDataVersionData()
        {
            try
            {
                string data = this.GetDataPrefs(DataName.DataVersion);
                if (data == null || data.Length == 0){
                    this.DataVersion = NTFunction.Clone(this.DataCenterHolder.DataVersion);
                    NTLog.LogMessage(JsonUtility.ToJson(this.DataCenterHolder.DataVersion));
                }
                else{
                    this.DataVersion = JsonUtility.FromJson<DataVersion>(data);
                    NTLog.LogMessage(JsonUtility.ToJson(this.DataVersion));
                }
                this.DataVersion.VersionGame = SystemManager.Instance.SystemData.GameVersion;
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString(), gameObject);
                this.DataVersion = new DataVersion();
            }

        }

        [NTButton]
        public void TestGetTimeServer(){
            StartCoroutine(this.IEGetTimeServer());
        }

        public IEnumerator IELoadDataCenterHolder(){
            int count = 0;
            AddressablesLoader.Instance.LoadAssetsFromLocal(AddressablesAssetsConfig.DataCenterHolder, (result) =>
            {
                this.DataCenterHolder = result.GetComponent<DataCenterHolder>();
                this.DataCenterHolder.transform.SetParent(transform);
                count++;
            });

            yield return new WaitUntil(() => count >= 1);
        }

        public bool CheckVersionDone = false;
        public IEnumerator CheckVersion(Action done = null)
        {
            this.CheckVersionDone = false;
            this.LoadDataVersionData();
            JSONNode jdata = new JSONObject();
            this.DataVersion.VersionGame = SystemManager.Instance.SystemData.GameVersion;
            jdata["dataVersion"] = JSONNode.Parse(JsonUtility.ToJson(this.DataVersion));
            jdata["platform"] = (int)Application.platform;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + DataCenterConfig.API_DataCenter_CheckVersion, (data) =>
            {
                jdata = JSONNode.Parse(data.downloadHandler.text);
                DataVersion dataVersion = JsonUtility.FromJson<DataVersion>(jdata["Data"]["DataVersion"].ToString());
                if (jdata["Data"]["AvatarData"] != null)
                {
                    this.SetData(DataName.AvatarData, jdata["Data"]["AvatarData"].ToString());
                    this.DataVersion.AvatarData = dataVersion.AvatarData;
                }
                if (jdata["Data"]["AvatarBorderData"] != null)
                {
                    this.SetData(DataName.AvatarBorderData, jdata["Data"]["AvatarBorderData"].ToString());
                    this.DataVersion.AvatarBorderData = dataVersion.AvatarBorderData;
                }
                if (jdata["Data"]["CostChangeName"] != null)
                {
                    this.SetData(DataName.CostChangeName, jdata["Data"]["CostChangeName"].ToString());
                    this.DataVersion.CostChangeName = dataVersion.CostChangeName;
                }
                if (jdata["Data"]["ItemDataInfo"] != null)
                {
                    this.SetData(DataName.ItemDataInfo, jdata["Data"]["ItemDataInfo"].ToString());
                    this.DataVersion.ItemDataInfo = dataVersion.ItemDataInfo;
                }
                if (jdata["Data"]["CharacterClothData"] != null)
                {
                    this.SetData(DataName.CharacterClothData, jdata["Data"]["CharacterClothData"].ToString());
                    this.DataVersion.CharacterClothData = dataVersion.CharacterClothData;
                }
                if (jdata["Data"]["CharacterPlayerData"] != null)
                {
                    this.SetData(DataName.CharacterPlayerData, jdata["Data"]["CharacterPlayerData"].ToString());
                    this.DataVersion.CharacterPlayerData = dataVersion.CharacterPlayerData;
                }
                if (jdata["Data"]["CharacterGearData"] != null)
                {
                    this.SetData(DataName.CharacterGearData, jdata["Data"]["CharacterGearData"].ToString());
                    this.DataVersion.CharacterGearData = dataVersion.CharacterGearData;
                }
                if (jdata["Data"]["CharacterGearUpgradeLvData"] != null)
                {
                    this.SetData(DataName.CharacterGearUpgradeLvData, jdata["Data"]["CharacterGearUpgradeLvData"].ToString());
                    this.DataVersion.CharacterGearUpgradeLvData = dataVersion.CharacterGearUpgradeLvData;
                }
                if (jdata["Data"]["IAP_Shop_Cfg"] != null)
                {
                    this.SetData(DataName.IAP_Shop_Cfg, jdata["Data"]["IAP_Shop_Cfg"].ToString());
                    this.DataVersion.IAP_Shop_Cfg = dataVersion.IAP_Shop_Cfg;
                }
                if (jdata["Data"]["CardPlayerData"] != null)
                {
                    this.SetData(DataName.CardPlayerData, jdata["Data"]["CardPlayerData"].ToString());
                    this.DataVersion.CardPlayerData = dataVersion.CardPlayerData;
                }
                if (jdata["Data"]["DailyQuestData"] != null)
                {
                    this.SetData(DataName.DailyQuestData, jdata["Data"]["DailyQuestData"].ToString());
                    this.DataVersion.DailyQuestData = dataVersion.DailyQuestData;
                }
                if (jdata["Data"]["AchievementData"] != null)
                {
                    this.SetData(DataName.AchievementData, jdata["Data"]["AchievementData"].ToString());
                    this.DataVersion.AchievementData = dataVersion.AchievementData;
                }
                if (jdata["Data"]["AchievementBadgeData"] != null)
                {
                    this.SetData(DataName.AchievementBadgeData, jdata["Data"]["AchievementBadgeData"].ToString());
                    this.DataVersion.AchievementBadgeData = dataVersion.AchievementBadgeData;
                }
                if (jdata["Data"]["CardEvolveData"] != null)
                {
                    this.SetData(DataName.CardEvolveData, jdata["Data"]["CardEvolveData"].ToString());
                    this.DataVersion.CardEvolveData = dataVersion.CardEvolveData;
                }
                if (jdata["Data"]["ServerGameData"] != null)
                {
                    this.SetData(DataName.ServerGameData, jdata["Data"]["ServerGameData"].ToString());
                    this.DataVersion.ServerGameData = dataVersion.ServerGameData;
                }
                if (jdata["Data"]["CardUpStarData"] != null)
                {
                    this.SetData(DataName.CardUpStarData, jdata["Data"]["CardUpStarData"].ToString());
                    this.DataVersion.CardUpStarData = dataVersion.CardUpStarData;
                }
                if (jdata["Data"]["BattleMonsterWorldMapData"] != null)
                {
                    this.SetData(DataName.BattleMonsterWorldMapData, jdata["Data"]["BattleMonsterWorldMapData"].ToString());
                    this.DataVersion.BattleMonsterWorldMapData = dataVersion.BattleMonsterWorldMapData;
                }
                if (jdata["Data"]["CardSkillData"] != null)
                {
                    this.SetData(DataName.CardSkillData, jdata["Data"]["CardSkillData"].ToString());
                    this.DataVersion.CardSkillData = dataVersion.CardSkillData;
                }
                if (jdata["Data"]["CardSkillPassiveData"] != null)
                {
                    this.SetData(DataName.CardSkillPassiveData, jdata["Data"]["CardSkillPassiveData"].ToString());
                    this.DataVersion.CardSkillPassiveData = dataVersion.CardSkillPassiveData;
                }
                if (jdata["Data"]["ExpPlayerData"] != null)
                {
                    this.SetData(DataName.ExpPlayerData, jdata["Data"]["ExpPlayerData"].ToString());
                    this.DataVersion.ExpPlayerData = dataVersion.ExpPlayerData;
                }
                if (jdata["Data"]["CardLevelData"] != null)
                {
                    this.SetData(DataName.CardLevelData, jdata["Data"]["CardLevelData"].ToString());
                    this.DataVersion.CardLevelData = dataVersion.CardLevelData;
                }
                if (jdata["Data"]["PackageIAPData"] != null)
                {
                    this.SetData(DataName.PackageIAPData, jdata["Data"]["PackageIAPData"].ToString());
                    this.DataVersion.PackageIAPData = dataVersion.PackageIAPData;
                }
                if (jdata["Data"]["InventoryBagData"] != null)
                {
                    this.SetData(DataName.InventoryBagData, jdata["Data"]["InventoryBagData"].ToString());
                    this.DataVersion.InventoryBagData = dataVersion.InventoryBagData;
                }
                if (jdata["Data"]["EnergyData"] != null)
                {
                    this.SetData(DataName.EnergyData, jdata["Data"]["EnergyData"].ToString());
                    this.DataVersion.EnergyData = dataVersion.EnergyData;
                }
                if (jdata["Data"]["ClanData"] != null)
                {
                    this.SetData(DataName.ClanData, jdata["Data"]["ClanData"].ToString());
                    this.DataVersion.ClanData = dataVersion.ClanData;
                }
                if (jdata["Data"]["CharacterGearStatsLvData"] != null)
                {
                    this.SetData(DataName.CharacterGearStatsLvData, jdata["Data"]["CharacterGearStatsLvData"].ToString());
                    this.DataVersion.CharacterGearStatsLvData = dataVersion.CharacterGearStatsLvData;
                }
                if (jdata["Data"]["PlayerChestData"] != null)
                {
                    this.SetData(DataName.PlayerChestData, jdata["Data"]["PlayerChestData"].ToString());
                    this.DataVersion.PlayerChestData = dataVersion.PlayerChestData;
                }
                if (jdata["Data"]["PlayerChestSlotData"] != null)
                {
                    this.SetData(DataName.PlayerChestSlotData, jdata["Data"]["PlayerChestSlotData"].ToString());
                    this.DataVersion.PlayerChestSlotData = dataVersion.PlayerChestSlotData;
                }
                if (jdata["Data"]["ClanBossData"] != null)
                {
                    this.SetData(DataName.ClanBossData, jdata["Data"]["ClanBossData"].ToString());
                    this.DataVersion.ClanBossData = dataVersion.ClanBossData;
                }
                if (jdata["Data"]["PlayerChestLevelData"] != null)
                {
                    this.SetData(DataName.PlayerChestLevelData, jdata["Data"]["PlayerChestLevelData"].ToString());
                    this.DataVersion.PlayerChestLevelData = dataVersion.PlayerChestLevelData;
                }
                if (jdata["Data"]["CardSummonData"] != null)
                {
                    this.SetData(DataName.CardSummonData, jdata["Data"]["CardSummonData"].ToString());
                    this.DataVersion.CardSummonData = dataVersion.CardSummonData;
                }
                if (jdata["Data"]["PlayerChestDecreaseTimeData"] != null)
                {
                    this.SetData(DataName.PlayerChestDecreaseTimeData, jdata["Data"]["PlayerChestDecreaseTimeData"].ToString());
                    this.DataVersion.PlayerChestDecreaseTimeData = dataVersion.PlayerChestDecreaseTimeData;
                }
                if (jdata["Data"]["DailyRewardData"] != null)
                {
                    this.SetData(DataName.DailyRewardData, jdata["Data"]["DailyRewardData"].ToString());
                    this.DataVersion.DailyRewardData = dataVersion.DailyRewardData;
                }
                if (jdata["Data"]["BattlePassData"] != null)
                {
                    this.SetData(DataName.BattlePassData, jdata["Data"]["BattlePassData"].ToString());
                    this.DataVersion.BattlePassData = dataVersion.BattlePassData;
                }
                if (jdata["Data"]["SkillLevelByStarData"] != null)
                {
                    this.SetData(DataName.SkillLevelByStarData, jdata["Data"]["SkillLevelByStarData"].ToString());
                    this.DataVersion.SkillLevelByStarData = dataVersion.SkillLevelByStarData;
                }
                if (jdata["Data"]["BossPortalData"] != null)
                {
                    this.SetData(DataName.BossPortalData, jdata["Data"]["BossPortalData"].ToString());
                    this.DataVersion.BossPortalData = dataVersion.BossPortalData;
                }
                if (jdata["Data"]["ClanShopData"] != null)
                {
                    this.SetData(DataName.ClanShopData, jdata["Data"]["ClanShopData"].ToString());
                    this.DataVersion.ClanShopData = dataVersion.ClanShopData;
                }
                if (jdata["Data"]["OutpostData"] != null)
                {
                    this.SetData(DataName.OutpostData, jdata["Data"]["OutpostData"].ToString());
                    this.DataVersion.OutpostData = dataVersion.OutpostData;
                }
                if (jdata["Data"]["CardTierOriginData"] != null)
                {
                    this.SetData(DataName.CardTierOriginData, jdata["Data"]["CardTierOriginData"].ToString());
                    this.DataVersion.CardTierOriginData = dataVersion.CardTierOriginData;
                }
                if (jdata["Data"]["ItemExchangeAdvData"] != null)
                {
                    this.SetData(DataName.ItemExchangeAdvData, jdata["Data"]["ItemExchangeAdvData"].ToString());
                    this.DataVersion.ItemExchangeAdvData = dataVersion.ItemExchangeAdvData;
                }
                if (jdata["Data"]["ItemExchangeData"] != null)
                {
                    this.SetData(DataName.ItemExchangeData, jdata["Data"]["ItemExchangeData"].ToString());
                    this.DataVersion.ItemExchangeData = dataVersion.ItemExchangeData;
                }
                if (jdata["Data"]["FunctionLockData"] != null)
                {
                    this.SetData(DataName.FunctionLockData, jdata["Data"]["FunctionLockData"].ToString());
                    this.DataVersion.FunctionLockData = dataVersion.FunctionLockData;
                }

                if (jdata["Data"]["ArenaRankingData"] != null)
                {
                    this.SetData(DataName.ArenaRankingData, jdata["Data"]["ArenaRankingData"].ToString());
                    this.DataVersion.ArenaRankingData = dataVersion.ArenaRankingData;
                }
                if (jdata["Data"]["ArenaData"] != null)
                {
                    this.SetData(DataName.ArenaData, jdata["Data"]["ArenaData"].ToString());
                    this.DataVersion.ArenaData = dataVersion.ArenaData;
                }
                if (jdata["Data"]["AdvLimitData"] != null)
                {
                    this.SetData(DataName.AdvLimitData, jdata["Data"]["AdvLimitData"].ToString());
                    this.DataVersion.AdvLimitData = dataVersion.AdvLimitData;

                }
                if (jdata["Data"]["GearSkillActiveData"] != null)
                {
                    this.SetData(DataName.GearSkillActiveData, jdata["Data"]["GearSkillActiveData"].ToString());
                    this.DataVersion.GearSkillActiveData = dataVersion.GearSkillActiveData;
                }
                if (jdata["Data"]["GearSkillPassiveData"] != null)
                {
                    this.SetData(DataName.GearSkillPassiveData, jdata["Data"]["GearSkillPassiveData"].ToString());
                    this.DataVersion.GearSkillPassiveData = dataVersion.GearSkillPassiveData;
                }
                if (jdata["Data"]["CharacterGearRarityBonusData"] != null)
                {
                    this.SetData(DataName.CharacterGearRarityBonusData, jdata["Data"]["CharacterGearRarityBonusData"].ToString());
                    this.DataVersion.CharacterGearRarityBonusData = dataVersion.CharacterGearRarityBonusData;
                }
                this.SetData(DataName.DataVersion, JsonUtility.ToJson(this.DataVersion));
                this.CheckVersionDone = true;
                done?.Invoke();
            });
            if (!this.CheckVersionDone)
            {
                // HUDCanvas.Instance.
            }
            yield return new WaitUntil(() => this.CheckVersionDone);
        }

        public IEnumerator IEGetTimeServer(){
            JSONNode jdata = new JSONObject();
            jdata["platform"] = (int)Application.platform;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + DataCenterConfig.API_DataCenter_GetTimeServer, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
            });
        }

        public void SetData(DataName dataName, string data)
        {
            PlayerPrefs.SetString("Rubik:2DGPS:" + dataName, data);
        }

        public string GetData(DataName dataName)
        {
            string data = null;
            if(this.GetDataVersion(dataName) > this.GetCenterHolderVersion(dataName)){
                data = this.GetDataPrefs(dataName);
            }else{
                data = this.GetCenterHolderData(dataName);
            }
            if(data == null || data.Length == 0){
                NTLog.LogError("Not found: " + dataName.ToString(), gameObject);
                return this.GetCenterHolderData(dataName);;
            }
            return data;
        }

        public int GetDataVersion(DataName dataName){
            // Get variable by string name
            var variable = this.DataVersion.GetType().GetField(dataName.ToString());
            if (variable == null)
            {
                NTLog.LogError("Not found: " + dataName.ToString(), gameObject);
                return 0;
            }
            return (int)variable.GetValue(this.DataVersion);
        }

        public string GetDataPrefs(DataName dataName){
            return PlayerPrefs.GetString("Rubik:2DGPS:" + dataName);
        }

        public int GetCenterHolderVersion(DataName dataName){
            var variable = this.DataCenterHolder.DataVersion.GetType().GetField(dataName.ToString());
            if (variable == null)
            {
                NTLog.LogError("Not found: " + dataName.ToString(), gameObject);
                return 0;
            }
            return (int)variable.GetValue(this.DataCenterHolder.DataVersion);
        }

        public string GetCenterHolderData(DataName dataName){
            HolderData holderData = this.DataCenterHolder.HolderDataList.Find(x => x.DataName == dataName);
            if (holderData == null)
            {
                NTLog.LogError("Not found: " + dataName.ToString(), gameObject);
                return null;
            }
            return holderData.Data;
        }

        public DataName DataNameToShow;
        [NTButton]
        public void ShowData()
        {
            NTLog.LogMessage(this.GetData(this.DataNameToShow));
        }
    }
}

