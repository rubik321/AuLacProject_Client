using NTPackage.Functions;
using NTPackage.UI;
using Rubik.CardPlayer;
using Rubik.Common.AudioHelper;
using Rubik.Myrk.Clan;
using Rubik.UI;
using UnityEngine;

public class ClanHomeUI : PopupUI
{
    public Transform bossPos;
    public override void OnUI(object data = null, bool isDefaultSound = true)
    {
        base.OnUI(data, isDefaultSound);
       
        
    }
    public override void UpdateData(object data = null)
    {
        base.UpdateData(data);
        if (ClanManager.Instance.IsClan())
        {
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.bossPos);
            Debug.Log(" Boss : " + ClanManager.Instance.GetBossCardPlayerIndex());
            var ske = CardPlayerManager.Instance.InstantiatePlayerSkeletonGraphic(ClanManager.Instance.GetBossCardPlayerIndex());
            ske.transform.SetParent(this.bossPos);
            NTFunction.ResetPosition(ske);
        }
    }
    public override void ScriptOffUI()
    {
        base.ScriptOffUI();
          ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.bossPos);
    }
    public void ShowClanUI()
    {
        AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
        PopupManager.Instance.OnUI(PopupCode.ClanUI);
    }
    public void ShowClanBossUI()
    {
        AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
        if (ClanManager.Instance.PlayerUserClan == null ||string.IsNullOrEmpty(ClanManager.Instance.PlayerUserClan.ClanId))
        {
            HUDCanvas.Instance.ShowNotification("You don't have clan ! ", "Message", null);
        }
        else
            PopupManager.Instance.OnUI(PopupCode.ClanBossUI);
    }
    public void ShowClanQuestUI()
    {
        AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
        if (ClanManager.Instance.PlayerUserClan == null || string.IsNullOrEmpty(ClanManager.Instance.PlayerUserClan.ClanId))
        {
            HUDCanvas.Instance.ShowNotification("You don't have clan ! ", "Message", null);
        }
        else
            PopupManager.Instance.OnUI(PopupCode.ClanQuestUI);
    }
    public void ShowClanShopUI()
    {
        AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
        if (ClanManager.Instance.PlayerUserClan == null || string.IsNullOrEmpty(ClanManager.Instance.PlayerUserClan.ClanId))
        {
            HUDCanvas.Instance.ShowNotification("You don't have clan ! ", "Message", null);
        }
        else
            PopupManager.Instance.OnUI(PopupCode.ClanShopUI);
    }
    public void ShowClanRankUI()
    {
        AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
        PopupManager.Instance.OnUI(PopupCode.ClanUI,null,(popup)=> {
            ClanUI clanUI = popup as ClanUI;
            clanUI.ShowRankUI();
        });

    }
}
