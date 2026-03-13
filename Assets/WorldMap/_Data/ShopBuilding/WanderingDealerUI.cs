using System.Collections;
using System.Collections.Generic;
using NTFunctions_old;
using TMPro;
using UnityEngine;

namespace GOA.ShopBuilding
{
    using NTPackage_old.Functions;
    using SimpleJSON;
    using UserData;
    public class WanderingDealerUI : PopupUI
    {
        public WanderingDealerData WanderingDealerData;

        public TextMeshProUGUI TextNumberCoin;
        public TextMeshProUGUI TextNumberGin;
        public TextMeshProUGUI TextNumberKey;

        public List<NeutralShopBoxUI> NeutralShopBoxUIs;
        public WanderingDealerBoxUI WanderingDealerBoxUICollection;

        public Transform Content;

        public override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadTextNumberCoin();
            this.LoadTextNumberGin();
            this.LoadTextNumberKey();
        }

        protected void LoadTextNumberCoin()
        {
            if (this.TextNumberCoin != null) return;
            this.TextNumberCoin = transform.Find("Panel").Find("Coin").Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        }
        protected void LoadTextNumberGin()
        {
            if (this.TextNumberGin != null) return;
            this.TextNumberGin = transform.Find("Panel").Find("Gin").Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        }
        protected void LoadTextNumberKey()
        {
            if (this.TextNumberKey != null) return;
            this.TextNumberKey = transform.Find("Panel").Find("Key").Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            try
            {
                TextNumberCoin.text = UserData.Instance.data.Coin.ToString();
                TextNumberGin.text = UserData.Instance.data.Gin.ToString();
                TextNumberKey.text = UserData.Instance.Inventory.PortalKey.ToString();
            }
            catch (System.Exception) { }
        }

        public void OnUI(string wanderingDealerID)
        {
            return;
            APIManager.Instance.GetWanderingDealerData(wanderingDealerID, (data) =>
            {
                this.WanderingDealerData = JsonUtility.FromJson<WanderingDealerData>(data);
                if (!this.CanShow()) return;
                this.UpdateData(this.WanderingDealerData);
                this.Show();
            });
        }

        public void UpdateData(WanderingDealerData wanderingDealerData)
        {
            this.UpdateData();
            ObjectPoolingManager.instance.PushChildObjectIntoPooling(this.Content);
            foreach (ItemShopData item in wanderingDealerData.ItemShopDatas)
            {
                WanderingDealerBoxUI wanderingDealerBoxUI = this.InstanceWanderingDealerBoxUI();
                wanderingDealerBoxUI.SetItem(item,this.WanderingDealerData);
                wanderingDealerBoxUI.transform.SetParent(this.Content);
            }
            foreach (Transform item in this.Content)
            {
                NTFunction.ResetPosition(item);
            }
        }

        protected WanderingDealerBoxUI InstanceWanderingDealerBoxUI()
        {
            Transform trans = ObjectPoolingManager.instance.GetObjectFromPooling("WanderingDealerBoxUI");
            WanderingDealerBoxUI wanderingDealerBoxUI = null;
            if (trans != null && trans.TryGetComponent<WanderingDealerBoxUI>(out WanderingDealerBoxUI res))
            {
                wanderingDealerBoxUI = res;
            }
            else
            {
                wanderingDealerBoxUI = Instantiate(this.WanderingDealerBoxUICollection);
            }
            wanderingDealerBoxUI.transform.name = "WanderingDealerBoxUI";
            return wanderingDealerBoxUI;
        }
    }
}
