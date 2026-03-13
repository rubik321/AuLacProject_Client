using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTFunctions_old;
using GOA.UIFriends;
using NTPackage_old.EventDispatcher;

namespace Rubik.Chat{
    public class WhisperUI : TabUI
    {
        public FriendBoxChat friendBoxCollection;


        public Transform transFriendList;
        public Transform transFriendChat;

        public ChannelChatUI friendChannelChatUI;
        
        public Transform content;
        public List<FriendBoxChat> friendBoxes;

        public FriendTitleUI FriendTitleUI;

        public override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadFriendBoxCollection();
            this.LoadTransFriendList();
            this.LoadTransFriendChat();
            this.LoadContent();
            this.LoadFriendBoxes();
            this.LoadFriendChannelChatUI();
        }

        protected void LoadFriendBoxCollection(){
            if(friendBoxCollection != null) return;
            this.friendBoxCollection = transform.Find("Collections").Find("FriendBox").GetComponent<FriendBoxChat>();
        }

        protected void LoadTransFriendList(){
            if(transFriendList != null) return;
            this.transFriendList = transform.Find("Panel").Find("FriendList");
        }
        protected void LoadTransFriendChat(){
            if(transFriendChat != null) return;
            this.transFriendChat = transform.Find("Panel").Find("FriendChat");
        }


        protected void LoadFriendChannelChatUI(){
            if(friendChannelChatUI != null) return;
            this.friendChannelChatUI = transform.Find("Panel").Find("FriendChat").Find("FriendChannelChatUI").GetComponent<ChannelChatUI>();
        }


        protected void LoadContent(){
            if(content != null) return;
            this.content = transform.Find("Panel").Find("ScrollView").Find("Viewport").Find("Content");
        }

        protected void LoadFriendBoxes(){
            this.friendBoxes.Clear();
            foreach (Transform item in this.content)
            {
                if(item.TryGetComponent<FriendBoxChat>(out FriendBoxChat friendBoxe)){
                    this.friendBoxes.Add(friendBoxe);
                    friendBoxe.whisperUI = this;
                }
            }
        }

        public void ChangeChatFrient(string channelId, string userName){
            this.FriendTitleUI.textName.text = userName;
            this.friendChannelChatUI.channelId = channelId;
            this.friendChannelChatUI.UpdateData();
            this.transFriendChat.gameObject.SetActive(true);
            this.transFriendList.gameObject.SetActive(false);
        }

        public void ChangeListFriend(){
            this.transFriendChat.gameObject.SetActive(false);
            this.transFriendList.gameObject.SetActive(true);
        }

        public override void OnUI()
        {
            base.OnUI();
            this.ChangeListFriend();
            this.SetNotion();
        }

        protected void SetNotion(bool notic = false){
            try
            {
                ChatUI chatUI = (ChatUI) UIManager.instance.GetPopupUIByCode(PopupCode.ChatUI);
                chatUI.Notic.gameObject.SetActive(notic);
            }
            catch (System.Exception)
            {
            }
        }

        public override void UpdateData()
        {
            base.UpdateData();
            this.LoadHistoryFriendBoxChat();
        }

        
        protected void LoadHistoryFriendBoxChat(){
            NTFunction.ClearChild(this.content);
            try
            {
                List<FriendBoxChatData> friendBoxChatDatas = new List<FriendBoxChatData>(ChatManager.Instance.FriendBoxChatDataDictionary.Values);
                friendBoxChatDatas.Sort((a,b)=>{return (a.Time > b.Time) ?-1 : 1;});
                Debug.LogWarning(friendBoxChatDatas.Count);
                foreach (FriendBoxChatData item in friendBoxChatDatas)
                {
                    FriendBoxChat friendBoxChat = Instantiate(this.friendBoxCollection);
                    friendBoxChat.SetData(item);
                    friendBoxChat.whisperUI = this;
                    friendBoxChat.transform.SetParent(this.content);
                    NTFunction.ResetPosition(friendBoxChat.transform);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e);
            }
            
        }

    }
}
