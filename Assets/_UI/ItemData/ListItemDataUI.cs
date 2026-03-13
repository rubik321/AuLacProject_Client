using System.Collections;
using System.Collections.Generic;
using NTPackage.Functions;
using Rubik.CardPlayer;
using UnityEngine;

namespace Rubik.ItemPlayer
{
    using Rubik.CardPlayer;
    public class ListItemDataUI : NTBehaviour
    {
        public ItemDataUI ItemDataUIPrefab;
        public List<ItemDataUI> ItemDataUIList;
        public ItemData[] ItemDatas;
        public CardPlayer[] CardPlayers;

        public void Clear()
        {
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(transform);
            this.ItemDataUIList.Clear();
        }

        public void SetData(ItemData[] itemData, CardPlayer[] cardPlayers, bool showMulti = false, bool isShowToolTip = false)
        {
            this.Clear();
            this.ItemDatas = itemData;
            this.CardPlayers = cardPlayers;
            if(cardPlayers != null){
                foreach (CardPlayer cardPlayer in cardPlayers)
                {
                    ItemDataUI itemDataUI = ObjectPoolingManager.Instance.InstantiateObject<ItemDataUI>(ObjectPoolingConfig.ItemDataUI, this.ItemDataUIPrefab.transform);
                    itemDataUI.transform.SetParent(this.transform);
                    itemDataUI.SetData(cardPlayer, isShowToolTip);
                    NTFunction.ResetPosition(itemDataUI.transform);
                    this.ItemDataUIList.Add(itemDataUI);
                }
            }
            if (itemData != null)
            {
                foreach (ItemData item in itemData)
                {
                    ItemDataUI itemDataUI = ObjectPoolingManager.Instance.InstantiateObject<ItemDataUI>(ObjectPoolingConfig.ItemDataUI, this.ItemDataUIPrefab.transform);
                    itemDataUI.transform.SetParent(this.transform);
                    itemDataUI.SetData(item, showMulti, isShowToolTip);
                    NTFunction.ResetPosition(itemDataUI.transform);
                    this.ItemDataUIList.Add(itemDataUI);
                }
            }
        }
    }
}
