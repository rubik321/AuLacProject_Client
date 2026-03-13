using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Cms;

public class CompanionItem : MonoBehaviour
{
    public TextMeshProUGUI itemName, compTxt;
    //public GameObject chooseObj, compTxt;
    public Button pressBtn;
    public Image gearIcon,rareIcon;
    public Sprite equipSprite;
    public Sprite unequipSprite,claimSprite;
    [SerializeField] Sprite[] lsRarity;
    public int EquipSlot;
    public bool IsEquipped { get; private set; }

    public void SetButtonEquip(bool isEquipped)
    {
        this.IsEquipped = isEquipped;
        if (isEquipped)
        {
            pressBtn.GetComponent<Image>().sprite = unequipSprite;
            pressBtn.GetComponentInChildren<TextMeshProUGUI>().text = Lean.Localization.LeanLocalization.GetTranslationText("unequipe");
        }
        else
        {
            pressBtn.GetComponent<Image>().sprite = equipSprite;
            pressBtn.GetComponentInChildren<TextMeshProUGUI>().text = Lean.Localization.LeanLocalization.GetTranslationText("equip");
        }
    }

    bool isNextAdventure = false;
    public void SetUp(CompanionData comp,UnityEngine.Events.UnityAction callbalk = null)
    {
        itemName.text = comp.CompName;
        gearIcon.sprite = SpriteHelper.Instance.GetSprite(comp.CompCode);
        rareIcon.sprite = lsRarity[comp.Rarity];
        pressBtn.onClick.RemoveAllListeners();
     
        if (!comp.IsInAdventure)
        {
            if (comp.Equiped)
            {
                pressBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Not Available";
                compTxt.text = "Equipped";
                pressBtn.GetComponent<Image>().sprite = unequipSprite;

            }
            else
            {
                if (comp.LastTimeAdventure > 0)
                {
                    isNextAdventure = true;
                    if (UnixTimeStampToDateTime(comp.LastTimeAdventure).Day == DateTime.Now.Day)
                    {
                        pressBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Not Available";
                        compTxt.text = "Rewarded";
                        pressBtn.GetComponent<Image>().sprite = unequipSprite;
                        pressBtn.onClick.RemoveAllListeners();
                        comp.IsInAdventure = false;
                        return;
                    }
                }
                pressBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Send";
                compTxt.text = "Idle";
                pressBtn.GetComponent<Image>().sprite = equipSprite;
                pressBtn.onClick.RemoveAllListeners();
                pressBtn.onClick.AddListener(() =>
                {
                    pressBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Not Available";
                    pressBtn.GetComponent<Image>().sprite = unequipSprite;
                    pressBtn.onClick.RemoveAllListeners();
                    comp.IsInAdventure = true;
                    comp.LastTimeAdventure = UnixTimestamp()*1000;
                    StartCoroutine(SetTimeAdventure(comp.LastTimeAdventure , comp));
                    if (callbalk != null)
                        callbalk();
                });
            }
            
        }
        else
        {
            pressBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Not Available";
            pressBtn.GetComponent<Image>().sprite = unequipSprite;
            StartCoroutine(SetTimeAdventure(comp.LastTimeAdventure, comp));

        }
      

    }
    private IEnumerator SetTimeAdventure(double startTime,CompanionData comp, UnityEngine.Events.UnityAction callbalk = null)
    {
        
       // var temp = ((DateTimeOffset)(DateTime.UtcNow)).ToUnixTimeSeconds()- startTime; 
        var temp =  (startTime/1000+3600*3) - UnixTimestamp();
        Debug.Log(temp);
        while (temp > 0)
        {
            compTxt.text = "On Adventure : " + CovertTime((long)temp);
            yield return new WaitForSeconds(1);
            temp--;
            compTxt.text = "On Adventure : " + CovertTime((long)temp);
        }
        pressBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Claim";
        compTxt.text = "Reward";
        pressBtn.GetComponent<Image>().sprite = claimSprite;
        pressBtn.onClick.AddListener(() =>
        {

            Debug.Log(comp.LastTimeAdventure);
            if (UnixTimeStampToDateTime(comp.LastTimeAdventure).Day == DateTime.Now.Day)
            {
                pressBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Resting";
                compTxt.text = "Rewarded";
                pressBtn.GetComponent<Image>().sprite = unequipSprite;
                pressBtn.onClick.RemoveAllListeners();
                comp.IsInAdventure = false;
            }
            else
            {
                comp.IsInAdventure = false;
                pressBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Send";
                compTxt.text = "Idle";
                pressBtn.GetComponent<Image>().sprite = equipSprite;
                pressBtn.onClick.RemoveAllListeners();
                pressBtn.onClick.AddListener(() =>
                {
                    pressBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Not Available";
                    pressBtn.GetComponent<Image>().sprite = unequipSprite;
                    pressBtn.onClick.RemoveAllListeners();
                    comp.IsInAdventure = true;
                    comp.LastTimeAdventure = UnixTimestamp()*1000;
                    StartCoroutine(SetTimeAdventure(comp.LastTimeAdventure , comp));
                    if (callbalk != null)
                        callbalk();
                   
                });
               
            }
            if (!isNextAdventure)
                comp.LastTimeAdventure = UnixTimestamp()*1000;
                APIManager.Instance.ClaimAdventure(comp);
            if (callbalk != null)
                callbalk();
        });

    }
    string CovertTime(long second)
    {
        TimeSpan t = TimeSpan.FromSeconds(second);

        string answer = string.Format("{0:D2}:{1:D2}:{2:D2}",
                        t.Hours,
                        t.Minutes,
                        t.Seconds
                        );
        return answer;
    }
    public static long UnixTimestamp()
    {
        return (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
    }
    bool CheckTimeToSendNext(long time)
    {

        return true;
    }
     DateTime UnixTimeStampToDateTime(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp/1000).ToLocalTime();
        return dateTime;
    }
}
