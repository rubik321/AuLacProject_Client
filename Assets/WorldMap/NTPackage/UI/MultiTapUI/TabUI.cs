using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NTFunctions_old{
    public class TabUI : LoadBehaviour
    {
        public MultiTabUI multiTab;
        public int number = 0;

        public override void LoadComponents()
        {
            base.LoadComponents();
        }

        public virtual void OnUI(){
            if(this.isActiveUI()) return;
            gameObject.SetActive(true);
            this.UpdateData();
        }

        public bool isActiveUI(){
            return gameObject.activeSelf;
        }
        
        [ContextMenu("OffUI")]
        public virtual void OffUI(){
            gameObject.SetActive(false);
        }

        public virtual void ResetData(){
        
        }

        public virtual void UpdateData(){
        
        }

        public void ChoseTab(){
            if(this.multiTab == null) return;
            this.multiTab.BtnTabOnclick(this.number);
        }
    }
}
