using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Rubik.Common.AudioHelper;

namespace Rubik.Combat
{
    [CreateAssetMenu(fileName = "Effect Heal", menuName = "Scriptable Object/Effects/Heal Effect Blueprint")]
    public class EffectHeal : BaseEffectSO
    {
        [ReadOnly] public SkillStat healStat;

        public override Action<CharacterCombatState> ActivateOnTaken(CharacterCombatState attacker)
        {
            Debug.Log("Heal : " + healStat.rate);
            AudioCtrl.Instance.Play(AudioName.Heal_Sound); 
            if (healStat.statType == StatType.HP)
            {
               
                return target =>
                {
                    if (healStat.isFixed)
                    {
                        target.hp = (int)Mathf.Clamp(target.hp + healStat.rate, 0, target.maxHp);
                    }
                    else
                    {
                        target.hp = (int)Mathf.Clamp(target.hp +healStat.startValue+ healStat.rate * target.maxHp, 0, target.maxHp);
                    }
                };
            }
            else if (healStat.statType == StatType.MP)
            {
                return target =>
                {
                    if (healStat.isFixed)
                    {
                        target.mp = (int)Mathf.Clamp(target.mp + healStat.rate, 0, target.maxMp);
                    }
                    else
                    {
                        target.mp = (int)Mathf.Clamp(target.mp + healStat.startValue+ healStat.rate * target.maxMp, 0, target.maxMp);
                    }
                };
            }
            else
            {
                return null;
            }
        }
    }
}
