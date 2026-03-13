using CodeHelper.MessageCollection;
using System;

namespace Rubik.Combat
{
    public abstract class BaseTurnState
    {
        protected Action<string> onActivate;
        protected bool isCompleted;
        public abstract Type GetTurnType();
        public abstract bool IsComplete();
        public virtual void Activate(CharacterCombatState character)
        {
            if (character == null)
                onActivate?.Invoke(null);
            else
                onActivate?.Invoke(character.data.characterInstanceId);
            CodeHelper.MessageManager.SendMessage(new CodeHelper.Message(nameof(OnTurnStateActivate), new object[] { GetTurnType() }));
        }
        public virtual void SubscribeOnActivate(Action<string> onActivate)
        {
            this.onActivate += onActivate;
        }
        public virtual void ReleaseObject()
        {

        }
    }
}

