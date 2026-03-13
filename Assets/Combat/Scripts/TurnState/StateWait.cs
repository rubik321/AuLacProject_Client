using CodeHelper;

namespace Rubik.Combat
{
    public class StateWait : BaseTurnState, IMessageHandle
    {
        public StateWait()
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
            return typeof(StateWait);
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