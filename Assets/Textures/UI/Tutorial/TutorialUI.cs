using Lean.Localization;
using NTPackage.UI;
using Rubik.CharacterGear;
using Rubik.Combat;
using Rubik.Common.AudioHelper;
using Rubik.Myrk.BattleTeam;
using Rubik.Myrk.Clan;
using Rubik.Myrk.Monster;
using Rubik.UI;
using Rubik.UIController;
using Rubik.UserProfile;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.Windows;

public class TutorialUI : PopupUI
{
    public List<ListTutorials> Tuts;

    [SerializeField]List<BaseTutorial> lsTuts ;
    [SerializeField]public int indexOftut = 0;
    public TutorialType tutType ;
    [SerializeField] UnityEngine.UI.Image skipImg;
    public bool isCanNextTut = false;
    [SerializeField] float timeSkip;
    bool isSkip =false;
    protected override void Start()
    {
        base.Start();
       
    }
    public override void OnUI(object data = null, bool isDefaultSound = true)
    {
        base.OnUI(data, isDefaultSound);
        Debug.Log("tut index " + (TutorialType)tutType);
        lsTuts = Tuts[(int)tutType].tuts;
        OffAllTuts();
        timeSkip = 1;
        skipImg.fillAmount = 0;
        isSkip = false;
        isCanNextTut = true;
        StartTut();
    }
    public override void UpdateData(object data = null)
    {
        base.UpdateData(data);
    }

    public void OffAllTuts()
    {
        foreach(ListTutorials tuts in Tuts)
        {
            foreach(BaseTutorial baseTut in tuts.tuts)
            {
                baseTut.gameObject.SetActive(false);
            }
        }
    }
    public void StartTut()
    {
        StartCoroutine(CoStartTut());
    }
    IEnumerator CoStartTut()
    {
        yield return new WaitForSeconds(lsTuts[indexOftut].timeDelay);
        ShowTut();
    }
    public BaseTutorial currentTut;
     void ShowTut()
    {
        var idTut = indexOftut;
       
        foreach (BaseTutorial tut in lsTuts)
        {
            if(tut.ID == idTut)
            {
                tut.gameObject.SetActive(true);
                currentTut = tut;
            }
            else
            {
                tut.gameObject.SetActive(false);
            }
        }
        indexOftut++;
        if (indexOftut >= lsTuts.Count)
        {
            EndTut();


        }
        
           
    }
    public bool CkeckNextTutorialIsShow()
    {
        int index = (int)tutType;
        isCanNextTut = false;
        while (index < 10)
        {
            if(UserProfileManager.Instance.IsFunctionLocked(Tuts[index].tuts[0].locktype)||UserProfileManager.Instance.IsTutorialDone((TutorialType)index))
            {
                index++;
            }
            else
            {
                isCanNextTut = true;
                tutType = (TutorialType)index;
                break;
            }

        }
        return isCanNextTut;
    }
    public void EndTut(Action callback = null)
    {
       
        Debug.Log("Done tut : " + (TutorialType)UserProfileManager.Instance.GetCountOfTutorials());
        if(tutType != TutorialType.Tutorial9 && tutType != TutorialType.Tutorial5 )
        {
            indexOftut = 0;
            UserProfileManager.Instance.DoneTutorial(tutType, () =>
            {
                CkeckNextTutorialIsShow();
                if (callback != null)
                {
                    callback();
                }
            });
        }
       
    }
    public void EndTutSkip(Action callback = null)
    {
            indexOftut = 0;
      
            UserProfileManager.Instance.DoneTutorial(tutType, () =>
            {
                if (tutType == TutorialType.Tutorial5)
                {
                    var lineup = PopupManager.Instance.GetPopupUI(PopupCode.LineUpUI) as HeroPanel;
                    lineup.ClosePopup();
                    OffUI();
                }
                else
                {
                    OffAllUI();
                }
                
              
                CkeckNextTutorialIsShow();
                
                if (isCanNextTut)
                    OnUI();
                if (callback != null)
                {
                    callback();
                }
            });
      

    }
    public bool isEndOftut()
    {
        return indexOftut >= lsTuts.Count;
    }
    public void SkipDown()
    {
        isSkip = true;
    }
    public void SkipUp()
    {
        isSkip = false;
        skipImg.fillAmount = 0;
      

    }
    protected override void Update()
    {
        base.Update();
        if (timeSkip > 0 && isSkip)
        {
            timeSkip-= Time.deltaTime;
            skipImg.fillAmount = (float)timeSkip / 1;
            if (timeSkip <= 0)
            {
                if (tutType == currentTut.tutDoneIndex)
                {
                    EndTutSkip(() => {
                    });
                    if (!isCanNextTut)
                        OffAllUI();
                }
                else
                {
                    OffAllUI();
                    if (isCanNextTut)
                        OnUI();
                }
                    
            }
        }else if (!isSkip)
        {
            timeSkip = 1;
        }
    }
   

