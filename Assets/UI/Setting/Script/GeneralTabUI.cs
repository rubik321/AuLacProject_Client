using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTFunctions_old;

namespace GOA.UIMenu{
    using UserData;

    public class GeneralTabUI : TabUI
    {
        public void OnclickClasses(){
            ClassesUI classesUI = (ClassesUI) UIManager.instance.GetPopupUIByCode(PopupCode.ClassesUI);
            if(classesUI == null) return;
            classesUI.OnUI();
        }

        public void OnclickSystem(){
            // SettingUI systemUI = (SettingUI) UIManager.instance.GetPopupUIByCode(PopupCode.SystemUI);
            // if(systemUI == null) return;
            // systemUI.OnUI();
        }
        public void OnclickDailyIncome(){
            DailyIncomeUI dailyIncomeUI = (DailyIncomeUI) UIManager.instance.GetPopupUIByCode(PopupCode.DailyIncomeUI);
            if(dailyIncomeUI == null) return;
            dailyIncomeUI.OnUI();
        }
    }
}
