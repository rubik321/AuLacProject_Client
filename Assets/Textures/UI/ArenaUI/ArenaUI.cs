using Lean.Localization;
using NTPackage.UI;
using Rubik.Common.AudioHelper;
using Rubik.Myrk.Arena;
using Rubik.Myrk.BattlePass;
using Rubik.Myrk.BattleTeam;
using Rubik.UI;
using Rubik.UserProfile;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ArenaUI : PopupUI
{
    public Image[] tabsImg,lsClanUI;
    public Sprite tabOn, tabOff;
    public GameObject battleGo, resultGo, rankGo, leaderBoardGo;
    public Sprite[] lsAvaRanks;
   
    protected override void Start()
    {
        base.Start();
    }
    public override void OnUI(object data = null, bool isDefaultSound = true)
    {
        base.OnUI(data, isDefaultSound);
        ArenaTabs(0);
        foreach(Image img in lsClanUI)
        {
            img.sprite = lsAvaRanks[GetIndexRank(ArenaManager.Instance.GetRankingTypeByScore(ArenaManager.Instance.ArenaResponse.Score).ToString())];
        }
        
    }
    public override void UpdateData(object data = null)
    {
        base.UpdateData(data);
    }
    public override void OffUI()
    {
        base.OffUI();
        
    }
    public void ArenaTabs(int index)
    {
        foreach (Image img in tabsImg)
        {
            img.sprite = tabOff;
        }
        tabsImg[index].sprite = tabOn;
        switch (index)
        {
            case 0:
                battleGo.SetActive(true);
                leaderBoardGo.SetActive(false);
                resultGo.SetActive(false);
                rankGo.SetActive(false);
                break;
            case 1:
                battleGo.SetActive(false);
                leaderBoardGo.SetActive(false);
                resultGo.SetActive(true);
                rankGo.SetActive(false);
                break;
            case 2:
                battleGo.SetActive(false);
                leaderBoardGo.SetActive(false);
                resultGo.SetActive(false);
                rankGo.SetActive(true);
                break;
            case 3:
                battleGo.SetActive(false);
                leaderBoardGo.SetActive(true);
                resultGo.SetActive(false);
                rankGo.SetActive(false);
                break;
        }
        AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
    }
    public void OnButtonLineUp()
    {
        AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
        if (UserProfileManager.Instance.IsFunctionLocked(LockFunctionType.LineUp))
        {
            PopupManager.Instance.OnUI(PopupCode.MessagePanel, null, (PopupUI popupUI) =>
            {
                popupUI.GetComponent<MessagePanel>().SetData(LeanLocalization.GetTranslationText("lock_title", "Feature Locked"), string.Format(LeanLocalization.GetTranslationText("unlock_feature", "Feature unlocks at Level {0}"), UserProfileManager.Instance.GetLevelUnlockFunction(LockFunctionType.LineUp)));
            });
            return;
        }
        PopupManager.Instance.OnUI(PopupCode.LineUpUI, BattleTeamConfig.ArenaDefendTeamIndex);
    }
    int GetIndexRank(string rank)
    {
        if (rank.Contains("Bronze"))
        {
            return 0;
        }
        if (rank.Contains("Silver"))
        {
            return 1;
        }
        if (rank.Contains("Gold"))
        {
            return 2;
        }
        if (rank.Contains("Platinum"))
        {
            return 3;
        }
        return 0;
    }
}
