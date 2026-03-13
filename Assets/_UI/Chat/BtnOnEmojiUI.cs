using System;
using System.Collections;
using System.Collections.Generic;
using NTPackage.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Rubik._2DGPS.Chat
{
    public class BtnOnEmojiUI : NTButtonEffect
    {
        public Action<int> ActionSend;
        public Vector2 position;

        public PointerEventData LastEventData;

        public override void OnPointerDown(PointerEventData eventData){
            base.OnPointerDown(eventData);
            this.LastEventData = eventData;
        }

        public void OnClick(){
            PopupManager.Instance.OnUI(PopupCode.Emoji_Popup, null, (popup) => {
                Emoji_Popup emojiPopup = (Emoji_Popup)popup;
                Vector2 localPoint;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(emojiPopup.transform as RectTransform, LastEventData.position, LastEventData.pressEventCamera, out localPoint);
                emojiPopup.SetPositionClick(localPoint);
                this.position = localPoint;
                emojiPopup.ActionSend = this.ActionSend;
            });
        }
    }
}