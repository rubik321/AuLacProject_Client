using UnityEngine;

namespace Rubik.Combat
{
    public class TriggerEnemyHitAnimation : MonoBehaviour
    {
        // Assign to animation event through reflection. DO NOT DELETE
        public void TriggerAnimation()
        {
            CodeHelper.MessageManager.SendMessage(new CodeHelper.Message(nameof(CodeHelper.MessageCollection.OnAttackHitFrame)));
        }
    }
}

