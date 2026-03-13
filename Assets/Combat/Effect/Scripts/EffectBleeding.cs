using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rubik.UI;

namespace Rubik.Combat
{
    [CreateAssetMenu(fileName = "Bleeding", menuName = "Scriptable Object/Effects/Bleeding")]
    public class EffectBleeding : BaseEffectSO
    {
        public float percentage;

        public override Action<CharacterCombatState> ActivateOnStartTurn(CharacterCombatState attacker)
        {
            return target =>
            {
                target.hp -= (int)(percentage / 100 * attacker.strength);
            };
        }
    }
}
