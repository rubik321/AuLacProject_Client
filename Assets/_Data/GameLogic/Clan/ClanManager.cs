using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using NTPackage;
using NTPackage.EventDispatcher;
using NTPackage.Functions;
using Rubik._2DGPS.Chat;
using Rubik.CardPlayer;
using Rubik.Config;
using Rubik.DataCenter;
using Rubik.ItemPlayer;
using Rubik.Manager;
using Rubik.MsgDelivery;
using Rubik.Myrk.BattleTeam;
using Rubik.ServerGame;
using Rubik.UserDataPlayer;
using Rubik.UserProfile;
using SimpleJSON;
using UnityEngine;

namespace Rubik.Myrk.Clan
{
    using Lean.Localization;
    using Rubik.ItemPlayer;
    using Rubik.UI;

    public class ClanConfig
    {
        public const string API_CREATE_CLAN = "/api/2D_GPS/clan/createClan";
        public const string API_GET_TOP_CLAN_LIST = "/api/2D_GPS/clan/getTopClanList";
        public const string API_JOIN_CLAN_REQUEST = "/api/2D_GPS/clan/joinClanRequest";
        public const string API_LEAVE_CLAN_REQUEST = "/api/2D_GPS/clan/leaveClanRequest";
        public const string API_GET_CLAN = "/api/2D_GPS/clan/getClan";
        public const string API_GET_LIST_CLAN_MEMBER_INFO = "/api/2D_GPS/clan/getListClanMemberInfo";
        public const string API_GET_PLAYER_CLAN = "/api/2D_GPS/clan/getPlayerClan";
        public const string API_GET_CLAN_REQUEST_LIST = "/api/2D_GPS/clan/getClanRequestList";
        public const string API_ACCEPT_CLAN_REQUEST = "/api/2D_GPS/clan/acceptClanRequest";
        public const string API_REJECT_CLAN_REQUEST = "/api/2D_GPS/clan/rejectClanRequest";
        public const string API_PROMOTE_CLAN_MEMBER = "/api/2D_GPS/clan/promoteClanMember";
        public const string API_REMOVE_CLAN_MEMBER = "/api/2D_GPS/clan/removeClanMember";
        public const string API_DELETE_CLAN = "/api/2D_GPS/clan/deleteClan";
        public const string API_DONATE_FUND = "/api/2D_GPS/clan/donateFund";
        public const string API_FIND_CLAN = "/api/2D_GPS/clan/findClan";
        public const string API_CHANGE_CLAN_LEADER = "/api/2D_GPS/clan/changeClanLeader";
        public const string API_EDIT_SLOGAN = "/api/2D_GPS/clan/edit_slogan";
        public const string API_CHANGE_AUTO_ACCEPT_MEMBER = "/api/2D_GPS/clan/change_auto_accept_member";
        public const string API_EDIT_CLAN = "/api/2D_GPS/clan/edit_clan";
        public const string API_EDIT_ANNOUNCE = "/api/2D_GPS/clan/edit_announce";
        public const string API_ATTACK_CLAN_BOSS = "/api/2D_GPS/clan/attackBoss";
        public const string API_GET_CLAN_BOSS_RANK = "/api/2D_GPS/clan/getClanBossRank";
        public const string API_GET_CLAN_BOSS = "/api/2D_GPS/clan/getClanBoss";
        public const string API_GET_CLAN_BOSS_CLAIM_REWARD = "/api/2D_GPS/clan/claimBossReward";

    }

    public class ClanManager : NTBehaviour
    {
        #region Data Player
        [Header("Data Player")]
        public UserClan PlayerUserClan = null;
        public long ClanLastTimeJoined = 0;
        [SerializeField] private NTDictionary<string, UserClan> PlayerClanRequest = null;
        public List<ClanDonateType> ClanDonate = null;
        public Clan PlayerClan = null;
        public ClanMemberInfo[] ClanRequest = null;
        public PlayerClanBossResponse PlayerClanBoss = null;
        public ClanBossBattleResult ClanBossBattleResult = null;
        public UserRankResponse ClanBossRank = null;
        #endregion

        #region Data Clan
        [Header("Data Clan")]
        public ClanRank[] ClansTopLevel = null;
        public NTDictionary<string, ClanInfo> ClanInfos = null;
        public NTDictionary<string, ListClanMemberInfo> ClanMemberList = null;
        public ClanInfo[] ClanFinding = null;
        #endregion

        #region Data Game
        [Header("Data Game")]
        public ClanData ClanData = null;
        public ClanBossData ClanBossData = null;
        #endregion




        public static ClanManager Instance;
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
        [NTButton]

        public void Test()
        {
            StartCoroutine(IEGetPlayerClan());
        }

