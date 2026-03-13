using System.Collections.Generic;
using NTPackage.Functions;
using Rubik.Manager;

namespace Rubik._2DGPS.Chat
{
    [System.Serializable]
    public class ChatMsg
    {
        public string _id;
        public ChatMsgType Type;
        public string Data;

        public ChatMsg()
        {
            this._id = NTFunction.GenerateId();
        }
    }

    public enum ChatMsgType
    {
        SendChat = 0,
        GetChatHistory = 1,
    }

    [System.Serializable]
    public class SendChatData
    {
        public string _id;
        public string ChannelKey;
        public string Text;
        public int Emoji = -1;
        public string UserID;
        public long TimeSending;
        public int Version;
        public string Data; //Config for client
        public bool IsSave = true;
        public PortalBossData PortalBossData;
        
        public SendChatData(string channelKey, string text, int emoji, string userID, string data, bool isSave)
        {
            this._id = NTFunction.GenerateId();
            this.Version = 1;
            this.ChannelKey = channelKey;
            this.Text = text;
            this.Emoji = emoji;
            this.UserID = userID;
            this.Data = data;
            this.IsSave = isSave;
        }
    }

    [System.Serializable]
    public class GetChatHistoryData
    {
        public string ChannelKey;
    }

    [System.Serializable]
    public class ChatHistoryData
    {
        public string ChannelKey;
        public List<SendChatData> Data;

        public long LastTimeUpdate;


        public void UpdateData()
        {
            foreach (SendChatData chat in Data)
            {
                if (chat.TimeSending > LastTimeUpdate)
                {
                    LastTimeUpdate = chat.TimeSending;
                }
            }
        }
        public long GetLastTimeUpdate()
        {
            return ServerManager.Instance.GetTimeServer() - LastTimeUpdate > 0 ? ServerManager.Instance.GetTimeServer() - LastTimeUpdate : 0;
        }
    }

    [System.Serializable]
    public class PlayerChatData
    {
        public string UserID;
        public string DisplayName;
        public int Avatar;
        public int Border;
        public string Custom;
        public int Level;

        public PlayerChatData(string userID, string displayName, int avatar, int border, int level)
        {
            this.UserID = userID;
            this.DisplayName = displayName;
            this.Avatar = avatar;
            this.Border = border;
            this.Level = level;
        }
    }

    [System.Serializable]
    public class SharePortalData
    {
        public string PointID;
        public double longitude;
        public double latitude;
    }

    [System.Serializable]
    public class PortalBossData
    {
        public string Owner;
        public long PointID;
    }
}