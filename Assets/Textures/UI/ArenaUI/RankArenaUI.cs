using Lean.Localization;
using NTPackage.Functions;
using Poly2Tri;
using Rubik.Manager;
using Rubik.Myrk.Arena;
using Rubik.Myrk.BattleTeam;
using Rubik.UserDataPlayer;
using Rubik.UserProfile;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankArenaUI : MonoBehaviour
{
    public RankArenaItem rankItem;
    public Sprite[] lsAvaRanks,lsIcons;
    public Transform content;
    public AvatarPlayerUI avatarPlayer;
    public TextMeshProUGUI namePlayerTxt, pointTxt, powerTxt, rankTxt, timeLeft;
    public Image rankIcon;
    public long countDown;
    Coroutine coroutineCountDown;
    private void OnEnable()
    {
        SetData();
    }
    void SetData()
    {

        var ArenaResponse = ArenaManager.Instance.RankingData;
        int index = 0;
        ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.content);
        foreach (var member in ArenaResponse)
        {
            RankArenaItem arenaItem = ObjectPoolingManager.Instance.InstantiateObject<RankArenaItem>(ObjectPoolingConfig.ArenaResultItem, this.rankItem.transform);
            arenaItem.SetUp(member, lsAvaRanks[GetIndexRank(member.Type.ToString())],lsIcons[GetIndexRank(member.Type.ToString())]);
            arenaItem.transform.SetParent(this.content);
            NTFunction.ResetPosition(arenaItem.transform);
            index++;
        }
        this.avatarPlayer.SetData(UserProfileManager.Instance.AvatarPlayer.Current, UserProfileManager.Instance.AvatarBorderPlayer.Current);
        namePlayerTxt.text = UserDataManager.Instance.UserData.DisplayName;
        powerTxt.text = BattleTeamManager.Instance.GetPower(BattleTeamManager.Instance.GetBattleTeamDataSelected().ToShortTeam()).ToString();
        pointTxt.text = ArenaManager.Instance.ArenaResponse.Score.ToString();
        GetIndexRank(ArenaManager.Instance.GetRankingTypeByScore(ArenaManager.Instance.ArenaResponse.Score).ToString());
        rankTxt.text = ArenaManager.Instance.GetRankingTypeByScore(ArenaManager.Instance.ArenaResponse.Score).ToString();
        rankIcon.sprite = lsIcons[GetIndexRank(ArenaManager.Instance.GetRankingTypeByScore(ArenaManager.Instance.ArenaResponse.Score).ToString())];
        this.countDown = ServerManager.Instance.GetTimeNewWeek();
        if (this.coroutineCountDown != null)
        {
            StopCoroutine(this.coroutineCountDown);
        }
        this.coroutineCountDown = StartCoroutine(this.CotimeLeft());
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
    IEnumerator CotimeLeft()
    {
        while (true)
        {
            this.countDown = ServerManager.Instance.GetTimeNewWeek();
            timeLeft.text = LeanLocalization.GetTranslationText("weekly_time_left", "Weekly Arena Rewards in ") + ": <color=#BD7E92>" + NTFunction.Format_Time(this.countDown,4) + "</color>";
            yield return new WaitForSeconds(1);
            if (countDown < 1)
            {
                //SetTime();
                yield break;
            }

        }
    }
}
