using System.Collections;
using System.Collections.Generic;
using NTPackage.EventDispatcher;
using NTPackage.UI;
using Rubik.Myrk.Controller;
using Rubik.UIController;
using UnityEngine;

namespace Rubik._2DGPS.Chat
{
    public class BtnChat : NTButtonEffect
    {
        public Transform Notify;

        public int TabIndex = 0;

        protected override void Awake()
        {
            this.Init();
        }

        protected override void OnEnable()
        {
            this.Init();
        }

        protected override void Start()
        {
            this.Init();
        }

        public void Click()
        {
            this.UpdateNotify(false);
        }

        public void Init()
        {
            this.Onclick.RemoveAllListeners();
            this.Onclick.AddListener(this.Click);
            this.Onclick.AddListener(this.OnChatUI);
            EventListenerManager.instance.Register(EventCode.Chat_UpdateNotifyChat, "Chat", (data) =>
            {
                this.UpdateNotify(true);
            });
            this.UpdateNotify(ChatService.Instance.NotifyChat.Get("Chat"));
        }

        private void UpdateNotify(bool isNotify)
        {
            if (!isNotify) ChatService.Instance.HideNotifyChat("Chat");
            if (this.IsChose())
            {
                ChatService.Instance.HideNotifyChat("Chat");
                this.Notify.gameObject.SetActive(false);
            }else{
                this.Notify.gameObject.SetActive(isNotify);
            }
        }

        public void OnChatUI(){
            WorldMapUIController.Instance.OnChat(this.TabIndex);
        }
    }
}