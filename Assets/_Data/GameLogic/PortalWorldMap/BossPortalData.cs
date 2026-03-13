
using System.Collections.Generic;

namespace Rubik.Myrk.Portal
{
    using Rubik.BattleEngine;
    using Rubik.ItemPlayer;
    using Rubik.Myrk.GeoPoint;
    using Rubik.UserProfile;

    [System.Serializable]
    public class BossPortalData
    {
        public long TimeAttack;// Time for attack
        public long TimeClose;// Time for close
        public long PortalKey;
        public long MaxTimeUnlock;
        public long TimeRecoveryAttack;
        public long MaxTimeAttack;
        public long TimeAttackDailyIncrease;
        public BossData[] BossData;
        public RankBossReward[] RankBossReward;
    }

    [System.Serializable]
    public class BossData
    {
        public int Level;
        public long HP;
        public int ATK;
        public int RewardMultiplier;
        public int FixRewardMultiplier;
    }

    [System.Serializable]
    public class RankBossReward
    {
        public int RankMin;
        public int RankMax; // -1 mean max
        public ItemData[] Reward;
        public ItemData[] FixReward;
    }

    [System.Serializable]
    public class PortalAttackData
    {
        public long PointID;
        public int GroupServer;
        public int Version;
        public int Level;
        public long HP;
        public long OpenTime;
        public string Owner;

        public long LastGetTime;
    }

    [System.Serializable]
    public class PortalBossAttackRespone
    {
        public long AmountPortalBossAttack;
        public long LastTimeRecovery;
        public long AmountPortalBossUnlock;
        public long HighestLevelPortalBoss;
    }

    [System.Serializable]
    public class PortalMsg
    {
        public string _id;
        public PortalMsgType Type;
        public string Data;
    }

    public enum PortalMsgType
    {
        GetPortalBossRank = 1,
        GetPortalAttackData = 2,
    }

    [System.Serializable]
    public class SendMsgGetPortalData
    {
        public long PointID;
        public int GroupServer;
        public string UserID;
    }

    [System.Serializable]
    public class SendMsgGetPortalAttackData
    {
        public long PointID;
        public int GroupServer;
    }

    [System.Serializable]
    public class PortalBossRank
    {
        public long PointID;
        public int GroupServer;
        public int PlayerRank;
        public long PlayerDamage;

        public long HP;

        public UserRank[] Rank;

        public long LastGetTime;
    }

    [System.Serializable]
    public class PortalHistory
    {
        public long PointID;
        public int Version;
        public int Level;
        public long OpenTime;
    }

    [System.Serializable]
    public class PortalHistoryRespone
    {
        public string _id;
        public PortalHistory[] PortalHistories;
    }

    [System.Serializable]
    public class PortalBossBattleResult
    {
        public string _id;
        public BattleShortData Result;
        public bool IsWin;
    }

    [System.Serializable]
    public class PortalRandomResponse{
        public List<GeoPoint> GeoPoints = new List<GeoPoint>();
        public long LastTimeFindGeoPoint;
    }
}
