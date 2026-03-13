using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NTFunctions_old;

namespace GOA.Shop{
    public class ShopBoxUI : LoadBehaviour
    {
        public Image ava;
        public TextMeshProUGUI titleItemName;
        public TextMeshProUGUI textDescript;
        public TextMeshProUGUI textCost;

        public void Buy(){
            Debug.LogWarning("Buy");
            InforItemShopUI inforItemShopUI = (InforItemShopUI)UIManager.instance.GetPopupUIByCode(PopupCode.InforItemShopUI);
            // if(inforItemShopUI == null) return;
            // inforItemShopUI.OnUI();
        }

        public void ShowInfo(){
            
        }
    }
}
