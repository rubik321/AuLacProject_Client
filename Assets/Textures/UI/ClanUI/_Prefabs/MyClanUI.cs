using NTPackage.Functions;
using NTPackage.UI;
using Rubik.Common.AudioHelper;
using Rubik.Myrk.Clan;
using Rubik.Quest;
using Rubik.UI;
using Rubik.UserDataPlayer;
using Rubik.UserProfile;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MyClanUI : MonoBehaviour
{
    public TextMeshProUGUI clanLvTxt,clanExpTxt,textPointTxt,txtMemberTxt,txtMessageClan;
    public Image expProcess;
    public bool Auto = false;
    public GameObject autoOn, autoOff;
    public TextMeshProUGUI txtTitle,txtChange;
    public ClanEditUI clanEdit;
    public AvatarPlayerUI avatar;
    public TextMeshProUGUI NameTxt, lvTxt;
    public void SetData(Clan PlayerClan)
    {
      
        clanLvTxt.text = (PlayerClan.Level+1).ToString();
        clanExpTxt.text = PlayerClan.Exp + "/" + PlayerClan.ExpNextLevel;
        expProcess.fillAmount = (float)PlayerClan.Exp / PlayerClan.ExpNextLevel;
        textPointTxt.text = PlayerClan.Fund.ToString();
        txtMemberTxt.text = PlayerClan.Member + "/" + PlayerClan.MaxMember;
        if (string.IsNullOrEmpty(PlayerClan.Announce))
        {
            txtMessageClan.text = "Hello everyone !";
        }
        else
        {
            txtMessageClan.text = PlayerClan.Announce;
        }
        UserProfileManager.Instance.GetUserDataShort(PlayerClan.ChiefId, (user) =>
        {
            avatar.SetData(user.Avatar, user.AvatarBorder);
            NameTxt.text = user.DisplayName;
            lvTxt.text = (user.Level+1).ToString();
        });
    }
    int index;
    public void SetEdit(int index)
    {
        AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
        clanEdit.ShowEdit(index) ;
    }
   
    public void ShowClanBossUI()
    {
        if (ClanManager.Instance.PlayerUserClan == null || string.IsNullOrEmpty(ClanManager.Instance.PlayerUserClan.ClanId))
        {
            HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("clan_noti_clan_08"), "Message", null);
        }
        else
            PopupManager.Instance.OnUI(PopupCode.ClanBossUI);
    }
   
    public void ShowClanShopUI()
    {
        PopupManager.Instance.OnUI(PopupCode.ClanShopUI);
    }
}
