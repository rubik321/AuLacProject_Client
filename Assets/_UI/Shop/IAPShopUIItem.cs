using NTPackage.EventDispatcher;
using NTPackage.UI;
using Rubik.CharacterGear;
using Rubik.IAP;
using Rubik.ItemPlayer;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rubik.Myrk.Shop
{
    using NTPackage.Functions;
    using Rubik.Common.AudioHelper;
    using Rubik.ItemPlayer;
    using Rubik.UI;
    using Rubik.UserDataPlayer;
    using System;

    public class IAPShopUIItem : NTButtonEffect
    {
        public ItemShop ItemShop;
        public IAPShopUI IAPShopUI;
        public Image Icon;

        public TextMeshProUGUI IAP_Price;
        public ItemDataBarUI IG_Price;
        public TextMeshProUGUI Amount;

        public string Title;
        public string Description;

        public bool IsEnough = true;

        public void SetData(ItemShop itemShop, IAPShopUI iapShopUI)
        {
            this.IAPShopUI = iapShopUI;
            this.ItemShop = itemShop;
            this.UpdateData();
        }

        public void UpdateData(){
            this.IsEnough = true;
            this.Title = ShopManager.Instance.GetShopItemTitle(this.ItemShop.Index);
            this.Description = ShopManager.Instance.GetShopItemDescription(this.ItemShop.Index);
            this.Amount.text = "";
            Sprite sprite = ShopManager.Instance.GetShopItemIcon(this.ItemShop.Index);
            if(sprite == null){
                if(this.ItemShop.OfferItems != null && this.ItemShop.OfferItems.Length > 0){
                    sprite = ItemDataManager.Instance.GetIcon(this.ItemShop.OfferItems[0].Type);
                    this.Title = ItemDataManager.Instance.GetItemName(this.ItemShop.OfferItems[0].Type);
                    this.Description = ItemDataManager.Instance.GetItemDescription(this.ItemShop.OfferItems[0].Type);
                    this.Amount.text = "x"+NTFunction.FormatNumber(this.ItemShop.OfferItems[0].Amount);
                }
                if(this.ItemShop.OfferCharacterGearRarity != null && this.ItemShop.OfferCharacterGearRarity.Length > 0){
                    sprite = CharacterGearManager.Instance.GetGearSpriteByIndex(this.ItemShop.OfferCharacterGearRarity[0].Index);
                    this.Title = CharacterGearManager.Instance.GetGearNameByIndex(this.ItemShop.OfferCharacterGearRarity[0].Index);
                    this.Description = CharacterGearManager.Instance.GetGearDesByIndex(this.ItemShop.OfferCharacterGearRarity[0].Index);
                    this.Amount.text = "Lv.1";
                }
            }
            this.Icon.sprite = sprite;

            if(this.ItemShop.ProductId != null && this.ItemShop.ProductId != ""){
                this.IAP_Price.gameObject.SetActive(true);
                this.IG_Price.gameObject.SetActive(false);
                this.IAP_Price.text = IAPManager.Instance.getPriceProduct(this.ItemShop.ProductId);
            }
            else if(this.ItemShop.PayItems.Length > 0){
                this.IG_Price.gameObject.SetActive(true);
                this.IAP_Price.gameObject.SetActive(false);
                this.IG_Price.SetData(this.ItemShop.PayItems[0], true);
                this.IG_Price.Amount.color = NTFunction.StringHexToColor("705748");
                foreach (ItemData item in this.ItemShop.PayItems)
                {
                    if(ItemDataManager.Instance.GetItem(item.Type).Amount < item.Amount){
                        this.IG_Price.Amount.color = Color.red;
                        this.IsEnough = false;
                    }
                }
            }

            if(this.IAPShopUI.ItemSelected != null && this.IAPShopUI.ItemSelected.ItemShop != null && this.IAPShopUI.ItemSelected.ItemShop.Index != null && this.IAPShopUI.ItemSelected.ItemShop.Index == this.ItemShop.Index){
                this.Chose();
            }else{
                this.Unchose();
            }
        }

        public void _OnClick()
        {
            AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
            this.IAPShopUI.OnDetail(this);
        }

        public void OnBuy(){
            AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
            if (UserDataManager.Instance.IsCapSlotInventoryBag())
            {
                UserDataManager.Instance.ShowNotificationCapSlotInventoryBag();
                return;
            }
            if (this.IsEnough){
              
                ShopManager.Instance.Buy(this.ItemShop.ProductId, this.ItemShop.Index, this.ItemShop.PayItems, this.OnPurchaseSuccess);
            }else{
                ItemDataManager.Instance.ShowDontEnoughItem(this.ItemShop.PayItems[0].Type);
            }
        }

        public void OnPurchaseSuccess(bool data)
        {
            if(data){
                // TODO: Handle purchase success
            }else{
                // TODO: Handle purchase failed
            }
        }

    }
}