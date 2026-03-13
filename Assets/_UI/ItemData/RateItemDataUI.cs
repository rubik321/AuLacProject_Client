using System.Collections.Generic;
using NTPackage.Functions;
using NTPackage.UI;
using UnityEngine;
using TMPro;

namespace Rubik.ItemPlayer
{
    public class RateItemDataUI : PopupUI
    {
        public RateItemDataElement RateItemDataElementPrefab;
        public List<RateItemDataElement> RateItemDataElements;
        public Transform Holder;
        public TextMeshProUGUI TextTitle;

        public void SetData(List<ListItemRate> listItemRates, List<string> titles, string title){
            this.TextTitle.text = title;
            this.RateItemDataElements = new List<RateItemDataElement>();
              Rubik.Common.Common.ResetContentY(Holder);
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(Holder);
            for (int i = 0; i < listItemRates.Count; i++)
            {
                RateItemDataElement rateItemDataElement = ObjectPoolingManager.Instance.InstantiateObject<RateItemDataElement>(ObjectPoolingConfig.RateItemDataElement, this.RateItemDataElementPrefab.transform);
                rateItemDataElement.SetData(listItemRates[i], titles[i], true);
                RateItemDataElements.Add(rateItemDataElement);
                rateItemDataElement.transform.SetParent(Holder);
                NTFunction.ResetPosition(rateItemDataElement.transform);
            }
        }

        public override void ScriptOffUI()
        {
            base.ScriptOffUI();
            foreach (RateItemDataElement rateItemDataElement in RateItemDataElements)
            {
                rateItemDataElement.Clear();
            }
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(Holder);
            RateItemDataElements.Clear();
        }
    }
}