using System.Collections;
using System.Collections.Generic;
using NTPackage.Functions;

using UnityEngine;

namespace NTPackage.UI{
    public class TabUI : NTBehaviour
    {
        public MultiTabUI multiTab;
        public int number = 0;

        public override void LoadComponents()
        {
            base.LoadComponents();
        }

        public virtual void SetData(object data = null){
            
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

        [NTButton]
        public virtual void UpdateData(){
        
        }
    }
}
