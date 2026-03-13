using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOA.Item
{
    [System.Serializable]
    public class ItemValue
    {
        public ItemCode Type;
        public int Amount;
    }
    public enum ItemCode
    {
        Unknow = -1,
        Potion = 0,
        HiPotion = 1,
        Ether = 2,
        HiEther = 3,
        Elixir = 4,
        PoorLeather = 5,
        PortalKey = 6,
        GinPacked = 7,
        NameTag = 8,
        WoodenChest = 9,
        BronzeChest = 10,
        SilverChest = 11,
        GoldChest = 12,
        SaikiSeal = 13,
        MysticMandate = 14,

        ChieftainBlood = 15,
        HardLeather = 16,
        AbyssRoots = 17,
        DragonFlower = 18,
        ProtectorSoul = 19,
        DestroyerSoul = 20,
        VanguardSoul = 21,
        TritinumAlloy = 22,
        Ferrieliquid = 23,
        MetalPlate = 24,
        StonePile = 25,
        SilicatePowder = 26,
        UpgradeStone = 27,
        FusionStone = 28,

        Gin = 10001,
        Coin = 10002,

        WoodChest_FK = 30001,
        BronzeChest_FK = 30002,
        GoldChest_FK = 30003,
        PlatinumChest_FK = 30004,
        DiamondChest_FK = 30005,
        ScoreChest_FK = 30006,
        GoldBar_FK = 30007,
        PromotionGem_FK = 30008,
        Iron_FK = 30009,
        SummonToken_FK = 30010,
    }

    [System.Serializable]
    public class ItemInfoData
    {
        public string Index = "";
        public ItemCode Code;
        public string Name;
        public string Images;
        public string Description;
        public ItemTypeCode Type;
        public bool Shop;
        public float Price;
        public CurrencyType Currency;
        public int Amount;
        public bool AbleUse;
    }

    [System.Serializable]
    public class ItemGearData
    {
        public string Index;
        public int Lv;
        public float Price;
        public CurrencyType Currency;
    }

    public class UserItemData
    {
        public string userID;
        public int itemID;
        public string charID;
        public float MaxHP;
        public float MaxMana;
        public float currentHP;
        public float currentMana;
        public string token;

        public UserItemData(string userID, int itemID, string charID, float MaxHP, float MaxMana, float currentHP, float currentMana, string token)
        {
            this.userID = userID;
            this.itemID = itemID;
            this.charID = charID;
            this.MaxHP = MaxHP;
            this.MaxMana = MaxMana;
            this.currentHP = currentHP;
            this.currentMana = currentMana;
            this.token = token;
        }
    }
}
