using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.Combat
{
    [CreateAssetMenu(fileName = "EffectDamageEffectChangeStatPercentage", menuName = "Scriptable Object/Effects/Effect change stat by percentage")]
    public class EffectChangeStat : BaseEffectSO
    {
        public float percentage;
        public StatType statAffected;
        private Action<CharacterCombatState> onDeactivate;
        private float Rate => percentage / 100;

        public override Action<CharacterCombatState> ActivateOnTaken(CharacterCombatState attacker)
        {
            return target =>
            {
                int statChange;
                switch (statAffected)
                {
                    case StatType.Strength:
                        statChange = (int)(Rate * target.baseStrength);
                        target.strength += statChange;
                        onDeactivate = target => { target.strength -= statChange; };
                        break;
                    case StatType.Vitality:
                        statChange = (int)(Rate * target.baseVitality);
                        target.vitality += statChange;
                        onDeactivate = target => { target.vitality -= statChange; };
                        break;
                    case StatType.Speed:
                        statChange = (int)(Rate * target.baseSpeed);
                        target.speed += statChange;
                        onDeactivate = target => { target.speed -= statChange; };
                        break;
                    case StatType.Mind:
                        statChange = (int)(Rate * target.baseMind);
                        target.mind += statChange;
                        onDeactivate = target => { target.mind -= statChange; };
                        break;
                    case StatType.Dexterity:
                        statChange = (int)(Rate * target.baseDexterity);
                        target.dexterity += statChange;
                        onDeactivate = target => { target.dexterity -= statChange; };
                        break;
                    case StatType.Spirit:
                        statChange = (int)(Rate * target.baseSpirit);
                        target.spirit += statChange;
                        onDeactivate = target => { target.spirit -= statChange; };
                        break;
                    case StatType.Evasion:
                        statChange = (int)(Rate * target.baseEvasion);
                        target.evasion += statChange;
                        onDeactivate = target => { target.evasion -= statChange; };
                        break;
                    case StatType.CritRate:
                        target.critRate += Rate;
                        onDeactivate = target => { target.critRate -= Rate; };
                        break;
                    case StatType.CritDamage:
                        target.critDamage += Rate;
                        onDeactivate = target => { target.critDamage -= Rate; };
                        break;
                    case StatType.HP:
                        statChange = (int)(Rate * target.maxHp);
                        target.maxHp += statChange;
                        onDeactivate = target => 
                        {
                            target.maxHp -= statChange;
                            target.hp = Mathf.Min(target.maxHp, target.hp);
                        };
                        break;
                    case StatType.MP:
                        statChange = (int)(Rate * target.maxMp);
                        target.maxMp += statChange;
                        onDeactivate = target =>
                        {
                            target.maxMp -= statChange;
                            target.mp = Mathf.Min(target.maxMp, target.mp);
                        };
                        break;
                    case StatType.Hit:
                        target.hit += Rate;
                        onDeactivate = target => { target.hit -= Rate; };
                        break;
                }
            };
        }

        public override Action<CharacterCombatState> Deactivate(CharacterCombatState attacker)
        {
            return target =>
            {
                onDeactivate?.Invoke(target);
            };
        }
    }
}
