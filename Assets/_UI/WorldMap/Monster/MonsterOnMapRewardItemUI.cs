using System.Collections;
using System.Collections.Generic;
using Rubik.ItemPlayer;
using UnityEngine;

namespace Rubik.Myrk.Monster
{
    using ItemPlayer;
    using NTPackage.Functions;
    using TMPro;

    public class MonsterOnMapRewardItemUI : MonoBehaviour
    {
        public ItemDataUI ItemDataUI;
        public TextMeshProUGUI TextRate;

        public void SetData(ItemRate itemRate){
            ItemData itemData = new ItemData(itemRate.Type, itemRate.Amount);
            this.ItemDataUI.SetData(itemData, true, true);
            this.TextRate.text = NTFunction.FormatLowerNumber(itemRate.Rate*100) + "% drop";
        }

        public void SetData(ItemData itemData){
            this.ItemDataUI.SetData(itemData, true, true);
            this.TextRate.text = "";
        }
    }
}