        public void Logout()
        {
            this.PlayerClan = null;
            this.PlayerUserClan = null;
            this.PlayerClanRequest = new NTDictionary<string, UserClan>();
            this.ClanDonate = new List<ClanDonateType>();

            this.PlayerClanBoss = null;
            this.ClanBossBattleResult = null;
            this.ClanBossRank = null;

            this.lastGetClanBossRank = 0;
            this.lastGetClanRequestList = 0;
            this.lastGetClanBossRank = 0;

            this.ClanLastTimeJoined = 0;
        }

        public void LoadData()
        {
            this.ClanData = JsonUtility.FromJson<ClanData>(DataCenterManager.Instance.GetData(DataName.ClanData));
            this.ClanBossData = JsonUtility.FromJson<ClanBossData>(DataCenterManager.Instance.GetData(DataName.ClanBossData));
        }

        public void UpdateClan(Clan clan)
        {
            if (clan == null || clan._id == null || clan._id.Length == 0) return;
            if (clan.Deleted)
            {
                this.PlayerClan = null;
                this.PlayerUserClan = null;
            }
            else this.PlayerClan = clan;
            RefeshPlayerClanMemberList();
        }
        public void UpdatePlayerUserClan(UserClan userClan)
        {
            if (userClan == null || userClan._id == null || userClan._id.Length == 0) return;
            if (userClan.Status == ClanStatus.NoClan)
            {
                this.PlayerClan = null;
                this.PlayerUserClan = null;
            }
            else
            {
                this.PlayerUserClan = userClan;
            }
            RefeshPlayerClanMemberList();
        }
        public void UpdateClanLastTimeJoined(long clanLastTimeJoined)
        {
            if(clanLastTimeJoined < 1000) return;
            this.ClanLastTimeJoined = clanLastTimeJoined;
        }

        public void UpdateClanRequest(ClanMemberInfo[] clanRequest)
        {
            if (clanRequest == null || clanRequest.Length == 0) return;
            this.ClanRequest = clanRequest;
        }
        public void UpdateClansTopLevel(ClanRank[] clansTopLevel)
        {
            if (clansTopLevel == null || clansTopLevel.Length == 0) return;
            this.ClansTopLevel = clansTopLevel;
        }
        public void UpdateClanInfos(ClanInfo[] clanInfos)
        {
            if (clanInfos == null || clanInfos.Length == 0) return;
            foreach (ClanInfo clanInfo in clanInfos)
            {
                clanInfo.LastUpdate = ServerManager.Instance.GetTimeServer();
                this.ClanInfos.Add(clanInfo._id, clanInfo);
            }
        }
        public void UpdateClanMemberList(ListClanMemberInfo[] clanMemberList)
        {
            if (clanMemberList == null || clanMemberList.Length == 0) return;
            foreach (ListClanMemberInfo clanMemberInfo in clanMemberList)
            {
                clanMemberInfo.LastUpdate = ServerManager.Instance.GetTimeServer();
                this.ClanMemberList.Add(clanMemberInfo.ClandID, clanMemberInfo);
            }
        }
        public void UpdateClanFinding(ClanInfo[] clanFinding)
        {
            if (clanFinding == null || clanFinding.Length == 0) return;
            this.ClanFinding = clanFinding;
        }
        public void UpdatePlayerClanRequest(UserClan[] playerClanRequest)
        {
            if (playerClanRequest == null || playerClanRequest.Length == 0) return;
            foreach (UserClan userClan in playerClanRequest)
            {
                this.PlayerClanRequest.Add(userClan.ClanId, userClan);
            }
        }
        public void UpdateClanBossResponse(PlayerClanBossResponse playerClanBossResponse)
        {
            if (playerClanBossResponse == null || playerClanBossResponse._id == null || playerClanBossResponse._id.Length == 0) return;
            this.PlayerClanBoss = playerClanBossResponse;
            EventListenerManager.instance.PostEvent(EventCode.Clan_UpdateClanBoss);
        }
        public void UpdateClanDonate(ClanDonateType[] clanDonateTypes)
        {
            this.ClanDonate = clanDonateTypes.ToList();
        }
        public void UpdateClanBossBattleResult(ClanBossBattleResult clanBossBattleResult)
        {
            this.ClanBossBattleResult = clanBossBattleResult;
            EventListenerManager.instance.PostEvent(EventCode.Clan_UpdateClanBoss);
        }

        public void UpdateClanBossRank(UserRankResponse clanBossRank)
        {
            if (clanBossRank == null || clanBossRank.UserRanks == null || clanBossRank.UserRanks.Length == 0) return;
            this.ClanBossRank = clanBossRank;
            EventListenerManager.instance.PostEvent(EventCode.Clan_UpdateClanBoss);
        }

