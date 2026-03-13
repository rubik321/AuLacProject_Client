using Rubik.BattleEngine;
using Rubik.UserDataPlayer;
using UnityEngine;

namespace Rubik.Myrk.Arena
{

    using Rubik.ItemPlayer;
    [System.Serializable]
    public enum RankingType
    {
        BronzeI,
        BronzeII,
        BronzeIII,
        SilverI,
        SilverII,
        SilverIII,
        GoldI,
        GoldII,
        GoldIII,
        PlatinumI,
        PlatinumII,
        PlatinumIII,
    }

    [System.Serializable]
    public class RankingData
    {
        public long Index;
        public RankingType Type;
        public long MinPoint;
        public long MaxPoint;
        public bool Decrease;
        public ItemData[] RewardItems;
    }

    [System.Serializable]
    public class ArenaData
    {
        public long MaxArenaTicket;
        public long ArenaTicketRecover;
        public long ArenaTicketOffer;
        public long FreeReset;
        public long AmountOpponent;
        public long WinHigherRank;
        public long WinLowerRank;
        public long LoseRank;
        public ItemData ResetCost;
        public ItemData LoseReward;
        public ItemRate[] WinReward;
    }

    [System.Serializable]
    public class ArenaResponse
    {
        public long ArenaVersion;
        public long ArenaTicket;
        public long ArenaTicketRecover;
        public long LastTimeReset;
        public long TimeAdvReset;
        public OpponentData[] Opponents;
        public long Score;
        public ArenaHistory[] History;
    }

    [System.Serializable]
    public class OpponentData
    {
        public string UserID;
        public long Score;
        public UserDataShort UserData;
        public bool IsWin;
    }

    [System.Serializable]
    public class ArenaBattleResult
    {
        public string _id;
        public bool IsWin;
        public ItemData[] Reward;
        public long Bonus;
        public OpponentData Opponent;
        public BattleShortData Result;
    }

    [System.Serializable]
    public enum ArenaHistoryType
    {
        Attack,
        Defense,
    }

    [System.Serializable]
    public class ArenaHistory
    {
        public string _id;
        public OpponentData Attacker;
        public OpponentData Defender;
        public ArenaHistoryType Type;
        public long BonusAttacker;
        public long BonusDefender;
        public long Time;

    }

    [System.Serializable]
    public class ArenaTicketResponse
    {
        public long Ticket;
        public long Recover;
    }
}
