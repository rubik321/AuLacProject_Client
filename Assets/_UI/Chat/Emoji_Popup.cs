using System;
using System.Collections;
using System.Collections.Generic;
using NTPackage.Functions;
using NTPackage.UI;
using Rubik.UserProfile;
using UnityEngine;

namespace Rubik._2DGPS.Chat
{
    public class Emoji_Popup : PopupUI
    {
        public List<BtnEmoji> ListBtnEmoji;
        public Transform Holder;
        public BtnEmoji BtnEmojiPrefab;
        public int EmojiSelected;
        public RectTransform Broad;
        public Action<int> ActionSend;

        public override void LoadComponents()
        {
            base.LoadComponents();
            this.popupCode = PopupCode.Emoji_Popup;
        }

        public override void OnUI(object data = null, bool isDefaultSound = true)
        {
            base.OnUI(data, isDefaultSound);
            this.EmojiSelected = 0;
            this.UpdateData();
        }


        public float x;
        public float y;
        public void SetPositionClick(Vector2 position){
            // Broad is square, set position of broad so corner of broad is position
            Vector2 size = Broad.sizeDelta;
            x = size.x / 2;
            if(position.x > 0) x = -x;
            y = size.y / 2;
            if(position.y > 0) y = -y;
            Broad.anchoredPosition = position + new Vector2(x,y);
        }

        public override void UpdateData(object data = null)
        {
            base.UpdateData(data);
            List<int> listEmojiAvailable = UserProfileManager.Instance.GetListEmojiAvailable();
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.Holder);

            foreach (int emoji in listEmojiAvailable)
            {
                BtnEmoji btnEmoji = ObjectPoolingManager.Instance.PullObjectFromPooling<BtnEmoji>(ObjectPoolingConfig.BtnEmoji);
                if (btnEmoji == null) btnEmoji = Instantiate(this.BtnEmojiPrefab).GetComponent<BtnEmoji>();
                btnEmoji.transform.SetParent(this.Holder);
                NTFunction.ResetPosition(btnEmoji.transform);
                btnEmoji.SetData(emoji, this);
                btnEmoji.gameObject.name = ObjectPoolingConfig.BtnEmoji;
                btnEmoji.gameObject.SetActive(true);
                this.ListBtnEmoji.Add(btnEmoji);
            }
            this.UpdateData();
        }

        public void UpdateData()
        {
            foreach (BtnEmoji btnEmoji in this.ListBtnEmoji)
            {
                btnEmoji.UpdateData();
                if (btnEmoji.Emoji == this.EmojiSelected) btnEmoji.Chose();
                else btnEmoji.Unchose();
            }
        }

        public void OnClickEmoji(int emoji)
        {
            this.EmojiSelected = emoji;
            // this.UpdateData();
            this.SendEmoji();
        }

        public void SendEmoji()
        {
            this.ActionSend?.Invoke(this.EmojiSelected);
            this.OffUI();
        }
    }
}
