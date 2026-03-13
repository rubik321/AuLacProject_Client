using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.CardPlayer
{
    using ItemPlayer;
    using Rubik.BattleEngine;
    using Rubik.DataType;
    using Rubik.UI.Statitic;
    using Rubik.Myrk.Monster;
    using Rubik.Myrk.BattleTeam;
    using System;

    public enum CardRarity
    {
        Epic = 0,
        Legendary = 1,
        Mythic = 2,
    }

    [System.Serializable]
    public class Gear
    {
        public int Lv = 1;
    }

    [System.Serializable]
    public class CardPlayer
    {
        public string _id;
        public string UserID;
        public CardPlayerIndex Index;
        public CardPlayerType Type;
        public int Lv = 0;
        public int Star = 0;

        // Cache
        [NonSerialized] public List<BattleTeamData> Teams;
        public bool IsEquiped {
            get {
                return Teams.Count > 0;
            }
        }
        public CardPlayerData CardPlayerData;
        public CardSkillLv CardSkillLv;
        public BattleStats BaseStats;
        public BattleStats SkillStats;
        public BattleStats TotalStats;

        public void UpdateData(CardPlayer cardPlayer)
        {
            this.Lv = cardPlayer.Lv;
            this.Star = cardPlayer.Star;
            this.Index = cardPlayer.Index;
        }

        public CardShortTeam ToShortTeam()
        {
            CardShortTeam cardShortTeam = new CardShortTeam();
            cardShortTeam._id = this._id;
            cardShortTeam.Index = this.Index;
            cardShortTeam.Level = this.Lv;
            cardShortTeam.Star = this.Star;
            return cardShortTeam;
        }
    }

    [System.Serializable]
    public class CardPlayerData
    {
        public CardPlayerIndex Index;
        public CardPlayerType Type;
        public RarityType Tier;
        public string Name;
        public CardRarity Rarity;
        public OriginType Origin;

        // Resource
        public string ResourceName;
        public string SoundNameBaseAttack;
        public float SoundDelayBaseAttack;
        public string SoundNameSkillAttack;
        public float SoundDelaySkillAttack;

        // Up Star, Evolve
        public ItemType ShardCardType;

        // Evolve
        public bool CanEvolve;
        public CardPlayerIndex EvolveTarget;

        // Stats
        public int HP;
        public int ATK;
        public int DEF;
        public int SPD;
        public TypePassive[] Passive;
        public TypeActive Skill;
    }

    [System.Serializable]
    public enum CardPlayerIndex
    {
        Card_0 = 0,
        Card_1 = 1,
        Card_2 = 2,
        Card_3 = 3,
        Card_4 = 4,
        Card_5 = 5,
        Card_6 = 6,
        Card_7 = 7,
        Card_8 = 8,
        Card_9 = 9,
        Card_10 = 10,
        Card_11 = 11,
        Card_12 = 12,
        Card_13 = 13,
        Card_14 = 14,
        Card_15 = 15,
        Card_16 = 16,
        Card_17 = 17,
        Card_18 = 18,
        Card_19 = 19,
        Card_20 = 20,
        Card_21 = 21,
        Card_22 = 22,
        Card_23 = 23,
        Card_24 = 24,
        Card_25 = 25,
        Card_26 = 26,
        Card_27 = 27,
        Card_28 = 28,
        Card_29 = 29,

        // Boss
        ClanBoss_0 = 1000,
        ClanBoss_1 = 1001,
        ClanBoss_2 = 1002,
        ClanBoss_3 = 1003,
        ClanBoss_4 = 1004,

        Portal_Boss_0 = 1100,
        Portal_Boss_1 = 1101,
        Portal_Boss_2 = 1102,
        Portal_Boss_3 = 1103,
        Portal_Boss_4 = 1104,


        Crep_0 = 2000,
        Crep_1 = 2001,
        Crep_2 = 2002,
        Crep_3 = 2003,
        Crep_4 = 2004,
        Crep_5 = 2005,
        Crep_6 = 2006,
        Crep_7 = 2007,
        Crep_8 = 2008,
        Crep_9 = 2009,
    }

    public enum CardPlayerType
    {
        Type_0 = 0,
        Type_1 = 1,
        Type_2 = 2,
        Type_3 = 3,
        Type_4 = 4,
        Type_5 = 5,
        Type_6 = 6,
        Type_7 = 7,
        Type_8 = 8,
        Type_9 = 9,

        Creep_0 = 1000,
        Creep_1 = 1001,
        Creep_2 = 1002,
        Creep_3 = 1003,
        Creep_4 = 1004,
        Creep_5 = 1005,
        Creep_6 = 1006,
        Creep_7 = 1007,
        Creep_8 = 1008,
        Creep_9 = 1009,

        Boss_0 = 2000,
        Portal_Boss_0 = 2100,
    }


    [System.Serializable]
    public class CardEvolveData
    {
        public int Level;
        public EvolveTierPrice[] Price;
        public CardPlayerIndex[][] PathEvolve;
    }

    [System.Serializable]
    public class EvolveTierPrice
    {
        public RarityType Tier;
        public int Shard;
        public ItemData[] Price;
    }


    [System.Serializable]
    public class CardUpStar
    {
        public int Star;
        public float UpStat;
        public int Shard;
        public ItemData[] Cost;
        public bool Max;
        public RarityType[] TierLimit;
    }

    public enum SummonType
    {
        Normal = 0,
        Premium = 1,
        Ultra = 2,
    }

    [System.Serializable]
    public class CardSummonData
    {
        public SummonType Type;
        public ItemData Ticket;
        public float CardRate;
        public CardPlayerIndex[] ListIndex;
        public ListItemRate[] ShardRate;
        public int Ensuare;
        public int AdvSummon;
    }

    [System.Serializable]
    public class SummonHistory
    {
        public SummonType Type;
        public int Count;
        public int TotalCount;
        public int AdvSummon;
    }

    [System.Serializable]
    public class RewardSummon
    {
        public CardPlayer[] Cards;
        public ItemData[] Shards;
        public CardPlayerIndex[] CardConvert;
    }

    [System.Serializable]
    public class CardLevelData
    {
        public int Level;
        public float UpStat;
        public ItemData[] Price;
        public bool Max;
    }

    [System.Serializable]
    public class CardTierOriginData
    {
        public OriginType Origin;
        public CardTierData[] Card;
    }

    [System.Serializable]
    public class CardTierData
    {
        public RarityType Tier;
        public CardPlayerIndex[] CardIndexes;
    }
}
