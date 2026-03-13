using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.Combat
{
    [CreateAssetMenu(fileName = "ActionFixedDamageSingle", menuName = "Scriptable Object/Moves/Action ignores stats and single target")]
    public class ActionFixedDamageSingle : BaseActionSO
    {
        [SerializeField] int baseDamage = 3;

        public override int GetDamage(CharacterCombatState attacker, CharacterCombatState target)
        {
            return baseDamage;
        }
    }
}