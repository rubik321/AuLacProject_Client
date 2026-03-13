using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NTFunctions_old;
using GOA.Item;
using System;
using Rubik.UI;

namespace GOA.Shop
{
    public class InforItemShopUI : PopupUI
    {
        public Transform Amount;
        public TextMeshProUGUI textCost;
        public TextMeshProUGUI textNumber;

        public Image image;
        public TextMeshProUGUI textName;
        public TextMeshProUGUI textDes;
        public CurrencyType Currency;
        public float Price;
        public bool StackBuy = false;

        public int num = 1;
        public int MaxAmount;
        public int TimeBuy;
        public int CoinInc;

        public void OnUI(Sprite icon, string name, string des, CurrencyType currency, float price, bool stackBuy, int timeBuy, int coinInc, Action<int> callback, int maxAmount = 99)
        {
            this.image.sprite = icon;
            this.EventBuy = callback;
            this.num = 1;
            this.textName.text = name;
            this.textDes.text = des;
            this.Currency = currency;
            this.Price = price;
            this.StackBuy = stackBuy;
            this.MaxAmount = maxAmount;
            this.TimeBuy = timeBuy;
            this.CoinInc = coinInc;
            this.UpdateData();
            this.Show();
        }

        public override void UpdateData()
        {
            base.UpdateData();
            if (this.Currency == CurrencyType.Gin)
            {
                this.textCost.text = this.num * this.Price + "<sprite index=1>";
            }
            else if (this.Currency == CurrencyType.Gold)
            {
                this.textCost.text = (this.num * this.Price + (this.num - 1 + this.TimeBuy) * this.CoinInc) + "<sprite index=0>";
            }
            if (this.StackBuy)
            {
                this.Amount.gameObject.SetActive(true);
            }
            else
            {
                this.Amount.gameObject.SetActive(false);
            }
            this.textNumber.text = this.num.ToString();
        }

        public void Add()
        {
            if (this.num >= this.MaxAmount) return;
            if (this.Currency == CurrencyType.Gin)
            {
                if ((this.num + 1) * this.Price  > UserData.UserData.Instance.data.Gin) return;
            }
            else if (this.Currency == CurrencyType.Gold)
            {
                if ((this.num + 1) * this.Price + (this.num + this.TimeBuy) * this.CoinInc > UserData.UserData.Instance.data.Coin) return;
            }
            this.num++;
            this.UpdateData();
        }

        public void Minus()
        {
            if (this.num <= 1) return;
            this.num--;
            this.UpdateData();
        }
        public void Max()
        {
            if (this.Currency == CurrencyType.Gin)
            {
                int amount = (int)(UserData.UserData.Instance.data.Gin / this.Price);
                if (amount > this.MaxAmount) amount = this.MaxAmount;
                this.num = amount;
            }
            else if (this.Currency == CurrencyType.Gold)
            {
                int amount = 1;
                for (amount = 1; amount < this.MaxAmount; amount++)
                {
                    if((amount + 1) * this.Price + (amount + this.TimeBuy) * this.CoinInc > UserData.UserData.Instance.data.Coin) break;
                }
                this.num = amount;
            }
            this.UpdateData();
        }

        public void Min()
        {
            this.num = 1;
            this.UpdateData();
        }

        public Action<int> EventBuy;

        public void Buy()
        {
            if (Currency == CurrencyType.Gold)
            {
                if (UserData.UserData.Instance.data.Coin < this.Price * this.num + (this.num -1 + this.TimeBuy) * this.CoinInc)
                {
                    HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("dont_enough_gold", "You have insufficient Gold."));
                    return;
                }
            }
            if (Currency == CurrencyType.Gin)
            {
                if (UserData.UserData.Instance.data.Gin < this.Price * this.num)
                {
                    HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("dont_enough_gin", "You have insufficient Gin."));
                    return;
                }
            }
            this.EventBuy.Invoke(num);
            this.OffUI();
        }
    }
}
