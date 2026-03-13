using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTFunctions_old;
using NTPackage_old.EventDispatcher;

namespace Rubik.Chat{
    public class ChatUI : PopupUI
    {
        public List<ChannelChatUI> channelChatUIs;
        public WhisperUI whisperUI;

        public Transform Notic;

        public override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadChannelChatUIs();
            this.lvUI = new PopupLv().GetValue(transform.name);
        }

        protected void LoadChannelChatUIs(){
            this.channelChatUIs.Clear();
            foreach (Transform item in transform.GetComponentsInChildren<Transform>(true))
            {
                if(item.TryGetComponent<ChannelChatUI>(out ChannelChatUI channelChatUI)){
                    this.channelChatUIs.Add(channelChatUI);
                }
            }
        }

        protected override void Start()
        {
            base.Start();
            EventListenerManager.instance.Register(EventCode.ReciveChatFriend,"ChatUI", (data)=>{this.Notic.gameObject.SetActive(true);});
        }

        //Function

        public void ReceiveChat(PlayerChat playerChat){
            foreach (ChannelChatUI item in this.channelChatUIs)
            {
                if(playerChat.channelId.Equals(item.channelId)){
                    item.InstantiateChatBox(playerChat);
                }
            }
        }

        public void OnUI(){
            if(!this.CanShow()) return;
            this.Show();
        }

        public void OnChatFriend(string channelId, string userName){
            this.OnUI();
            this.whisperUI.ChoseTab();
            this.whisperUI.ChangeChatFrient(channelId, userName);
        }
    }
}