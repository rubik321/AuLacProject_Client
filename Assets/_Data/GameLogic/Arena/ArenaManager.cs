using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NTPackage;
using NTPackage.Functions;
using Rubik.BattleEngine;
using Rubik.DataCenter;
using Rubik.ItemPlayer;
using Rubik.Manager;
using Rubik.Myrk.BattleTeam;
using Rubik.ServerGame;
using Rubik.UserDataPlayer;
using Rubik.UserProfile;
using SimpleJSON;
using UnityEngine;

namespace Rubik.Myrk.Arena
{

    public class ArenaConfig
    {
        public const string API_Arena_GetData = "/api/2D_GPS/arena/get_arena_data";
        public const string API_Arena_ResetOpponent = "/api/2D_GPS/arena/reset_opponent";
        public const string API_Arena_AttackOpponent = "/api/2D_GPS/arena/attack_opponent";
        public const string API_Arena_GetArenaRankAll = "/api/2D_GPS/arena/get_arena_rank_all";
    }

    public class ArenaManager : NTBehaviour
    {

        #region Data Player
        public ArenaResponse ArenaResponse;
        public UserRankResponse ArenaRankAll;
        #endregion

        #region Data Game
        public List<RankingData> RankingData;
        public ArenaData ArenaData;
        #endregion

        public static ArenaManager Instance;
        protected override void Awake()
        {
            base.Awake();
            if (ArenaManager.Instance != null)
            {
                NTLog.LogError("Only 1 Instance allow");
                return;
            }
            ArenaManager.Instance = this;
        }

        #region Function
        public void LoadData()
        {
            this.RankingData = new List<RankingData>();
            JSONNode jdata = JSONNode.Parse(DataCenterManager.Instance.GetData(DataName.ArenaRankingData));
            foreach (JSONNode item in jdata)
            {
                RankingData rankingData = JsonUtility.FromJson<RankingData>(item.ToString());
                this.RankingData.Add(rankingData);
            }
            this.ArenaData = JsonUtility.FromJson<ArenaData>(DataCenterManager.Instance.GetData(DataName.ArenaData));
        }

        public void UpdateArenaResponse(ArenaResponse arenaResponse)
        {
            this.ArenaResponse = arenaResponse;
        }
        public void UpdateArenaRankAll(UserRankResponse arenaRankAll)
        {
            this.ArenaRankAll = arenaRankAll;
        }
        public void UpdateArenaBattleResult(ArenaBattleResult arenaBattleResult)
        {
            BattleEngineController.Instance.SetArenaBattleResult(arenaBattleResult);
        }

        public void GetArenaData(Action<ArenaResponse> callback = null)
        {
            StartCoroutine(this.IEGetArenaData(() =>
            {
                callback?.Invoke(this.ArenaResponse);
            }));
        }

        public void ResetOpponent(bool isAdv, bool isGem, Action callback = null)
        {
            if (isGem)
            {
                if (ItemDataManager.Instance.GetItem(ItemType.Gem).Amount < this.ArenaData.ResetCost.Amount)
                {
                    ItemDataManager.Instance.ShowDontEnoughItem(ItemType.Gem);
                    return;
                }
            }
            StartCoroutine(this.IEResetOpponent(isAdv, isGem, () =>
            {
                callback?.Invoke();
            }));
        }

        public void AttackOpponent(string opponentID, bool isAdv, Action callback = null){
            StartCoroutine(this.IEAttackOpponent(opponentID, isAdv, () =>
            {
                callback?.Invoke();
            }));
        }

        public long LastTimeGetArenaRankAll = 0;
        public void GetArenaRankAll(Action<UserRankResponse> callback = null){
            // Reset 1 minutes
            if(this.LastTimeGetArenaRankAll > ServerManager.Instance.GetTimeServer() - 60){
                callback?.Invoke(this.ArenaRankAll);
                return;
            }
            this.LastTimeGetArenaRankAll = ServerManager.Instance.GetTimeServer();
            StartCoroutine(this.IEGetArenaRankAll(callback));
            {
                this.LastTimeGetArenaRankAll = ServerManager.Instance.GetTimeServer();
                callback?.Invoke(this.ArenaRankAll);
            };
        }

        #endregion

        #region API
        public IEnumerator IEGetArenaData(Action callback = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(jdata.ToString(), Rubik.Config.URL_Config.BASE_API_URL + ArenaConfig.API_Arena_GetData, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                callback?.Invoke();
            });

        }

