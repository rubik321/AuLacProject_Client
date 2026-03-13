using System;
using System.Collections;
using System.Collections.Generic;
using NTPackage.EventDispatcher;
using NTPackage.Functions;
using NTPackage.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rubik._2DGPS.Chat
{
    public class ChatChannelUI : NTBehaviour
    {
        public string ChannelID;
        public TMP_InputField inputField;
        public string strInput;

        public BoxChatItemUI BoxChatItemPrefab;
        public Transform ContentBoxChat;
        public List<BoxChatItemUI> ListBoxChatItem = new List<BoxChatItemUI>();

        public ChatHistoryData ChatHistoryData;

        public NTButtonEffect BtnMute;

        public BtnOnEmojiUI BtnOnEmojiUI;

        public Action<SendChatData> ActionReceiveRealTimeChat = null;

        public bool IsSave = true;

        public Action SendChatAction;

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            this.UnSetData();
        }

        public void SetData(string channelID, Action sendChatAction = null){
            this.SendChatAction = sendChatAction;
            this.ChannelID = channelID;
            EventListenerManager.instance.Register(EventCode.Chat_ReceiveChat, this.ChannelID, this.ReceiveRealTimeChat);
            EventListenerManager.instance.Register(EventCode.Chat_ReceiveHistoryChat, this.ChannelID, this.ReceiveHistoryChat);
            this.ListBoxChatItem.Clear();
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.ContentBoxChat);
            this.inputField.text = "";
            if(ChatService.Instance.IsMuted()){
                this.BtnMute.Unchose();
            }else{
                this.BtnMute.Chose();
            }
            this.UpdateData();
            this.BtnOnEmojiUI.ActionSend = this.SendChatWithEmoji;
        }

        public void UnSetData(){
            this.ChannelID = "";
            EventListenerManager.instance.RemoveListener(EventCode.Chat_ReceiveChat, this.ChannelID);
            EventListenerManager.instance.RemoveListener(EventCode.Chat_ReceiveHistoryChat, this.ChannelID);
            this.ListBoxChatItem.Clear();
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.ContentBoxChat);
        }

        public void UpdateData(){
            this.ChatHistoryData = ChatService.Instance.GetChatHistoryData(this.ChannelID);
            if(this.ChatHistoryData == null) return;
            foreach(SendChatData sendChatData in this.ChatHistoryData.Data){
                this.ReceiveChat(sendChatData);
            }
        }

        public void ReceiveHistoryChat(object data){
            this.ReceiveChat(data, true);
        }

        public void ReceiveRealTimeChat(object data){
            this.ReceiveChat(data, false);
        }

        public void ReceiveChat(object data, bool isHistory = false){
            SendChatData sendChatData = (SendChatData)data;
            if(this.ListBoxChatItem.Count > ChatService.MAX_CHAT_HISTORY){
                BoxChatItemUI oldestBoxChatItem = this.ListBoxChatItem[0];
                ObjectPoolingManager.Instance.PushObjectIntoPooling(oldestBoxChatItem.transform);
                this.ListBoxChatItem.RemoveAt(0);
            }

            BoxChatItemUI boxChatItem = ObjectPoolingManager.Instance.PullObjectFromPooling<BoxChatItemUI>(ObjectPoolingConfig.BoxChatItemUI);
            if(boxChatItem == null){
                boxChatItem = Instantiate<BoxChatItemUI>(this.BoxChatItemPrefab);
            }
            boxChatItem.transform.name = "BoxChatItemUI";
            boxChatItem.SetData(sendChatData);
            this.ListBoxChatItem.Add(boxChatItem);
            this.ListBoxChatItem.Sort((a, b) => a.SendChatData.TimeSending.CompareTo(b.SendChatData.TimeSending));
            boxChatItem.transform.SetParent(this.ContentBoxChat);
            boxChatItem.transform.SetSiblingIndex(this.ListBoxChatItem.IndexOf(boxChatItem));
            boxChatItem.gameObject.SetActive(true);
            NTFunction.ResetPosition(boxChatItem.transform);
            if(!isHistory){
                this.ActionReceiveRealTimeChat?.Invoke(sendChatData);
            }
        }

        public void OnTextChanged(string text){
            this.strInput = text;
        }

        public void SendChat(){
            if(this.strInput == "") return;
            if(this.ChannelID == null || this.ChannelID == "") return;
            ChatService.Instance.SendChat(this.ChannelID, this.strInput, this.IsSave);
            this.inputField.text = "";
            this.strInput = "";
            this.SendChatAction?.Invoke();
        }

        public void SendChatWithEmoji(int emoji){
            ChatService.Instance.SendChatWithEmoji(this.ChannelID, emoji, this.IsSave);
            this.inputField.text = "";
            this.strInput = "";
        }

        public void OnMute(){
            ChatService.Instance.MuteChat();
            if(ChatService.Instance.IsMuted()){
                this.BtnMute.Unchose();
            }else{
                this.BtnMute.Chose();
            }
        }

    }
}