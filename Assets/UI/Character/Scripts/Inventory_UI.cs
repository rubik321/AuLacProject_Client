using DG.Tweening;
using NTPackage.Functions;
using NTPackage.UI;
using Rubik.Common.AudioHelper;
using Rubik.ItemPlayer;
using Rubik.UIController;
using Rubik.UserDataPlayer;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_UI : PopupUI
{
    public ItemInventory invenItem;
    public Transform contentInven;
    public ItemInfo itemInfo;
    public Transform coinTrans;
    public List<ItemInventory> lsItemInvens = new List<ItemInventory>();
    public TextMeshProUGUI slotTxt,txtPriceTxt, txtPriceTxt1, txtBuyslot;
    public UnityEngine.UI.Image slotProcess;
    public GameObject confirmPopup,invenEmpty;
    ItemType itemCurrentType;
    [SerializeField] List<GameObject> lsEmptySlots = new List<GameObject>();

    public TextMeshProUGUI TextAmount;

    public override void OnUI(object data = null, bool isDefaultSound = true)
    {
        base.OnUI(data, isDefaultSound);
       
        SetInvenItem();
    }

    public void SetInvenItem()
    {
        var items = ItemDataManager.Instance.GetItemBag();
        Debug.Log("Show all item : " + items.Count);
        int index = 0;
        itemInfo.gameObject.SetActive(false);
        foreach (ItemInventory item in lsItemInvens)
        {
            item.gameObject.SetActive(false);
            item.UnChose();
        }
        //if (items.Count <= 5)
        //{
        //    contentInven.GetComponent<GridLayoutGroup>().constraint = GridLayoutGroup.Constraint.Flexible;
        //}
        //else if (items.Count <= 10)
        //{
        //    contentInven.GetComponent<GridLayoutGroup>().constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        //    contentInven.GetComponent<GridLayoutGroup>().constraintCount = 5;
        //}
        //else
        //{
        //    contentInven.GetComponent<GridLayoutGroup>().constraint = GridLayoutGroup.Constraint.FixedRowCount;
        //    contentInven.GetComponent<GridLayoutGroup>().constraintCount = 2;
        //}
        itemInfo.sellButton.gameObject.SetActive(false);
        itemInfo.useButton.gameObject.SetActive(false);
        itemInfo.numberSell.gameObject.SetActive(false);
        contentInven.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        contentInven.GetComponentInParent<ScrollRect>().vertical = false;
        //contentInven.transform.localPosition = Vector3.zero;
       
           // contentInven.GetComponentInParent<ScrollRect>().vertical = true;
       
        foreach (Rubik.ItemPlayer.ItemData item in items)
        {
            if(index >= lsItemInvens.Count)
            {
                ItemInventory itemInven = Instantiate(invenItem);
                lsItemInvens.Add(itemInven);
            }
            lsItemInvens[index].gameObject.SetActive(true);
            lsItemInvens[index].transform.SetParent(contentInven, false);
            lsItemInvens[index].itemData.SetData(item,true);
            if(ItemDataManager.Instance.GetPriceSell(item.Type)!=null)
                lsItemInvens[index].txtCoin.text = ItemDataManager.Instance.GetPriceSell(item.Type).Amount.ToString();
            lsItemInvens[index].btnClick.onClick.RemoveAllListeners();
            int temp = index;
            lsItemInvens[index].btnClick.onClick.AddListener(() =>
            {
                AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
                OffItems(temp, item);
                currentItem = temp;
                currentType = item;
                itemInfo.sellButton.gameObject.SetActive( ItemDataManager.Instance.CanSellItem(item.Type));
                //itemInfo.useButton.gameObject.SetActive(!ItemDataManager.Instance.CanSellItem(item.Type));
                itemInfo.numberSell.gameObject.SetActive(ItemDataManager.Instance.CanSellItem(item.Type));
            });

            index++;
        }
        int number = lsItemInvens.Count / 10+1;
       
        if (items.Count > 0)
        {
            if (currentItem >= items.Count || (currentItem < items.Count&&items[currentItem].Amount <= 0))
            {
                currentItem = 0;
            }
            lsItemInvens[currentItem].btnClick.onClick.Invoke();
            itemInfo.gameObject.SetActive(true);
        }
        SetProcessSlot(items.Count, UserDataManager.Instance.GetCurrentSlotInventoryBag());
        foreach(GameObject go in lsEmptySlots)
        {
            go.SetActive(false);
        }
        for (int i = 0; i < UserDataManager.Instance.GetCurrentSlotInventoryBag() - items.Count; i++)
        {
            //lsEmptySlots[i].SetActive(true);
            if(i>= lsEmptySlots.Count)
            {
                GameObject go = Instantiate(invenEmpty);
                go.transform.SetParent(contentInven, false);
                go.transform.SetAsLastSibling();
                lsEmptySlots.Add(go);
            }
            lsEmptySlots[i].SetActive(true);
            lsEmptySlots[i].transform.SetAsLastSibling();
        }
        DOVirtual.DelayedCall(1f, () =>
        {
            
            contentInven.GetComponentInParent<ScrollRect>().vertical = true;
        });
    }
    public void SetProcessSlot(int current,int maxSlot)
    {
        slotTxt.text = current + "/" + maxSlot;
        slotProcess.fillAmount = (float)current / maxSlot;
        txtPriceTxt.text = UserDataManager.Instance.GetPriceExpandInventoryBag().Amount.ToString();
        txtPriceTxt1.text = UserDataManager.Instance.GetPriceExpandInventoryBag().Amount.ToString();
        txtBuyslot.text ="Buy " + UserDataManager.Instance.GetSlotIncreaseInventoryBag().ToString()+ " new slots";
    }

    public void BuySlot()
    {
        AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
        confirmPopup.SetActive(true);
    }
    public void BuySlotYes()
    {
        AudioCtrl.Instance.Play(AudioName.UI_Button_Confirm);
        confirmPopup.SetActive(false);
        StartCoroutine(UserDataManager.Instance.IEExpandInventoryBag(() => {
            AppsFlyerManager.TrackingEvent(AppsflyerEvents.gems_spent, "buy_Slot", 100);
            SetInvenItem();
        }));
    }
    int currentItem = 0;
    [SerializeField]Rubik.ItemPlayer.ItemData currentType;
    public void OffItems(int index, Rubik.ItemPlayer.ItemData itemData)
    {
        foreach(ItemInventory item in lsItemInvens)
        {
            item.UnChose();

        }
        //lsItemInvens[index].hightLight.enabled = true;
        lsItemInvens[index].Chose();
        TextAmount.text = "x" + NTFunction.FormatNumberWithComa(itemData.Amount);
        numberSell = 1;
        itemInfo.SetItem(itemData);
    }
    public long numberSell = 1;
    public void OnButtonSell()
    {
        AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
        PopupManager.Instance.OnUI(PopupCode.ConfirmUI, null, (popupUI) =>
        {

            ComfirmUI confirmUI = popupUI as ComfirmUI;
            confirmUI.SetItem(currentType);
            confirmUI.SetAction(() =>
            {
                AudioCtrl.Instance.Play(AudioName.UI_Button_Confirm);
                ItemDataManager.Instance.SellItem(currentType.Type,(int) numberSell, () => {
                    
                    confirmUI.OffUI();
                    Invoke("GiveCOin", 0.1f);
                });
            });

        });
       
    }
    public void OnButtonUse()
    {
        PopupManager.Instance.OnUI(PopupCode.ShardUI, currentType, (popupUI) =>
        {

            

        });

    }
    public void GiveCOin()
    {
       // WorldMapUIController.Instance.IncreaseEffect(itemInfo.sellPriceTxt.transform.position, true);
        SetInvenItem();
    }
}
