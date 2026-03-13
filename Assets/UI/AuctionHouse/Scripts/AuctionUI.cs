using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
using System;
using UnityEngine.UI;

namespace Rubik.UI
{
    public class AuctionUI : State
    {
        [SerializeField] AuctionConfirmPanel confirmPanel;
        [SerializeField] GameObject panelSell;
        [SerializeField] Transform itemsHolder;
        [SerializeField] AuctionItemElement itemElementPrefab;
        List<AuctionItemElement> itemElementInstances = new List<AuctionItemElement>();
        ListItem data;
        bool canChangePage = false;
        int currentPage = 1;

        private void OnEnable()
        {
            GetItemInAuctionHouse(1, FillItemToScroll);
        }

        private void FillItemToScroll()
        {
            for (int i = 0; i < data.items.Length; i++)
            {
                if (i >= itemElementInstances.Count)
                {
                    var element = Instantiate(itemElementPrefab, itemsHolder);
                    itemElementInstances.Add(element);
                }
                itemElementInstances[i].gameObject.SetActive(true);
                itemElementInstances[i].InitData(data.items[i], false, BuyItem);
            }

            for (int i = data.items.Length; i < itemElementInstances.Count; i++)
            {
                itemElementInstances[i].gameObject.SetActive(false);
            }
        }

        private void BuyItem(string instanceId)
        {
            confirmPanel.Show(() =>
            {
                // Call API
            });
        }

        private void GetItemInAuctionHouse(int page, Action onComplete)
        {
            canChangePage = false;
            // Call API bag
            // => On Complete 
            canChangePage = true;
            data = new AuctionUI.ListItem();
        }

        public void OnClickSellItem()
        {
            panelSell.SetActive(true);
        }

        public void OnClickClose()
        {
            StateMachine.Exit();
        }

        public void OnClickNext()
        {
            if (!canChangePage)
                return;
            currentPage++;
            GetItemInAuctionHouse(currentPage, FillItemToScroll);
        }

        public void OnClickPrevious()
        {
            if (!canChangePage)
                return;
            currentPage--;
            GetItemInAuctionHouse(currentPage, FillItemToScroll);
        }

        [Serializable]
        public class ListItem
        {
            public ItemData[] items;
        }

        [Serializable]
        public class ItemData
        {
            public string instanceId;
            public string id;
            public int quantity;
            public float price;
        }
    }
}
