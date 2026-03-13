using System.Collections;
using System.Collections.Generic;
using NTPackage.Functions;
using Rubik.MsgDelivery;
using UnityEngine;

namespace Rubik._2DGPS.Chat
{
    using MsgDelivery;
    using NTPackage;
    using NTPackage.EventDispatcher;
    using Rubik.Friend;
    using Rubik.Manager;
    using Rubik.Myrk.Clan;
    using Rubik.UserDataPlayer;
    using Rubik.UserProfile;
    public class NotifyChatList
    {
        public List<string> Datas = new List<string>();
    }

    public class ChatService : NTBehaviour
    {
        public const int MAX_CHAT_HISTORY = 20;

        public const string NOTIFY_CHAT_LIST = "NotifyChatList";
        public const string NOTIFY_CHAT_COUNT = "NotifyChatCount";
        public const string NOTIFY_CHAT_BOOL = "NotifyChatBool";
        public const string CHAT_MUTED = "Chat_Muted";

        public NTDictionary<string, ChatHistoryData> ChannelList = new NTDictionary<string, ChatHistoryData>();

        // Notify Chat
        public NotifyChatList NotifyChatList = new NotifyChatList();
        public NTDictionary<string, bool> NotifyChat = new NTDictionary<string, bool>();
        public NTDictionary<string, long> NotifyChatCount = new NTDictionary<string, long>();

        public static ChatService Instance;
        protected override void Awake()
        {
            base.Awake();
            if (ChatService.Instance != null)
            {
                NTLog.LogWarning("Only 1 Instance allow");
                return;
            }
            ChatService.Instance = this;
        }

        protected override void Start()
        {
            base.Start();
            // Load Notify Chat
            NotifyChatList = JsonUtility.FromJson<NotifyChatList>(PlayerPrefs.GetString(NOTIFY_CHAT_LIST, JsonUtility.ToJson(new NotifyChatList())));
            if (NotifyChatList != null && NotifyChatList.Datas != null)
            {
                foreach (string channelKey in NotifyChatList.Datas)
                {
                    long count = PlayerPrefs.GetInt(NOTIFY_CHAT_COUNT + channelKey);
                    this.NotifyChatCount.Add(channelKey, count);
                    bool isNotify = PlayerPrefs.GetInt(NOTIFY_CHAT_BOOL + channelKey) == 1;
                    this.NotifyChat.Add(channelKey, isNotify);
                }
            }
        }

        public void RegisterChannel()
        {
            // Server Chat
            StartCoroutine(this.RegisterChannelServerChat());
            StartCoroutine(this.RegisterChannelFriendChat());
            StartCoroutine(this.RegisterChannelClanChat());
        }

        public IEnumerator RegisterChannelServerChat()
        {
            yield return RegisterChannelRoom(GetServerChatChannel());
        }

        public IEnumerator RegisterChannelClanChat()
        {
            if(ClanManager.Instance.IsClan()){
                yield return RegisterChannelRoom(this.GetClanChatChannel());
            }
        }

        public void RegisterChannel(string channelKey){
            StartCoroutine(this.RegisterChannelRoom(channelKey));
        }

        public void LeaveClanChannel(){
            if(this.GetClanChatChannel()!= null){
                this.UnRegisterChannelRoom(this.GetClanChatChannel());
            }
        }

        public IEnumerator RegisterChannelFriendChat()
        {
            foreach (string channelKey in FriendManager.Instance.GetFriendChatChannelList())
            {
                MsgDeliveryRoom.Instance.RegisterChannel(channelKey);
            }
            yield return new WaitForSeconds(1f);
            foreach (string channelKey in FriendManager.Instance.GetFriendChatChannelList())
            {
                this.GetChatHistory(channelKey);
            }
        }

        public IEnumerator RegisterChannelRoom(string roomID)
        {
            MsgDeliveryRoom.Instance.RegisterChannel(roomID);
            yield return new WaitForSeconds(1f);
            this.GetChatHistory(roomID);
        }

        public void UnRegisterChannelRoom(string channelKey)
        {
            MsgDeliveryRoom.Instance.LeaveChannel(channelKey);
        }

