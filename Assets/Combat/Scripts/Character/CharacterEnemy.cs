using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.Combat
{
    public class CharacterEnemy : BaseCharacter
    {
        public override void SetupData(CharacterCombatState characterState)
        {
            this.CharacterState = characterState;
        }
    }
}
