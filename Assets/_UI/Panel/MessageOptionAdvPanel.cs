using System.Collections;
using System.Collections.Generic;
using NTPackage.UI;
using UnityEngine;
using TMPro;
using NTPackage.Functions;
using UnityEngine.UIElements;
using UnityEngine.UI;
using System;
namespace Rubik.UI
{
    using Rubik.CardPlayer;
    using Rubik.CharacterGear;
    using Rubik.Combat;
    using Rubik.RewardData;

    public class MessageOptionAdvPanel : PopupUI
    {
        public TextMeshProUGUI Title;
        public TextMeshProUGUI TextDes;

        public Transform TransReward;
        public RewardDataItemUI RewardDataItemPrefab;
        public Transform Holder;
        public List<RewardDataItemUI> RewardDataItemList = new List<RewardDataItemUI>();

        public NTButtonEffect AgreeAdvBtn;
        public NTButtonEffect CancelBtn;
        private Action ActionAgree;
        public TextMeshProUGUI TextUpperAgree;
        public TextMeshProUGUI TextAgree;
        private Action ActionAgreeAdv;
        public TextMeshProUGUI TextAgreeAdv;


        public override void LoadComponents()
        {
            base.LoadComponents();
        }

        public override void OnUI(object data = null, bool isDefaultSound = true)
        {
            base.OnUI(data, isDefaultSound);
        }

        public void SetReward(RewardData data)
        {
            RewardData rewardData = data;
            Title.text = Lean.Localization.LeanLocalization.GetTranslationText(rewardData.Title, rewardData.Title);
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(Holder);
            RewardDataItemList.Clear();
            if (rewardData.GearItems != null)
            {
                foreach (CharacterGear item in rewardData.GearItems)
                {
                    RewardDataItemUI rewardDataItemUI = ObjectPoolingManager.Instance.PullObjectFromPooling<RewardDataItemUI>(ObjectPoolingConfig.RewardDataItemUI);
                    if (rewardDataItemUI == null)
                    {
                        rewardDataItemUI = Instantiate(RewardDataItemPrefab, Holder);
                    }
                    rewardDataItemUI.gameObject.SetActive(true);
                    rewardDataItemUI.transform.SetParent(Holder);
                    NTFunction.ResetPosition(rewardDataItemUI.transform);
                    rewardDataItemUI.SetData(item);
                    RewardDataItemList.Add(rewardDataItemUI);
                }
            }
            if (rewardData.Items != null)
            {
                foreach (var item in rewardData.Items)
                {
                    RewardDataItemUI rewardDataItemUI = ObjectPoolingManager.Instance.PullObjectFromPooling<RewardDataItemUI>(ObjectPoolingConfig.RewardDataItemUI);
                    if (rewardDataItemUI == null)
                    {
                        rewardDataItemUI = Instantiate(RewardDataItemPrefab, Holder);
                    }
                    rewardDataItemUI.gameObject.SetActive(true);
                    rewardDataItemUI.transform.SetParent(Holder);
                    NTFunction.ResetPosition(rewardDataItemUI.transform);
                    rewardDataItemUI.SetData(item);
                    RewardDataItemList.Add(rewardDataItemUI);
                }
            }
            if (rewardData.Card != null)
            {
                foreach (Rubik.CardPlayer.CardPlayer item in rewardData.Card)
                {
                    RewardDataItemUI rewardDataItemUI = ObjectPoolingManager.Instance.PullObjectFromPooling<RewardDataItemUI>(ObjectPoolingConfig.RewardDataItemUI);
                    if (rewardDataItemUI == null)
                    {
                        rewardDataItemUI = Instantiate(RewardDataItemPrefab, Holder);
                    }
                    rewardDataItemUI.gameObject.SetActive(true);
                    rewardDataItemUI.transform.SetParent(Holder);
                    NTFunction.ResetPosition(rewardDataItemUI.transform);
                    rewardDataItemUI.SetData(item);
                    RewardDataItemList.Add(rewardDataItemUI);
                }
            }
            if (LevelPlayAds.Instance.IsCanShowAds())
            {
                this.AgreeAdvBtn.gameObject.SetActive(true);
                this.CancelBtn.gameObject.SetActive(false);
            }
            else
            {
                this.AgreeAdvBtn.gameObject.SetActive(false);
                this.CancelBtn.gameObject.SetActive(true);
            }
            this.TextDes.gameObject.SetActive(false);
            this.TransReward.gameObject.SetActive(true);
        }

        public void SetDescription(string title, string des)
        {
            this.Title.text = title;
            this.TextDes.text = des;
            this.TextDes.gameObject.SetActive(true);
            this.TransReward.gameObject.SetActive(false);
            if (LevelPlayAds.Instance.IsCanShowAds())
            {
                this.AgreeAdvBtn.gameObject.SetActive(true);
                this.CancelBtn.gameObject.SetActive(false);
            }
            else
            {
                this.AgreeAdvBtn.gameObject.SetActive(false);
                this.CancelBtn.gameObject.SetActive(true);
            }
        }

        public void SetAgreeAction(Action action, string text, string textUpper = ""){
            this.ActionAgree = action;
            this.TextAgree.text = text;
            this.TextUpperAgree.text = textUpper;
        }

        public void Agree()
        {
            ActionAgree?.Invoke();
            this.OffUI();
        }

        public void SetAgreeAdvAction(Action action, string text){
            this.ActionAgreeAdv = action;
            this.TextAgreeAdv.text = text;
        }

        public void AgreeAdv()
        {
            ActionAgreeAdv?.Invoke();
            this.OffUI();
        }

        public override void ScriptOffUI()
        {
            base.ScriptOffUI();
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(Holder);
        }
    }
}