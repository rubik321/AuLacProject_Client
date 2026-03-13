using NTPackage.UI;
using Rubik.Common.AudioHelper;
using Rubik.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Rubik.ItemPlayer
{
    public class ExchangeItemUI : PopupUI
    {
        public ItemDataUI ItemRequire;
        public ItemDataUI ItemResult;
        public long Amount;
        public ItemType ItemType;

        public NTButtonEffect AgreeBtn;
        public TextMeshProUGUI Cap;
        public NTButtonEffect AgreeAdvBtn;
        public TextMeshProUGUI CapAdv;
        public TextMeshProUGUI TextBtnAdv;
        public TextMeshProUGUI TextAsk;

        public override void OnUI(object data = null, bool isDefaultSound = true)
        {
            base.OnUI(data, isDefaultSound);
        }

        public override void UpdateData(object data = null)
        {
            base.UpdateData(data);
            if (ItemDataManager.Instance.IsExchangeAdv(this.ItemType))
            {
                ExchangeItemAdv exchangeItemAdv = ItemDataManager.Instance.GetExchangeItemAdv(this.ItemType);
                this.TextBtnAdv.text = "+" + exchangeItemAdv.Result.Amount.ToString() + ItemDataManager.Instance.GetItemTextSprite(exchangeItemAdv.Result.Type);
                if (LevelPlayAds.Instance.IsCanShowAds())
                {
                    this.AgreeAdvBtn.gameObject.SetActive(true);
                }
                else
                {
                    this.AgreeAdvBtn.gameObject.SetActive(false);
                }
                long remainDailyExchangeAdv = ItemDataManager.Instance.GetRemainDailyExchangeAdv(this.ItemType);
                this.CapAdv.text = Lean.Localization.LeanLocalization.GetTranslationText("remains", "Remains: ") + remainDailyExchangeAdv;
                if(remainDailyExchangeAdv > 0){
                    this.AgreeAdvBtn.Chose();
                }
                else
                {
                    this.AgreeAdvBtn.Unchose();
                }
            }
            else
            {
                this.AgreeAdvBtn.gameObject.SetActive(false);
                this.CapAdv.text = "";
            }

            long remainDailyExchange = ItemDataManager.Instance.GetRemainDailyExchange(this.ItemType);
            this.Cap.text = Lean.Localization.LeanLocalization.GetTranslationText("remains", "Remains: ") + remainDailyExchange;
            if(remainDailyExchange > 0){
                this.AgreeBtn.Chose();
            }
            else
            {
                this.AgreeBtn.Unchose();
            }
        }

        public void SetData(ItemType itemType)
        {
            ExchangeItem exchangeItem = ItemDataManager.Instance.GetExchangeItem(itemType);
            if (exchangeItem == null) return;
            Amount = 1;
            ItemData itemRequire = new ItemData(exchangeItem.ExchangeRequire.Type, exchangeItem.ExchangeRequire.Amount * Amount);
            ItemData itemResult = new ItemData(exchangeItem.ExchangeResult.Type, ItemDataManager.Instance.GetAmountExchange(itemType) * Amount);
            ItemRequire.SetData(itemRequire);
            ItemResult.SetData(itemResult);
            string str = Lean.Localization.LeanLocalization.GetTranslationText("exchange_item_ask", "Will you offer {0} to the fates in exchange for {1}'s blessing?");
            this.TextAsk.text = string.Format(str,
                itemRequire.Amount + " " + ItemDataManager.Instance.GetItemTextSprite(itemRequire.Type) + " " + ItemDataManager.Instance.GetItemName(itemRequire.Type),
                itemResult.Amount + " " + ItemDataManager.Instance.GetItemTextSprite(itemResult.Type) + " " + ItemDataManager.Instance.GetItemName(itemResult.Type)
            );
            this.ItemType = itemType;
            this.UpdateData();
        }

        public void _OnAgree()
        {
            AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
            if (ItemDataManager.Instance.GetItem(this.ItemRequire.ItemData.Type).Amount < this.ItemRequire.ItemData.Amount)
            {
                ItemDataManager.Instance.ShowDontEnoughItem(this.ItemRequire.ItemData.Type);
                return;
            }
            if (ItemDataManager.Instance.IsCapExchange(this.ItemType))
            {
                HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("exchange_reach_cap", "You have reached the daily exchange limit!"));
                return;
            }
            ItemDataManager.Instance.ExchangeItem(this.ItemType, (int)this.Amount, () =>
            {
                this.UpdateData();
            });
        }

        public void _OnAgreeAdv()
        {
            AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
            if (ItemDataManager.Instance.IsCapExchangeAdv(this.ItemType))
            {
                HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("exchange_reach_cap", "You have reached the daily exchange limit!"));
                return;
            }
            LevelPlayAds.Instance.OnShowReward(() =>
            {
                AppsFlyerManager.TrackingAds("Exchange_item");
                StartCoroutine(ItemDataManager.Instance.IEExchangeItemAdv(this.ItemType, () =>
                {
                    this.UpdateData();
                }));
            });
        }
    }
}