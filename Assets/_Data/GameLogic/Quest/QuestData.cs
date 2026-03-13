
namespace Rubik.Quest
{
    using ItemPlayer;

    [System.Serializable]
    public enum QuestIndex
    {
        DailyQuest_CompleteAll,
        DailyQuest_LogIn,
        DailyQuest_Summon,
        DailyQuest_WinBattleMap,
        DailyQuest_OpenChest,

        ClanQuest_AttackBoss,
        ClanQuest_Donate,
        ClanQuest_Chat,
        ClanQuest_BuyClanShop,
    }

    [System.Serializable]
    public enum QuestType
    {
        Daily = 0,
        Clan = 1,
    }

    [System.Serializable]
    public class DailyQuestPlayer
    {
        public QuestIndex Index;
        public int Process;
        public bool Reward;
    }

    [System.Serializable]
    public class DailyQuestPlayerData
    {
        public QuestIndex Index;
        public int Max;
        public QuestType Type;
        public ItemData[] ItemRewards;
    }
}