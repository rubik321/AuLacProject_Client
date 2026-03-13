using UnityEngine;

namespace Rubik.Myrk.PlayerChest
{
    using ItemPlayer;

    [System.Serializable]
    public class PlayerChestData
    {
        public ItemType Type;
        public long TimeUnlock;
        public long Exp;
        public ItemData[] ItemRewards;
        public ListItemRate[] ShardCardRewards;
    }

    [System.Serializable]
    public class PlayerChestSlot
    {
        public int Index;
        public ItemType ChestType;
        public long TimeClaim;
        public bool Unlocking = false;
        public int TimeAdv = 0;
    }

    [System.Serializable]
    public class PlayerChestSlotData
    {
        public int Index;
        public RequireUnlock RequireUnlock;
    }

    [System.Serializable]
    public class RequireUnlock
    {
        public int Level;
        public ItemData[] Price;
    }

    [System.Serializable]
    public class PlayerChestLevelData
    {
        public int Level;
        public long Exp;
        public bool Max;
        public float Inc;
    }


    [System.Serializable]
    public class PlayerChestResponse
    {
        public string _id;
        public int Level;
        public long Exp;
        public PlayerChestSlot[] PlayerChestSlot;

    }

    [System.Serializable]
    public class PlayerChestDecreaseTime
    {
        public int GemDecreaseTime;
        public int AdvDecreaseTime;
        public int LimitTimeAdv;
    }
}
