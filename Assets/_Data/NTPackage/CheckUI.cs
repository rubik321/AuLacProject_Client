using System.Collections;
using System.Collections.Generic;
using NTPackage.Functions;
using UnityEngine;

namespace NTPackage.Functions
{
    public class CheckUI : NTBehaviour
    {
        public Transform transTrue;
        public Transform transFalse;

        public override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadTransTrue();
            this.LoadTransFalse();
        }

        protected void LoadTransTrue(){
            if(transTrue != null) return;
            this.transTrue = transform.Find("True");
        }
        protected void LoadTransFalse(){
            if(transFalse != null) return;
            this.transFalse = transform.Find("False");
        }

        public void SetState(bool state){
            if(state){
                this.transTrue.gameObject.SetActive(true);
                this.transFalse.gameObject.SetActive(false);
            }else{
                this.transTrue.gameObject.SetActive(false);
                this.transFalse.gameObject.SetActive(true);
            }
        }
    }
    
}