        // Update 5 minutes
        private long lastGetTopClanList = 0;
        public void GetTopClanList(Action<List<ClanInfo>> done = null)
        {
            List<ClanInfo> clanInfos = new List<ClanInfo>();
            if (this.ClansTopLevel == null || this.ClansTopLevel.Length == 0 || ServerManager.Instance.GetTimeServer() - this.lastGetTopClanList > 300)
            {
                this.lastGetTopClanList = ServerManager.Instance.GetTimeServer();
                int groupServer = ServerGameManager.Instance.GetPlayerGroupServer();
                StartCoroutine(IEGetTopClanList(groupServer, () =>
                {
                    done?.Invoke(this.ConvertClanRankToClanInfo(this.ClansTopLevel));
                }));
            }
            else
            {
                done?.Invoke(this.ConvertClanRankToClanInfo(this.ClansTopLevel));
            }
        }

        public List<ClanInfo> ConvertClanRankToClanInfo(ClanRank[] clansTopLevel)
        {
            if (clansTopLevel == null || clansTopLevel.Length == 0) return null;
            List<ClanInfo> clanInfos = new List<ClanInfo>();
            foreach (ClanRank clanRank in clansTopLevel)
            {
                ClanInfo clanInfo = this.ClanInfos.Get(clanRank.ClanID);
                if (clanInfo == null) continue;
                clanInfos.Add(clanInfo);
            }
            return clanInfos;
        }

        // Update 5 minutes
        public void GetClan(string clanID, Action<ClanInfo> done = null)
        {
            ClanInfo clanInfo = this.ClanInfos.Get(clanID);
            if (clanInfo == null || clanInfo._id == null || clanInfo._id.Length == 0 || ServerManager.Instance.GetTimeServer() - clanInfo.LastUpdate > 300)
            {
                StartCoroutine(IEGetClan(clanID, () =>
                {
                    clanInfo = this.ClanInfos.Get(clanID);
                    if (clanInfo == null)
                    {
                        done?.Invoke(null);
                        return;
                    }
                    done?.Invoke(clanInfo);
                }));
            }
            else
            {
                done?.Invoke(clanInfo);
            }
        }

        // Update 5 minutes
        public void GetClanMemberInfo(string clanID, Action<ClanMemberInfo[]> done = null)
        {
            ListClanMemberInfo listClanMemberInfo = this.ClanMemberList.Get(clanID);
            if (listClanMemberInfo == null || ServerManager.Instance.GetTimeServer() - listClanMemberInfo.LastUpdate > 300)
            {
                StartCoroutine(IEGetListClanMemberInfo(clanID, () =>
                {
                    listClanMemberInfo = this.ClanMemberList.Get(clanID);
                    if (listClanMemberInfo == null)
                    {
                        done?.Invoke(null);
                        return;
                    }
                    done?.Invoke(listClanMemberInfo.MemberList);
                }));
            }
            else
            {
                done?.Invoke(listClanMemberInfo.MemberList);
            }
        }

        // Update 1 minutes
        private long lastGetClanRequestList = 0;
        public void GetClanRequestList(Action<ClanMemberInfo[]> done = null)
        {
            if (ServerManager.Instance.GetTimeServer() - this.lastGetClanRequestList < 60)
            {
                done?.Invoke(this.ClanRequest);
                return;
            }
            if (this.ClanRequest == null || this.ClanRequest.Length == 0)
            {
                this.lastGetClanRequestList = ServerManager.Instance.GetTimeServer();
                StartCoroutine(IEGetClanRequestList(this.PlayerClan._id, () =>
                {
                    done?.Invoke(this.ClanRequest);

                }));
            }
            else
            {
                done?.Invoke(this.ClanRequest);
            }
        }

        // Msg Delivery
        public void RegisterChannel()
        {
            if (this.IsClan())
            {
                MsgDeliveryRoom.Instance.RegisterChannel(this.GetClanChannel());
                StartCoroutine(ChatService.Instance.RegisterChannelClanChat());
            }
        }
        public void LeaveChannel()
        {
            if (this.GetClanID() != null)
            {
                MsgDeliveryRoom.Instance.LeaveChannel(this.GetClanChannel());
                ChatService.Instance.UnRegisterChannelRoom(this.GetClanChannel());
            }
        }

