using System.Collections;
using System.Collections.Generic;
using NTPackage.Functions;
using NTPackage.UI;
using Rubik.ItemPlayer;
using TMPro;
using UnityEngine;

namespace Rubik.Myrk.Portal
{
    public class PortalOnMapRewardTab : NTBehaviour
    {
        public PortalOnMapRewardItem PortalOnMapRewardItemPrefab;
        public RectTransform Content;
        public List<PortalOnMapRewardItem> PortalOnMapRewardItems;
        public PortalOnMapUI PortalOnMapUI;

        public SelectionLevelUI SelectionLevelUI;
        public TextMeshProUGUI LevelText;
        public Transform TransLevel;
        public Transform BtnUnlock;
        public TextMeshProUGUI TextUnlock;

        public TextMeshProUGUI TextTimeRemain;

        public Coroutine CorColdDown;

        public int CacheReward = -1;

        public void OnUI(object data = null)
        {
            this.CacheReward = -1;
            if (!gameObject.activeSelf)
            {
                this.Content.anchoredPosition = new Vector2(this.Content.anchoredPosition.x, 0);
            }
            gameObject.SetActive(true);
            this.Clear();
            this.UpdateData();
            this.SelectionLevelUI.SetData(PortalWorldMapManager.Instance.GetHighestLevelPortal());
            this.SelectionLevelUI.ChangeLevel = null;
            this.SelectionLevelUI._OnclickLevel(this.PortalOnMapUI.Level);
            this.SelectionLevelUI.ChangeLevel = this.PortalOnMapUI.OnChangeLevel;
        }

        public void OffUI()
        {
            this.CacheReward = -1;
            this.SelectionLevelUI.Clear();
            gameObject.SetActive(false);
            if (this.CorColdDown != null)
            {
                this.StopCoroutine(this.CorColdDown);
            }
        }

        public void UpdateData()
        {
            this.Clear(false);
            if (!gameObject.activeSelf) return;

            this.TextUnlock.text = Lean.Localization.LeanLocalization.GetTranslationText("unlock", "Unlock") + "\n" +
                PortalWorldMapManager.Instance.GetKeyPortalRemain() + " " + ItemDataManager.Instance.GetItemTextSprite(ItemType.KeyPortal);

            this.TextTimeRemain.gameObject.SetActive(false);

            long coldDown = PortalWorldMapManager.Instance.GetRemainingTimeAttackPortal(this.PortalOnMapUI.PortalAttackData.OpenTime);

            if (this.PortalOnMapUI.IsClose)
            {
                this.SelectionLevelUI.gameObject.SetActive(true);
                this.TransLevel.gameObject.SetActive(false);
                this.BtnUnlock.gameObject.SetActive(true);
                this.TextTimeRemain.gameObject.SetActive(false);
            }
            else
            {
                this.SelectionLevelUI.gameObject.SetActive(false);
                this.TransLevel.gameObject.SetActive(true);
                this.LevelText.text = "Lv." + (this.PortalOnMapUI.Level + 1).ToString();
                this.BtnUnlock.gameObject.SetActive(false);
                if (coldDown > 0 || this.PortalOnMapUI.HP > 0)
                {
                    if (this.CorColdDown != null)
                    {
                        this.StopCoroutine(this.CorColdDown);
                    }
                    this.CorColdDown = this.StartCoroutine(this.IEAttackColdDown());
                }
                else
                {
                    if (this.PortalOnMapUI.HP < 1)
                    {
                        this.TextTimeRemain.gameObject.SetActive(true);
                        this.TextTimeRemain.text = Lean.Localization.LeanLocalization.GetTranslationText("portal_defeated", "Portal defeated!");
                    }
                    else
                    {
                        this.TextTimeRemain.gameObject.SetActive(true);
                        this.TextTimeRemain.text = Lean.Localization.LeanLocalization.GetTranslationText("portal_closed", "Portal closed!");
                    }
                }
            }
            this.UpdateReward();
        }

        public void UpdateReward()
        {
            if (this.CacheReward == this.PortalOnMapUI.Level)
            {
                return;
            }
            this.CacheReward = this.PortalOnMapUI.Level;
            RankBossReward[] rankBossRewards = PortalWorldMapManager.Instance.GetRankBossReward();
            float rewardMultiplier = PortalWorldMapManager.Instance.GetRewardMultiplier(this.PortalOnMapUI.Level);
            float fixRewardMultiplier = PortalWorldMapManager.Instance.GetFixRewardMultiplier(this.PortalOnMapUI.Level);
            foreach (RankBossReward rankBossReward in rankBossRewards)
            {
                PortalOnMapRewardItem portalOnMapRewardItem = ObjectPoolingManager.Instance.InstantiateObject<PortalOnMapRewardItem>(ObjectPoolingConfig.PortalOnMapRewardItem, this.PortalOnMapRewardItemPrefab.transform);
                portalOnMapRewardItem.SetData(rankBossReward, rewardMultiplier, fixRewardMultiplier);
                this.PortalOnMapRewardItems.Add(portalOnMapRewardItem);
                portalOnMapRewardItem.transform.SetParent(this.Content);
                NTFunction.ResetPosition(portalOnMapRewardItem.transform);
            }
        }

        public void Clear(bool isClearContent = true)
        {
            if (isClearContent)
            {
                foreach (PortalOnMapRewardItem portalOnMapRewardItem in this.PortalOnMapRewardItems)
                {
                    portalOnMapRewardItem.Clear();
                }
                this.PortalOnMapRewardItems.Clear();
                ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.Content);
            }
        }

        public void _OnClickUnlock()
        {
            if (ItemDataManager.Instance.GetItem(ItemType.KeyPortal).Amount <= 0)
            {
                ItemDataManager.Instance.ShowDontEnoughItem(ItemType.KeyPortal);
                return;
            }
            StartCoroutine(PortalWorldMapManager.Instance.IEUnlockPortal(this.PortalOnMapUI.Level, this.PortalOnMapUI.PointID));
        }

        public IEnumerator IEAttackColdDown()
        {
            while (true)
            {
                long coldDown = PortalWorldMapManager.Instance.GetRemainingTimeAttackPortal(this.PortalOnMapUI.PortalAttackData.OpenTime);
                if (coldDown > 0 && this.PortalOnMapUI.HP > 0)
                {
                    this.TextTimeRemain.gameObject.SetActive(true);
                    this.TextTimeRemain.text = Lean.Localization.LeanLocalization.GetTranslationText("portal_close_in", "Close in: ") + "<color=#BD7E92>" + NTFunction.FormatTimeHour(coldDown);
                }
                else
                {
                    if (this.PortalOnMapUI.HP < 1)
                    {
                        this.TextTimeRemain.gameObject.SetActive(true);
                        this.TextTimeRemain.text = Lean.Localization.LeanLocalization.GetTranslationText("portal_defeated", "Portal defeated!");
                    }
                    else
                    {
                        this.TextTimeRemain.gameObject.SetActive(true);
                        this.TextTimeRemain.text = Lean.Localization.LeanLocalization.GetTranslationText("portal_closed", "Portal closed!");
                    }
                    yield break;
                }
                yield return new WaitForSeconds(1f);
            }
        }
    }
}