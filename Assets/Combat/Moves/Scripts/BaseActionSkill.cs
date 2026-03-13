using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.Combat
{
    public class BaseActionSkill : BaseActionSO
    {
        [Tooltip("Should output damage be splited across all targets")]
        public bool shareDamage;
        public SkillStat skillMultiplier;
        public SkillDamageType damageType;

        public override int GetDamage (CharacterCombatState attacker, CharacterCombatState target)
        {
            Debug.Log(" skill orb");
            float skillPower = GetStat(attacker) * skillMultiplier.rate;
            if (damageType == SkillDamageType.Healing)
            {
                // Making damage negative stands for healing
                return (int)(-skillPower); //(physicalStat.rate * GetPhysicalStat(attacker) + magicalStat.rate * GetMagicStat(attacker)));
            }

            float targetBaseDefense = damageType == SkillDamageType.Physical ? target.vitality : target.spirit;
            float targetDefense = targetBaseDefense/2;
            //float targetDefense = 0;

            return (int)(Mathf.Max(skillPower - targetDefense, 1) + GetRandomize(attacker, skillMultiplier.statType));

            //float physicalDmg = baseDamage * physicalStat.rate * GetPhysicalStat(attacker) * GetPhysicalStat(attacker) / (GetPhysicalStat(attacker) + target.vitality);
            //float magicDmg = baseDamage * magicalStat.rate * GetMagicStat(attacker) * GetMagicStat(attacker) / (GetMagicStat(attacker) + target.spirit);

            //if (shareDamage)
            //{
            //    return (int)Mathf.Max((physicalDmg + magicDmg)
            //                             / GetTargets(CharacterManager.Instance.GetCharacterObject(target.data.characterInstanceId)).Count, 1);
            //}
            //else
            //{
            //    return (int)Mathf.Max(physicalDmg + magicDmg, 1);
            //}
        }

        private float GetRandomize(CharacterCombatState attacker, StatType statType)
        {
            switch (statType)
            {
                case StatType.Strength:
                    return UnityEngine.Random.Range(1, attacker.data.level + attacker.strength / 8f);
                case StatType.Dexterity:
                    return UnityEngine.Random.Range(1, attacker.data.level + attacker.dexterity / 8f);
                case StatType.Mind:
                    return UnityEngine.Random.Range(1, attacker.data.level + attacker.mind / 6f);
                case StatType.HP:
                    return UnityEngine.Random.Range(1, attacker.data.level + attacker.maxHp / 32f);
                default:
                    return UnityEngine.Random.Range(1, attacker.data.level);
            }
        }

        protected int GetStat(CharacterCombatState attacker)
        {
            switch (skillMultiplier.statType)
            {
                case StatType.Mind:
                    return attacker.mind;
                case StatType.Strength:
                    return attacker.strength;
                case StatType.HP:
                    return attacker.hp;
                case StatType.Dexterity:
                    return attacker.dexterity;
                case StatType.MP:
                    return attacker.mp;
                case StatType.Vitality:
                    return attacker.vitality;
                case StatType.Spirit:
                    return attacker.spirit;
                default:
                    return 1;
            }
        }
    }

    public enum SkillDamageType
    {
        Physical,
        Magical,
        Healing
    }
}
