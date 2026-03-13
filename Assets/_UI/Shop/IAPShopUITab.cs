using System.Collections;
using System.Collections.Generic;
using NTPackage.UI;
using UnityEngine;

namespace Rubik.Myrk.Shop
{
    public class IAPShopUITab : TabUI
    {
        public IAPShopUI IAPShopUI;

        public override void OnUI()
        {
            base.OnUI();
            this.IAPShopUI.OnTab(this.number);
        }
    }
}
