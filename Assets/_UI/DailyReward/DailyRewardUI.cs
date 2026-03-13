using Lean.Localization;
using NTPackage.EventDispatcher;
using NTPackage.Functions;
using NTPackage.UI;
using NTPackage.UI;
using Rubik.CardPlayer;
using Rubik.Common.AudioHelper;
using Rubik.ItemPlayer;
using Rubik.Manager;
using Rubik.Quest;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rubik.Myrk.DailyReward
{
    public class DailyRewardUI : PopupUI
    {
        public Image IconReward;

        public TextMeshProUGUI TextCountDown;
        public NTButtonEffect ButtonClaim;
        public TextMeshProUGUI TextLastDay;


        public List<DateDailyRewardItem> DateDailyRewardItems;

        public List<ItemDataBarUI> ItemDataBars;

        [NTButton]
        public void TestOnUI(){
            this.OnUI();
        }

        public override void OnUI(object data = null, bool isDefaultSound = true){
            base.OnUI(data, isDefaultSound);
            EventListenerManager.instance.Register(EventCode.DailyRewardUpdate, "DailyRewardUI", this.UpdateData);
        }

        public override void ScriptOffUI()
        {
            base.ScriptOffUI();
            EventListenerManager.instance.RemoveListener(EventCode.DailyRewardUpdate, "DailyRewardUI");
            if(this.CorCountDown != null){
                StopCoroutine(this.CorCountDown);
            }
        }

        public override void UpdateData(object data = null){
            base.UpdateData(data);
            for (int i = 0; i < this.DateDailyRewardItems.Count; i++)
            {
                this.DateDailyRewardItems[i].SetData(DailyRewardManager.Instance.GetDailyRewardData(i), this);
            }

            DailyReward dailyReward = DailyRewardManager.Instance.DailyReward;
            if(dailyReward.Claimed == true){
                DailyRewardData dailyRewardData = DailyRewardManager.Instance.GetDailyRewardData(dailyReward.Day+1);
                if(dailyRewardData != null){
                    this.ButtonClaim.gameObject.SetActive(false);
                    this.TextCountDown.text = "";
                    ItemType itemType = dailyRewardData.RewardItems[0].Type;
                    if(this.CorCountDown != null){
                        StopCoroutine(this.CorCountDown);
                    }
                    this.CorCountDown = StartCoroutine(this.IECountDown());
                }else{
                    this.ButtonClaim.gameObject.SetActive(false);
                    dailyRewardData = DailyRewardManager.Instance.GetDailyRewardData(dailyReward.Day);
                    ItemType itemType = dailyRewardData.RewardItems[0].Type;
                    if(this.CorCountDown != null){
                        StopCoroutine(this.CorCountDown);
                    }
                    this.CorCountDown = StartCoroutine(this.IECountDown(true));
                }

            }else{
                this.ButtonClaim.gameObject.SetActive(true);
                this.TextCountDown.text = "";
                DateDailyRewardItem dateDailyRewardItem = this.DateDailyRewardItems.Find(x => x.DailyRewardData.Day == dailyReward.Day);
                ItemType itemType = dateDailyRewardItem.ItemDataUI.ItemData.Type;
            }

            DailyRewardData lastDailyRewardData = DailyRewardManager.Instance.GetDailyRewardData(20);
            for(int i = 0; i < this.ItemDataBars.Count; i++){
                this.ItemDataBars[i].SetData(lastDailyRewardData.RewardItems[lastDailyRewardData.RewardItems.Length - 1 - i], true, true);
            }
            string str = LeanLocalization.GetTranslationText("day_count", "Day {0}");
            this.TextLastDay.text = string.Format(str, lastDailyRewardData.Day+1);
        }

        public void OnClickClaim(){

            AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
            if (LevelPlayAds.Instance.IsCanShowAds())
            {
                PopupManager.Instance.OnUI(PopupCode.WatchAdsUI, null, (popup) => {
                    WatchAdsUI watchAds = (WatchAdsUI)popup;
                    watchAds.claimAction = () =>
                    {

                        StartCoroutine(DailyRewardManager.Instance.IEClaimDailyReward(false, () =>
                        {
                            UpdateData();
                        }));
                    };
                    watchAds.doubleAction = () =>
                    {
                        StartCoroutine(DailyRewardManager.Instance.IEClaimDailyReward(true, () =>
                        {
                            UpdateData();
                        }));
                    };
                });
            }
            else
            {
                StartCoroutine(DailyRewardManager.Instance.IEClaimDailyReward(false, () =>
                {
                    UpdateData();
                }));
            }
           


        }

        public Coroutine CorCountDown;
        public IEnumerator IECountDown(bool isReset = false){
            while(true){
                string text = "";
                if(isReset){
                    text = Lean.Localization.LeanLocalization.GetTranslationText("daily_reward_reset", "Reset in:\n");
                }else{
                    text = Lean.Localization.LeanLocalization.GetTranslationText("daily_reward_countdown", "Claim in:\n");
                }
                this.TextCountDown.text = text + NTFunction.FormatTimeHour(ServerManager.Instance.GetNextTimeNewDay());
                yield return new WaitForSeconds(1);
            }
        }

    }
}