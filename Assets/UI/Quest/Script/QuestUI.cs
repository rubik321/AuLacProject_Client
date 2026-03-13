using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Pixelplacement;
using UnityEngine.UI;
using GOA.UserData;
using System;
using System.Globalization;

public class QuestUI : MonoBehaviour
{
    public GameObject itemQuestPrefab, questMain, detailGo;
    public Transform dailyContent, weeklyContent, mainContent;
    public GameObject[] btnTabOns, tabContents;
    public TextMeshProUGUI timeLeft;
    public List<ItemQuest> ItemQuests = new List<ItemQuest>();

    public DateTime day;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CotimeLeft());
    }
    public void BtnTab_Onclick(int index)
    {

        foreach (GameObject go in btnTabOns)
        {
            go.SetActive(false);
        }

        foreach (GameObject go in tabContents)
        {
            go.SetActive(false);
        }
        btnTabOns[index].SetActive(true);
        tabContents[index].SetActive(true);

    }
    IEnumerator CotimeLeft()
    {
        while (true)
        {
            
            yield return new WaitForSeconds(1);
            CultureInfo ci = CultureInfo.InvariantCulture;
            var temp = day - DateTime.UtcNow;
            temp = new TimeSpan(temp.Hours, temp.Minutes, temp.Seconds);
            if (temp.TotalSeconds < 0)
            {
                foreach (ItemQuest item in this.ItemQuests)
                {
                    item.gameObject.SetActive(false);
                }
                QuestManager.Instance.SetDailyQuest();
                
                break;
            }
            timeLeft.text = "Time left : " + temp.ToString();
        }


    }
    public void ShowQuestUI()
    {
        questMain.SetActive(true);
        SetUpForDailyQuest();
    }
    public void ShowDetailOn_Onclick()
    {
        detailGo.SetActive(true);
    }
    public void ShowDetailOff_Onclick()
    {
        detailGo.SetActive(false);
    }
    public void BtnQuit()
    {
        CharacterUIController.Instance.CameraUI.SetActive(false);
        CharacterUIController.Instance.bgGo.SetActive(false);
        questMain.SetActive(false);
        GetComponentInParent<NTFunctions_old.UIManager>().MainCanvas.SetActive(true);
    }
    public void SetUpForDailyQuest()
    {
        if (ItemQuests.Count > 0)
        {
            for(int i = 0; i < ItemQuests.Count; i++)
            {
                Destroy(ItemQuests[i].gameObject);
            }
            this.ItemQuests.Clear();
        }

        int index = 0;
        foreach (QuestData data in UserData.Instance.questData.QuestData)
        {
            if (data.Group == 0)
            {

                GameObject go = Instantiate(itemQuestPrefab);
                go.transform.SetParent(dailyContent, false);
                go.GetComponent<ItemQuest>().SetUp(data);
                this.ItemQuests.Add(go.GetComponent<ItemQuest>());
            }
        }
    }
}


