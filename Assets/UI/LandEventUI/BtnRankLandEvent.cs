using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTPackage_old.UI;
using NTFunctions_old;

namespace GOA.LandEvent
{
    public class BtnRankLandEvent : NTButtonEffect
    {
        protected override void Start() {
            gameObject.SetActive(false);
        }

        public void OnClick()
        {
            RankLandEventUI rankLandEventUI = (RankLandEventUI) UIManager.instance.GetPopupUIByCode(PopupCode.RankLandEventUI);
            if(rankLandEventUI != null){
                rankLandEventUI.OnUI();
            }
        }
    }
}