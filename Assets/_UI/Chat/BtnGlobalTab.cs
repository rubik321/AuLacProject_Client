using System.Collections;
using System.Collections.Generic;
using NTPackage.EventDispatcher;
using NTPackage.UI;
using UnityEngine;

namespace Rubik._2DGPS.Chat
{
    public class BtnGlobalTab : BtnTab
    {
        public Transform Notify;

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

        public override void Click()
        {
            base.Click();
            this.UpdateNotify(false);
        }

        public void Init()
        {
            this.Onclick.RemoveAllListeners();
            this.Onclick.AddListener(this.Click);
            EventListenerManager.instance.Register(EventCode.Chat_UpdateNotifyChat, "Chat_Server_", (data) =>
            {
                this.UpdateNotify(true);
            });
            this.UpdateNotify(ChatService.Instance.NotifyChat.Get("Chat_Server_"));
        }

        private void UpdateNotify(bool isNotify)
        {
            if (!isNotify) ChatService.Instance.HideNotifyChat("Chat_Server_");
            if (this.IsChose())
            {
                ChatService.Instance.HideNotifyChat("Chat_Server_");
                this.Notify.gameObject.SetActive(false);
            }else{
                this.Notify.gameObject.SetActive(isNotify);
            }
        }
    }
}