        public void OnClanMsg(MsgData msgData)
        {
            ClanMsg clanMsg = JsonUtility.FromJson<ClanMsg>(msgData.Data);
            switch (clanMsg.Type)
            {
                // Clan
                case ClanMsgType.ClanNewMember:
                    ClanManager.Instance.OnClanNewMember(clanMsg.Data);
                    break;
                case ClanMsgType.ClanRequest:
                    ClanManager.Instance.OnClanNewRequest(clanMsg.Data);
                    break;
                case ClanMsgType.ClanDonate:
                    ClanManager.Instance.OnClanDonate(clanMsg.Data);
                    break;
                case ClanMsgType.ClanNewAnnounce:
                    ClanManager.Instance.OnClanNewAnnounce(clanMsg.Data);
                    break;
                case ClanMsgType.ClanJoin:
                    ClanManager.Instance.OnClanJoin(clanMsg.Data);
                    break;
                case ClanMsgType.ClanLeave:
                    ClanManager.Instance.OnClanLeave(clanMsg.Data);
                    break;
                case ClanMsgType.ClanDelete:
                    ClanManager.Instance.OnClanDelete(clanMsg.Data);
                    break;
                default:
                    break;
            }
        }

        public void OnClanNewMember(string data)
        {
            string displayName = data;
        }

        public void OnClanNewRequest(string data)
        {
            string displayName = data;
        }

        public void OnClanDonate(string data)
        {
            UserClanDonate userClanDonate = JsonUtility.FromJson<UserClanDonate>(data);
            this.RefeshPlayerClanMemberList();
        }

        public void OnClanNewAnnounce(string data)
        {
            string announce = data;
            if (this.IsClan())
            {
                this.PlayerClan.Announce = announce;
                EventListenerManager.instance.PostEvent(EventCode.Clan_UpdateAnnounce, announce);
            }
        }

        public void OnClanJoin(string data)
        {
            StartCoroutine(IEGetPlayerClan((clan) =>
            {
                if (this.IsClan())
                {
                    this.RegisterChannel();
                }
            }, false));
        }

        public void OnClanLeave(string data)
        {
            this.LeaveChannel();
            StartCoroutine(IEGetPlayerClan((clan) =>
            {

            }));
        }

        public void OnClanDelete(string data)
        {
            this.LeaveChannel();
            StartCoroutine(IEGetPlayerClan((clan) =>
            {

            }));
        }

        public void RefeshPlayerClanMemberList()
        {
            if (!this.IsClan()) return;
            ListClanMemberInfo listClanMemberInfo = this.ClanMemberList.Get(this.GetClanID());
            if (listClanMemberInfo == null) return;
            if (ServerManager.Instance.GetTimeServer() - listClanMemberInfo.LastUpdate > 300) return;
            listClanMemberInfo.LastUpdate = ServerManager.Instance.GetTimeServer() + 300 - 5;
            EventListenerManager.instance.PostEvent(EventCode.Clan_UpdateDonate);
        }


        #endregion

        #region API
        public IEnumerator IECreateClan(ClanCreateData clanCreateData, Action<Clan> done = null)
        {
            JSONNode data = new JSONObject();
            data["userID"] = UserDataManager.Instance.GetUserID();
            data["clanName"] = clanCreateData.Name;
            data["clanIcon"] = clanCreateData.Icon;
            data["clanFrame"] = clanCreateData.Frame;
            data["clanColor"] = clanCreateData.Color;
            data["clanSlogan"] = clanCreateData.Slogan;
            data["autoAccept"] = clanCreateData.AutoAcceptMember;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(data.ToString(), URL_Config.BASE_API_URL + ClanConfig.API_CREATE_CLAN, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                done?.Invoke(apiResponseData.PlayerClan);
            });
        }

