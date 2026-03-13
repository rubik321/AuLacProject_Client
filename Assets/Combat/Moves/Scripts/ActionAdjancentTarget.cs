using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.Combat
{
    [CreateAssetMenu(fileName = "ActionAdjancentTarget", menuName = "Scriptable Object/Moves/Action targets adjancent character of chosen one")]
    public class ActionAdjancentTarget : BaseActionSkill
    {
        [SerializeField] bool excludeSelf = false;
        [SerializeField] bool excludeChosenTarget = false;

        public override List<BaseCharacter> GetTargets(BaseCharacter targetChosen)
        {
            List<BaseCharacter> result = new List<BaseCharacter>();
            List<CharacterCombatState> adjancentCharacter = CombatManager.Instance.GetAliveAdjancentCharacters(targetChosen.CharacterState.data.characterInstanceId);
            foreach (CharacterCombatState characterState in adjancentCharacter)
            {
                BaseCharacter character = CharacterManager.Instance.GetCharacterObject(characterState.data.characterInstanceId);
                if (character.GetCharacterSideInfo() != 0 || !excludeSelf)
                {
                    result.Add(character);
                }
            }
            if (!excludeChosenTarget)
            {
                result.Add(targetChosen);
            }
            return result;
        }
    }
}
