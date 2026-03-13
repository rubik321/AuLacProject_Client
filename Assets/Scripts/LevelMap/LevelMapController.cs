using System.Collections;
using System.Collections.Generic;
using GOA.Config;
using Rubik._2DGPS.Campaign;
using Rubik._2DGPS.UserData;
using UnityEngine;

public class LevelMapController : MonoBehaviour
{
    public List<LevelItem> lsLevelMapItems;
    public LevelPopup levelPopup;
    public void Start()
    {
        SetUpLevel();
    }

    public void SetUpLevel()
    {
        for(int i= 0; i < lsLevelMapItems.Count; i++)
        {
            int temp = i;
            if (UserDataManager.instance.UserData.CampaignLv < i)
            {
                lsLevelMapItems[i].SetUp(0, temp);
            }
            else if (UserDataManager.instance.UserData.CampaignLv == i)
            {
                lsLevelMapItems[i].SetUp(1, temp);
            }
            else
            {
                lsLevelMapItems[i].SetUp(2, temp);
            }
        }
    }
    public void BackToWorldMap()
    {
        bl_SceneLoaderManager.LoadScene(Configs.WorldMap_Screen);
    }
    public void StartGame()
    {
        StaticData.levelCampaign = UserDataManager.instance.UserData.CampaignLv;
        bl_SceneLoaderManager.LoadScene(Configs.Combat_Screen);
        StaticData.GameMode = GameMode.Adventure;
    }
}
