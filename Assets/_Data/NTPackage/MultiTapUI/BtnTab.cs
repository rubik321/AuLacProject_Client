using System.Collections;
using System.Collections.Generic;
using NTPackage.UI;
using UnityEngine;

namespace NTPackage.UI{
    public class BtnTab : NTButtonEffect
    {
        public int number = 0;

        public MultiTabUI multiTab;

        public override void LoadComponents()
        {
            base.LoadComponents();
            this.MultiTabUI();
        }

        protected void MultiTabUI(){
            if(this.multiTab != null) return;
            try
            {
                this.multiTab = transform.parent.parent.GetComponent<MultiTabUI>();
            }
            catch (System.Exception)
            {
                Debug.LogWarning("Can't MultiTabUI");
            } 
        }


        //Function

        public virtual void Click(){
            if(this.multiTab == null) return;
            this.multiTab.BtnTabOnclick(this.number);
        }
    }
}