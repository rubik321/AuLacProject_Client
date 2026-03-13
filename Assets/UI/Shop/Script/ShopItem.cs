using System.Collections;
using System.Collections.Generic;
using NTPackage.UI;
using Rubik.Myrk.Shop;
using UnityEngine.UI;
using TMPro;
using UnityEngine;


namespace Rubik.Minigame.Restaurant
{
    using global::GOA.Shop;
    using Rubik.CharacterCloth;
    using Rubik.CharacterGear;
    using Rubik.ItemPlayer;
    using Spine.Unity;
    public class ShopItem : NTButtonEffect
    {
        public ItemShop ItemShop;
        public ChracterGearRarity ItemDataUI;
      
        public TextMeshProUGUI InfoText;
       
        public Image Icon, bgIgm;
        public GameObject hightlightGO,tickGo,priceGo;
        public CharacterGearType typeGear;
        public SkeletonGraphic playerAnim;
        public string animName;
        public Sprite sprOn, sprOff;
        public ItemShop itemShop;
        public string gearIndex;

        public TextMeshProUGUI PriceText;

        public void SetData(ItemShop itemShop)
        {
            this.ItemShop = itemShop;
            gearIndex = itemShop.Index;
            this.UpdateData();
            this.Onclick.RemoveAllListeners();
            this.Onclick.AddListener(this._OnClick);
            this.Onhold.RemoveAllListeners();
            this.Onhold.AddListener(this._OnHover);
           // this.PriceText.text = IAPManager.Instance.getPriceProduct(this.ItemShop.IAP_ID);
        }

