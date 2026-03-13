using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Rubik.GOA.Blacksmith;
using GOA.Gear;
public class ModelConfigs : MonoBehaviour
{



}

public enum CharType
{
    WARRIOR = 0,
    THIFT = 1,
    MAGE = 2
}
[Serializable]
public class SkillDataStat
{
    public string Name;
    public int Index;
    public int CostType;
    public int Cost;
    public int SkillType;
    public int Target;
    public int StatSkill;
    public int Multiplier;
    public int DameType;
    public int SkillOrb;

}
[Serializable]
public class BaseDataStat
{
    public string StatKey;
    public float StatValue;
}
[Serializable]
public enum StatSkill
{
    MAX_HP = 0,
    MAX_MP = 1,
    STR = 2,
    VIT = 3,
    DEX = 4,
    SPD = 5,
    MND = 6,
    SPI = 7,
    HIT = 8,
    EVA = 9,
    CRI = 10,
    CRD = 11
}
[Serializable]
public class GearData
{
    public string _id;
    public string GearCode;
    public string GearName;
    public string Concept;
    public string Description;
    public int GearSlot;
    public string WearingPosition;
    public string Type;
    public int Level;
    public string classRestricted;
    public int Restricted;
    public int DMG;
    public int Slot;
    public int STR;
    public int MND;
    public float CRI;
    public float CRD;
    public float HIT;
    public float EVA;
    public int HP;
    public float HP_Per = 0;
    public int MP;
    public float MP_Per = 0;
    public int VIT;
    public int SPI;
    public int DEX;
    public int SPD;
    public bool Equiped;
    public int EquipedSlot;
    public int Rarity;
    public int UpgradeLv;
    public int Star;
    public BaseDataStat[] BaseStats;
    public BaseDataStat[] ModStats;
    public BaseDataStat[] OptionStats;
    public void SetUp()
    {
        GearInfoData gearInfoData = GearAsset.instance.GetItemDataByIndex(this.GearCode);
        GearTypeData gearTypeData = BlacksmithManager.instance.GearTypeDataDic.Get(gearInfoData.Slot.ToString());
        UpgradeLevelData upgradeLevelData = BlacksmithManager.instance.ListUpgradeLevelData[(int)gearTypeData.IndexUpgrade].Data[this.UpgradeLv];
        
        List<BaseDataStat> statsWithoutSpi = new();
        for (int i = 0; i < BaseStats.Length; i++)
        {
            if(!BaseStats[i].StatKey.Equals("SPI")){
                statsWithoutSpi.Add(BaseStats[i]);
            }
        }
        this.BaseStats = statsWithoutSpi.ToArray();

        statsWithoutSpi = new();
        for (int i = 0; i < ModStats.Length; i++)
        {
            if(!ModStats[i].StatKey.Equals("SPI")){
                statsWithoutSpi.Add(ModStats[i]);
            }
        }
        this.ModStats = statsWithoutSpi.ToArray();
        
        for (int i = 0; i < BaseStats.Length; i++)
        {
            switch (BaseStats[i].StatKey)
            {
                case "STR":
                    STR = (int)(BaseStats[i].StatValue * (1 + upgradeLevelData.Stats_Increase));
                    break;
                case "MND":
                    MND = (int)(BaseStats[i].StatValue * (1 + upgradeLevelData.Stats_Increase));
                    break;
                case "CRI":
                    CRI = BaseStats[i].StatValue * (1 + upgradeLevelData.Stats_Increase);
                    break;
                case "CRD":
                    CRD = BaseStats[i].StatValue * (1 + upgradeLevelData.Stats_Increase);
                    break;
                case "HIT":
                    HIT = (float)BaseStats[i].StatValue * (1 + upgradeLevelData.Stats_Increase);
                    break;
                case "EVA":
                    EVA = BaseStats[i].StatValue * (1 + upgradeLevelData.Stats_Increase);
                    break;
                case "HP":
                    HP = (int)(BaseStats[i].StatValue * (1 + upgradeLevelData.Stats_Increase));
                    break;
                case "MP":
                    MP = (int)(BaseStats[i].StatValue * (1 + upgradeLevelData.Stats_Increase));
                    break;
                case "VIT":
                    VIT = (int)(BaseStats[i].StatValue * (1 + upgradeLevelData.Stats_Increase));
                    break;
                case "SPI":
                    SPI = (int)(BaseStats[i].StatValue * (1 + upgradeLevelData.Stats_Increase));
                    SPI = 0;
                    break;
                case "DEX":
                    DEX = (int)(BaseStats[i].StatValue * (1 + upgradeLevelData.Stats_Increase));
                    break;
                case "SPD":
                    SPD = (int)(BaseStats[i].StatValue * (1 + upgradeLevelData.Stats_Increase));
                    break;
                case "DMG":
                    DMG = (int)(BaseStats[i].StatValue * (1 + upgradeLevelData.Stats_Increase));
                    break;
            }
        }
        for (int i = 0; i < ModStats.Length; i++)
        {
            switch (ModStats[i].StatKey)
            {
                case "STR":
                    STR += (int)ModStats[i].StatValue;
                    break;
                case "MND":
                    MND += (int)ModStats[i].StatValue;
                    break;
                case "CRI":
                    CRI += ModStats[i].StatValue;
                    break;
                case "CRD":
                    CRD += ModStats[i].StatValue;
                    break;
                case "HIT":
                    HIT += (int)ModStats[i].StatValue;
                    break;
                case "EVA":
                    EVA += ModStats[i].StatValue;
                    break;
                case "HP":
                    HP += (int)ModStats[i].StatValue;
                    break;
                case "MP":
                    MP += (int)ModStats[i].StatValue;
                    break;
                case "VIT":
                    VIT += (int)ModStats[i].StatValue;
                    break;
                case "SPI":
                    SPI += (int)ModStats[i].StatValue;
                    SPI = 0;
                    break;
                case "DEX":
                    DEX += (int)ModStats[i].StatValue;
                    break;
                case "SPD":
                    SPD += (int)ModStats[i].StatValue;
                    break;
                case "DMG":
                    DMG += (int)ModStats[i].StatValue;
                    break;
            }
        }
        for (int i = 0; i < OptionStats.Length; i++)
        {
            switch (OptionStats[i].StatKey)
            {
                case "HP_Per":
                    HP_Per += OptionStats[i].StatValue;
                    break;
                case "MP_Per":
                    MP_Per += OptionStats[i].StatValue;
                    break;
                case "CRI":
                    CRI += OptionStats[i].StatValue;
                    break;
                case "CRD":
                    CRD += OptionStats[i].StatValue;
                    break;
            }
        }
    }

}
[Serializable]
public class CompanionData
{
    public string _id;
    public string UserId;
    public string CompName;
    public string CompCode;
    public int CompType;
    public int Rarity;
    public float STR;
    public float Attack;
    public float MND;
    public float SPD;
    public float HIT;
    public float CRI;
    public float CRD;
    public string Skill_1;
    public string Skill_2;
    public bool IsInAdventure;
    public double LastTimeAdventure;
    public bool Equiped;
    public int Level;
    public float Skill_1_Rate;
    public float Skill_2_Rate;

}
[Serializable]
public class ListCompanionData
{
    public List<CompanionData> Companions = new List<CompanionData>();

}
[Serializable]
public class OrbData
{
    public string _id;
    public string Index;
    public string Name;
    public string Des;
    public int CostType;
    public int Cost;
    public int SkillType;
    public int Target;
    public int StatSkill;
    public float Multiplier;
    public int DameType;
    public int SkillOrb;
    public string OrbCode;
    public bool Equiped;
    public int EquipedSlot;
}
[Serializable]
public class SkillData
{
    public string OrbCode;
    public bool Equiped;
    public int EquipedSlot;
}
[Serializable]
public class ListSkillData
{
    public List<OrbData> SkillData = new List<OrbData>();
}
[Serializable]
public class ListOrbsData
{
    public List<OrbData> OrbsData = new List<OrbData>();
}
[Serializable]
public class UserRegister
{
    public string email;
    public string userName;
    public string passWord;
    public string displayName;
}
[Serializable]
public class UserClass
{
    public string userID;
    public string charID;
}
[Serializable]
public class CharacterData
{
    public string _id;
    public int CharType;
    public string CharacterID;
    public int Level;
    public float Exp;
    public string UserId;
    public int BaseDame;
    public int HP;
    public int MP;
    public int Str;

