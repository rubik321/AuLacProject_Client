using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTFunctions_old;
using TMPro;

namespace GOA.UIMenu{
    public class DailyIncomeUI : PopupUI
    {
        public TextMeshProUGUI titleIncomeReport;
        public TextMeshProUGUI textGold;
        public TextMeshProUGUI textGin;
        public override void LoadComponents()
        {
            base.LoadComponents();
            this.lvUI = new PopupLv().GetValue(transform.name);
        }

        [ContextMenu("OnUI")]
        public void OnUI(){
            return;
            this.Show();
            this.UpdateData();
        }

        public override void UpdateData(){
            DateTime currentDate = DateTime.Now;
            this.titleIncomeReport.text = Lean.Localization.LeanLocalization.GetTranslationText("title_income_report", "Income Report:") +" "+ currentDate.Year+"/"+currentDate.Month+"/"+currentDate.Day;
        }

        public void Continue(){

        }
    }
}