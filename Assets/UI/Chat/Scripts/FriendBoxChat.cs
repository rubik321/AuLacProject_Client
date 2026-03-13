using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NTFunctions_old;
using GOA.UserData;
using TMPro;
using GOA.UIProfile;

namespace Rubik.Chat{
    public class FriendBoxChat : LoadBehaviour
    {
        public WhisperUI whisperUI;
        public string channelChatUI;

        public TextMeshProUGUI TextName;
        public TextMeshProUGUI TextLv;
        public TextMeshProUGUI TextMsg;
        public Image Ava;

        public Transform online;
        public Transform offline;

        public Transform Notic;
        public FriendBoxChatData FriendBoxChatData;

        public override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadOnline();
            this.LoadOffline();
        }

        public void SetData(Chat.FriendBoxChatData friendBoxChatData){
            this.FriendBoxChatData = friendBoxChatData;
            string[] userNames = friendBoxChatData.PlayerChat.channelId.Split("&");
            string userName1 = userNames[0];
            string userName2 = userNames[1];
            if(userName1.Equals(UserData.Instance.data.UserName))
                this.TextName.text = userName2;
            else this.TextName.text = userName1;
            this.TextMsg.text = friendBoxChatData.PlayerChat.msg;
            this.channelChatUI = friendBoxChatData.PlayerChat.channelId;
            if(friendBoxChatData.IsNew){
                this.Notic.gameObject.SetActive(true);
            }else{
                this.Notic.gameObject.SetActive(false);
            }
        }

        protected void LoadOnline(){
            if(online != null) return;
            this.online = transform.Find("Online");
        }
        protected void LoadOffline(){
            if(offline != null) return;
            this.offline = transform.Find("Offline");
        }

        public void OnChatFriend(){
            if(this.whisperUI == null) return;
            this.whisperUI.ChangeChatFrient(channelChatUI, this.TextName.text);
            this.FriendBoxChatData.IsNew = false;
            this.Notic.gameObject.SetActive(false);
            ChatManager.Instance.UpdateHistoryChat();
        }

        public void OnSeeFriend(){
            ProfileUI profileUI = (ProfileUI) NTFunctions_old.UIManager.instance.GetPopupUIByCode(PopupCode.ProfileUI);
            if(profileUI == null) return;
            profileUI.OnUI(new Data("fake", "fakeId", 33));
        }
    }
}
