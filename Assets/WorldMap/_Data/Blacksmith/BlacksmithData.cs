
using System.Collections.Generic;
using GOA.Item;

namespace Rubik.GOA.Blacksmith
{
    [System.Serializable]
    public class ListUpgradeLevelData
    {
        public List<UpgradeLevelData> Data = new();
    }
    [System.Serializable]
    public class UpgradeLevelData
    {
        public float Level;
        public float Rate;
        public float Boost;
        public float BoostRate;
        public float Mat_1;
        public float Mat_2;
        public float Gold;
        public float Gin;
        public float Stats_Increase;
    }

    [System.Serializable]
    public class GearTypeData
    {
        public float Type;
        public float IndexUpgrade;
        public float Mat_1;
        public float Mat_2;
    }

    [System.Serializable]
    public class FusionGear
    {
        public float Rarity;
        public float Rate;
        public float FusionStones;
        public float Gold;
        public float GIN;
        public float Boost;
        public float BoostRate;
    }

    [System.Serializable]
    public class ResultFusionGear
    {
        public GearData Gear;
        public string[] GearLost;
        public ItemValue[] ItemUpdates;
        public bool Success;
    }

    [System.Serializable]
    public class FragmentData{
        public int Rarity;
        public int Mat_1_Min;
        public int Mat_1_Max;
        public int Mat_2_Min;
        public int Mat_2_Max;
        public int Mat_3_Min;
        public int Mat_3_Max;
    }

}

