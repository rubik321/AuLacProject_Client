using System.Collections;
using System.Collections.Generic;
using NTPackage.Functions;
using NTPackage.UI;
using Rubik._2DGPS.Chat;
using Rubik.Chat;
using UnityEngine;

namespace Rubik.Friend
{
    public class FriendTab : TabUI
    {
        public FriendItemUI FriendItemPrefab;
        public List<FriendItemUI> FriendItemList;
        public Transform Holder;
        public ChatChannelUI ChatChannelUI;
        public FriendItemUI FriendItemSelected;

        public override void UpdateData()
        {
            base.UpdateData();
            this.UpdateFriendList();
        }

        public override void OffUI()
        {
            base.OffUI();
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(Holder);
            this.FriendItemSelected = null;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            this.OffUI();
        }

        public void UpdateFriendList()
        {
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(Holder);
            this.FriendItemList.Clear();
            foreach (FriendData friendData in FriendManager.Instance.GetFriendList())
            {
                FriendItemUI friendItemUI = ObjectPoolingManager.Instance.PullObjectFromPooling<FriendItemUI>(ObjectPoolingConfig.FriendItemUI);
                if(friendItemUI == null) friendItemUI = Instantiate(this.FriendItemPrefab);
                this.FriendItemList.Add(friendItemUI);
                friendItemUI.transform.SetParent(Holder);
                friendItemUI.gameObject.SetActive(true);
                friendItemUI.transform.name = ObjectPoolingConfig.FriendItemUI;
                NTFunction.ResetPosition(friendItemUI.transform);
                friendItemUI.SetData(friendData, this);
            }
            this.ChatChannelUI.gameObject.SetActive(false);
            if(this.FriendItemList.Count > 0) this.OnClickChat(this.FriendItemList[0]);
        }

        public void OnClickChat(FriendItemUI friendItemUI)
        {
            if(this.FriendItemSelected == friendItemUI) return;
            this.FriendItemSelected = friendItemUI;
            this.ChatChannelUI.gameObject.SetActive(true);
            this.ChatChannelUI.SetData(FriendManager.Instance.GetFriendChatChannel(friendItemUI.FriendData._id));
        }
    }
}