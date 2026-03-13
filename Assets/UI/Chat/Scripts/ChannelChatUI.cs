using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Rubik.Chat;
using GOA.WorldMap;
using NTFunctions_old;

namespace Rubik.Chat{
    public class ChannelChatUI : LoadBehaviour
    {
        public ChatUI chatUI;
        public string channelId;
        public TMP_InputField inputField;
        public string strInput;

        public BoxChat boxChatCollections;
        public Transform contentBoxChat;

        public ObjectPooling objectPooling;


        public override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadInputField();
            this.LoadContentBoxChat();
            this.LoadBoxChatCollections();
            this.LoadObjectPooling();
        }

        protected void LoadInputField(){
            if(inputField != null) return;
            this.inputField = transform.Find("Panel").Find("InputChatField").Find("InputField(TMP)").GetComponent<TMP_InputField>();
        }

        protected void LoadContentBoxChat(){
            if(contentBoxChat != null) return;
            this.contentBoxChat = transform.Find("Panel").Find("HistoryChat").Find("ScrollView").Find("Viewport").Find("Content");
        }
        
        protected void LoadBoxChatCollections(){
            if(boxChatCollections != null) return;
            this.boxChatCollections = transform.Find("Collections").Find("BoxChat").GetComponent<BoxChat>();
        }

        protected void LoadObjectPooling(){
            if(objectPooling != null) return;
            this.objectPooling = transform.GetComponent<ObjectPooling>();
        }

        //Function
        public void InstantiateChatBox(PlayerChat playerChat){
            try
            {
                if(this.contentBoxChat.childCount > 0){
                    if(this.contentBoxChat.GetChild(0).TryGetComponent(out BoxChat chatBox)){
                        if(chatBox.playerChat.senderId.Equals(playerChat.senderId)){
                            chatBox.AddText(playerChat.msg);
                            return;
                        }
                    }
                }
                
            }
            catch (System.Exception){}
            BoxChat boxChat = null;
            Transform transBoxChat = this.objectPooling.GetObjectFromPooling("BoxChat");
            if(transBoxChat == null){
                boxChat = Instantiate<BoxChat>(this.boxChatCollections);
            }else{
                boxChat = transBoxChat.GetComponent<BoxChat>();
            }
            boxChat.transform.name = "BoxChat";
            boxChat.SetData(playerChat);
            boxChat.transform.SetParent(this.contentBoxChat);
            boxChat.transform.SetSiblingIndex(0);
            boxChat.transform.localScale = new Vector3(1,1,1);
            boxChat.gameObject.SetActive(true);
        }

        public void ReadInput(string a){
            this.strInput = a;
        }

        public void SendChat(){
            Rubik.Common.AudioHelper.AudioCtrl.Instance.Play(Rubik.Common.AudioHelper.AudioName.UI_Button_Worldmap);
            if(inputField.text.Length == 0) return;
            if(!ChatManager.Instance.isConnect) return;
            ChatManager.Instance.sendChatChannel(this.strInput, this.channelId);
            inputField.text = "";
        }

        public void OnUI()
        {
            // Debug.LogWarning("On chat");
            // this.TakeHistoryChat();
        }

        public void UpdateData(){
            this.TakeHistoryChat();
        }

        protected async void TakeHistoryChat(){
            this.objectPooling.PushChildObjectIntoPooling(this.contentBoxChat);
            if(!ChatManager.Instance.isConnect) return;
            await ChatManager.Instance._room.Send("getHistory", new GetHistoryRequest()
            {
                //Muốn lấy lịch sử của room office hay garden thì truyen key vào đây
                channelId = this.channelId,
            });
        }

        public void OffUI(){
            // this.objectPooling.PushChildObjectIntoPooling(this.contentBoxChat);
            gameObject.SetActive(false);
        }
    }
}
