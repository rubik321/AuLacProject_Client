using NTPackage.Functions;
using NTPackage.UI;
using Rubik.CardPlayer;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rubik.Myrk.Outpost
{
    using ItemPlayer;
    using Rubik.Combat;
    using Rubik.Config;
    using Rubik.Myrk.Battle;
    using Rubik.Myrk.Portal;
    using Rubik.UI;
    using Rubik.UIController;
    using System;
    using Rubik.Myrk.Monster;
    using Rubik.BattleEngine;

    public class OutpostOnMapUIConfig
    {
        public const string OnUI = "OnUI";
        public const string OffUI = "OffUI";
        public const string Attack = "Attack";
    }

    public class OutpostOnMapUI : PopupUI
    {
        public long TileX;
        public long TileY;

        public TextMeshProUGUI Title;
        public MonsterAttackData MonsterAttackData;
        public List<MonsterOnMapCardItem> monsterOnMapCardItems;
        public TextMeshProUGUI TextPower;

        public MonsterOnMapRewardItemUI MonsterOnMapRewardItemUIPrefab;
        public Transform HolderReward;

        public NTButtonEffect CheckSkipBattle;

        public Animator Anim;

        public override void OnUI(object data = null, bool isDefaultSound = true)
        {
            base.OnUI(data, isDefaultSound);
            this.Title.text = Lean.Localization.LeanLocalization.GetTranslationText("title_outpost", "Outpost");
            this.Anim.Play(MonsterOnMapUIConfig.OnUI);
            if (MonsterManager.Instance.IsSkipBattle())
            {
                this.CheckSkipBattle.Chose();
            }
            else
            {
                this.CheckSkipBattle.Unchose();
            }
        }

        public override void ScriptOffUI()
        {
            this.Anim.Play(MonsterOnMapUIConfig.OffUI);
            StartCoroutine(this.OffPlayerMailAnim());
        }

        public void SetData(long tileX, long tileY)
        {
            int totalPower = 0;

            this.TileX = tileX;
            this.TileY = tileY;
            this.MonsterAttackData = OutpostWorldMapManager.Instance.GetMonsterAttackData(this.TileX, this.TileY);

            for (int i = 0; i < this.MonsterAttackData.Monsters.Length; i++)
            {
                this.monsterOnMapCardItems[i].SetData(this.MonsterAttackData.Monsters[i]);
                if (this.MonsterAttackData.Monsters[i] == null || this.MonsterAttackData.Monsters[i]._id == null || this.MonsterAttackData.Monsters[i]._id == "")
                {
                    continue;
                }
                totalPower += MonsterManager.Instance.GetPower(this.MonsterAttackData.Monsters[i].Index, this.MonsterAttackData.Monsters[i].Lv);
            }
            totalPower = (int)((float)totalPower * this.MonsterAttackData.ScaleMonster);
            this.TextPower.text = totalPower.ToString();

            List<ItemData> rewardItem = OutpostWorldMapManager.Instance.GetOutpostReward();

            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.HolderReward);
            foreach (ItemData itemData in rewardItem)
            {
                MonsterOnMapRewardItemUI monsterOnMapRewardItemUI = ObjectPoolingManager.Instance.InstantiateObject<MonsterOnMapRewardItemUI>(ObjectPoolingConfig.MonsterOnMapRewardItemUI, this.MonsterOnMapRewardItemUIPrefab.transform);
                monsterOnMapRewardItemUI.SetData(itemData);
                monsterOnMapRewardItemUI.transform.SetParent(this.HolderReward);
                NTFunction.ResetPosition(monsterOnMapRewardItemUI.transform);
            }
        }
        public void _OnclickAttack()
        {
            StartCoroutine(OutpostWorldMapManager.Instance.IEAttack(this.TileX, this.TileY, this.MonsterAttackData, (data) =>
            {
                PopupManager.Instance.OnUI(PopupCode.BattleLoadingUI, null, (popup) =>
                {
                    BattleLoadingUI battleLoadingUI = popup as BattleLoadingUI;
                    battleLoadingUI.SetData(BattleType.Outpost, null, this.MonsterAttackData.Monsters[4]);
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
            }));
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
            else
            {
                SceneController.Instance.LoadScene(SceneConfig.Battle_Screen);
            }
            base.OffUI();
        }

        public void _OnclickCheckSkipBattle()
        {
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

        public void _OnclickOutpostOnMapReward(){
            PopupManager.Instance.OnUI(PopupCode.OutpostOnMapRewardUI, null, (popup) =>
            {
                
            });
        }
    }
}