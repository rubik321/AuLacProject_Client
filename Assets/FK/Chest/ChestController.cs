using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rubik.KingFish.Chest;

public class ChestController : MonoBehaviour
{
    public List<ChestItem> lsButtonChest;
    public Text chestNumber,chestNameTxt;
    public GameObject nextGo, preGo,openChess;
    public Sprite buttonOn, buttonOff;
 
    int indexCurrent = 0;
    private void Start()
    {
       
    }
    private void OnEnable()
    {
        OnButtonChest(0);
    }
    public void OnButtonChest(int indexOfChest)
    {
        indexCurrent = indexOfChest;
        for (int i = 0; i < lsButtonChest.Count; i++)
        {
            lsButtonChest[i].SetUp();
            lsButtonChest[i].SetButtonOff();
            
        }
        lsButtonChest[indexOfChest].SetButtonOn();
        chestNameTxt.text = lsButtonChest[indexOfChest].typeOfItem.ToString();
        if (lsButtonChest[indexOfChest].number <= 0)
        {
            openChess.GetComponent<Image>().sprite = buttonOff;
            //openChess.GetComponent<UIButtonSimple>().onPress.ru
        }
        else
        {
            openChess.GetComponent<Image>().sprite = buttonOn;
        }
        if (indexCurrent == 0)
        {
            preGo.SetActive(false);
        }
        if (indexCurrent == lsButtonChest.Count)
        {
            nextGo.SetActive(false);
        }
        if (lsButtonChest[indexCurrent].number > 10)
        {
            openChess.GetComponentInChildren<Text>().text = "Open 10 chest !";
        }
        else if ((lsButtonChest[indexCurrent].number > 0))
        {
            openChess.GetComponentInChildren<Text>().text = " Open "+ lsButtonChest[indexCurrent].number+ " chest !";
        }
        else
        {
            openChess.GetComponentInChildren<Text>().text = " Open 1 chest !";
        }
        // chestBox.Skeleton.SetSkin((indexCurrent + 1).ToString());
    }
    public void OnNextButton()
    {
        if (indexCurrent < lsButtonChest.Count - 1)
        {
            indexCurrent++;
            OnButtonChest(indexCurrent);
        }
        preGo.SetActive(true);
        if (indexCurrent == lsButtonChest.Count-1)
        {
            nextGo.SetActive(false);
        }
    }
    public void OnPreButton()
    {
        if (indexCurrent>0)
        {
            indexCurrent--;
            OnButtonChest(indexCurrent);
        }
        nextGo.SetActive(true);
        if (indexCurrent == 0)
        {
            preGo.SetActive(false);
        }
    }
     public void OnOpenChest()
    {
        ChestManager.instance.OpenChest((ChestType)indexCurrent, lsButtonChest[indexCurrent].number, (data) =>
        {
            // ShowChestReward(data);
            FindObjectOfType<ChestController>().OnButtonChest(indexCurrent);
        }, ()=> {
            Debug.Log("Chest Error");
        });
            
    }
}
