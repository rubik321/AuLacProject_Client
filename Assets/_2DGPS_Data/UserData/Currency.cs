namespace Rubik._2DGPS.UserData
{
    [System.Serializable]
    public enum CurrencyType
    {
        Coin = 0,
        GoldBar = 1,
        SummonToken = 2,
        PromotionGem = 3,
        Iron = 4,
        NightmareCrystal = 5,

        TorchWooden = 6,
        TorchBronze = 7,
        TorchGolden = 8,

        WoodChest = 9,
        BronzeChest = 10,
        GoldChest = 11,
        PlatinumChest = 12,
        DiamondChest = 13,
        ScoreChest = 27,

        ArenaTicket = 14,

        RandEpicShard = 15,
        RandLegendaryShard = 16,
        RandMythicShard = 17,
        OmniEpicShard = 18,
        OmniLegendaryShard = 19,
        OmniMythicShard = 20,

        Prism = 21,

        TowerEnergy = 22,

        CommonRod = 23,
        GoldenRod = 24,

        Jade = 25,

        Wrench = 26,
    }

    [System.Serializable]
    public class CurrencyData
    {
        public CurrencyType Type;
        public int Amount;
    }
}