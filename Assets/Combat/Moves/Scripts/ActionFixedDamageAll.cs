using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.Combat
{
    [CreateAssetMenu(fileName = "ActionFixedDamageAll", menuName = "Scriptable Object/Moves/Action ignores stats and all target")]
    public class ActionFixedDamageAll : BaseActionSO
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
            foreach (CharacterCombatState characterState in CombatManager.Instance.GetAliveCharacters())
            {
                BaseCharacter character = CharacterManager.Instance.GetCharacterObject(characterState.data.characterInstanceId);
                if (character.GetCharacterSideInfo() != 0 || !excludeSelf)
                {
                    result.Add(character);
                }
            }
            return result;
        }
    }
}
