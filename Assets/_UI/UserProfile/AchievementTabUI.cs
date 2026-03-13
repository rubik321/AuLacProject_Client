using NTPackage.EventDispatcher;
using NTPackage.Functions;
using NTPackage.UI;
using Rubik.ItemPlayer;
using Rubik.Quest;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace Rubik.UserProfile { 
public class AchievementTabUI : TabUI
{
    public AvatarChangeUI AvatarChangeUI;

    //Avatar
    public AchievementChangeItemUI FrameChangeItemPrefab;
    public Transform FrameChangeItemParent;
    public List<AchievementChangeItemUI> FrameChangeItemList = new List<AchievementChangeItemUI>();
    public int IndexSelectedFrame = -1;

    public override void OnUI()
    {
        base.OnUI();
        this.IndexSelectedFrame = AchievementManager.Instance.AchievementBadgePlayer.Equip;
           
        this.AvatarChangeUI.BtnBuy.Onclick.RemoveAllListeners();
        this.AvatarChangeUI.BtnBuy.Onclick.AddListener(this.OnClickBuy);
        this.AvatarChangeUI.BtnEquip.Onclick.RemoveAllListeners();
        this.AvatarChangeUI.BtnEquip.Onclick.AddListener(this.OnClickEquip);
        this.AvatarChangeUI.BtnLock.Onclick.RemoveAllListeners();
        this.AvatarChangeUI.ConditionText.text = "";
        this.SelectFrame(this.IndexSelectedFrame);
        this.UpdateData();
    }

    public override void OffUI()
    {
        base.OffUI();
        //EventListenerManager.instance.RemoveListener(EventCode.ChangeAvatar, "AchievementTabUI");
        ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.FrameChangeItemParent);
    }

    public override void UpdateData()
    {
        this.AvatarChangeUI.AvatarPlayerUI.SetData(this.IndexSelectedFrame, UserProfileManager.Instance.AvatarBorderPlayer.Current);
        this.FrameChangeItemList.Clear();
        ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.FrameChangeItemParent);
        var Achiments = AchievementManager.Instance.GetAchievement();
        foreach (AchievementPlayer avatar in Achiments)
        {
            AchievementChangeItemUI item = ObjectPoolingManager.Instance.InstantiateObject<AchievementChangeItemUI>(ObjectPoolingConfig.AchievementChangeItemUI, this.FrameChangeItemPrefab.transform);
            item.transform.name = ObjectPoolingConfig.AchievementChangeItemUI;
            item.transform.SetParent(this.FrameChangeItemParent);
            NTFunction.ResetPosition(item.transform);
             item.SetData(avatar, this);
            this.FrameChangeItemList.Add(item);
        }
        NTFunction.ResetPosition(this.FrameChangeItemParent);
    }

    public void SelectFrame(int data)
        {
            this.IndexSelectedFrame = data;
            this.AvatarChangeUI.AvatarPlayerUI.SetData(UserProfileManager.Instance.AvatarPlayer.Current, this.IndexSelectedFrame);
            this.AvatarChangeUI.BtnLock.gameObject.SetActive(false);
            this.AvatarChangeUI.BtnEquip.gameObject.SetActive(false);
            this.AvatarChangeUI.BtnBuy.gameObject.SetActive(false);
            //if (AchievementManager.Instance.AchievementBadgePlayer.Equip ==-1)
            //    return;
            if (IndexSelectedFrame == -1||!AchievementManager.Instance.IsAchievementCompleted((AchievementPlayerIndex)IndexSelectedFrame))
            {
                this.AvatarChangeUI.BtnLock.gameObject.SetActive(true);
            }
            else if (AchievementManager.Instance.AchievementBadgePlayer.Equip == IndexSelectedFrame)
            {
                //this.AvatarChangeUI.BtnLock.gameObject.SetActive(true);

            }
            else
            {
                this.AvatarChangeUI.BtnEquip.gameObject.SetActive(true);
            }

        }

        public void OnClickBuy()
    {
        if (UserProfileManager.Instance.IsAvatarBorderAvailable(this.IndexSelectedFrame))
        {
            return;
        }
        StartCoroutine(UserProfileManager.Instance.IEBuyAvatarBorder(this.IndexSelectedFrame, () =>
        {
            this.UpdateData();
            this.AvatarChangeUI.UpdateData();
        }));
    }

    public void OnClickEquip()
    {

            AchievementManager.Instance.EquipAchievementBadge(IndexSelectedFrame,
                () => {
                    UpdateData();
                    this.AvatarChangeUI.UpdateData();
                    AvatarChangeUI.SetBadge((AchievementPlayerIndex)IndexSelectedFrame);

                });
    }
}
}
