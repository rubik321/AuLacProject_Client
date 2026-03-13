using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Rubik.Combat;

namespace Rubik.UI
{
    public class EffectElement : MonoBehaviour
    {
        [SerializeField] Image icon;
        [SerializeField] TMP_Text textTurn;
        [SerializeField] TMP_Text textStack;
        [SerializeField] Sprite bgBuff, bgDebuff, bgNeutral;

        public void SetupData(string effectId, int turn, int stack = 1)
        {
            BaseEffectSO effectSO = EffectPool.Instance.GetEffectSO(effectId);
            icon.sprite = effectSO.icon;
            switch (effectSO.effectType)
            {
                case EffectType.Neutral:
                    GetComponent<Image>().sprite = bgNeutral;
                    break;
                case EffectType.Buff:
                    GetComponent<Image>().sprite = bgBuff;
                    break;
                case EffectType.Debuff:
                    GetComponent<Image>().sprite = bgDebuff;
                    break;
            }
            textTurn.text = turn.ToString();
            //textStack.gameObject.SetActive(stack > 1);
            //textStack.text = "x" + stack;
        }
    }
}
