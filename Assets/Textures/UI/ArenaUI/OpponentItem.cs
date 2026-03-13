using NTPackage.UI;
using Poly2Tri;
using Rubik.ItemPlayer;
using Rubik.Myrk.Arena;
using Rubik.Myrk.BattleTeam;
using Rubik.UserProfile;
using TMPro;
using UnityEngine;

public class OpponentItem : MonoBehaviour
{
    public AvatarPlayerUI avatarPlayer;
    public TextMeshProUGUI namePlayerTxt, pointTxt, powerTxt;
    OpponentData atkData;
   
    public void SetUp(OpponentData data,long plusPoint)
    {
        avatarPlayer.SetData(data.UserData.Avatar, data.UserData.AvatarBorder);
        this.atkData = data;
        if (plusPoint > 0)
        {
            string temp1 = "{+" + plusPoint + "}";
            pointTxt.text = data.Score.ToString() + string.Format("<color=green>{0}</color>", temp1);
        }

        else
        {
            string temp1 = "{" + plusPoint + "}";
            pointTxt.text = data.Score.ToString() + string.Format("<color=red>{0}</color>", temp1) ;
        }
            
        namePlayerTxt.text = data.UserData.DisplayName.ToString();
        powerTxt.text = BattleTeamManager.Instance.GetPower(data.UserData.BattleTeam).ToString();

    }
    public void GetInfoAtk()
    {
        PopupManager.Instance.GetPopupUI(PopupCode.LineUpHeroUI).GetComponent<LineupHeroUI>().indexAttack = -1;
        PopupManager.Instance.OnUI(PopupCode.LineUpHeroUI, atkData);
       

    }
}
