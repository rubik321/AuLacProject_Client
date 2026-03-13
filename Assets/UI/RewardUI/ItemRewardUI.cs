using System.Collections;
using System.Collections.Generic;
using GOA.Item;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GOA.Reward
{
    public class ItemRewardUI : MonoBehaviour
    {
        public Image Icon;
        public TextMeshProUGUI TextNumber;

        public void SetData(ItemCode code, int amount){
            GOA.Item.ItemInfoData itemData = ItemAsset.instance.GetItemDataByCode(code);
            this.Icon.sprite = SpriteHelper.Instance.GetSprite(itemData.Images);
            this.TextNumber.text = amount.ToString();
        }
    }
}
