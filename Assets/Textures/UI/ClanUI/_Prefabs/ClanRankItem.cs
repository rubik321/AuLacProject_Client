using Rubik.Myrk.Clan;
using Rubik.UserProfile;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClanRankItem : MonoBehaviour
{
    public Image rankImg;
    public TextMeshProUGUI NameTxt, typeTxtTxt, pointTxt, memberTxt,rankTxt;
    public Image bgImg;
    public ClanInfoItem clanInfoItem;

    public void SetClanInfo(ClanInfo member,bool isShowRank = true)
    {
       
        NameTxt.text = member.Name;
       // typeTxtTxt.text = member.Role.ToString();
        //lvTxt.text = "Lv." + member.Level.ToString();
        pointTxt.text = member.Fund.ToString();
        memberTxt.text = member.Member+"/" + member.MaxMember;
        clanInfoItem.SetInfoClanInfo(member);
        rankImg.gameObject.SetActive(isShowRank);
        if (member.AutoAccept)
        {
            typeTxtTxt.text = "Public";
            typeTxtTxt.color = Color.green;
        }
        else
        {
            typeTxtTxt.text = "Private";
            typeTxtTxt.color = Color.red;
        }

    }
}
