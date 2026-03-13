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

    public class ClanShopUIItem : NTButtonEffect
    {
        public ItemShop ItemShop;
        public ClanShopUI ClanShopUI;
        public Image Icon;
        public TextMeshProUGUI Amount;
        public ItemDataBarUI IG_Price;

        public Transform SoldOut;

        public string Title;
        public string Description;

        public bool IsEnough = true;

        public void SetData(ItemShop itemShop, ClanShopUI clanShopUI)
        {
            this.ClanShopUI = clanShopUI;
            this.ItemShop = itemShop;
            this.UpdateData();
        }

        public void UpdateData(){
            this.SoldOut.gameObject.SetActive(false);
            this.IsEnough = true;
            this.Title = ShopManager.Instance.GetShopItemTitle(this.ItemShop.Index);
            this.Description = ShopManager.Instance.GetShopItemDescription(this.ItemShop.Index);
            Sprite sprite = ShopManager.Instance.GetShopItemIcon(this.ItemShop.Index);
            if(sprite == null){
                if(this.ItemShop.OfferItems != null && this.ItemShop.OfferItems.Length > 0){
                    sprite = ItemDataManager.Instance.GetIcon(this.ItemShop.OfferItems[0].Type);
                    this.Title = ItemDataManager.Instance.GetItemName(this.ItemShop.OfferItems[0].Type);
                    this.Description = ItemDataManager.Instance.GetItemDescription(this.ItemShop.OfferItems[0].Type);
                }
                if(this.ItemShop.OfferCharacterGearRarity != null && this.ItemShop.OfferCharacterGearRarity.Length > 0){
                    sprite = CharacterGearManager.Instance.GetGearSpriteByIndex(this.ItemShop.OfferCharacterGearRarity[0].Index);
                    this.Title = CharacterGearManager.Instance.GetGearNameByIndex(this.ItemShop.OfferCharacterGearRarity[0].Index);
                    this.Description = CharacterGearManager.Instance.GetGearDesByIndex(this.ItemShop.OfferCharacterGearRarity[0].Index);
                }
            }
            this.Icon.sprite = sprite;
            this.Amount.text = "x"+NTFunction.FormatNumber(this.ItemShop.OfferItems[0].Amount);

            if(this.ItemShop.PayItems.Length > 0){
                this.IG_Price.gameObject.SetActive(true);
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

            if(this.ClanShopUI.ItemSelected != null && this.ClanShopUI.ItemSelected.ItemShop != null && this.ClanShopUI.ItemSelected.ItemShop.Index != null && this.ClanShopUI.ItemSelected.ItemShop.Index == this.ItemShop.Index){
                this.Chose();
            }else{
                this.Unchose();
            }

            if(this.IsSoldOut()){
                this.SoldOut.gameObject.SetActive(true);
            }else{
                this.SoldOut.gameObject.SetActive(false);
            }
        }

        public void _OnClick()
        {
            AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
            this.ClanShopUI.OnDetail(this);
        }

        public void OnBuy(){
            AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
            if (UserDataManager.Instance.IsCapSlotInventoryBag())
            {
                UserDataManager.Instance.ShowNotificationCapSlotInventoryBag();
                return;
            }
            if (this.IsEnough){
                ShopManager.Instance.BuyClanShop(this.ItemShop.Index, this.OnPurchaseSuccess);
            }else{
                ItemDataManager.Instance.ShowDontEnoughItem(this.ItemShop.PayItems[0].Type);
            }
        }

        public void OnPurchaseSuccess()
        {
            this.ClanShopUI.UpdateData();
        }

        public bool IsSoldOut(){
            int amount = ShopManager.Instance.GetAmountBought(this.ItemShop.Index);
            if(this.ItemShop.LimitType == LimitType.None){
                return false;
            }
            if(amount >= this.ItemShop.Limit){
                return true;
            }else{
                return false;
            }
        }
    }
}