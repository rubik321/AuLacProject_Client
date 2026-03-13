using System.Collections;
using System.Collections.Generic;
using NTPackage.Functions;
using NTPackage.UI;
using Rubik.BattleEngine;
using Rubik.Config;
using Rubik.Myrk.Battle;
using Rubik.Myrk.Monster;
using Rubik.UserDataPlayer;
using Rubik.UserProfile;
using TMPro;
using UnityEngine;

namespace Rubik.Myrk.Portal
{
    public class PortalOnMapRankTab : NTBehaviour
    {
        public PortalOnMapRankItem PortalOnMapRankItemPrefab;
        public Transform Content;
        public List<PortalOnMapRankItem> PortalOnMapRankItems;
        public PortalOnMapUI PortalOnMapUI;

        public Transform Rank;
        public Transform EmptyRank;

        public PortalOnMapRankItem PlayerRank;

        public TextMeshProUGUI TextRemainAttack;

        public NTButtonEffect ButtonAttack;
        public TextMeshProUGUI TextBtnAttack;
        public Coroutine CorColdDown;

        public void OnUI()
        {
            gameObject.SetActive(true);
            this.UpdateData();
        }

        public void OffUI()
        {
            gameObject.SetActive(false);
            this.Clear();
            if (this.CorColdDown != null)
            {
                this.StopCoroutine(this.CorColdDown);
            }
        }

        public void UpdateData()
        {
            if (!gameObject.activeSelf) return;
            this.ButtonAttack.Unchose();
            float rewardMultiplier = PortalWorldMapManager.Instance.GetRewardMultiplier(this.PortalOnMapUI.Level);
            float fixRewardMultiplier = PortalWorldMapManager.Instance.GetFixRewardMultiplier(this.PortalOnMapUI.Level);
            List<UserRank> userRanks = new List<UserRank>();
            List<string> userIDs = new List<string>();
            if (this.PortalOnMapUI.PortalBossRank == null || this.PortalOnMapUI.PortalBossRank.Rank == null)
            {
                this.EmptyRank.gameObject.SetActive(true);
                this.Rank.gameObject.SetActive(false);
                return;
            }
            foreach (UserRank userRank in this.PortalOnMapUI.PortalBossRank.Rank)
            {
                if (userRank.Score < 1) continue;
                userRanks.Add(userRank);
                userIDs.Add(userRank.UserID);
            }
            UserProfileManager.Instance.GetUserDataShorts(userIDs, (userProfiles) =>
            {
                this.Clear();
                foreach (UserRank userRank in userRanks)
                {
                    UserDataShort userDataShort = UserProfileManager.Instance.GetUserDataShortCache(userRank.UserID);
                    if (userDataShort == null) continue;
                    PortalOnMapRankItem portalOnMapRankItem = ObjectPoolingManager.Instance.InstantiateObject<PortalOnMapRankItem>(ObjectPoolingConfig.PortalOnMapRankItem, this.PortalOnMapRankItemPrefab.transform);
                    portalOnMapRankItem.SetData(userDataShort, userRank, PortalWorldMapManager.Instance.GetRankBossRewardByRank(userRank.Rank), rewardMultiplier, fixRewardMultiplier);
                    portalOnMapRankItem.transform.SetParent(this.Content);
                    this.PortalOnMapRankItems.Add(portalOnMapRankItem);
                    NTFunction.ResetPosition(portalOnMapRankItem.transform);
                }
            });
            if (userIDs.Count == 0)
            {
                this.EmptyRank.gameObject.SetActive(true);
                this.Rank.gameObject.SetActive(false);
            }
            else
            {
                this.EmptyRank.gameObject.SetActive(false);
                this.Rank.gameObject.SetActive(true);
            }

            this.PlayerRank.SetPlayerRank(this.PortalOnMapUI.PortalBossRank.PlayerRank, this.PortalOnMapUI.PortalBossRank.PlayerDamage, PortalWorldMapManager.Instance.GetRankBossRewardByRank(this.PortalOnMapUI.PortalBossRank.PlayerRank), rewardMultiplier, fixRewardMultiplier);

            if (this.CorColdDown != null)
            {
                this.StopCoroutine(this.CorColdDown);
            }
            this.CorColdDown = this.StartCoroutine(this.IEAttackColdDown());
        }

