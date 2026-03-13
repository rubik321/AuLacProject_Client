using NTPackage.Functions;
using NTPackage.UI;
using Rubik.Myrk.Skill;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.CardPlayer
{
    using ItemPlayer;
    using Rubik.Common.AudioHelper;
    using Rubik.UI;
    using Rubik.UI.Statitic;
    using Rubik.UserDataPlayer;
    using TMPro;

    public class CardPlayerEvolveTab : NTBehaviour
    {
        public CardPlayerInfoUI CardPlayerInfoUI;

        public Transform TargetModelHolder;
        public List<ItemDataBarUI> PriceEvolve;
        public NTButtonEffect ButtonEvolve;
        public TextMeshProUGUI TargetName;

        public List<Transform> EvolveTransformList;
        public List<Transform> MaxEvolveTransformList;

        public bool IsDontEnoughItem = false;
        public ItemType DontEnoughItemType;

        public ListSkillItemUI CurrentSkillItemUI;
        public ListStatUI CurrentListStatUI;
        public ListSkillItemUI NextSkillItemUI;
        public ListStatUI NextListStatUI;

        public void OnUI()
        {
            if (!CardPlayerManager.Instance.CanEvolve(this.CardPlayerInfoUI.CardPlayer.Index))
            {
                this.CardPlayerInfoUI.ShowItem(new List<ItemType>() { ItemType.Coin });
            }
            else
            {
                List<ItemType> itemTypes = new List<ItemType>();
                List<ItemData> price = CardPlayerManager.Instance.GetPriceEvolveCard(this.CardPlayerInfoUI.CardPlayer.Index);
                foreach (ItemData itemData in price)
                {
                    itemTypes.Add(itemData.Type);
                }
                this.CardPlayerInfoUI.ShowItem(itemTypes);
            }
        }

        public void SetData(CardPlayerInfoUI cardPlayerInfoUI)
        {
            this.CardPlayerInfoUI = cardPlayerInfoUI;
        }

        public void UpdateData()
        {
            this.ButtonEvolve.Chose();
            for (int i = 0; i < this.PriceEvolve.Count; i++)
            {
                this.PriceEvolve[i].gameObject.SetActive(false);
            }
            if (!CardPlayerManager.Instance.CanEvolve(this.CardPlayerInfoUI.CardPlayer.Index))
            {
                for (int i = 0; i < this.EvolveTransformList.Count; i++)
                {
                    this.EvolveTransformList[i].gameObject.SetActive(false);
                }
                for (int i = 0; i < this.MaxEvolveTransformList.Count; i++)
                {
                    this.MaxEvolveTransformList[i].gameObject.SetActive(true);
                }
                this.ButtonEvolve.Unchose();
            }
            else
            {
                for (int i = 0; i < this.EvolveTransformList.Count; i++)
                {
                    this.EvolveTransformList[i].gameObject.SetActive(true);
                }
                for (int i = 0; i < this.MaxEvolveTransformList.Count; i++)
                {
                    this.MaxEvolveTransformList[i].gameObject.SetActive(false);
                }

                CardPlayerIndex evolveTarget = CardPlayerManager.Instance.GetNextEvolveCard(this.CardPlayerInfoUI.CardPlayer.Index);
                this.TargetName.text = CardPlayerManager.Instance.GetCardName(evolveTarget);

                ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.TargetModelHolder);
                Transform model = CardPlayerManager.Instance.InstantiatePlayerSkeletonGraphic(evolveTarget);
                model.SetParent(this.TargetModelHolder);
                NTFunction.ResetPosition(model);

                this.IsDontEnoughItem = false;
                List<ItemData> price = CardPlayerManager.Instance.GetPriceEvolveCard(this.CardPlayerInfoUI.CardPlayer.Index);
                for (int i = 0; i < price.Count; i++)
                {
                    this.PriceEvolve[i].gameObject.SetActive(true);
                    this.PriceEvolve[i].SetData(price[i]);
                    if (ItemDataManager.Instance.GetItem(price[i].Type).Amount < price[i].Amount)
                    {
                        this.IsDontEnoughItem = true;
                        this.PriceEvolve[i].Amount.color = Color.red;
                        this.ButtonEvolve.Unchose();
                        this.DontEnoughItemType = price[i].Type;
                    }
                    else
                    {
                        this.PriceEvolve[i].Amount.color = Color.white;
                    }
                }
            
                int level = this.CardPlayerInfoUI.CardPlayer.Lv;
                int star = this.CardPlayerInfoUI.CardPlayer.Star;
                CardPlayerIndex currentIndex = this.CardPlayerInfoUI.CardPlayer.Index;
                CardPlayer fakeCardPlayer = CardPlayerManager.Instance.GetFakeCardPlayerByIndex(currentIndex);
                CardPlayerManager.Instance.UpdateCacheCardPlayer(fakeCardPlayer);
                CardPlayer fakeCardPlayerNext = CardPlayerManager.Instance.GetFakeCardPlayerByIndex(evolveTarget);
                CardPlayerManager.Instance.UpdateCacheCardPlayer(fakeCardPlayerNext);
                this.CurrentSkillItemUI.SetData(CardPlayerManager.Instance.GetCardSkill(currentIndex, star));
                this.NextSkillItemUI.SetData(CardPlayerManager.Instance.GetCardSkill(evolveTarget, star));
                this.CurrentListStatUI.SetData(fakeCardPlayer.TotalStats.ATK, fakeCardPlayer.TotalStats.DEF, fakeCardPlayer.TotalStats.SPD, fakeCardPlayer.TotalStats.HP);
                this.NextListStatUI.SetData(fakeCardPlayerNext.TotalStats.ATK, fakeCardPlayerNext.TotalStats.DEF, fakeCardPlayerNext.TotalStats.SPD, fakeCardPlayerNext.TotalStats.HP);
            }
        }

        public void OnClickEvolve()
        {
            // CardPlayerInfoUI.monsterAppearEvolve.Play();
           
            if (this.IsDontEnoughItem)
            {
                ItemDataManager.Instance.ShowDontEnoughItem(this.DontEnoughItemType);
                return;
            }
            PopupManager.Instance.OnUI(PopupCode.MessageOptionPanel, null, (popup) =>
            {
                MessageOptionPanel messageOptionPanel = popup as MessageOptionPanel;
                messageOptionPanel.SetActionConfirm(() =>
                {
                    this.OnStartEvolve();
                }, Lean.Localization.LeanLocalization.GetTranslationText("btn_confirm", "Confirm"));
                messageOptionPanel.SetActionReject(() =>
                {
                    messageOptionPanel.OffUI();
                }, Lean.Localization.LeanLocalization.GetTranslationText("btn_cancel", "Cancel"));
                messageOptionPanel.SetData(
                    Lean.Localization.LeanLocalization.GetTranslationText("evolve_confirm_title", "Evolve Echo"), 
                    Lean.Localization.LeanLocalization.GetTranslationText("evolve_confirm_content", "Your Echo is about to ascend to a higher form. This evolution is irreversible. Do you wish to proceed?")
                );
            });


        }
        public void OnStartEvolve()
        {
            //this.CardPlayerInfoUI.StartEvolve();
            AudioCtrl.Instance.Play(AudioName.Achievement_Sound);
            StartCoroutine(CardPlayerManager.Instance.IEEvolveCard(this.CardPlayerInfoUI.CardPlayer._id, () =>
            {
                this.CardPlayerInfoUI.StartEvolve();
                AppsFlyerManager.TrackingEvent(AppsflyerEvents.gems_spent, "evolve_monster", 100);

            }));
        }
    }
}
