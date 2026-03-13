using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.Combat
{
    public class EffectData
    {
    }

    public class EffectState
    {
        public string effectId;
        public int turnLeft;
        public Action<CharacterCombatState> onTaken = null;
        public Action<CharacterCombatState> onStartTurn = null;
        public Action<CharacterCombatState> onEndTurn = null;
        public Action<CharacterCombatState> onDisappear = null;

        public EffectState (BaseEffectSO effectSO, CharacterCombatState attacker)
        {
            effectId = effectSO.effectId;
            turnLeft = effectSO.turn;
            onTaken = effectSO.ActivateOnTaken(attacker);
            onStartTurn = effectSO.ActivateOnStartTurn(attacker);
            onEndTurn = effectSO.ActivateOnEndTurn(attacker);
            onDisappear = effectSO.Deactivate(attacker);
        }

        public void ActivateOnTaken(CharacterCombatState target)
        {
            onTaken?.Invoke(target);
        }

        public void ActivateOnStartTurn(CharacterCombatState target)
        {
            if (onStartTurn != null)
                turnLeft--;
            onStartTurn?.Invoke(target);
        }

        public void ActivateOnEndTurn(CharacterCombatState target)
        {
            if (onEndTurn != null)
                turnLeft--;
            onEndTurn?.Invoke(target);
        }

        public void Deactivate(CharacterCombatState target)
        {
            onDisappear?.Invoke(target);
        }
    }
}
