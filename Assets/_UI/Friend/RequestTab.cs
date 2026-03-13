using System.Collections;
using System.Collections.Generic;
using NTPackage.Functions;
using NTPackage.UI;
using UnityEngine;

namespace Rubik.Friend
{
    public class RequestTab : TabUI
    {
        public RequestItemUI RequestItemUIPrefab;
        public List<RequestItemUI> RequestItemList;
        public Transform Holder;

        public override void UpdateData()
        {
            base.UpdateData();
            this.UpdateFriendList();
        }

        public override void OffUI()
        {
            base.OffUI();
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(Holder);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            this.OffUI();
        }

        public void UpdateFriendList()
        {
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(Holder);
            this.RequestItemList.Clear();
            List<FriendData> friendRequestList = FriendManager.Instance.GetFriendRequestList();
            foreach (FriendData friendData in friendRequestList)
            {
                RequestItemUI requestItemUI = ObjectPoolingManager.Instance.PullObjectFromPooling<RequestItemUI>(ObjectPoolingConfig.RequestItemUI);
                if(requestItemUI == null) requestItemUI = Instantiate(this.RequestItemUIPrefab);
                requestItemUI.SetData(friendData);
                this.RequestItemList.Add(requestItemUI);
                requestItemUI.transform.SetParent(Holder);
                requestItemUI.transform.name = ObjectPoolingConfig.RequestItemUI;
                NTFunction.ResetPosition(requestItemUI.transform);
            }
        }
    }
}