        public IEnumerator IEGetArenaRankAll(Action<UserRankResponse> callback = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            jdata["groupServer"] = ServerGameManager.Instance.GetPlayerGroupServer();
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(jdata.ToString(), Rubik.Config.URL_Config.BASE_API_URL + ArenaConfig.API_Arena_GetArenaRankAll, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                callback?.Invoke(this.ArenaRankAll);
            });
        }

        public IEnumerator IEResetOpponent(bool isAdv, bool isGem, Action callback = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            jdata["isAdv"] = isAdv;
            jdata["isGem"] = isGem;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(jdata.ToString(), Rubik.Config.URL_Config.BASE_API_URL + ArenaConfig.API_Arena_ResetOpponent, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                callback?.Invoke();
            });
        }

        public IEnumerator IEAttackOpponent(string opponentID, bool isAdv, Action callback = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            jdata["team"] = JSONNode.Parse(JsonUtility.ToJson(BattleTeamManager.Instance.GetBattleTeamDataSelected().ToShortTeam()));
            jdata["opponentID"] = opponentID;
            jdata["isAdv"] = isAdv;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(jdata.ToString(), Rubik.Config.URL_Config.BASE_API_URL + ArenaConfig.API_Arena_AttackOpponent, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                callback?.Invoke();
            });
        }
        #endregion

        #region Getter
        public long GetArenaTicket()
        {
            long time = ServerManager.Instance.GetTimeServer() - this.ArenaResponse.ArenaTicketRecover;
            long gainTicket = (long)(time / this.ArenaData.ArenaTicketRecover);
            if (this.ArenaResponse.ArenaTicket + gainTicket >= this.ArenaData.MaxArenaTicket)
            {
                return this.ArenaData.MaxArenaTicket;
            }
            else
            {
                return this.ArenaResponse.ArenaTicket + gainTicket;
            }
        }

        public long GetArenaTicketRecovery()
        {
            long time = ServerManager.Instance.GetTimeServer() - this.ArenaResponse.ArenaTicketRecover;
            long gainTicket = (long)(time / this.ArenaData.ArenaTicketRecover);
            if (this.ArenaResponse.ArenaTicket + gainTicket >= this.ArenaData.MaxArenaTicket)
            {
                return -1;
            }
            else
            {
                return time - gainTicket * this.ArenaData.ArenaTicketRecover;
            }
        }

        public (long ticket, long maxTicket) GetArenaTicketStatus()
        {
            long ticket = this.GetArenaTicket();
            long maxTicket = this.ArenaData.MaxArenaTicket;
            return (ticket, maxTicket);
        }

        public List<OpponentData> GetOpponents()
        {
            return this.ArenaResponse.Opponents.ToList();
        }

        public long GetWinScore(long score)
        {
            if (score > this.ArenaResponse.Score)
            {
                return this.ArenaData.WinHigherRank;
            }
            return this.ArenaData.WinLowerRank;
        }

        public RankingType GetRankingTypeByScore(long score)
        {
            if(score <= 1) return RankingType.BronzeI;
            foreach (RankingData rankingData in this.RankingData)
            {
                if (score >= rankingData.MinPoint && score <= rankingData.MaxPoint)
                {
                    return rankingData.Type;
                }
            }
            return RankingType.PlatinumIII;
        }

        public long GetFreeReset(){
            long time = this.ArenaResponse.LastTimeReset + this.ArenaData.FreeReset;
            if(time > ServerManager.Instance.GetTimeServer()){
                return time - ServerManager.Instance.GetTimeServer();
            }
            return -1;
        }

        public int GetAdvResetRemaining(){
            return UserDataManager.Instance.GetRemainDailyAdvLimit(AdvLimitDataConfig.ArenaReset);
        }

        public int GetAdvAttackRemaining(){
            return UserDataManager.Instance.GetRemainDailyAdvLimit(AdvLimitDataConfig.ArenaAttack);
        }

        public (int ticket, int maxTicket, int recover) GetUserArenaTicketItem()
        {
            ArenaTicketResponse arenaTicketResponse = this.GetUserArenaTicket(this.ArenaResponse.ArenaTicket, this.ArenaResponse.ArenaTicketRecover);
            int ticket = (int)arenaTicketResponse.Ticket;
            int maxTicket = (int)this.ArenaData.MaxArenaTicket;
            int recover = (int)(arenaTicketResponse.Recover + this.ArenaData.ArenaTicketRecover - ServerManager.Instance.GetTimeServer());
            if (ticket >= maxTicket)
            {
                recover = -1;
            }
            return (ticket, maxTicket, recover);
        }

        public ArenaTicketResponse GetUserArenaTicket(long ticket, long recover)
        {
            var arenaTicketResponse = new ArenaTicketResponse();
            arenaTicketResponse.Ticket = ticket;
            arenaTicketResponse.Recover = recover;
            int ticket_rec = Mathf.FloorToInt((ServerManager.Instance.GetTimeServer() - recover) / this.ArenaData.ArenaTicketRecover);
            if (ticket_rec < 1)
            {
                return arenaTicketResponse;
            }

            if (ticket + ticket_rec > this.ArenaData.MaxArenaTicket)
            {
                ticket_rec = (int)this.ArenaData.MaxArenaTicket - (int)ticket;
                if (ticket_rec < 1)
                {
                    ticket_rec = 0;
                }
                arenaTicketResponse.Recover = ServerManager.Instance.GetTimeServer();
            }
            else
            {
                arenaTicketResponse.Recover = recover + ticket_rec * this.ArenaData.ArenaTicketRecover;
            }
            arenaTicketResponse.Ticket += ticket_rec;
            return arenaTicketResponse;
        }

        #endregion
    }
}
