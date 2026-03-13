using System.Collections;
using System.Collections.Generic;
using Lean.Localization;
using NTPackage.Functions;
using NTPackage.UI;
using Rubik.ItemPlayer;
using Rubik.Manager;
using Rubik.UserDataPlayer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rubik.Myrk.Shop
{
    public class ClanShopUI : PopupUI
    {
        public ClanShopUIItem ClanShopUIItemPrefab;
        public Transform ClanShopUIItemParent;
        public List<ClanShopUIItem> ClanShopUIItems;

        public ClanShopUIItem ItemSelected;

        public Transform DetailUI;
        public Image DetailIcon;
        public TextMeshProUGUI DetailName;
        public TextMeshProUGUI DetailDescription;
        public ItemDataBarUI DetailPriceItem;


        public TextMeshProUGUI TimeLeft;
        public Coroutine CorTimeLeft;
        public long countDown;

        public NTButtonEffect BuyButton;

        [NTButton]
        public void TestOnUI()
        {
            this.OnUI(null);
        }

        public override void OnUI(object data, bool isDefaultSound = true)
        {
            this.ItemSelected = null;
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.ClanShopUIItemParent);
            this.ClanShopUIItems.Clear();
            foreach (ItemShop item in ShopManager.Instance.ClanShop.ShopList)
            {
                if (UserDataManager.Instance.IsRole(item.Role))
                {
                    ClanShopUIItem itemUI = ObjectPoolingManager.Instance.InstantiateObject<ClanShopUIItem>(ObjectPoolingConfig.Clan_Shop_Item, this.ClanShopUIItemPrefab.transform);
                    itemUI.transform.SetParent(this.ClanShopUIItemParent);
                    NTFunction.ResetPosition(itemUI.transform);
                    itemUI.SetData(item, this);
                    this.ClanShopUIItems.Add(itemUI);
                }
            }
            base.OnUI(data, isDefaultSound);
        }

        public override void ScriptOffUI()
        {
            base.ScriptOffUI();
            if (this.CorTimeLeft != null)
            {
                StopCoroutine(this.CorTimeLeft);
            }
        }

        public override void UpdateData(object data = null)
        {
            base.UpdateData(data);

            this.DetailUI.gameObject.SetActive(false);
            this.BuyButton.gameObject.SetActive(false);
            foreach (ClanShopUIItem itemUI in this.ClanShopUIItems)
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
                    this.DetailPriceItem.SetData(itemUI.IG_Price.ItemData);
                    this.DetailPriceItem.Amount.color = itemUI.IG_Price.Amount.color;
                }
            }

            if (this.CorTimeLeft != null)
            {
                StopCoroutine(this.CorTimeLeft);
            }
            if (this.ItemSelected == null)
            {
                this.TimeLeft.text = "";
            }
            else
            {
                if (this.ItemSelected.ItemShop.LimitType != LimitType.None)
                {
                    int amount = ShopManager.Instance.GetAmountBought(this.ItemSelected.ItemShop.Index);
                    int max = this.ItemSelected.ItemShop.Limit;
                    this.DetailName.text += " (" + amount + "/" + max + ")";
                }
                if (!this.ItemSelected.IsSoldOut())
                {
                    this.BuyButton.gameObject.SetActive(true);
                    this.TimeLeft.text = "";
                }
                else
                {
                    if (this.ItemSelected.ItemShop.LimitType == LimitType.Daily)
                    {
                        this.countDown = ServerManager.Instance.GetNextTimeNewDay();
                        this.CorTimeLeft = StartCoroutine(this.CotimeLeft());
                    }
                    if (this.ItemSelected.ItemShop.LimitType == LimitType.Weekly)
                    {
                        this.countDown = ServerManager.Instance.GetTimeNewWeek();
                        this.CorTimeLeft = StartCoroutine(this.CotimeLeft());
                    }
                    this.BuyButton.gameObject.SetActive(false);
                }

            }
        }

        public void OnDetail(ClanShopUIItem itemUI)
        {
            this.ItemSelected = itemUI;
            this.UpdateData();
        }

        public void OnBuy()
        {
            if (this.ItemSelected != null)
            {
                this.ItemSelected.OnBuy();
            }
        }

        IEnumerator CotimeLeft()
        {
            while (true)
            {
                if (this.ItemSelected.ItemShop.LimitType == LimitType.Daily)
                {
                    this.countDown = ServerManager.Instance.GetNextTimeNewDay();
                }
                if (this.ItemSelected.ItemShop.LimitType == LimitType.Weekly)
                {
                    this.countDown = ServerManager.Instance.GetTimeNewWeek();
                }
                TimeLeft.text = LeanLocalization.GetTranslationText("reset_in", "Reset in: ") + " <color=#BD7E92>" + NTFunction.Format_Time(this.countDown, 2) + "</color>";
                yield return new WaitForSeconds(1);
                if (this.countDown < 0)
                {
                    this.TimeLeft.text = "";
                    break;
                }
            }
        }
    }
}