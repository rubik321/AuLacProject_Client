using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTFunctions_old;

namespace GOA.UIWorldMap{
    public class BtnInventoryUI : BaseButton
    {
        protected override void OnClick()
        {
            base.OnClick();
            try
            {
                UIManager.instance.OnBtnInventory_Onclick();
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e);
            }
        }
    }
}