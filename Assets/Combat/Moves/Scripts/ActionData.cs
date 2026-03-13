using System.Collections.Generic;
using UnityEngine;

namespace Rubik.Combat
{
    public delegate int DamageFormula(CharacterCombatState attacker, CharacterCombatState target);

    public class ActionData
    {
        public CharacterCombatState attacker;
        public string actionId;
        public List<CharacterCombatState> targets;

        public ActionData(CharacterCombatState attacker, string moveId, List<CharacterCombatState> targets)
        {
            this.attacker = attacker;
            this.actionId = moveId;
            this.targets = targets;
        }
    }
    public class ChooseActionData
    {
        public BaseCharacter attacker;
        public string actionId;
        public BaseCharacter targetChosen;
    }

    public class ActionRequest
    {
        public string actionId;
        public BaseCharacter target;
        public BaseCharacter attacker;
    }

    [System.Serializable]
    public class ActionResponse
    {
        public string actionId;
        public bool isLongRange = false;
        public BaseCharacter attacker;
        public BaseCharacter mainTarget;
        public List<BaseCharacter> targets = new List<BaseCharacter>();
        public List<BaseEffectSO> effects = new List<BaseEffectSO>();
        public DamageFormula damageFormula;
        public SkillStat cost;
        /// <summary>
        /// <para> 0 = Ready to attack </para>
        /// <para> 1 = Run </para>
        /// <para> 2 = Attack </para>
        /// <para> 3 = End attack action or fallback </para>
        /// </summary>
        public AttackAnimationInfo[] attackAnimationInfo = new AttackAnimationInfo[4];
    }
}