        public IEnumerator IEGetTopClanList(int groupServer, Action done = null)
        {
            JSONNode data = new JSONObject();
            data["groupServer"] = groupServer;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(data.ToString(), URL_Config.BASE_API_URL + ClanConfig.API_GET_TOP_CLAN_LIST, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                done?.Invoke();
            });
        }

        public IEnumerator IEJoinClanRequest(string userID, string clanID, Action<UserClan> done = null)
        {
            JSONNode data = new JSONObject();
            data["userID"] = userID;
            data["clanID"] = clanID;
            
            long timeCoolDown = this.ClanLastTimeJoined + this.ClanData.CoolDownJoinClan - ServerManager.Instance.GetTimeServer();
            if(timeCoolDown > 0){
                string str = LeanLocalization.GetTranslationText("cooldown_join_new_clan", "You must wait {0} before joining another clan!");
                str = string.Format(str, NTFunction.Format_Time(timeCoolDown));
                HUDCanvas.Instance.ShowNotification(str, LeanLocalization.GetTranslationText("cooldown_join_new_clan_title", "Oath of Patience"));
                yield break;
            }

            yield return Rubik.Server.APIManager.Instance.PostDataUrl(data.ToString(), URL_Config.BASE_API_URL + ClanConfig.API_JOIN_CLAN_REQUEST, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                if (this.IsClan())
                {
                    this.ClanMemberList.Remove(this.GetClanID());
                }
                done?.Invoke(apiResponseData.PlayerUserClan);
            });
        }

        public IEnumerator IELeaveClanRequest(string clanID, Action done = null)
        {
            JSONNode data = new JSONObject();
            data["userID"] = UserDataManager.Instance.GetUserID();
            data["clanID"] = clanID;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(data.ToString(), URL_Config.BASE_API_URL + ClanConfig.API_LEAVE_CLAN_REQUEST, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                done?.Invoke();
            });
        }

        public IEnumerator IEGetClan(string clanID, Action done = null)
        {
            JSONNode data = new JSONObject();
            data["clanID"] = clanID;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(data.ToString(), URL_Config.BASE_API_URL + ClanConfig.API_GET_CLAN, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                done?.Invoke();
            });
        }

        public IEnumerator IEGetListClanMemberInfo(string clanID, Action done = null)
        {
            JSONNode data = new JSONObject();
            data["clanID"] = clanID;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(data.ToString(), URL_Config.BASE_API_URL + ClanConfig.API_GET_LIST_CLAN_MEMBER_INFO, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                done?.Invoke();
            });
        }

        public IEnumerator IEGetPlayerClan(Action<Clan> done = null, bool isShowLoading = true)
        {
            JSONNode data = new JSONObject();
            data["userID"] = UserDataManager.Instance.GetUserID();
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(data.ToString(), URL_Config.BASE_API_URL + ClanConfig.API_GET_PLAYER_CLAN, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                done?.Invoke(apiResponseData.PlayerClan);
            }, isShowLoading);
        }

        public IEnumerator IEGetClanRequestList(string clanID, Action done = null)
        {
            JSONNode data = new JSONObject();
            data["clanID"] = clanID;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(data.ToString(), URL_Config.BASE_API_URL + ClanConfig.API_GET_CLAN_REQUEST_LIST, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                done?.Invoke();
            });
        }

        public IEnumerator IEAcceptClanRequest(string clanID, string memberID, Action done = null)
        {
            JSONNode data = new JSONObject();
            data["userID"] = UserDataManager.Instance.GetUserID();
            data["clanID"] = clanID;
            data["memberID"] = memberID;

            yield return Rubik.Server.APIManager.Instance.PostDataUrl(data.ToString(), URL_Config.BASE_API_URL + ClanConfig.API_ACCEPT_CLAN_REQUEST, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                this.lastGetClanRequestList = 0;
                this.ClanMemberList.Remove(clanID);
                this.ClanRequest = this.ClanRequest.Where(x => x.UserId != memberID).ToArray();
                done?.Invoke();
            });
        }

        public IEnumerator IERejectClanRequest(string clanID, string memberID, Action<UserClan> done = null)
        {
            JSONNode data = new JSONObject();
            data["userID"] = UserDataManager.Instance.GetUserID();
            data["clanID"] = clanID;
            data["memberID"] = memberID;

            yield return Rubik.Server.APIManager.Instance.PostDataUrl(data.ToString(), URL_Config.BASE_API_URL + ClanConfig.API_REJECT_CLAN_REQUEST, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                this.lastGetClanRequestList = 0;
                this.ClanMemberList.Remove(clanID);
                this.ClanRequest = this.ClanRequest.Where(x => x.UserId != memberID).ToArray();
                done?.Invoke(apiResponseData.PlayerUserClan);
            });
        }

        public IEnumerator IERemoveClanMember(string clanID, string memberID, Action<UserClan> done = null)
        {
            JSONNode data = new JSONObject();
            data["userID"] = UserDataManager.Instance.GetUserID();
            data["clanID"] = clanID;
            data["memberID"] = memberID;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(data.ToString(), URL_Config.BASE_API_URL + ClanConfig.API_REMOVE_CLAN_MEMBER, (data) =>
            {
                this.ClanMemberList.Remove(clanID);
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                done?.Invoke(apiResponseData.PlayerUserClan);
            });
        }

        public IEnumerator IEPromoteClanMember(string memberToPromoteID, ClanRole roles, Action done = null)
        {
            if (!this.IsClan()) yield break;
            ListClanMemberInfo listClanMemberInfo = this.ClanMemberList.Get(this.GetClanID());
            if (listClanMemberInfo == null || ServerManager.Instance.GetTimeServer() - listClanMemberInfo.LastUpdate > 300)
            {
                yield return IEGetListClanMemberInfo(this.GetClanID());
            }
            listClanMemberInfo = this.ClanMemberList.Get(this.GetClanID());
            if(listClanMemberInfo == null){
                ServerManager.Instance.ShowNotificationDataError();
                yield break;
            }
            if (roles == ClanRole.CAPTAIN && this.IsMaxCaptain())
            {
                HUDCanvas.Instance.ShowNotification(LeanLocalization.GetTranslationText("clan_max_captain", "Clan has reached the maximum number of Captains!"));
                yield break;
            }
            if (roles == ClanRole.COLEADER && this.IsMaxColeader())
            {
                HUDCanvas.Instance.ShowNotification(LeanLocalization.GetTranslationText("clan_max_deputy", "Clan has reached the maximum number of Co-Leaders!"));
                yield break;
            }

            JSONNode data = new JSONObject();
            data["userID"] = UserDataManager.Instance.GetUserID();
            data["clanID"] = this.GetClanID();
            data["memberToPromoteID"] = memberToPromoteID;
            data["roles"] = (int)roles;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(data.ToString(), URL_Config.BASE_API_URL + ClanConfig.API_PROMOTE_CLAN_MEMBER, (data) =>
            {
                this.ClanMemberList.Remove(this.GetClanID());
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                done?.Invoke();
            });
        }

        public IEnumerator IEDeleteClan(string clanID, Action done = null)
        {
            JSONNode data = new JSONObject();
            data["userID"] = UserDataManager.Instance.GetUserID();
            data["clanID"] = clanID;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(data.ToString(), URL_Config.BASE_API_URL + ClanConfig.API_DELETE_CLAN, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                done?.Invoke();
            });
        }

        public IEnumerator IEDonateFund(string userID, string clanID, ClanDonateType donateType, Action<ClanDonateType[]> done = null)
        {
            ItemData payItem = this.GetCostDonate(donateType);
            if (payItem != null)
            {
                if (ItemDataManager.Instance.GetItem(payItem.Type).Amount < payItem.Amount)
                {
                    ItemDataManager.Instance.ShowDontEnoughItem(payItem.Type);
                    yield break;
                }
            }
            JSONNode data = new JSONObject();
            data["userID"] = userID;
            data["clanID"] = clanID;
            data["donateType"] = (int)donateType;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(data.ToString(), URL_Config.BASE_API_URL + ClanConfig.API_DONATE_FUND, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                done?.Invoke(apiResponseData.ClanDonate);
            });
        }

        public IEnumerator IEFindClan(string clanName, string userID, Action<ClanInfo[]> done = null)
        {
            JSONNode data = new JSONObject();
            data["clanName"] = clanName;
            data["userID"] = userID;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(data.ToString(), URL_Config.BASE_API_URL + ClanConfig.API_FIND_CLAN, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                done?.Invoke(apiResponseData.ClanFinding);
            });
        }

        public IEnumerator IEChangeClanLeader(string userID, string clanID, string newLeaderID, Action<UserClan> done = null)
        {
            JSONNode data = new JSONObject();
            data["userID"] = userID;
            data["clanID"] = clanID;
            data["newLeaderID"] = newLeaderID;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(data.ToString(), URL_Config.BASE_API_URL + ClanConfig.API_CHANGE_CLAN_LEADER, (data) =>
            {
                this.ClanMemberList.Remove(clanID);
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                done?.Invoke(apiResponseData.PlayerUserClan);
            });
        }

        public IEnumerator IEEditSlogan(string userID, string clanID, string slogan, Action done = null)
        {
            JSONNode data = new JSONObject();
            data["userID"] = userID;
            data["clanID"] = clanID;
            data["slogan"] = slogan;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(data.ToString(), URL_Config.BASE_API_URL + ClanConfig.API_EDIT_SLOGAN, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                done?.Invoke();
            });
        }

        public IEnumerator IEChangeAutoAcceptMember(string userID, string clanID, bool autoAcceptMember, Action done = null)
        {
            JSONNode data = new JSONObject();
            data["userID"] = userID;
            data["clanID"] = clanID;
            data["auto"] = autoAcceptMember;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(data.ToString(), URL_Config.BASE_API_URL + ClanConfig.API_CHANGE_AUTO_ACCEPT_MEMBER, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                done?.Invoke();
            });
        }

        public IEnumerator IEditClan(ClanCreateData clanCreateData, Action done = null)
        {
            JSONNode data = new JSONObject();
            data["userID"] = UserDataManager.Instance.GetUserID();
            data["clanID"] = this.GetClanID();
            data["clanName"] = clanCreateData.Name;
            data["clanIcon"] = clanCreateData.Icon;
            data["clanFrame"] = clanCreateData.Frame;
            data["clanColor"] = clanCreateData.Color;
            data["clanSlogan"] = clanCreateData.Slogan;
            data["autoAccept"] = clanCreateData.AutoAcceptMember;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(data.ToString(), URL_Config.BASE_API_URL + ClanConfig.API_EDIT_CLAN, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                done?.Invoke();
            });
        }

        public IEnumerator IEEditAnnounce(string userID, string clanID, string announce, Action done = null)
        {
            JSONNode data = new JSONObject();
            data["userID"] = userID;
            data["clanID"] = clanID;
            data["announce"] = announce;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(data.ToString(), URL_Config.BASE_API_URL + ClanConfig.API_EDIT_ANNOUNCE, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                done?.Invoke();
            });
        }

        public IEnumerator IEGetClanBoss(Action<PlayerClanBossResponse> done = null, bool isShowLoading = true)
        {
            JSONNode data = new JSONObject();
            data["userID"] = UserDataManager.Instance.GetUserID();
            data["clanID"] = this.GetClanID();
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(data.ToString(), URL_Config.BASE_API_URL + ClanConfig.API_GET_CLAN_BOSS, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                done?.Invoke(this.PlayerClanBoss);
            }, isShowLoading);
        }

        public IEnumerator IEAttackClanBoss(bool isAdv = false, Action<ClanBossBattleResult> done = null)
        {
            JSONNode data = new JSONObject();
            data["userID"] = UserDataManager.Instance.GetUserID();
            data["clanID"] = this.PlayerClan._id;
            data["team"] = JSONNode.Parse(JsonUtility.ToJson(BattleTeamManager.Instance.GetBattleTeamDataSelected().ToShortTeam()));
            data["bossIndex"] = (int)this.GetBossCardPlayerIndex();
            data["isAdv"] = isAdv;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(data.ToString(), URL_Config.BASE_API_URL + ClanConfig.API_ATTACK_CLAN_BOSS, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                this.lastGetClanBossRank = 0;
                if (apiResponseData.Status == 0) return;
                done?.Invoke(apiResponseData.ClanBossBattleResult);
            });
        }

        public long lastGetClanBossRank = 0;
        public IEnumerator IEGetClanBossRank(Action done = null)
        {
            if (this.lastGetClanBossRank > ServerManager.Instance.GetTimeServer() - 5 * 60)
            {
                done?.Invoke();
                yield break;
            }
            this.lastGetClanBossRank = ServerManager.Instance.GetTimeServer();
            JSONNode data = new JSONObject();
            data["userID"] = UserDataManager.Instance.GetUserID();
            data["clanID"] = this.GetClanID();
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(data.ToString(), URL_Config.BASE_API_URL + ClanConfig.API_GET_CLAN_BOSS_RANK, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                done?.Invoke();
            });
        }

        public IEnumerator IEClaimClanBossReward(Action done = null)
        {
            JSONNode data = new JSONObject();
            data["userID"] = UserDataManager.Instance.GetUserID();
            data["clanID"] = this.GetClanID();
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(data.ToString(), URL_Config.BASE_API_URL + ClanConfig.API_GET_CLAN_BOSS_CLAIM_REWARD, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                done?.Invoke();
            });
        }

        #endregion

        #region Get

        public CardPlayerIndex GetBossCardPlayerIndex()
        {
            long day = NTFunction.GetTotalDay(ServerManager.Instance.GetTimeServer());
            return CardPlayerConfig.ClanBossCardPlayerIndex[(int)(day % CardPlayerConfig.ClanBossCardPlayerIndex.Count)];
        }

        public (int AmountReward, long MaxDamage) GetClanBossReward(long damage)
        {
            int amountReward = 0;
            long maxDamage = 0;
            if (damage < this.ClanBossData.DamageMilestone[0].Damage)
            {
                return (0, this.ClanBossData.DamageMilestone[0].Damage);
            }

            for (int i = 0; i < this.ClanBossData.DamageMilestone.Length; i++)
            {
                if (damage >= this.ClanBossData.DamageMilestone[i].Damage)
                {
                    amountReward = this.ClanBossData.DamageMilestone[i].Index + 1;
                    maxDamage = this.ClanBossData.DamageMilestone[i + 1].Damage;
                }
                else
                {
                    break;
                }
            }
            return (amountReward, maxDamage);
        }

        public (int PlayerAttack, int[] PlayerReward, List<TimeAttackReward> Milestone) GetMilestoneClanBossReward()
        {
            int timeAttack = (int)this.PlayerClan.BossTimeAttack;
            int[] rewardIndex = PlayerClanBoss.RecieveReward;
            List<TimeAttackReward> milestone = new List<TimeAttackReward>(this.ClanBossData.TimeAttackReward);

            return (timeAttack, rewardIndex, milestone);
        }

        public (int rest, int max, long timeReset) GetPlayerClanBossAttack()
        {
            int rest = this.ClanBossData.MaxTimeAttack - (int)this.PlayerClanBoss.TimeAttack;
            int max = this.ClanBossData.MaxTimeAttack;
            return (rest, max, ServerManager.Instance.GetNextTimeNewDay());
        }

        public bool IsPlayerClanRequest(string clanID)
        {
            return this.PlayerClanRequest.Contains(clanID);
        }

        public bool IsDonated(ClanDonateType donateType)
        {
            foreach (ClanDonateType clanDonateType in this.ClanDonate)
            {
                if (clanDonateType == donateType)
                {
                    return true;
                }
            }
            return false;
        }

        public ItemRate[] GetItemRewardClanBoss()
        {
            return this.ClanBossData.MilestoneReward;
        }

        public string GetClanID()
        {
            if (this.PlayerClan == null || this.PlayerClan._id == null || this.PlayerClan._id.Length == 0) return null;
            return this.PlayerClan._id;
        }

        public bool IsClan()
        {
            if (
                this.PlayerClan == null || this.PlayerClan._id == null || this.PlayerClan._id.Length == 0 ||
                this.PlayerUserClan == null || this.PlayerUserClan._id == null || this.PlayerUserClan._id.Length == 0
            ) return false;
            if (this.PlayerClan.Deleted) return false;
            return true;
        }

        public string GetClanChannel()
        {
            if (this.GetClanID() == null) return null;
            return "Clan_" + this.GetClanID();
        }

        public string GetClanAnnouce()
        {
            if (this.PlayerClan == null || this.PlayerClan.Announce == null || this.PlayerClan.Announce.Length == 0) return null;
            return this.PlayerClan.Announce;
        }

        public bool IsClanEdit()
        {
            if (!this.IsClan()) return false;
            if (this.PlayerUserClan == null) return false;
            if (
                this.PlayerUserClan.Role == ClanRole.LEADER
                || this.PlayerUserClan.Role == ClanRole.COLEADER
            ) return true;
            return false;
        }

        public bool IsClanAcceptMember()
        {
            if (!this.IsClan()) return false;
            if (this.PlayerUserClan == null) return false;
            if (
                this.PlayerUserClan.Role == ClanRole.LEADER
                || this.PlayerUserClan.Role == ClanRole.COLEADER
                || this.PlayerUserClan.Role == ClanRole.CAPTAIN
            ) return true;
            return false;

        }

        public bool IsClanDelete()
        {
            if (!this.IsClan()) return false;
            if (this.PlayerUserClan == null) return false;
            if (this.PlayerUserClan.Role == ClanRole.LEADER) return true;
            return false;
        }

        public bool IsClaimClanBossReward()
        {
            if (this.IsClan()) return false;
            foreach (TimeAttackReward timeAttackReward in this.ClanBossData.TimeAttackReward)
            {
                if (timeAttackReward.Amount <= this.PlayerClan.BossTimeAttack && this.PlayerClanBoss.RecieveReward.ToList().IndexOf(timeAttackReward.Amount) == -1)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsClanBossAdv()
        {

            if (!this.IsClan()) return false;
            if (this.PlayerClanBoss.TimeAttackAdv < this.ClanBossData.MaxTimeAttackAdv) return true;
            return false;
        }

        public ItemData GetCostDonate(ClanDonateType donateType)
        {
            foreach (ClanDonate clanDonate in this.ClanData.Donate)
            {
                if (clanDonate.Type == donateType)
                {
                    return clanDonate.PayItem;
                }
            }
            return null;
        }

        public bool IsMaxCaptain()
        {
            int amountCaptain = 0;
            foreach (ClanMemberInfo clanMemberInfo in this.ClanMemberList.Get(this.GetClanID()).MemberList)
            {
                if (clanMemberInfo.Role == ClanRole.CAPTAIN)
                {
                    amountCaptain++;
                }
            }
            if (amountCaptain >= this.GetLevelClanData().MaxCaptain)
            {
                return true;
            }
            return false;
        }

        public bool IsMaxColeader()
        {
            int amountColeader = 0;
            foreach (ClanMemberInfo clanMemberInfo in this.ClanMemberList.Get(this.GetClanID()).MemberList)
            {
                if (clanMemberInfo.Role == ClanRole.COLEADER)
                {
                    amountColeader++;
                }
            }
            if (amountColeader >= this.GetLevelClanData().MaxDeputy)
            {
                return true;
            }
            return false;
        }

        public LevelClanData GetLevelClanData()
        {
            return this.ClanData.LevelData[this.PlayerClan.Level];
        }


        #endregion
    }
}