    public int Vit;
    public int Dex;
    public int Mind;
    public int Spirit;
    public int Speed;
    public float HitRate;
    public float Evade;
    public float CritRate;
    public float CritDame;
    public float Vis;
    public float MaxEXP;
    public int CurrentHP;
    public int CurrentMP;
}
[Serializable]
public class GearDatas
{
    public ListGearData Data;

}
[Serializable]
public class ListGearData
{
    public List<GearData> GearData = new List<GearData>();

    public void RemoveById(string gearID){
        int index = -1;
        for (int i = 0; i < GearData.Count; i++)
        {
            if(GearData[i]._id.Equals(gearID)){
                index = i;
                break;
            }
        }
        GearData.RemoveAt(index);
    }

    public void SortGear()
    {
        GearData.Sort((g1, g2) =>
        {
            if (g1 == null || g2 == null) return 1;
            if (g1.UpgradeLv > g2.UpgradeLv) return -1;
            if (g1.UpgradeLv == g2.UpgradeLv)
            {
                if (g1.Level > g2.Level) return -1;
                if (g1.Level == g2.Level) return g1._id.CompareTo(g2._id);
            }
            return 1;
        });
    }
}

[Serializable]
public class GearIDName
{
    public string ID;
    public string Name;
}
[Serializable]
public class ListGearIDName
{
    public List<GearIDName> GearDataName = new List<GearIDName>();
}
[Serializable]
public class ItemGearDrop
{
    public GearItemDrop Data;

    [Serializable]
    public class GearItemDrop
    {
        public GearData GearDrop;
        public RewardItem Reward;
        public bool LevelUp;
        public GOA.UIMenu.ClassesData LevelUpStats;
    }
    [Serializable]
    public class RewardItem
    {
        public int Coin;
        public int EXP;
        public int Gin;
        public string Potion;
        public int PoorLeather;
        public int ChieftainBlood;
        public int HardLeather;
        public int AbyssRoots;
        public int DragonFlower;
        public int TritinumAlloy;
        public int Ferrieliquid;
        public int MetalPlate;
        public int StonePile;
        public int SilicatePowder;

        public string Orb;
        public string Companion;
    }

}

[Serializable]
public class QuestData
{
    public string Index;
    public int Group;
    public int Type;
    public string Description;
    public int Number;
    public int Target_Index;
    public int GoldReward;
    public string ItemReward1;
    public int AmountReward1;
    public string ItemReward2;
    public int AmountReward2;
    public int SkillOrb;
}
[Serializable]
public class ListQuestData
{
    public List<QuestData> QuestData = new List<QuestData>();
}
