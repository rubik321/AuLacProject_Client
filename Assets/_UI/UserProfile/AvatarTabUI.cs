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
    public class AvatarTabUI : TabUI
    {
        public AvatarChangeUI AvatarChangeUI;
        
        //Avatar
        public AvatarChangeItemUI AvatarChangeItemPrefab;
        public Transform AvatarChangeItemParent;
        public List<AvatarChangeItemUI> AvatarChangeItemList = new List<AvatarChangeItemUI>();
        public int IndexSelectedAvatar = -1;

        public override void OnUI()
        {
            base.OnUI();
            this.IndexSelectedAvatar = UserProfileManager.Instance.AvatarPlayer.Current;
            EventListenerManager.instance.Register(EventCode.ChangeAvatar, "AvatarTabUI", (data)=>{
                this.IndexSelectedAvatar = UserProfileManager.Instance.AvatarPlayer.Current;
                this.SelectAvatar(this.IndexSelectedAvatar);
            });
            this.AvatarChangeUI.BtnBuy.Onclick.RemoveAllListeners();
            this.AvatarChangeUI.BtnBuy.Onclick.AddListener(this.OnClickBuy);
            this.AvatarChangeUI.BtnEquip.Onclick.RemoveAllListeners();
            this.AvatarChangeUI.BtnEquip.Onclick.AddListener(this.OnClickEquip);
            this.AvatarChangeUI.BtnLock.Onclick.RemoveAllListeners();
            AvatarChangeUI.SetBadgeOff();
            this.SelectAvatar(this.IndexSelectedAvatar);
            this.UpdateData();
            NTFunction.ResetPosition(this.AvatarChangeItemParent);
        }

        public override void OffUI()
        {
            base.OffUI();
            EventListenerManager.instance.RemoveListener(EventCode.ChangeAvatar, "AvatarChangeUI");
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.AvatarChangeItemParent);
        }

        public override void UpdateData(){
            this.AvatarChangeUI.AvatarPlayerUI.SetData(this.IndexSelectedAvatar, UserProfileManager.Instance.AvatarPlayer.Current);
            this.AvatarChangeItemList.Clear();
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.AvatarChangeItemParent);
            foreach (AvatarData avatar in UserProfileManager.Instance.GetListAvatarData())
            {
                if(avatar.Lock) continue;
                AvatarChangeItemUI item = ObjectPoolingManager.Instance.InstantiateObject<AvatarChangeItemUI>(ObjectPoolingConfig.AvatarChangeItemUI, this.AvatarChangeItemPrefab.transform);
                item.transform.name = ObjectPoolingConfig.AvatarChangeItemUI;
                item.transform.SetParent(this.AvatarChangeItemParent);
                NTFunction.ResetPosition(item.transform);
                item.SetData(avatar, this);
                this.AvatarChangeItemList.Add(item);
            }
        }

        public void SelectAvatar(object data){
            this.IndexSelectedAvatar = (int)data;
            this.AvatarChangeUI.AvatarPlayerUI.SetData(this.IndexSelectedAvatar, UserProfileManager.Instance.AvatarBorderPlayer.Current);
            this.AvatarChangeUI.BtnLock.gameObject.SetActive(false);
            this.AvatarChangeUI.BtnEquip.gameObject.SetActive(false);
            this.AvatarChangeUI.BtnBuy.gameObject.SetActive(false);
            if(UserProfileManager.Instance.IsAvatarAvailable(this.IndexSelectedAvatar)){
                if(this.IndexSelectedAvatar != UserProfileManager.Instance.AvatarPlayer.Current){
                    this.AvatarChangeUI.BtnEquip.gameObject.SetActive(true);
                }
            } else {
                AvatarData avatarData = UserProfileManager.Instance.GetAvatarData(this.IndexSelectedAvatar);
                if(avatarData == null || avatarData.Lock){
                    this.AvatarChangeUI.BtnLock.gameObject.SetActive(true);
                    this.AvatarChangeUI.ConditionText.text = Lean.Localization.LeanLocalization.GetTranslationText("ava_not_available", "Frame not available");
                }else{
                    if(avatarData.UnlockData.UnlockType == UnlockType.Level){
                        this.AvatarChangeUI.BtnLock.gameObject.SetActive(true);
                        string str = Lean.Localization.LeanLocalization.GetTranslationText("ava_locked", "Frame unlock by at level {0}");
                        this.AvatarChangeUI.ConditionText.text = string.Format(str, avatarData.UnlockData.LevelUnlock);
                    }else if(avatarData.UnlockData.UnlockType == UnlockType.Purchase){
                        this.AvatarChangeUI.BtnBuy.gameObject.SetActive(true);
                        this.AvatarChangeUI.Price.SetData(avatarData.UnlockData.Price[0]);
                        if(ItemDataManager.Instance.GetItem(avatarData.UnlockData.Price[0].Type).Amount < avatarData.UnlockData.Price[0].Amount){
                            this.AvatarChangeUI.Price.Amount.color = Color.red;
                        }else{
                            this.AvatarChangeUI.Price.Amount.color = Color.white;
                        }
                    }else{
                        this.AvatarChangeUI.BtnLock.gameObject.SetActive(true);
                        this.AvatarChangeUI.ConditionText.text = Lean.Localization.LeanLocalization.GetTranslationText("ava_get_event", "Available in event");
                    }
                }
            }
        }

        public void OnClickBuy(){
            if(UserProfileManager.Instance.IsAvatarAvailable(this.IndexSelectedAvatar)){
                return;
            }
            StartCoroutine(UserProfileManager.Instance.IEBuyAvatar(this.IndexSelectedAvatar, () =>
            {
                this.UpdateData();
            }));
        }

        public void OnClickEquip(){
            StartCoroutine(UserProfileManager.Instance.IEChangeAvatar(this.IndexSelectedAvatar, () =>
            {
                this.UpdateData();
            }));
        }

    }
}