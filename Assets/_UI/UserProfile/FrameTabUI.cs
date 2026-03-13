using System.Collections;
using System.Collections.Generic;
using NTPackage.EventDispatcher;
using NTPackage.Functions;
using NTPackage.UI;
using Rubik.ItemPlayer;
using TMPro;
using UnityEngine;

namespace Rubik.UserProfile
{
    public class FrameTabUI : TabUI
    {
        public AvatarChangeUI AvatarChangeUI;
        
        //Avatar
        public FrameChangeItemUI FrameChangeItemPrefab;
        public Transform FrameChangeItemParent;
        public List<FrameChangeItemUI> FrameChangeItemList = new List<FrameChangeItemUI>();
        public int IndexSelectedFrame = -1;

        public override void OnUI()
        {
            base.OnUI();
            this.IndexSelectedFrame = UserProfileManager.Instance.AvatarBorderPlayer.Current;
            EventListenerManager.instance.Register(EventCode.ChangeAvatar, "AvatarTabUI", (data)=>{
                this.IndexSelectedFrame = UserProfileManager.Instance.AvatarBorderPlayer.Current;
                this.SelectFrame(this.IndexSelectedFrame);
            });
            this.AvatarChangeUI.BtnBuy.Onclick.RemoveAllListeners();
            this.AvatarChangeUI.BtnBuy.Onclick.AddListener(this.OnClickBuy);
            this.AvatarChangeUI.BtnEquip.Onclick.RemoveAllListeners();
            this.AvatarChangeUI.BtnEquip.Onclick.AddListener(this.OnClickEquip);
            this.AvatarChangeUI.BtnLock.Onclick.RemoveAllListeners();
            this.SelectFrame(this.IndexSelectedFrame);
            this.UpdateData();
            AvatarChangeUI.SetBadgeOff();
        }

        public override void OffUI()
        {
            base.OffUI();
            EventListenerManager.instance.RemoveListener(EventCode.ChangeAvatar, "AvatarChangeUI");
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.FrameChangeItemParent);
        }

        public override void UpdateData(){
            this.AvatarChangeUI.AvatarPlayerUI.SetData(this.IndexSelectedFrame, UserProfileManager.Instance.AvatarBorderPlayer.Current);
            this.FrameChangeItemList.Clear();
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.FrameChangeItemParent);
            foreach (AvatarBorderData avatar in UserProfileManager.Instance.GetListAvatarBorderData())
            {
                FrameChangeItemUI item = ObjectPoolingManager.Instance.InstantiateObject<FrameChangeItemUI>(ObjectPoolingConfig.FrameChangeItemUI, this.FrameChangeItemPrefab.transform);
                item.transform.name = ObjectPoolingConfig.FrameChangeItemUI;
                item.transform.SetParent(this.FrameChangeItemParent);
                NTFunction.ResetPosition(item.transform);
                item.SetData(avatar, this);
                this.FrameChangeItemList.Add(item);
            }
            NTFunction.ResetPosition(this.FrameChangeItemParent);
        }

        public void SelectFrame(object data){
            this.IndexSelectedFrame = (int)data;
            this.AvatarChangeUI.AvatarPlayerUI.SetData(UserProfileManager.Instance.AvatarPlayer.Current, this.IndexSelectedFrame);
            this.AvatarChangeUI.BtnLock.gameObject.SetActive(false);
            this.AvatarChangeUI.BtnEquip.gameObject.SetActive(false);
            this.AvatarChangeUI.BtnBuy.gameObject.SetActive(false);
            if(UserProfileManager.Instance.IsAvatarBorderAvailable(this.IndexSelectedFrame)){
                if(this.IndexSelectedFrame != UserProfileManager.Instance.AvatarBorderPlayer.Current){
                    this.AvatarChangeUI.BtnEquip.gameObject.SetActive(true);
                }
            } else {
                AvatarBorderData avatarBorderData = UserProfileManager.Instance.GetAvatarBorderData(this.IndexSelectedFrame);
                if(avatarBorderData == null || avatarBorderData.Lock){
                    this.AvatarChangeUI.BtnLock.gameObject.SetActive(true);
                    this.AvatarChangeUI.ConditionText.text = Lean.Localization.LeanLocalization.GetTranslationText("frame_not_available", "Frame not available");
                }else{
                    if(avatarBorderData.UnlockData.UnlockType == UnlockType.Level){
                        this.AvatarChangeUI.BtnLock.gameObject.SetActive(true);
                        string str = Lean.Localization.LeanLocalization.GetTranslationText("frame_locked", "Frame unlock by at level {0}");
                        this.AvatarChangeUI.ConditionText.text = string.Format(str, avatarBorderData.UnlockData.LevelUnlock);
                    }else if(avatarBorderData.UnlockData.UnlockType == UnlockType.Purchase){
                        this.AvatarChangeUI.BtnBuy.gameObject.SetActive(true);
                        this.AvatarChangeUI.Price.SetData(avatarBorderData.UnlockData.Price[0]);
                        if(ItemDataManager.Instance.GetItem(avatarBorderData.UnlockData.Price[0].Type).Amount < avatarBorderData.UnlockData.Price[0].Amount){
                            this.AvatarChangeUI.Price.Amount.color = Color.red;
                        }else{
                            this.AvatarChangeUI.Price.Amount.color = Color.white;
                        }
                    }else{
                        this.AvatarChangeUI.BtnLock.gameObject.SetActive(true);
                        this.AvatarChangeUI.ConditionText.text = Lean.Localization.LeanLocalization.GetTranslationText("frame_get_event", "Available in event");
                    }
                }
            }
        }

        public void OnClickBuy(){
            if(UserProfileManager.Instance.IsAvatarBorderAvailable(this.IndexSelectedFrame)){
                return;
            }
            StartCoroutine(UserProfileManager.Instance.IEBuyAvatarBorder(this.IndexSelectedFrame, () =>
            {
                this.UpdateData();
                this.AvatarChangeUI.UpdateData();
            }));
        }

        public void OnClickEquip(){
            StartCoroutine(UserProfileManager.Instance.IEChangeAvatarBorder(this.IndexSelectedFrame, () =>
            {
                this.UpdateData();
                this.AvatarChangeUI.UpdateData();
            }));
        }

    }
}