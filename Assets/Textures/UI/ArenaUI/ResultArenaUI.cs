using NTPackage.Functions;
using Rubik.Myrk.Arena;
using Rubik.Myrk.BattleTeam;
using Rubik.Myrk.Monster;
using Rubik.UserDataPlayer;
using Rubik.UserProfile;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultArenaUI : MonoBehaviour
{
    public ArenaResultItem itemPre;
    public List<ArenaResultItem> lsMembers = new List<ArenaResultItem>();
    public AvatarPlayerUI avatarPlayer;
    public TextMeshProUGUI namePlayerTxt, pointTxt, powerTxt, rankTxt;
    public Transform content;
    public Sprite[] lsIcons;
    public Image rankIcon;
    private void OnEnable()
    {
        SetData();
    }
    void SetData()
    {
       
        var ArenaResponse = ArenaManager.Instance.ArenaResponse;
        int index = 0;
        ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.content);
        lsMembers.Clear();
        foreach (var member in ArenaResponse.History)
        {
            ArenaResultItem arenaItem = ObjectPoolingManager.Instance.InstantiateObject<ArenaResultItem>(ObjectPoolingConfig.ArenaResultItem, this.itemPre.transform);
            arenaItem.SetUp(member);
            arenaItem.transform.SetParent(this.content);
            NTFunction.ResetPosition(arenaItem.transform);
            index++;
        }
        this.avatarPlayer.SetData(UserProfileManager.Instance.AvatarPlayer.Current, UserProfileManager.Instance.AvatarBorderPlayer.Current);
        namePlayerTxt.text = UserDataManager.Instance.UserData.DisplayName;
        pointTxt.text = ArenaResponse.Score.ToString();
        powerTxt.text = BattleTeamManager.Instance.GetPower(BattleTeamManager.Instance.GetBattleTeamDataSelected().ToShortTeam()).ToString();
        rankIcon.sprite = lsIcons[GetIndexRank(ArenaManager.Instance.GetRankingTypeByScore(ArenaResponse.Score).ToString())];
        rankTxt.text = ArenaManager.Instance.GetRankingTypeByScore(ArenaManager.Instance.ArenaResponse.Score).ToString();
    }
    int GetIndexRank(string rank)
    {
        if (rank.Contains("Bronze"))
        {
            rankTxt.color = Color.brown;
            return 0;
        }
        if (rank.Contains("Silver"))
        {
            rankTxt.color = Color.white;
            return 1;
        }
        if (rank.Contains("Gold"))
        {
            rankTxt.color = Color.yellow;
            return 2;
        }
        if (rank.Contains("Platinum"))
        {
            rankTxt.color = Color.pink;
            return 3;
        }
        return 0;
    }
}
