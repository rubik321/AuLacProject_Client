using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.Myrk.Clan
{
    using ItemPlayer;
    public enum ClanStatus
    {
        NoClan = 0,
        Apply = 1,
        Joined = 2,
    }

    public enum ClanRole
    {
        /**
     * Thành viên thường
     */
        MEMBER = 0,

        CAPTAIN = 1,
        /**
         * Phó bang
         */
        COLEADER = 2,
        /**
         * Chủ bang
         */
        LEADER = 3

    }

    [System.Serializable]
    public class UserClan
    {
        public string _id;
        public string UserId;
        public string ClanId;
        public long Fund;            //điểm đóng góp cho Clan hiện tại

        public ClanStatus Status;
        public ClanRole Role;
        public long TimeJoin;

        public long LastDonateDate;
    }

    [System.Serializable]
    public class Clan
    {
        public string _id;

        public string Name; //tên clan
        public int Icon; //icon clan 
        public int Frame; // khung viền
        public int Color; // màu nền
        public string Slogan;
        public string Announce;
        public string ChiefId;

        public int GroupServer;
        public bool AutoAcceptMember;
        public long Fund;

        public int MaxMember;
        public int Member;
        public int Level;
        public long Exp;
        public long ExpNextLevel;

        // Boss
        public int BossTimeAttack;

        // Version
        public long DayVersion;


        public long TimeCreated;
        public bool Deleted;
    }


    [System.Serializable]
    public class ClanData
    {
        public ItemData[] CreateCost;
        public ItemData[] EditCost;
        public LevelClanData[] LevelData;
        public long CoolDownJoinClan;

        public ClanDonate[] Donate;
    }

    [System.Serializable]
    public class LevelClanData
    {
        public int Level;
        public long Exp;
        public int MaxMember;
        public int MaxCaptain;
        public int MaxDeputy;
        public bool Max;
    }

    [System.Serializable]
    public class ClanDonate
    {
        public ClanDonateType Type;
        public long Amount;
        public ItemData PayItem;
        public ItemData OffHonorPoint;
    }

    [System.Serializable]
    public enum ClanDonateType
    {
        Normal = 0,
        Royal = 1,
        Premium = 2,
    }

    [System.Serializable]
    public class ClanInfo
    {
        public string _id;
        public string Name;
        public int Icon;
        public int Frame;
        public int Color;
        public int MaxMember;
        public int Member;
        public string Slogan;
        public string Announce;
        public int Level;
        public long Fund;
        public bool AutoAccept;
        public string ChiefID;


        // Cache
        public long LastUpdate;

        public ClanInfo(string _id, string Name, int Icon, int Frame, int Color, int MaxMember, int Member, string Slogan, string Announce, int Level, long Fund, bool AutoAccept, string ChiefID)
        {
            this._id = _id;
            this.Name = Name;
            this.Icon = Icon;
            this.Frame = Frame;
            this.Color = Color;
            this.MaxMember = MaxMember;
            this.Member = Member;
            this.Slogan = Slogan;
            this.Announce = Announce;
            this.Level = Level;
            this.Fund = Fund;
            this.AutoAccept = AutoAccept;
            this.ChiefID = ChiefID;
        }
    }

    [System.Serializable]
    public class ListClanMemberInfo
    {

        public ClanMemberInfo[] MemberList;
        public string ClandID;
        // Cache
        public long LastUpdate;
    }

    [System.Serializable]
    public class ClanMemberInfo
    {
        public string _id;
        public string UserId;
        public ClanRole Role;
        public long FundDonate;
        public int Avatar;
        public int AvatarBorder;
        public string DisplayName;
        public int Level;
        public long LastLogin;
        public ClanStatus Status;

        public ClanMemberInfo(string _id, string UserId, ClanRole Role, long FundDonate, int Avatar, int AvatarBorder, string DisplayName, int Level, long LastLogin, ClanStatus Status)
        {
            this._id = _id;
            this.UserId = UserId;
            this.Role = Role;
            this.FundDonate = FundDonate;
            this.Avatar = Avatar;
            this.AvatarBorder = AvatarBorder;
            this.DisplayName = DisplayName;
            this.Level = Level;
            this.LastLogin = LastLogin;
            this.Status = Status;
        }
    }

    public class ClanCreateData
    {
        public string Name;
        public int Icon = 0;
        public int Frame = 0;
        public int Color = 0;
        public string Slogan = "";
        public bool AutoAcceptMember = false;

        public ClanCreateData(string name)
        {
            this.Name = name;
        }
    }

    [System.Serializable]
    public class UserClanDonate
    {
        public string UserID;
        public string DisplayName;
        public ClanDonateType DonateType;
        public int Point;
    }

    [System.Serializable]
    public class ClanRank{
        public string ClanID;
        public long Score;
    }


    public enum ClanMsgType
    {
        ClanRequest = 1,
        ClanRequestAccept = 2,
        ClanRequestReject = 3,
        ClanNewMember = 4,
        ClanDonate = 5,
        ClanNewAnnounce = 6,
        ClanJoin = 7,
        ClanLeave = 8,
        ClanDelete = 9,
    }

    [System.Serializable]
    public class ClanMsg
    {
        public ClanMsgType Type;
        public string Data;
    }
}