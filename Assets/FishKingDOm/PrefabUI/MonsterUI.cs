using DG.Tweening;
using NTPackage.EventDispatcher;
using NTPackage.Functions;
using NTPackage.UI;
using Rubik.CardPlayer;
using Rubik.Combat;
using Rubik.Common.AudioHelper;
using Rubik.Myrk.Skill;
using Rubik.UI.Statitic;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class MonsterUI : PopupUI
{
    public List<CardUISlot> lsMonsters;
    [SerializeField] List<TabInven> lsButtonSlots;
    [SerializeField] CardUISlot cardMonster;
    [SerializeField] Transform content, ModelHolder;

    public TextMeshProUGUI TitleName;
    public ListStatUI ListStatUI;
    public ListSkillItemUI ListSkillItemUI;
    public StarUI StarUI;

    [SerializeField] ParticleSystem appearEffect;

    public string IdCurrent;
    public int IndexCurrent;

    public override void OnUI(object data = null, bool isDefaultSound = true)
    {
        base.OnUI(data, isDefaultSound);
        foreach (CardUISlot card in lsMonsters)
        {
            card.UnSetUp();
        }
        this.IdCurrent = "";
        this.IndexCurrent = 0;
        ObjectPoolingManager.Instance.PushChildObjectIntoPooling(content);
        InitCard();

        //StartGame();
        EventListenerManager.instance.Register(EventCode.UpdateCardPlayerWhenOffUI, "MonsterUI", (object data) =>
        {
            InitCard();
        });
    }
    public override void UpdateData(object data = null)
    {
        base.UpdateData(data);
    }
    public override void ScriptOffUI()
    {
        base.ScriptOffUI();
        EventListenerManager.instance.RemoveListener(EventCode.UpdateCardPlayerWhenOffUI, "MonsterUI");
        foreach (CardUISlot card in lsMonsters)
        {
            card.UnSetUp();
        }
        ObjectPoolingManager.Instance.PushChildObjectIntoPooling(content);
        // idCurent = null;
    }

    public void ClosePopup()
    {
        OffUI();

    }
    public void OnTabs(int index)
    {
        AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
        //foreach(TabInven tab in lsButtonSlots)
        //{
        //    tab.TabOn();
        //}
        //lsButtonSlots[index].TabOn(true);
        this.IndexCurrent = index;
        foreach (CardUISlot card in lsMonsters)
        {
            if (index == 0)
            {
                card.gameObject.SetActive(true);
            }
            else
            {
                card.gameObject.SetActive((index - 1) == card.Origin);
            }

        }
    }

    // Todo
    public void InitCard()
    {
        ObjectPoolingManager.Instance.PushChildObjectIntoPooling(content);
        var lsMonster = CardPlayerManager.Instance.GetListCardPlayer();
      
        lsMonsters.Clear();
        NTLog.LogMessage("Temp count : " + lsMonster.Count);
        foreach (CardPlayer stats in lsMonster)
        {
            if (string.IsNullOrEmpty(stats._id))
                continue;
            CardPlayerManager.Instance.UpdateCacheCardPlayer(stats);
            CardUISlot cardChar = ObjectPoolingManager.Instance.InstantiateObject<CardUISlot>(ObjectPoolingConfig.CardUISlot, cardMonster.transform);
            cardChar.transform.SetParent(content);
            NTFunction.ResetPosition(cardChar.transform);
            lsMonsters.Add(cardChar);
            cardChar.SetUp(stats, ChoseMonsterAction);
            if (string.IsNullOrEmpty(IdCurrent))
            {
                this.IdCurrent = stats._id;
            }
        }
        Rubik.Common.Common.ResetContent(content);
        this.ChoseMonsterAction(this.IdCurrent);
        OnTabs(this.IndexCurrent);
    }

    public void ChoseMonsterAction(string id)
    {
        this.IdCurrent = id;
        AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
        foreach (CardUISlot cardSlot in lsMonsters)
        {
            // Monster chosed
            if (cardSlot.CardPlayer._id == this.IdCurrent)
            {
                cardSlot.tickava.gameObject.SetActive(true);
                appearEffect.Play();
                ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.ModelHolder);
                Transform model = CardPlayerManager.Instance.InstantiatePlayerSkeletonGraphic(cardSlot.CardPlayer.Index);
                model.SetParent(this.ModelHolder);
                NTFunction.ResetPosition(model);

                //skeAnim.skeletonDataAsset = lsMonsters[temp].GetComponentInChildren<SkeletonGraphic>().skeletonDataAsset;
                //skeAnim.AnimationState.SetAnimation(0, "idle", true);
                //skeAnim.Initialize(true);
                //skeAnim.SkeletonDataAsset

                // Stat
                this.ListStatUI.SetData(cardSlot.CardPlayer.TotalStats.ATK, cardSlot.CardPlayer.TotalStats.DEF, cardSlot.CardPlayer.TotalStats.SPD, cardSlot.CardPlayer.TotalStats.HP);
                this.StarUI.SetStar(cardSlot.CardPlayer.Star);
                this.ListSkillItemUI.SetData(CardPlayerManager.Instance.GetCardSkill(cardSlot.CardPlayer.Index, cardSlot.CardPlayer.Star));
                this.TitleName.text = CardPlayerManager.Instance.GetCardName(cardSlot.CardPlayer.Index);
            }
            else
            {
                cardSlot.tickava.gameObject.SetActive(false);
            }
        }
    }

    void StartGame()
    {
        InitCard();

    }
    public void OnButtonUpgrade()
    {
        AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
        appearEffect.Stop();
        PopupManager.Instance.OnUI(PopupCode.CardPlayerInfoUI, (object)IdCurrent, (PopupUI popupUI) =>
        {
            popupUI.GetComponent<CardPlayerInfoUI>().statusUI = 0;
            if (AssetLoader.Instance.IsTut)
            {
                // PopupManager.Instance.GetPopupUI(PopupCode.PlayerMailUI).OffUI();
                var temp = PopupManager.Instance.GetPopupUI(PopupCode.TutorialUI).GetComponent<TutorialUI>();
                if (temp.isCanNextTut)
                    PopupManager.Instance.OnUI(PopupCode.TutorialUI);

                AssetLoader.Instance.IsTut = false;
            }
        });
    }

    public void _ConclickStory()
    {
        AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
        CardPlayer cardPlayer = CardPlayerManager.Instance.GetCardByID(IdCurrent);
        if (cardPlayer == null)
        {
            NTLog.LogError("CardPlayer is null");
            return;
        }
        CardPlayerManager.Instance.ShowPopupStory(cardPlayer.Index, cardPlayer.Star, cardPlayer.Lv);
    }
}
