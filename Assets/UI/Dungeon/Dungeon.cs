using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOA.UserData;
using Sirenix.OdinInspector;
using  GOA.WorldMap;
using GOA.Config;
[Serializable]
public class DungeonInfo
{
    public int Stage;
    public int Rank;
    public RankInfo[] Rankers;
}
[Serializable]
public class RankInfo
{
    public string userID;
    public int score;
    public DataRankInfo data;
}
[Serializable]
public class DataRankInfo
{
    public string UserName;
    public int Avatar;

}
public class Dungeon : SimplePopup
{
    public List<StageItem> lsStages;
    public Sprite floorOn, floorOff, stageOn, stageOff;
    public DungeonInfo info;
    public TMPro.TextMeshProUGUI numberTxt;
    protected override void Start()
    {
        base.Start();
        APIManager.Instance.GetDungeon();
    }
    public override void ShowUp(AnimationPopupType type = AnimationPopupType.OnTopDown)
    {
        base.ShowUp(type);
    }
    [Button]
    public void ShowDungeon()
    {
        gameObject.SetActive(true);
        ShowUp(AnimationPopupType.OnFade);
        Setup();
        numberTxt.text = UserData.Instance.Inventory.SaikiSeal.ToString();
    }

    public void HideDungeon()
    {
        //gameObject.SetActive(false);
        Hide();
    }
    void Setup()
    {
        int temp = 0;
        if (UserData.Instance.DungeonInfo.Stage > 5)
        {
            for(int i = UserData.Instance.DungeonInfo.Stage - 5;i< (UserData.Instance.DungeonInfo.Stage + 6); i++)
            {
                if(i == UserData.Instance.DungeonInfo.Stage)
                {
                    lsStages[temp].SetUp(i, floorOn, stageOn);
                }
                else
                {
                    lsStages[temp].SetUp(i, floorOff, stageOff);
                }
                temp++;
            }
        }
        else
        {
            for (int i =0; i < lsStages.Count; i++)
            {
                if (i == UserData.Instance.DungeonInfo.Stage)
                {
                    lsStages[i].SetUp(i, floorOn, stageOn);
                }
                else
                {
                    lsStages[i].SetUp(i, floorOff, stageOff);
                }

            }
        }
    }

    public void FightDungeon()
    {
        Debug.Log("Saiki seal : " + UserData.Instance.Inventory.SaikiSeal);
        if (UserData.Instance.Inventory.SaikiSeal < 1)
        {
            Rubik.UI.HUDCanvas.Instance.ShowNotification("Dungeon Ticket is not enough");
            return;
        }
        APIManager.Instance.JoinDungeon((UnityEngine.Events.UnityAction)(()=> {
            UserData.Instance.Inventory.SaikiSeal--;
            DataInCombat data = new DataInCombat();
            data.TypeCombat = GOA.WorldMap.MobTypeCode.Dungeon;
            data.mobsInCombat = new List<MobInfo>();
            int count;
            if (UserData.Instance.DungeonInfo.Stage == 0)
            {
                count = 1;
            }
            else if (UserData.Instance.DungeonInfo.Stage % 3 == 0)
            {
                count = 3;
            }
            else
            {
                count = UserData.Instance.DungeonInfo.Stage % 3;
            }
            int lv;
            if (UserData.Instance.DungeonInfo.Stage == 0)
            {
                lv = 1;
            }
            else
            {
                lv = (UserData.Instance.DungeonInfo.Stage - 1) / 3 + 2;
            }
            for (int i = 0; i < count; i++)
            {
                var index = MobManager.fixedIndexRegularMobs[UnityEngine.Random.Range(0, MobManager.fixedIndexRegularMobs.Length)];
                var mobInfo = UserData.Instance.DictionaryMobInfo[index];

                mobInfo.Lv = lv;
                mobInfo.CurHp = mobInfo.HP;
                data.mobsInCombat.Add(mobInfo);
            }
            UserData.Instance.DataInCombat = data;
            try
            {
                bl_SceneLoaderManager.LoadScene(Configs.Combat_Screen);
                Rubik.Common.AudioHelper.AudioCtrl.Instance.Play(Rubik.Common.AudioHelper.AudioName.Test);
                HideDungeon();
            }
            catch (System.Exception e)
            {
                NTPackage_old.Functions.NTLog.LogError(e.ToString(), gameObject);
            }
        }));
       
    }
    public void ShowRank()
    {
        FindObjectOfType<RankDungeon>().ShowRank();
    }
}
