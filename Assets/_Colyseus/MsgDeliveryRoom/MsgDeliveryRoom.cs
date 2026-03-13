using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Colyseus;
using NTPackage.Functions;
using UnityEngine;

namespace Rubik.MsgDelivery
{
    using NTPackage;
    using Rubik._2DGPS.Chat;
    using Rubik.Banner;
    using Rubik.Colyseus;
    using Rubik.Config;
    using Rubik.Friend;
    using Rubik.Manager;
    using Rubik.Myrk.Clan;
    using Rubik.Myrk.Portal;
    using Rubik.UI;
    using Rubik.UserDataPlayer;
    using Sirenix.OdinInspector;

    public class MsgDeliveryRoom : NTBehaviour
    {
        private ColyseusRoom<State> _room;
        public bool IsConnecting = false;
        public bool IsInit = false;

        public NTDictionary<string, bool> UserOnlineStatusDic;

        public string SessionId = "";
        public string SessionAppID = "";

        public float CountDelayPing = 0;
        public float PingTime = 0;
        public float PongTime = 0;

        public List<string> ChannelRegisters = new List<string>();

        public static MsgDeliveryRoom Instance;
        protected override void Awake()
        {
            base.Awake();
            if (MsgDeliveryRoom.Instance != null)
            {
                NTLog.LogWarning("Only 1 Instance allow");
                return;
            }
            SessionAppID = NTFunction.GenerateId();
            MsgDeliveryRoom.Instance = this;
        }

        protected override void Update()
        {
            if (this.IsInit)
            {
                try
                {
                    if (this._room != null && this._room.colyseusConnection.IsOpen){
                        this.CountDelayPing += Time.deltaTime;
                        if(this.CountDelayPing >= 1){
                            float currentTime = Time.time;
                            this.PingTime = (currentTime - this.PongTime)/Time.timeScale;
                            if(this.PingTime > 3){
                                BannerManager.Instance.AddTopBanner(BannerTopConfig.ConnectUnstable, Lean.Localization.LeanLocalization.GetTranslationText("connect_unstable", "Connect unstable"));
                            }
                            else{
                                BannerManager.Instance.RemoveTopBanner(BannerTopConfig.ConnectUnstable);
                            }
                            this.Ping();
                            this.CountDelayPing = 0;
                        }
                        return;
                    }
                    this.Reconnect();
                }
                catch (System.Exception e)
                {
                    BannerManager.Instance.AddTopBanner(BannerTopConfig.ConnectUnstable, Lean.Localization.LeanLocalization.GetTranslationText("connect_unstable", "Connect unstable"));
                    NTLog.LogError(e.ToString());
                }
            }
        }

        void OnApplicationQuit()
        {
            StartCoroutine(this.UnInit());
        }

        private async void Reconnect()
        {
            if (this.IsConnecting) return;
            try
            {
                BannerManager.Instance.AddTopBanner(BannerTopConfig.ConnectUnstable, Lean.Localization.LeanLocalization.GetTranslationText("connect_unstable", "Connect unstable"));
                this.IsConnecting = true;
                await this.InitClient();
                Debug.Log("Reconnect suc");
            }
            catch (System.Exception e)
            {
                BannerManager.Instance.AddTopBanner(BannerTopConfig.ConnectUnstable, Lean.Localization.LeanLocalization.GetTranslationText("connect_unstable", "Connect unstable"));
                Debug.Log("Reconnect fail: " + e);
                this.IsConnecting = false;
            }
        }

        public void Init()
        {
            NTPackage.Functions.NTLog.LogMessage("Init MsgDeliveryRoom", gameObject);
            StartCoroutine(this.UnInit(() =>
            {
                this.IsInit = true;
                _ = InitClient();
            }));
        }

        public IEnumerator UnInit(Action done = null)
        {
            NTLog.LogMessage("UnInit MsgDeliveryRoom");
            HUDCanvas.Instance.ShowLoadingPanel();
            this.IsInit = false;
            this.IsConnecting = false;
            this.LeaveAllChannel();
            try
            {
                if (_room != null) _ = _room.Leave();

            }
            catch (System.Exception e)
            {
                NTPackage.Functions.NTLog.LogError(e.ToString(), gameObject);
            }
            int time = 0;
            while (true)
            {
                yield return new WaitForSeconds(1);
                time++;
                if (time > 5) break;
                try
                {
                    if (!this._room.colyseusConnection.IsOpen) break;
                }
                catch (System.Exception)
                {
                    break;
                }
            }
            HUDCanvas.Instance.HideLoadingPanel();
            done?.Invoke();
        }

