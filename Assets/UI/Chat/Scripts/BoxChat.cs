using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rubik.Chat;
using TMPro;
using NTFunctions_old;
using UnityEngine.UI;
using GOA.UserData;
using GOA.UIProfile;

namespace Rubik.Chat
{
    public class BoxChat : LoadBehaviour{
        public PlayerChat playerChat;

        public BoxChatPlayer author;
        public BoxChatPlayer other;

        public override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadAuthor();
            this.LoadOther();
        }

        protected void LoadAuthor(){
            if(author != null) return;
            this.author = transform.Find("Author").GetComponent<BoxChatPlayer>();
        }
        protected void LoadOther(){
            if(other != null) return;
            this.other = transform.Find("Other").GetComponent<BoxChatPlayer>();
        }

        public void SetData(PlayerChat playerChat){
            this.playerChat = playerChat;
            if(this.isAuthor()){
                this.author.gameObject.SetActive(true);
                this.other.gameObject.SetActive(false);
            }else{
                this.author.gameObject.SetActive(false);
                this.other.gameObject.SetActive(true);
            }
            this.author.textSenderName.text = playerChat.senderName;
            this.author.textContent.text = playerChat.msg;
            this.other.textSenderName.text = playerChat.senderName;
            this.other.textContent.text = playerChat.msg;
        }

        public void AddText(string msg){
            this.author.textContent.text = this.author.textContent.text + "\n" + msg;
            this.other.textContent.text = this.other.textContent.text + "\n" + msg;
        }

        protected bool isAuthor(){
            try
            {
                return UserData.Instance.data.UserId.Equals(this.playerChat.senderId);
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public void AddFriend(){
            AddFriendUI addFriendUI = (AddFriendUI) UIManager.instance.GetPopupUIByCode(PopupCode.AddFriendUI);
            if(addFriendUI == null) return;
            addFriendUI.OnUI(this.playerChat);
        }
    }
}