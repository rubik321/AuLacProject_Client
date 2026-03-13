using NTPackage.UI;
using Poly2Tri;
using Rubik.Myrk.Clan;
using Rubik.UserDataPlayer;
using Rubik.UserProfile;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Mapbox.VectorTile.Geometry.InteralClipperLib.InternalClipper;

public class ClanRankUI : MonoBehaviour
{
    public ClanRankItem memberPre;
    public Transform memberHolder;
    List<ClanRankItem> lsMembers = new List<ClanRankItem>();

    public AvatarPlayerUI avatar;
    public TextMeshProUGUI NameTxt, lvTxt;
    public Sprite bgOn, bgOff;
    public GameObject joinGo, joinedGo, memberGo;
    [SerializeField] Sprite[] rankSprs;
    [SerializeField] Sprite[] rankBGSprs;
    [SerializeField] ClanInfo currentClanInfo;
    public void ShowRankMember()
    {
        indexCurrnet = 0;
        if (lsMembers.Count > 0)
        {
            for (int i = 0; i < lsMembers.Count; i++)
            {
                Destroy(lsMembers[i].gameObject);
            }
            lsMembers.Clear();
        }
        joinGo.SetActive(false);
        joinedGo.SetActive(false);
        memberGo.SetActive(false);
        Showmembers();
        //StartCoroutine(ClanManager.Instance.IEGetListClanMemberInfo(ClanManager.Instance.PlayerClan._id, () => {
        //    Showmembers();
        //}));
    }
    int indexCurrnet = 0;
    public void Showmembers()
    {
       
        ClanManager.Instance.GetTopClanList((clans) =>
        {

            int temp = 0;
            memberHolder.GetComponentInParent<ScrollRect>().vertical = false;
            memberHolder.transform.localPosition = Vector3.zero;
           
          
            foreach (ClanInfo info in clans)
            {
                ClanRankItem member = Instantiate(memberPre);
                member.transform.SetParent(memberHolder, false);

                member.SetClanInfo(info, true);
                lsMembers.Add(member);
                if (temp < 3)
                {
                    member.rankImg.sprite = rankSprs[temp];
                    member.rankTxt.text = "";
                    member.bgImg.sprite = rankBGSprs[temp];
                }
                else
                {
                    member.rankImg.gameObject.SetActive(false);
                    member.rankTxt.text = (temp + 1).ToString();
                    member.bgImg.sprite = rankBGSprs[3];
                }

                int indexTemp = temp;

                member.GetComponent<Button>().onClick.AddListener(() =>
                {
                    SetMemberInfo(info);
                    indexCurrnet = temp;
                    currentClanInfo = info;
                    for (int i = 0; i < lsMembers.Count; i++)
                    {
                        //lsMembers[i].bgImg.sprite = bgOff;
                    }
                    // member.bgImg.sprite = bgOn;

                });
                temp++;

            };
            Invoke("ActiveScroll", 0.5f);
            lsMembers[indexCurrnet].GetComponent<Button>().onClick.Invoke();
           

        });
       



    }
    void ActiveScroll()
    {
        memberHolder.GetComponentInParent<ScrollRect>().vertical = true;
    }
    public void SetMemberInfo(ClanInfo member)
    {
        UserProfileManager.Instance.GetUserDataShort(member.ChiefID, (user) =>
        {
           
          
            GetComponentInParent<ClanUI>().SetInfoClanInfo(member);
           
            avatar.SetData(user.Avatar, user.AvatarBorder);
            NameTxt.text = user.DisplayName;
            lvTxt.text = user.Level.ToString();
        });
        if (ClanManager.Instance.PlayerUserClan != null && !string.IsNullOrEmpty(ClanManager.Instance.PlayerUserClan.ClanId))
        {
            joinGo.gameObject.SetActive(false);
            joinedGo.SetActive(false);
            memberGo.SetActive(false);
        }
        else
        {
            joinGo.gameObject.SetActive(true);
            joinedGo.SetActive(false);
            memberGo.SetActive(false);
            if (ClanManager.Instance.IsPlayerClanRequest(member._id))
            {
                joinGo.gameObject.SetActive(false);
                joinedGo.SetActive(true);
                return;
            }
        }
        if(member.Member== member.MaxMember)
        {
            joinGo.gameObject.SetActive(false);
            joinedGo.SetActive(false);
            memberGo.SetActive(false);
        }
    }
    public void JoinClan()
    {
        StartCoroutine(ClanManager.Instance.IEJoinClanRequest(UserDataManager.Instance.GetUserID(), currentClanInfo._id, (user) =>
        {

            //ShowRankMember();
            if (ClanManager.Instance.IsPlayerClanRequest(currentClanInfo._id))
            {
                joinGo.gameObject.SetActive(false);
                joinedGo.SetActive(true);
              
            }
            GetComponentInParent<ClanUI>().SetTitle();
        if (ClanManager.Instance.IsClan()){
                PopupManager.Instance.OnUI(PopupCode.ClanUI);
            }
            //joinGo.gameObject.SetActive(false);
           // joinedGo.SetActive(true);

        }));
    }
    public void ShowMember()
    {
        PopupManager.Instance.OnUI(PopupCode.MemberClanUI, (object)currentClanInfo);
    }
}
