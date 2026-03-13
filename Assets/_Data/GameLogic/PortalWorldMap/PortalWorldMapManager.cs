using System;
using System.Collections;
using System.Collections.Generic;
using NTPackage.Functions;
using Rubik.CardPlayer;
using Rubik.Config;
using Rubik.Manager;
using SimpleJSON;
using UnityEngine;

namespace Rubik.Myrk.Portal
{
    using NTPackage.UI;
    using Rubik.DataCenter;
    using Rubik.Myrk.BattleTeam;
    using Rubik.Server;
    using Rubik.Myrk.Monster;
    using Sirenix.OdinInspector;
    using Spine.Unity;
    using UserDataPlayer;
    using Rubik.ServerGame;
    using Rubik.DataType;
    using Rubik.MsgDelivery;
    using Rubik.UserProfile;
    using NTPackage;
    using NTPackage.EventDispatcher;
    using System.Linq;

    public class PortalWorldMapConfig
    {
        public const string API_Attack = "/api/2D_GPS/portal_world_map/attack_portal";
        public const string API_UnlockPortal = "/api/2D_GPS/portal_world_map/unlock_portal";
        public const string API_CheckPortalHistory = "/api/2D_GPS/portal_world_map/check_portal_history";
        public const string API_GetPortalAttack = "/api/2D_GPS/portal_world_map/get_portal_attack";
        public const string API_GetRandomPortal = "/api/2D_GPS/portal_world_map/random_portal";
    }

    public class PortalWorldMapManager : NTBehaviour
    {
        #region Player Data
        public PortalBossAttackRespone PortalBossAttackRespone;
        public List<PortalHistory> PortalHistories;
        public PortalRandomResponse PortalRandomResponse;
        #endregion

        #region Game Data
        public BossPortalData BossPortalData;
        public NTDictionary<string, PortalAttackData> BossAttackDataDic;
        public NTDictionary<string, PortalBossRank> PortalBossRankDic;
        #endregion

        #region Resource
        public List<SkeletonGraphic> BossSkeletonGraphic;
        public List<Sprite> GateSprite;
        #endregion

        public Coroutine CorAutoCheckPortalHistory;

        public static PortalWorldMapManager Instance;
        protected override void Awake()
        {
            base.Awake();
            if (PortalWorldMapManager.Instance != null)
            {
                NTLog.LogError("Only 1 Instance allow");
                return;
            }
            PortalWorldMapManager.Instance = this;
        }

        protected override void Start()
        {
            if (this.CorAutoCheckPortalHistory != null) StopCoroutine(this.CorAutoCheckPortalHistory);
            this.CorAutoCheckPortalHistory = StartCoroutine(this.IEAutoCheckPortalHistory());
        }

        #region Test
        [NTButton]
        public void TestGetPortalAttackData()
        {
            this.SendMsgGetPortalAttackData(0);
        }
        [NTButton]
        public void TestGetPortalBossRank()
        {
            this.SendMsgGetPortalBossRank(0);
        }
        #endregion

        #region Function
        public void LoadData()
        {
            JSONNode jdata = JSONNode.Parse(DataCenterManager.Instance.GetData(DataName.BossPortalData));
            this.BossPortalData = JsonUtility.FromJson<BossPortalData>(jdata.ToString());
        }

        public void Logout()
        {
            this.PortalBossAttackRespone = new PortalBossAttackRespone();
            this.PortalHistories = new List<PortalHistory>();
            this.PortalRandomResponse = new PortalRandomResponse();
        }

        public void UpdatePortalBossAttackRespone(PortalBossAttackRespone portalBossAttackRespone)
        {
            if (portalBossAttackRespone == null || portalBossAttackRespone.LastTimeRecovery == 0) return;
            this.PortalBossAttackRespone = portalBossAttackRespone;
        }

        public void UpdatePortalBossRank(List<PortalBossRank> portalBossRanks)
        {
            if (portalBossRanks == null || portalBossRanks.Count == 0) return;
            if (this.PortalBossRankDic == null) this.PortalBossRankDic = new NTDictionary<string, PortalBossRank>();
            foreach (PortalBossRank portalBossRank in portalBossRanks)
            {
                this.PortalBossRankDic.Add(this.GetKeyPortalBossRank(portalBossRank.PointID, portalBossRank.GroupServer), portalBossRank);
                EventListenerManager.instance.PostEvent(EventCode.UpdatePortalBossRank, portalBossRank.PointID);
            }
        }

