using GOA.UserData;
using Lean.Localization;
using NTPackage.Functions;
using NTPackage.UI;
using Rubik.ItemPlayer;
using Rubik.UI;
using Rubik.UIController;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace Rubik.Quest
{
    using Rubik.Common.AudioHelper;
    using Rubik.ItemPlayer;
    using Rubik.UserDataPlayer;

    public class ItemQuestUI : NTBehaviour
    {
        [SerializeField] TextMeshProUGUI questNameTxt, questDescTxt, processTxt;
        [SerializeField] ListItemDataUI ListItemDataUI;

        [SerializeField] Image BG, questIcon;

        [SerializeField] Sprite processing, processCompleted, processClaimed;
        [SerializeField] NTButtonEffect BtnGo, BtnClaim, BtnClaimed;

        public QuestIndex questIndex;
        bool isClanQuest = false;
        public void SetUp(QuestIndex questIndex,bool isClanQuest = false)
        {
            this.questIndex = questIndex;
            this.isClanQuest = isClanQuest;
            UpdateData();
        }

        public void UpdateData()
        {

            questNameTxt.text = QuestManager.Instance.GetQuestName(questIndex);// QuestManager.Instance.GetQuestName(questIndex);
            questDescTxt.text = QuestManager.Instance.GetQuestDesc(questIndex);// QuestManager.Instance.GetQuestDesc(questIndex);
            questIcon.sprite = QuestManager.Instance.GetQuestSprite(questIndex);
            this.ListItemDataUI.Clear();
            this.ListItemDataUI.SetData(QuestManager.Instance.GetQuestReward(questIndex), null, true, true);
            processTxt.text = QuestManager.Instance.GetQuestProcess(questIndex) + "/" + QuestManager.Instance.GetQuestMaxProcess(questIndex);
            if (QuestManager.Instance.GetQuestProcess(questIndex) < QuestManager.Instance.GetQuestMaxProcess(questIndex))
            {
                BG.sprite = processing;
                BtnGo.gameObject.SetActive(true);
                BtnClaim.gameObject.SetActive(false);
                BtnClaimed.gameObject.SetActive(false);
            }
            else if (!QuestManager.Instance.IsQuestClaimed(questIndex))
            {
                BG.sprite = processCompleted;
                BtnGo.gameObject.SetActive(false);
                BtnClaim.gameObject.SetActive(true);
                BtnClaimed.gameObject.SetActive(false);
            }
            else
            {
                BG.sprite = processClaimed;
                BtnGo.gameObject.SetActive(false);
                BtnClaim.gameObject.SetActive(false);
                BtnClaimed.gameObject.SetActive(true);
            }
            //BtnGo.gameObject.SetActive(true);
        }

        public void _OnClickGo()
        {
            if (QuestManager.Instance.GetQuestProcess(questIndex) < QuestManager.Instance.GetQuestMaxProcess(questIndex))
            {
                HUDCanvas.Instance.ShowNotification(
                    QuestManager.Instance.WarningNotDoneQuest(questIndex)
                    , QuestManager.Instance.GetQuestName(questIndex)
                );
                return;
            }
        }

        public void _OnClickClaim()
        {
            AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
            if (UserDataManager.Instance.IsCapSlotInventoryBag())
            {
                UserDataManager.Instance.ShowNotificationCapSlotInventoryBag();
                return;
            }
            if (!LevelPlayAds.Instance.IsCanShowAds())
            {
                AudioCtrl.Instance.Play(AudioName.Achievement_Sound);
                StartCoroutine(QuestManager.Instance.IEClaimQuest(questIndex, false, () =>
                {

                    UpdateData();
                    foreach (ItemDataUI item in this.ListItemDataUI.ItemDataUIList)
                    {
                        if (item.ItemData.Type == ItemType.Energy)
                        {
                            //WorldMapUIController.Instance.IncreaseEnergy(BtnClaim.transform.position);
                            AudioCtrl.Instance.Play(AudioName.Collect_Energy_Sound);
                        }
                        if (item.ItemData.Type == ItemType.Coin)
                        {
                            AudioCtrl.Instance.Play(AudioName.Collect_Gold_Sound);
                           // WorldMapUIController.Instance.IncreaseEffect(BtnClaim.transform.position, true);
                        }
                        if (item.ItemData.Type == ItemType.Gem)
                        {
                            AudioCtrl.Instance.Play(AudioName.Collect_Gem_Sound);
                          //  WorldMapUIController.Instance.IncreaseEffect(BtnClaim.transform.position, false);
                        }
                    }
                }));
            }
            else
            {

                if (!isClanQuest)
                {
                    PopupManager.Instance.OnUI(PopupCode.WatchAdsUI, null, (popup) => {
                        WatchAdsUI watchAds = (WatchAdsUI)popup;
                        watchAds.claimAction = () =>
                        {
                            AudioCtrl.Instance.Play(AudioName.Achievement_Sound);
                            StartCoroutine(QuestManager.Instance.IEClaimQuest(questIndex, false, () =>
                            {
                                UpdateData();
                                foreach (ItemDataUI item in this.ListItemDataUI.ItemDataUIList)
                                {
                                    if (item.ItemData.Type == ItemType.Energy)
                                    {
                                        AudioCtrl.Instance.Play(AudioName.Collect_Energy_Sound);
                                       // WorldMapUIController.Instance.IncreaseEnergy(BtnClaim.transform.position);
                                    }
                                    if (item.ItemData.Type == ItemType.Coin)
                                    {
                                        AudioCtrl.Instance.Play(AudioName.Collect_Gold_Sound);
                                       // WorldMapUIController.Instance.IncreaseEffect(BtnClaim.transform.position, true);
                                    }
                                    if (item.ItemData.Type == ItemType.Gem)
                                    {
                                        AudioCtrl.Instance.Play(AudioName.Collect_Gem_Sound);
                                       // WorldMapUIController.Instance.IncreaseEffect(BtnClaim.transform.position, false);
                                    }
                                }
                            }));
                        };
                        watchAds.doubleAction = () =>
                        {
                            AudioCtrl.Instance.Play(AudioName.Achievement_Sound);
                            StartCoroutine(QuestManager.Instance.IEClaimQuest(questIndex, true, () =>
                            {
                                UpdateData();
                                foreach (ItemDataUI item in this.ListItemDataUI.ItemDataUIList)
                                {
                                    if (item.ItemData.Type == ItemType.Energy)
                                    {
                                        AudioCtrl.Instance.Play(AudioName.Collect_Energy_Sound);
                                       // WorldMapUIController.Instance.IncreaseEnergy(BtnClaim.transform.position);
                                    }
                                    if (item.ItemData.Type == ItemType.Coin)
                                    {
                                        AudioCtrl.Instance.Play(AudioName.Collect_Gold_Sound);
                                       // WorldMapUIController.Instance.IncreaseEffect(BtnClaim.transform.position, true);
                                    }
                                    if (item.ItemData.Type == ItemType.Gem)
                                    {
                                        AudioCtrl.Instance.Play(AudioName.Collect_Gem_Sound);
                                        //WorldMapUIController.Instance.IncreaseEffect(BtnClaim.transform.position, false);
                                    }
                                }
                            }));
                        };
                    });
                }
                else
                {
                    AudioCtrl.Instance.Play(AudioName.Achievement_Sound);
                    StartCoroutine(QuestManager.Instance.IEClaimQuest(questIndex, false, () =>
                    {
                        UpdateData();
                        foreach (ItemDataUI item in this.ListItemDataUI.ItemDataUIList)
                        {
                            if (item.ItemData.Type == ItemType.Energy)
                            {
                                AudioCtrl.Instance.Play(AudioName.Collect_Energy_Sound);
                               // WorldMapUIController.Instance.IncreaseEnergy(BtnClaim.transform.position);
                            }
                            if (item.ItemData.Type == ItemType.Coin)
                            {
                                AudioCtrl.Instance.Play(AudioName.Collect_Gold_Sound);
                                //WorldMapUIController.Instance.IncreaseEffect(BtnClaim.transform.position, true);
                            }
                            if (item.ItemData.Type == ItemType.Gem)
                            {
                                AudioCtrl.Instance.Play(AudioName.Collect_Gem_Sound);
                                //WorldMapUIController.Instance.IncreaseEffect(BtnClaim.transform.position, false);
                            }
                        }
                    }));

                }


            }

        }

        protected override void OnDestroy()
        {
            this.ListItemDataUI.Clear();
            base.OnDestroy();
        }

        protected override void OnDisable()
        {
            this.ListItemDataUI.Clear();
            base.OnDisable();
        }
    }
}