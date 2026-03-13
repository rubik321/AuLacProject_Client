using System.Collections;
using System.Collections.Generic;
using NTPackage.Functions;
using UnityEngine;
using NTPackage.UI;

namespace Rubik.Friend
{
    using Rubik.Chat;
    using Rubik.MsgDelivery;
    using TMPro;
    using UserProfile;
    public class FriendItemUI : NTButtonEffect
    {
        public TextMeshProUGUI TextStatus;
        public TextMeshProUGUI TextName;
        public AvatarPlayerUI AvatarPlayerUI;

        public FriendData FriendData;

        public FriendTab FriendTab;

        public void SetData(FriendData friendData, FriendTab friendTab)
        {
            this.FriendData = friendData;
            this.FriendTab = friendTab;
            this.AvatarPlayerUI.SetData(friendData.PartnerAvatar, friendData.PartnerBorder, friendData.PartnerCustom, friendData.PartnerLevel);
            this.TextName.text = friendData.PartnerDisplay;

            if (MsgDeliveryRoom.Instance.GetUserOnlineStatus(friendData.GetPartnerID()) == true)
            {
                this.TextStatus.text = "Online";
                this.TextStatus.color = Color.green;
            }
            else
            {
                if (this.UpdateFriendStatusCoroutine != null) StopCoroutine(this.UpdateFriendStatusCoroutine);
                this.UpdateFriendStatusCoroutine = StartCoroutine(this.UpdateFriendStatus());
            }
        }

        public void RemoveFriend(FriendData friendData)
        {
            StartCoroutine(FriendManager.Instance.RemoveFriend(friendData.GetPartnerID(), () =>
            {
                gameObject.SetActive(false);
            }));
        }

        public Coroutine UpdateFriendStatusCoroutine;
        public IEnumerator UpdateFriendStatus()
        {
            if (MsgDeliveryRoom.Instance.GetUserOnlineStatus(this.FriendData.GetPartnerID()) == true)
            {
                this.TextStatus.text = "Online";
                this.TextStatus.color = Color.green;
            }else{
                this.TextStatus.text = "Offline";
                this.TextStatus.color = Color.gray;
            }
            yield return new WaitForSeconds(0.5f);
            if (MsgDeliveryRoom.Instance.GetUserOnlineStatus(this.FriendData.GetPartnerID()) == true)
            {
                this.TextStatus.text = "Online";
                this.TextStatus.color = Color.green;
            }
            else
            {
                this.TextStatus.text = NTFunction.Format_Time(this.FriendData.GetTimeOffline(), 1) + " ago";
                this.TextStatus.color = Color.gray;
            }
        }

        public void OnclickChat(){
            this.FriendTab.OnClickChat(this);
        }
    }
}