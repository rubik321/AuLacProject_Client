using System.Collections;
using System.Collections.Generic;
using NTFunctions_old;
using TMPro;
using UnityEngine;

namespace GOA.ShopBuilding
{
    using NTPackage_old.Functions;
    using UserData;

    public class NeutralShopUI : PopupUI
    {
        public NeutralShopData NeutralShopData;

        public TextMeshProUGUI TextNumberCoin;
        public TextMeshProUGUI TextNumberGin;
        public TextMeshProUGUI TextNumberKey;

        public List<NeutralShopBoxUI> NeutralShopBoxUIs;
        public NeutralShopBoxUI NeutralShopBoxUICollection;

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

        public void OnUI(string neutralShopID)
        {
            return;
            APIManager.Instance.GetNeutralShopData(neutralShopID, (data) =>
            {
                this.NeutralShopData = JsonUtility.FromJson<NeutralShopData>(data);
                if (!this.CanShow()) return;
                this.UpdateData(this.NeutralShopData);
                this.Show();
            });
        }

        public void UpdateData(NeutralShopData neutralShopData)
        {
            this.UpdateData();
            ObjectPoolingManager.instance.PushChildObjectIntoPooling(this.Content);
            if(this.NeutralShopData == null) this.NeutralShopData = neutralShopData;
            else{
                this.NeutralShopData.Id = neutralShopData.Id;
                this.NeutralShopData.version = neutralShopData.version;
                this.NeutralShopData.ItemShopDatas = neutralShopData.ItemShopDatas;
                this.NeutralShopData.ItemGearShopDatas = neutralShopData.ItemGearShopDatas;
            }
            foreach (ItemShopData item in NeutralShopData.ItemShopDatas)
            {
                NeutralShopBoxUI neutralShopBoxUI = this.InstanceNeutralShopBoxUI();
                neutralShopBoxUI.SetItem(item,this.NeutralShopData);
                neutralShopBoxUI.transform.SetParent(this.Content);
            }
            foreach (ItemGearShopData item in NeutralShopData.ItemGearShopDatas)
            {
                if(item.Index == null || item.Index.Length == 0) continue;
                NeutralShopBoxUI neutralShopBoxUI = this.InstanceNeutralShopBoxUI();
                neutralShopBoxUI.SetGear(item,this.NeutralShopData);
                neutralShopBoxUI.transform.SetParent(this.Content);
            }
            foreach (Transform item in this.Content)
            {
                NTFunction.ResetPosition(item);
            }
        }

        protected NeutralShopBoxUI InstanceNeutralShopBoxUI()
        {
            Transform trans = ObjectPoolingManager.instance.GetObjectFromPooling("NeutralShopBoxUI");
            NeutralShopBoxUI neutralShopBoxUI = null;
            if (trans != null && trans.TryGetComponent<NeutralShopBoxUI>(out NeutralShopBoxUI neutralShopBox))
            {
                neutralShopBoxUI = neutralShopBox;
            }
            else
            {
                neutralShopBoxUI = Instantiate(this.NeutralShopBoxUICollection);
            }
            neutralShopBoxUI.transform.name = "NeutralShopBoxUI";
            return neutralShopBoxUI;
        }
    }
}