using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Rubik.ItemPlayer;

public class ItemInfo : MonoBehaviour
{
    public Image iconitem;
    public TextMeshProUGUI itemNameTxt,itemDesTxt,sellPriceTxt,placeHolderTxt;
    Rubik.ItemPlayer.ItemData itemData;
    [SerializeField] Inventory_UI inven;
    public Button sellButton,useButton;
    public GameObject numberSell;
    public TMP_InputField TMP_InputField;
    public void OnButtonSell()
    {
      //  ItemDataManager.Instance.sell
    }
    public void Start()
    {
        TMP_InputField.onValueChanged.AddListener(OnInputValueChanged);
       
    }
    public void SetItem(Rubik.ItemPlayer.ItemData itemData)
    {
        this.itemData = itemData;
        iconitem.sprite = ItemDataManager.Instance.GetIcon(itemData.Type);
        itemNameTxt.text= ItemDataManager.Instance.GetItemName(itemData.Type);
        itemDesTxt.text = ItemDataManager.Instance.GetItemDescription(itemData.Type);
        placeHolderTxt.text = inven.numberSell.ToString();
        TMP_InputField.text = inven.numberSell.ToString();
        sellPriceTxt.text = (inven.numberSell * ItemDataManager.Instance.GetPriceSell(itemData.Type).Amount).ToString();
    }
    public void Plus(int number)
    {
        if (inven.numberSell + number > 0&& inven.numberSell+number<= itemData.Amount)
        {
            inven.numberSell += number;
            TMP_InputField.text = inven.numberSell.ToString();
            sellPriceTxt.text = (inven.numberSell * ItemDataManager.Instance.GetPriceSell(itemData.Type).Amount).ToString();
        }
    }
    public void OnInputValueChanged(string text)
    {
        string filtered = "";
        foreach (char c in text)
        {
            if (char.IsDigit(c)) filtered += c;
        }

        // Nếu có ký tự bị loại, cập nhật lại text trong input
        if (filtered != text)
        {
            TMP_InputField.text = filtered;
        }

        // Parse sang int nếu có giá trị
        if (!string.IsNullOrEmpty(filtered))
        {
            long amount = long.Parse(filtered);
            if (amount < itemData.Amount)
            {
                inven.numberSell = amount;
            }
            else
            {
                inven.numberSell = itemData.Amount;
                TMP_InputField.text = inven.numberSell.ToString();
                
            }
            sellPriceTxt.text = (inven.numberSell * ItemDataManager.Instance.GetPriceSell(itemData.Type).Amount).ToString();

        }
        else
        {
            inven.numberSell =1;
            sellPriceTxt.text = (inven.numberSell * ItemDataManager.Instance.GetPriceSell(itemData.Type).Amount).ToString();

        }

    }
}
