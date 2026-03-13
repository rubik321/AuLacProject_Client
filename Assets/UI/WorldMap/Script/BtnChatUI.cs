using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTFunctions_old;
using NTPackage_old.EventDispatcher;
using Rubik.Chat;

namespace GOA.UIWorldMap{
    public class BtnChatUI : BaseButton
    {
        public Transform Notic;

        protected override void Start()
        {
            base.Start();
            EventListenerManager.instance.Register(EventCode.ReciveChatFriend, "BtnChatUI", this.OnNotic);
            EventListenerManager.instance.Register(EventCode.ReciveChatGlobal, "BtnChatUI", this.OnNotic);
        }

        protected override void OnClick()
        {
            base.OnClick();
            try
            {
                Rubik.Chat.ChatUI chatUI = (Rubik.Chat.ChatUI) UIManager.instance.GetPopupUIByCode(PopupCode.ChatUI);
                if(chatUI == null) return;
                chatUI.OnUI();
                this.Notic.gameObject.SetActive(false);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e);
            }
        }

        public void OnNotic(object data = null){
            Rubik.Chat.ChatUI chatUI = (Rubik.Chat.ChatUI) UIManager.instance.GetPopupUIByCode(PopupCode.ChatUI);
            if(chatUI.IsShow()) return;
            this.Notic.gameObject.SetActive(true);
        }
    }
}
