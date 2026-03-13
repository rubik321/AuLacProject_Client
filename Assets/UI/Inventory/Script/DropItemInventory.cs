using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DropItemInventory : MonoBehaviour
{
    public Image avaItem;
    public TextMeshProUGUI txtPos, txtDes,txtnumber;
    int numberOfItem;
    Animator anim;
    // Start is called before the first frame update
    void Awake()
    {
        numberOfItem = 10;
        txtnumber.text = numberOfItem.ToString();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if (anim != null)
        {
            anim.Play("ShowPopup");
        }
    }
    private void OnDisable()
    {
        if (anim != null)
        {
            anim.Play("HidePopup");
        }
    }
    public void OnButtonMin()
    {
        numberOfItem = 0;
        txtnumber.text = numberOfItem.ToString();
    }
    public void OnButtonMax()
    {
        numberOfItem =100;
        txtnumber.text = numberOfItem.ToString();
    }
    public void OnButtonMinus()
    {
        if (numberOfItem > 0)
        {
            numberOfItem--;
        }
        txtnumber.text = numberOfItem.ToString() ;
    }
    public void OnButtonPlus()
    {
        if (numberOfItem < 100)
        {
            numberOfItem++;
        }
        txtnumber.text = numberOfItem.ToString();
    }
    public void OnButtonClose()
    {
        if (anim != null)
        {
            GetComponentInParent<InventoryUI>().OnButtonClose(anim);
        }

    }
}
