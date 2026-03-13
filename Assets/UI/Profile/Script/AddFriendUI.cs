using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NTFunctions_old;
using TMPro;
using GOA.UserData;
using Rubik.Chat;
using Rubik.Friend;

namespace GOA.UIProfile
{
    public class AddFriendUI : PopupUI
    {
        public PlayerChat PlayerChat;
        public Image avatar;
        public TextMeshProUGUI textName;
        public TextMeshProUGUI textLevel;

        public Transform btn_addFriend;

        public override void LoadComponents()
        {
            base.LoadComponents();
            this.lvUI = new PopupLv().GetValue(transform.name);
            this.LoadTextName();
            this.LoadTextLevel();
            this.LoadAvatar();
        }

        protected void LoadAvatar(){
            if(avatar != null) return;
            this.avatar = transform.Find("Panel").Find("Board").Find("Player").Find("AvatarBG").Find("Avatar").GetComponent<Image>();
        }
        protected void LoadTextName(){
            if(textName != null) return;
            this.textName = transform.Find("Panel").Find("Board").Find("Player").Find("NameBG").Find("TextName(TMP)").GetComponent<TextMeshProUGUI>();
        }
        protected void LoadTextLevel(){
            if(textLevel != null) return;
            this.textLevel = transform.Find("Panel").Find("Board").Find("Player").Find("TextLevel(TMP)").GetComponent<TextMeshProUGUI>();
        }
        
        public override void UpdateData(){
            this.UpdateText();
        }

        protected void UpdateText(){
            this.textName.text = this.PlayerChat.senderName.ToString();
            this.textLevel.text = Lean.Localization.LeanLocalization.GetTranslationText("level", "Level") +" "+ 1;
        }

        public void OnUI(PlayerChat playerChat){
            if(!this.CanShow()) return;
            this.PlayerChat = playerChat;
            this.Show();
            try
            {
                if(FriendController.instance.ListRelationship.Get(playerChat.senderId)){
                    this.btn_addFriend.gameObject.SetActive(false);
                }else{
                    throw null;
                }
            }
            catch (System.Exception)
            {
                this.btn_addFriend.gameObject.SetActive(true);
            }
            this.textName.text = this.PlayerChat.senderName;
        }

        public void AddFriend(){
            FriendController.instance.SendFriendRequest(this.PlayerChat.senderId);
            Debug.LogWarning("Add Friend");
            this.btn_addFriend.gameObject.SetActive(false);
            FriendRequestUI friendRequestUI = (FriendRequestUI)UIManager.instance.GetPopupUIByCode(PopupCode.FriendRequestUI);
            if(friendRequestUI == null) return;
            friendRequestUI.OnUI();
        }

        // public void See(){
        //     ProfileUI profileUI = (ProfileUI) UIManager.instance.GetPopupUIByCode(PopupCode.ProfileUI);
        //     if(profileUI == null) return;
        //     profileUI.OnUI(new Data("Fake user", "fake", 30));
        // }
    }
}
