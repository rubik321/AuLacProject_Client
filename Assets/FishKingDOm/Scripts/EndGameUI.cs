using DG.Tweening;
using GOA.Config;
using NTPackage.UI;
using Rubik._2DGPS.Campaign;
using Rubik.BattleEngine;
using Rubik.Common.AudioHelper;
using Rubik.Config;
using Rubik.ItemPlayer;
using Rubik.Myrk.Battle;
using Rubik.Myrk.Monster;
using Rubik.UI;
using Rubik.UserDataPlayer;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EndGameUI : PopupUI
{
    public GameObject win, lose, end, rewardGo,lightGo,x2Reward;
    public GameObject[] lsRewards;


    public override void OnUI(object data = null, bool isDefaultSound = true)
    {
        base.OnUI(data, isDefaultSound);
    }
    public void End(bool isWin)
    {
        // ShowNone(); 
        //OnUI();
        MonsterManager.Instance.ResultAttackMonster(() =>
        {
            foreach (GameObject go in lsRewards)
            {
                go.SetActive(false);
            }
            lightGo.gameObject.SetActive(false);
            // UIGamePlayBattle.Instance.ExitPanelButton();
            gameObject.SetActive(true);
            ShowNone();
            win.gameObject.SetActive(false);
            lose.gameObject.SetActive(false);
            end.gameObject.SetActive(false);
            x2Reward.SetActive(false);
            if (isWin)
            {
                AudioCtrl.Instance.Play(AudioName.UI_Button_Battle_Victory);
                win.gameObject.SetActive(true);
                DOVirtual.DelayedCall(1, () =>
                {
                    lightGo.gameObject.SetActive(true);
                });
                lose.gameObject.SetActive(false);
                // win.AnimationState.SetAnimation(0, "Win", false);
                if (StaticData.GameMode == GameMode.Adventure)
                    //CampaignManager.instance.PassCampaign(UserDataManager.instance.UserData.CampaignLv+1);
                    rewardGo.SetActive(true);
                var battleData = BattleEngineController.Instance.ResultAttackMonsterResponse.ItemReward;
                int index = 0;
                foreach (Rubik.ItemPlayer.ItemData data in battleData)
                {
                    lsRewards[index].GetComponentInChildren<ItemDataUI>().SetData(data, true, true);
                    lsRewards[index].SetActive(true);
                    index++;
                }
                if (LevelPlayAds.Instance.IsCanShowAds())
                {
                    if(UserDataManager.Instance.GetRemainDailyAdvLimit(AdvLimitDataConfig.BattleX2Reward) > 0)
                    {
                        x2Reward.SetActive(true);
                    }
                    else
                    {
                        x2Reward.SetActive(false);
                    }
                }
            }
            else
            {
                AudioCtrl.Instance.Play(AudioName.UI_Button_Battle_Defeat);
                x2Reward.SetActive(false);
                win.gameObject.SetActive(false);
                lose.gameObject.SetActive(true);
                rewardGo.SetActive(true);
                // lose.AnimationState.SetAnimation(0, "animation", false);
            }
           
        });

    }
    public void EndBoss(bool isWin)
    {
       
            foreach (GameObject go in lsRewards)
            {
                go.SetActive(false);
            }
            // UIGamePlayBattle.Instance.ExitPanelButton();
            gameObject.SetActive(true);
        ShowNone();
        win.gameObject.SetActive(false);
            lose.gameObject.SetActive(false);
            x2Reward.SetActive(false);
        end.gameObject.SetActive(true);
        if (isWin)
            {
            AudioCtrl.Instance.Play(AudioName.UI_Button_Battle_Victory);
            win.gameObject.SetActive(true);
                lose.gameObject.SetActive(false);
                // win.AnimationState.SetAnimation(0, "Win", false);
                if (StaticData.GameMode == GameMode.Adventure)
                    //CampaignManager.instance.PassCampaign(UserDataManager.instance.UserData.CampaignLv+1);
                    rewardGo.SetActive(true);
                var battleData = BattleEngineController.Instance.ClanBattleResult.ItemReward;
                int index = 0;
                foreach (Rubik.ItemPlayer.ItemData data in battleData)
                {
                    lsRewards[index].SetActive(true);
                    lsRewards[index].GetComponentInChildren<ItemDataUI>().SetData(data, true, true);
                    index++;
                }
            }
            else
            {
            AudioCtrl.Instance.Play(AudioName.UI_Button_Battle_Defeat);
            win.gameObject.SetActive(false);
            lose.gameObject.SetActive(false);
            rewardGo.SetActive(true);
            var battleData = BattleEngineController.Instance.ClanBattleResult.ItemReward;
            int index = 0;
            foreach (Rubik.ItemPlayer.ItemData data in battleData)
            {
                lsRewards[index].SetActive(true);
                lsRewards[index].GetComponentInChildren<ItemDataUI>().SetData(data, true, true);
                index++;
            }
            // lose.AnimationState.SetAnimation(0, "animation", false);
        }
        var eventValues = new Dictionary<string, string>()
            {
                { "Clan_boss_battle_result",isWin.ToString() },
                  { "Clan_boss_battle_index","1" }
            }
            ;

        AppsFlyerManager.SendEvent("Clan_boss_battle", eventValues);

    }
    public void EndPortal(bool isWin)
    {
        AudioCtrl.Instance.Play(AudioName.UI_Button_Battle_Victory);
        foreach (GameObject go in lsRewards)
        {
            go.SetActive(false);
        }
        // UIGamePlayBattle.Instance.ExitPanelButton();
        ShowNone();
        gameObject.SetActive(true);
        win.gameObject.SetActive(false);
        lose.gameObject.SetActive(false);
        x2Reward.SetActive(false);
        rewardGo.SetActive(false);
        end.gameObject.SetActive(true);
      


    }
    public void EndOutPost(bool isWin)
    {
        AudioCtrl.Instance.Play(AudioName.UI_Button_Battle_Victory);
        foreach (GameObject go in lsRewards)
        {
            go.SetActive(false);
        }
        // UIGamePlayBattle.Instance.ExitPanelButton();
        ShowNone();
        gameObject.SetActive(true);
        win.gameObject.SetActive(false);
        lose.gameObject.SetActive(false);
        x2Reward.SetActive(false);
        rewardGo.SetActive(false);
        end.gameObject.SetActive(false);
        if (isWin)
        {

            win.gameObject.SetActive(true);
            DOVirtual.DelayedCall(1, () =>
            {
                lightGo.gameObject.SetActive(true);
            });
            lose.gameObject.SetActive(false);
            rewardGo.SetActive(true);
            var battleData = BattleEngineController.Instance.OutpostBattleResult.Reward;
            int index = 0;
            foreach (Rubik.ItemPlayer.ItemData data in battleData)
            {
                lsRewards[index].SetActive(true);
                lsRewards[index].GetComponentInChildren<ItemDataUI>().SetData(data, true, true);
                index++;
            }
            //if (LevelPlayAds.Instance.IsCanShowAds())
            //{
            //    x2Reward.SetActive(true);
            //}
            AppsFlyerManager.TrackingEvent(AppsflyerEvents.capture_fortress, 1, 1);
        }
        else
        {
            x2Reward.SetActive(false);
            win.gameObject.SetActive(false);
            lose.gameObject.SetActive(true);
            rewardGo.SetActive(false);
            // lose.AnimationState.SetAnimation(0, "animation", false);
        }

    }
    public void EndArena(bool isWin)
    {
        AudioCtrl.Instance.Play(AudioName.UI_Button_Battle_Victory);
        foreach (GameObject go in lsRewards)
        {
            go.SetActive(false);
        }
        // UIGamePlayBattle.Instance.ExitPanelButton();
        ShowNone();
        gameObject.SetActive(true);
        win.gameObject.SetActive(false);
        lose.gameObject.SetActive(false);
        x2Reward.SetActive(false);
        rewardGo.SetActive(false);
        end.gameObject.SetActive(false);
        AppsFlyerManager.TrackingArenaEnd(isWin?"win":"lose");

        if (isWin)
        {
            win.gameObject.SetActive(true);
            DOVirtual.DelayedCall(1, () =>
            {
                lightGo.gameObject.SetActive(true);
            });
            lose.gameObject.SetActive(false);
            rewardGo.SetActive(true);
            var battleData = BattleEngineController.Instance.ArenaBattleResult.Reward;
            int index = 0;
            foreach (Rubik.ItemPlayer.ItemData data in battleData)
            {
                lsRewards[index].SetActive(true);
                lsRewards[index].GetComponentInChildren<ItemDataUI>().SetData(data, true, true);
                index++;
            }
            //if (LevelPlayAds.Instance.IsCanShowAds())
            //{
            //    x2Reward.SetActive(true);
            //}
        }
        else
        {
            x2Reward.SetActive(false);
            win.gameObject.SetActive(false);
            lose.gameObject.SetActive(true);
            rewardGo.SetActive(false);
            // lose.AnimationState.SetAnimation(0, "animation", false);
        }
       
            var eventValues = new Dictionary<string, string>()
            {
                { "arena_battle_result",isWin.ToString() }
            }
            ;

            AppsFlyerManager.SendEvent("arena_battle", eventValues);
        
    }
    public void Menu()
    {
        AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
        if (StaticData.GameMode == GameMode.Adventure)
        {
            bl_SceneLoaderManager.LoadScene(Rubik.Config.Configs.Campaign_Screen);
            this.OffUI();
        }
        else
        {
            // Check scene
            if (SceneManager.GetActiveScene().name == SceneConfig.WorldMap_Screen)
            {

            }
            else
            {
                SceneController.Instance.LoadScene(SceneConfig.WorldMap_Screen);
            }
            this.OffUI();
        }
    }

    public void _OnclickStatistic()
    {
        AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
        PopupManager.Instance.OnUI(PopupCode.BattleResultUI, null, (popup) =>
        {

        });
    }
    public void DoubleReward()
    {
        AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
        if (UserDataManager.Instance.GetRemainDailyAdvLimit(AdvLimitDataConfig.BattleX2Reward) <= 0) {
            HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("adv_reach_cap", "You have reached the daily adv limit!"));
            return;
        }
        LevelPlayAds.Instance.OnShowReward(() =>
        {
            AppsFlyerManager.TrackingAds("End_game_x2_reward");
            x2Reward.SetActive(false);
            StartCoroutine(MonsterManager.Instance.IEDoubleReward(() => {
                var battleData = BattleEngineController.Instance.ResultAttackMonsterResponse.ItemReward;
                int index = 0;
                foreach (Rubik.ItemPlayer.ItemData data in battleData)
                {
                    lsRewards[index].SetActive(true);
                    data.Amount = 2*data.Amount;
                    lsRewards[index].GetComponentInChildren<ItemDataUI>().SetData(data, true, true);
                    index++;
                }

            }));
        });
       
    }
    public void _OnclickReplay()
    {
        SceneController.Instance.LoadScene(SceneConfig.Battle_Screen);
        this.OffUI();
    }

}
