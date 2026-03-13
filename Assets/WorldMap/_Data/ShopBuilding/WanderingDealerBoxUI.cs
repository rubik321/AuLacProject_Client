using System.Collections;
using System.Collections.Generic;
using GOA.Item;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Rubik.UI;

namespace GOA.ShopBuilding
{
    using UserData;
    using Item;
    using NTFunctions_old;
    using SimpleJSON;
    using GOA.Shop;

    public class WanderingDealerBoxUI : MonoBehaviour
    {
        public TextMeshProUGUI titleItemName;
        public TextMeshProUGUI textDescript;
        public TextMeshProUGUI cost, costCash;
        public Image Icon;

        public WanderingDealerData WanderingDealerData;
        public GOA.Item.ItemInfoData ItemData;

        public ItemShopData ItemShopData;

        public CurrencyType Currency;
        public float Price;
        public bool StackBuy = false;
        public int MaxAmount = 1;

        public Transform Gold;
        public Transform Gin;
        public Transform RealMoney;
        public GameObject btnBuyGold, btnBuyCash;
        public Transform TransCover;

        public void SetItem(ItemShopData itemShopData, WanderingDealerData wanderingDealerData)
        {
            this.Currency = itemShopData.Currency;
            this.Price = itemShopData.Price;
            this.StackBuy = true;
            if (itemShopData.Unlimited)
            {
                this.MaxAmount = 99;
            }
            else
            {
                this.MaxAmount = (int)itemShopData.Amount;
            }
            this.ItemShopData = itemShopData;
            this.WanderingDealerData = wanderingDealerData;
            this.ItemData = ItemAsset.instance.GetItemDataByCode(this.ItemShopData.Code);
            this.SetData(ItemData);
        }

        public void SetData(GOA.Item.ItemInfoData itemDat)
        {
            this.cost.text = this.ItemShopData.Price.ToString();
            this.costCash.text = this.ItemShopData.Price.ToString();
            this.textDescript.text = this.textDescript.text = NTFunction.CollapString(Lean.Localization.LeanLocalization.GetTranslationText(this.ItemShopData.Index + "_Description", "Description"), 100);
            if (this.ItemShopData.Unlimited)
            {
                this.titleItemName.text = Lean.Localization.LeanLocalization.GetTranslationText(this.ItemData.Index + "_Name", "Item");
            }
            else
            {
                this.titleItemName.text = Lean.Localization.LeanLocalization.GetTranslationText(this.ItemData.Index + "_Name", "Item") + " (Remaining: " + this.ItemShopData.Amount + ")";
            }
            try
            {
                this.Icon.sprite = SpriteHelper.Instance.GetSprite(this.ItemData.Images);
            }
            catch (System.Exception) { }

            this.Gold.gameObject.SetActive(false);
            this.Gin.gameObject.SetActive(false);
            this.RealMoney.gameObject.SetActive(false);
            switch (this.ItemShopData.Currency)
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
            if (this.ItemShopData.Amount <= 0 && !this.ItemShopData.Unlimited)
            {
                this.TransCover.gameObject.SetActive(true);
                this.btnBuyGold.gameObject.SetActive(false);
            }
            else
            {
                this.TransCover.gameObject.SetActive(false);
                this.btnBuyGold.gameObject.SetActive(true);
            }
        }
        public void Buy()
        {
            if (this.ItemData == null)
            {
                return;
            }
            if (this.ItemShopData.Currency == CurrencyType.Gold)
            {
                if (UserData.Instance.data.Coin < this.ItemShopData.Price)
                {
                    HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("dont_enough_gold", "You have insufficient Gold."));
                    return;
                }
            }
            if (this.ItemShopData.Currency == CurrencyType.Gin)
            {
                if (UserData.Instance.data.Gin < this.ItemShopData.Price)
                {
                    HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("dont_enough_gin", "You have insufficient Gin."));
                    return;
                }
            }
            if (this.ItemShopData.Currency == CurrencyType.RealMoney)
            {
                HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("coming_soon", "Coming soon!"));
                return;
            }
            InforItemShopUI inforItemShopUI = (InforItemShopUI)UIManager.instance.GetPopupUIByCode(PopupCode.InforItemShopUI);
            inforItemShopUI.OnUI(this.Icon.sprite, this.titleItemName.text, this.textDescript.text, Currency, Price, this.StackBuy, 0, 0, this.ConfirmBuy, this.MaxAmount);
            //  APIManager.Instance.BuyItemInShop(UserData.Instance.data.UserId, (int)ItemData.Code, this.BuySuccess);
        }

        public void ConfirmBuy(int amount)
        {
            int itemID = -1;
            string slot = "";
            if (this.ItemData != null)
            {
                itemID = (int)this.ItemData.Code;
                slot = this.ItemShopData.Slot;
            }
            APIManager.Instance.BuyWanderingDealerData(itemID, slot, amount, this.WanderingDealerData.Id, (System.Action<string>)((data) =>
            {
                JSONNode jdata = JSON.Parse(data);
                Debug.LogWarning(jdata["buyItem"]);
                Debug.LogWarning(jdata["isSuccess"]);
                if (jdata["isSuccess"])
                {
                    Debug.LogWarning("Buy success");
                    this.BuySuccess(amount);
                    Rubik.Common.AudioHelper.AudioCtrl.Instance.Play(Rubik.Common.AudioHelper.AudioName.UI_Popup_Panel_Daily_Reward);
                    // HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("purchase_successful", "Purchase Successful"));
                }
                else
                {
                    HUDCanvas.Instance.ShowNotification(jdata["message"]);
                }
                try
                {
                    WanderingDealerUI wanderingDealerUI = (WanderingDealerUI)UIManager.instance.GetPopupUIByCode(PopupCode.WanderingDealerUI);
                    wanderingDealerUI.UpdateData(JsonUtility.FromJson<WanderingDealerData>(jdata["wanderingDealerData"].ToString()));
                }
                catch (System.Exception) { }
            }));
        }


        public void BuySuccess(int amount)
        {
            UserData.Instance.Inventory.AddInventoryByCode(this.ItemData.Code, amount);
            if (this.ItemShopData.Currency == CurrencyType.Gold)
            {
                UserData.Instance.data.Coin -= this.ItemShopData.Price * amount;
                QuestManager.Instance.UpdateQuest(QuestType.Spend_Gold, (int)this.ItemShopData.Price * amount);
            }
            if (this.ItemShopData.Currency == CurrencyType.Gin)
            {
                UserData.Instance.data.Gin -= this.ItemShopData.Price * amount;
                QuestManager.Instance.UpdateQuest(QuestType.Spend_Gin, (int)this.ItemShopData.Price * amount);
            }
        }
        public void ShowInforInApp()
        {

        }
    }
}
