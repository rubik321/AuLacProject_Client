using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Spine.Unity;

namespace Rubik.Battle
{
    [Serializable]
    public enum AttackType
    {
        Melee,
        Ranged,
        Arrow
    }
    [Serializable]
    public class BattleData
    {
        public TurnBallte[] ListTurns;
        public string WinnerID;
        public Team TeamA;
        public Team TeamB;
    }
    [Serializable]
    public class TurnBallte
    {
        public int round;
        public Team CurrentTeamA;
        public Team CurrentTeamB;
        public bool[] TargetEnemyCrtDmg;
        public string TeamID;
        public int ActionSlot;
        public int ActionType;
        public int[] TargetEnemySlot;
        public float[] TargetEnemyDame;

    }
    public enum CharacterRarity
    {

    }
    public enum CharacterType
    {
        Hero,
        Enemy,
        Boss,
        Box
    }
    public enum SkillAction
    {
        NONE,
        SKILL_MOVE,
        SKILL_STAND
    }
    public enum AttackActionType
    {
        ATTACK_WITH_EVENT,
        ATTACK_WITH_TIME_DELAY
    }
    public enum MoveActionType
    {
        MOVE_WITH_EVENT,
        MOVE_WITH_TIME_DELAY
    }
    public enum TargetType
    {
        SINGLE_TARGET,
        ALL_TARGETS,
        FRONT_ROW_TARGETS,
        BACK_ROW_TARGETS,
        RANDOM_THREE_TARGETS
    }
    public enum EffectType { 
        ATTACK_EFFECT = 0,
        SKILL_EFFECT = 1,
        HIT_DAME_EFFECT =2
    }

    [Serializable]
    public class EffectData
    {
        public EffectType effectID;
        public SkeletonDataAsset sketetonEffect;
        public GameObject particalEffect;
        public GameObject hitDameEffect;
        public float delayEffect = 0;
        public float startEffect = 0;
        public Vector2 Scale = Vector2.one;
        public bool isStartPos = false;
        public bool isSpwanOnTarget = false;
    }
    [Serializable]
    public class AttacAction
    {
        public AttackActionType type;
        public TargetType TargetType;
        public float hitdame_delay_time;
        public float completed_delay_time;
        public float effectDelay;
        public int attackCount;
       // public bool isSelfSpwan = false;
       
    }
    [Serializable]
    public class MoveAction
    {
        public MoveActionType type;
        public float delay_time;
    }

    [Serializable]
    public class BaseCharacterData
    {
        public CharacterType CharacterType;
        [Header("Character Type")]
        public AttackType CardType;
        public bool isMoveToEnemyPosition = false;
        public Origin Origin;
        [Header("Melee hero  Move or Stand to cast skill")]
        public SkillAction SkillType;
        public bool isMultiTarget = false;
        [Header("Melee hero move to target")]
        public MoveAction MoveAction;
        public AttacAction AttackAction;
        public AttacAction SkillAction;
        public int Index;
        public int skinIndex = -1;
        public int Star;
        public int Slot;
        public int Class;
        public string Name;
        public int ATK;
        public float Def;
        public float CritRate;
        public float MaxHP;
        public float CurHP;
        public float AP;
        public Vector2 ScaleData;

    }
    [Serializable]
    public class HeroData
    {
        public BaseCharacterData characterData;
        public int level;
    }
    [Serializable]
    public enum Origin
    {
        Wang = 0,
        Wei = 1,
        Shu = 2,
        Wu = 3,
        Qan = 4
    }
    [Serializable]
    public class Team
    {
        public string sessionID;
        public BaseCharacterData[] lsTeam;
    }   
}         