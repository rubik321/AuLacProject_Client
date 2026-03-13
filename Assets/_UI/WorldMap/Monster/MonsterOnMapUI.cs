using NTPackage.Functions;
using NTPackage.UI;
using Rubik.CardPlayer;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rubik.Myrk.Monster
{
    using ItemPlayer;
    using Rubik.BattleEngine;
    using Rubik.Combat;
    using Rubik.Common.AudioHelper;
    using Rubik.Config;
    using Rubik.Myrk.Battle;
    using Rubik.Myrk.BattleTeam;
    using Rubik.Myrk.Portal;
    using Rubik.UI;
    using Rubik.UIController;
    using Rubik.UserDataPlayer;
    using System;

    public class MonsterOnMapUIConfig
    {
        public const string OnUI = "OnUI";
        public const string OffUI = "OffUI";
        public const string Attack = "Attack";
    }

    public class MonsterOnMapUI : PopupUI
    {
        public TextMeshProUGUI Title;
        public MonsterOnMapData MonsterOnMapData;
        public MonsterAttackData MonsterAttackData;
        public List<MonsterOnMapCardItem> monsterOnMapCardItems;
        public TextMeshProUGUI TextPower;

        public MonsterOnMapRewardItemUI MonsterOnMapRewardItemUIPrefab;
        public Transform HolderReward;

        public MonsterOnMapRewardBarItemUI MonsterOnMapRewardBarItemUIPrefab;
        public Transform HolderRewardBar;

        public NTButtonEffect CheckSkipBattle;

        public Animator Anim;

        public ItemDataBarUI CostEnergy;

        public bool DontEnoughEnergy = false;

        public NTButtonEffect AdvAttackBtn;
        public ItemDataBarUI OfferedAdv;
        public Transform btnAdv;
        public TextMeshProUGUI TextUpperAdv;

        public long Power;

        public override void OnUI(object data = null, bool isDefaultSound = true)
        {
            base.OnUI(data, isDefaultSound);
            this.Title.text = Lean.Localization.LeanLocalization.GetTranslationText("title_mosters", "Monster");
            this.Anim.Play(MonsterOnMapUIConfig.OnUI);
            if (MonsterManager.Instance.IsSkipBattle())
            {
                this.CheckSkipBattle.Chose();
            }
            else
            {
                this.CheckSkipBattle.Unchose();
            }
            this.CostEnergy.SetData(MonsterManager.Instance.GetCostEnergy(), false, true);
            this.CostEnergy.AddMinus();
            if (ItemDataManager.Instance.GetItem(ItemType.Energy).Amount < MonsterManager.Instance.GetCostEnergy().Amount)
            {
                this.DontEnoughEnergy = true;
                this.CostEnergy.Amount.color = Color.red;
            }
            else
            {
                this.DontEnoughEnergy = false;
                this.CostEnergy.Amount.color = Color.white;
            }
            if (AssetLoader.Instance.IsTut)
            {
                var temp = PopupManager.Instance.GetPopupUI(PopupCode.TutorialUI).GetComponent<TutorialUI>();
                if (temp.isCanNextTut)
                    PopupManager.Instance.OnUI(PopupCode.TutorialUI);
                AssetLoader.Instance.IsTut = false;
            }
            if (LevelPlayAds.Instance.IsCanShowAds())
            {
                int remainAdv = UserDataManager.Instance.GetRemainDailyAdvLimit(AdvLimitDataConfig.BattleAddEnergy);
                this.TextUpperAdv.text = Lean.Localization.LeanLocalization.GetTranslationText("remains", "Remains: ") + remainAdv;
                this.AdvAttackBtn.gameObject.SetActive(true);
                this.OfferedAdv.SetData(MonsterManager.Instance.BattleMonsterWorldMapData.OfferedAdv);
                this.OfferedAdv.AddPlus();
                if(remainAdv > 0)
                {
                    this.AdvAttackBtn.Chose();
                }
                else
                {
                    this.AdvAttackBtn.Unchose();
                }
            }
            else
            {
                this.AdvAttackBtn.gameObject.SetActive(false);
            }
        }

        public override void ScriptOffUI()
        {
            this.Anim.Play(MonsterOnMapUIConfig.OffUI);
            StartCoroutine(this.OffPlayerMailAnim());
        }

        public void SetData(MonsterOnMapData monsterOnMapData)
        {
            int totalPower = 0;
            this.MonsterOnMapData = monsterOnMapData;
            this.MonsterAttackData = new MonsterAttackData();
            this.MonsterAttackData.Monsters = MonsterManager.Instance.GetMonsterData(monsterOnMapData).ToArray();
            this.MonsterAttackData.TeamID = monsterOnMapData._id;
            this.MonsterAttackData.AttackType = this.MonsterOnMapData.AttackType;
            this.MonsterAttackData.ScaleMonster = MonsterManager.Instance.GetScaleMonsterByLevel(this.MonsterOnMapData.AttackType);

            if (this.MonsterAttackData.AttackType == AttackType.Shard)
            {
                this.Title.text = Lean.Localization.LeanLocalization.GetTranslationText("title_great_monster", "Elite");
            }

            for (int i = 0; i < this.MonsterAttackData.Monsters.Length; i++)
            {
                this.monsterOnMapCardItems[i].SetData(this.MonsterAttackData.Monsters[i]);
                if (this.MonsterAttackData.Monsters[i] == null || this.MonsterAttackData.Monsters[i]._id == null || this.MonsterAttackData.Monsters[i]._id == "")
                {
                    continue;
                }
                totalPower += MonsterManager.Instance.GetPower(this.MonsterAttackData.Monsters[i].Index, this.MonsterAttackData.Monsters[i].Lv, this.MonsterAttackData.Monsters[i].Star);
            }
            totalPower = (int)((float)totalPower * this.MonsterAttackData.ScaleMonster);
            this.TextPower.text = totalPower.ToString();

            RewardItem_Rate rewardItem_Rate = MonsterManager.Instance.GetMonsterReward(this.MonsterAttackData);

            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.HolderReward);
            foreach (ItemRate itemRate in rewardItem_Rate.Rates)
            {
                MonsterOnMapRewardItemUI monsterOnMapRewardItemUI = ObjectPoolingManager.Instance.InstantiateObject<MonsterOnMapRewardItemUI>(ObjectPoolingConfig.MonsterOnMapRewardItemUI, this.MonsterOnMapRewardItemUIPrefab.transform);
                monsterOnMapRewardItemUI.SetData(itemRate);
                monsterOnMapRewardItemUI.transform.SetParent(this.HolderReward);
                NTFunction.ResetPosition(monsterOnMapRewardItemUI.transform);
            }

            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.HolderRewardBar);
            foreach (ItemData itemData in rewardItem_Rate.Items)
            {
                MonsterOnMapRewardBarItemUI monsterOnMapRewardBarItemUI = ObjectPoolingManager.Instance.InstantiateObject<MonsterOnMapRewardBarItemUI>(ObjectPoolingConfig.MonsterOnMapRewardBarItemUI, this.MonsterOnMapRewardBarItemUIPrefab.transform);
                monsterOnMapRewardBarItemUI.SetData(itemData);
                monsterOnMapRewardBarItemUI.transform.SetParent(this.HolderRewardBar);
                NTFunction.ResetPosition(monsterOnMapRewardBarItemUI.transform);
            }

            BattleShortTeam battleShortTeam = MonsterManager.Instance.GetBattleShortTeam(this.MonsterAttackData);
            this.Power = BattleTeamManager.Instance.GetPower(battleShortTeam, this.MonsterAttackData.ScaleMonster);
        }
        public void _OnclickAttack()
        {
            AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
            if (this.DontEnoughEnergy)
            {
                ItemDataManager.Instance.ShowDontEnoughItem(MonsterManager.Instance.GetCostEnergy().Type);
                return;
            }
            if (UserDataManager.Instance.IsCapSlotInventoryBag())
            {
                UserDataManager.Instance.ShowNotificationCapSlotInventoryBag();
                return;
            }
            MonsterManager.Instance.AttackMonster(this.MonsterAttackData, () =>
            {
                if (AssetLoader.Instance.IsTut)
                {
                    PopupManager.Instance.GetPopupUI(PopupCode.TutorialUI).OffUI();

                    AssetLoader.Instance.IsTut = false;
                }
                PopupManager.Instance.OnUI(PopupCode.BattleLoadingUI, null, (popup) =>
                {
                    BattleLoadingUI battleLoadingUI = popup as BattleLoadingUI;
                    battleLoadingUI.SetData(BattleType.Map, null, this.MonsterAttackData.Monsters[4]);
                    StartCoroutine(NTFunction.WaitSecond(1f, () =>
                    {
                        if (MonsterManager.Instance.IsSkipBattle())
                        {
                            battleLoadingUI.OffUI();
                            PopupManager.Instance.OnUI(PopupCode.EndGameUI, null, (popup) =>
                            {
                                EndGameUI endGameUI = popup as EndGameUI;
                                endGameUI.End(BattleEngine.BattleEngineController.Instance.BattleResult.IsWin);
                            });
                        }
                        else
                        {
                            SceneController.Instance.LoadScene(SceneConfig.Battle_Screen);
                        }
                    }));
                });

                this.OffUI();
            });
        }

        public void _OnclickAdvAttack()
        {
            if (UserDataManager.Instance.IsCapSlotInventoryBag())
            {
                UserDataManager.Instance.ShowNotificationCapSlotInventoryBag();
                return;
            }
            AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
            if (UserDataManager.Instance.GetRemainDailyAdvLimit(AdvLimitDataConfig.BattleAddEnergy) <= 0) {
                HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("adv_reach_cap", "You have reached the daily adv limit!"));
                return;
            }
            LevelPlayAds.Instance.OnShowReward(() =>
            {
                MonsterManager.Instance.AdvAttackMonster(this.MonsterAttackData, () =>
                {
                    this.Anim.Play(MonsterOnMapUIConfig.Attack);
                    btnAdv.gameObject.SetActive(false);
                    WorldMapUIController.Instance.IncreaseEnergy(btnAdv.transform.position);
                    PopupManager.Instance.OnUI(PopupCode.BattleLoadingUI, null, (popup) =>
                    {
                        BattleLoadingUI battleLoadingUI = popup as BattleLoadingUI;
                        battleLoadingUI.SetData(BattleType.Map, null, this.MonsterAttackData.Monsters[4]);
                        StartCoroutine(NTFunction.WaitSecond(1f, () =>
                        {
                            if (MonsterManager.Instance.IsSkipBattle())
                            {
                                battleLoadingUI.OffUI();
                                PopupManager.Instance.OnUI(PopupCode.EndGameUI, null, (popup) =>
                                {
                                    EndGameUI endGameUI = popup as EndGameUI;
                                    endGameUI.End(BattleEngine.BattleEngineController.Instance.BattleResult.IsWin);
                                });
                            }
                            else
                            {
                                SceneController.Instance.LoadScene(SceneConfig.Battle_Screen);
                            }
                        }));
                });
                });
                this.OffUI();
            });
        }

        public IEnumerator OffPlayerMailAnim()
        {
            AnimationClip[] clips = this.Anim.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in clips)
            {
                if (clip.name == MonsterOnMapUIConfig.OffUI)
                {
                    yield return new WaitForSeconds(clip.length);
                    break;
                }
            }
            base.ScriptOffUI();
        }

        public IEnumerator OffAttackAnim()
        {
            AnimationClip[] clips = this.Anim.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in clips)
            {
                if (clip.name == MonsterOnMapUIConfig.Attack)
                {
                    yield return new WaitForSeconds(clip.length);
                    break;
                }
            }
            if (MonsterManager.Instance.IsSkipBattle())
            {
                PopupManager.Instance.OnUI(PopupCode.EndGameUI, null, (popup) =>
                {
                    EndGameUI endGameUI = popup as EndGameUI;
                    endGameUI.End(BattleEngine.BattleEngineController.Instance.BattleResult.IsWin);
                });
            }
            base.OffUI();
        }

        public void _OnclickCheckSkipBattle()
        {
            AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
            MonsterManager.Instance.SetSkipBattle();
            if (MonsterManager.Instance.IsSkipBattle())
            {
                this.CheckSkipBattle.Chose();
            }
            else
            {
                this.CheckSkipBattle.Unchose();
            }
        }
    }
}