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
    using UnityEngine.Purchasing;
    using GOA.Shop;

    public class PlayerShopBoxUI : MonoBehaviour
    {
        public TextMeshProUGUI titleItemName;
        public TextMeshProUGUI textDescript;
        public TextMeshProUGUI cost,costCash;
        public Image Icon;

        public GOA.Item.ItemInfoData ItemData;
        public GOA.Item.ItemGearData ItemGearData;

        public ItemShopData ItemShopData;
        public ItemGearShopData ItemGearShopData;

        public PlayerShopData PlayerShopData;

        public CurrencyType  Currency;
        public float Price;
        public bool StackBuy = false;
        public int MaxAmount = 1;

        public Transform Gold;
        public Transform Gin;
        public Transform RealMoney;
        public GameObject btnBuyGold, btnBuyCash;
        public Transform TransCover;

        public string MoreDetail;

        public int TimeBuy;
        public int CoinInc;

        public void SetItem(ItemShopData itemShopData,PlayerShopData playerShopData){
            if(itemShopData.Unlimited){
                this.MaxAmount = 99;
            }else{
                this.MaxAmount = (int)itemShopData.Amount;
            }
            this.ItemShopData = itemShopData;
            this.PlayerShopData = playerShopData;
            this.ItemGearData = null;
            this.ItemData = ItemAsset.instance.GetItemDataByCode(itemShopData.Code);
            if(ItemData == null) {
                gameObject.SetActive(false);
                return;    
            }
            this.SetDataItem();
        }

        public void SetGear(ItemGearShopData itemGearShopData, PlayerShopData playerShopData){
            this.PlayerShopData = playerShopData;
            this.ItemData = null;
            this.ItemGearShopData = itemGearShopData;
            this.ItemGearData = new ItemGearData();
            this.ItemGearData.Index = itemGearShopData.Index;
            this.ItemGearData.Lv = (int)itemGearShopData.Lv;
            this.ItemGearData.Price = itemGearShopData.Price;
            this.ItemGearData.Currency = itemGearShopData.Currency;
            this.SetDataGear();
        }

        public void SetDataItem(){
            this.StackBuy = true;
            this.cost.text = (this.ItemShopData.Price + this.PlayerShopData.TimeBuy  * this.PlayerShopData.CoinInc).ToString();
            this.costCash.text = (this.ItemShopData.Price + this.PlayerShopData.TimeBuy  * this.PlayerShopData.CoinInc).ToString();
            this.textDescript.text = this.textDescript.text = NTFunction.CollapString(Lean.Localization.LeanLocalization.GetTranslationText(this.ItemShopData.Index + "_Description", "Description"), 100);
            this.MoreDetail = Lean.Localization.LeanLocalization.GetTranslationText(this.ItemShopData.Index + "_Description", "Description");
            if(this.ItemShopData.Unlimited){
                this.titleItemName.text = Lean.Localization.LeanLocalization.GetTranslationText(this.ItemShopData.Index + "_Name", "Item");
            }else{
                this.titleItemName.text = Lean.Localization.LeanLocalization.GetTranslationText(this.ItemShopData.Index + "_Name", "Item")+ " (Remaining: " + this.ItemShopData.Amount+")";
            }
            try
            {
                this.Icon.sprite = SpriteHelper.Instance.GetSprite(this.ItemData.Images);
            }
            catch (System.Exception){}

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
            if(this.ItemShopData.Amount <= 0 && !this.ItemShopData.Unlimited){
                this.TransCover.gameObject.SetActive(true);
                this.btnBuyGold.gameObject.SetActive(false);
            }else{
                this.TransCover.gameObject.SetActive(false);
                this.btnBuyGold.gameObject.SetActive(true);
            }
        }
        public void SetDataGear(){
            this.StackBuy = false;
            this.cost.text = this.ItemGearShopData.Price.ToString();
            this.costCash.text = this.ItemGearShopData.Price.ToString();
            string des = "";
            Gear.GearInfoData gearData = Gear.GearAsset.instance.GetItemDataByIndex(this.ItemGearShopData.Index);

            if(gearData == null){
                gameObject.SetActive(false);
                return;
            }
            if (gearData.DMG > 0) des += "DMG: ???  ";
            if (gearData.STR > 0) des += "STR: ???  ";
            if (gearData.MND > 0) des += "MND: ???  ";
            if (gearData.CRI > 0) des += "CRI: ???  ";
            if (gearData.CRD > 0) des += "CRD: ???  ";
            if (gearData.HIT > 0) des += "HIT: ???  ";
            if (gearData.EVA > 0) des += "EVA: ???  ";
            if (gearData.HP > 0) des += "HP: ???  ";
            if (gearData.MP > 0) des += "MP: ???  ";
            if (gearData.VIT > 0) des += "VIT: ???  ";
            if (gearData.SPI > 0) des += "SPI: ???  ";
            if (gearData.DEX > 0) des += "DEX: ???  ";
            if (gearData.SPD > 0) des += "SPD: ???  ";
            this.textDescript.text = des;
            this.MoreDetail = Lean.Localization.LeanLocalization.GetTranslationText("more_detail_gear", "The stats of this Gear is randomized, and will be visible upon purchase.");
            this.titleItemName.text = gearData.Name + " Lv." + this.ItemGearShopData.Lv;
            try
            {
                this.Icon.sprite = SpriteHelper.Instance.GetSprite(this.ItemGearShopData.Index);
            }
            catch (System.Exception){}

            this.Gold.gameObject.SetActive(false);
            this.Gin.gameObject.SetActive(false);
            this.RealMoney.gameObject.SetActive(false);
            switch (this.ItemGearShopData.Currency)
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
            if(this.ItemGearShopData.Amount <= 0 && !this.ItemGearShopData.Unlimited){
                this.TransCover.gameObject.SetActive(true);
                this.btnBuyGold.gameObject.SetActive(false);
            }else{
                this.TransCover.gameObject.SetActive(false);
                this.btnBuyGold.gameObject.SetActive(true);
            }
        }

        public void Buy(){
            if(this.ItemData == null){
                Currency = this.ItemGearData.Currency;
                Price = this.ItemGearData.Price;
                this.TimeBuy = 0;
                this.CoinInc = 0;
            }else{
                Currency = this.ItemData.Currency;
                Price = this.ItemData.Price;
                this.TimeBuy = this.PlayerShopData.TimeBuy;
                this.CoinInc = this.PlayerShopData.CoinInc;
            }
            if(Currency == CurrencyType.Gold){
                if(UserData.Instance.data.Coin < Price){
                    HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("dont_enough_gold", "You have insufficient Gold."));
                    return;
                }
            }
            if(Currency == CurrencyType.Gin){
                if(UserData.Instance.data.Gin < Price){
                    HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("dont_enough_gin", "You have insufficient Gin."));
                    return;
                }
            }
            InforItemShopUI inforItemShopUI = (InforItemShopUI) UIManager.instance.GetPopupUIByCode(PopupCode.InforItemShopUI);
            inforItemShopUI.OnUI(this.Icon.sprite, this.titleItemName.text, this.MoreDetail, Currency, Price, this.StackBuy,this.TimeBuy, this.CoinInc, this.ConfirmBuy, this.MaxAmount);
            
           //  APIManager.Instance.BuyItemInShop(UserData.Instance.data.UserId, (int)ItemData.Code, this.BuySuccess);
        }

        public void ConfirmBuy(int amount){
            int itemID = -1;
            string slot = "";
            if (this.ItemData != null){
                itemID = (int)this.ItemData.Code;
                slot = this.ItemShopData.Slot;
            }
            string gearID = "";
            if (this.ItemGearData != null){
                gearID = this.ItemGearData.Index;
                slot = this.ItemGearShopData.Slot;
            }
            APIManager.Instance.BuyPlayerShopData(itemID, gearID, slot, amount, this.PlayerShopData.Id, (System.Action<string>)((data)=>{
                JSONNode jdata = JSON.Parse(data);
                Debug.LogWarning(jdata["buyGear"]);
                Debug.LogWarning(jdata["buyItem"]);
                Debug.LogWarning(jdata["isSuccess"]);
                if (jdata["isSuccess"])
                {
                    Debug.LogWarning("Buy success");
                    this.BuySuccess(amount);
                    Rubik.Common.AudioHelper.AudioCtrl.Instance.Play(Rubik.Common.AudioHelper.AudioName.UI_Popup_Panel_Daily_Reward);
                    // HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("purchase_successful", "Purchase Successful"));
                }else{
                    HUDCanvas.Instance.ShowNotification(jdata["message"]);
                }
                try
                {
                    PlayerShopUI playerShopUI = (PlayerShopUI)UIManager.instance.GetPopupUIByCode(PopupCode.PlayerShopUI);
                    playerShopUI.UpdateData(JsonUtility.FromJson<PlayerShopData>(jdata["shopPlayerData"].ToString()));
                }
                catch (System.Exception){}
            }));
        }
        public void BuySuccess(int amount){
            if(this.ItemData == null){
                APIManager.Instance.GetUserGear();
            }else{
                UserData.Instance.data.Coin -= (amount + this.PlayerShopData.TimeBuy - 1) * this.PlayerShopData.CoinInc;
                this.PlayerShopData.TimeBuy += amount;
                UserData.Instance.Inventory.AddInventoryByCode(this.ItemData.Code, amount);
                QuestManager.Instance.UpdateQuest(QuestType.Buy_Item, amount);
                QuestManager.Instance.UpdateQuest(QuestType.Spend_Gold, (amount + this.PlayerShopData.TimeBuy - 1) * this.PlayerShopData.CoinInc);
            }   
            if (this.Currency == CurrencyType.Gold)
            {
                UserData.Instance.data.Coin -= this.Price * amount;
                QuestManager.Instance.UpdateQuest(QuestType.Spend_Gold, (int)this.ItemShopData.Price * amount);
            }
            if (this.Currency == CurrencyType.Gin)
            {
                UserData.Instance.data.Gin -= this.Price * amount;
                QuestManager.Instance.UpdateQuest(QuestType.Spend_Gin, (int)this.ItemShopData.Price * amount);
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
