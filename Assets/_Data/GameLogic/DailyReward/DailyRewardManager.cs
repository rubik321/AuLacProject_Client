using NTPackage.Functions;
using Rubik.DataCenter;
using SimpleJSON;
using UnityEngine;
using System.Collections.Generic;
using System;
using Rubik.Manager;
using Rubik.UserDataPlayer;
using Rubik.Config;
using System.Collections;
using NTPackage.EventDispatcher;

namespace Rubik.Myrk.DailyReward
{

    public class DailyRewardConfig
    {
        public const string API_GET_DAILY_REWARD = "/api/2D_GPS/daily_reward/get_data";
        public const string API_CLAIM_DAILY_REWARD = "/api/2D_GPS/daily_reward/claim_reward";
        public const string API_RESET_DAILY_REWARD = "/api/2D_GPS/daily_reward/reset_day";
    }

    public class DailyRewardManager : NTBehaviour
    {

        #region Data Player
        [Header("Data Player")]
        public DailyReward DailyReward = new DailyReward();
        #endregion

        #region Data Game
        [Header("Data Game")]
        public List<DailyRewardData> DailyRewardData = new List<DailyRewardData>();
        #endregion

        public static DailyRewardManager Instance;
        protected override void Awake()
        {
            base.Awake();
            if (DailyRewardManager.Instance != null)
            {
                NTLog.LogError("Only 1 Instance allow");
                return;
            }
            DailyRewardManager.Instance = this;
        }

        #region Function

        public void LoadData()
        {
            JSONNode json = JSONNode.Parse(DataCenterManager.Instance.GetData(DataName.DailyRewardData));
            this.DailyRewardData = new List<DailyRewardData>();
            foreach (JSONNode item in json)
            {
                DailyRewardData dailyRewardData = JsonUtility.FromJson<DailyRewardData>(item.ToString());
                this.DailyRewardData.Add(dailyRewardData);
            }

        }

        public void Logout()
        {
            this.DailyReward = new DailyReward();
        }

        public void UpdateDailyReward(DailyReward dailyReward)
        {
            if (this.DailyReward == null || dailyReward.Version < 1) return;
            this.DailyReward = dailyReward;
            EventListenerManager.instance.PostEvent(EventCode.DailyRewardUpdate);
        }

        [NTButton]
        public void ResetDailyReward(){
            StartCoroutine(this.IEResetDailyReward());
        }

        #endregion

        #region API
        public IEnumerator IEGetDailyReward(Action<DailyReward> done = null)
        {
            JSONNode data = new JSONObject();
            data["userID"] = UserDataManager.Instance.GetUserID();
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(data.ToString(), URL_Config.BASE_API_URL + DailyRewardConfig.API_GET_DAILY_REWARD, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if(apiResponseData.Status == 0) return;
                done?.Invoke(apiResponseData.DailyReward);
            });
        }

        public IEnumerator IEClaimDailyReward(bool isAdv = false, Action done = null){
            JSONNode data = new JSONObject();
            data["userID"] = UserDataManager.Instance.GetUserID();
            data["isAdv"] = isAdv;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(data.ToString(), URL_Config.BASE_API_URL + DailyRewardConfig.API_CLAIM_DAILY_REWARD, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if(apiResponseData.Status == 0) return;
                done?.Invoke();
            });
        }

        public IEnumerator IEResetDailyReward(Action done = null){
            JSONNode data = new JSONObject();
            data["userID"] = UserDataManager.Instance.GetUserID();
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(data.ToString(), URL_Config.BASE_API_URL + DailyRewardConfig.API_RESET_DAILY_REWARD, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if(apiResponseData.Status == 0) return;
                done?.Invoke();
            });
        }
        #endregion

        #region Getter
        public DailyRewardData GetDailyRewardData(int day){
            return this.DailyRewardData.Find(x => x.Day == day);
        }

        public int GetStatusClaim(int day){ // 0: not claim, 1: claimed, 2: not claim but can claim
            if(this.DailyReward.Day > day) return 1;
            if(this.DailyReward.Day == day) return this.DailyReward.Claimed ? 1 : 2;
            return 0;
        }

        public int GetCurrentDay(){
            return this.DailyReward.Day;
        }
        #endregion
    }
}
