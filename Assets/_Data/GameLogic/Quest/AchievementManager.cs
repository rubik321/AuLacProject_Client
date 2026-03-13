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

namespace Rubik.Quest
{
    using NTPackage.EventDispatcher;
    using Rubik.CharacterPlayer;
    using Rubik.UI;
    using Rubik.ItemPlayer;
    using Rubik.Server;
    using Lean.Localization;
    using Rubik.AddressablesLoader;

    public class AchievementConfig
    {
        public const string API_Claim_Achievement = "/api/2D_GPS/achievement/claim_achievement";
        public const string API_Get_Achievement = "/api/2D_GPS/achievement/get_achievement";
        public const string API_Client_Do_Achievement = "/api/2D_GPS/achievement/client_do_achievement";
        public const string API_Get_Achievement_Badge = "/api/2D_GPS/achievement/equip_achievement_badge";

    }

    public class AchievementManager : NTBehaviour
    {
        #region Player Data
        public NTDictionary<string, AchievementPlayer> AchievementPlayerDic;
        public AchievementBadgePlayer AchievementBadgePlayer;
        #endregion

        #region Data
        public NTDictionary<string, AchievementPlayerData> AchievementDataDic;
        public NTDictionary<string, AchievementBadge> AchievementBadgeDic;
        #endregion

        #region Resource
        public ListSpriteAddressable IconAchievementAddressable;
        public NTDictionary<string, Sprite> IconAchievementDic;
        public Sprite DefaultIcon;
        #endregion

        public static AchievementManager Instance;
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
            this.AchievementPlayerDic = new NTDictionary<string, AchievementPlayer>();
            this.AchievementDataDic = new NTDictionary<string, AchievementPlayerData>();
            this.AchievementBadgeDic = new NTDictionary<string, AchievementBadge>();
            this.AchievementBadgePlayer = new AchievementBadgePlayer();
            JSONNode jsonNode = JSONNode.Parse(DataCenterManager.Instance.GetData(DataName.AchievementData));
            int index = 0;
            foreach (JSONNode item in jsonNode)
            {
                AchievementPlayerData achievementPlayerData = JsonUtility.FromJson<AchievementPlayerData>(item.ToString());
                AchievementDataDic.Add(achievementPlayerData.Index.ToString(), achievementPlayerData);
                index++;
            }
            jsonNode = JSONNode.Parse(DataCenterManager.Instance.GetData(DataName.AchievementBadgeData));
            foreach (JSONNode item in jsonNode)
            {
                AchievementBadge achievementBadge = JsonUtility.FromJson<AchievementBadge>(item.ToString());
                AchievementBadgeDic.Add(achievementBadge.Index.ToString(), achievementBadge);
            }

            int count = 0;
            AddressablesLoader.Instance.LoadAssetsFromLocal(AddressablesAssetsConfig.AchievementSprite, (result) =>
            {
                this.IconAchievementAddressable = result.GetComponent<ListSpriteAddressable>();
                this.IconAchievementAddressable.transform.SetParent(transform);
                this.IconAchievementDic = new NTDictionary<string, Sprite>();
                foreach (Sprite item in this.IconAchievementAddressable.ListSprite)
                {
                    this.IconAchievementDic.Add(item.name, item);
                }
                count++;
            });
            yield return new WaitUntil(() => count >= 1);
        }

        public void Logout()
        {
            this.AchievementPlayerDic.Clear();
            this.AchievementBadgePlayer = new AchievementBadgePlayer();
        }

        public void UpdateAchievement(AchievementPlayer[] achievements)
        {
            if (achievements == null || achievements.Length == 0) return;

            foreach (AchievementPlayer achievement in achievements)
            {
                this.AchievementPlayerDic.Add(achievement.Index.ToString(), achievement);
            }
        }

        public void UpdateAchievementBadgePlayer(AchievementBadgePlayer achievementBadgePlayer)
        {
            if (achievementBadgePlayer == null || achievementBadgePlayer.Equip < 0) return;
            this.AchievementBadgePlayer = achievementBadgePlayer;
        }

