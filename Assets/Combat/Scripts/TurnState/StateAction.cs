namespace Rubik.Combat
{
    public class StateAction : BaseTurnState
    {
        public override void Activate(CharacterCombatState character)
        {
            base.Activate(character);
            isCompleted = false;
            CharacterManager.Instance.GetCharacterObject(character.data.characterInstanceId)
                .Action(() => isCompleted = true);
        }

        public override System.Type GetTurnType()
        {
            return typeof(StateAction);
        }

        public override bool IsComplete()
        {
            return isCompleted;
        }
    }
}
