using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.Combat
{
    [CreateAssetMenu(fileName = "ActionAllTarget", menuName = "Scriptable Object/Moves/Action targets all character alives")]
    public class ActionAllTarget : BaseActionSkill
    {
        [SerializeField] bool excludeSelf = true;

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
