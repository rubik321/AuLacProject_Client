using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Spine.Unity;
using Rubik._2DGPS.Card;
using GOA.UserData;
using Rubik.Combat;
using Rubik.CardPlayer;
using TMPro;
using NTPackage.UI;
using NTPackage.Functions;
using Rubik.UI.Statitic;
public class CardUISlot : MonoBehaviour
{
    public string IdCard;
    public int index;
    [SerializeField] public Image avaCharacter, tickava,boxAva;
    [SerializeField]public SkeletonGraphic charAnim;
    [SerializeField] TextMeshProUGUI nameTxt,levelTxt,originNameTxt;
    public LongClickButton cardButton;
    public int slotIndex;
    public GameObject characterOfCard;
    public Image originImg;
    public StarUI StarUI;
    public int Origin;
    public CardPlayer CardPlayer;
    public CardPlayerData CardPlayerData;
    //public CharacterDragPosition characterOfThisCard;

    // Start is called before the first frame update
    void Start()
    {
        //tickava.gameObject.SetActive(false);
    }
    public bool isClick = false;
    internal int classIndex;

    public void SetUp(CardPlayer card, UnityAction<string> callback = null)
    {
        this.CardPlayer = card;
        this.CardPlayerData = CardPlayerManager.Instance.GetCardPlayerDataByIndex(card.Index);
        //var hero = AssetLoader.Instance.GetHeroDataByIndex(card.Index);
      // avaCharacter.sprite = AssetLoader.Instance.lsCharacters[(int)card.Index].icon;
        //Debug.Log(UserData.Instance.lsCharacters.Length);
        nameTxt.text = "Lvl "+(card.Lv + 1); //AssetLoader.Instance.lsCharacters[(int)card.Index].baseData.Name;
        originNameTxt.text = nameTxt.text;
        IdCard = card._id;
        Origin = (int)this.CardPlayerData.Origin;
        ObjectPoolingManager.Instance.PushChildObjectIntoPooling(boxAva.transform);
        var cardPlayer = CardPlayerManager.Instance.InstantiatePlayerAvatar(card.Index);

        cardPlayer.SetParent(boxAva.transform, false);
        cardPlayer.localPosition = Vector3.zero;
        cardPlayer.localScale = Vector3.one;
        cardPlayer.eulerAngles = Vector3.zero;
       
        //levelTxt.text = card.Lv.ToString();
        //boxAva.sprite = AssetLoader.Instance.lsRaritySprs[hero.baseData.Class];
        ////originNameTxt.text = hero.baseData.Origin.ToString();
        //classIndex = (int)hero.baseData.Origin;
        originImg.sprite = CardPlayerManager.Instance.GetOriginSpriteCircle(this.CardPlayerData.Origin);
        //this.index = card.Index;
        //tickava.gameObject.SetActive(false);
        //cardButton = GetComponent<LongClickButton>();
        StarUI.SetStar(card.Star);
        cardButton.onClick.RemoveAllListeners();
        cardButton.onClick.AddListener((go) =>
        {
            
            if (callback != null)
            {
                callback(this.CardPlayer._id);
            }
            else
            {
                //  List_Axie_Controller.insatace.ShowAxieDetailPopup(stats);
            }

        });
    }

    public void UnSetUp(){
        ObjectPoolingManager.Instance.PushChildObjectIntoPooling(boxAva.transform);
    }
    public void SetUpCardShard(Card card, UnityAction callback = null)
    {
        //if (card.Shard==0)
        //{
        //    gameObject.SetActive(false);
        //}
        //var hero = AssetLoader.Instance.GetHeroDataByIndex(card.Index);

        //avaCharacter.sprite = hero.icon;
        //nameTxt.text = hero.baseData.Name;
        //IdCard = card._id;
        //levelTxt.text = "x"+card.Shard.ToString();
        //boxAva.sprite = AssetLoader.Instance.lsRaritySprs[hero.baseData.Class];
        ////originNameTxt.text = hero.baseData.Origin.ToString();
        ////classIndex = (int)hero.baseData.Origin;
        ////originImg.sprite = AssetLoader.Instance.lsOriginSprs[classIndex - 1];
        //originImg.gameObject.SetActive(false);
        //this.index = card.Index;
        ////tickava.gameObject.SetActive(false);
        //cardButton = GetComponent<LongClickButton>();
        //for (int i = 0; i < lsStars.Count; i++)
        //{
        //    lsStars[i].SetActive(false);
        //}
        cardButton.onClick.AddListener((go) =>
        {

            if (callback != null)
            {
                callback();
            }
            else
            {
                //  List_Axie_Controller.insatace.ShowAxieDetailPopup(stats);
            }

        });
    }
}
