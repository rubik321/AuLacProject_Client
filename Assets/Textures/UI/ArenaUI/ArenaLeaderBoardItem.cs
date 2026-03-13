using NTPackage.UI;
using Rubik.Myrk.Arena;
using Rubik.Myrk.BattleTeam;
using Rubik.UserDataPlayer;
using Rubik.UserProfile;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArenaLeaderBoardItem : MonoBehaviour
{
    public AvatarPlayerUI avatarPlayer;
    public TextMeshProUGUI namePlayerTxt, pointTxt, powerTxt,rankTxt,rankNameTxt;
    public GameObject[] RankLists;
    //public Image icon;
    public Sprite[] rankSprs,lsIcons;
    [SerializeField]UserDataShort teamData;
    public void SetUp(UserRank rank, UserDataShort info)
    {
        teamData = info;
        this.avatarPlayer.SetData(info.Avatar, info.AvatarBorder);
        namePlayerTxt.text = info.DisplayName;
        pointTxt.text = rank.Score.ToString();
        if (rank.Rank < 3)
            return;
        powerTxt.text = BattleTeamManager.Instance.GetPower(info.BattleTeam).ToString();
        rankTxt.text = (rank.Rank+1).ToString();
        rankNameTxt.text = ArenaManager.Instance.GetRankingTypeByScore(rank.Score).ToString();
        RankLists[3].SetActive(true);
       // icon.sprite = lsIcons[GetIndexRank(ArenaManager.Instance.GetRankingTypeByScore(rank.Score).ToString())];
        //foreach(GameObject item in RankLists)
        //{
        //   item.SetActive(false);
        //}
        //if (rank.Rank < 3)
        //{
        //    RankLists[rank.Rank].SetActive(true);
        //    GetComponent<Image>().sprite = rankSprs[rank.Rank];
        //}
        //else
        //{
        //    RankLists[3].SetActive(true);
        //    GetComponent<Image>().sprite = rankSprs[3];
        //}

        // pointTxt.text = ArenaResponse.Score.ToString();
    }
    public void ButtonClick()
    {
        OpponentData memberData = new OpponentData();
        memberData.UserData = teamData;
        PopupManager.Instance.GetPopupUI(PopupCode.LineUpHeroUI).GetComponent<LineupHeroUI>().indexAttack = -1;
        PopupManager.Instance.OnUI(PopupCode.LineUpHeroUI, memberData);
       
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

 
