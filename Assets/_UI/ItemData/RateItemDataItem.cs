using NTPackage.Functions;
using UnityEngine;
using TMPro;

namespace Rubik.ItemPlayer
{
    using Rubik.CardPlayer;
    using Rubik.ItemPlayer;
    public class RateItemDataItem : NTBehaviour
    {
        public ItemDataUI ItemData;
        public TextMeshProUGUI TextRate;

        public void SetData(ItemData itemData, float rate, bool showMulti = false, bool isShowToolTip = false)
        {
            ItemData.SetData(itemData, showMulti,isShowToolTip);
            TextRate.text = NTFunction.FormatLowerNumber(rate*100) + "% drop";
        }

        public void SetData(CardPlayerIndex cardPlayerIndex, float rate)
        {
            CardPlayer cardPlayer = new CardPlayer();
            cardPlayer.Index = cardPlayerIndex;
            cardPlayer.Star = 0;
            cardPlayer.Lv = 0;
            ItemData.SetData(cardPlayer);
            TextRate.text = NTFunction.FormatLowerNumber(rate*100) + "% drop";
        }
    }
}