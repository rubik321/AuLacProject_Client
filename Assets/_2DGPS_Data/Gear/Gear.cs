

namespace Rubik._2DGPS.Gear
{
    using DataType;
    using UserData;

    [System.Serializable]
    public class Gear
    {
        public string _id;
        public string UserID;
        public GearIndex Index;

        public int Lv;
        public int Star;
        public RarityType Rarity;

        public OriginType Origin;
        public ClassType Class;

        public string CharacterID;

        //Cache
        public GearData GearData;

        public void UpdateGear(Gear gear){
            this.UserID = gear.UserID;
            this.Index = gear.Index;
            this.Lv = gear.Lv;
            this.Star = gear.Star;
            this.Rarity = gear.Rarity;
            this.Class = gear.Class;
            this.Origin = gear.Origin;
            this.Class = gear.Class;
            this.CharacterID = gear.CharacterID;
            this.GearData = GearManager.instance.GetGearDataByIndex(this.Index);
        }

        public int Slot()
        {
            return this.GearData.Slot;
        }
    }

    [System.Serializable]
    public class GearLvCost
    {
        public CurrencyData Cost;
        public float Rate;
        public bool Max;
    }

    [System.Serializable]
    public class GearData
    {
        public GearIndex Index;
        public int Slot;
    }

    public enum GearIndex
    {
        Gear_0 = 0,
        Gear_1 = 1,
        Gear_2 = 2,
        Gear_3 = 3,
        Gear_4 = 4,
        Gear_5 = 5,
        Gear_6 = 6,
        Gear_7 = 7,
        Gear_8 = 8,
        Gear_9 = 9,
        Gear_10 = 10,
        Gear_11 = 11,
        Gear_12 = 12,
        Gear_13 = 13,
        Gear_14 = 14,
        Gear_15 = 15,
        Gear_16 = 16,
        Gear_17 = 17,
    }
}
