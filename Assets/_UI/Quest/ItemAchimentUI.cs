using GOA.UserData;
using Lean.Localization;
using NTPackage.Functions;
using NTPackage.UI;
using Rubik.ItemPlayer;
using Rubik.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rubik.Quest
{
    using Rubik.CardPlayer;
    using Rubik.Common.AudioHelper;
    using Rubik.UserDataPlayer;
    using System.Drawing;
    using static Rubik.UI.AuctionUI;

    public class ItemAchimentUI : NTBehaviour
    {
        [SerializeField] TextMeshProUGUI AchimentNameTxt, AchimentDescTxt, ProcessTxt;
        // [SerializeField] ListItemDataUI ListItemDataUI;
        [SerializeField]  List<ItemDataUI> ItemDataUIList;
        [SerializeField] Image BG, questIcon;

        [SerializeField] Sprite Processing, ProcessCompleted, ProcessMax;
        [SerializeField] NTButtonEffect BtnClaim;
        public AchievementPlayerReward AchievementPlayerReward;
        public AchievementPlayerIndex AchimentIndex;
        public void SetUp(AchievementPlayerIndex achimentIndex)
        {
            this.AchimentIndex = achimentIndex;
            UpdateData();
        }
        public void UpdateData()
        {
            AchimentNameTxt.text = AchievementManager.Instance.GetAchievementName(AchimentIndex);
            AchimentDescTxt.text = AchievementManager.Instance.GetAchievementDesc(AchimentIndex);
            questIcon.sprite = AchievementManager.Instance.GetAchiementSprite(AchimentIndex);
           // this.ListItemDataUI.Clear();
            this.AchievementPlayerReward = AchievementManager.Instance.GetAchievementReward(AchimentIndex);
            List<CardPlayer> cardPlayers = new List<CardPlayer>();
            foreach (CardPlayerIndex cardPlayerIndex in this.AchievementPlayerReward.Cards)
            {
                cardPlayers.Add(CardPlayerManager.Instance.GetFakeCardPlayerByIndex(cardPlayerIndex));
            }
           
           // this.ListItemDataUI.SetData(this.AchievementPlayerReward.Items, cardPlayers.ToArray(), true, true);
            ProcessTxt.text = SetColor( AchievementManager.Instance.GetAchievementProcess(AchimentIndex).ToString(), "B70C23") + "/" + SetColor(AchievementManager.Instance.GetAchievementMaxProcess(AchimentIndex).ToString(), "178C29" ) ;
            if (AchievementManager.Instance.GetAchievementProcess(AchimentIndex) < AchievementManager.Instance.GetAchievementMaxProcess(AchimentIndex))
            {
                BG.sprite = Processing;
                BtnClaim.SetActive(true);
                BtnClaim.Unchose();
            }
            else if (!AchievementManager.Instance.IsAchievementCap(AchimentIndex))
            {
                BtnClaim.SetActive(true);
                BG.sprite = ProcessCompleted;
                BtnClaim.gameObject.SetActive(true);
                BtnClaim.Chose();
            }
            else
            {
                BG.sprite = ProcessMax;
                BtnClaim.SetActive(false);
                BtnClaim.Unchose();
                this.ProcessTxt.text = LeanLocalization.GetTranslationText("btn_max", "Max");
            }
            int index = 0;
            foreach (Rubik.ItemPlayer.ItemData item in this.AchievementPlayerReward.Items)
            {
                //ItemDataUI itemDataUI = ObjectPoolingManager.Instance.InstantiateObject<ItemDataUI>(ObjectPoolingConfig.ItemDataUI, this.ItemDataUIPrefab.transform);
                //ItemDataUIList.transform.SetParent(this.transform);
                ItemDataUIList[index].SetData(item, true, true);
                //NTFunction.ResetPosition(itemDataUI.transform);
                //this.ItemDataUIList.Add(itemDataUI);
                index++;
            }
        }
        public string SetColor(string content, string hexColor)
        {
           // string hexColor = ColorUtility.ToHtmlStringRGB(color);
          var temp = $"<color=#{hexColor}>{content}</color>";
            return temp;
        }
        public void _OnClickGo()
        {
            // 
        }

        public void _OnClickClaim()
        {
            if (AchievementManager.Instance.GetAchievementProcess(AchimentIndex) < AchievementManager.Instance.GetAchievementMaxProcess(AchimentIndex))
            {
                AudioCtrl.Instance.Play(AudioName.Collect_Gem_Sound);
                HUDCanvas.Instance.ShowNotification(
                    AchievementManager.Instance.WarningNotDoneAchievement(AchimentIndex)
                    , AchievementManager.Instance.GetAchievementName(AchimentIndex)
                );
                return;
            }
            if (AchievementManager.Instance.IsAchievementCap(AchimentIndex))
            {
                return;
            }
            if (UserDataManager.Instance.IsCapSlotInventoryBag())
            {
                UserDataManager.Instance.ShowNotificationCapSlotInventoryBag();
                return;
            }
            AudioCtrl.Instance.Play(AudioName.Achievement_Sound);
            StartCoroutine(AchievementManager.Instance.IEClaimAchievement(AchimentIndex, () =>
            {
                UpdateData();
            }));
        }

        protected override void OnDestroy()
        {
            //this.ListItemDataUI.Clear();
            base.OnDestroy();
        }

        protected override void OnDisable()
        {
           // this.ListItemDataUI.Clear();
            base.OnDisable();
        }
    }
}