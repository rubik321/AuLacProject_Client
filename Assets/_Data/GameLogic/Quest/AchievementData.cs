
namespace Rubik.Quest
{
    using ItemPlayer;
    using Rubik.CardPlayer;

    [System.Serializable]
    public enum AchievementPlayerIndex
    {
        // SummonTime,
        // LogInDay,
        // WinBattle,
        // OpenChest,
        // ArenaBattle,
        // ArenaRanking,
        // ClanBossAttack,
        // ClanDonate,
        // ClanBossDamage,
        // MonsterLevel,
        // MonsterStar,
        // MonsterPower,

        Founder = 1001,
        HeraldOfEchoes = 1002,
        GateInitiate = 1003,
        GateBreaker = 1004,
        ApprenticeTrainer = 1005,
        FortressGuardian = 1006,
        WardenOfTheRealm = 1007,
        IronSovereign = 1008,
        Kindled = 1009,
        Emberkeeper = 1010,
        OathboundScion = 1011,
        Initiate = 1012,
        Sparked = 1013,
        WoodenWarden = 1014,
        BronzeBreaker = 1015,
        SilverSeeker = 1016,
        GoldenHoarder = 1017,
        ExpertSummoner = 1018,
        Beastslayer = 1019,
        GaianHuntsman = 1020,
    }

    [System.Serializable]
    public class AchievementPlayer
    {
        public AchievementPlayerIndex Index;
        public int Process;
        public int Achive;
    }

    [System.Serializable]
    public class AchievementPlayerData
    {
        public AchievementPlayerIndex Index;
        public string ImagePath;
        public int[] Requireds;
        public AchievementPlayerReward[] Rewards;
        public bool Hidden = false;
    }

    [System.Serializable]
    public class AchievementPlayerReward
    {
        public ItemData[] Items;
        public CardPlayerIndex[] Cards;
    }

    [System.Serializable]
    public class AchievementBadge
    {
        public AchievementPlayerIndex Index;
        public bool Locked = false;
    }

    [System.Serializable]
    public class AchievementBadgePlayer
    {
        public int Equip = -1;
    }
}