using System.Collections;
using System.Collections.Generic;
using NTPackage.EventDispatcher;
using UnityEngine;

namespace Rubik._2DGPS.Chat
{
    public class BtnOnChatUI : MonoBehaviour
    {
        public Transform Notify;

        private void Awake()
        {
            this.Init();
        }

        private void OnEnable()
        {
            this.Init();
        }

        private void Start()
        {
            this.Init();
        }
        

        public void Init()
        {
            EventListenerManager.instance.Register(EventCode.Chat_UpdateNotifyChat, "Chat", (data) =>
            {
                this.UpdateNotify(true);
            });
            this.UpdateNotify(ChatService.Instance.NotifyChat.Get("Chat"));
        }

        private void UpdateNotify(bool isNotify)
        {
            // TODO: Update Notify
        }

        public void _Onclick()
        {
            ChatService.Instance.HideNotifyChat("Chat");
            this.Notify.gameObject.SetActive(false);
            // TODO: Show Chat UI
        }
    }
}