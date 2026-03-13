using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.Combat
{
    [CreateAssetMenu(fileName = "ActionSingleTarget", menuName = "Scriptable Object/Moves/Action targets single charater")]
    public class ActionSingleTarget : BaseActionSkill
    {

        public override List<BaseCharacter> GetTargets(BaseCharacter targetChosen)
        {
            return new List<BaseCharacter>() { targetChosen };
        }
    }
}
