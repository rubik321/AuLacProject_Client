using Lean.Localization;
using NTPackage.Functions;
using NTPackage.UI;
using Rubik.Common.AudioHelper;
using Rubik.MsgDelivery;
using Rubik.Myrk.Clan;
using Rubik.PlayerMail;
using Rubik.Quest;
using Rubik.UI;
using Rubik.UserDataPlayer;
using Rubik.UserProfile;
using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Data;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MemberClanUI : PopupUI
{
   
    public MemberClanItem memberPre,memberRequest;
    public Transform memberHolder;
    List<MemberClanItem> lsMembers = new List<MemberClanItem>();
    public Image frameImg, colorImg, iconImg, iconImg1;
    public AvatarPlayerUI avatar;
    public TextMeshProUGUI NameTxt, lvTxt, pointTxt;
    public Sprite bgOn, bgOff;
    public GameObject removeMemGo,requestGo,memberInfoGo,removeClanGO,noRequest,infoMember,avaInfo;
    public Button btnPromote,btnDemote,btnLeader;
    public Image[] tabsImg;
    public Sprite tabOn, tabOff;
    [SerializeField] ClanMemberInfo currentMemberInfo;
    int frameIndex = 0, colorIndex = 0, iconIndex = 0;
    public TextMeshProUGUI  nameClanTxt, desClanTxt,memberRoleTxt;
    public GameObject autoOn, autoOff;

   
    public override void OnUI(object data = null, bool isDefaultSound = true)
    {
        base.OnUI(data, isDefaultSound);
        thisClanId = null;
        ShowMember(0,data);
    }
    
    public void Tabs(int index)
    {
        foreach (Image img in tabsImg)
        {
            img.sprite = tabOff;
        }
        tabsImg[index].sprite = tabOn;
        switch (index)
        {
            case 0:
                ShowMember(0, thisClanId);
                break;
            case 1:
                ShowMemberRequest();
                break;
            case 2:
               
                break;
        }
        AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
    }
    ClanInfo thisClanId = null;
    public void OnShowMember()
    {
        gameObject.SetActive(true);
        thisClanId = null;
        ShowMember(0, null);
    }
    public void OnShowRequest()
    {
        gameObject.SetActive(true);
        thisClanId = null;
        ShowMemberRequest();
    }
    public void ShowMember(int index  =-1, object clanID = null)
    {
        string idCLan;
        infoMember.SetActive(true);
        avaInfo.SetActive(true);
        if (clanID==null)
        {
            idCLan = ClanManager.Instance.PlayerClan._id;

        }
        else
        {
            thisClanId = (ClanInfo)clanID;
            idCLan = thisClanId._id;
        }
        noRequest.SetActive(false);
        if (index > 0)
        {
            currentIndex = index;
        }
        if (lsMembers.Count > 0)
        {
            for(int i = 0; i < lsMembers.Count; i++)
            {
                Destroy(lsMembers[i].gameObject);
            }
            lsMembers.Clear();
        }
        
        //requestGo.SetActive(false);
        memberInfoGo.SetActive(idCLan == ClanManager.Instance.PlayerClan._id);
        if (ClanManager.Instance.PlayerUserClan.Role == 0)
        {
            requestGo.SetActive(false);
        }
        
        ClanManager.Instance.GetClanMemberInfo(idCLan, (members)=> {
            //var member = ClanManager.Instance.GetClanMemberInfo();
            if (clanID == null)
            {
                SetInfoClan(ClanManager.Instance.PlayerClan);

            }
            else
            {
                SetInfoClanInfo(thisClanId);
            }
           
            //StartCoroutine(ClanManager.Instance.IEGetPlayerClan(ClanManager.Instance.ClanMemberList.Get(clanID).MemberList[0].UserId, (clan) =>
            //{
            //    SetInfoClan(clan);

            //}));
            Showmembers(members);
        });
    }

    public void ShowMemberRequest()
    {
       
        if (lsMembers.Count > 0)
        {
            for (int i = 0; i < lsMembers.Count; i++)
            {
                Destroy(lsMembers[i].gameObject);
            }
            lsMembers.Clear();
        }
        ClanManager.Instance.GetClanRequestList( (userCLans) => {

            ShowMembersRequest(userCLans);
        });
    }
    int currentIndex = 0;
    public void Showmembers(Rubik.Myrk.Clan.ClanMemberInfo[] members)
    {
        // To do :
        List<string> listUserId = new List<string>();
        foreach(ClanMemberInfo info in members)
        {
            listUserId.Add(info.UserId);
        }
        MsgDeliveryRoom.Instance.CheckUserOnline(listUserId);
        
        int index = 0;
        if(ClanManager.Instance.PlayerUserClan.Role == ClanRole.LEADER)
        {
            removeClanGO.SetActive(true);
        }
        else
        {
            removeClanGO.SetActive(false);
        }
        foreach(ClanMemberInfo info in members )
        {
            MemberClanItem member = Instantiate(memberPre);
            member.transform.SetParent(memberHolder, false);
            member.SetMemberInfo(info);
            lsMembers.Add(member);
            member.bgImg.sprite = bgOff;
            int tempMem = index;
            member.GetComponent<Button>().onClick.AddListener(()=> {
                currentIndex = tempMem;
                SetMemberInfo(info);
                for (int i = 0; i < lsMembers.Count; i++)
                {
                    lsMembers[i].bgImg.sprite = bgOff;
                }
                member.bgImg.sprite = bgOn;
                if ((int)ClanManager.Instance.PlayerUserClan.Role >= 2 && ClanManager.Instance.PlayerUserClan.UserId != info.UserId)
                {

                    if ((int)ClanManager.Instance.PlayerUserClan.Role > (int)info.Role)
                    {
                        removeMemGo.SetActive(true);
                        btnPromote.gameObject.SetActive(true);
                        btnDemote.gameObject.SetActive(true);
                    }
                    else
                    {
                        removeMemGo.SetActive(false);
                        btnPromote.gameObject.SetActive(false);
                        btnDemote.gameObject.SetActive(false);
                    }
                }
                else { 
                    removeMemGo.SetActive(false);
                    btnPromote.gameObject.SetActive(false);
                    btnDemote.gameObject.SetActive(false);
                }
                
                int indexBtnRole = 0;
               
                if(ClanManager.Instance.PlayerUserClan.Role==  ClanRole.LEADER && info.Role == ClanRole.COLEADER)
                {
                    btnPromote.gameObject.SetActive(false);
                    btnLeader.gameObject.SetActive(true);
                }
                else
                {
                    btnLeader.gameObject.SetActive(false);
                }

                if (ClanManager.Instance.PlayerUserClan.Role == ClanRole.MEMBER|| info.Role == ClanRole.LEADER|| ClanManager.Instance.PlayerUserClan.UserId == info.UserId || ClanManager.Instance.PlayerUserClan.Role == ClanRole.CAPTAIN)
                {
                    btnPromote.gameObject.SetActive(false);
                    btnDemote.gameObject.SetActive(false);
                    btnLeader.gameObject.SetActive(false);
                }

            });
            index++;
        }

        lsMembers[currentIndex].GetComponent<Button>().onClick.Invoke();
    }
    public void ShowMembersRequest(ClanMemberInfo[] members)
    {
        if (members != null && members.Length > 0)
        {
            foreach (ClanMemberInfo info in members)
            {
                MemberClanItem member = Instantiate(memberRequest);
                member.transform.SetParent(memberHolder, false);
                member.SetMemberInfo(info);
                lsMembers.Add(member);
                member.bgImg.sprite = bgOff;
                member.GetComponent<Button>().onClick.AddListener(() => {
                    SetMemberInfo(info);
                    for (int i = 0; i < lsMembers.Count; i++)
                    {
                        lsMembers[i].bgImg.sprite = bgOff;
                    }
                    member.bgImg.sprite = bgOn;

                });

            }
            infoMember.SetActive(false);
            avaInfo.SetActive(true);
            lsMembers[0].GetComponent<Button>().onClick.Invoke();
            noRequest.SetActive(false);
        }
        else
        {
            avaInfo.SetActive(false);
            noRequest.SetActive(true);
            infoMember.SetActive(false);
            //HUDCanvas.Instance.ShowNotification("No request  ! ", "Message", null);
            // ShowMember();
        }
       
    }
    public void SetMemberInfo(ClanMemberInfo member)
    {

        currentMemberInfo = member;
        avatar.SetData(member.Avatar, member.AvatarBorder);
        NameTxt.text = member.DisplayName;
        
        lvTxt.text = ( member.Level+1).ToString();
        pointTxt.text = member.FundDonate.ToString();
       memberRoleTxt.text = member.Role.ToString() ;
        switch (member.Role)
        {
            case ClanRole.MEMBER:
                memberRoleTxt.color = Color.white;  
                break;
            case ClanRole.CAPTAIN:
                memberRoleTxt.color = Color.green;
                break;
            case ClanRole.COLEADER:
                memberRoleTxt.color = Color.blue;
                break;
            case ClanRole.LEADER:
                memberRoleTxt.color = Color.red;
                break;
        }

    }
    public void SetRole(int role)
    {
        AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
        //Debug.LogError(JsonUtility.ToJson(currentMemberInfo));
        //Debug.LogError(currentMemberInfo.UserId);
        StartCoroutine(ClanManager.Instance.IEPromoteClanMember(currentMemberInfo.UserId, (ClanRole)role,()=>{
            ShowMember();
        }));
    }
    public void Promote()
    {
        AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
        if (ClanManager.Instance.PlayerUserClan.Role == ClanRole.MEMBER|| ClanManager.Instance.PlayerUserClan.Role == ClanRole.CAPTAIN)
        {
            HUDCanvas.Instance.ShowNotification(LeanLocalization.GetTranslationText("clan_noti_clan_19"), LeanLocalization.GetTranslationText("clan_message"), null);
            return;
            
        }else if (ClanManager.Instance.PlayerUserClan.Role <= currentMemberInfo.Role)
        {
            HUDCanvas.Instance.ShowNotification(LeanLocalization.GetTranslationText("clan_noti_clan_20"), LeanLocalization.GetTranslationText("clan_message"), null);
            return;
        }
        StartCoroutine(ClanManager.Instance.IEPromoteClanMember(currentMemberInfo.UserId, (ClanRole)((int)currentMemberInfo.Role+1), () => {
            ShowMember();
        }));
    }
    public void Deomote()
    {
        AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
        if (ClanManager.Instance.PlayerUserClan.Role == ClanRole.MEMBER || ClanManager.Instance.PlayerUserClan.Role == ClanRole.CAPTAIN)
        {
            HUDCanvas.Instance.ShowNotification(LeanLocalization.GetTranslationText("clan_noti_clan_19"), LeanLocalization.GetTranslationText("clan_message"), null);
            return;

        }
        else if (ClanManager.Instance.PlayerUserClan.Role <= currentMemberInfo.Role)
        {
            HUDCanvas.Instance.ShowNotification(LeanLocalization.GetTranslationText("clan_noti_clan_20"), LeanLocalization.GetTranslationText("clan_message"), null);
            return;
        }else if(currentMemberInfo.Role == ClanRole.MEMBER)
        {
            HUDCanvas.Instance.ShowNotification(LeanLocalization.GetTranslationText("clan_noti_clan_18"), LeanLocalization.GetTranslationText("clan_message"), null);
            return;
        }
        StartCoroutine(ClanManager.Instance.IEPromoteClanMember(currentMemberInfo.UserId, (ClanRole)((int)currentMemberInfo.Role - 1), () => {
            ShowMember();
        }));
    }
    public void ChangeLeader()
    {
        AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
        if (ClanManager.Instance.PlayerUserClan.Role == ClanRole.LEADER)
        {
            PopupManager.Instance.OnUI(PopupCode.MessageOptionPanel, null, (popupUI) =>
            {
                MessageOptionPanel messageOptionPanel = popupUI as MessageOptionPanel;
                messageOptionPanel.SetData(Lean.Localization.LeanLocalization.GetTranslationText("clan_message"), Lean.Localization.LeanLocalization.GetTranslationText("clan_noti_clan_17"));

                messageOptionPanel.SetActionConfirm(() =>
                {
                    popupUI.OffUI();
                    StartCoroutine(ClanManager.Instance.IEChangeClanLeader(UserDataManager.Instance.GetUserID(), ClanManager.Instance.PlayerClan._id, currentMemberInfo.UserId, (UserClan) => {
                        // OffUI();
                        // PopupManager.Instance.OnUI(PopupCode.ClanUI);
                        ShowMember();
                    }));
                }, Lean.Localization.LeanLocalization.GetTranslationText("btn_confirm", "Confirm"));
                messageOptionPanel.SetActionReject(() =>
                {
                    popupUI.OffUI();
                }, Lean.Localization.LeanLocalization.GetTranslationText("btn_cancel", "Cancel"));
                messageOptionPanel.SetTimeConfirm();
            });

        }
        else
        {
            HUDCanvas.Instance.ShowNotification(LeanLocalization.GetTranslationText("clan_noti_cant_action"), LeanLocalization.GetTranslationText("clan_message"), null);
        }
    }
    public void RemoveMember()
    {
        AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
        if (ClanManager.Instance.PlayerUserClan.Role == ClanRole.LEADER|| ClanManager.Instance.PlayerUserClan.Role == ClanRole.COLEADER)
        {
            HUDCanvas.Instance.ShowNotification(LeanLocalization.GetTranslationText("clan_noti_clan_10"), LeanLocalization.GetTranslationText("clan_message"), null,()=> {
                StartCoroutine(ClanManager.Instance.IERemoveClanMember(ClanManager.Instance.PlayerClan._id, currentMemberInfo.UserId, (user) => {
                    ShowMember();
                    btnDemote.gameObject.SetActive(false);
                    btnDemote.gameObject.SetActive(false);
                   
                }));
            });
        }
        else
        {
            HUDCanvas.Instance.ShowNotification(LeanLocalization.GetTranslationText("clan_noti_cant_action"), LeanLocalization.GetTranslationText("clan_message"), null);
        }
        
    }
    public void RemoveClan()
    {
        AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
        if (ClanManager.Instance.PlayerUserClan.Role == ClanRole.LEADER)
        {
            PopupManager.Instance.OnUI(PopupCode.MessageOptionPanel, null, (popupUI) =>
            {
                MessageOptionPanel messageOptionPanel = popupUI as MessageOptionPanel;
                messageOptionPanel.SetData(Lean.Localization.LeanLocalization.GetTranslationText("clan_message"), Lean.Localization.LeanLocalization.GetTranslationText("clan_noti_clan_11"));
                
                messageOptionPanel.SetActionConfirm(() =>
                {
                    popupUI.OffUI();
                    StartCoroutine(ClanManager.Instance.IEDeleteClan(ClanManager.Instance.PlayerClan._id, () => {
                       // OffUI();
                        PopupManager.Instance.OnUI(PopupCode.ClanUI);

                    }));
                }, Lean.Localization.LeanLocalization.GetTranslationText("btn_confirm", "Confirm"));
                messageOptionPanel.SetActionReject(() =>
                {
                    popupUI.OffUI();
                }, Lean.Localization.LeanLocalization.GetTranslationText("btn_cancel", "Cancel"));
                messageOptionPanel.SetTimeConfirm();
            });
           
        }
        else
        {
            HUDCanvas.Instance.ShowNotification(LeanLocalization.GetTranslationText("clan_noti_cant_action"), LeanLocalization.GetTranslationText("clan_message"), null);
        }

    }
    public void LeaveClann()
    {
        AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
        if (ClanManager.Instance.PlayerUserClan.Role != ClanRole.LEADER)
        {
            PopupManager.Instance.OnUI(PopupCode.MessageOptionPanel, null, (popupUI) =>
            {
                MessageOptionPanel messageOptionPanel = popupUI as MessageOptionPanel;
                messageOptionPanel.SetData(LeanLocalization.GetTranslationText("clan_message"),LeanLocalization.GetTranslationText("clan_noti_clan_12"));

                messageOptionPanel.SetActionConfirm(() =>
                {
                    popupUI.OffUI();
                    StartCoroutine(ClanManager.Instance.IELeaveClanRequest(ClanManager.Instance.PlayerClan._id, () => {
                       // OffUI();
                        PopupManager.Instance.OnUI(PopupCode.ClanUI);
                    }));
                }, Lean.Localization.LeanLocalization.GetTranslationText("btn_confirm", "Confirm"));
                messageOptionPanel.SetActionReject(() =>
                {
                    popupUI.OffUI();
                }, Lean.Localization.LeanLocalization.GetTranslationText("btn_cancel", "Cancel"));
                messageOptionPanel.SetTimeConfirm();
            });
           
        }
        else
        {
            HUDCanvas.Instance.ShowNotification(LeanLocalization.GetTranslationText("clan_noti_clan_13"), LeanLocalization.GetTranslationText("clan_message"), null);
        }

    }
    public void AddPromote()
    {

    }
    public void SetColor(int indexIcon)
    {
        colorImg.sprite = SpriteHelper.Instance.color[indexIcon];
        colorImg.SetNativeSize();
        colorIndex = indexIcon;
    }
    public void SetIcon(int indexIcon)
    {
        iconImg.sprite = SpriteHelper.Instance.icons[indexIcon];
        iconImg1.sprite = SpriteHelper.Instance.icons[indexIcon];
        iconImg.SetNativeSize();
        iconIndex = indexIcon;
    }
    public void SetFrame(int indexIcon)
    {
        frameImg.sprite = SpriteHelper.Instance.patterm[indexIcon];
        frameImg.SetNativeSize();
        frameIndex = indexIcon;
    }
    public void SetInfoClan(Clan clan)
    {

        SetIcon(clan.Icon);
        SetColor(clan.Color);
        SetFrame(clan.Frame);
        nameClanTxt.text = clan.Name;
        desClanTxt.text = clan.Slogan;
        SetAuto(clan.AutoAcceptMember);
    }
    public void SetInfoClanInfo(ClanInfo clan)
    {

        SetIcon(clan.Icon);
        SetColor(clan.Color);
        SetFrame(clan.Frame);
        nameClanTxt.text = clan.Name;
        desClanTxt.text = clan.Slogan;
        //SetAuto(clan.AutoAcceptMember);
    }
    bool Auto;
    public void SetAuto(bool isAuto)
    {
        Auto = isAuto;
        autoOff.SetActive(!isAuto);
        autoOn.SetActive(isAuto);
    }
}
