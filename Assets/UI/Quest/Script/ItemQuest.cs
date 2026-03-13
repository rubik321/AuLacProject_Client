using GOA.UserData;
using Rubik.Common.AudioHelper;
using Rubik.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemQuest : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI questNameTxt, processTxt, questInoTxt;
    [SerializeField] GameObject itemQuestPrefab,goButton,claimButtom;
    [SerializeField] Transform contentReward;
    [SerializeField] GOA.Item.BagInventoryItemUI[] lsItem;
    [SerializeField] Image processImg;
    [SerializeField] Sprite processCompleted, processClaimed, processing;
    void Start()
    {
        
    }
    public void SetUp(QuestData data)
    {
        questNameTxt.text = data.Index;
        questInoTxt.text = data.Description;
        //lsItem[0].ShowUp("icon_coin", data.GoldReward);
        //if (data.ItemReward1 != "")
        //    lsItem[1].ShowUp(data.ItemReward1, data.AmountReward1);
        //else
        //    lsItem[1].gameObject.SetActive(false);
        //if (data.ItemReward2 != "")
        //    lsItem[2].ShowUp(data.ItemReward2, data.AmountReward2);
        //else
        //    lsItem[2].gameObject.SetActive(false);
        //lsItem[3].ShowUp("icon_gin", data.SkillOrb);
        var nameOfQuest = UserData.Instance.data.UserName + data.Index;
        processTxt.text = PlayerPrefs.GetInt(nameOfQuest) + "/" + data.Number;
        processImg.fillAmount = ((float)PlayerPrefs.GetInt(nameOfQuest))/data.Number;
        if (PlayerPrefs.GetInt(nameOfQuest) < data.Number)
        {
            processImg.sprite = processing;
        }
        else if(!UserData.Instance.ListDailyQuest[data.Index])
        {
            processImg.sprite = processClaimed;
            claimButtom.SetActive(true);
            goButton.SetActive(false);
            claimButtom.GetComponent<Button>().onClick.AddListener(() =>
            {
                List<(string, int)> lsItem = new List<(string, int)>();
                if(data.Target_Index != 7)
                {
                    UserData.Instance.data.Coin += data.GoldReward;
                    lsItem.Add(new("icon_coin", data.GoldReward));
                }

                else
                {

                    lsItem.Add(new("icon_gin", data.GoldReward));
                    UserData.Instance.data.Gin += data.GoldReward;
                }

                //post API
                AudioCtrl.Instance.Play(AudioName.Collect_Gem_Sound);
                APIManager.Instance.ClaimDailyQuestAPI(data);
                processTxt.text = Lean.Localization.LeanLocalization.GetTranslationText("text_claimed");
                UserData.Instance.ListDailyQuest[data.Index] = true;
                goButton.SetActive(true);
                claimButtom.SetActive(false);
                HUDCanvas.Instance.ShowReward(lsItem);
               
            });
        }
        if(UserData.Instance.ListDailyQuest[data.Index])
        {
            processImg.sprite = processCompleted;
            claimButtom.SetActive(false);
            goButton.SetActive(true);
            processTxt.text = Lean.Localization.LeanLocalization.GetTranslationText("text_claimed");
        }
        //Debug.Log(data.Description + " : " + PlayerPrefs.GetInt(data.Index) + "/" + data.Number);
    }
}
