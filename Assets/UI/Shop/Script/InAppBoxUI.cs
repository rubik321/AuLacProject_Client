using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NTFunctions_old;
using Rubik.UI;
using UnityEngine.Purchasing;
namespace GOA.Shop{
    using UserData;
    using Item;

    public class InAppBoxUI : MonoBehaviour
    {
        public Image ava;
        public TextMeshProUGUI titleItemName;
        public TextMeshProUGUI textDescript;
        public TextMeshProUGUI cost,costCash;
        public Image Icon;

        public GOA.Item.ItemInfoData ItemData;

        public Transform Gold;
        public Transform Gin;
        public Transform RealMoney;
        public GameObject btnBuyGold, btnBuyCash,hightlightGo;
        private void Start()
        {
            hightlightGo.SetActive(false);
        }
        public void SetData(GOA.Item.ItemInfoData itemData){
            this.ItemData = itemData;
            this.cost.text = this.ItemData.Price.ToString();
            this.costCash.text = this.ItemData.Price.ToString();
            this.textDescript.text = Lean.Localization.LeanLocalization.GetTranslationText(this.ItemData.Index + "_Description", "Description");
            this.titleItemName.text = Lean.Localization.LeanLocalization.GetTranslationText(this.ItemData.Index + "_Name", "Item");
            try
            {
                this.Icon.sprite = SpriteHelper.Instance.GetSprite(this.ItemData.Images);
            }
            catch (System.Exception){}

            this.Gold.gameObject.SetActive(false);
            this.Gin.gameObject.SetActive(false);
            this.RealMoney.gameObject.SetActive(false);
            switch (itemData.Currency)
            {
                case CurrencyType.Gold:
                    this.Gold.gameObject.SetActive(true);
                    btnBuyGold.SetActive(true);
                    btnBuyCash.SetActive(false);
                    break;
                case CurrencyType.Gin:
                    this.Gin.gameObject.SetActive(true);
                    btnBuyGold.SetActive(true);
                    btnBuyCash.SetActive(false);
                    break;
                case CurrencyType.RealMoney:
                   // btnBuyCash.GetComponent<IAPButton>().productId = "ginpack";
                    btnBuyGold.SetActive(false);
                    btnBuyCash.SetActive(true);
                    this.RealMoney.gameObject.SetActive(true);
                    break;
            }
        }

        public void Buy(){
            // if(this.ItemData.Code == ItemCode.SaikiSeal || this.ItemData.Code == ItemCode.MysticMandate){
            //     HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("coming_soon", "Coming soon!"));
            //     return;
            // }
            if(this.ItemData.Currency == CurrencyType.Gold){
                if(UserData.Instance.data.Coin < this.ItemData.Price){
                    HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("dont_enough_gold", "You have insufficient Gold."));
                    return;
                }
            }
            if(this.ItemData.Currency == CurrencyType.Gin){
                if(UserData.Instance.data.Gin < this.ItemData.Price){
                    HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("dont_enough_gin", "You have insufficient Gin."));
                    return;
                }
            }
            if(this.ItemData.Currency == CurrencyType.RealMoney){
                HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("coming_soon", "Coming soon!"));
                return;
            }
            InforItemInAppUI inforItemInAppUI = (InforItemInAppUI)UIManager.instance.GetPopupUIByCode(PopupCode.InforItemInAppUI);
            if(inforItemInAppUI == null) return;
            inforItemInAppUI.OnUI(this.ItemData);
           //  APIManager.Instance.BuyItemInShop(UserData.Instance.data.UserId, (int)ItemData.Code, this.BuySuccess);
        }

        public void BuySuccess(){
            if (this.ItemData.Currency == CurrencyType.RealMoney)
            {
                APIManager.Instance.BuyItemInShop(UserData.Instance.data.UserId, 7,(System.Action)(()=> {
                    UserData.Instance.data.Gin += 1000;
                    Rubik.Common.AudioHelper.AudioCtrl.Instance.Play(Rubik.Common.AudioHelper.AudioName.UI_Popup_Panel_Daily_Reward);
                    // HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("buy_success", "Buy success"));
                }));
            }else
            {
                UserData.Instance.Inventory.AddInventoryByCode(this.ItemData.Code, 1);
                if (this.ItemData.Currency == CurrencyType.Gold)
                {
                    UserData.Instance.data.Coin -= this.ItemData.Price;
                }
                if (this.ItemData.Currency == CurrencyType.Gin)
                {
                    UserData.Instance.data.Coin -= this.ItemData.Price;
                }
                Rubik.Common.AudioHelper.AudioCtrl.Instance.Play(Rubik.Common.AudioHelper.AudioName.UI_Popup_Panel_Daily_Reward);
                // HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("buy_success", "Buy success"));
            }
           
           
           
        }
        public void BuyIAPFail()
        {
            HUDCanvas.Instance.ShowNotification("Failed");
        }
        public void ShowInforInApp(){
            
        }
    }
}
