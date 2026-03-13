using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.Combat
{
    public abstract class BaseActionSO : ScriptableObject
    {
        protected int MAX_LEVEL = 25;

        public string actionId;
        public string actionName;
        public bool isPassive;
        public ActionTargetType actionTargetType = ActionTargetType.EnemyAlive;
        public SkillStat cost;
        public bool isLongRange = false;
        public List<BaseEffectSO> effects = new List<BaseEffectSO>();
        [Tooltip("Ready/Run/Attack/Fallback animation. Leave blank name if absent. DO NOT change array length")]
        public AttackAnimationInfo[] attackAnimationInfo = new AttackAnimationInfo[4];

        public virtual List<BaseCharacter> GetTargets(BaseCharacter targetChosen)
        {
            return new List<BaseCharacter> { targetChosen };
        }
        public abstract int GetDamage(CharacterCombatState attacker, CharacterCombatState target);
        //protected int GetPhysicalStat(CharacterCombatState attacker)
        //{
        //    switch (physicalStat.statType)
        //    {
        //        case StatType.Mind:
        //            return attacker.mind;
        //        case StatType.Strength:
        //            return attacker.strength;
        //        case StatType.HP:
        //            return attacker.hp;
        //        case StatType.Dexterity:
        //            return attacker.dexterity;
        //        case StatType.MP:
        //            return attacker.mp;
        //        case StatType.Vitality:
        //            return attacker.vitality;
        //        case StatType.Spirit:
        //            return attacker.spirit;
        //        default:
        //            return (int)physicalStat.rate;
        //    }
        //}

        //protected int GetMagicStat(CharacterCombatState attacker)
        //{
        //    switch (magicalStat.statType)
        //    {
        //        case StatType.Mind:
        //            return attacker.mind;
        //        case StatType.Strength:
        //            return attacker.strength;
        //        case StatType.HP:
        //            return attacker.hp;
        //        case StatType.Dexterity:
        //            return attacker.dexterity;
        //        case StatType.MP:
        //            return attacker.mp;
        //        case StatType.Vitality:
        //            return attacker.vitality;
        //        case StatType.Spirit:
        //            return attacker.spirit;
        //        default:
        //            return 1;
        //    }
        //}

    }

    [System.Serializable]
    public class AttackAnimationInfo
    {
        public string animationName;
        [Tooltip("VFXs are spawned when animation starts playing, delay manually when create particle system. \n" +
            "VFXs are spawned on player or targets exact position, offset manually when create particle system")]
        public VFXInfo[] vfxInfo = new VFXInfo[0];
        public bool flipDirection = false;
    }

    [System.Serializable]
    public class VFXInfo
    {
        public GameObject prefab;
        [Header("Where to spawn vfx")]
        [Tooltip("True if spawn vfx at attacker position, false if spawn vfx at target(s) position")]
        public bool spawnOnSelf;
        [Header("When to spawn vfx")]
        [Tooltip("When should vfx be spawned")]
        public SpawnTimingType spawnTiming;
        [Tooltip("True if spawn only one very big fx on all target")]
        public bool spawnOnAllEnemy;
    }

    [System.Serializable]
    public struct SkillStat
    {
        public float rate;
        public float startValue;
        [Tooltip("Should rate be multiplies or fixed")]
        public bool isFixed;
        public StatType statType;
    }

    public enum SpawnTimingType
    {
        OnStartAnimation = 0,
        OnEndAnimation = 1,
        Manual = 2
    }
}
