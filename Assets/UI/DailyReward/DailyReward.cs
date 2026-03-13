using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTFunctions_old;
using TMPro;
using Rubik.Format;
public class DailyReward : PopupUI
{
    public float timeShow = 1f;
    public TextMeshProUGUI rewardTxt;
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.lvUI = new PopupLv().GetValue(transform.name);
    }

    public void OnUI()
    {
        this.Show();
        rewardTxt.text = GOA.UserData.UserData.Instance.dailyIncome.ToString() ;
        //StartCoroutine(this.AutoHide());
    }
    public override void Show()
    {
        base.Show();
        rewardTxt.text = FormatData.GetFriendlyShortNumber(GOA.UserData.UserData.Instance.dailyIncome);
    }

    protected override void ShowSound()
    {
        Rubik.Common.AudioHelper.AudioCtrl.Instance.Play(Rubik.Common.AudioHelper.AudioName.UI_Popup_Panel_Daily_Reward);
    }

    IEnumerator AutoHide()
    {
        yield return new WaitForSeconds(this.timeShow);
        this.OffUI();
    }
    public override void Hide()
    {
        base.Hide();
        Rubik.Common.AudioHelper.AudioCtrl.Instance.Play(Rubik.Common.AudioHelper.AudioName.UI_Button_Worldmap_2);
        GOA.UserData.UserData.Instance.data.Coin += GOA.UserData.UserData.Instance.dailyIncome;
    }
}