        public void DoAchievement(AchievementPlayerIndex achievementIndex, long number = -1, long numberSet = -1){
            if(number > 0){
                StartCoroutine(IEClientDoAchievement(achievementIndex, number, -1, null));
            }
            if(numberSet > 0){
                AchievementPlayer achievementPlayer = this.GetAchievement(achievementIndex);
                if(achievementPlayer != null && achievementPlayer.Process >= numberSet) return;
                StartCoroutine(IEClientDoAchievement(achievementIndex, -1, numberSet, null));
            }
        }

        public void EquipAchievementBadge(int index, Action done = null){
            StartCoroutine(IEEquipAchievementBadge(index, done));
        }

        #endregion

        #region API
        public IEnumerator IEClaimAchievement(AchievementPlayerIndex achievementIndex, Action done = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            jdata["index"] = (int)achievementIndex;
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), Rubik.Config.URL_Config.BASE_API_URL + AchievementConfig.API_Claim_Achievement, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                done?.Invoke();
            });
        }

        public IEnumerator IEGetAchievement(Action done = null){
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), Rubik.Config.URL_Config.BASE_API_URL + AchievementConfig.API_Get_Achievement, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                done?.Invoke();
            });
        }

        public IEnumerator IEClientDoAchievement(AchievementPlayerIndex achievementIndex, long number = 1, long numberSet = -1, Action done = null){
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            jdata["index"] = (int)achievementIndex;
            jdata["number"] = number;
            jdata["numberSet"] = numberSet;
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), Rubik.Config.URL_Config.BASE_API_URL + AchievementConfig.API_Client_Do_Achievement, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                done?.Invoke();
            }, false);
        }

        public IEnumerator IEEquipAchievementBadge(int index, Action done = null){
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            jdata["index"] = index;
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), Rubik.Config.URL_Config.BASE_API_URL + AchievementConfig.API_Get_Achievement_Badge, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                done?.Invoke();
            }, false);
        }

        #endregion

        #region Getter
        public List<AchievementPlayer> GetAchievement(){
            List<AchievementPlayer> achievementPlayers = new List<AchievementPlayer>();
            foreach (AchievementPlayer achievementPlayer in this.AchievementPlayerDic.ToList()){
                AchievementPlayerData achievementPlayerData = this.GetAchievementData(achievementPlayer.Index);
                if(achievementPlayerData == null){
                    NTLog.LogError("GetAchievement: achievementPlayerData is null : " + achievementPlayer.Index.ToString());
                    continue;
                }
                if(!achievementPlayerData.Hidden || achievementPlayer.Process > 0){
                    achievementPlayers.Add(achievementPlayer);
                }
            }
            return achievementPlayers;
        }
        public AchievementPlayer GetAchievement(AchievementPlayerIndex index)
        {
            return this.AchievementPlayerDic.Get(index.ToString());
        }

        public AchievementPlayerData GetAchievementData(AchievementPlayerIndex index)
        {
            return this.AchievementDataDic.Get(index.ToString());
        }

        public int GetAchievementProcess(AchievementPlayerIndex index)
        {
            AchievementPlayer achievementPlayer = this.GetAchievement(index);
            if (achievementPlayer == null) return 0;
            return achievementPlayer.Process;
        }

        public int GetAchievementMaxProcess(AchievementPlayerIndex index)
        {
            AchievementPlayerData achievementPlayerData = this.GetAchievementData(index);
            if (IsAchievementCap(index)) return achievementPlayerData.Requireds[achievementPlayerData.Requireds.Length - 1];
            AchievementPlayer achievementPlayer = this.GetAchievement(index);
            int achive = -1;
            if(achievementPlayer != null){
                achive = achievementPlayer.Achive;
            }
            return achievementPlayerData.Requireds[achive+1];
        }

        public bool IsAchievementCap(AchievementPlayerIndex index)
        {
            AchievementPlayer achievementPlayer = this.GetAchievement(index);
            if (achievementPlayer == null) return true;
            int achive = achievementPlayer.Achive;
            if(achive >= this.GetAchievementData(index).Requireds.Length-1) return true;
            return false;
        }

        public bool IsAchievementCompleted(AchievementPlayerIndex index)
        {
            return this.GetAchievementProcess(index) >= this.GetAchievementMaxProcess(index);
        }

        public AchievementPlayerReward GetAchievementReward(AchievementPlayerIndex index)
        {
            if(this.IsAchievementCap(index)) return this.GetAchievementData(index).Rewards[this.GetAchievementData(index).Rewards.Length-1];
            AchievementPlayer achievementPlayer = this.GetAchievement(index);
            if(achievementPlayer == null) return this.GetAchievementData(index).Rewards[0];
            return this.GetAchievementData(index).Rewards[achievementPlayer.Achive+1];
        }

        public string GetAchievementName(AchievementPlayerIndex index)
        {
            return LeanLocalization.GetTranslationText("Achievement_"+index.ToString());
        }

        public string GetAchievementDesc(AchievementPlayerIndex index)
        {
            string str = LeanLocalization.GetTranslationText("Achievement_"+index.ToString() + "_info","{0}");
            str = string.Format(str, this.GetAchievementMaxProcess(index));
            return str;
        }

        public string WarningNotDoneAchievement(AchievementPlayerIndex index){
            int rest = AchievementManager.Instance.GetAchievementMaxProcess(index) - AchievementManager.Instance.GetAchievementProcess(index);
            if(rest <= 0) rest = 1;
            string str = LeanLocalization.GetTranslationText("Achievement_"+index.ToString()+"_notdone", rest.ToString());
            str = string.Format(str, rest);
            return str;
        }

        public Sprite GetAchiementSprite(AchievementPlayerIndex index)
        {
            AchievementPlayerData achievementPlayerData = this.GetAchievementData(index);
            if(achievementPlayerData == null){
                NTLog.LogError("GetAchiementSprite: achievementPlayerData is null : " + index.ToString());
                 return this.DefaultIcon;
            }
            Sprite sprite = this.IconAchievementDic.Get(achievementPlayerData.ImagePath);
            if(sprite == null){
                NTLog.LogError("GetAchiementSprite: sprite is null : " + achievementPlayerData.ImagePath);
                return this.DefaultIcon;
            }
            return sprite;
        }
        public int CompareTo(AchievementPlayer a, AchievementPlayer b){
            int aValue = 0;
            int bValue = 0;
            if(this.IsAchievementCompleted(a.Index)) aValue = -1;
            if(this.IsAchievementCompleted(b.Index)) bValue = -1;
            if(this.IsAchievementCap(a.Index)) aValue = 1;
            if(this.IsAchievementCap(b.Index)) bValue = 1;
            if(aValue == bValue) return a.Index.CompareTo(b.Index);
            return aValue.CompareTo(bValue);
        }
        
        public List<AchievementBadge> GetAchievementBadge(){
            List<AchievementBadge> achievementBadges = new List<AchievementBadge>();
            foreach (AchievementBadge achievementBadge in this.AchievementBadgeDic.ToList()){
                AchievementPlayer achievementPlayer = this.GetAchievement(achievementBadge.Index);
                if(achievementPlayer != null){
                    achievementBadge.Locked = true;
                    NTLog.LogError("GetAchievementBadge: achievementPlayer is null : " + achievementBadge.Index.ToString());
                    continue;
                }
                if(achievementPlayer.Achive < 0){
                    achievementBadge.Locked = true;
                }
                achievementBadge.Locked = false;
                if(achievementBadge.Locked){
                    AchievementPlayerData achievementPlayerData = this.GetAchievementData(achievementBadge.Index);
                    if(achievementPlayerData.Hidden && achievementPlayer.Process <= 0){
                        continue;
                    }
                }
                achievementBadges.Add(achievementBadge);
            }
            return achievementBadges;
        }

        #endregion
    }
}
