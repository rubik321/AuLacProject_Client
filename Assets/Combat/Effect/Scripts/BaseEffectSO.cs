using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.Combat
{
    public abstract class BaseEffectSO : ScriptableObject
    {
        public string effectId;
        public string effectName;
        public Sprite icon;
        public int turn;
        public bool stackable;
        public EffectType effectType;

        public virtual Action<CharacterCombatState> ActivateOnStartTurn(CharacterCombatState attacker)
        {
            return null;
        }

        public virtual Action<CharacterCombatState> ActivateOnEndTurn(CharacterCombatState attacker)
        {
            return null;
        }

        public virtual Action<CharacterCombatState> Deactivate(CharacterCombatState attacker)
        {
            return null;
        }

        public virtual Action<CharacterCombatState> ActivateOnTaken(CharacterCombatState attacker)
        {
            return null;
        }
    }

    public enum EffectType
    {
        Neutral = 0,
        Buff = 1,
        Debuff = 2
    }
}
