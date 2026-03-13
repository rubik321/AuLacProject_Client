namespace Rubik.Combat
{
    public class StateStartTurnEffect : BaseTurnState
    {
        public override void Activate(CharacterCombatState character)
        {
            base.Activate(character);
            isCompleted = false;
            CharacterManager.Instance.GetCharacterObject(character.data.characterInstanceId)
                .ApplyStartTurnEffect(() => isCompleted = true);
        }

        public override System.Type GetTurnType()
        {
            return typeof(StateStartTurnEffect);
        }

        public override bool IsComplete()
        {
            return isCompleted;
        }
    }
}
