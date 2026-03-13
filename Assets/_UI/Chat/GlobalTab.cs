using System.Collections;
using System.Collections.Generic;
using NTPackage.EventDispatcher;
using NTPackage.UI;
using UnityEngine;

namespace Rubik._2DGPS.Chat
{
    public class GlobalTab : TabUI
    {
        public bool IsInit = false;
        public ChatChannelUI ChatChannelUI;

        public override void OnUI()
        {
            if(this.IsInit) return;
            this.IsInit = true;
            base.OnUI();
            this.SetData();
        }

        public override void OffUI()
        {
            base.OffUI();
            this.IsInit = false;
        }

        public override void SetData(object data = null)
        {
            base.SetData(data);
            this.ChatChannelUI.SetData(ChatService.Instance.GetServerChatChannel());
            this.ChatChannelUI.gameObject.SetActive(true);
        }
    }
}