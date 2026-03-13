namespace Rubik.Combat
{
    public class StateStartTurn : BaseTurnState
    {
        public override void Activate(CharacterCombatState character)
        {
            base.Activate(character);
            // Save game perhaps ?
        }

        public override System.Type GetTurnType()
        {
            return typeof(StateStartTurn);
        }

        public override bool IsComplete()
        {
            return true;
        }
    }
}
