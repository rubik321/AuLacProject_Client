

namespace Rubik.CharacterGear
{
    using System.Collections.Generic;
    using DataType;
    using Rubik.CardPlayer;
    using Rubik.ItemPlayer;
    using Rubik.Myrk.BattleTeam;
    using UnityEngine;

    [System.Serializable]
    public class CharacterGear
    {
        public string _id;
        public string UserID;
        public CharacterGearIndex Index = CharacterGearIndex.None;

        public int Lv;
        public int Star;
        public RarityType Rarity;

        public OriginType Origin;
        public ClassType Class;

        public bool Delete;


        //Cache
        public List<int> Teams;
        public bool IsEquiped
        {
            get
            {
                return Teams.Count > 0;
            }
        }
        public bool IsEquipedByIndex(int index)
        {
            return Teams.Contains(index);
        }

        public CharacterGearData GearData;
        // Stats
        public GearStats GearStats;

        // Stats Next Level
        public GearStats GearStatsNextLevel;

        public void UpdateGear(CharacterGear gear)
        {
            this.UserID = gear.UserID;
            this.Index = gear.Index;
            this.Lv = gear.Lv;
            this.Star = gear.Star;
            this.Rarity = gear.Rarity;
            this.Class = gear.Class;
            this.Origin = gear.Origin;
            this.Class = gear.Class;
            this.Delete = gear.Delete;
        }

        public int Slot()
        {
            return this.GearData.Slot;
        }
    }

    [System.Serializable]
    public class CharacterGearUpgradeLvData
    {
        public int Level;
        public ItemData[] Cost;
        public bool Max;
        public float UpStats;
        public long HeroATK;
        public long TeamHP;
    }

    [System.Serializable]
    public class CharacterGearData
    {
        public CharacterGearIndex Index;
        public CharacterGearType Type;
        public int Slot;
        public OriginType Origin;

        public TypeActiveGear SkillActive;
        public TypePassiveGear SkillPassive;
        // Stats
        public GearStats GearStats;

    }

    [System.Serializable]
    public class ChracterGearRarity
    {
        public CharacterGearIndex Index;
        public RarityType Rarity;
    }

    [System.Serializable]
    public enum CharacterGearType
    {

        Hair = 0,
        Armor = 1,
        Weapon = 2,

        Shield = 3,
        Bracelet = 4,
        Ring = 5,
        Book = 6,
    }

    public enum CharacterGearIndex
    {
        None = -1,
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

    [System.Serializable]
    public class CharacterGearSpriteData
    {
        public CharacterGearIndex Index;
        public string SpriteName;
        public Sprite Sprite;
    }

    [System.Serializable]
    public enum TypeActiveGear
    {
        None = 0,
        Summons_a_storm_of_spectral_swords_unleashing_a_surge_of_Metal_energy_that_deals_0_of_ATK_damage_to_all_enemies = 1,
        Unleashes_a_ground_shaking_explosion_dealing_Earth_Element_0_of_ATK_damage_to_all_enemies = 2,
        Slashes_through_enemies_with_a_blazing_sickle_engulfing_them_in_flames_dealing_0_of_ATK_Fire_damage_to_all_enemies = 3,
    }

    [System.Serializable]
    public enum TypePassiveGear
    {
        None = 0,
        Armor_Break = 1,
        Damage_Reduction = 2,
        Block = 3,
        Armor_Break_Resistance = 4,
        Crit = 5,
        Block_Resistance = 6,
        Skill_Damage = 7,
        Crit_Resistance = 8,
        Damage = 9,
        Skill_Damage_Reduction = 10,
        Metal_Power = 11,
        Earth_Power = 12,
        Fire_Power = 13,
        Water_Power = 14,
        Wood_Power = 15,
    }


    [System.Serializable]
    public class GearSkillPassive
    {
        public TypePassiveGear Index;
        public string Detail;
        public string Image;
        public int MaxLv;
        public SkillValueByLevel[] Value;
    }

    [System.Serializable]
    public class GearSkillActive
    {
        public TypeActiveGear Index;
        public string Detail;
        public string Image;
        public int MaxLv;
        public SkillValueByLevel[] Value;
    }

    [System.Serializable]
    public class GearSkillActiveLv
    {
        public TypeActiveGear Index;
        public int Level;
        public RarityType Rarity;
    }

    [System.Serializable]
    public class GearSkillPassiveLv
    {
        public TypePassiveGear Index;
        public int Level;
        public RarityType Rarity;
    }

    [System.Serializable]
    public class GearSkillLv
    {
        public GearSkillActiveLv Active;
        public List<GearSkillPassiveLv> Passive = new List<GearSkillPassiveLv>(); 
    }

    [System.Serializable]
    public class CharacterGearRarityBonus
    {
        public RarityType Rarity;
        public float Bonus;
        public GearStats Stat = new GearStats();
        public int SkillLevel;
    }
}
