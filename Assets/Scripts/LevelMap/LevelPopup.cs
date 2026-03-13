using System.Collections;
using System.Collections.Generic;
using GOA.Config;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Rubik._2DGPS.Campaign;
using Rubik._2DGPS.Card;
using Rubik.Combat;

public class LevelPopup : MonoBehaviour
{
    int index;
    public TextMeshProUGUI txtTitle;
    public DragCamera2D DragCam;
    public Image[] lsImageEnemies;
    public Image[] lsImageEnemiesBox;
    public Sprite defaultSpr,sprBoxOn,sprBoxOff;
    public Sprite[] lsSprites;
    public List<RewardItem> lsRewards;
    // Start is called before the first frame update
    private void OnEnable()
    {
        
    }
    public void ShowUp(int level,int levelCampaign)
    {
        index = level;
        StaticData.levelCampaign = levelCampaign;
        gameObject.SetActive(true);
        txtTitle.text = "Level "+(levelCampaign + 1).ToString();
        DragCam.enabled = false;
        var levelCard = CampaignManager.instance.CampaignLvDatas[levelCampaign].Cards;
        for(int i = 0; i < lsImageEnemies.Length; i++)
        {
            lsImageEnemies[i].sprite = defaultSpr;
            lsImageEnemiesBox[i].sprite = sprBoxOff;
        }
        foreach(CardSlot data in levelCard)
        {
            var temp = (int)data.Index;
            Debug.Log(data.Index);
           // lsImageEnemies[data.Slot].sprite = AssetLoader.Instance.lsCharacters[temp].icon;
           // lsImageEnemiesBox[data.Slot].sprite = sprBoxOn;
        }
        var reward = CampaignManager.instance.CampaignLvDatas[levelCampaign].Rewards;
        for (int i = 0; i < lsRewards.Count; i++)
        {
           
            lsRewards[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < reward.Count; i++)
        {
            //lsRewards[i].itemAva.sprite = SpriteHelper.Instance.GetSprite(reward[i].Type.ToString());
            lsRewards[i].itemAva.sprite = lsSprites[Random.Range(0,lsSprites.Length)];
            lsRewards[i].valueTxt.text = reward[i].Amount.ToString();
            lsRewards[i].gameObject.SetActive(true);
        }
    }
    public void Close()
    {
        gameObject.SetActive(false);
        DragCam.enabled = true;
    }
    public void OnButtonPlay()
    {
        //if (index == 1)
        {
            bl_SceneLoaderManager.LoadScene(Configs.Combat_Screen);
            StaticData.GameMode = GameMode.Adventure;

        }
    }
}
