using UnityEngine;

namespace Rubik.Myrk.BattlePass
{
    using NTPackage.UI;
    using Rubik.Common.AudioHelper;
    using Rubik.ItemPlayer;
    using Rubik.RewardData;
    using Rubik.UI;
    using TMPro;
    using UnityEngine.UI;
    public class BattlePassMilestoneData
    {
        public int Level;
        public ItemData FreeItem;
        public ItemData PremiumItem;
        public int Point;

        public BattlePassMilestoneData(int level, ItemData freeItem, ItemData premiumItem, int point)
        {
            this.Level = level;
            this.FreeItem = freeItem;
            this.PremiumItem = premiumItem;
            this.Point = point;
        }
    }

    public class BattlePassMilestoneItem : MonoBehaviour
    {
        public BattlePassRewardItem FreeItem;
        public BattlePassRewardItem PremiumItem;
        public TextMeshProUGUI TextPoint;

        public Transform NextSlide;
        public Image NextSlideImage;

        public BattlePassMilestoneData Data;
        public BattlePassMilestoneData NextData;

        public BattlePassStatus StatusFree;
        public BattlePassStatus StatusPremium;

        public void SetData(BattlePassMilestoneData data, BattlePassMilestoneData nextData)
        {
            this.Data = data;
            this.StatusFree = BattlePassManager.Instance.GetStatusFree(data.Level);
            this.StatusPremium = BattlePassManager.Instance.GetStatusPremium(data.Level);
            this.FreeItem.SetData(data.FreeItem, this.StatusFree);
            this.PremiumItem.SetData(data.PremiumItem, this.StatusPremium);
            this.TextPoint.text = data.Point.ToString();

            if (nextData != null)
            {
                this.NextData = nextData;
                this.NextSlide.gameObject.SetActive(true);
                long battlePassPoint = ItemDataManager.Instance.GetItem(ItemType.BattlePassPoint).Amount;
                if (battlePassPoint >= nextData.Point)
                {
                    this.NextSlideImage.fillAmount = 1;
                }
                else
                {
                    this.NextSlideImage.fillAmount = (float)(battlePassPoint - data.Point) / (float)(nextData.Point - data.Point);
                }
            }
            else
            {
                this.NextSlide.gameObject.SetActive(false);
            }
        }

        public void OnclickFreeItem()
        {
            AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
            if (this.StatusFree == BattlePassStatus.Claim)
            {
                RewardData rewardData = new RewardData();
                rewardData.Title = "battle_pass_free";
                rewardData.Items = new ItemData[] { this.Data.FreeItem };
                PopupManager.Instance.OnUI(PopupCode.MessageOptionAdvPanel, null, (data) =>
                {

                    MessageOptionAdvPanel messageOptionAdvPanel = data as MessageOptionAdvPanel;
                    messageOptionAdvPanel.SetReward(rewardData);
                    messageOptionAdvPanel.SetAgreeAction(() =>
                    {
                        PopupUI popupUI = PopupManager.Instance.GetPopupUIByCode(PopupCode.BattlePassUI);
                        BattlePassUI battlePassUI = popupUI as BattlePassUI;
                        battlePassUI.OnclickFreeItem(this.Data.Level, false);
                    }, Lean.Localization.LeanLocalization.GetTranslationText("btn_claim", "Claim"));
                    messageOptionAdvPanel.SetAgreeAdvAction(() =>
                    {
                        PopupUI popupUI = PopupManager.Instance.GetPopupUIByCode(PopupCode.BattlePassUI);
                        BattlePassUI battlePassUI = popupUI as BattlePassUI;
                        battlePassUI.OnclickFreeItem(this.Data.Level, true);
                    }, "x2");
                });
            }
            else
            {
                ItemDataManager.Instance.ShowToolTip(this.Data.FreeItem.Type);
            }
        }

        public void OnclickPremiumItem()
        {
            AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
            if (BattlePassManager.Instance.IsPremium())
            {
                if (this.StatusPremium == BattlePassStatus.Claim)
                {
                    RewardData rewardData = new RewardData();
                    rewardData.Title = "battle_pass_prenium";
                    rewardData.Items = new ItemData[] { this.Data.PremiumItem };
                    PopupManager.Instance.OnUI(PopupCode.MessageOptionAdvPanel, null, (data) =>
                    {

                        MessageOptionAdvPanel messageOptionAdvPanel = data as MessageOptionAdvPanel;
                        messageOptionAdvPanel.SetReward(rewardData);
                        messageOptionAdvPanel.SetAgreeAction(() =>
                        {
                            PopupUI popupUI = PopupManager.Instance.GetPopupUIByCode(PopupCode.BattlePassUI);
                            BattlePassUI battlePassUI = popupUI as BattlePassUI;
                            battlePassUI.OnclickPremiumItem(this.Data.Level, false);
                        }, Lean.Localization.LeanLocalization.GetTranslationText("btn_claim", "Claim"));
                        messageOptionAdvPanel.SetAgreeAdvAction(() =>
                        {
                            PopupUI popupUI = PopupManager.Instance.GetPopupUIByCode(PopupCode.BattlePassUI);
                            BattlePassUI battlePassUI = popupUI as BattlePassUI;
                            battlePassUI.OnclickPremiumItem(this.Data.Level, true);
                        }, "x2");
                    });
                }
                else
                {
                    ItemDataManager.Instance.ShowToolTip(this.Data.PremiumItem.Type);
                }
            }
            else
            {
                BattlePassManager.Instance.UnlockPremium();
            }
        }
    }
}
