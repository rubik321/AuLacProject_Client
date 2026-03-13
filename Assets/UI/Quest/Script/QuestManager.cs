using System;
using System.Collections;
using System.Collections.Generic;
using GOA.UserData;
using UnityEngine;
public enum QuestType
{
    Kill_Monster = 0,
    Kill_Regular_Mobs = 1,
    Kill_Greater_Mobs = 2,
    Outpost = 3,
    Reaper = 4,
    Materials = 5,
    Spend_Gin = 6,
    Spend_Gold =7,
    Buy_Item=8,
    Use_Potion=9


}
public class QuestManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static QuestManager Instance;
    void Start()
    {
        Instance = this;
        //PlayerPrefs.DeleteAll();
        //CheckResetDay();
    }
    void CheckResetDay()
    {
        if (CheckNewDay())
        {
            SetDailyQuest();
            Debug.Log("Is new day ________________________");  
        }
           
    }
    public void SetDailyQuest() {
        foreach (QuestData data in UserData.Instance.questData.QuestData)
        {
            if (data.Group == 0)
            {
                PlayerPrefs.SetInt(UserData.Instance.data.UserName+data.Index, 0);
            }
        }
    }
    public bool CheckNewDay()
    {
        
        DateTime today = DateTime.Now;
        int year = today.Year;
        int month = today.Month;
        int day = today.Day;
        string dayNow = day+"/"+month+"/"+year;
        dayNow = "";
        if (PlayerPrefs.GetString("CheckNewDay") == dayNow)
        {
            return false;
        }
        else
        {
            PlayerPrefs.SetString("CheckNewDay", dayNow);
            return true;
        }
            
    }
    public void UpdateQuest(QuestType target,int number)
    {
        
        Debug.Log("________________"+target +" Qeust number : "+ number);
        GetQuestbyIndex(target, number);
    }
    void GetQuestbyIndex(QuestType targetIndex,int number)
    {
        foreach (QuestData data in UserData.Instance.questData.QuestData)
        {
            string nameOfData = UserData.Instance.data.UserName + data.Index;
           // Debug.Log((QuestType)data.Target_Index+" - "+targetIndex+"-"+ PlayerPrefs.GetInt(nameOfData) +"-"+ data.Number);
            if ((QuestType)data.Target_Index == targetIndex && PlayerPrefs.GetInt(nameOfData)<data.Number)
            {
                var temp = PlayerPrefs.GetInt(nameOfData) + number;
                PlayerPrefs.SetInt(nameOfData, temp);
                Debug.Log(nameOfData + " : "+PlayerPrefs.GetInt(nameOfData));
            }
        }
    }
}
