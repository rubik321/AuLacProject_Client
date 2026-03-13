using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTFunctions_old;

namespace GOA.UIMenu{
    public class MenuUI : PopupUI
    {
        public MultiTabUI multiTabUI;
        public override void LoadComponents()
        {
            base.LoadComponents();
            this.lvUI = new PopupLv().GetValue(transform.name);
        }

        public void OnUI(){
            if(!this.CanShow()) return;
            this.Show();
        }
        public void OnTab(int number){
            this.OnUI();
            this.multiTabUI.BtnTabOnclick(1);
        }
    }
}