    public void Skip()
    {
       
        currentTut.OnSkip();
       
    }
    public void ShowWordl()
    {
        WorldMapUIController.Instance.OnButtonCharacter_Onclick();
    }
    public void ShowBag()
    {
        WorldMapUIController.Instance.OnButtonInven();
    }
    public void ShowLineUp()
    {
        WorldMapUIController.Instance.OnButtonLineUp();
    }
    public void ShowQuest()
    {
        WorldMapUIController.Instance._OnclickQuest();
    }
    public void ShowShop()
    {
        WorldMapUIController.Instance._OnclickIAPShop();
    }
    public void ShowArena()
    {
        WorldMapUIController.Instance._OnclickArena();
    }
    public void ShowMonster()
    {
        WorldMapUIController.Instance.OnButtonMonster();
        AssetLoader.Instance.IsTut = true;
    }
    public void GetMonsterOnMap()
    {
        FindFirstObjectByType<MonsterManager>().GetMonsterOnMap(0).MonsterSelection.OnClick();
        AssetLoader.Instance.IsTut = true;
    }
    public void ShowShield()
    {
       FindFirstObjectByType<CharacterGearUI>().ButtonTabsSlot(3);
    }
    public void ShowGearInfo()
    {
        FindFirstObjectByType<CharacterGearUI>().GetInvenItem(3).GetComponent<UnityEngine.UI.Button>().onClick.Invoke() ;

        OffUI();
        DG.Tweening.DOVirtual.DelayedCall(0.5f, () => {

            var temp = PopupManager.Instance.GetPopupUI(PopupCode.TutorialUI).GetComponent<TutorialUI>();
            if (temp.isCanNextTut)
                PopupManager.Instance.OnUI(PopupCode.TutorialUI);

        });
    }
    public void StartAttack()
    {
        AssetLoader.Instance.IsTut = true;
        FindFirstObjectByType<MonsterOnMapUI>()._OnclickAttack();
    }
    public void OffAllUI()
    {
        PopupManager.Instance.OffAllPopupUI();
    }
    public void OffTutorial5()
    {
        var lineup = PopupManager.Instance.GetPopupUI(PopupCode.LineUpUI) as HeroPanel;
        lineup.ClosePopup();
        OffUI();
    }
    public void OnButtonLineUp()
    {
        AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
        if (UserProfileManager.Instance.IsFunctionLocked(LockFunctionType.LineUp))
        {
            PopupManager.Instance.OnUI(PopupCode.MessagePanel, null, (PopupUI popupUI) =>
            {
                popupUI.GetComponent<MessagePanel>().SetData(LeanLocalization.GetTranslationText("lock_title", "Feature Locked"), string.Format(LeanLocalization.GetTranslationText("unlock_feature", "Feature unlocks at Level {0}"), UserProfileManager.Instance.GetLevelUnlockFunction(LockFunctionType.LineUp)));
            });
            return;
        }
        PopupManager.Instance.OnUI(PopupCode.LineUpUI, BattleTeamConfig.ArenaDefendTeamIndex);
    }
    public void OnButtonRankArena()
    {
        AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
        FindFirstObjectByType<ArenaUI>().ArenaTabs(2);
        
    }
    public void OnButtonLeaderArena()
    {
        AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
        FindFirstObjectByType<ArenaUI>().ArenaTabs(3);

    }
    public void OffArena()
    {
        AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
        FindFirstObjectByType<ArenaUI>().OffUI();

    }
    public void ShowUpgradeUI()
    {
        AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
        FindFirstObjectByType<MonsterUI>().OnButtonUpgrade();

    }
    public void OnButtonSkip()
    {
        PopupManager.Instance.OnUI(PopupCode.MessageOptionPanel, null, (popupUI) =>
        {
            MessageOptionPanel messageOptionPanel = popupUI as MessageOptionPanel;
            messageOptionPanel.SetData(Lean.Localization.LeanLocalization.GetTranslationText("clan_message"), Lean.Localization.LeanLocalization.GetTranslationText("skip_this_tutorial1","Do you want to skip this tutorial ?"));

            messageOptionPanel.SetActionConfirm(() =>
            {
                EndTutSkip(() => {
                });
                if (!isCanNextTut)
                    OffAllUI();
                popupUI.OffUI();
               
            }, Lean.Localization.LeanLocalization.GetTranslationText("btn_confirm", "Confirm"));
            messageOptionPanel.SetActionReject(() =>
            {
                popupUI.OffUI();
            }, Lean.Localization.LeanLocalization.GetTranslationText("btn_cancel", "Cancel"));
           
        });
    }
}
[Serializable]
public class ListTutorials
{
    public List<BaseTutorial> tuts = new List<BaseTutorial>();
}