        public void OnChatMsg(ChatMsg msgChat)
        {
            NTLog.LogMessage("OnChatMsg: " + msgChat.Data);
            if (msgChat.Type == ChatMsgType.SendChat)
            {
                this.ReceiveChat(JsonUtility.FromJson<SendChatData>(msgChat.Data), false);
            }
            else if (msgChat.Type == ChatMsgType.GetChatHistory)
            {
                this.UpdateChatHistory(JsonUtility.FromJson<ChatHistoryData>(msgChat.Data));
            }

        }

        public void ReceiveChat(SendChatData chatData, bool isHistory = false)
        {
            if (chatData.Version == 0) chatData.Emoji = -1;
            NTLog.LogMessage("ReceiveChat: " + JsonUtility.ToJson(chatData));
            if (this.ChannelList == null || this.ChannelList.Count == 0)
            {
                this.ChannelList = new NTDictionary<string, ChatHistoryData>();
            }

            ChatHistoryData chatHistoryData = this.ChannelList.Get(chatData.ChannelKey);
            if (chatHistoryData == null)
            {
                chatHistoryData = new ChatHistoryData();
                chatHistoryData.ChannelKey = chatData.ChannelKey;
                this.ChannelList.Add(chatData.ChannelKey, chatHistoryData);
            }

            if (chatHistoryData.Data == null || chatHistoryData.Data.Count == 0)
            {
                chatHistoryData.Data = new List<SendChatData>();
            }

            if (!this.IsContainsChatHistory(chatHistoryData, chatData))
            {
                chatHistoryData.Data.Add(chatData);
                chatHistoryData.Data.Sort((a, b) => a.TimeSending.CompareTo(b.TimeSending));

                if (chatHistoryData.Data.Count > MAX_CHAT_HISTORY)
                {
                    chatHistoryData.Data.RemoveAt(0);
                }


                if (this.NotifyChatCount.Get(chatData.ChannelKey) < chatData.TimeSending)
                {
                    this.UpdateNotifyChat(chatData.ChannelKey, chatData.TimeSending);
                    
                    if (chatData.ChannelKey.IndexOf("Chat_Server_") != -1)
                    {
                        this.UpdateNotifyChat("Chat", chatData.TimeSending);
                        this.UpdateNotifyChat("Chat_Server_", chatData.TimeSending);
                    }
                    else if (chatData.ChannelKey.IndexOf("Chat_Clan_") != -1)
                    {
                        this.UpdateNotifyChat("Chat", chatData.TimeSending);
                        this.UpdateNotifyChat("Chat_Clan_", chatData.TimeSending);
                    }
                    else if (chatData.ChannelKey.IndexOf("Chat_Friend_") != -1)
                    {
                        this.UpdateNotifyChat("Chat_Friend_", chatData.TimeSending);
                    }
                }
                if (isHistory)
                {
                    EventListenerManager.instance.PostEventWithKey(EventCode.Chat_ReceiveHistoryChat, chatData.ChannelKey, (object)chatData);
                }
                else
                {
                    EventListenerManager.instance.PostEventWithKey(EventCode.Chat_ReceiveChat, chatData.ChannelKey, (object)chatData);
                }

                chatHistoryData.UpdateData();
            }
        }

        public void UpdateNotifyChat(string channelKey, long timeSending)
        {
            if (!this.NotifyChatList.Datas.Contains(channelKey))
            {
                this.NotifyChatList.Datas.Add(channelKey);
            }
            this.NotifyChatCount.Add(channelKey, timeSending);
            this.NotifyChat.Add(channelKey, true);
            PlayerPrefs.SetString(NOTIFY_CHAT_LIST, JsonUtility.ToJson(NotifyChatList));
            PlayerPrefs.SetInt(NOTIFY_CHAT_BOOL + channelKey, this.IsMuted() ? 0 : 1);
            PlayerPrefs.SetInt(NOTIFY_CHAT_COUNT + channelKey, (int)timeSending);
            EventListenerManager.instance.PostEventWithKey(EventCode.Chat_UpdateNotifyChat, channelKey, !this.IsMuted());
        }

        public void HideNotifyChat(string channelKey)
        {
            this.NotifyChat.Add(channelKey, false);
            PlayerPrefs.SetInt(NOTIFY_CHAT_BOOL + channelKey, 0);
        }

