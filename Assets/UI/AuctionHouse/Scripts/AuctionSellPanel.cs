using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.UI
{
    public class AuctionSellPanel : MonoBehaviour
    {
        [SerializeField] AuctionConfirmPanel confirmPanel;
        [SerializeField] Transform itemsHolder;
        [SerializeField] AuctionItemElement itemElementPrefab;
        List<AuctionItemElement> itemElementInstances = new List<AuctionItemElement>();

        AuctionUI.ListItem data;
        int currentPage = 1;
        bool canChangePage = false;

        private void OnEnable()
        {
            GetItemInBag(currentPage, FillItemToScroll);
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
                itemElementInstances[i].InitData(data.items[i], false, SellItem);
            }

            for (int i = data.items.Length; i < itemElementInstances.Count; i++)
            {
                itemElementInstances[i].gameObject.SetActive(false);
            }
        }

        private void SellItem(string instanceId)
        {
            confirmPanel.Show(() =>
            {
                // Call API
            });
        }

        private void GetItemInBag(int page, Action onComplete)
        {
            canChangePage = false;
            // Call API bag
            // => On Complete 
            canChangePage = true;
            data = new AuctionUI.ListItem();
        }

        public void OnClickClose()
        {
            gameObject.SetActive(false);
        }

        public void OnClickNext()
        {
            if (!canChangePage)
                return;
            currentPage++;
            GetItemInBag(currentPage, FillItemToScroll);
        }

        public void OnClickPrevious()
        {
            if (!canChangePage)
                return;
            currentPage--;
            GetItemInBag(currentPage, FillItemToScroll);
        }
    }
}
