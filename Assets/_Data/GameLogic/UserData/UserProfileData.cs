using System.Collections.Generic;

namespace Rubik.UserProfile
{
    using Rubik.UserDataPlayer;
    using Rubik.ItemPlayer;

    public enum UnlockType
    {
        None = 0,
        Level = 1,
        Purchase = 2,
        Event = 3,
    }

    [System.Serializable]
    public class UnlockData
    {
        public UnlockType UnlockType;
        public int LevelUnlock;
        public ItemData[] Price;
        public ItemData[] PriceInstead;
    }

    [System.Serializable]
    public class AvatarData
    {
        public int Index;
        public string Sprite;
        public UnlockData UnlockData;
        public bool Lock;
    }

    [System.Serializable]
    public class AvatarPlayer
    {
        public List<int> Own;
        public int Current = -2;
    }

    [System.Serializable]
    public class AvatarBorderData
    {
        public int Index;
        public string Sprite;
        public UnlockData UnlockData;
        public bool Lock;
    }

    [System.Serializable]
    public class AvatarBorderPlayer
    {
        public List<int> Own;
        public int Current = -1;
    }

    [System.Serializable]
    public class EmojiData
    {
        public int Index;
        public bool Lock;
        public UnlockData UnlockData;
    }

    [System.Serializable]
    public class EmojiPlayer
    {
        public List<int> Own;
    }

    [System.Serializable]
    public class UserRank
    {
        public string UserID;
        public int Rank;
        public long Score;
    }

    [System.Serializable]
    public class UserRankResponse
    {
        public string _id;
        public int Rank;
        public long Score;
        public UserRank[] UserRanks;
    }

    public enum TutorialType
    {
        Tutorial0 = 0,
        Tutorial1 = 1,
        Tutorial2 = 2,
        Tutorial3 = 3,
        Tutorial4 = 4,
        Tutorial5 = 5,
        Tutorial6 = 6,
        Tutorial7 = 7,
        Tutorial8 = 8,
        Tutorial9 = 9,
        Tutorial10 = 10,
        Tutorial11 = 11,
        Tutorial12 = 12,
        Tutorial13 = 13,
        Tutorial14 = 14,
        Tutorial15 = 15,
        Tutorial16 = 16,
    }

    public enum LockFunctionType
    {
        Mail,//Leve 0
        Hero,//Level 59
        LineUp,//Level 0
        Quest,//Level 1
        Bag,//Level 1
        Monster,// Level 1
        Market,// Level 4,
        Clan,// Level 4,
        Arena,// Level 19,
        Summon,// Level 2,
        None,
        Chest,// Level 2,
        BattlePass,// Level 2,
    }

    public class LockFunction
    {
        public LockFunctionType Type;
        public int LevelUnlock;
    }
}