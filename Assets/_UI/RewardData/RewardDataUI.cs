using System.Collections;
using System.Collections.Generic;
using NTPackage.UI;
using UnityEngine;
using TMPro;
using NTPackage.Functions;
using UnityEngine.UIElements;
using UnityEngine.UI;

namespace Rubik.RewardData
{
    using System.Linq;
    using Rubik.CardPlayer;
    using Rubik.CharacterGear;
    using Rubik.Combat;

    public class RewardDataUI : PopupUI
    {
        public TextMeshProUGUI Title;

        public RewardDataItemUI RewardDataItemPrefab;
        public Transform Holder;
        public List<RewardDataItemUI> RewardDataItemList = new List<RewardDataItemUI>();

        public List<RewardData> RewardDatas = new List<RewardData>();

        public ScrollRect ScrollRect;

        public override void LoadComponents()
        {
            base.LoadComponents();
            this.popupCode = PopupCode.RewardDataUI;
        }

        public void SetData(RewardData[] rewardDatas)
        {
            this.RewardDatas = new List<RewardData>(rewardDatas);
            this.OnReward();
        }

        public void OnReward(){
            if(this.RewardDatas.Count == 0){
                this.OffUI();
                return;
            }
            RewardData rewardData = this.RewardDatas.First();
            this.RewardDatas.RemoveAt(0);
            Title.text = Lean.Localization.LeanLocalization.GetTranslationText(rewardData.Title, rewardData.Title);
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(Holder);
            RewardDataItemList.Clear();
            foreach (CharacterGear item in rewardData.GearItems)
            {
                RewardDataItemUI rewardDataItemUI = ObjectPoolingManager.Instance.PullObjectFromPooling<RewardDataItemUI>(ObjectPoolingConfig.RewardDataItemUI);
                if(rewardDataItemUI == null){
                    rewardDataItemUI = Instantiate(RewardDataItemPrefab, Holder);
                }
                rewardDataItemUI.gameObject.SetActive(true);
                rewardDataItemUI.transform.SetParent(Holder);
                NTFunction.ResetPosition(rewardDataItemUI.transform);
                rewardDataItemUI.SetData(item);
                RewardDataItemList.Add(rewardDataItemUI);
            }
            foreach (var item in rewardData.Items)
            {
                RewardDataItemUI rewardDataItemUI = ObjectPoolingManager.Instance.PullObjectFromPooling<RewardDataItemUI>(ObjectPoolingConfig.RewardDataItemUI);
                if(rewardDataItemUI == null){
                    rewardDataItemUI = Instantiate(RewardDataItemPrefab, Holder);
                }
                rewardDataItemUI.gameObject.SetActive(true);
                rewardDataItemUI.transform.SetParent(Holder);
                NTFunction.ResetPosition(rewardDataItemUI.transform);
                rewardDataItemUI.SetData(item);
                RewardDataItemList.Add(rewardDataItemUI);
            }

            foreach (CardPlayer item in rewardData.Card)
            {
                RewardDataItemUI rewardDataItemUI = ObjectPoolingManager.Instance.PullObjectFromPooling<RewardDataItemUI>(ObjectPoolingConfig.RewardDataItemUI);
                if(rewardDataItemUI == null){
                    rewardDataItemUI = Instantiate(RewardDataItemPrefab, Holder);
                }
                rewardDataItemUI.gameObject.SetActive(true);
                rewardDataItemUI.transform.SetParent(Holder);
                NTFunction.ResetPosition(rewardDataItemUI.transform);
                rewardDataItemUI.SetData(item);
                RewardDataItemList.Add(rewardDataItemUI);
            }

            if(RewardDataItemList.Count > 10){
                this.ScrollRect.vertical = true;
            }else{
                this.ScrollRect.vertical = false;
            }
        }

        public override void OnUI(object data = null, bool isDefaultSound = true)
        {
            base.OnUI(data, isDefaultSound);
        }

        public override void ScriptOffUI()
        {
            base.ScriptOffUI();
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(Holder);
            if (AssetLoader.Instance.IsTut)
            {
                PopupManager.Instance.GetPopupUI(PopupCode.PlayerMailUI).OffUI();
               
                AssetLoader.Instance.IsTut = false;
                var temp = PopupManager.Instance.GetPopupUI(PopupCode.TutorialUI).GetComponent<TutorialUI>();
                if (temp.isCanNextTut)
                    PopupManager.Instance.OnUI(PopupCode.TutorialUI);

            }
            if(this.RewardDatas.Count > 0){
                this.OnReward();
            }
        }
    }
}