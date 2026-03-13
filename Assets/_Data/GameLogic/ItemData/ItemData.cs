using System.Collections.Generic;
using Rubik.CardPlayer;
using UnityEngine;

namespace Rubik.ItemPlayer
{
    [System.Serializable]
    public enum ItemType
    {
        Adv = 0,
        Coin = 1,
        Energy = 2,
        Cake = 3,
        Gem = 4,
        HonorPoint = 7,
        GearStone = 8,
        ExpPlayer = 9,
        Fruit = 15,
        KeyPortal = 16,
        ArenaTicket = 23,

        // Summon Ticket

        NormalSummonCardTicket = 12,
        PremiumSummonCardTicket = 13,
        UltraSummonCardTicket = 14,

        // Chest
        WoodChest = 17,
        BronzeChest = 18,
        SilverChest = 19,
        GoldChest = 20,
        ExpChest = 21,
        BattlePassPoint = 22,

        // Card Player
        Card_0 = 1000,
        Card_1 = 1001,
        Card_2 = 1002,
        Card_3 = 1003,
        Card_4 = 1004,
        Card_5 = 1005,
        Card_6 = 1006,
        Card_7 = 1007,
        Card_8 = 1008,
        Card_9 = 1009,
        Card_10 = 1010,
        Card_11 = 1011,
        Crep_0 = 1500,
        Crep_1 = 1501,
        Crep_2 = 1502,
        Crep_3 = 1503,
        Crep_4 = 1504,
        Crep_5 = 1505,
        Crep_6 = 1506,
        Crep_7 = 1507,
        Crep_8 = 1508,
        Crep_9 = 1509,

        // Card Player
        ShardCard_0 = 2000,
        ShardCard_1 = 2001,
        ShardCard_2 = 2002,
        ShardCard_3 = 2003,
        ShardCard_4 = 2004,
        ShardCard_5 = 2005,
        ShardCard_6 = 2006,
        ShardCard_7 = 2007,
        ShardCard_8 = 2008,
        ShardCard_9 = 2009,

        ShardCrep_0 = 2500,
        ShardCrep_1 = 2501,
        ShardCrep_2 = 2502,
        ShardCrep_3 = 2503,
        ShardCrep_4 = 2504,
        ShardCrep_5 = 2505,
        ShardCrep_6 = 2506,
        ShardCrep_7 = 2507,
        ShardCrep_8 = 2508,
        ShardCrep_9 = 2509,

        // Gift
        SampleGift_Red = 3000,
    }

    [System.Serializable]
    public class ItemData
    {
        public ItemType Type;
        public long Amount;

        public ItemData()
        {

        }

        public ItemData(ItemType type, long amount)
        {
            this.Type = type;
            this.Amount = amount;
        }
    }

    [System.Serializable]
    public class ItemDataInfo
    {
        public ItemType Type;
        public string ImagePath;

        // Food
        public int Energy;

        // Function
        public bool IsSell;
        public bool IsUse;

        // Sell
        public ItemData Price;

        // Merge
        public bool IsMerge;
        public ItemData[] MergeRequire;
        public ItemData MergeResult;

        // Combine
        public bool IsCombine;
        public ItemData[] CombineRequire;
        public CardPlayerIndex CombineResult;

        // Cache
        public Sprite Icon;
    }

    [System.Serializable]
    public class ItemRate
    {
        public ItemType Type;
        public int Amount;
        public float Rate;
    }

    public class RewardItem_Rate
    {
        public List<ItemData> Items;
        public List<ItemRate> Rates;

        public RewardItem_Rate()
        {
            this.Items = new List<ItemData>();
            this.Rates = new List<ItemRate>();
        }
    }

    [System.Serializable]
    public class ItemRange
    {
        public ItemType Type;
        public int Min;
        public int Max;
    }

    [System.Serializable]
    public class ListItemRate
    {
        public ItemType[] Types;
        public float Rate;
        public int Amount;

        public CardPlayerIndex[] CardPlayerIndexes;
    }


    [System.Serializable]
    public class AmountRate
    {
        public int Amount;
        public float Rate;
    }

    [System.Serializable]
    public class ExchangeItem
    {
        public string _id;
        public ItemType Type;
        public int Cap;
        public ItemData ExchangeRequire;
        public ItemData ExchangeResult;
    }

    [System.Serializable]
    public class ExchangeItemAdv
    {
        public string _id;
        public ItemType Type;
        public int Cap;
        public ItemData Result;
    }

    [System.Serializable]
    public class UserExchange
    {
        public string _id;
        public int Amount;
    }

}