        public async Task InitClient()
        {
            NTLog.LogMessage("InitClient MsgDeliveryRoom");
            try
            {
                this.IsConnecting = true;
                PlayerJoinData playerJoinData = new PlayerJoinData();
                playerJoinData.UserID = UserDataManager.Instance.GetUserID();
                playerJoinData.SessionAppID = SessionAppID;
                _room = await ColyseusRoomManager.Instance.Client.JoinOrCreate<State>(ColyseusRoomName.MsgDelivery, playerJoinData.ToDictionary());
                _room.OnLeave += (code) =>
                {
                    NTLog.LogMessage("client left the room");
                };
                _room.OnMessage<MsgData>(MsgDeliveryKey.System, SystemMsg);
                _room.OnMessage<CheckUserOnlineData>(MsgDeliveryKey.CheckUserOnline, OnCheckUserOnline);
                _room.OnMessage<string>(MsgDeliveryKey.RegisterChannel, OnRegisterChannel);
                _room.OnMessage<ChatMsg>(MsgDeliveryKey.ChatMsg, ChatService.Instance.OnChatMsg);
                _room.OnMessage<PortalMsg>(MsgDeliveryKey.PortalMsg, PortalWorldMapManager.Instance.OnPortalMsg);
                _room.OnMessage<float>(MsgDeliveryKey.Pong, Pong);

                this.SessionId = _room.SessionId;
                ChatService.Instance.RegisterChannel();
                ClanManager.Instance.RegisterChannel();
                this.RegisterPersonalChannel();
                this.IsConnecting = false;
                this.PongTime = Time.time;
            }
            catch (System.Exception e)
            {
                NTPackage.Functions.NTLog.LogError(e.ToString(), gameObject);
                this.IsConnecting = false;
            }
        }

        public void RegisterPersonalChannel(){
            this.RegisterChannel(this.GetPersonalChannel());
        }

        private void SystemMsg(MsgData msgData)
        {
            NTLog.LogMessage("SystemMsg: " + JsonUtility.ToJson(msgData));
            if (msgData.Type == MsgType.DuplicateDevice)
            {
                HUDCanvas.Instance.ShowNotification("Your account is logged in another device");
                ServerManager.Instance.LogOut();
                return;
            }else{
                switch(msgData.Type){
                    // Clan
                    case MsgType.Clan:
                        ClanManager.Instance.OnClanMsg(msgData);
                        break;
                    default:
                        break;
                }
            }
        }

        public void RegisterChannel(string channelID){
            NTLog.LogMessage("RegisterChannel: " + channelID);
            RegisterChannelData registerChannelData = new RegisterChannelData();
            registerChannelData.UserID = UserDataManager.Instance.GetUserID();
            registerChannelData.ChannelID = channelID;
            this.SendMessage(MsgDeliveryKey.RegisterChannel, registerChannelData);
        }

        public void OnRegisterChannel(string channelID){
            ChannelRegisters.Add(channelID);
        }

        public void LeaveChannel(string channelID){
            LeaveChannelData leaveChannelData = new LeaveChannelData();
            leaveChannelData.UserID = UserDataManager.Instance.GetUserID();
            leaveChannelData.ChannelID = channelID;
            this.SendMessage(MsgDeliveryKey.LeaveChannel, leaveChannelData);
        }

        public void LeaveAllChannel(){
            this.SendMessage(MsgDeliveryKey.LeaveAllChannel, UserDataManager.Instance.GetUserID());
        }

        public void Ping(){
            this.SendMessage(MsgDeliveryKey.Ping, Time.time);
        }

        public void Pong(float timeServer){
            this.PongTime =  timeServer;
        }

        public void CheckUserOnline(List<string> userIDs){
            CheckUserOnlineData checkUserOnlineData = new CheckUserOnlineData();
            checkUserOnlineData.UserOnlineStatus = new List<UserOnlineStatusData>();
            foreach (var userID in userIDs)
            {
                checkUserOnlineData.UserOnlineStatus.Add(new UserOnlineStatusData(){UserID = userID, Status = false});
            }
            this.SendMessage(MsgDeliveryKey.CheckUserOnline, checkUserOnlineData);
        }

        public void OnCheckUserOnline(CheckUserOnlineData checkUserOnlineData){
            if(this.UserOnlineStatusDic == null || this.UserOnlineStatusDic.Count == 0) this.UserOnlineStatusDic = new NTDictionary<string, bool>();
            foreach (var userOnlineStatus in checkUserOnlineData.UserOnlineStatus)
            {
                this.UserOnlineStatusDic.Add(userOnlineStatus.UserID, userOnlineStatus.Status);
            }
        }

        public void SendMessage<T>(string key, T msg){
            NTLog.LogMessage("SendMessage: " + key + " " + JsonUtility.ToJson(msg));
            try
            {
                _ = _room.Send(key, msg);
            }
            catch (System.Exception e)
            {
                NTLog.LogWarning(e.ToString());
            }
        }

        #region Getter

        public bool GetUserOnlineStatus(string userID){
            return this.UserOnlineStatusDic.Get(userID);
        }

        public string GetPersonalChannel(){
            return "Personal_" + UserDataManager.Instance.GetUserID();
        }

        public bool IsRegisterChannel(string channelKey){
            return this.ChannelRegisters.Contains(channelKey);
        }

        #endregion
    }
}