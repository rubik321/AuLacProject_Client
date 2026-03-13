using NTPackage.Functions;
using NTPackage.UI;
using Rubik.ItemPlayer;
using Rubik.Manager;
using Rubik.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Rubik.Myrk.PlayerChest
{
    using ItemPlayer;
    using Rubik.Common.AudioHelper;
    using Rubik.UserDataPlayer;

    public class PlayerChestSlotUI : MonoBehaviour
    {
        public int Index;
        public PlayerChestSlotData PlayerChestSlotData;
        public PlayerChestSlot PlayerChestSlot;

        public List<ChestUI> ChestUI;
        public TextMeshProUGUI TextBanner;
        public Transform Banner;

        public Transform Empty;

        public void SetData(PlayerChestSlotData playerChestSlotData)
        {
            this.PlayerChestSlotData = playerChestSlotData;
            this.Index = playerChestSlotData.Index;
        }

        public void Lock()
        {
            this.Empty.gameObject.SetActive(false);
            this.Banner.gameObject.SetActive(true);
            foreach (ChestUI chestUI in ChestUI)
            {
                chestUI.gameObject.SetActive(false);
            }
            if (PlayerChestManager.Instance.IsUnlockFree(Index))
            {
                TextBanner.text = Lean.Localization.LeanLocalization.GetTranslationText("chest_unlock_free", "Free");
            }
            else
            {
                string str = "";
                if (PlayerChestSlotData.RequireUnlock.Level > 0)
                {
                    str += Lean.Localization.LeanLocalization.GetTranslationText("chest_unlock_require_level", "Level")+" " + (PlayerChestSlotData.RequireUnlock.Level + 1) + "\n";
                }
                if (PlayerChestSlotData.RequireUnlock.Price.Length > 0)
                {
                    str += Lean.Localization.LeanLocalization.GetTranslationText("chest_unlock_require_price", "Price:")+" " + PlayerChestSlotData.RequireUnlock.Price[0].Amount + ItemDataManager.Instance.GetItemTextSprite(PlayerChestSlotData.RequireUnlock.Price[0].Type);
                }
                TextBanner.text = str;
            }
        }

        public void SetPlayerData(PlayerChestSlot playerChestSlot)
        {
            this.PlayerChestSlot = playerChestSlot;
            UpdateData();
        }

        public void UpdateData()
        {
            for (int i = 0; i < ChestUI.Count; i++)
            {
                ChestUI[i].gameObject.SetActive(false);
            }
            this.Banner.gameObject.SetActive(false);

            if (this.PlayerChestSlot.Unlocking)
            {
                this.Empty.gameObject.SetActive(false);
                switch (this.PlayerChestSlot.ChestType)
                {
                    case ItemPlayer.ItemType.WoodChest:
                        ChestUI[0].gameObject.SetActive(true);
                        break;
                    case ItemPlayer.ItemType.BronzeChest:
                        ChestUI[1].gameObject.SetActive(true);
                        break;
                    case ItemPlayer.ItemType.SilverChest:
                        ChestUI[2].gameObject.SetActive(true);
                        break;
                    case ItemPlayer.ItemType.GoldChest:
                        ChestUI[3].gameObject.SetActive(true);
                        break;
                }

                if (this.PlayerChestSlot.TimeClaim <= ServerManager.Instance.GetTimeServer())
                {
                    this.Banner.gameObject.SetActive(true);
                    this.TextBanner.text = Lean.Localization.LeanLocalization.GetTranslationText("chest_claim", "Claim");
                }
                else
                {
                    this.Banner.gameObject.SetActive(true);
                    if (_coroutineUpdateTimeClaim != null)
                    {
                        StopCoroutine(_coroutineUpdateTimeClaim);
                    }
                    _coroutineUpdateTimeClaim = StartCoroutine(UpdateTimeClaim());
                }
            }
            else
            {
                this.Empty.gameObject.SetActive(true);
                this.Banner.gameObject.SetActive(false);
            }
        }

        private Coroutine _coroutineUpdateTimeClaim;
        private IEnumerator UpdateTimeClaim()
        {
            while (this.PlayerChestSlot.TimeClaim > ServerManager.Instance.GetTimeServer())
            {
                string str = Lean.Localization.LeanLocalization.GetTranslationText("chest_open_in", "Open in");
                str += NTFunction.FormatTimeMinus(this.PlayerChestSlot.TimeClaim - ServerManager.Instance.GetTimeServer());
                this.TextBanner.text = str;
                yield return new WaitForSeconds(1);
            }
            this.UpdateData();
        }

        public void _OnClickSelectChest()
        {
            if (PlayerChestManager.Instance.IsSlotLock(this.Index))
            {
                if (PlayerChestManager.Instance.IsNotEnoughLevel(this.Index))
                {
                    string str = Lean.Localization.LeanLocalization.GetTranslationText("chest_unlock_slot_lower_level", "Requires Chest Proficiency Level {0} to unlock.");
                    str = string.Format(str, (PlayerChestSlotData.RequireUnlock.Level + 1));
                    HUDCanvas.Instance.ShowNotification(
                            str,
                            Lean.Localization.LeanLocalization.GetTranslationText("chest_unlock_slot_title", "Unlock Chest Slot"),
                            Lean.Localization.LeanLocalization.GetTranslationText("btn_confirm", "Confirm")
                        );
                    return;
                }

                if (PlayerChestManager.Instance.IsUnlockFree(this.Index))
                {
                    PopupManager.Instance.OnUI(PopupCode.MessageOptionPanel, null, (popupUI) =>
                        {
                            MessageOptionPanel messageOptionPanel = popupUI as MessageOptionPanel;
                            messageOptionPanel.SetData(
                                Lean.Localization.LeanLocalization.GetTranslationText("chest_unlock_slot_title", "Unlock Chest Slot"),
                                Lean.Localization.LeanLocalization.GetTranslationText("chest_unlock_slot_detail", "Do you want to unlock this chest slot?")
                            );
                            messageOptionPanel.SetActionConfirm(() =>
                            {
                                StartCoroutine(PlayerChestManager.Instance.IEUnlockChestSlot(this.Index, () =>
                                {
                                    PlayerChestUI playerChestUI = PopupManager.Instance.GetPopupUI(PopupCode.PlayerChestUI) as PlayerChestUI;
                                    playerChestUI.UpdateData();
                                }));
                            }, Lean.Localization.LeanLocalization.GetTranslationText("btn_free", "Free"));
                            messageOptionPanel.SetActionReject(() =>
                            {
                                popupUI.OffUI();
                            }, Lean.Localization.LeanLocalization.GetTranslationText("btn_cancel", "Cancel"));
                        });
                    return;
                }
                else
                {
                    ItemData price = PlayerChestSlotData.RequireUnlock.Price[0];
                    if (ItemDataManager.Instance.GetItem(price.Type).Amount < price.Amount)
                    {
                        string str = Lean.Localization.LeanLocalization.GetTranslationText("dont_enough_item", "You have insufficient {0}!");
                        str = string.Format(str, ItemDataManager.Instance.GetItemName(price.Type));
                        HUDCanvas.Instance.ShowNotification(
                            str,
                            Lean.Localization.LeanLocalization.GetTranslationText("chest_unlock_slot_title", "Unlock Chest Slot"),
                            Lean.Localization.LeanLocalization.GetTranslationText("btn_confirm", "Confirm")
                        );
                    }
                    else
                    {
                        AppsFlyerManager.TrackingEvent(AppsflyerEvents.gems_spent, "unlock_chest", (int)price.Amount);
                        PopupManager.Instance.OnUI(PopupCode.MessageOptionPanel, null, (popupUI) =>
                        {
                            MessageOptionPanel messageOptionPanel = popupUI as MessageOptionPanel;
                            messageOptionPanel.SetData(
                                Lean.Localization.LeanLocalization.GetTranslationText("chest_unlock_slot_title", "Unlock Chest Slot"),
                                Lean.Localization.LeanLocalization.GetTranslationText("chest_unlock_slot_detail", "Do you want to unlock this chest slot?")
                            );
                            messageOptionPanel.SetActionConfirm(() =>
                            {
                                StartCoroutine(PlayerChestManager.Instance.IEUnlockChestSlot(this.Index, () =>
                                {
                                    PlayerChestUI playerChestUI = PopupManager.Instance.GetPopupUI(PopupCode.PlayerChestUI) as PlayerChestUI;
                                    playerChestUI.UpdateData();
                                }));
                            }, price.Amount + " " + ItemDataManager.Instance.GetItemTextSprite(price.Type));
                            messageOptionPanel.SetActionReject(() =>
                            {
                                popupUI.OffUI();
                            }, Lean.Localization.LeanLocalization.GetTranslationText("btn_cancel", "Cancel"));
                        });
                        return;
                    }
                }

                return;
            }

            if (this.PlayerChestSlot.Unlocking)
            {
                if (this.PlayerChestSlot.TimeClaim < ServerManager.Instance.GetTimeServer())
                {
                    if (UserDataManager.Instance.IsCapSlotInventoryBag())
                    {
                        UserDataManager.Instance.ShowNotificationCapSlotInventoryBag();
                        return;
                    }
                    AudioCtrl.Instance.Play(AudioName.Achievement_Sound);
                    StartCoroutine(PlayerChestManager.Instance.IEOpenChest(this.Index, () =>
                    {
                        PlayerChestUI playerChestUI = PopupManager.Instance.GetPopupUI(PopupCode.PlayerChestUI) as PlayerChestUI;
                        playerChestUI.UpdateData();
                    }));
                }
                else
                {
                    (long gem, bool isAdv) decreaseTime = PlayerChestManager.Instance.GetDecreaseTime(this.Index);
                    PopupManager.Instance.OnUI(PopupCode.MessageOptionAdvPanel, null, (popupUI) =>
                    {
                        MessageOptionAdvPanel messageOptionAdvPanel = popupUI as MessageOptionAdvPanel;
                        string strAdv = "-" + NTFunction.Format_Time(PlayerChestManager.Instance.GetDecreaseTimeAdv(), 1);
                        string strDes = Lean.Localization.LeanLocalization.GetTranslationText("chest_decrease_time_des", "Using {0} to speed up the unlock and claim your rewards sooner?");
                        strDes = string.Format(strDes, decreaseTime.gem + ItemDataManager.Instance.GetItemTextSprite(ItemType.Gem) + ItemDataManager.Instance.GetItemName(ItemType.Gem));
                        messageOptionAdvPanel.SetDescription(
                            Lean.Localization.LeanLocalization.GetTranslationText("chest_decrease_time_title", "Open It Now!"),
                            strDes
                        );
                        messageOptionAdvPanel.SetAgreeAction(() =>
                        {
                            if (ItemDataManager.Instance.GetItem(ItemType.Gem).Amount < decreaseTime.gem)
                            {
                                ItemDataManager.Instance.ShowDontEnoughItem(ItemType.Gem);
                                return;
                            }
                            StartCoroutine(PlayerChestManager.Instance.IEDecreaseTimeChest(this.Index, (int)decreaseTime.gem, false, () =>
                            {
                                PlayerChestUI playerChestUI = PopupManager.Instance.GetPopupUI(PopupCode.PlayerChestUI) as PlayerChestUI;
                                playerChestUI.UpdateData();
                            }));
                        }, 
                        Lean.Localization.LeanLocalization.GetTranslationText("chest_open_now", "Open Now"),
                        "-" + decreaseTime.gem + ItemDataManager.Instance.GetItemTextSprite(ItemType.Gem));

                        messageOptionAdvPanel.SetAgreeAdvAction(() =>
                        {
                            LevelPlayAds.Instance.OnShowReward(() =>
                            {
                                AppsFlyerManager.TrackingAds("Chess");
                                StartCoroutine(PlayerChestManager.Instance.IEDecreaseTimeChest(this.Index, (int)decreaseTime.gem, true, () =>
                              {
                                  PlayerChestUI playerChestUI = PopupManager.Instance.GetPopupUI(PopupCode.PlayerChestUI) as PlayerChestUI;
                                  playerChestUI.UpdateData();
                              }));
                            });
                        }, strAdv);
                    });
                }
            }
            else
            {
                PopupManager.Instance.OnUI(PopupCode.PlayerChestSelectUI, this.Index);
            }
        }
    }

}
