using System.Collections;
using System.Collections.Generic;
using NTPackage.Functions;
using NTPackage.UI;
using Rubik.Format;
using Rubik.UI;
using Rubik.UserDataPlayer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rubik.ItemPlayer
{
    public class ItemDataBarUI : NTBehaviour
    {
        public Image Icon;
        public TextMeshProUGUI Amount;
        public ItemData ItemData;
        public NTButtonEffect OnclickToolTip;
        public void SetData(ItemData itemData, bool isFormatNumber = false, bool isShowToolTip = false){
            this.ItemData = itemData;
            Sprite sprite = ItemDataManager.Instance.GetIcon(itemData.Type);
            Icon.sprite = sprite;
            if (isFormatNumber)
            {
                Amount.text = FormatData.GetFriendlyShortNumber(itemData.Amount);
            }
            else
            {
                Amount.text = itemData.Amount.ToString();
            }
            this.OnclickToolTip.gameObject.SetActive(isShowToolTip);
        }

        public void AddPlus(){
            this.Amount.text = "+"+this.Amount.text;
        }

        public void AddMinus(){
            this.Amount.text = "-" + this.Amount.text;
        }

        public void OnToolTip(){
            ItemDataManager.Instance.ShowToolTip(this.ItemData.Type);
        }
    }
}

