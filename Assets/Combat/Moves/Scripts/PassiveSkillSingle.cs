using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.Combat
{
    [CreateAssetMenu(fileName = "Passive", menuName = "Scriptable Object/Passive/Passive skill activate for self")]
    public class PassiveSkillSingle : BaseActionPassive
    {
        public override List<BaseCharacter> GetTargets(BaseCharacter targetChosen)
        {
            return new List<BaseCharacter>() { targetChosen };
        }
    }
}