        public void UpdateData()
        {
            if (this.ItemShop.OfferCharacterGearRarity.Length > 0)
            {
               // this.ItemDataUI.SetData(this.ItemShop.OfferCharacterGearRand[0]);
               // this.ItemDataInfo = ItemDataManager.Instance.GetItemDataInfo(this.ItemShop.OfferCharacterGearRand[0].Type);
            }
           
            var temp = CharacterGearManager.Instance.GetGearDataByIndex(ItemShop.OfferCharacterGearRarity[0].Index);
            Debug.Log(temp.Type);
            typeGear = temp.Type;
            //else if (this.ItemShop.OfferCharacterGearRand.Length > 0)
            //{
            //    //this.ItemDataUI.SetClothData(this.ItemShop.OfferCharacterGearRand[0]);
            //  //  this.ItemDataInfo = ItemDataManager.Instance.GetItemDataInfo(this.ItemShop.OfferCharacterCloth[0].Index);
            //}

            //this.InfoText.text = "";
            //if (this.ItemDataInfo.Energy > 0)
            //{
            //    this.InfoText.text += "+" + this.ItemDataInfo.Energy + " SP";
            //}
           // this.ItemPriceBarUI.SetData(this.ItemShop.PayItems[0]);
        }
        public void OffUI()
        {
            bgIgm.sprite = sprOff;
           // hightlightGO.SetActive(false);
        }
        public void _OnClick()
        {
          
            GetComponentInParent<ShopUI>().OnButtonOffUI(gearIndex,this);
            bgIgm.sprite = sprOn;
            //hightlightGO.SetActive(true);
            //if (this.ItemShop.OfferItems.Length > 0)
            //{
            //    if (this.ItemShop.PayItems[0].Amount > ItemDataManager.Instance.GetItem(this.ItemShop.PayItems[0].Type).Amount)
            //    {
            //        HUDCanvas.Instance.ShowNotification("You don't have enough " + ItemDataManager.Instance.GetItemName(this.ItemShop.PayItems[0].Type) + "!");
            //        return;
            //    }
            //    PopupManager.Instance.OnUI(PopupCode.MessageOptionPanel, null, (popup) =>
            //    {
            //        MessageOptionPanel messageOptionPanel = popup as MessageOptionPanel;
            //        if (this.ItemShop.OfferItems.Length > 0)
            //        {
            //            messageOptionPanel.SetData("Restaurant", "Are you sure you want to buy x" + this.ItemShop.OfferItems[0].Amount + " " + ItemDataManager.Instance.GetItemName(this.ItemShop.OfferItems[0].Type)
            //       + " for " + this.ItemShop.PayItems[0].Amount + " " + ItemDataManager.Instance.GetItemName(this.ItemShop.PayItems[0].Type) + "?");
            //            messageOptionPanel.SetItemData(ItemDataManager.Instance.GetItem(this.ItemShop.PayItems[0].Type));
            //        }
            //        else
            //        {

            //            //     messageOptionPanel.SetData("Restaurant", "Are you sure you want to buy x" + this.ItemShop.OfferCharacterCloth[0].Index + " " + ItemDataManager.Instance.GetItemName(this.ItemShop.OfferCharacterCloth[0].Type)
            //            //+ " for " + this.ItemShop.PayItems[0].Amount + " " + ItemDataManager.Instance.GetItemName(this.ItemShop.PayItems[0].Type) + "?");
            //            messageOptionPanel.SetItemData(ItemDataManager.Instance.GetItem(this.ItemShop.PayItems[0].Type));
            //        }
            //        messageOptionPanel.SetActionConfirm(() =>
            //        {
            //            StartCoroutine(ShopManager.Instance.Buy(this.ItemShop.Index, () =>
            //            {
            //                popup.OffUI();
            //            }));
            //        }, "Buy");
            //        messageOptionPanel.SetActionReject(() =>
            //        {
            //            popup.OffUI();
            //        }, "No");


            //    });
            //}
            //else if (this.ItemShop.OfferCharacterCloth.Length > 0)
            //{
            //    var clothShop = PopupManager.Instance.GetPopupUIByCode(PopupCode.RestaurantSellFoodUI).GetComponent<RestaurantSellFoodUI>();
            //   var checkExist= CharacterClothManager.Instance.CheackClothIsExist(this.ItemShop.OfferCharacterCloth[0].Index);
            //    clothShop.OnChangeCloth( ItemShop.OfferCharacterCloth[0].Index);
            //    clothShop.BuyCloth.gameObject.SetActive(true);

            //    if (checkExist)
            //    {
            //        clothShop.BuyCloth.onClick.RemoveAllListeners();
            //        clothShop.BuyCloth.GetComponentInChildren<TextMeshProUGUI>().text = "Owned";
            //        return;
            //    }
            //    else
            //    {
            //        clothShop.BuyCloth.GetComponentInChildren<TextMeshProUGUI>().text = "Buy";
            //    }

            //    clothShop.BuyCloth.onClick.RemoveAllListeners();
            //    clothShop.BuyCloth.onClick.AddListener(() =>
            //    {
            //        if (this.ItemShop.PayItems[0].Amount > ItemDataManager.Instance.GetItem(this.ItemShop.PayItems[0].Type).Amount)
            //        {
            //            HUDCanvas.Instance.ShowNotification("You don't have enough " + ItemDataManager.Instance.GetItemName(this.ItemShop.PayItems[0].Type) + "!");
            //            return;
            //        }
            //        PopupManager.Instance.OnUI(PopupCode.MessageOptionPanel, null, (popup) =>
            //        {
            //            MessageOptionPanel messageOptionPanel = popup as MessageOptionPanel;
            //            messageOptionPanel.SetData("Cloth", "Are you sure you want to buy this ?");
            //            //if (this.ItemShop.OfferItems.Length > 0)
            //            //{
            //            //    messageOptionPanel.SetData("Cloth", "Are you sure you want to buy this ?" );
            //            //   // messageOptionPanel.SetItemData(ItemDataManager.Instance.GetItem(this.ItemShop.PayItems[0].Type));
            //            //}
            //            //else
            //            //{

            //            //    //     messageOptionPanel.SetData("Restaurant", "Are you sure you want to buy x" + this.ItemShop.OfferCharacterCloth[0].Index + " " + ItemDataManager.Instance.GetItemName(this.ItemShop.OfferCharacterCloth[0].Type)
            //            //    //+ " for " + this.ItemShop.PayItems[0].Amount + " " + ItemDataManager.Instance.GetItemName(this.ItemShop.PayItems[0].Type) + "?");
            //            //   // messageOptionPanel.SetItemData(ItemDataManager.Instance.GetItem(this.ItemShop.PayItems[0].Type));
            //            //}
            //            messageOptionPanel.SetActionConfirm(() =>
            //            {
            //                StartCoroutine(ShopManager.Instance.Buy(this.ItemShop.Index, () =>
            //                {
            //                    popup.OffUI();
            //                    clothShop.BuyCloth.onClick.RemoveAllListeners();
            //                    clothShop.BuyCloth.GetComponentInChildren<TextMeshProUGUI>().text = "Owned";
            //                }));
            //            }, "Buy");
            //            messageOptionPanel.SetActionReject(() =>
            //            {
            //                popup.OffUI();
            //            }, "No");


            //        });
            //    });
            //}

        }

        public void _OnHover()
        {
            ItemData itemData = new ItemData();
            itemData.Type = this.ItemShop.OfferItems[0].Type;
            itemData.Amount = 0;
            ItemDataManager.Instance.ShowInfo(itemData);
        }
    }
}