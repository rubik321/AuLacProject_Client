using NTPackage.UI;
using Rubik.Myrk.Skill;
using Rubik.UI;
using Rubik.UI.Statitic;
using Rubik.UserDataPlayer;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Rubik.CardPlayer
{
    using NTPackage.Functions;
    using Rubik.Common.AudioHelper;
    using Rubik.ItemPlayer;
    using System.Linq;
    public class CardPlayerLevelTab : NTBehaviour
    {
        public CardPlayerInfoUI CardPlayerInfoUI;

        public TextMeshProUGUI TextLevel,warnningTxtLv,warnningTxtStar;
        public NTButtonEffect ButtonLevelUp;
        public List<ItemDataBarUI> PriceLevelUp;

        public StarUI StarUI;
        public NTButtonEffect ButtonStarUp;
        public List<ItemDataBarUI> PriceStarUp;


        public ListStatUI ListStatUI;
        public ListSkillItemUI ListSkillItemUI;


        public bool IsLevelCap = false;
        public bool IsOverPlayerLevel = false;
        public bool IsDontEnoughItemLevelUp = false;
        public ItemType DontEnoughItemTypeLevelUp;

        public bool IsStarCap = false;
        public bool IsDontEnoughItemStarUp = false;
        public ItemType DontEnoughItemTypeStarUp;

        public void SetData(CardPlayerInfoUI cardPlayerInfoUI)
        {
            this.CardPlayerInfoUI = cardPlayerInfoUI;
            this.UpdateData();
        }

        public void OnUI()
        {
            List<ItemType> itemTypes = new List<ItemType>();
            List<ItemData> price = CardPlayerManager.Instance.GetPriceUpStarCard(this.CardPlayerInfoUI.CardPlayer.Index, this.CardPlayerInfoUI.CardPlayer.Star);
            foreach (ItemData itemData in price)
            {
                itemTypes.Add(itemData.Type);
            }
            itemTypes.Add(ItemType.Fruit);
            this.CardPlayerInfoUI.ShowItem(itemTypes);
        }

        public void UpdateData()
        {
            if(this.CardPlayerInfoUI.CardPlayer == null || string.IsNullOrEmpty(this.CardPlayerInfoUI.CardPlayer._id)) return;
            CardPlayerManager.Instance.UpdateCacheCardPlayer(this.CardPlayerInfoUI.CardPlayer);
            // Level
            this.IsLevelCap = false;
            this.IsOverPlayerLevel = false;
            this.IsDontEnoughItemLevelUp = false;

            for (int i = 0; i < this.PriceLevelUp.Count; i++)
            {
                this.PriceLevelUp[i].gameObject.SetActive(false);
            }
            CardLevelData cardLevelData = CardPlayerManager.Instance.GetCardLevelDataByLevel(this.CardPlayerInfoUI.CardPlayer.Lv);
            if (cardLevelData == null || cardLevelData.Max)
            {
                this.IsLevelCap = true;
                this.ButtonLevelUp.Unchose();
                this.TextLevel.text = "Lv." + (this.CardPlayerInfoUI.CardPlayer.Lv + 1) + Lean.Localization.LeanLocalization.GetTranslationText("max", "(Max)");
                warnningTxtLv.text = Lean.Localization.LeanLocalization.GetTranslationText("card_player_level_max", "Monster reached the max level!");
            }
            else
            {
                this.ButtonLevelUp.Chose();
                List<ItemData> price = cardLevelData.Price.ToList();
                warnningTxtLv.text = "";
                for (int i = 0; i < price.Count; i++)
                {
                    this.PriceLevelUp[i].gameObject.SetActive(true);
                    this.PriceLevelUp[i].SetData(price[i], true, true);
                    if (ItemDataManager.Instance.GetItem(price[i].Type) == null || ItemDataManager.Instance.GetItem(price[i].Type).Amount < price[i].Amount)
                    {
                        this.PriceLevelUp[i].Amount.color = Color.red;
                        this.ButtonLevelUp.Unchose();
                        this.IsDontEnoughItemLevelUp = true;
                        warnningTxtLv.text = Lean.Localization.LeanLocalization.GetTranslationText("card_player_level_dont_enough_item", "You don't have enough materials to upgrade!");
                        this.DontEnoughItemTypeLevelUp = price[i].Type;
                    }
                    else
                    {
                        this.PriceLevelUp[i].Amount.color = Color.white;
                    }
                }
                this.TextLevel.text = "Lv." + (this.CardPlayerInfoUI.CardPlayer.Lv + 1);
               
                if (cardLevelData.Level >= UserDataManager.Instance.GetLevel())
                {
                    this.ButtonLevelUp.Unchose();
                    this.IsOverPlayerLevel = true;
                    this.TextLevel.text = "Lv." + (this.CardPlayerInfoUI.CardPlayer.Lv + 1) + Lean.Localization.LeanLocalization.GetTranslationText("max", "(Max)");
                    warnningTxtLv.text = Lean.Localization.LeanLocalization.GetTranslationText("card_player_must_level_up", "You must level up before upgrading the monster!");
                }
            }


            this.StarUI.SetStar(this.CardPlayerInfoUI.CardPlayer.Star);
            // Star
            this.IsStarCap = false;
            this.IsDontEnoughItemStarUp = false;

            for (int i = 0; i < this.PriceStarUp.Count; i++)
            {
                this.PriceStarUp[i].gameObject.SetActive(false);
            }

            CardUpStar starData = CardPlayerManager.Instance.GetCardUpStarDataByStar(this.CardPlayerInfoUI.CardPlayer.Star);
            if (starData == null || starData.Max)
            {
                this.IsStarCap = true;
                this.ButtonStarUp.Unchose();
            }
            else
            {
                List<ItemData> price = CardPlayerManager.Instance.GetPriceUpStarCard(this.CardPlayerInfoUI.CardPlayer.Index, this.CardPlayerInfoUI.CardPlayer.Star);
                this.ButtonStarUp.Chose();
                warnningTxtStar.text = "";

                for (int i = 0; i < price.Count; i++)
                {
                    this.PriceStarUp[i].gameObject.SetActive(true);
                    this.PriceStarUp[i].SetData(price[i], true, true);
                    if (ItemDataManager.Instance.GetItem(price[i].Type) == null || ItemDataManager.Instance.GetItem(price[i].Type).Amount < price[i].Amount)
                    {
                        this.PriceStarUp[i].Amount.color = Color.red;
                        this.ButtonStarUp.Unchose();
                        this.IsDontEnoughItemStarUp = true;
                        warnningTxtStar.text = Lean.Localization.LeanLocalization.GetTranslationText("card_player_star_dont_enough_item", "You don't have enough materials to upgrade!");
                        this.DontEnoughItemTypeStarUp = price[i].Type;
                    }
                    else
                    {
                        this.PriceStarUp[i].Amount.color = Color.white;
                    }
                }
            }


            this.ListStatUI.SetData(
                this.CardPlayerInfoUI.CardPlayer.TotalStats.ATK, 
                this.CardPlayerInfoUI.CardPlayer.TotalStats.DEF, 
                this.CardPlayerInfoUI.CardPlayer.TotalStats.SPD, 
                this.CardPlayerInfoUI.CardPlayer.TotalStats.HP
            );

            // Skill
            this.ListSkillItemUI.SetData(CardPlayerManager.Instance.GetCardSkill(this.CardPlayerInfoUI.CardPlayer.Index, this.CardPlayerInfoUI.CardPlayer.Star));
        }

        public void OnClickLevelUp()
        {
            if (this.IsLevelCap)
            {
                HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("card_player_max_level", "This Echo companions has reached the max level!."));
                return;
            }
            if (this.IsOverPlayerLevel)
            {
                HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("card_player_over_player_level", "Echo companions may only level up to match your Hero's level!"));
                return;
            }
            if (this.IsDontEnoughItemLevelUp)
            {
                ItemDataManager.Instance.ShowDontEnoughItem(this.DontEnoughItemTypeLevelUp);
                return;
            }

            (long atk, long def, long hp, long spd) = CardPlayerManager.Instance.GetCardIncreaseStatByNextLevel(this.CardPlayerInfoUI.CardPlayer.Index, this.CardPlayerInfoUI.CardPlayer.Lv, this.CardPlayerInfoUI.CardPlayer.Star);
            this.ListStatUI.ShowValue(atk, def, spd, hp);
            AudioCtrl.Instance.Play(AudioName.Collect_Gem_Sound);
            StartCoroutine(CardPlayerManager.Instance.IELevelUp(this.CardPlayerInfoUI.CardPlayer._id, () =>
            {
                AppsFlyerManager.TrackingEvent(AppsflyerEvents.money_spent, "upgrade_monster", 100);
                this.CardPlayerInfoUI.UpdateData();
                this.CardPlayerInfoUI.lvEffect.Play();
                this.ListStatUI.HideValue();

            }));

        }

        public void OnClickStarUp()
        {
            if (this.IsStarCap)
            {
                HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("card_player_max_star", "This Echo companions has reached the max star!"));
                return;
            }
            if (this.IsDontEnoughItemStarUp)
            {
                ItemDataManager.Instance.ShowDontEnoughItem(this.DontEnoughItemTypeStarUp);
                return;
            }

            (long atk, long def, long hp, long spd) = CardPlayerManager.Instance.GetCardIncreaseStatByNextStar(this.CardPlayerInfoUI.CardPlayer.Index, this.CardPlayerInfoUI.CardPlayer.Lv, this.CardPlayerInfoUI.CardPlayer.Star);
            this.ListStatUI.ShowValue(atk, def, spd, hp);
            AudioCtrl.Instance.Play(AudioName.Collect_Gem_Sound);
            StartCoroutine(CardPlayerManager.Instance.IEUpStarCard(this.CardPlayerInfoUI.CardPlayer._id, () =>
            {
                this.CardPlayerInfoUI.UpdateData();
                this.ListStatUI.HideValue();
            }));

        }

        public void ShowStatsLevelUp()
        {
            (long atk, long def, long hp, long spd) = CardPlayerManager.Instance.GetCardIncreaseStatByNextLevel(this.CardPlayerInfoUI.CardPlayer.Index, this.CardPlayerInfoUI.CardPlayer.Lv, this.CardPlayerInfoUI.CardPlayer.Star);
            this.ListStatUI.ShowValue(atk, def, spd, hp);
        }

        public void ShowStatsStarUp()
        {
            (long atk, long def, long hp, long spd) = CardPlayerManager.Instance.GetCardIncreaseStatByNextStar(this.CardPlayerInfoUI.CardPlayer.Index, this.CardPlayerInfoUI.CardPlayer.Lv, this.CardPlayerInfoUI.CardPlayer.Star);
            this.ListStatUI.ShowValue(atk, def, spd, hp);
        }

        public void OffStats(){
            this.ListStatUI.HideValue();
        }
    }
}