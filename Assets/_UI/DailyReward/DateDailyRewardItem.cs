using NTPackage.Functions;
using NTPackage.UI;
using Rubik.CardPlayer;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rubik.Myrk.DailyReward
{
    using Lean.Localization;
    using Rubik.Common.AudioHelper;
    using Rubik.ItemPlayer;
    using Rubik.UserDataPlayer;

    public class DateDailyRewardItem : NTBehaviour
    {
        public Transform Empty;
        public Transform Claimed;
        public Transform Claim;
        public Transform OnClick;

        public ItemDataUI ItemDataUI;

        public DailyRewardData DailyRewardData;

        public DailyRewardUI DailyRewardUI;

        public TextMeshProUGUI TextDay;


        public void SetData(DailyRewardData dailyRewardData, DailyRewardUI dailyRewardUI)
        {
            this.DailyRewardData = dailyRewardData;
            this.DailyRewardUI = dailyRewardUI;
            this.UpdateData();
        }

        public void UpdateData()
        {
            int status = DailyRewardManager.Instance.GetStatusClaim(this.DailyRewardData.Day);
            this.Empty.gameObject.SetActive(false);
            this.Claimed.gameObject.SetActive(false);
            this.Claim.gameObject.SetActive(false);
            this.OnClick.gameObject.SetActive(false);
            switch (status)
            {
                case 0:
                    this.Empty.gameObject.SetActive(true);
                    break;
                case 1:
                    this.Claimed.gameObject.SetActive(true);
                    break;
                case 2:
                    this.Claim.gameObject.SetActive(true);
                    this.OnClick.gameObject.SetActive(true);
                    break;
            }
            this.ItemDataUI.SetData(this.DailyRewardData.RewardItems[0], true, true);
            string str = LeanLocalization.GetTranslationText("day_count", "Day {0}");
            this.TextDay.text = string.Format(str, this.DailyRewardData.Day+1);
        }

        public void OnClickClaim(){
            AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
          
            int status = DailyRewardManager.Instance.GetStatusClaim(this.DailyRewardData.Day);
            if(status == 2){
                if (UserDataManager.Instance.IsCapSlotInventoryBag())
                {
                    UserDataManager.Instance.ShowNotificationCapSlotInventoryBag();
                    return;
                }
                this.DailyRewardUI.OnClickClaim();
            }else{
                return;
            }
        }
    }
}