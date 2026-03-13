using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTFunctions_old;

namespace Rubik.Chat{
    public class GlobalUI : TabUI
    {
        public ChannelChatUI globalChannelChatUI;

        public override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadGlobalChannelChatUI();
        }

        protected void LoadGlobalChannelChatUI(){
            if(globalChannelChatUI != null) return;
            this.globalChannelChatUI = transform.Find("GlobalChannelChatUI").GetComponent<ChannelChatUI>();
        }

        public override void OnUI()
        {
            base.OnUI();
            // this.globalChannelChatUI.OnUI();
        }

        public override void UpdateData()
        {
            base.UpdateData();
            this.globalChannelChatUI.UpdateData();
        }
    }
}
