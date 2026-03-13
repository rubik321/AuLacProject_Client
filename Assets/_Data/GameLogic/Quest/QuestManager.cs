using System;
using System.Collections;
using System.Collections.Generic;
using NTPackage;
using NTPackage.Functions;
using Rubik.DataCenter;
using Rubik.Manager;
using Rubik.UserDataPlayer;
using SimpleJSON;
using Sirenix.OdinInspector;
using UnityEngine;
using Pixelplacement;
namespace Rubik.Quest
{
    using NTPackage.EventDispatcher;
    using Rubik.CharacterPlayer;
    using Rubik.UI;
    using Rubik.ItemPlayer;
    using Rubik.Server;
    using Lean.Localization;

    public class QuestConfig
    {
        public const string API_Claim_Quest = "/api/2D_GPS/quest/claim_quest";
        public const string API_Get_Daily_Quest = "/api/2D_GPS/quest/get_daily_quest";
        public const string API_Do_Quest = "/api/2D_GPS/quest/client_do_quest";

    }

    public class QuestManager : NTBehaviour
    {
        public NTDictionary<string, DailyQuestPlayerData> DailyQuestDataDic;
        public NTDictionary<string, DailyQuestPlayer> DailyQuestPlayerDic;
        public NTDictionary<string, Sprite> spriteQuestDic;
        public Sprite[] lsQuest;

        public static QuestManager Instance;
        protected override void Awake()
        {
            base.Awake();
            if (Instance != null)
            {
                NTLog.LogError("Only 1 Instance allow");
                return;
            }
            Instance = this;
        }

        #region Function
        public IEnumerator LoadData()
        {
            this.DailyQuestPlayerDic = new NTDictionary<string, DailyQuestPlayer>();
            this.DailyQuestDataDic = new NTDictionary<string, DailyQuestPlayerData>();
            JSONNode jsonNode = JSONNode.Parse(DataCenterManager.Instance.GetData(DataName.DailyQuestData));
            int index = 0;
            foreach (JSONNode item in jsonNode)
            {
                DailyQuestPlayerData dailyQuestPlayerData = JsonUtility.FromJson<DailyQuestPlayerData>(item.ToString());
                DailyQuestDataDic.Add(dailyQuestPlayerData.Index.ToString(), dailyQuestPlayerData);
                spriteQuestDic.Add(dailyQuestPlayerData.Index.ToString(), lsQuest[index]);
                index++;
            }
            yield return null;
        }

        public void Logout()
        {
            this.DailyQuestPlayerDic.Clear();
        }

        public void UpdateQuest(DailyQuestPlayer[] quests)
        {
            if (quests == null || quests.Length == 0) return;

            foreach (DailyQuestPlayer quest in quests)
            {
                this.DailyQuestPlayerDic.Add(quest.Index.ToString(), quest);
            }
        }

        #endregion

        #region API
        public IEnumerator IEClaimQuest(QuestIndex questIndex, bool isAdv = false, Action done = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            jdata["index"] = (int)questIndex;
            jdata["isAdv"] = isAdv;
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), Rubik.Config.URL_Config.BASE_API_URL + QuestConfig.API_Claim_Quest, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                done?.Invoke();
            });
        }

        public IEnumerator IEGetDailyQuest(Action done = null){
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), Rubik.Config.URL_Config.BASE_API_URL + QuestConfig.API_Get_Daily_Quest, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                done?.Invoke();
            });
        }

        public IEnumerator IEClientDoQuest(QuestIndex questIndex, int amount = 1, Action done = null){
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            jdata["index"] = (int)questIndex;
            jdata["amount"] = amount;
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), Rubik.Config.URL_Config.BASE_API_URL + QuestConfig.API_Do_Quest, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                done?.Invoke();
            });
        }
        #endregion

        #region Getter
        public List<DailyQuestPlayerData> GetDailyQuestData(){
            List<DailyQuestPlayerData> quests = this.DailyQuestDataDic.ToList().FindAll(x => x.Type == QuestType.Daily);
            return quests;
        }

        public List<DailyQuestPlayerData> GetClanQuestData(){
            List<DailyQuestPlayerData> quests = this.DailyQuestDataDic.ToList().FindAll(x => x.Type == QuestType.Clan);
            return quests;
        }

        public DailyQuestPlayer GetQuest(QuestIndex index)
        {
            return this.DailyQuestPlayerDic.Get(index.ToString());
        }

        public DailyQuestPlayerData GetQuestData(QuestIndex index)
        {
            return this.DailyQuestDataDic.Get(index.ToString());
        }

        public int GetQuestProcess(QuestIndex index)
        {
            DailyQuestPlayer quest = this.GetQuest(index);
            if (quest == null) return 0;
            return quest.Process;
        }
        public Sprite GetQuestSprite(QuestIndex index)
        {
            
            return this.spriteQuestDic.Get(index.ToString());
        }

        public int GetQuestMaxProcess(QuestIndex index)
        {
            return this.GetQuestData(index).Max;
        }

        public bool IsQuestClaimed(QuestIndex index)
        {
            DailyQuestPlayer quest = this.GetQuest(index);
            if (quest == null) return false;
            return quest.Reward;
        }

        public bool IsQuestCompleted(QuestIndex index)
        {
            DailyQuestPlayer quest = this.GetQuest(index);
            if (quest == null) return false;
            return quest.Process >= this.GetQuestData(index).Max;
        }

        public ItemData[] GetQuestReward(QuestIndex index)
        {
            return this.GetQuestData(index).ItemRewards;
        }

        public string GetQuestName(QuestIndex index)
        {

            return LeanLocalization.GetTranslationText(index.ToString());
        }

        public string GetQuestDesc(QuestIndex index)
        {
            return LeanLocalization.GetTranslationText(index.ToString() + "_info");
        }

        public string WarningNotDoneQuest(QuestIndex index){
            int rest = this.GetQuestMaxProcess(index) - this.GetQuestProcess(index);
            if(rest <= 0) rest = 1;
            string str = LeanLocalization.GetTranslationText(index.ToString()+"_notdone", rest.ToString());
            str = string.Format(str, rest);
            return str;
        }

        public int CompareTo(DailyQuestPlayer a, DailyQuestPlayer b){
            int aValue = 0;
            int bValue = 0;
            if(this.IsQuestCompleted(a.Index)) aValue = -1;
            if(this.IsQuestCompleted(b.Index)) bValue = -1;
            if(this.IsQuestClaimed(a.Index)) aValue = 1;
            if(this.IsQuestClaimed(b.Index)) bValue = 1;
            if(aValue == bValue) return a.Index.CompareTo(b.Index);
            return aValue.CompareTo(bValue);
        }

        public bool IsDoneQuest(QuestIndex index){
            DailyQuestPlayer quest = this.GetQuest(index);
            if(quest == null) return false;
            return quest.Process >= this.GetQuestData(index).Max;
        }
        
        #endregion
    }
}
