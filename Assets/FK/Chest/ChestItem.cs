using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using GOA.Item;
using GOA.UserData;
public class ChestItem : MonoBehaviour
{
    [SerializeField] public ItemCode typeOfItem;
    public Image iconChest;
    public GameObject hightlightGO,notiGo;
    public Text numberTxt;
    public Vector2 orginPos;
    bool isFirst = false;
    public int number;
    private void Awake()
    {
        orginPos = transform.localPosition;
    }
    public void SetUp()
    {
        //if (!isFirst)
        //{
        //    orginPos = transform.localPosition;
        //}
        isFirst = true;
         number = GetAmount();
        numberTxt.text ="x"+ number;
    }
    int GetAmount() {
        switch (typeOfItem)
        {
            case ItemCode.BronzeChest_FK:
                return UserData.Instance.data.BronzeChest_FK;
                break;
            case ItemCode.GoldChest_FK:
                return UserData.Instance.data.GoldChest_FK;
                break;
            case ItemCode.DiamondChest_FK:
                return UserData.Instance.data.DiamondChest_FK;
                break;
            case ItemCode.PlatinumChest_FK:
                return UserData.Instance.data.PlatinumChest_FK;
                break;
            case ItemCode.WoodChest_FK:
                return UserData.Instance.data.WoodChest_FK;
                break;
            default:
                return UserData.Instance.data.WoodChest_FK;
                break;
        }
    }
   public void SetButtonOn()
    {
        //transform.DOLocalMoveY(orginPos.y + 10,.2f);
        hightlightGO.SetActive(true);
    }
    public void SetButtonOff()
    {
        //transform.DOLocalMoveY(orginPos.y, .2f);
        hightlightGO.SetActive(false);
    }
}