        public void UpdateChatHistory(ChatHistoryData chatHistoryDataInput)
        {
            NTLog.LogMessage("UpdateChatHistory: " + JsonUtility.ToJson(chatHistoryDataInput));
            foreach (SendChatData chatData in chatHistoryDataInput.Data)
            {
                this.ReceiveChat(chatData, true);
            }
        }

        public void SendChat(string channelKey, string text, bool isSaveChat = true)
        {
            if (text == "") return;
            ChatMsg chatMsg = new ChatMsg();
            chatMsg.Type = ChatMsgType.SendChat;
            SendChatData sendChatData = new SendChatData(channelKey, text, -1, UserDataManager.Instance.GetUserID(), "", isSaveChat);
            sendChatData.TimeSending = ServerManager.Instance.GetTimeServer();

            PlayerChatData playerChatData = new PlayerChatData(
                UserDataManager.Instance.GetUserID(),
                UserDataManager.Instance.GetDisplayName(),
                UserProfileManager.Instance.GetAvatarUsedIndex(),
                UserProfileManager.Instance.GetAvatarBorderUsedIndex(),
                UserDataManager.Instance.GetLevel()
            );

            sendChatData.Data = JsonUtility.ToJson(playerChatData);
            chatMsg.Data = JsonUtility.ToJson(sendChatData);
            MsgDeliveryRoom.Instance.SendMessage(MsgDeliveryKey.ChatMsg, chatMsg);
        }

        public void SendChatWithEmoji(string channelKey, int emoji, bool isSaveChat = true)
        {
            if (emoji < 0) return;
            ChatMsg chatMsg = new ChatMsg();
            chatMsg.Type = ChatMsgType.SendChat;
            SendChatData sendChatData = new SendChatData(channelKey, "", emoji, UserDataManager.Instance.GetUserID(), "", isSaveChat);
            sendChatData.TimeSending = ServerManager.Instance.GetTimeServer();

            PlayerChatData playerChatData = new PlayerChatData(
                UserDataManager.Instance.GetUserID(),
                UserDataManager.Instance.GetDisplayName(),
                UserProfileManager.Instance.GetAvatarUsedIndex(),
                UserProfileManager.Instance.GetAvatarBorderUsedIndex(),
                UserDataManager.Instance.GetLevel()
            );

            sendChatData.Data = JsonUtility.ToJson(playerChatData);
            chatMsg.Data = JsonUtility.ToJson(sendChatData);
            MsgDeliveryRoom.Instance.SendMessage(MsgDeliveryKey.ChatMsg, chatMsg);
        }

        public void GetChatHistory(string channelKey)
        {
            ChatMsg chatMsg = new ChatMsg();
            chatMsg.Type = ChatMsgType.GetChatHistory;
            chatMsg.Data = JsonUtility.ToJson(new ChatHistoryData() { ChannelKey = channelKey });
            MsgDeliveryRoom.Instance.SendMessage(MsgDeliveryKey.ChatMsg, chatMsg);
        }

        public void MuteChat()
        {
            PlayerPrefs.SetInt(CHAT_MUTED, this.IsMuted() ? 0 : 1);
        }

        #region Get
        public string GetServerChatChannel()
        {
            return "Chat_Server_" + UserDataManager.Instance.GetServerPlay();
        }

        public string GetClanChatChannel(){
            if(ClanManager.Instance.IsClan()){
                return "Chat_Clan_" + ClanManager.Instance.GetClanID();
            }
            return null;
        }

        public ChatHistoryData GetChatHistoryData(string channelKey)
        {
            return this.ChannelList.Get(channelKey);
        }

        public bool IsMuted()
        {
            return PlayerPrefs.GetInt(CHAT_MUTED, 0) == 1;
        }

        public bool IsRegisterChannel(string channelKey)
        {
            return MsgDeliveryRoom.Instance.IsRegisterChannel(channelKey);
        }

        #endregion

        #region Other
        private bool IsContainsChatHistory(ChatHistoryData chatHistoryData, SendChatData chatData)
        {
            foreach (SendChatData chat in chatHistoryData.Data)
            {
                if (chat._id.Equals(chatData._id))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}

