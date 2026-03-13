using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.Friend
{
    using NTPackage.UI;
    using Rubik.MsgDelivery;
    using UnityEngine.UI;

    public class FriendUI : PopupUI
    {
        public MultiTabUI MultiTabUI;
       
        public override void LoadComponents()
        {
            base.LoadComponents();
            this.popupCode = PopupCode.FriendUI;
        }

        public override void OnUI(object data = null, bool isDefaultSound = true)
        {
            base.OnUI(data, isDefaultSound);
            this.UpdateData();
            this.MultiTabUI.OnUI();
        }
        public override void ScriptOffUI()
        {
            base.ScriptOffUI();
            this.MultiTabUI.OnUI();

        }
        public void UpdateData()
        {
            List<string> friendID = new List<string>();
            foreach (FriendData friendData in FriendManager.Instance.GetFriendList())
            {
                friendID.Add(friendData.GetPartnerID());
            }
            foreach (FriendData friendData in FriendManager.Instance.GetFriendRequestList())
            {
                friendID.Add(friendData.GetPartnerID());
            }
            MsgDeliveryRoom.Instance.CheckUserOnline(friendID);
        }
    }
}