        public void UpdatePortalAttackData(List<PortalAttackData> portalAttackDatas)
        {
            if (portalAttackDatas == null || portalAttackDatas.Count == 0) return;
            if (this.BossAttackDataDic == null) this.BossAttackDataDic = new NTDictionary<string, PortalAttackData>();
            foreach (PortalAttackData portalAttackData in portalAttackDatas)
            {
                portalAttackData.LastGetTime = ServerManager.Instance.GetTimeServer();
                this.BossAttackDataDic.Add(this.GetKeyPortalAttackData(portalAttackData.PointID, portalAttackData.GroupServer), portalAttackData);
                EventListenerManager.instance.PostEvent(EventCode.UpdatePortalAttackData, portalAttackData.PointID);
            }
        }

        public void UpdatePortalHistory(PortalHistoryRespone portalHistories)
        {
            if (portalHistories == null || portalHistories._id == null || portalHistories._id.Length == 0) return;
            this.PortalHistories = portalHistories.PortalHistories.ToList();
        }

        public void UpdatePortalRandom(PortalRandomResponse portalRandomResponse)
        {
            if (portalRandomResponse == null || portalRandomResponse.GeoPoints == null || portalRandomResponse.GeoPoints.Count == 0) return;
            this.PortalRandomResponse = portalRandomResponse;
        }

        [NTButton]
        public void GetRandomPortal()
        {
            StartCoroutine(this.IEGetRandomPortal(() =>
            {
                this.UpdatePortalRandom(this.PortalRandomResponse);
            }));
        }

        public void GetPortalAttackAPI(long pointID, Action<PortalAttackData> done = null)
        {
            StartCoroutine(this.IEGetPortalAttack(pointID, () =>
            {
                done?.Invoke(PortalWorldMapManager.Instance.GetPortalAttackData(pointID));
            }));
        }

        public IEnumerator IEAutoCheckPortalHistory(Action done = null)
        {
            while (true)
            {
                yield return new WaitForSeconds(5);
                bool isCheck = false;
                if (this.PortalHistories == null || this.PortalHistories.Count == 0) continue;
                foreach (PortalHistory portalHistory in this.PortalHistories)
                {
                    PortalAttackData portalAttackData = this.GetPortalAttackData(portalHistory.PointID);
                    if (portalAttackData == null || portalAttackData.LastGetTime + 60 < ServerManager.Instance.GetTimeServer())
                    {
                        this.SendMsgGetPortalAttackData(portalHistory.PointID);
                    }
                    else
                    {
                        if (portalAttackData.HP <= 0 || portalAttackData.Version != portalHistory.Version || portalAttackData.OpenTime + this.BossPortalData.TimeAttack + this.BossPortalData.TimeClose < ServerManager.Instance.GetTimeServer())
                        {
                            isCheck = true;
                        }
                    }
                }
                if (isCheck)
                {
                    yield return this.IECheckPortalHistory();
                }
            }
        }

        public void SendMsgGetPortalBossRank(long pointID)
        {
            PortalBossRank portalBossRank = this.GetPortalBossRank(pointID);
            if (portalBossRank != null && portalBossRank.LastGetTime + 3 > ServerManager.Instance.GetTimeServer()) return;
            PortalMsg portalMsg = new PortalMsg();
            portalMsg.Type = PortalMsgType.GetPortalBossRank;
            SendMsgGetPortalData sendMsgGetPortalData = new SendMsgGetPortalData();
            sendMsgGetPortalData.PointID = pointID;
            sendMsgGetPortalData.GroupServer = ServerGameManager.Instance.GetPlayerGroupServer();
            sendMsgGetPortalData.UserID = UserDataManager.Instance.GetUserID();
            portalMsg.Data = JsonUtility.ToJson(sendMsgGetPortalData);
            MsgDeliveryRoom.Instance.SendMessage<PortalMsg>(MsgDeliveryKey.PortalMsg, portalMsg);
        }

        public void SendMsgGetPortalAttackData(long pointID)
        {
            PortalAttackData portalAttackData = this.GetPortalAttackData(pointID);
            if (portalAttackData != null && portalAttackData.LastGetTime + 3 > ServerManager.Instance.GetTimeServer()) return;
            PortalMsg portalMsg = new PortalMsg();
            portalMsg.Type = PortalMsgType.GetPortalAttackData;
            SendMsgGetPortalAttackData sendMsgGetPortalAttackData = new SendMsgGetPortalAttackData();
            sendMsgGetPortalAttackData.PointID = pointID;
            sendMsgGetPortalAttackData.GroupServer = ServerGameManager.Instance.GetPlayerGroupServer();
            portalMsg.Data = JsonUtility.ToJson(sendMsgGetPortalAttackData);
            MsgDeliveryRoom.Instance.SendMessage<PortalMsg>(MsgDeliveryKey.PortalMsg, portalMsg);
        }


        public void OnPortalMsg(PortalMsg portalMsg)
        {
            NTLog.LogMessage("OnPortalMsg: " + JsonUtility.ToJson(portalMsg));
            switch (portalMsg.Type)
            {
                case PortalMsgType.GetPortalBossRank:
                    this.OnGetPortalBossRank(portalMsg.Data);
                    break;
                case PortalMsgType.GetPortalAttackData:
                    this.OnGetPortalAttackData(portalMsg.Data);
                    break;
                default:
                    break;
            }
        }

