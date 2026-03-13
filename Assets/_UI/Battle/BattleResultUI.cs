using Rubik.CardPlayer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NTPackage.UI;
using System.Collections.Generic;
using Rubik.Myrk.Monster;
using NTPackage;
using Rubik.BattleEngine;
using System.Collections;

namespace Rubik.Myrk.Battle
{
    public class BattleResultAnim{
        public const string OnUI = "OnUI";
        public const string OffUI = "OffUI";
    }

    public class BattleResultUI : PopupUI
    {
        public List<BattleResultItem> TeamA;
        public List<BattleResultItem> TeamB;

        public TextMeshProUGUI TextRound;

        public NTDictionary<string, ElementStatisData> ElementStatis;
        public List<CardBattleShordData> TeamACards;
        public List<CardBattleShordData> TeamBCards;
        public Transform TeamContent;

        public long MaxDamage = 0;
        public long MaxHeal = 0;
        public long MaxDamaged = 0;

        public List<NTButtonEffect> TabButtons;

        public List<Image> TitleImages;
        public List<Sprite> TitleSprites;

        public Animator Animator;

        public override void OnUI(object data = null, bool isDefaultSound = true)
        {
            base.OnUI(data, isDefaultSound);
            this.Animator.Play(BattleResultAnim.OnUI);
        }

        public override void OffUI()
        {
            if(this.ScreenDim != null) this.ScreenDim.gameObject.SetActive(false);
            this.Animator.Play(BattleResultAnim.OffUI);
            StartCoroutine(this.OffBattleResultAnim());
        }

        public override void UpdateData(object data = null)
        {
            base.UpdateData(data);
            BattleShortData result = BattleEngineController.Instance.Result;
            this.TextRound.text = Lean.Localization.LeanLocalization.GetTranslationText("turn", "Turn") + " " + result.RoundDatas.Length.ToString();
            this.ElementStatis = new NTDictionary<string, ElementStatisData>();
            this.TeamACards = new List<CardBattleShordData>();
            this.TeamBCards = new List<CardBattleShordData>();
            this.MaxDamage = 1;
            this.MaxHeal = 1;
            this.MaxDamaged = 1;
            if(result.TeamWin == result.StatisData.TeamA){
                this.TitleImages[0].sprite = this.TitleSprites[0];
                this.TitleImages[1].sprite = this.TitleSprites[0];
            }
            else{
                this.TitleImages[0].sprite = this.TitleSprites[1];
                this.TitleImages[1].sprite = this.TitleSprites[1];
            }
            foreach (ElementStatisData item in result.StatisData.ElementStatis)
            {
                this.ElementStatis.Add(item._id, item);
                if (item.Dmg > this.MaxDamage)
                {
                    this.MaxDamage = item.Dmg;
                }
                if (item.Heal > this.MaxHeal)
                {
                    this.MaxHeal = item.Heal;
                }
                if (item.Hit > this.MaxDamaged)
                {
                    this.MaxDamaged = item.Hit;
                }
            }

            foreach (CardBattleShordData item in result.TeamA.Cards)
            {
                this.TeamACards.Add(item);
            }

            foreach (CardBattleShordData item in result.TeamB.Cards)
            {
                this.TeamBCards.Add(item);
            }

            this._OnclickTab(0);
        }

        public void _OnclickTab(int tab) // 0: Damage, 1: Heal, 2: Damaged
        {
            this.TeamContent.gameObject.SetActive(false);

            foreach (NTButtonEffect item in this.TabButtons)
            {
                item.Unchose();
            }
            this.TabButtons[tab].Chose();

            foreach (BattleResultItem item in this.TeamA)
            {
                item.gameObject.SetActive(false);
            }

            foreach (BattleResultItem item in this.TeamB)
            {
                item.gameObject.SetActive(false);
            }

            for (int i = 0; i < this.TeamACards.Count; i++)
            {
                CardBattleShordData card = this.TeamACards[i];
                BattleResultItem item = this.TeamA[i];
                ElementStatisData statis = this.ElementStatis.Get(card._id);
                item.gameObject.SetActive(true);
                item.SetData(card.Index, card.Star, card.Level, card.Slot);
                if (tab == 0)
                {
                    item.SetDamage(statis.Dmg, this.MaxDamage);
                }
                else if (tab == 1)
                {
                    item.SetHeal(statis.Heal, this.MaxHeal);
                }
                else if (tab == 2)
                {
                    item.SetDamaged(statis.Hit, this.MaxDamaged);
                }
            }

            for (int i = 0; i < this.TeamBCards.Count; i++)
            {
                CardBattleShordData card = this.TeamBCards[i];
                BattleResultItem item = this.TeamB[i];
                ElementStatisData statis = this.ElementStatis.Get(card._id);
                item.gameObject.SetActive(true);
                item.SetData(card.Index, card.Star, card.Level, card.Slot);
                if (tab == 0)
                {
                    item.SetDamage(statis.Dmg, this.MaxDamage);
                }
                else if (tab == 1)
                {
                    item.SetHeal(statis.Heal, this.MaxHeal);
                }
                else if (tab == 2)
                {
                    item.SetDamaged(statis.Hit, this.MaxDamaged);
                }
            }

            this.TeamContent.gameObject.SetActive(true);
        }

        public IEnumerator OffBattleResultAnim(){
            AnimationClip[] clips = this.Animator.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in clips)
            {
                if (clip.name == BattleResultAnim.OffUI)
                {
                    yield return new WaitForSeconds(clip.length);
                    break;
                }
            }
            base.OffUI();
        }
    }

}