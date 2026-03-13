using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.Combat
{
    [CreateAssetMenu(fileName = "ActionAllSameSideTarget", menuName = "Scriptable Object/Moves/Action targets all of same side")]
    public class ActionAllSameSideTarget : BaseActionSkill
    {
        [SerializeField] bool excludeSelf = true;

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
