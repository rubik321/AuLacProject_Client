using Rubik.UI.Statitic;
using Rubik.UserDataPlayer;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rubik.ItemPlayer
{
    using NTPackage.Functions;
    using NTPackage.UI;
    using Rubik.BattleEngine;
    using Rubik.CardPlayer;
    using Rubik.CharacterGear;
    using Rubik.Format;
    using Rubik.Myrk.BattleTeam;
    using Rubik.UI;

    public class ItemDataUI : NTBehaviour
    {
        public ItemData ItemData;
        public CharacterGear CharacterGear; 
        public CardPlayer CardPlayer;
        Vector2 originSkePos;

        // Item
        public Transform Item;
        public Image IconItem;
        public TextMeshProUGUI AmountItem;

        // Gear
        public CharacterGearItemUI CharacterGearItemUI;

        // Card
        public Transform Card;
        public Transform CardAvaHolder;
        public TextMeshProUGUI TextNameCard;
        public StarUI StarUI;


        public NTButtonEffect OnclickToolTip;


        public void SetData(ItemData itemData,bool showMulti = false, bool isShowToolTip = false){
            this.ItemData = itemData;

            IconItem.sprite = ItemDataManager.Instance.GetIcon(itemData.Type);
            string str = NTFunction.FormatHigherNumber(itemData.Amount);
            if(!showMulti)
                AmountItem.text = str;
            else
                AmountItem.text = "x"+str;
            if (this.ItemData.Amount <= 0) this.AmountItem.text = "";
            Item.gameObject.SetActive(true);
            CharacterGearItemUI.gameObject.SetActive(false);
            Card.gameObject.SetActive(false);
            this.OnclickToolTip.gameObject.SetActive(isShowToolTip);
        }

        public void SetData(CharacterGear characterGear, bool isShowToolTip = false){
            this.CharacterGear = characterGear;
            CharacterGearItemUI.SetData(characterGear, isShowToolTip);
            Item.gameObject.SetActive(false);
            CharacterGearItemUI.gameObject.SetActive(true);
            Card.gameObject.SetActive(false);
            this.OnclickToolTip.gameObject.SetActive(isShowToolTip);
        }

        public void SetData(CardPlayer cardPlayer, bool isShowToolTip = false){
            this.CardPlayer = cardPlayer;
            TextNameCard.text = CardPlayerManager.Instance.GetCardName(cardPlayer.Index);
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(CardAvaHolder);
            Transform cardAva = CardPlayerManager.Instance.InstantiatePlayerAvatar(cardPlayer.Index);
            cardAva.SetParent(CardAvaHolder);
            NTFunction.ResetPosition(cardAva);
            StarUI.SetStar(cardPlayer.Star);
            Item.gameObject.SetActive(false);
            CharacterGearItemUI.gameObject.SetActive(false);
            Card.gameObject.SetActive(true);
            this.OnclickToolTip.gameObject.SetActive(isShowToolTip);
        }
        public void SetData(CardShortTeam cardPlayer, bool isShowToolTip = false)
        {
            //this.CardPlayer = cardPlayer;
            TextNameCard.text = CardPlayerManager.Instance.GetCardName(cardPlayer.Index);
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(CardAvaHolder);
            Transform cardAva = CardPlayerManager.Instance.InstantiatePlayerAvatar(cardPlayer.Index);
            cardAva.SetParent(CardAvaHolder);
            NTFunction.ResetPosition(cardAva);
            StarUI.SetStar(cardPlayer.Star);
            Item.gameObject.SetActive(false);
            CharacterGearItemUI.gameObject.SetActive(false);
            Card.gameObject.SetActive(true);
            this.OnclickToolTip.gameObject.SetActive(isShowToolTip);
        }

        public void OnToolTip(){
            if(this.Item.gameObject.activeSelf){
                ItemDataManager.Instance.ShowToolTip(this.ItemData.Type);
            }
            else if(this.Card.gameObject.activeSelf){
                CardPlayerManager.Instance.ShowPopupStory(this.CardPlayer.Index, this.CardPlayer.Star, this.CardPlayer.Lv, 1);
            }
            else{
                string title = CharacterGearManager.Instance.GetGearNameByIndex(this.CharacterGear.Index);
                string description = CharacterGearManager.Instance.GetGearDesByIndex(this.CharacterGear.Index);
                HUDCanvas.Instance.ShowToolTip(title, description);
            }
        }
    }
}
