using CodeHelper;

namespace Rubik.Combat
{
    public class StateStart : BaseTurnState, IMessageHandle
    {
        public StateStart()
        {
            MessageManager.AddSubcriber<CodeHelper.MessageCollection.OnCharacterStartTurn>(this);
        }

        public void Handle(Message message)
        {
            isCompleted = true;
        }

        public override void Activate(CharacterCombatState character)
        {
            base.Activate(character);
            isCompleted = false;
        }

        public override System.Type GetTurnType()
        {
            return typeof(StateStart);
        }

        public override bool IsComplete()
        {
            return isCompleted;
        }

        public override void ReleaseObject()
        {
            MessageManager.RemoveSubcriber<CodeHelper.MessageCollection.OnCharacterStartTurn>(this);
        }
    }
}
