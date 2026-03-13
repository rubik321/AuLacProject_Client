using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTFunctions_old;
using TMPro;
using UnityEngine.UI;

namespace Rubik.Chat
{
    public class BoxChatPlayer : LoadBehaviour
    {
        public TextMeshProUGUI textSenderName;
        public TextMeshProUGUI textContent;
        public Image avatar;

        public override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadTextContent();
            this.LoadTextSenderName();
            this.LoadAvatar();
            ContentSizeFitter sizeFitter = GetComponent<ContentSizeFitter>();
        }

        protected void LoadTextContent(){
            if(textContent != null) return;
            this.textContent = transform.Find("TextContent(TMP)").GetComponent<TextMeshProUGUI>();
        }
        protected void LoadTextSenderName(){
            if(textSenderName != null) return;
            this.textSenderName = transform.Find("SenderName(TMP)").GetComponent<TextMeshProUGUI>();
        }
        protected void LoadAvatar(){
            if(avatar != null) return;
            this.avatar = transform.Find("AvatarBG").Find("Avatar").GetComponent<Image>();
        }

    }
}
