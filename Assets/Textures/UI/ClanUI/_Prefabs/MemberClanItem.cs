using Rubik.UserProfile;
using UnityEngine;
using TMPro;
using Rubik.Myrk.Clan;
using UnityEngine.UI;
using Rubik.UserDataPlayer;
using System.Collections;
using Rubik.MsgDelivery;
using Rubik.Manager;
public class MemberClanItem : MonoBehaviour
{
    public AvatarPlayerUI avatar;
    public TextMeshProUGUI NameTxt, roleTxt, lvTxt, pointTxt, statusTxt;
    public Image bgImg;
    [SerializeField] private ClanMemberInfo currentInfo;
    public void SetMemberInfo(ClanMemberInfo member)
    {
        avatar.SetData(member.Avatar, member.AvatarBorder);
        NameTxt.text = member.DisplayName;
        roleTxt.text = member.Role.ToString();
        lvTxt.text = "Lv."+(member.Level+1).ToString();
        pointTxt.text = member.FundDonate.ToString();
        StartCoroutine(this.UpdateOnlineStatus());
        currentInfo = member;
        switch (member.Role)
        {
            case ClanRole.MEMBER:
                roleTxt.color = Color.white;
                break;
            case ClanRole.CAPTAIN:
                roleTxt.color = Color.green;
                break;
            case ClanRole.COLEADER:
                roleTxt.text = "CO-LEADER";
                roleTxt.color = Color.blue;
                break;
            case ClanRole.LEADER:
                roleTxt.color = Color.red;
                break;
        }

    }
    public void Accept()
    {
        StartCoroutine(ClanManager.Instance.IEAcceptClanRequest(ClanManager.Instance.PlayerClan._id, currentInfo.UserId, () =>
        {
            GetComponentInParent<MemberClanUI>().ShowMember();
        }));
    }
    public void Reject()
    {
        StartCoroutine(ClanManager.Instance.IERejectClanRequest(ClanManager.Instance.PlayerClan._id, currentInfo.UserId, (user) =>
        {
            GetComponentInParent<MemberClanUI>().ShowMember();
        }));
    }

    public IEnumerator UpdateOnlineStatus()
        {
            if (MsgDeliveryRoom.Instance.GetUserOnlineStatus(this.currentInfo.UserId) == true)
            {
                this.statusTxt.text = Lean.Localization.LeanLocalization.GetTranslationText("online", "Online");
                this.statusTxt.color = Color.green;
            }else{
                this.statusTxt.text = Lean.Localization.LeanLocalization.GetTranslationText("offline", "Offline");
                this.statusTxt.color = Color.gray;
            }
            yield return new WaitForSeconds(0.5f);
            if (MsgDeliveryRoom.Instance.GetUserOnlineStatus(this.currentInfo.UserId) == true)
            {
                this.statusTxt.text = Lean.Localization.LeanLocalization.GetTranslationText("online", "Online");
                this.statusTxt.color = Color.green;
            }
            else
            {
                this.statusTxt.text = ServerManager.Instance.GetTimeOffline(this.currentInfo.LastLogin) + " " + Lean.Localization.LeanLocalization.GetTranslationText("ago", "ago");
                this.statusTxt.color = Color.gray;
            }
        }
}
