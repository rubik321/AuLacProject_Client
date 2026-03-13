namespace Rubik.Combat
{
    public class StateChooseAction : BaseTurnState
    {
        public override void Activate(CharacterCombatState character)
        {
            base.Activate(character);
            isCompleted = false;
            CharacterManager.Instance.GetCharacterObject(character.data.characterInstanceId)
                .ChooseAction(() => isCompleted = true);
        }

        public override System.Type GetTurnType()
        {
            return typeof(StateChooseAction);
        }

        public override bool IsComplete()
        {
            return isCompleted;
        }
    }
}
