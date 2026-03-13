using UnityEngine;


namespace Rubik.Myrk.BattlePass
{
    using Rubik.ItemPlayer;

    [System.Serializable]
    public class BattlePassUpdate
    {
        public int BattlePassPoint;
        public int BattlePassVersion;
        public bool[] BattlePassFreeClaimed;
        public bool[] BattlePassPremiumClaimed;
        public BattlePassType[] BattlePassUnlocked;
    }

    [System.Serializable]
    public enum BattlePassType
    {
        Free = 0,
        Premium = 1,
    }

    [System.Serializable]
    public class BattlePassData
    {
        public BattlePassPath Free;
        public BattlePassPath Premium;
    }

    [System.Serializable]
    public class BattlePassPath
    {
        public BattlePassType BattlePassType;
        public ItemData[] UnlockPrice;
        public BattlePassReward[] Reward;
    }

    [System.Serializable]
    public class BattlePassReward
    {
        public int Level;
        public int ExpRequire;
        public ItemData[] RewardItems;
    }

    [System.Serializable]
    public class BattlePassRewardClaimData
    {
        public BattlePassType Type;
        public int Level;
    }

    public enum BattlePassStatus
    {
        NotClaim = 0,
        Claim = 1,
        Claimed = 2,
        Locked = 3,
    }

}