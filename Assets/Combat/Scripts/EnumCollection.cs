using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.Combat
{
    public enum TurnState
    {
        StartTurn,
        StartTurnEffect,
        ChooseAction,
        Action,
        EndTurnEffect,
        EndTurn,
        Wait
    }

    public enum ActionType
    {
        Attack,
        Skill,
        Item,
        Defense,
        Spell,
        Flee
    }

    public enum StatType
    {
        None = 0,
        Strength = 1,
        Mind = 2,
        Dexterity = 3,
        HP = 4,
        MP = 5,
        Vitality = 6,
        Spirit = 7,
        Speed = 8,
        CritRate = 9,
        CritDamage = 10,
        Evasion = 11,
        Hit = 12
    }

    public enum ActionTargetType
    {
        Self,
        EnemyAlive,
        AllyAlive,
        EnemyDead,
        AllyDead,
        AnyAlive,
        AnyDead,
        Owner
    }


    public enum GearSlot
    {
        Helmet = 0,
        Body = 1,
        Leg = 2,
        MainHand = 3,
        OffHand = 4,
        Accessory = 5
    }

    public enum WeaponType
    {
        None = 0,
        Sword = 1,
        Shield = 2,
        Staff = 3,
        Floating = 4,
        Knife = 5,
        Bow = 6,
        Gun = 7,
        Mace = 8 
    }

    public class WeaponTypeParse{
        public static WeaponType FromString(string name){
            try
            {
                //name = name.ToLower();
                return (WeaponType)Enum.Parse(typeof(WeaponType), name);
            }
            catch (System.Exception)
            {
                return WeaponType.None;
            }
        }
    }

    public enum ChooseActionState
    {
        Attack = 0,
        ChooseSkill = 1,
        Defend = 2,
        ChooseItem = 3,
        Flee = 4
    }
}
