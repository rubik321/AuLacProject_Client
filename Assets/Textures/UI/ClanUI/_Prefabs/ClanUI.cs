using DG.Tweening;
using Lean.Localization;
using NTPackage.Functions;
using NTPackage.UI;
using Rubik.Common.AudioHelper;
using Rubik.ItemPlayer;
using Rubik.Myrk.Clan;
using Rubik.Quest;
using Rubik.UI;
using Rubik.UserDataPlayer;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClanUI  : PopupUI
{
   
    public IconClanItem[] lsIconsItem ;
    public Transform iconContent;
    public Image iconImg1;
    int frameIndex = 0, colorIndex = 0,iconIndex = 0;
    public TextMeshProUGUI nameTxt, desTxt, nameClanTxt, desClanTxt,myClanNameTxt,RankClanTxt,txtprice;
    public bool Auto = false;
    public GameObject autoOn, autoOff, autoOn1, autoOff1,myCLanGo;
    public GameObject creteClanGo, myClanGo,rankClan,clanInfo,donateGo,myClanInfo,rankTitleGO;
    public Image[] tabsImg;
    public Sprite tabOn,tabOff;
    public ClanInfoItem clanInfoItem;
    public GameObject[] lsColors, lsPattern;
    public GameObject tickColor, tickFrame, tickIcon,btnCancel,btnEditName,btnEditDes,btnEditMess;
    public DonateClanUI donateCLan;
    public MemberClanUI memberClan;
    public ScrollRect scrollRect;
    public TMPro.TMP_InputField nameInput, desInput;
    protected override void Start()
    {
        base.Start();
        int count = 0;

        foreach (Sprite spr in SpriteHelper.Instance.icons)
        {
            //IconClanItem itemIcon = Instantiate(this.itemIconPre, this.iconContent);
            //if (itemIcon == null)
            //{
            //    itemIcon = Instantiate(this.itemIconPre, this.iconContent);
            //}
            lsIconsItem[count].icon.sprite = spr;
            lsIconsItem[count].transform.SetParent(this.iconContent, false);
           
            int indexTemp = count;

            lsIconsItem[count].GetComponent<Button>().onClick.AddListener(() =>
            {

                SetIcon(indexTemp);
            });
            count++;
            //itemIcon.transform.name = ObjectPoolingConfig.ItemQuestUI;

        }
       
    }
    public override void OnUI(object data = null, bool isDefaultSound = true)
    {
        base.OnUI(data, isDefaultSound);
       
        isEdit = false;
        tabsImg[1].gameObject.SetActive(true);
        tabsImg[3].gameObject.SetActive(true);
        ClanTabs(0);
        myCLanGo.SetActive(true);
        RankClanTxt.text = LeanLocalization.GetTranslationText("clans");
        scrollRect.vertical = false;
        DOVirtual.DelayedCall(1, () =>
        {
            scrollRect.vertical = true;
            scrollRect.GetComponent<RectTransform>().anchoredPosition = new Vector2(scrollRect.GetComponent<RectTransform>().anchoredPosition.x, 0);
        });
        
    }
    public override void ScriptOffUI()
    {
        base.ScriptOffUI();
      //  ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.iconContent);
    }
    bool isStart = false;
    public override void UpdateData(object data = null)
    {
        base.UpdateData(data);
        // ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.iconContent);
        
    }
    public void ClanTabs(int index)
    {
        AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
        foreach (Image img in tabsImg)
        {
            img.sprite = tabOff;
        }
        tabsImg[index].sprite = tabOn;
        switch (index)
        {
            case 0:
               
                ShowMyClan();
                break;
            case 1:
                myClanInfo.SetActive(false);
                myClanGo.SetActive(false);
                rankTitleGO.SetActive(false);
                ShowMemBerClan();
                break;
            case 2:
                myClanInfo.SetActive(true);
                memberClan.gameObject.SetActive(false);
                ShowRankClan();
                break;
            case 3:
                isShowRank = false;
                rankTitleGO.SetActive(false);
                myClanGo.SetActive(false);
                myClanInfo.SetActive(false);
                memberClan.OnShowRequest();
                break;
        }
        AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
    }
    bool isShowRank = false;
    public void ShowRankUI()
    {
        ClanTabs(2);
        rankTitleGO.SetActive(true);
        myCLanGo.SetActive(false);
        tabsImg[1].gameObject.SetActive(false);
        tabsImg[3].gameObject.SetActive(false);
        isShowRank = true;
        RankClanTxt.text = LeanLocalization.GetTranslationText("clan_Rank"); 
    }
    bool isEdit = false;
    public void ShowMyClan()
    {

        isShowRank = false ;
        tabsImg[1].gameObject.SetActive(true);
        tabsImg[3].gameObject.SetActive(true);
        rankClan.SetActive(false);
        clanInfo.SetActive(true);
        memberClan.gameObject.SetActive(false);
        myClanInfo.SetActive(true);
        memberClan.requestGo.SetActive(ClanManager.Instance.IsClanAcceptMember());
        if (ClanManager.Instance.PlayerUserClan != null && !string.IsNullOrEmpty(ClanManager.Instance.PlayerUserClan.ClanId)&&!isEdit)
        {
            myClanGo.SetActive(true);
            creteClanGo.SetActive(false);
            rankTitleGO.SetActive(false);
            btnEditName.SetActive(ClanManager.Instance.IsClanEdit());
            btnEditDes.SetActive(ClanManager.Instance.IsClanEdit());
            btnEditMess.SetActive(ClanManager.Instance.IsClanEdit());
            myClanNameTxt.text = LeanLocalization.GetTranslationText("my_clan");
            myClanGo.GetComponent<MyClanUI>().SetData(ClanManager.Instance.PlayerClan);
            SetInfoClan(ClanManager.Instance.PlayerClan);
            return;
        }
        myClanGo.SetActive(false);
        creteClanGo.SetActive(true);
        clanInfo.SetActive(false);
        rankTitleGO.SetActive(true);
        tabsImg[1].gameObject.SetActive(false);
        tabsImg[3].gameObject.SetActive(false);
        if (!isEdit)
        {
           
            myClanNameTxt.text = LeanLocalization.GetTranslationText("clan_create"); 
            txtprice.text = "150";
            btnCancel.SetActive(false);
        }
           
        else
        {
            myClanNameTxt.text = LeanLocalization.GetTranslationText("clan_edit"); ;
            iconIndex = ClanManager.Instance.PlayerClan.Icon;
            colorIndex = ClanManager.Instance.PlayerClan.Color;
            frameIndex = ClanManager.Instance.PlayerClan.Frame;
            Auto = ClanManager.Instance.PlayerClan.AutoAcceptMember;
            nameInput.text = ClanManager.Instance.PlayerClan.Name;
            desInput.text = ClanManager.Instance.PlayerClan.Slogan;
            txtprice.text = "50";
            btnCancel.SetActive(true);
            scrollRect.GetComponent<RectTransform>().anchoredPosition = new Vector2(scrollRect.GetComponent<RectTransform>().anchoredPosition.x, 0);
        }
       // this.iconContent.GetComponent<GridLayoutGroup>().
        SetIcon(iconIndex);
        SetColor(colorIndex);
        SetFrame(frameIndex);
        SetAuto(Auto);
        PopupManager.Instance.GetPopupUIByCode(PopupCode.ClanHomeUI).GetComponent<ClanHomeUI>().UpdateData();
      
    }
    public void SetTitle()
    {
        if (ClanManager.Instance.PlayerUserClan != null && !string.IsNullOrEmpty(ClanManager.Instance.PlayerUserClan.ClanId))
        {
           
            myClanNameTxt.text = LeanLocalization.GetTranslationText("my_clan");
            
        }
        else
            myClanNameTxt.text = LeanLocalization.GetTranslationText("clan_create");
    }
    public void ShowMemBerClan()
    {
        isShowRank = false;
        //PopupManager.Instance.OnUI(PopupCode.MemberClanUI,null);
        memberClan.OnShowMember();
    }
    public void ShowRankClan()
    {
        rankClan.SetActive(true);
        btnEditName.SetActive(false);
        btnEditDes.SetActive(false);
        myClanGo.SetActive(false);
        creteClanGo.SetActive(false);
        clanInfo.SetActive(true);
        rankClan.GetComponent<ClanRankUI>().ShowRankMember();
       
    }
    public void SetColor(int indexIcon)
    {
        clanInfoItem.SetColor(indexIcon);
        colorIndex = indexIcon;
        tickColor.transform.SetParent(lsColors[indexIcon].transform,false);
        tickColor.transform.localPosition = Vector3.zero;
    }
    public void SetIcon(int indexIcon)
    {
        clanInfoItem.SetIcon(indexIcon);
        iconIndex = indexIcon;
        tickIcon.transform.SetParent(lsIconsItem[indexIcon].transform,false);
        tickIcon.transform.localPosition = Vector3.zero;
    }
    public void SetFrame(int indexIcon)
    {
        clanInfoItem.SetFrame(indexIcon);
        frameIndex = indexIcon;
        tickFrame.transform.SetParent(lsPattern[indexIcon].transform, false);
        tickFrame.transform.localPosition = Vector3.zero;
    }
    public void SetInfoClan(Clan clan)
    {
       
        SetIcon(clan.Icon);
        SetColor(clan.Color);
        SetFrame(clan.Frame);
        nameClanTxt.text = clan.Name;
        desClanTxt.text = clan.Slogan;
        autoOff1.SetActive(!clan.AutoAcceptMember);
        autoOn1.SetActive(clan.AutoAcceptMember);
        Auto = clan.AutoAcceptMember;
        //SetAuto(clan.AutoAcceptMember);
    }
    public void SetInfoClanInfo(ClanInfo clan)
    {

        SetIcon(clan.Icon);
        SetColor(clan.Color);
        SetFrame(clan.Frame);
        nameClanTxt.text = clan.Name;
        desClanTxt.text = clan.Slogan;
        autoOff1.SetActive(!clan.AutoAccept);
        autoOn1.SetActive(clan.AutoAccept);
        //SetAuto(clan.AutoAcceptMember);
    }
    public void SetAuto(bool isAuto)
    {
        AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
        Auto = isAuto;
        autoOff.SetActive(!isAuto);
        autoOn.SetActive(isAuto);
    }
    public void SetAutoEdit(bool isAuto)
    {
        AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
        if (isShowRank)
            return;

        if (ClanManager.Instance.IsClanAcceptMember())
        {
            Auto = isAuto;
            //autoOff1.SetActive(!isAuto);
            //autoOn1.SetActive(isAuto);
            StartCoroutine(ClanManager.Instance.IEChangeAutoAcceptMember(UserDataManager.Instance.GetUserID(), ClanManager.Instance.PlayerClan._id, isAuto, () => {
                GetComponentInParent<ClanUI>().ShowMyClan();
                
            }));
        }
        else
        {
            HUDCanvas.Instance.ShowNotification(LeanLocalization.GetTranslationText("clan_noti_cant_action"));
        }
    }
    public void DonateUI()
    {
        AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
        // donateGo.SetActive(true);
        donateCLan.gameObject.SetActive(true);
        donateCLan.SetActiveButton();
    }
    public void Donate(int index)
    {
        if (!ClanManager.Instance.IsDonated((ClanDonateType)index))
        {
            
            StartCoroutine(ClanManager.Instance.IEDonateFund(UserDataManager.Instance.GetUserID(), ClanManager.Instance.PlayerClan._id, (ClanDonateType)index, (type) => {
                if((ClanDonateType) index == ClanDonateType.Normal)
                {
                    AppsFlyerManager.TrackingEvent(AppsflyerEvents.money_spent, "donate_clan", 100);
                }
                donateGo.SetActive(false);
                AudioCtrl.Instance.Play(AudioName.Achievement_Sound);
                HUDCanvas.Instance.ShowNotification(LeanLocalization.GetTranslationText("clan_noti_donate_succesful"));
                ShowMyClan();
                AppsFlyerManager.TrackingEvent(AppsflyerEvents.donate_clan, 10, -1);
            }));
        }
          
        else
        {
            HUDCanvas.Instance.ShowNotification(LeanLocalization.GetTranslationText("clan_noti_limit_donate"));
        }
    }
    public void CreatClan()
    {
        if (ItemDataManager.Instance.GetItem(ItemType.Gem).Amount < 150&&!isEdit)
        {
            ItemDataManager.Instance.ShowDontEnoughItem(ItemType.Gem);
            return;
        }
        if (ItemDataManager.Instance.GetItem(ItemType.Gem).Amount < 50 && isEdit)
        {
            ItemDataManager.Instance.ShowDontEnoughItem(ItemType.Gem);
            return;
        }
        if (nameTxt.text == null)
        {
            HUDCanvas.Instance.ShowNotification(LeanLocalization.GetTranslationText("clan_noti_clan_03"));
            return;
        }else if (nameTxt.text.Length < 4)
        {
            HUDCanvas.Instance.ShowNotification(LeanLocalization.GetTranslationText("clan_noti_clan_04"));
            return;
        }
        else if (nameTxt.text.Length >20)
        {
            HUDCanvas.Instance.ShowNotification(LeanLocalization.GetTranslationText("clan_noti_clan_05"));
            return;
        }
        if (desTxt.text == null)
        {
            HUDCanvas.Instance.ShowNotification(LeanLocalization.GetTranslationText("clan_noti_clan_06"));
            return;
        }
        else if (desTxt.text.Length>200)
        {
            HUDCanvas.Instance.ShowNotification(LeanLocalization.GetTranslationText("clan_noti_clan_07"));
            return;
        }
        ClanCreateData clanData = new ClanCreateData("");
        clanData.Name = nameTxt.text;
        clanData.Icon = iconIndex;
        clanData.Frame = frameIndex;
        clanData.Color = colorIndex;
        clanData.Slogan = desTxt.text;
        clanData.AutoAcceptMember = Auto;
        if (!isEdit)
        {
            StartCoroutine(ClanManager.Instance.IECreateClan(clanData, (myClan) =>
            {
                HUDCanvas.Instance.ShowNotification(LeanLocalization.GetTranslationText("clan_create_sucsessfully"));
                ShowMyClan();
            }));
        }
        else {
            StartCoroutine(ClanManager.Instance.IEditClan(clanData, () =>
            {
                HUDCanvas.Instance.ShowNotification(LeanLocalization.GetTranslationText("clan_edit_sucsessfully"));
                isEdit = false;
                ShowMyClan();
            }));
        }
        
    }
    public void ButtonEdit()
    {
        AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
        isEdit = true;
        ShowMyClan();
    }
    public void ButtonCancelEdit()
    {
        AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
        isEdit = false;
        ShowMyClan();
    }
}
