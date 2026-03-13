using System.Collections;
using System.Collections.Generic;
using NTPackage.Functions;
using Rubik.UserDataPlayer;
using Rubik.UserProfile;
using TMPro;
using UnityEngine;

namespace Rubik._2DGPS.Chat
{
    public class BoxChatItemUI : MonoBehaviour
    {
        // Other
        public Transform OtherContent;
        public AvatarPlayerUI OtherAvatarPlayerIcon;
        public TextMeshProUGUI OtherNameText;
        public TextMeshProUGUI OtherMessageText;
        public TextMeshProUGUI OtherTimeText;
        public Transform OtherTransEmoji;
        public Transform OtherHolder;

        // Player
        public Transform PlayerContent;
        public AvatarPlayerUI PlayerAvatarPlayerIcon;
        public TextMeshProUGUI PlayerNameText;
        public TextMeshProUGUI PlayerMessageText;
        public TextMeshProUGUI PlayerTimeText;
        public Transform PlayerTransEmoji;
        public Transform PlayerHolder;

        public SendChatData SendChatData;
        public void SetData(SendChatData sendChatData)
        {
            this.SendChatData = sendChatData;
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.OtherHolder);
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.PlayerHolder);
            PlayerChatData playerChatData = JsonUtility.FromJson<PlayerChatData>(sendChatData.Data);
            Transform transEmoji = null;
            if (sendChatData.Emoji >= 0)
            {
                transEmoji = ObjectPoolingManager.Instance.PullObjectFromPooling(ObjectPoolingConfig.EmojiPrefab + sendChatData.Emoji);
                if (transEmoji == null) transEmoji = Instantiate(UserProfileManager.Instance.GetEmojiPrefabByIndex(sendChatData.Emoji)).transform;
                transEmoji.name = ObjectPoolingConfig.EmojiPrefab + sendChatData.Emoji;
            }
            if (sendChatData.UserID.Equals(UserDataManager.Instance.GetUserID()))
            {
                PlayerContent.gameObject.SetActive(true);
                OtherContent.gameObject.SetActive(false);
                PlayerMessageText.text = sendChatData.Text;
                PlayerTimeText.text = NTFunction.UnixTimestampToDateTime(sendChatData.TimeSending).ToString();

                PlayerAvatarPlayerIcon.SetData(playerChatData.Avatar, playerChatData.Border, playerChatData.Custom, playerChatData.Level);
                PlayerNameText.text = playerChatData.DisplayName;
                if (transEmoji != null)
                {
                    this.PlayerTransEmoji.gameObject.SetActive(true);
                    transEmoji.SetParent(this.PlayerHolder);
                    NTFunction.ResetPosition(transEmoji);
                }
                else
                {
                    this.PlayerTransEmoji.gameObject.SetActive(false);
                }
            }
            else
            {
                PlayerContent.gameObject.SetActive(false);
                OtherContent.gameObject.SetActive(true);
                OtherMessageText.text = sendChatData.Text;
                OtherTimeText.text = NTFunction.UnixTimestampToDateTime(sendChatData.TimeSending).ToString();

                OtherAvatarPlayerIcon.SetData(playerChatData.Avatar, playerChatData.Border, playerChatData.Custom, playerChatData.Level);
                OtherNameText.text = playerChatData.DisplayName;
                if (transEmoji != null)
                {
                    this.OtherTransEmoji.gameObject.SetActive(true);
                    transEmoji.SetParent(this.OtherHolder);
                    NTFunction.ResetPosition(transEmoji);
                }
                else
                {
                    this.OtherTransEmoji.gameObject.SetActive(false);
                }
            }

        }

        public void _ShowUserInfo()
        {
            UserProfileManager.Instance.ShowUserDataShortUI(this.SendChatData.UserID);
        }
    }
}