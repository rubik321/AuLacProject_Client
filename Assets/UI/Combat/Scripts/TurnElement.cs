using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Rubik.CardPlayer;
using Rubik.BattleEngine;
using Rubik.UI.Statitic;
using NTPackage.Functions;
using Rubik.Myrk.Battle;
using NTPackage.UI;
using Rubik.DataType;

namespace Rubik.UI
{
    public class TurnElement : MonoBehaviour
    {
        public string ID;
        public Image icon,bg,origin;
        public Sprite bgEnemy, bgAlly;
        public Image hpProcess, apProcess;
        public List<Image> lsStars;
        public GameObject hightL1, hightL2;
        public Material blinkMat;

        public StarUI StarUI;

        public Transform EffectIconParent;

        public List<BuffDataShort> Buffs;

        public CardPlayerIndex CardPlayerIndex;
        public int Star;
        public int Lv;

        public void TurnEffectBuff(BuffDataShort[] buffs)
        {
            this.Buffs.Clear();
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(EffectIconParent);
            foreach (BuffDataShort idBuff in buffs){
                EffectIcon effectIcon = BattleEngineController.Instance.GetEffectIcon(idBuff.Type, idBuff.Amount);
                if(effectIcon != null){
                    effectIcon.transform.SetParent(EffectIconParent);
                    NTFunction.ResetPosition(effectIcon.transform);
                    this.Buffs.Add(idBuff);
                }
            }
        }
        public void SetSide(bool isEnemy)
        {
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(EffectIconParent);
            //ringEnemy.SetActive(isEnemy);
            //ringAlly.SetActive(!isEnemy);

            if (isEnemy)
            {
                bg.sprite = bgEnemy;
            }
            else
            {
                bg.sprite = bgAlly;
            }
        }
        public void SetHpBar(float value =0)
        {
            
            hpProcess.DOFillAmount(value,0.5f) ;
        }
        public void SetApBar(float value = 0)
        {
            apProcess.DOFillAmount(value, 0.5f);
        }
        public void SetCharacterAva(int Index)
        {
            this.CardPlayerIndex = (CardPlayerIndex)Index;
            OriginType OriginIndex = CardPlayerManager.Instance.GetCardPlayerDataByIndex((CardPlayerIndex)Index).Origin;
            var cardPlayer = CardPlayerManager.Instance.InstantiatePlayerAvatar((CardPlayerIndex)Index);
            origin.sprite = CardPlayerManager.Instance.GetOriginSpriteFlag(OriginIndex);
            cardPlayer.SetParent(icon.transform, false);
            cardPlayer.localPosition = Vector3.zero;
            cardPlayer.localScale = Vector3.one;
            cardPlayer.eulerAngles = Vector3.zero;
            
        }
        public void SetStar(int star)
        {
            this.Star = star;
            this.StarUI.SetStar(star);
        }

        public void SetLv(int lv)
        {
            this.Lv = lv;
        }
        public void SetHightLight(bool isOn)
        {
            hightL1.SetActive(isOn);
            hightL2.SetActive(isOn);
            if (isOn)
            {
                transform.localScale = new Vector2(1.3f, 1.3f);
                bg.material = blinkMat;
            }
            else
            {
                bg.material = null;
                transform.localScale = new Vector2(1f, 1f);
            }
        }

        public void OnClickEffectDetail(){
            PopupManager.Instance.OnUI(PopupCode.EffectDetailUI, null, (popup) => {
                EffectDetailUI effectDetailUI = popup as EffectDetailUI;
                effectDetailUI.SetData(this.Buffs, this.CardPlayerIndex, this.Star, this.Lv);
            });
        }
    }
}
