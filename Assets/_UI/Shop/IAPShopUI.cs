using DG.Tweening;
using NTPackage.Functions;
using NTPackage.UI;
using Rubik.CharacterGear;
using Rubik.Common.AudioHelper;
using Rubik.ItemPlayer;
using Rubik.UI;
using Rubik.UserDataPlayer;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rubik.Myrk.Shop
{
    public class IAPShopUI : PopupUI
    {
        public IAPShopUIItem IAPShopUIItemPrefab;
        public Transform IAPShopUIItemParent;
        public List<IAPShopUIItem> IAPShopUIItems;

        public IAPShopUIItem ItemSelected;

        public MultiTabUI MultiTabUI;

        public Transform DetailUI;
        public Image DetailIcon;
        public TextMeshProUGUI DetailName;
        public TextMeshProUGUI DetailDescription;
        public TextMeshProUGUI DetailPrice;
        public ItemDataBarUI DetailPriceItem;

        public NTButtonEffect BuyButton;

        public NTButtonEffect BtnMoreInfo;

        public override void OnUI(object data, bool isDefaultSound = true)
        {
            base.OnUI(data, isDefaultSound);
            this.MultiTabUI.OnUI();
        }

        public void MoveToTab(int index)
        {
            this.MultiTabUI.BtnTabOnclick(index);
        }

        public void OnTab(int index = 0)
        {
            AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
            this.ItemSelected = null;
            // Rubik.Common.Common.ResetContentY(this.IAPShopUIItemParent);
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.IAPShopUIItemParent);
            this.IAPShopUIItems.Clear();
            foreach (ItemShop item in ShopManager.Instance.IapShopData.ShopList)
            {
                if (item.Group == index && UserDataManager.Instance.IsRole(item.Role))
                {
                    IAPShopUIItem itemUI = ObjectPoolingManager.Instance.InstantiateObject<IAPShopUIItem>(ObjectPoolingConfig.IAP_Shop_Item, this.IAPShopUIItemPrefab.transform);
                    itemUI.transform.SetParent(this.IAPShopUIItemParent);
                    NTFunction.ResetPosition(itemUI.transform);
                    itemUI.SetData(item, this);
                    this.IAPShopUIItems.Add(itemUI);
                }
            }

            this.UpdateData();
        }

        public void UpdateData()
        {
            this.BtnMoreInfo.gameObject.SetActive(false);
            this.DetailUI.gameObject.SetActive(false);
            this.BuyButton.gameObject.SetActive(false);
            foreach (IAPShopUIItem itemUI in this.IAPShopUIItems)
            {
                if (this.ItemSelected == null) this.ItemSelected = itemUI;
                itemUI.UpdateData();
                if (this.ItemSelected != null && itemUI.ItemShop != null && itemUI.ItemShop.Index != null && itemUI.ItemShop.Index == this.ItemSelected.ItemShop.Index)
                {
                    this.DetailUI.gameObject.SetActive(true);
                    this.BuyButton.gameObject.SetActive(true);
                    this.DetailIcon.sprite = itemUI.Icon.sprite;
                    this.DetailName.text = itemUI.Title;
                    this.DetailDescription.text = itemUI.Description;
                    this.DetailPrice.gameObject.SetActive(itemUI.IAP_Price.gameObject.activeSelf);
                    if (itemUI.IAP_Price.gameObject.activeSelf)
                    {
                        this.DetailPrice.text = itemUI.IAP_Price.text;
                    }
                    this.DetailPriceItem.gameObject.SetActive(itemUI.IG_Price.gameObject.activeSelf);
                    if (itemUI.IG_Price.gameObject.activeSelf)
                    {
                        this.DetailPriceItem.SetData(itemUI.IG_Price.ItemData);
                        this.DetailPriceItem.Amount.color = itemUI.IG_Price.Amount.color;
                    }
                }
            }
            if (this.ItemSelected != null && this.ItemSelected.ItemShop != null && this.ItemSelected.ItemShop.OfferCharacterGearRarity != null && this.ItemSelected.ItemShop.OfferCharacterGearRarity.Length > 0)
            {
                this.BtnMoreInfo.gameObject.SetActive(true);
                this.BtnMoreInfo.Onclick.RemoveAllListeners();
                this.BtnMoreInfo.Onclick.AddListener(this._OnClickMoreGearInfo);
            }
        }

        public void OnDetail(IAPShopUIItem itemUI)
        {
            this.ItemSelected = itemUI;
            this.UpdateData();
        }

        public void OnBuy()
        {
            if (this.ItemSelected != null)
            {
                if (this.ItemSelected.ItemShop.ProductId == null || this.ItemSelected.ItemShop.ProductId == "")
                {
                    PopupManager.Instance.OnUI(PopupCode.MessageOptionPanel, null, (data) =>
                    {
                        MessageOptionPanel messageOptionPanel = data as MessageOptionPanel;
                        string title = Lean.Localization.LeanLocalization.GetTranslationText("shop_ask_title", "Purchase");
                        string content = Lean.Localization.LeanLocalization.GetTranslationText("shop_ask_detail", "Do you wish to purchase <b>{0}</b> for <b>{1}</b> ?");
                        string name = this.ItemSelected.Title;
                        string price = this.ItemSelected.ItemShop.PayItems[0].Amount.ToString() + " " + ItemDataManager.Instance.GetItemName(this.ItemSelected.ItemShop.PayItems[0].Type) + ItemDataManager.Instance.GetItemTextSprite(this.ItemSelected.ItemShop.PayItems[0].Type);
                        content = string.Format(content, name, price);
                        messageOptionPanel.SetData(title, content);
                        messageOptionPanel.SetActionConfirm(() =>
                        {
                            data.OffUI();
                            this.ItemSelected.OnBuy();
                        }, Lean.Localization.LeanLocalization.GetTranslationText("btn_agree", "Agree"));
                        messageOptionPanel.SetActionReject(() =>
                        {
                            data.OffUI();
                        }, Lean.Localization.LeanLocalization.GetTranslationText("btn_cancel", "Cancel"));
                    });
                }
                else
                {
                    this.ItemSelected.OnBuy();
                }
            }
        }

        public void _OnClickMoreGearInfo()
        {
            AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
            PopupManager.Instance.OnUI(PopupCode.GearShortInfoUI, null, (popupUI) =>
            {
                GearShortInfoUI gearShortInfoUI = popupUI as GearShortInfoUI;
                gearShortInfoUI.SetData(this.ItemSelected.ItemShop.OfferCharacterGearRarity[0].Index, this.ItemSelected.ItemShop.OfferCharacterGearRarity[0].Rarity, 0);
            });
        }

    }
}