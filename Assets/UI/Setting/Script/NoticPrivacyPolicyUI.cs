using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTFunctions_old;

namespace GOA.UIMenu{
    public class NoticPrivacyPolicyUI : PopupUI
    {
        public override void LoadComponents()
        {
            base.LoadComponents();
            this.lvUI = new PopupLv().GetValue(transform.name);
        }
        
        public void OnUI(){
            if(!this.CanShow()) return;
            this.Show();
        }
    }
}
