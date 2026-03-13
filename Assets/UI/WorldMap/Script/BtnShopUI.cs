using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTFunctions_old;

namespace GOA.UIWorldMap{
    public class BtnShopUI : BaseButton
    {
        protected override void OnClick()
        {
            base.OnClick();
          //  try
            {
                GOA.Shop.ShopUI shopUI = (GOA.Shop.ShopUI) UIManager.instance.GetPopupUIByCode(PopupCode.ShopUI);
                if(shopUI == null) return;
                shopUI.OnUI();
            }
            //catch (System.Exception e)
            //{
            //    Debug.LogWarning(e);
            //}
        }

        protected override void OnSound()
        {
           // Rubik.Common.AudioHelper.AudioCtrl.instance.Play(Rubik.Common.AudioHelper.AudioName.UI_Button_Worldmap_2);
        }
    }
}
