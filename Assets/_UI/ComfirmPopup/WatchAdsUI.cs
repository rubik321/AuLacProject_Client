using NTPackage.UI;
using Rubik.Common.AudioHelper;
using System;
using UnityEngine;

public class WatchAdsUI : PopupUI
{
    public Action claimAction,doubleAction;
    public GameObject x2button;
    public override void OnUI(object data = null, bool isDefaultSound = true)
    {
        base.OnUI(data, isDefaultSound);

        x2button.SetActive(LevelPlayAds.Instance.IsCanShowAds());
        
    }
    public void ClaimButton()
    {
        AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
        claimAction.Invoke();
        OffUI();
    }
    public void DoubleReward()
    {
        AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
        LevelPlayAds.Instance.OnShowReward(() => {
            AppsFlyerManager.TrackingAds("Double_reward");
            doubleAction.Invoke();
            OffUI();
        });
        
    }

   
}
