using Rubik.Myrk.Clan;
using UnityEngine;
using UnityEngine.UI;

public class ClanInfoItem : MonoBehaviour
{
    public Image frameImg, colorImg, iconImg;
    public void SetColor(int indexIcon)
    {
        colorImg.sprite = SpriteHelper.Instance.color[indexIcon];
        colorImg.SetNativeSize();
       
    }
    public void SetIcon(int indexIcon)
    {
        iconImg.sprite = SpriteHelper.Instance.icons[indexIcon];
       
        iconImg.SetNativeSize();
     
    }
    public void SetFrame(int indexIcon)
    {
        frameImg.sprite = SpriteHelper.Instance.patterm[indexIcon];
        frameImg.SetNativeSize();
       
    }
    public void SetInfoClan(Clan clan)
    {

        SetIcon(clan.Icon);
        SetColor(clan.Color);
        SetFrame(clan.Frame);
       
    }
    public void SetInfoClanInfo(ClanInfo clan)
    {

        SetIcon(clan.Icon);
        SetColor(clan.Color);
        SetFrame(clan.Frame);
       
        //SetAuto(clan.AutoAcceptMember);
    }
}
