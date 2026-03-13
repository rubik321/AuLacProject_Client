using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.Combat
{
    public class BaseActionPassive : BaseActionSO
    {
        public override int GetDamage(CharacterCombatState attacker, CharacterCombatState target)
        {
            return 0;
        }
    }
}
