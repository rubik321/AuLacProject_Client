using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTFunctions_old;
using GOA.WorldMap.Outpost;

namespace GOA.WorldMap.Outpost{
    public class BtnOutPostUI : BaseButton
    {
        protected override void OnClick()
        {
            OutpostUI outpostUI = (OutpostUI) UIManager.instance.GetPopupUIByCode(PopupCode.OutpostUI);
            if(outpostUI == null) return;
            outpostUI.Show();
        }
    }
}
