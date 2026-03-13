using Rubik.BattleEngine;
using Rubik.Myrk.Battle;
using UnityEngine;
using TMPro;
using NTPackage.Functions;

namespace Rubik.Myrk.Battle
{
    public class EffectDetailElement : MonoBehaviour
    {
        public Transform Holder;
        public TextMeshProUGUI EffectName;
        public TextMeshProUGUI EffectDescription;

        public BuffDataShort BuffDataShort;

        public void SetData(BuffDataShort buffDataShort)
        {
            this.BuffDataShort = buffDataShort;
            this.EffectName.text = BattleEngineController.Instance.GetEffectName(buffDataShort.Type);
            this.EffectDescription.text = BattleEngineController.Instance.GetEffectDescription(buffDataShort.Type);
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.Holder);
            EffectIcon effectIcon = BattleEngineController.Instance.GetEffectIcon(buffDataShort.Type, buffDataShort.Amount);
            effectIcon.transform.SetParent(this.Holder);
            NTFunction.ResetPosition(effectIcon.transform);
        }
    }
}
