using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.Combat
{
    [CreateAssetMenu(fileName = "ActionFixedDamageAllSameSide", menuName = "Scriptable Object/Moves/Action ignores stats and all target of 1 side")]
    public class ActionFixedDamageAllSameSide : BaseActionSO
    {
        [SerializeField] int baseDamage = 3;
        [SerializeField] bool excludeSelf = true;

        public override int GetDamage(CharacterCombatState attacker, CharacterCombatState target)
        {
            return baseDamage;
        }

        public override List<BaseCharacter> GetTargets(BaseCharacter targetChosen)
        {
            List<BaseCharacter> result = new List<BaseCharacter>();
            if (targetChosen.GetCharacterSideInfo() == 1)
            {
                foreach (CharacterCombatState characterState in CombatManager.Instance.GetAliveEnemies())
                {
                    result.Add(CharacterManager.Instance.GetCharacterObject(characterState.data.characterInstanceId));
                }
            }
            else
            {
                foreach (CharacterCombatState characterState in CombatManager.Instance.GetAliveAllies())
                {
                    BaseCharacter character = CharacterManager.Instance.GetCharacterObject(characterState.data.characterInstanceId);
                    if (character.GetCharacterSideInfo() != 0 || !excludeSelf)
                    {
                        result.Add(character);
                    }
                }
            }
            return result;
        }
    }
}
