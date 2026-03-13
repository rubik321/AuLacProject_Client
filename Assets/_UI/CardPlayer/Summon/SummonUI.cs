using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GOA.UserData;
using Rubik._2DGPS.Card;
using Spine.Unity;
using Rubik.Combat;
using DG.Tweening;
public class SummonUI : SimplePopup
{
    public Text textSummon;
    public SkeletonGraphic ske;
    protected override void Start()
    {
        base.Start();
    }
    public override void ShowUp(AnimationPopupType type = AnimationPopupType.OnTopDown)
    {
        base.ShowUp(type);
        textSummon.text = UserData.Instance.data.SummonToken_FK.ToString();
    }
    public override void Hide()
    {
        base.Hide();
    }
    IEnumerator StartSummon(List<Card> cards)
    {
        foreach (Card card in cards)
        {
            //ske.skeletonDataAsset =AssetLoader.Instance.lsCharacters[(int)card.Index].skeAsset;
            //ske.Initialize(true);
            //ske.transform.localScale = Vector2.zero;
            //ske.transform.DOScale(new Vector2(.2f,.2f), .4f);
            yield return new WaitForSeconds(1);
            
        }
    }
    
    public void OnButtonSummon(int index)
    {
        CardManager.instance.StartSummon(1,(card)=> {

            StartCoroutine(StartSummon(card));
            
            
        });
        if (UserData.Instance.data.SummonToken_FK > 0)
        {
            //List<Card> lsCardHeros = new List<Card>();
            //for (int i = 0; i < CardManager.instance.CardDic.Keys.Count; i++)
            //{
            //    var temp = CardManager.instance.GetCardByID(CardManager.instance.CardDic.Keys[i]);
            //    //lsIDCard.Add(CardManager.instance.CardDic[CardManager.instance.CardDic.Keys[i]].);
            //    // stats = axieInits.GetAxieStats(UserData.instance.lsCardData.Cards[i].Index);
            //    lsCardHeros.Add(temp);

            //}
            //CardManager.instance.StartSummon(index, (data) => {
            //    textSummon.text = UserData.Instance.data.SummonToken_FK.ToString();
            //});
            
        }
        
        
    }
}