        public void OnGetPortalBossRank(string data)
        {
            if (data == null || data == "") return;
            if (this.PortalBossRankDic == null) this.PortalBossRankDic = new NTDictionary<string, PortalBossRank>();
            PortalBossRank portalBossRank = JsonUtility.FromJson<PortalBossRank>(data);
            portalBossRank.LastGetTime = ServerManager.Instance.GetTimeServer();
            this.UpdatePortalBossRank(new List<PortalBossRank>() { portalBossRank });
        }

        public void OnGetPortalAttackData(string data)
        {
            if (data == null || data == "") return;
            if (this.BossAttackDataDic == null) this.BossAttackDataDic = new NTDictionary<string, PortalAttackData>();
            PortalAttackData portalAttackData = JsonUtility.FromJson<PortalAttackData>(data);
            this.UpdatePortalAttackData(new List<PortalAttackData>() { portalAttackData });
        }

        #endregion

        #region API
        public IEnumerator IEAttack(int level, long pointID, Action<BattleResult> done = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            jdata["team"] = JSONNode.Parse(JsonUtility.ToJson(BattleTeamManager.Instance.GetBattleTeamDataSelected().ToShortTeam()));
            jdata["groupServer"] = ServerGameManager.Instance.GetPlayerGroupServer();
            jdata["pointID"] = pointID;
            jdata["cardPlayerIndex"] = (int)this.GetBossCardPlayerIndex(pointID);

            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + PortalWorldMapConfig.API_Attack, (data) =>
            {
                APIResponseData aPIResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (aPIResponseData.Status == 0) return;
                done?.Invoke(aPIResponseData.BattleResult);
            });
        }

