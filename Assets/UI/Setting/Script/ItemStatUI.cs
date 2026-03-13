using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTFunctions_old;
using TMPro;
using Lean.Localization;
using GOA.UserData;
namespace GOA.UIMenu{
    public class ItemStatUI : LoadBehaviour
    {
        public TextMeshProUGUI text;
        public TextMeshProUGUI textStats;

        public override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadText();
            this.LoadTextStats();
        }
        protected void LoadText(){
            if(this.text != null) return;
            this.text = transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        }

        protected void LoadTextStats(){
            if(this.textStats != null) return;
            this.textStats = transform.Find("TextStat(TMP)").GetComponent<TextMeshProUGUI>();
        }

        public void SetData(float stat){
            this.textStats.text = "+" + stat + " " + LeanLocalization.GetTranslationText("per_level", "per level");
            
        }
        public void SetData(string stat)
        {
            this.textStats.text = stat;

        }
    }
}
