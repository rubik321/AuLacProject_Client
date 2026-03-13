using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NTFunctions_old;
using GOA.UserData;

namespace WorldMap.Header{
    public class AccUI : LoadBehaviour
    {
        public Image avatar,processHP,processMP;
        public TextMeshProUGUI textName;
        public TextMeshProUGUI textLv;
        public TextMeshProUGUI DailyIncome;
        private void Start()
        {
            
        }
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            this.UpdateData();
        }

        public void UpdateData(){
            this.UpdateAvatar();
            this.UpdateText();
          //  SetProcessbar(UserData.Instance.characterData.CurrentHP, UserData.Instance.characterData.HP, processHP);
           // SetProcessbar(UserData.Instance.characterData.CurrentMP, UserData.Instance.characterData.MP, processMP);
        }

        public void UpdateAvatar(){

        }

        public void UpdateText(){
            try
            {
                this.textName.text = UserData.Instance.data.DisplayName;
                this.textLv.text = UserData.Instance.characterData.Level.ToString();
                
                this.DailyIncome.text = Lean.Localization.LeanLocalization.GetTranslationText("daily_income", "Daily Income:") +" "+UserData.Instance.data.DailyIncome.ToString();
            }
            catch (System.Exception)
            {
                
            }
        }
        void SetProcessbar(float current, float max, Image bar)
        {
            bar.fillAmount = current / max;
        }
    }

}
