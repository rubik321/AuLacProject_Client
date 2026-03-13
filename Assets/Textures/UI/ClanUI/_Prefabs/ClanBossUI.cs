using Lean.Localization;
using NTPackage.Functions;
using NTPackage.UI;
using Rubik.BattleEngine;
using Rubik.CardPlayer;
using Rubik.Common.AudioHelper;
using Rubik.Config;
using Rubik.ItemPlayer;
using Rubik.Manager;
using Rubik.Myrk.Battle;
using Rubik.Myrk.Clan;
using Rubik.Myrk.Monster;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClanBossUI : PopupUI
{
    public CardPlayerIndex bossIndex;
    public List<MilestoneClanBossReward> lsRewardMilestones;
    public List<RateItemDataItem> lsRawardChess;
    public List<TextMeshProUGUI> lsAmountReward;
    public TextMeshProUGUI barProcessTxt;
    public Image[] barProcessImg;
    public TextMeshProUGUI timeLeft, ticketTxt;
    public long countDown;
    public Coroutine coroutineCountDown;
    public GameObject btnAdv;
    public Transform bossPos;
    public Sprite bgOn, bgOff;
    public override void OnUI(object data = null, bool isDefaultSound = true)
    {
        base.OnUI(data, isDefaultSound);
        this.bossIndex = ClanManager.Instance.GetBossCardPlayerIndex();
        this.countDown = ServerManager.Instance.GetNextTimeNewDay();
        if (this.coroutineCountDown != null)
        {
            StopCoroutine(this.coroutineCountDown);
        }
        this.coroutineCountDown = StartCoroutine(this.CotimeLeft());
        if (ClanManager.Instance.IsClaimClanBossReward())
        {
            StartCoroutine(ClanManager.Instance.IEClaimClanBossReward());
        }
        ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.bossPos);
        var ske = CardPlayerManager.Instance.InstantiatePlayerSkeletonGraphic(ClanManager.Instance.GetBossCardPlayerIndex());
        ske.transform.SetParent(this.bossPos);
        NTFunction.ResetPosition(ske);

    }

    public override void UpdateData(object data)
    {
        base.UpdateData(data);
        GetMilestoneClanBossReward();
        GetClanPlayer();
        GetRateItem();
        if (LevelPlayAds.Instance.IsCanShowAds() && ClanManager.Instance.IsClanBossAdv())
        {
            btnAdv.SetActive(true);
        }
        else
        {
            btnAdv.SetActive(false);
        }
        // textSummon.text = ItemDataManager.Instance.GetItem(ItemType.SummonToken).Amount.ToString();
    }

    public void AttackBoss()
    {
        AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
        StartCoroutine(ClanManager.Instance.IEAttackClanBoss(false, (result) =>
        {
            UpdateData(null);

            PopupManager.Instance.OnUI(PopupCode.BattleLoadingUI, null, (popup) =>
            {
                BattleLoadingUI battleLoadingUI = popup as BattleLoadingUI;
                MonsterData monsterData = new MonsterData(this.bossIndex, 0, 0, 1);
                battleLoadingUI.SetData(BattleType.Clan, null, monsterData);
                StartCoroutine(NTFunction.WaitSecond(1f, () =>
                {
                    if (MonsterManager.Instance.IsSkipBattle())
                    {
                        battleLoadingUI.OffUI();
                        PopupManager.Instance.OnUI(PopupCode.EndGameUI, null, (popup) =>
                        {
                            EndGameUI endGameUI = popup as EndGameUI;
                            endGameUI.End(BattleEngineController.Instance.BattleResult.IsWin);
                        });
                    }
                    else
                    {
                        SceneController.Instance.LoadScene(SceneConfig.Battle_Screen);
                    }
                }));
            });
        }));

    }
    public (int PlayerAttack, int[] PlayerReward, List<TimeAttackReward> Milestone) reward;
    [NTButton]
    public void GetMilestoneClanBossReward()
    {
        // ClanCreateData clandata = new ClanCreateData("Dat123");
        reward = ClanManager.Instance.GetMilestoneClanBossReward();
        int index = 0, amount = 0;
        barProcessTxt.text = reward.PlayerAttack.ToString();
        foreach (TimeAttackReward atkRw in reward.Milestone)
        {
            Debug.Log("Index : " + index);
            lsRewardMilestones[index].SetData(atkRw.Items[0]);
            lsAmountReward[index].text = atkRw.Amount.ToString();
            amount = atkRw.Amount;
            if (reward.PlayerAttack >= atkRw.Amount)
            {
                lsRewardMilestones[index].Background.sprite = bgOn;
                barProcessImg[index].fillAmount = 1;
            }
            else
            {
                lsRewardMilestones[index].Background.sprite = bgOff;
                if(index>0)
                    barProcessImg[index].fillAmount = (float)(reward.PlayerAttack - reward.Milestone[index-1].Amount) / (atkRw.Amount- reward.Milestone[index - 1].Amount);
                else
                    barProcessImg[index].fillAmount = (float)(reward.PlayerAttack ) / (atkRw.Amount );
            }
            index++;
        }
        barProcessTxt.text = reward.PlayerAttack + "/" + amount;
      //  barProcessImg[0].fillAmount = (float)reward.PlayerAttack / amount;
    }
    public void GetClanPlayer()
    {
        // ClanCreateData clandata = new ClanCreateData("Dat123");
        var reward = ClanManager.Instance.GetPlayerClanBossAttack();
        int index = 0;
        ticketTxt.text = reward.rest + "/" + reward.max;

    }
    public void GetRateItem()
    {
        foreach (RateItemDataItem item in lsRawardChess)
        {
            item.gameObject.SetActive(false);
        }
        var reward = ClanManager.Instance.GetItemRewardClanBoss();
        int index = 0;
        foreach (ItemRate item in reward)
        {
            Rubik.ItemPlayer.ItemData data = new Rubik.ItemPlayer.ItemData();
            data.Type = item.Type;
            data.Amount = item.Amount;
            lsRawardChess[index].SetData(data, item.Rate, true, true);
            lsRawardChess[index].gameObject.SetActive(true);
            index++;
        }
        // ticketTxt.text = reward.rest + "/" + reward.max;

    }
    IEnumerator CotimeLeft()
    {
        while (true)
        {
            this.countDown = this.countDown = ServerManager.Instance.GetNextTimeNewDay();
            timeLeft.text = LeanLocalization.GetTranslationText("time_left", "Time left ") + ": <color=#BD7E92>" + NTFunction.FormatTimeHour(this.countDown) + "</color>";
            yield return new WaitForSeconds(1);
        }
    }

    public void _OnclickClanBossRank()
    {
        PopupManager.Instance.OnUI(PopupCode.ClanBossRankUI);
    }
    public void WatchAdsToAttack()
    {
        AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
        LevelPlayAds.Instance.OnShowReward(() =>
        {
            AppsFlyerManager.TrackingAds("Clan_boss");
            StartCoroutine(ClanManager.Instance.IEAttackClanBoss(true, (result) =>
            {
                SceneController.Instance.LoadScene(SceneConfig.Battle_Screen);
                UpdateData(null);
                // PopupManager.Instance.OffAllPopupUI();
            }));
        });
    }
}
