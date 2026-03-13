namespace Rubik._2DGPS.Character
{
    using DataType;
    using UserData;

    [System.Serializable]
    public class Character
    {
        public string _id;
        public string UserID;
        public CharacterIndex Index;

        public int Lv;
        public int Star;
        public RarityType Rarity;

        public CharacterGear[] GearIDs;

        public void UpdateCharacter(Character character)
        {
            this.UserID = character.UserID;
            this.Index = character.Index;
            this.Lv = character.Lv;
            this.Star = character.Star;
            this.Rarity = character.Rarity;
            this.GearIDs = character.GearIDs;
        }
    }

    [System.Serializable]
    public class CharacterGear
    {
        public string GearID;
        public int Slot;
    }

    [System.Serializable]
    public class CharacterLvCost
    {
        public CurrencyData Cost;
        public float Rate;
    }

    public enum CharacterIndex
    {
        Character_0 = 0,
        Character_1 = 1,
        Character_2 = 2,
        Character_3 = 3,
        Character_4 = 4,
        Character_5 = 5,
        Character_6 = 6,
        Character_7 = 7,
        Character_8 = 8,
        Character_9 = 9,
        Character_10 = 10,
        Character_11 = 11,
    }
}