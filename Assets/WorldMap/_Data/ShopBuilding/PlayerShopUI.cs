using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOA.ShopBuilding
{
    using NTFunctions_old;
    using NTPackage_old.Functions;
    using TMPro;
    using UserData;
    public class PlayerShopUI : PopupUI
    {
        public PlayerShopData PlayerShopData;

        public TextMeshProUGUI TextNumberCoin;
        public TextMeshProUGUI TextNumberGin;
        public TextMeshProUGUI TextNumberKey;

        public List<PlayerShopBoxUI> PlayerShopBoxUIs;
        public PlayerShopBoxUI PlayerShopBoxUICollection;

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

        public void OnUI(string playerShopID)
        {
            return;
            APIManager.Instance.GetPlayerShopData(playerShopID, (data) =>
            {
                this.PlayerShopData = JsonUtility.FromJson<PlayerShopData>(data);
                if (!this.CanShow()) return;
                this.UpdateData(this.PlayerShopData);
                this.Show();
            });
        }

        public void UpdateData(PlayerShopData playerShopData)
        {
            this.UpdateData();
            ObjectPoolingManager.instance.PushChildObjectIntoPooling(this.Content);
            if(this.PlayerShopData == null) this.PlayerShopData = playerShopData;
            else{
                this.PlayerShopData.Id = playerShopData.Id;
                this.PlayerShopData.version = playerShopData.version;
                this.PlayerShopData.ItemShopDatas = playerShopData.ItemShopDatas;
                this.PlayerShopData.ItemGearShopDatas = playerShopData.ItemGearShopDatas;
            }
            foreach (ItemShopData item in PlayerShopData.ItemShopDatas)
            {
                PlayerShopBoxUI playerShopBoxUI = this.InstancePlayerShopBoxUI();
                playerShopBoxUI.SetItem(item, this.PlayerShopData);
                playerShopBoxUI.transform.SetParent(this.Content);
            }
            foreach (ItemGearShopData item in PlayerShopData.ItemGearShopDatas)
            {
                if (item.Index == null || item.Index.Length == 0) continue;
                PlayerShopBoxUI playerShopBoxUI = this.InstancePlayerShopBoxUI();
                playerShopBoxUI.SetGear(item, this.PlayerShopData);
                playerShopBoxUI.transform.SetParent(this.Content);
            }
            foreach (Transform item in this.Content)
            {
                NTFunction.ResetPosition(item);
            }
        }

        protected PlayerShopBoxUI InstancePlayerShopBoxUI()
        {
            Transform trans = ObjectPoolingManager.instance.GetObjectFromPooling("PlayerShopBoxUI");
            PlayerShopBoxUI playerShopBoxUI = null;
            if (trans != null && trans.TryGetComponent<PlayerShopBoxUI>(out PlayerShopBoxUI playerShopBox))
            {
                playerShopBoxUI = playerShopBox;
            }
            else
            {
                playerShopBoxUI = Instantiate(this.PlayerShopBoxUICollection);
            }
            playerShopBoxUI.transform.name = "PlayerShopBoxUI";
            return playerShopBoxUI;
        }
    }
}