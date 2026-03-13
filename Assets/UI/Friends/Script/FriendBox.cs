using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTFunctions_old;
using TMPro;
using GOA.UserData;
using Rubik.Friend;

namespace GOA.UIFriends{
    public class FriendBox : LoadBehaviour
    {
        public RelationshipData RelationshipData;

        public TextMeshProUGUI textName;
        public TextMeshProUGUI textLv;
        public string channelChatUI;
        public Transform online;
        public Transform offline;

        public void UpdateData(){
            if(this.RelationshipData.user1.Equals(UserData.UserData.Instance.data.UserId)){
                this.textName.text = this.RelationshipData.user2Name;
            }else{
                this.textName.text = this.RelationshipData.user1Name;
            }
            this.channelChatUI = this.RelationshipData.user1Name+"&"+this.RelationshipData.user2Name;
        }

        public void OnChatFriend(){
            Rubik.Chat.ChatUI chatUI = (Rubik.Chat.ChatUI) UIManager.instance.GetPopupUIByCode(PopupCode.ChatUI);
            chatUI.OnChatFriend(this.channelChatUI, this.textName.text);
        }

        public void OnSeeFriend(){
            GOA.UIProfile.ProfileUI profileUI = (GOA.UIProfile.ProfileUI) NTFunctions_old.UIManager.instance.GetPopupUIByCode(PopupCode.ProfileUI);
            if(profileUI == null) return;
            profileUI.OnUI(new Data("fake", "fakeId", 33));
        }
    }
}
