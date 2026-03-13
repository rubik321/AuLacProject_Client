using NTPackage.Functions;
using Rubik.CardPlayer;
using TMPro;
using UnityEngine;

namespace Rubik.ItemPlayer
{
    public class RateItemDataElement : MonoBehaviour
    {
        public TextMeshProUGUI TextTitle;
        public RateItemDataItem RateItemDataItemPrefab;
        public Transform Holder;

        public void SetData(ListItemRate listItemRate, string title, bool isShowToolTip = false)
        {
            TextTitle.text = title;
            float eachRate = 0;
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(Holder);
            if (listItemRate.Types != null && listItemRate.Types.Length > 0)
            {
                eachRate = listItemRate.Rate / listItemRate.Types.Length;
                foreach (ItemType item in listItemRate.Types)
                {
                    RateItemDataItem itemData = ObjectPoolingManager.Instance.InstantiateObject<RateItemDataItem>(ObjectPoolingConfig.RateItemDataItem, RateItemDataItemPrefab.transform);
                    itemData.SetData(new ItemData(item, listItemRate.Amount), eachRate, isShowToolTip);
                    itemData.transform.SetParent(Holder);
                    NTFunction.ResetPosition(itemData.transform);
                }
            }
            else if (listItemRate.CardPlayerIndexes != null && listItemRate.CardPlayerIndexes.Length > 0)
            {
                eachRate = listItemRate.Rate / listItemRate.CardPlayerIndexes.Length;
                foreach (CardPlayerIndex cardPlayerIndex in listItemRate.CardPlayerIndexes)
                {
                    RateItemDataItem itemData = ObjectPoolingManager.Instance.InstantiateObject<RateItemDataItem>(ObjectPoolingConfig.RateItemDataItem, RateItemDataItemPrefab.transform);
                    itemData.SetData(cardPlayerIndex, eachRate);
                    itemData.transform.SetParent(Holder);
                    NTFunction.ResetPosition(itemData.transform);
                }
            }
            else
            {
                eachRate = listItemRate.Rate;
            }

        }

        public void Clear()
        {
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(Holder);
        }
    }
}