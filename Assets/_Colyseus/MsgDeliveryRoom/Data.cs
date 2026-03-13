using System.Collections.Generic;

namespace Rubik.MsgDelivery
{
    [System.Serializable]
    public class PlayerJoinData
    {
        public string UserID = "";
        public string SessionAppID = "";

        public Dictionary<string, object> ToDictionary()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("UserID", UserID);
            data.Add("SessionAppID", SessionAppID);
            return data;
        }
    }

    [System.Serializable]
    public class MsgData
    {
        public MsgType Type;
        public string Data = "";
    }

    public enum MsgType
    {
        System = 0,
        DuplicateDevice = 1,

        Clan = 100,

        Friend = 200,
    }

    [System.Serializable]
    public class RegisterChannelData
    {
        public string UserID = "";
        public string ChannelID = "";
    }

    [System.Serializable]
    public class LeaveChannelData
    {
        public string UserID = "";
        public string ChannelID = "";
    }

    public enum MsgDeliveryType
    {
        System = 0,
        DuplicateDevice = 1,

        Clan = 100,
        ClanRequest = 101,
        ClanRequestAccept = 102,
        ClanRequestReject = 103,
        ClanNewMember = 104,
    }

    [System.Serializable]
    public class CheckUserOnlineData
    {
        public List<UserOnlineStatusData> UserOnlineStatus;
    }

    [System.Serializable]
    public class UserOnlineStatusData
    {
        public string UserID = "";
        public bool Status = false;
    }

    [System.Serializable]
    public class NoticeBarData
    {
        public string Title = "";
        public string Message = "";
        public string[] InjectionsString;
    }
}