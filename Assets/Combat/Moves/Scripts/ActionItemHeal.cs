using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Rubik.Combat
{
    [CreateAssetMenu(fileName = "Action Heal", menuName = "Scriptable Object/Moves/Action for healing items")]
    public class ActionItemHeal : BaseActionSO
    {
        [SerializeField] TargetingType targetingType;
        [SerializeField] List<SkillStat> healStats = new List<SkillStat>();
        [SerializeField] EffectHeal healEffectBlueprint;

        delegate List<BaseCharacter> GetTarget(BaseCharacter targetChosen);
        GetTarget targetChooser = null;

        private void Awake()
        {
            targetChooser = null;
            effects.RemoveAll(e => e == null || e.GetType() == typeof(EffectHeal));

            foreach (SkillStat healStat in healStats)
            {
                // EffectHeal is just a blueprint, its rate is wrong, only stattype is right
                // This is to create an instance from the blueprint and assign the right rate to the instance.
                // TODO: Need rework to make this effect works for normal action too.
                EffectHeal clone = Instantiate(healEffectBlueprint);
                clone.healStat = healStat;
                effects.Add(clone);
            }
        }

        public override int GetDamage(CharacterCombatState attacker, CharacterCombatState target)
        {
            return 0;
        }

        public override List<BaseCharacter> GetTargets(BaseCharacter targetChosen)
        {
            if (targetChooser == null)
            {
                switch (targetingType)
                {
                    case TargetingType.SingleTarget:
                        return new List<BaseCharacter>() { targetChosen };
                    case TargetingType.AllOfOneSide:
                        targetChooser = CreateInstance<ActionAllSameSideTarget>().GetTargets;
                        break;
                    case TargetingType.All:
                        targetChooser = CreateInstance<ActionAllTarget>().GetTargets;
                        break;
                    default:
                        return new List<BaseCharacter>();
                }
            }
            return targetChooser(targetChosen);
        }

        public enum TargetingType
        {
            SingleTarget,
            AllOfOneSide,
            All
        }
    }
}
