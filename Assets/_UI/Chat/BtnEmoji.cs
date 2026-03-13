using System.Collections;
using System.Collections.Generic;
using NTPackage.Functions;
using NTPackage.UI;
using Rubik.UserProfile;
using UnityEngine;
using UnityEngine.UI;

namespace Rubik._2DGPS.Chat
{
    public class BtnEmoji : NTButtonEffect
    {
        public int Emoji;
        public Image ImgEmoji;
        public GameObject EmojiHolder;
        public Emoji_Popup EmojiPopup;

        public void SetData(int emoji, Emoji_Popup emojiPopup)
        {
            this.Emoji = emoji;
            this.EmojiPopup = emojiPopup;
            this.UpdateData();
        }

        public void UpdateData()
        {
            this.ImgEmoji.sprite = UserProfileManager.Instance.GetEmojiSpriteByIndex(this.Emoji);
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.EmojiHolder.transform);
            Transform go = ObjectPoolingManager.Instance.PullObjectFromPooling(ObjectPoolingConfig.EmojiPrefab + this.Emoji);
            if (go == null) go = Instantiate(UserProfileManager.Instance.GetEmojiPrefabByIndex(this.Emoji)).transform;
            go.name = ObjectPoolingConfig.EmojiPrefab + this.Emoji;
            go.SetParent(this.EmojiHolder.transform);
            NTFunction.ResetPosition(go);
        }

        public void SelectEmoji(){
            this.EmojiPopup.OnClickEmoji(this.Emoji);
        }
    }
}