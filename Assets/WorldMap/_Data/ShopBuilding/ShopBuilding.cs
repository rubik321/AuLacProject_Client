using System.Collections;
using System.Collections.Generic;
using GoShared;
using NTFunctions_old;
using UnityEngine;

namespace GOA.ShopBuilding
{
    using Building;

    [System.Serializable]
    public class ShopBuildingData
    {
        public string Id;
        public ShopBuildingCode Code;
        public double latitude;
        public double longitude;
        public bool IsBuild = false;
    }

    public class ShopBuilding : Building
    {
        public ShopBuildingCode Code;
        public ShopBuildingData shopPlayerData;

        public virtual void UpdateData()
        {
    
        }

        public void Chose()
        {
            this.Holding = true;
            StartCoroutine(this.CountHolding());
        }

        IEnumerator CountHolding()
        {
            yield return new WaitForSeconds(TimeDelay);
            this.Holding = false;
        }

        void OnMouseUp()
        {
            if (!this.Holding) return;
            if (this.shopPlayerData.Code == ShopBuildingCode.NeutralShop)
            {
                NeutralShopUI neutralShopUI = (NeutralShopUI) UIManager.instance.GetPopupUIByCode(PopupCode.NeutralShopUI);
                if(neutralShopUI == null) return;
                neutralShopUI.OnUI(shopPlayerData.Id);
            }
            if (this.shopPlayerData.Code == ShopBuildingCode.WanderingDealer)
            {
                WanderingDealerUI wanderingDealerUI = (WanderingDealerUI) UIManager.instance.GetPopupUIByCode(PopupCode.WanderingDealerUI);
                if(wanderingDealerUI == null) return;
                wanderingDealerUI.OnUI(shopPlayerData.Id);
            }

        }
    }
}
