using UnityEngine;

namespace Rubik.Combat
{
    public class StateEndTurn : BaseTurnState
    {
        float startTime;
        public override void Activate(CharacterCombatState character)
        {
            // Do something that happen to the field after a turn end
            base.Activate(character);
            startTime = Time.time;
        }

        public override System.Type GetTurnType()
        {
            return typeof(StateEndTurn);
        }

        public override bool IsComplete()
        {
            return Time.time - startTime > 1;
        }
    }
}
