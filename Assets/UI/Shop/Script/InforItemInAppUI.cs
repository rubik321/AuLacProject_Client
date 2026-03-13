using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NTFunctions_old;
using GOA.Item;
using Rubik.UI;

namespace GOA.Shop{
    public class InforItemInAppUI : PopupUI
    {
        public Image ava;
        public TextMeshProUGUI textCost;
        public Transform contentItemReward;

        public TextMeshProUGUI TetxContent;

        public GOA.Item.ItemInfoData ItemData;

        public void OnUI(GOA.Item.ItemInfoData itemData ){
            this.ItemData = itemData;
            string iconSprite ="";
            switch (this.ItemData.Currency)
            {
                case CurrencyType.Gold:
                    iconSprite = "<sprite=0>";
                    break;
                case CurrencyType.Gin:
                    iconSprite = "<sprite=1>";
                    break;
            }
            this.TetxContent.text =  string.Format(
                Lean.Localization.LeanLocalization.GetTranslationText("confirm_buy_shop", "Do you want to buy {0} for {1}{2}?"), 
                Lean.Localization.LeanLocalization.GetTranslationText(this.ItemData.Index + "_Name", "Item"),
                this.ItemData.Price,
                iconSprite
            );
            if(!this.CanShow()) return;
            this.Show();
        }

        public void Buy(){
            Debug.LogWarning("Buy");
            this.Hide();
            APIManager.Instance.BuyItemInShop(UserData.UserData.Instance.data.UserId, (int)ItemData.Code, this.BuySuccess);
        }

        public void BuySuccess(){
            UserData.UserData.Instance.Inventory.AddInventoryByCode(this.ItemData.Code, 1);
            if(this.ItemData.Currency == CurrencyType.Gold){
                UserData.UserData.Instance.data.Coin -= this.ItemData.Price;
                QuestManager.Instance.UpdateQuest(QuestType.Spend_Gold, (int)this.ItemData.Price);
            }
            if(this.ItemData.Currency == CurrencyType.Gin){
                UserData.UserData.Instance.data.Gin -= this.ItemData.Price;
                QuestManager.Instance.UpdateQuest(QuestType.Spend_Gin, (int)this.ItemData.Price);
            }
            HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("buy_success", "Buy success"));
        }
    }
}
