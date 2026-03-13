using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTFunctions_old;
using TMPro;
using GOA.UserData;
using Rubik.Friend;

namespace GOA.UIFriends{
    public class FriendRequestBox : MonoBehaviour
    {
        public RelationshipData RelationshipData;
        public RequestTabUI FriendUI;
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
        }

        public void Cancel(){
            NoticFriendDeniedUI noticFriendDeniedUI = (NoticFriendDeniedUI)UIManager.instance.GetPopupUIByCode(PopupCode.NoticFriendDeniedUI);
            noticFriendDeniedUI.OnUI();
            gameObject.SetActive(false);
        }
        public void Accept(){
            FriendController.instance.AcceptFriendRequest(this.RelationshipData._id);
            NoticAddFriendUI noticAddFriendUI = (NoticAddFriendUI)UIManager.instance.GetPopupUIByCode(PopupCode.NoticAddFriendUI);
            noticAddFriendUI.OnUI();
            gameObject.SetActive(false);
        }

        public void OnSeeFriend(){
            GOA.UIProfile.ProfileUI profileUI = (GOA.UIProfile.ProfileUI) NTFunctions_old.UIManager.instance.GetPopupUIByCode(PopupCode.ProfileUI);
            if(profileUI == null) return;
            profileUI.OnUI(new Data("fake", "fakeId", 33));
        }
    }
}
