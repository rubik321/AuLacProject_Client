using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rubik.UI
{
    public class AuctionItemElement : MonoBehaviour
    {
        [SerializeField] TMP_Text textName;
        [SerializeField] TMP_Text textDescription;
        [SerializeField] Image iconItem;
        [SerializeField] TMP_Text textQuantity;
        [SerializeField] TMP_Text textPrice;
        bool isBuy;
        string instanceId;
        Action<string> onAlterItem;

        public void InitData(AuctionUI.ItemData data, bool isBuy, Action<string> onAlterItem = null)
        {
            textName.text = GetItemName(data.id);
            textDescription.text = GetItemDescription(data.id);
            textPrice.text = data.price.ToString();
            textQuantity.text = data.quantity.ToString();
            iconItem.sprite = GetIconSprite(data.id);
            this.isBuy = isBuy;
            instanceId = data.instanceId;
            this.onAlterItem = onAlterItem;
        }

        private string GetItemName(string id)
        {
            return "name_" + id;
        }

        private string GetItemDescription(string id)
        {
            return "des__" + id;
        }

        private Sprite GetIconSprite(string id)
        {
            return null;
        }

        public void OnClick()
        {
            onAlterItem?.Invoke(instanceId);
            gameObject.SetActive(false);
        }
    }
}
