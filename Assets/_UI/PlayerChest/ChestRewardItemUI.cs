using NTPackage.Functions;
using NTPackage.UI;
using Rubik.ItemPlayer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rubik.Myrk.PlayerChest
{
    using ItemPlayer;
    using NTPackage.Functions;
    using TMPro;
    using System.Collections.Generic;
    public class ChestRewardItemUI : NTBehaviour
    {
        public ItemDataUI ItemDataUI;
        public TextMeshProUGUI TextRate;
        public Transform OnclickTooltip;
        public List<ListItemRate> ListItemRate;


        public void SetData(ItemData itemData){
            this.ItemDataUI.SetData(itemData, true, true);
            this.TextRate.text = "";
            this.OnclickTooltip.gameObject.SetActive(false);
        }

        public void SetData(ItemRate itemRate)
        {
            ItemData itemData = new ItemData(itemRate.Type, itemRate.Amount);
            this.ItemDataUI.SetData(itemData, true, true);
            this.TextRate.text = NTFunction.FormatLowerNumber(itemRate.Rate * 100) + "% drop";
            this.OnclickTooltip.gameObject.SetActive(false);
        }

        public void SetData(ItemRange itemRange){
            ItemData itemData = new ItemData(itemRange.Type, itemRange.Min);
            this.ItemDataUI.SetData(itemData, true, true);
            this.ItemDataUI.AmountItem.text = "x" + NTFunction.FormatLowerNumber(itemRange.Min) + " - " + NTFunction.FormatLowerNumber(itemRange.Max);
            this.TextRate.text = "";
            this.OnclickTooltip.gameObject.SetActive(false);
        }
        
        public void SetData(List<ListItemRate> listItemRates){
            this.ListItemRate = new List<ListItemRate>();
            foreach (ListItemRate itemRate in listItemRates){
                if(itemRate.Rate > 0.000001){
                    ListItemRate listItemRate = new ListItemRate();
                    listItemRate.Rate = itemRate.Rate;
                    listItemRate.Amount = (int)PlayerChestManager.Instance.GetAmountRewardByLevel(itemRate.Amount);
                    listItemRate.Types = itemRate.Types;
                    this.ListItemRate.Add(listItemRate);
                }
            }
            float rate = 0;
            foreach (ListItemRate itemRate in this.ListItemRate){
                rate += itemRate.Rate;
            }
            this.ItemDataUI.SetData(new ItemData(ItemType.SampleGift_Red, 1), true, false);
            this.TextRate.text = NTFunction.FormatLowerNumber(rate * 100) + "% drop";
            this.OnclickTooltip.gameObject.SetActive(true);
        }

        public void OnClickTooltip(){
            PopupManager.Instance.OnUI(PopupCode.RateItemDataUI, null, (popup) => {
                RateItemDataUI rateItemDataUI = popup as RateItemDataUI;

                List<string> titles = new List<string>();
                foreach (ListItemRate itemRate in this.ListItemRate)
                {
                    string str = Lean.Localization.LeanLocalization.GetTranslationText("shard_monster", "Monster shard");
                    str += " x" + NTFunction.FormatNumber(itemRate.Amount);
                    str += " (" + NTFunction.FormatLowerNumber(itemRate.Rate * 100) + "%)";
                    titles.Add(str);
                }
                rateItemDataUI.SetData(this.ListItemRate, titles, Lean.Localization.LeanLocalization.GetTranslationText("chest_reward", "Chest reward"));
            });
        }
    }
}