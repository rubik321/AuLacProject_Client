using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTFunctions_old;

namespace GOA.UIFriends{
    public class FriendUI : PopupUI
    {
        public override void LoadComponents()
        {
            base.LoadComponents();
            this.lvUI = new PopupLv().GetValue(transform.name);
        }

        public void OnUI(){
            this.Show();
        }

        public FriendTabUI FriendTabUI;
    }
}
