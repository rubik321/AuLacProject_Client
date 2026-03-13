using System.Collections.Generic;
using UnityEngine;

namespace Rubik.Combat
{
    [CreateAssetMenu(fileName = "Normal Attack", menuName = "Scriptable Object/Moves/Normal Attack")]
    public class ActionNormalAttack : BaseActionSO
    {
        public override int GetDamage(CharacterCombatState attacker, CharacterCombatState target)
        {
            float baseDamage = (attacker.baseDamage + attacker.strength) ;
            //float baseDamage = attacker.baseDamage + attacker.strength * 1f / 2;
            // float targetDefense = (target.vitality / 2f) * (1 + ((attacker.data.level - target.data.level) * (100 / MAX_LEVEL)));
            //float targetDefense = target.vitality/2;
            //Debug.Log(" base dame : " + baseDamage + "   defense : " + targetDefense);
            //float randomize = Random.Range(1, attacker.data.level + attacker.strength / 8f);
            // return (int)(Mathf.Max(baseDamage - targetDefense, 1) + randomize);
            return (int)(baseDamage);
        }

        public override List<BaseCharacter> GetTargets(BaseCharacter targetChosen)
        {
            return new List<BaseCharacter>() { targetChosen };
        }
    }
}
