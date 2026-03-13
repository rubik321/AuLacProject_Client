using Rubik.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.Combat
{
    [CreateAssetMenu(fileName = "EffectDamagePercentage", menuName = "Scriptable Object/Effects/Effect reduce target hp or mp by percentage")]
    public class EffectDamagePercentage : BaseEffectSO
    {
        public float percentage;
        public bool affectHP;
        public bool affectMP;
        [Tooltip("False if damage based on percentage of what is left")]
        public bool percentageOnMax;
        private float Rate => percentage / 100;

        public override Action<CharacterCombatState> ActivateOnStartTurn(CharacterCombatState attacker)
        {
            return target =>
            {
                if (percentageOnMax)
                {
                    if (affectHP)
                    {
                        target.hp -= (int)(Rate * target.maxHp);
                    }
                    if (affectMP)
                    {
                        target.mp -= (int)(Rate * target.maxMp);
                    }
                }
                else
                {
                    if (affectHP)
                    {
                        target.hp -= (int)(Rate * target.hp);
                    }
                    if (affectMP)
                    {
                        target.mp -= (int)(Rate * target.mp);
                    }
                }
            };
        }
    }
}
