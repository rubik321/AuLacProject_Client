using DG.Tweening;
using GOA.Item;
using NTPackage.Functions;
using NTPackage.UI;
using Rubik.UI.Statitic;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Rubik.CardPlayer
{
    using NTPackage.EventDispatcher;
    using Rubik.Combat;
    using Rubik.Common.AudioHelper;
    using Rubik.ItemPlayer;
    using Rubik.Myrk.BattleTeam;

    public class CardPlayerEvolveTabAnim{
        public const string Anim_OnUI = "OnUI";
        public const string Anim_Evolve = "Evolve";
        public const string Anim_Level = "Level";
    }

    public class CardPlayerInfoUI : PopupUI
    {
        public CardPlayer CardPlayer;
        public CardPlayerData CardPlayerData;
        public CardPlayerLevelTab CardPlayerLevelTab;
        public CardPlayerEvolveTab CardPlayerEvolveTab;
        public GameObject evovelList;
        public Transform contentLeft,targetPoint;
        public TextMeshProUGUI Name;
        public TextMeshProUGUI TextPower;
        public Transform ModelHolder;
        public UnityEngine.UI.Image originImg,originUpgrade;

        public ItemDataBarUI ItemDataBarUIPrefab;
        public List<UserItemDataBarUI> UserItemDataBarUIList;

        public Animator Anim;
        public ParticleSystem monsterAppear, monsterAppearEvolve,lvEffect;
        [SerializeField] ParticleSystem[] lsOriginEffects;

        public int status = 0; // 0: level, 1: evolve
        public int statusUI = 0; // 0: level, 1: evolve
        public override void LoadComponents()
        {
            base.LoadComponents();
            this.popupCode = PopupCode.CardPlayerInfoUI;
        }

        public override void OnUI(object data = null, bool isDefaultSound = true)
        {
            timeDelay = 1;
            string cardID = (string)data;
            this.CardPlayer = CardPlayerManager.Instance.GetCardByID(cardID);
            this.CardPlayerData = CardPlayerManager.Instance.GetCardPlayerDataByIndex(this.CardPlayer.Index);
            if (this.CardPlayer == null) return;
            base.OnUI(data, isDefaultSound);
            this.CardPlayerLevelTab.OnUI();
            this.status = 0;
        }

        public override void ScriptOffUI()
        {
            base.ScriptOffUI();
            EventListenerManager.instance.PostEvent(EventCode.UpdateCardPlayerWhenOffUI, null);
        }

       float timeDelay = 1;
        public override void UpdateData(object data = null)
        {
            base.UpdateData(data);
            if(this.CardPlayer == null || string.IsNullOrEmpty(this.CardPlayer._id)) return;
            CardPlayerManager.Instance.UpdateCacheCardPlayer(this.CardPlayer);
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.ModelHolder);
            Transform model = CardPlayerManager.Instance.InstantiatePlayerSkeletonGraphic(this.CardPlayer.Index);
            model.SetParent(this.ModelHolder);
           
            NTFunction.ResetPosition(model);
            this.CardPlayerLevelTab.SetData(this);
            this.CardPlayerEvolveTab.SetData(this);
            this.Anim.Play(CardPlayerEvolveTabAnim.Anim_OnUI);
            
            Invoke("MonsterPlayEffect", timeDelay);
            timeDelay = 0.1f;
            originImg.sprite = CardPlayerManager.Instance.GetOriginSpriteCircle(this.CardPlayerData.Origin);
            originUpgrade.sprite = originImg.sprite;

            this.CardPlayerLevelTab.OnUI();
            this.status = 0;
            this.Name.text = CardPlayerManager.Instance.GetCardName(this.CardPlayer.Index);
            this.TextPower.text = BattleTeamManager.Instance.GetCardPower(this.CardPlayer.ToShortTeam()).ToString();
            this.CardPlayerLevelTab.UpdateData();
            this.CardPlayerEvolveTab.UpdateData();
        }
        void MonsterPlayEffect()
        {
            monsterAppear.Play();
           
        }
        public void UpgradeEffectOrigin()
        {
            AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
            foreach (ParticleSystem par in lsOriginEffects)
            {
                par.gameObject.SetActive(false);
            }
            DOVirtual.DelayedCall(1, () => {

                lsOriginEffects[(int)CardPlayerManager.Instance.GetCardPlayerDataByIndex(CardPlayer.Index).Origin].gameObject.SetActive(true);
            });
        }
        //public override void UpdateData(object data = null)
        //{
        //    base.UpdateData(data);

        //    this.Name.text = CardPlayerManager.Instance.GetCardName(this.CardPlayer.Index);
        //    this.TextPower.text = CardPlayerManager.Instance.GetPower(this.CardPlayer.Index, this.CardPlayer.Lv, this.CardPlayer.Star).ToString();
        //    this.CardPlayerLevelTab.UpdateData();
        //    this.CardPlayerEvolveTab.UpdateData();
        //}

        public void ShowItem(List<ItemType> itemTypes)
        {
            foreach (UserItemDataBarUI item in this.UserItemDataBarUIList)
            {
                item.gameObject.SetActive(false);
            }
            for (int i = 0; i < itemTypes.Count; i++)
            {
                if(i < this.UserItemDataBarUIList.Count){
                    this.UserItemDataBarUIList[i].gameObject.SetActive(true);
                    this.UserItemDataBarUIList[i].SetData(ItemDataManager.Instance.GetItem(itemTypes[i]).Type);
                }
            }

        }

        public void _OnClickEvolve(){
            AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
            this.CardPlayerEvolveTab.OnUI();
            this.Anim.Play(CardPlayerEvolveTabAnim.Anim_Evolve);
            this.status = 1;
            monsterAppearEvolve.Play();
            this.CardPlayerEvolveTab.UpdateData();
        }

        public void _OnClickLevel(){
            AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
            this.CardPlayerLevelTab.OnUI();
            this.Anim.Play(CardPlayerEvolveTabAnim.Anim_Level);
            this.status = 0;
            this.CardPlayerLevelTab.UpdateData();
        }

        public void _OnClickBack(){
            AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
            if (this.status == 0){
                this.OffUI();
                if (statusUI == 1)
                {
                    PopupManager.Instance.OnUI(PopupCode.LineUpUI);
                }
                //var monster = PopupManager.Instance.GetPopupUIByCode(PopupCode.MonsterUI).GetComponent<MonsterUI>();
                //monster.InitCard();
            }
            else if (this.status == 1)
            {
                this._OnClickLevel();
            }
           
        }

        public void ShowStatslevelUp(){

        }

        public void ShowStatsEvolve(){

        }
        public void StartEvolve()
        {
            this.CardPlayerEvolveTab.gameObject.SetActive(false); 
            this.evovelList.gameObject.SetActive(false);
            var tempPos = contentLeft.localPosition;
            contentLeft.DOLocalMove(targetPoint.localPosition, 0.5f).OnComplete(() =>
            {
                //lsOriginEffects[0].gameObject.SetActive(true);
                //lsOriginEffects[0].Play();
                monsterAppear.Play();
                ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.ModelHolder);
                Transform model = CardPlayerManager.Instance.InstantiatePlayerSkeletonGraphic(this.CardPlayer.Index);
                model.SetParent(this.ModelHolder);
                NTFunction.ResetPosition(model);
               
                DOVirtual.DelayedCall(2, () => {
                  
                    //lsOriginEffects[0].gameObject.SetActive(false);
                 
                    monsterAppear.Play();
                    contentLeft.DOLocalMove(tempPos, 0.5f).OnComplete(() =>
                    {
                        this.CardPlayerEvolveTab.gameObject.SetActive(true);
                        this.evovelList.gameObject.SetActive(true);
                        UpdateData();
                     
                    });
                   

                });

            });
           
           
        }
    }
}
