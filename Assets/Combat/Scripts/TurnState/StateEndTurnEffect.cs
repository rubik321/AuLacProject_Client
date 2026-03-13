namespace Rubik.Combat
{
    public class StateEndTurnEffect : BaseTurnState
    {
        public override void Activate(CharacterCombatState character)
        {
            base.Activate(character);
            isCompleted = false;
            CharacterManager.Instance.GetCharacterObject(character.data.characterInstanceId)
                .ApplyEndTurnEffect(() => isCompleted = true);
        }
        public override System.Type GetTurnType()
        {
            return typeof(StateEndTurnEffect);
        }

        public override bool IsComplete()
        {
            return isCompleted;
        }
    }
}
