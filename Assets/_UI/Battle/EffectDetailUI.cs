using System.Collections.Generic;
using NTPackage.Functions;
using NTPackage.UI;
using Rubik.BattleEngine;
using Rubik.CardPlayer;
using Rubik.Myrk.Skill;
using TMPro;
using UnityEngine;

namespace Rubik.Myrk.Battle
{
    public class EffectDetailUI : PopupUI
    {

        public CardAvatarUI CardAvatarUI;
        public ListSkillItemUI ListSkillItemUI;

        public Transform HolderEffect;
        public EffectDetailElement EffectDetailElementPrefab;
        public List<EffectDetailElement> ListEffectDetailElement;
        public TextMeshProUGUI EmptyEffect;
        public RectTransform BoardTrans;

        public float HasEffectHeight = 851;
        public float NoEffectHeight = 500;

        public void SetData(List<BuffDataShort> buffDataShorts, CardPlayerIndex cardPlayerIndex, int star, int lv)
        {
            this.CardAvatarUI.SetData(cardPlayerIndex, star, lv);
            CardSkillLv cardSkillLv = CardPlayerManager.Instance.GetCardSkill(cardPlayerIndex, star);
            this.ListSkillItemUI.SetData(cardSkillLv);

            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.HolderEffect);

            for (int i = 0; i < buffDataShorts.Count; i++)
            {
                EffectDetailElement effectDetailElement = ObjectPoolingManager.Instance.InstantiateObject<EffectDetailElement>(ObjectPoolingConfig.EffectDetailElement, this.EffectDetailElementPrefab.transform);
                effectDetailElement.transform.SetParent(this.HolderEffect);
                NTFunction.ResetPosition(effectDetailElement.transform);
                effectDetailElement.SetData(buffDataShorts[i]);
            }
            if (buffDataShorts.Count == 0)
            {
                this.EmptyEffect.gameObject.SetActive(true);
                this.BoardTrans.sizeDelta = new Vector2(this.BoardTrans.sizeDelta.x, this.NoEffectHeight);
            }
            else
            {
                this.EmptyEffect.gameObject.SetActive(false);
                this.BoardTrans.sizeDelta = new Vector2(this.BoardTrans.sizeDelta.x, this.HasEffectHeight);
            }

        }

        public override void ScriptOffUI()
        {
            base.ScriptOffUI();
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.HolderEffect);
        }
    }
}
