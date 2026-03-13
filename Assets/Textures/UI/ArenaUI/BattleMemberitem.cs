using NTPackage.Functions;
using NTPackage.UI;
using Rubik.BattleEngine;
using Rubik.Config;
using Rubik.ItemPlayer;
using Rubik.Myrk.Arena;
using Rubik.Myrk.Battle;
using Rubik.Myrk.BattleTeam;
using Rubik.Myrk.Clan;
using Rubik.UserProfile;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleMemberitem : MonoBehaviour
{
    public AvatarPlayerUI avatarPlayer;
    public TextMeshProUGUI namePlayerTxt, pointTxt, powerTxt;
    public ItemDataUI[] lsMonsters;
    OpponentData memberData;
    public Button battlebutton;
    public Sprite winSpr, normalSpr;
    public void SetUp(OpponentData data)
    {
        memberData = data;
        pointTxt.text = data.Score.ToString();
        namePlayerTxt.text = data.UserData.DisplayName.ToString();
        if (data.UserData.BattleTeam.Cards != null)
        {
            powerTxt.text = BattleTeamManager.Instance.GetPower(data.UserData.BattleTeam).ToString();
            int index = 0;
            foreach (var item in data.UserData.BattleTeam.Cards)
            {
                lsMonsters[index].SetData(item);
                index++;
            }
        }
           
       
        if (data.IsWin)
        {
            GetComponent<Image>().sprite = winSpr;
            battlebutton.interactable = false;
        }
        else
        {
            battlebutton.interactable = true;
            GetComponent<Image>().sprite = normalSpr;
        }
        avatarPlayer.SetData(data.UserData.Avatar, data.UserData.AvatarBorder);
    }
    public void GetInfo()
    {
        PopupManager.Instance.GetPopupUI(PopupCode.LineUpHeroUI).GetComponent<LineupHeroUI>().indexAttack = 1;
        PopupManager.Instance.OnUI(PopupCode.LineUpHeroUI, memberData);
       
    }
    public void Attack()
    {
        PopupManager.Instance.GetPopupUI(PopupCode.LineUpHeroUI).GetComponent<LineupHeroUI>().indexAttack = 1;
        PopupManager.Instance.OnUI(PopupCode.LineUpHeroUI, memberData);
      

    }
}
