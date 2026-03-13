using DG.Tweening;
using GOA.UserData;
using Lean.Localization;
using NTPackage.EventDispatcher;
using NTPackage.Functions;
using NTPackage.UI;
using Rubik.Battle;
using Rubik.CardPlayer;
using Rubik.CharacterPlayer;
using Rubik.Combat;
using Rubik.Common.AudioHelper;
using Rubik.Config;
using Rubik.Myrk.BattleTeam;
using Rubik.UI;
using Rubik.UserProfile;
using Sirenix.OdinInspector;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class HeroPanel : PopupUI
{
    public List<PositionItem> lsHeroPos;
    public List<GameObject> lsOns, lsOffs;
    public List<GearItem> lsHeroButton;
    public GameObject heroGo;
    //[SerializeField] HeroInfoPopup heroInfo;
    public Transform transformSlot, heroPosTrans;
    //AxieInit axieInits;
    public CardUISlot cardCharacterPrefat;
    public GameObject charPre,lineUpButtuonGo;
    public TextAsset DataAxie;
    public TextMeshProUGUI titleTxt;
    public List<CardUISlot> lsChars = new List<CardUISlot>();
    public DragCharacterIUI heroChar;
    public Transform posTrans = null;
    [SerializeField] List<CardPlayer> lsCardHeros = new List<CardPlayer>();
    [SerializeField] List<CardPlayer> lsCardDatas;
    [SerializeField] string[] teams;
    [SerializeField] GearDetail_UI charGearUI;
    [SerializeField] LineUI lineUI;
    [SerializeField] TextMeshProUGUI slotTxt;
    public TextMeshProUGUI TextPower;
    public Camera cam;
    public int IndexSelected = 0;

    [SerializeField] public List<string> lsIDCardTemp = new List<string> { "", "", "", "", "", "", "", "", "" };
    public override void OnUI(object data = null, bool isDefaultSound = true)
    {
        base.OnUI(data, isDefaultSound);
        if(data!=null )
            this.IndexSelected = (int)data;
        if (this.IndexSelected == 0)
            titleTxt.text = "Team";
        else titleTxt.text = "Defense Team";
        isSetTeeam = false;
        Debug.Log("Start line up");

        UpdateData();
        Debug.Log("Start line up : UpdateData");
        foreach(PositionItem item in lsHeroPos)
        {
            item.SetHightOn(false);
        }
        SetUp();
        Debug.Log("End line up");
        EventListenerManager.instance.Register(EventCode.UpdateCardPlayerWhenOffUI, "HeroPanel", (object data) =>
        {
            SetUp();
        });
        lineUpButtuonGo.SetActive(!UserProfileManager.Instance.IsFunctionLocked(LockFunctionType.Hero));
    }
    public override void UpdateData(object data = null)
    {
        base.UpdateData(data);
    }
    public override void ScriptOffUI()
    {
        base.ScriptOffUI();
        EventListenerManager.instance.RemoveListener(EventCode.UpdateCardPlayerWhenOffUI, "HeroPanel");
        foreach (CardUISlot card in lsChars)
        {
            card.UnSetUp();
        }
    }

    public void Character_GearDetail()
    {
        Rubik.Common.AudioHelper.AudioCtrl.Instance.Play(Rubik.Common.AudioHelper.AudioName.UI_Popup_Panel_Open_Default);
        charGearUI.gameObject.SetActive(true);
        charGearUI.SetUpCharacter();
    }
    public void ButtonOnClick(int index)
    {

        //heroGo.SetActive(true);
        //posTrans = lsHeroPos[index].transform;
        //lsHeroPos[index].GetComponent<Image>().sprite = posSprWaitChoose;
    }
    private void SetUp()
    {
        // OpenPopup();
        //SortOriginByIndex(0);
        numberCount = 0;
        var lsCard = CardPlayerManager.Instance.GetListCardPlayer();
        if (lsCard.Count() > 0)
        {

            foreach (CardPlayer card in lsCard)
            {
                if (card != null)
                {
                    Debug.Log(card.Index);
                    // var temp = CardPlayerManager.Instance.GetCardByID(card._id);
                    lsCardDatas.Add(card);
                }

            }
        }
        InitCard(lsCardDatas);
        SetTeam();
        //AssetLoader.Instance.MixSkin(heroChar.charAnim);
        AssetLoader.Instance.MixSkinWithGearsUI(heroChar.charAnim, BattleTeamManager.Instance.GetGearIndexsByIndex(IndexSelected));
    }
    public void SetANim()
    {
        AssetLoader.Instance.MixSkinWithGearsUI(heroChar.charAnim, BattleTeamManager.Instance.GetGearIndexsByIndex(IndexSelected));
    }
    DragCharacterIUI GetCharacter(int index)
    {
        if (lsHeroPos[index].GetComponentInChildren<DragCharacterIUI>() == null)
            return null;
        else
            return lsHeroPos[index].GetComponentInChildren<DragCharacterIUI>();
    }
    public void ChangePosition(int oldPos, int newPos, GameObject go)
    {
        Debug.Log("New pos : " + newPos + " Old Pos " + oldPos);
        var newChar = GetCharacter(newPos);
        string tempNew = teams[oldPos];
        if (newChar != null)
        {
            newChar.transform.SetParent(lsHeroPos[oldPos].transform, false);
            newChar.transform.localPosition = new Vector2(0, 0);
            newChar.indexOfCharacter = oldPos;
            lsHeroPos[oldPos].SetHightOn(true);

            // lsHeroPos[oldPos].GetComponent<Image>().sprite = posSprChoose;
        }
        else
        {
            lsHeroPos[oldPos].SetHightOn(false);
        }

        go.transform.SetParent(lsHeroPos[newPos].transform, false);
        go.transform.localPosition = Vector3.zero;
        go.GetComponent<DragCharacterIUI>().indexOfCharacter = newPos;
        lsHeroPos[newPos].SetHightOn(true);
        teams[oldPos] = teams[newPos];
        teams[newPos] = tempNew;
        //lsHeroPos[newPos].GetComponent<Image>().sprite = posSprChoose;
        this.UpdateCardTeam();

    }
    public void SetLayerCharacter(Transform thisTran, int index, bool setIn = false)
    {
        if (setIn)
        {
            thisTran.SetParent(heroPosTrans, false);
        }
        else
        {
            thisTran.SetParent(lsHeroPos[index].transform, false);
            thisTran.localPosition = Vector2.zero;
        }
    }
    public void SpawnEnemy(int id, Transform parentCard)
    {

        //GameObject charGo = AssetLoader.Instance.lsHeros[id].gameObject;
        //var chess = Instantiate(charGo);
        //SkeletonGraphic characterOfThisCard = chess.GetComponentInChildren<SkeletonGraphic>();
        //characterOfThisCard.transform.localScale = new Vector2(-.5f, .5f);
        //characterOfThisCard.transform.SetParent(parentCard, false);
        //characterOfThisCard.transform.localPosition = Vector2.zero;
        //characterOfThisCard.AnimationState.SetAnimation(1,AnimationConfigs.IDLE, true);

    }
    public List<DragCharacterIUI> lsDragHero = new List<DragCharacterIUI>();
    public GameObject SpawnHero(CardPlayer card, Transform parentCard)
    {

        //var charGo = AssetLoader.Instance.GetHeroDataByIndex(card.Index);
        //SkeletonGraphic charGo;
        var chess = Instantiate(charPre);
        chess.transform.SetParent(parentCard, false);
        chess.transform.localPosition = new Vector2(0, -20);
        chess.transform.localScale = Vector2.zero;
        Vector2 target = 0.8f * Vector2.one;
        //if ((int)card.Index >=12)
        {
            target = 1.5f * target;
        }



        var charAnim = chess.GetComponent<DragCharacterIUI>().charAnim;
        lsDragHero.Add(chess.GetComponent<DragCharacterIUI>());
        chess.GetComponent<DragCharacterIUI>().SetUp(card, cam);
        // CardPlayerManager.Instance.SetSkeletonAnimationData(charAnim, (int)card.Index);
        //SkeletonDataAsset skeletonData;//= charGo.skeAsset;
        //charAnim.skeletonDataAsset = CardPlayerManager.Instance.GetCharacterByIndex((int)card.Index).skeAsset;
        //charAnim.transform.localScale = CardPlayerManager.Instance.GetCharacterByIndex((int)card.Index).baseData.ScaleData;
        //charAnim.Skeleton.SetToSetupPose();
        //charAnim.Initialize(true);
        //if (card.Index == CardPlayerIndex.Card_15)
        //{
        //    charAnim.Skeleton.SetSkin("1");
        //}
        //else if (card.Index == CardPlayerIndex.Card_16)
        //{
        //    charAnim.Skeleton.SetSkin("2");
        //}
        //else if (card.Index == CardPlayerIndex.Card_17)
        //{
        //    charAnim.Skeleton.SetSkin("3");
        //}
        //charAnim.Skeleton.SetSlotsToSetupPose();
        //charAnim.LateUpdate();

        // SpineEditorUtilities.ReloadSkeletonDataAssetAndComponent(charAnim);
        var data = CardPlayerManager.Instance.GetCharacterByIndex((int)card.Index);
        charAnim.skeletonDataAsset = data.skeAsset;
        charAnim.skeletonDataAsset.GetSkeletonData(true);
        charAnim.Initialize(true);
        if ((int)card.Index == 1 || (int)card.Index == 2 || (int)card.Index == 4 || (int)card.Index == 10)
        {
            charAnim.transform.localScale = 0.65f * charAnim.transform.localScale;
        }
        if ((int)card.Index == 0 || (int)card.Index == 3 || (int)card.Index == 8)
        {
            charAnim.transform.localScale = 1.2f * charAnim.transform.localScale;
        }
        else if ((int)card.Index == 7)
        {
            charAnim.transform.localScale = 1.3f * charAnim.transform.localScale;
        }
        else if ((int)card.Index == 11 || (int)card.Index == 5)
        {
            charAnim.transform.localScale = .5f * charAnim.transform.localScale;
        }
        else if ((int)card.Index == 6)
        {
            charAnim.transform.localScale = 1.7f * charAnim.transform.localScale;
        }
        else if ((int)card.Index == 9)
        {
            charAnim.transform.localScale = 1f * charAnim.transform.localScale;
        }
        if ((int)card.Index>=24&& (int)card.Index <=29)
        {
            charAnim.transform.localScale =new Vector2(-charAnim.transform.localScale.x, charAnim.transform.localScale.y);
        }


        if (data.baseData.skinIndex > 0)
            charAnim.Skeleton.SetSkin(data.baseData.skinIndex.ToString());
        if (SpineController.CheckIfAnimationExists(charAnim.AnimationState, AnimationConfigs.IDLE))
        {
            charAnim.AnimationState.SetAnimation(0, AnimationConfigs.IDLE, true);
        }
        else
        {
            charAnim.AnimationState.SetAnimation(0, AnimationConfigs.IDLE_DEFAULT, true);
        }
        chess.transform.DOScale(target, 1).SetEase(Ease.InOutBack);
        return chess;
    }
    Transform GetPosiblePosition()
    {
        foreach (PositionItem go in lsHeroPos)
        {
            if (go.GetComponentInChildren<SkeletonGraphic>() == null)
            {
                return go.transform;
            }
        }
        return null;
    }
    public List<string> lsIDCard = new List<string>();
    public int numberCount = 0;
    bool isSetTeeam = false;
    private void InitCard(List<CardPlayer> lsHero)
    {
        foreach (Transform child in transformSlot)
        {
            Destroy(child.gameObject);
        }

        int i = 0;
        foreach (CardPlayer stats in lsHero)
        {
            if (string.IsNullOrEmpty(stats._id))
                continue;
            CardUISlot cardChar = Instantiate(cardCharacterPrefat, transformSlot);
            lsChars.Add(cardChar);

            ///SpawnEnemy(stats.Index, cardChar.posChar);
            int temp = i;
            int tempX = (int)stats.Index;
            cardChar.SetUp(stats, (string id) =>
            {
                AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
                if (!cardChar.isClick)
                {
                    if (posTrans == null)
                        posTrans = GetPosiblePosition();
                    if (numberCount == 5)
                    {

                        PopupManager.Instance.OnUI(PopupCode.MessagePanel, null, (PopupUI popupUI) =>
                        {
                            popupUI.GetComponent<MessagePanel>().SetData(LeanLocalization.GetTranslationText("deck_full_title", "Lineup full"), LeanLocalization.GetTranslationText("deck_full_des", "Your Line Up is full.  You can only have 5  monsters in the battlefield."));
                        });
                        return;
                    }

                    if (posTrans != null)
                    {
                        cardChar.slotIndex = int.Parse(posTrans.name);
                        NTLog.LogMessage("Id : " + id);
                        cardChar.characterOfCard = SpawnHero(stats, posTrans);
                        cardChar.characterOfCard.GetComponent<DragCharacterIUI>().indexOfCharacter = cardChar.slotIndex;

                        teams[cardChar.slotIndex] = (cardChar.IdCard);
                        numberCount++;


                        slotTxt.text = LeanLocalization.GetTranslationText("slot", "Slot") + " :" + numberCount + "/5";

                        //Time left : < color =#BD7E92>12:02:31</color>
                        cardChar.tickava.gameObject.SetActive(true);
                        posTrans = null;
                        lsHeroPos[cardChar.slotIndex].SetHightOn(true);
                        lsHeroPos[cardChar.slotIndex].spawnEffect.Play();
                    }
                    else
                    {
                        // ZfxGameplayController.Instance.ShowNotiText();

                        return;
                    }
                }
                else
                {
                    if (numberCount == 1)
                    {
                        PopupManager.Instance.OnUI(PopupCode.MessagePanel, null, (PopupUI popupUI) =>
                        {
                            popupUI.GetComponent<MessagePanel>().SetData(LeanLocalization.GetTranslationText("message"), LeanLocalization.GetTranslationText("must_have_1_moster_in_deck"));
                        });
                        return;
                    }


                    teams[cardChar.slotIndex] = "";
                    lsHeroPos[cardChar.characterOfCard.GetComponent<DragCharacterIUI>().indexOfCharacter].SetHightOn(false);
                    lsHeroPos[cardChar.characterOfCard.GetComponent<DragCharacterIUI>().indexOfCharacter].destroyEffect.Play();
                    lsDragHero.Remove(cardChar.characterOfCard.GetComponent<DragCharacterIUI>());
                    Destroy(cardChar.characterOfCard.gameObject);
                    cardChar.characterOfCard = null;

                    cardChar.tickava.gameObject.SetActive(false);
                    numberCount--;
                    slotTxt.text = LeanLocalization.GetTranslationText("slot", "Slot") + " :" + numberCount + "/5";

                }
                cardChar.isClick = !cardChar.isClick;
                if (isSetTeeam)
                    this.UpdateCardTeam();
            });
            cardChar.cardButton.onLongClick.AddListener((t) =>
            {
                ClosePopup();
                PopupManager.Instance.OnUI(PopupCode.CardPlayerInfoUI, (object)stats._id, (PopupUI popupUI) =>
                {
                    popupUI.GetComponent<CardPlayerInfoUI>().statusUI = 1;

                });
            });
            i++;

            // Debug.Log(stats.Index.ToString());
        }
        int numberLine = lsChars.Count / 8;
        //lineUI.ShowLine(numberLine, 700);
        //  AssetLoader.Instance.MixSkin(heroChar.charAnimation);
        //axiesCount = axies;
    }
    //public List<AxieStats> axies;
    void SetTeam()
    {
        int index = 0;
        this.TextPower.text = BattleTeamManager.Instance.GetPower(BattleTeamManager.Instance.GetBattleTeamDataTemp(IndexSelected).ToShortTeam()).ToString();
        List<CardPlayer> lsCards;
        if (IndexSelected == 0)
        {
            lsCards= BattleTeamManager.Instance.GetBattleCardPlayer();
        }
        else if(IndexSelected == 101)
        {
            lsCards= BattleTeamManager.Instance.GetArenaDefendCardPlayer();
        }
        else
        {
            lsCards= BattleTeamManager.Instance.GetBattleCardPlayer();
        }
        foreach (CardPlayer card in lsCards)
        {
            if (card != null)
            {
                var temp = GetCardByID(card._id);
                // 
                if (temp != null)
                {
                    posTrans = lsHeroPos[index].transform;
                    lsHeroPos[index].SetHightOn(true);
                    temp.cardButton.onClick.Invoke(false);
                    //numberCount++;
                    //ChangePosition(lsChars[temp].slotIndex, data.Slot, lsChars[temp].characterOfCard.gameObject);
                }

            }
            index++;
            //ChangePosition(lsChars[temp].slotIndex, data.Slot, lsChars[temp].characterOfCard.gameObject);
        }
        isSetTeeam = true;

    }
    CardUISlot GetCardByID(string _ID)
    {
        for (int i = 0; i < lsChars.Count; i++)
        {
            if (lsChars[i].IdCard == _ID)
            {
                return lsChars[i];
            }
        }
        return null;
    }
    public void SortOrigin()
    {


        //lsIDCard = new List<string>();
        //for (int i = 0; i < CardManager.instance.CardDic.Keys.Count; i++)
        //{
        //    var temp = CardManager.instance.GetCardByID(CardManager.instance.CardDic.Keys[i]);
        //    //lsIDCard.Add(CardManager.instance.CardDic[CardManager.instance.CardDic.Keys[i]].);
        //    // stats = axieInits.GetAxieStats(UserData.instance.lsCardData.Cards[i].Index);
        //    lsCardHeros.Add(temp);

        //}


        //RefreshAxieCard(lsCardHeros);

    }
    public void SortOriginByIndex(int index)
    {
        foreach (GameObject go in lsOns)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in lsOffs)
        {
            go.SetActive(true);
        }
        lsOns[index].SetActive(true);
        lsOffs[index].SetActive(false);
        foreach (CardUISlot card in lsChars)
        {
            if (card.classIndex == index || index == 0)
            {
                card.gameObject.SetActive(true);
            }
            else
            {
                card.gameObject.SetActive(false);
            }
        }

    }
    float offset = .8f;
    public int CheckNearPos(Vector2 pos)
    {
        int index = 0;
        int result = -1;
        foreach (PositionItem hero in lsHeroPos)
        {
            hero.SetOverHightlight(false);
            if (pos.x < hero.transform.position.x + offset && pos.x > hero.transform.position.x - offset && pos.y < hero.transform.position.y + offset && pos.y > hero.transform.position.y - offset)
            {
                //Debug.Log(pos);
                hero.SetOverHightlight(true);
                result = index;
            }

            index++;
        }
        return result;
    }
    public void OpenPopup()
    {
        gameObject.SetActive(true);
        foreach (GameObject go in lsOns)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in lsOffs)
        {
            go.SetActive(true);
        }

        // var lsIDCardTemp = new List<string> { "", "", "", "", "" };
        //teams = lsIDCardTemp.ToArray();
        posTrans = null;
        SortOrigin();
        // SetTeam();

    }
    public void ClosePopup()
    {
        ConfirmUpdateCard();
        OffUI();
        DOVirtual.DelayedCall(0.5f, () =>
        {
            for (int j = 0; j < lsChars.Count; j++)
            {
                if (lsChars[j].characterOfCard != null)
                    Destroy(lsChars[j].characterOfCard);
                Destroy(lsChars[j].gameObject);

            }
            lsChars.Clear();
            lsCardHeros.Clear();
            lsCardDatas.Clear();
            lsDragHero.Clear();
        });




    }

    public void GetCard()
    {
        // APIManager.instance.RandomCard(UserData.instance.data.UserId);
    }

    public void UpdateCardTeam()
    {
        lsIDCardTemp = new List<string> { "", "", "", "", "", "", "", "", "" };
        var tems = lsDragHero;
        if (tems.Count > 0)
        {
            foreach (DragCharacterIUI item in tems)
            {
                lsIDCardTemp[item.indexOfCharacter] = item._ID;
            }
        }
        BattleTeamManager.Instance.UpdateBattleCards(lsIDCardTemp.ToArray(), IndexSelected);
        this.TextPower.text = BattleTeamManager.Instance.GetPower(BattleTeamManager.Instance.GetBattleTeamDataTemp(IndexSelected).ToShortTeam()).ToString();
    }

    [Button]
    public void ConfirmUpdateCard()
    {
        BattleTeamManager.Instance.ConfirmUpdateBattleCards(IndexSelected, (data) =>
        {
            if (data != null)
            {
                NTLog.LogMessage("Update Battle Team Success");
            }
        });
        //Debug.Log(lsIDCard.ToString());
        // CardManager.instance.UpdateListCardTeam( lsIDCardTemp);
    }
    public void OnButtonHero()
    {
        PopupManager.Instance.OnUI(PopupCode.CharacterGear_UI, IndexSelected);
    }
}