        public void Clear()
        {
            foreach (PortalOnMapRankItem portalOnMapRankItem in this.PortalOnMapRankItems)
            {
                portalOnMapRankItem.Clear();
            }
            this.PortalOnMapRankItems.Clear();
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.Content);
        }

        public IEnumerator IEAttackColdDown()
        {
            while (true)
            {
                this.ButtonAttack.Chose();
                long coldDown = PortalWorldMapManager.Instance.GetTimeRecoveryAttackPortal();
                (long amount, long lastTimeRecovery) = PortalWorldMapManager.Instance.RecoveryAttack();
                this.TextRemainAttack.text = Lean.Localization.LeanLocalization.GetTranslationText("portal_attack_remain", "Battle chances: ") + " " + PortalWorldMapManager.Instance.GetPortalAttackRemain();
                long timeRemain = PortalWorldMapManager.Instance.GetRemainingTimeAttackPortal(this.PortalOnMapUI.PortalAttackData.OpenTime);
                if (timeRemain <= 0)
                {
                    this.TextBtnAttack.text = Lean.Localization.LeanLocalization.GetTranslationText("portal_closed", "Closed");
                    this.ButtonAttack.Unchose();
                    this.TextRemainAttack.text = "";
                    yield break;
                }

                if (this.PortalOnMapUI.HP <= 0)
                {
                    this.TextBtnAttack.text = Lean.Localization.LeanLocalization.GetTranslationText("portal_defeated", "Defeated");
                    this.ButtonAttack.Unchose();
                    this.TextRemainAttack.text = "";
                    yield break;
                }

                if (amount < 1)
                {
                    this.ButtonAttack.Unchose();
                }
                this.TextBtnAttack.text = Lean.Localization.LeanLocalization.GetTranslationText("attack", "Attack!");

                if (coldDown > 0)
                {
                    this.TextRemainAttack.text = Lean.Localization.LeanLocalization.GetTranslationText("portal_attack_remain", "Battle chances: ") + " " + PortalWorldMapManager.Instance.GetPortalAttackRemain() + "<color=#BD7E92> (" + NTFunction.FormatTimeMinus(coldDown) + ")</color>";
                }
                else
                {
                    this.TextRemainAttack.text = Lean.Localization.LeanLocalization.GetTranslationText("portal_attack_remain", "Battle chances: ") + " " + PortalWorldMapManager.Instance.GetPortalAttackRemain() + "<color=#BD7E92> " + Lean.Localization.LeanLocalization.GetTranslationText("max", "(Max)") + "</color>";
                }
                yield return new WaitForSeconds(1f);
            }
        }

        public void _OnClickAttack()
        {
            if (!this.ButtonAttack.IsChose())
            {
                return;
            }
            StartCoroutine(PortalWorldMapManager.Instance.IEAttack(this.PortalOnMapUI.Level, this.PortalOnMapUI.PointID, (result) =>
            {
                PopupManager.Instance.OnUI(PopupCode.BattleLoadingUI, null, (popup) =>
                {
                    MonsterData monsterData = new MonsterData(this.PortalOnMapUI.CardPlayerIndex, 0, this.PortalOnMapUI.Level, 1);
                    BattleLoadingUI battleLoadingUI = popup as BattleLoadingUI;
                    battleLoadingUI.SetData(BattleType.Portal, null, monsterData);
                    StartCoroutine(NTFunction.WaitSecond(0.5f, () =>
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
            }));
        }
    }
}

// {
//   "PointID": 1,
//   "Version": 0,
//   "Level": 0,
//   "OpenTime": 1754628292
// }