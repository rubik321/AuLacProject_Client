using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NTFunctions_old{
    public class BtnTab : BaseButton
    {
        public int number = 0;
        public Transform transSelected;
        public Transform transUnselect;

        public MultiTabUI multiTab;

        public override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadTransSelected();
            this.LoadTransUnselect();
            this.MultiTabUI();
        }

        protected void LoadTransSelected(){
            if(transSelected != null) return;
            this.transSelected = transform.Find("Selected");
        }
        protected void LoadTransUnselect(){
            if(transUnselect != null) return;
            this.transUnselect = transform.Find("Unselect");
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

        protected override void OnClick(){
            base.OnClick();
            if(this.multiTab == null) return;
            this.multiTab.BtnTabOnclick(this.number);
        }

        [ContextMenu("On")]
        public virtual void OnUI(){
            this.transSelected.gameObject.SetActive(true);
            this.transUnselect.gameObject.SetActive(false);
        }

        [ContextMenu("Off")]
        public virtual void OffUI(){
            this.transSelected.gameObject.SetActive(false);
            this.transUnselect.gameObject.SetActive(true);
        }
    }
}