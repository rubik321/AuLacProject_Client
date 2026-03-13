using System.Collections;
using System.Collections.Generic;
using Rubik.CardPlayer;
using UnityEngine;

namespace Rubik.BattleEngine
{
    [System.Serializable]
    public class BattleShortData
    {
        public TeamBattleShortData TeamA;
        public TeamBattleShortData TeamB;

        public ListTurnState StartBattleStates;
        public RoundData[] RoundDatas;

        public StatisData StatisData;

        public string TeamWin;
    }

    [System.Serializable]
    public class RoundData
    {
        public int round;
        public TurnData[] TurnDatas;
        public ListTurnState EndTurnStates;
    }

    [System.Serializable]
    public class TurnData
    {
        public TurnCardData TurnCardData;
        public TurnHeroData TurnHeroData;
    }

    [System.Serializable]
    public class TurnCardData
    {
        public int ActionSlot;              // vị trí hành động
        public string TeamID;
        public ActionType ActionType;
        public ListTurnState TurnStates;    // vị trí cập nhật
    }

    [System.Serializable]
    public class TurnHeroData
    {
        public string TeamID;
        public ListTurnState TurnStates;    // vị trí cập nhật
        public int AP;

    }

    [System.Serializable]
    public class ListTurnState
    {
        public TurnCardState[] TurnCardStates;
        public TurnHeroState[] TurnHeroStates;
    }

    [System.Serializable]
    public class TurnCardState
    {
        public int Slot;
        public string TeamID;
        public int[] Dmg;
        public TypeDmg[] TypeDmg;
        public int MaxHP;
        public int HP;
        public int AP;
        public BuffDataShort[] Buff;
    }

    [System.Serializable]
    public class BuffDataShort
    {
        public TypeBuff Type;
        public TypePassive Passive;
        public int Amount;
    }

    [System.Serializable]
    public class TurnHeroState
    {
        public string TeamID;
        public int AP;
    }

    public enum ActionType
    {
        Flag_Action = -1,
        BaseAttack = 0,
        SkillAttack = 1,
        Passive = 2,
    }

    public enum TypeDmg
    {
        Normal = 0,
        Crit = 1,
        Block = 2,
        Bleed = 4,
        Heal = 5,
        Burn = 6,
        Poison = 7,
    }

    public enum TypeBuff
    {
        Flag_Buff = -1, // Using for count trigger (first time- in first amunt round);
        Debuff_Passive,
        Buff_Passive,
        Stun,
        Silence,
        Bleed,
        Freeze,
        Petrify,
        Burn,
        DragonSlash,
        Buff_Dmg_Bleed,
        Buff_Dmg_Frozen,
        Shield,
        Buff_ATK,
        Debuff_ATK,
        Buff_DEF,
        Debuff_DEF,
        Buff_SPD,
        Debuff_SPD,
        Buff_CRT,
        Debuff_CRT,
        Buff_CRD,
        Debuff_CRD,
        Buff_DR,
        Debuff_DR,
        Buff_Active,
        Debuff_Active,
        Poison,
    }

    [System.Serializable]
    public class StatisData
    {
        public string TeamA;
        public string TeamB;
        public ElementStatisData[] ElementStatis;
        public string TeamWin;
    }

    [System.Serializable]
    public class ElementStatisData
    {
        public string _id = "";
        public long Dmg = 0;
        public long Heal = 0;
        public long Hit = 0;
        public long LastHP = 0;
    }
}