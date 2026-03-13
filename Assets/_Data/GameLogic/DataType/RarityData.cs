namespace Rubik.DataType
{
    [System.Serializable]
    public enum RarityType
    {
        Common = 0,
        Uncommon = 1,
        Rare = 2,
        Epic = 3,
        Legendary = 4,
    }

    [System.Serializable]
    public class TierRate
    {
        public RarityType Tier;
        public float Rate;
    }

    [System.Serializable]
    public class RarityValue{
        public RarityType Rarity;
        public long Value;
    }

}
