
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rubik.UI.Statitic
{
    public class StatUI : MonoBehaviour
    {
        public List<Sprite> IconList;
        public Image Icon;
        public TextMeshProUGUI Text;

        public TextMeshProUGUI TextAdd;
        public Coroutine CoroutineAnimaAddValue;
        public float TimeShowValue = 0f;

        public StatData StatData;

        public void SetData(StatData statData){
            this.StatData = statData;
            this.TimeShowValue = 0f;
            this.Icon.sprite = this.IconList[(int)statData.TypeStat];
            this.Text.text = statData.Value.ToString();
        }

        public void ShowValue(long value){
            this.TimeShowValue = 5f;
            this.TextAdd.text = "+" + value.ToString();
            if(this.CoroutineAnimaAddValue == null){
                this.CoroutineAnimaAddValue = StartCoroutine(this.AnimaAddValue());
            }
        }

        public IEnumerator AnimaAddValue(){
            this.TextAdd.transform.localScale = Vector3.zero;
            this.TextAdd.gameObject.SetActive(true);
            this.TextAdd.transform.DOScale(1, 0.25f);
            while(this.TimeShowValue > 0){
                yield return new WaitForSeconds(0.5f);
                this.TimeShowValue -= 0.5f;
            }
            this.TextAdd.transform.DOScale(0, 0.25f);
            yield return new WaitForSeconds(0.25f);
            this.TextAdd.gameObject.SetActive(false);
            this.CoroutineAnimaAddValue = null;
        }

        public void HideValue(){
            this.TimeShowValue = 0.25f;
        }

        public void Onclick(){
            string title = Lean.Localization.LeanLocalization.GetTranslationText("stat_name_" + (int)this.StatData.TypeStat, "Stat");
            string description = Lean.Localization.LeanLocalization.GetTranslationText("stat_des_" + (int)this.StatData.TypeStat, "Stat");
            HUDCanvas.Instance.ShowToolTip(title, description);
        }

    }
}