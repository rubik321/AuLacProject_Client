using UnityEngine;
using Rubik.Myrk.PlayerChest;
using NTPackage.Functions;
using NTPackage;
using Rubik.ItemPlayer;
using SimpleJSON;
using Rubik.DataCenter;
using System;
using Rubik.UserDataPlayer;
using Rubik.Manager;
using System.Collections;
using Rubik.Config;
using System.Collections.Generic;
using System.Linq;

namespace Rubik.Myrk.PlayerChest
{
    public class PlayerChestConfig
    {
        public const string API_UNLOCK_CHEST_SLOT = "/api/2D_GPS/player_chest/unlock_chest_slot";
        public const string API_UNLOCK_CHEST = "/api/2D_GPS/player_chest/unlock_chest";
        public const string API_OPEN_CHEST = "/api/2D_GPS/player_chest/open_chest";
        public const string API_DECREASE_TIME_CHEST = "/api/2D_GPS/player_chest/decrease_time_chest";
    }

    public class PlayerChestManager : NTBehaviour
    {
        #region User Data
        [Header("User Data")]
        public PlayerChestResponse PlayerChest;
        #endregion

        #region Game Data
        [Header("Game Data")]
        public NTDictionary<ItemType, PlayerChestData> PlayerChestData;
        public List<PlayerChestSlotData> PlayerChestSlotData;
        public List<PlayerChestLevelData> PlayerChestLevelDatas;
        public PlayerChestDecreaseTime PlayerChestDecreaseTime;
        #endregion

        public static PlayerChestManager Instance;
        protected override void Awake()
        {
            base.Awake();
            if (PlayerChestManager.Instance != null)
            {
                NTLog.LogError("Only 1 Instance allow");
                return;
            }
            PlayerChestManager.Instance = this;
        }

        #region Function
        public void LoadData()
        {
            this.PlayerChestData = new NTDictionary<ItemType, PlayerChestData>();
            JSONNode data = JSONNode.Parse(DataCenterManager.Instance.GetData(DataName.PlayerChestData));
            foreach (JSONNode item in data)
            {
                PlayerChestData playerChestData = JsonUtility.FromJson<PlayerChestData>(item.ToString());
                this.PlayerChestData.Add(playerChestData.Type, playerChestData);
            }

            this.PlayerChestSlotData = new List<PlayerChestSlotData>();
            data = JSONNode.Parse(DataCenterManager.Instance.GetData(DataName.PlayerChestSlotData));
            foreach (JSONNode item in data)
            {
                PlayerChestSlotData playerChestSlotData = JsonUtility.FromJson<PlayerChestSlotData>(item.ToString());
                this.PlayerChestSlotData.Add(playerChestSlotData);
            }

            this.PlayerChestLevelDatas = new List<PlayerChestLevelData>();
            data = JSONNode.Parse(DataCenterManager.Instance.GetData(DataName.PlayerChestLevelData));
            foreach (JSONNode item in data)
            {
                PlayerChestLevelData playerChestLevelData = JsonUtility.FromJson<PlayerChestLevelData>(item.ToString());
                this.PlayerChestLevelDatas.Add(playerChestLevelData);
            }

            this.PlayerChestDecreaseTime = JsonUtility.FromJson<PlayerChestDecreaseTime>(DataCenterManager.Instance.GetData(DataName.PlayerChestDecreaseTimeData));
        }

        public void Logout()
        {
            this.PlayerChest = new PlayerChestResponse();
        }

        public void UpdatePlayerChest(PlayerChestResponse playerChestResponse)
        {
            this.PlayerChest = playerChestResponse;
        }

        #endregion

        #region API
        public IEnumerator IEUnlockChestSlot(int index, Action done = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            jdata["index"] = index;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + PlayerChestConfig.API_UNLOCK_CHEST_SLOT, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                done?.Invoke();
            });
        }

        public IEnumerator IEUnlockChest(int index, ItemType type, Action done = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            jdata["index"] = index;
            jdata["type"] = (int)type;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + PlayerChestConfig.API_UNLOCK_CHEST, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                done?.Invoke();
            });
        }

        public IEnumerator IEOpenChest(int index, Action done = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            jdata["index"] = index;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + PlayerChestConfig.API_OPEN_CHEST, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                done?.Invoke();
            });
        }

        public IEnumerator IEDecreaseTimeChest(int index, int gem, bool isAdv, Action done = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            jdata["index"] = index;
            jdata["gem"] = gem;
            jdata["isAdv"] = isAdv;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + PlayerChestConfig.API_DECREASE_TIME_CHEST, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                done?.Invoke();
            });
        }
        #endregion

        #region Getter

        public PlayerChestData GetPlayerChestData(ItemType chestType)
        {
            PlayerChestData playerChestData = this.PlayerChestData.Get(chestType);
            if (playerChestData == null)
            {
                NTLog.LogError("PlayerChestData not found : " + chestType.ToString());
            }
            return playerChestData;
        }

        public bool IsNotEnoughLevel(int index)
        {
            PlayerChestSlotData playerChestSlotData = this.PlayerChestSlotData.Find(x => x.Index == index);
            if (playerChestSlotData == null)
            {
                return false;
            }
            return playerChestSlotData.RequireUnlock.Level > this.PlayerChest.Level;
        }

        public bool IsUnlockFree(int index)
        {
            PlayerChestSlotData playerChestSlotData = this.PlayerChestSlotData.Find(x => x.Index == index);
            if (playerChestSlotData == null)
            {
                return false;
            }
            if (playerChestSlotData.RequireUnlock.Level <= this.PlayerChest.Level && playerChestSlotData.RequireUnlock.Price.Length == 0)
            {
                return true;
            }
            return false;
        }

        public bool IsSlotLock(int index)
        {
            PlayerChestSlot playerChestSlot = this.PlayerChest.PlayerChestSlot.ToList().Find(x => x.Index == index);
            if (playerChestSlot == null)
            {
                return true;
            }
            return false;
        }

        public (int Level, long Exp, long ExpMax) GetLevelData()
        {
            int level = this.PlayerChest.Level;
            long exp = this.PlayerChest.Exp;
            long expMax = this.PlayerChestLevelDatas.Find(x => x.Level == level).Exp;
            return (level, exp, expMax);
        }

        public long GetAmountRewardByLevel(long amount)
        {
            PlayerChestLevelData playerChestLevelData = this.PlayerChestLevelDatas.Find(x => x.Level == this.PlayerChest.Level);
            if (playerChestLevelData == null)
            {
                return amount;
            }
            long inc = (long)(playerChestLevelData.Inc * amount);
            amount += inc;
            return amount;
        }

        public (long gem, bool isAdv) GetDecreaseTime(int index)
        {
            PlayerChestSlot playerChestSlot = this.PlayerChest.PlayerChestSlot.ToList().Find(x => x.Index == index);
            if (playerChestSlot == null)
            {
                return (0, false);
            }
            if (playerChestSlot.TimeClaim > ServerManager.Instance.GetTimeServer())
            {
                long gem = (playerChestSlot.TimeClaim - ServerManager.Instance.GetTimeServer()) / this.PlayerChestDecreaseTime.GemDecreaseTime;
                if (LevelPlayAds.Instance.IsCanShowAds())
                {
                    if (playerChestSlot.TimeAdv < this.PlayerChestDecreaseTime.LimitTimeAdv)
                    {
                        return (gem, true);
                    }
                    return (gem, false);
                }
                return (gem, false);
            }
            return (0, false);
        }

        public int GetDecreaseTimeAdv(){
            return this.PlayerChestDecreaseTime.AdvDecreaseTime;
        }

        #endregion
    }
}