        public IEnumerator IEUnlockPortal(int level, long pointID, Action done = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            jdata["level"] = level;
            jdata["groupServer"] = ServerGameManager.Instance.GetPlayerGroupServer();
            jdata["pointID"] = pointID;

            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + PortalWorldMapConfig.API_UnlockPortal, (data) =>
            {
                APIResponseData aPIResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (aPIResponseData.Status == 0) return;
                done?.Invoke();
            });
        }

        public IEnumerator IECheckPortalHistory(Action done = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            jdata["groupServer"] = ServerGameManager.Instance.GetPlayerGroupServer();

            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + PortalWorldMapConfig.API_CheckPortalHistory, (data) =>
            {
                APIResponseData aPIResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (aPIResponseData.Status == 0) return;
                done?.Invoke();
            }, false);
        }

        public IEnumerator IEGetPortalAttack(long pointID, Action done = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["pointID"] = pointID;
            jdata["groupServer"] = ServerGameManager.Instance.GetPlayerGroupServer();

            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + PortalWorldMapConfig.API_GetPortalAttack, (data) =>
            {
                APIResponseData aPIResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (aPIResponseData.Status == 0) return;
                done?.Invoke();
            });
        }

        public IEnumerator IEGetRandomPortal(Action done = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            jdata["amount"] = 5;

            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + PortalWorldMapConfig.API_GetRandomPortal, (data) =>
            {
                APIResponseData aPIResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                done?.Invoke();
            });
        }

        #endregion

        #region Getter
        public SkeletonGraphic GetBossSkeletonGraphic(int pointID)
        {
            int index = pointID % this.BossSkeletonGraphic.Count;
            SkeletonGraphic skeletonGraphic = ObjectPoolingManager.Instance.PullObjectFromPooling<SkeletonGraphic>(ObjectPoolingConfig.BossSkeletonGraphic + index);
            if (skeletonGraphic == null)
            {
                skeletonGraphic = Instantiate(this.BossSkeletonGraphic[index]);
            }
            skeletonGraphic.name = ObjectPoolingConfig.BossSkeletonGraphic + index;
            skeletonGraphic.gameObject.SetActive(true);
            return skeletonGraphic;
        }

        public Sprite GetGateSprite(OriginType originType)
        {
            int index = (int)originType;
            return this.GateSprite[index % this.GateSprite.Count];
        }

        public OriginType GetOriginType(int pointID)
        {
            return (OriginType)(pointID % Enum.GetValues(typeof(OriginType)).Length);
        }

        public RankBossReward[] GetRankBossReward()
        {
            return this.BossPortalData.RankBossReward;
        }

        public RankBossReward GetRankBossRewardByRank(int rank)
        {
            foreach (RankBossReward rankBossReward in this.BossPortalData.RankBossReward)
            {
                if (rank >= rankBossReward.RankMin && rank <= rankBossReward.RankMax)
                {
                    return rankBossReward;
                }
            }
            return this.BossPortalData.RankBossReward[this.BossPortalData.RankBossReward.Length - 1];
        }

        public float GetRewardMultiplier(int level)
        {
            foreach (BossData bossData in this.BossPortalData.BossData)
            {
                if (bossData.Level == level)
                {
                    return bossData.RewardMultiplier;
                }
            }
            return 1;
        }

        public float GetFixRewardMultiplier(int level)
        {
            foreach (BossData bossData in this.BossPortalData.BossData)
            {
                if (bossData.Level == level)
                {
                    return bossData.FixRewardMultiplier;
                }
            }
            return 1;
        }

        public int GetHighestLevelOpened()
        {
            return (int)this.PortalBossAttackRespone.HighestLevelPortalBoss;
        }

        public int GetHighestLevelPortal()
        {
            return this.BossPortalData.BossData[BossPortalData.BossData.Length - 1].Level;
        }

        public string GetKeyPortalBossRank(long pointID, int server)
        {
            return pointID + "_" + server;
        }

        public string GetKeyPortalAttackData(long pointID, int server)
        {
            return pointID + "_" + server;
        }

        public long GetTimeRecoveryAttackPortal()
        {
            (long amount, long lastTimeRecovery) = this.RecoveryAttack();
            if (amount >= this.BossPortalData.MaxTimeAttack) return -1;
            long remain = this.BossPortalData.TimeRecoveryAttack - (ServerManager.Instance.GetTimeServer() - lastTimeRecovery);
            return remain;
        }

        public long GetTimeResetPortal(long timeOpen)
        {
            long res = timeOpen + this.BossPortalData.TimeAttack + this.BossPortalData.TimeClose - ServerManager.Instance.GetTimeServer();
            if (res < 0) return 0;
            return res;
        }

        public long GetRemainingTimeAttackPortal(long timeOpen)
        {
            long res = timeOpen + this.BossPortalData.TimeAttack - ServerManager.Instance.GetTimeServer();
            if (res < 0) return 0;
            return res;
        }

        public PortalAttackData GetPortalAttackData(long pointID)
        {
            return this.BossAttackDataDic.Get(this.GetKeyPortalAttackData(pointID, ServerGameManager.Instance.GetPlayerGroupServer()));
        }

        public PortalBossRank GetPortalBossRank(long pointID)
        {
            return this.PortalBossRankDic.Get(this.GetKeyPortalBossRank(pointID, ServerGameManager.Instance.GetPlayerGroupServer()));
        }

        public int GetPortalAttack(int level)
        {
            foreach (BossData bossData in this.BossPortalData.BossData)
            {
                if (bossData.Level == level)
                {
                    return bossData.ATK;
                }
            }
            return 1;
        }

        public long GetPortalHp(int level)
        {
            foreach (BossData bossData in this.BossPortalData.BossData)
            {
                if (bossData.Level == level)
                {
                    return bossData.HP;
                }
            }
            return 1;
        }

        public CardPlayerIndex GetBossCardPlayerIndex(long pointID)
        {
            return CardPlayerConfig.PortalBossCardPlayerIndex[(int)(pointID % (long)CardPlayerConfig.PortalBossCardPlayerIndex.Count)];
        }

        public int GetKeyPortalAmount()
        {
            int max = (int)BossPortalData.MaxTimeUnlock;
            int remain = max - (int)this.PortalBossAttackRespone.AmountPortalBossUnlock;
            return remain;
        }

        public string GetKeyPortalRemain()
        {
            int max = (int)BossPortalData.MaxTimeUnlock;
            int remain = max - (int)this.PortalBossAttackRespone.AmountPortalBossUnlock;
            if (remain < 0) remain = 0;
            return remain + "/" + max;
        }

        public string GetPortalAttackRemain(){
            (long amount, long lastTimeRecovery) = this.RecoveryAttack();
            return amount + "/" + this.BossPortalData.MaxTimeAttack;
        }

        public int GetCostUnlockPortal()
        {
            return (int)this.BossPortalData.PortalKey;
        }

        public (long amount, long lastTimeRecovery) RecoveryAttack()
        {
            long currentAmount = this.PortalBossAttackRespone.AmountPortalBossAttack;
            long lastTimeRecovery = this.PortalBossAttackRespone.LastTimeRecovery;
            var gain = (ServerManager.Instance.GetTimeServer() - lastTimeRecovery) / this.BossPortalData.TimeRecoveryAttack;
            currentAmount += gain;
            if (currentAmount >= this.BossPortalData.MaxTimeAttack)
            {
                currentAmount = this.BossPortalData.MaxTimeAttack;
                lastTimeRecovery = ServerManager.Instance.GetTimeServer();
            }
            else
            {
                lastTimeRecovery += this.BossPortalData.TimeRecoveryAttack * gain;
            }
            return (currentAmount, lastTimeRecovery);
        }

        #endregion
    }
}

