using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using NTFunctions_old;

namespace WorldMap.Header{
    public class ResUI : LoadBehaviour
    {
        public TextMeshProUGUI textNumberRes;

        public override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadTextNumberRes();
        }

        protected void LoadTextNumberRes(){
            if(this.textNumberRes != null) return;
            this.textNumberRes = transform.Find("TextNumber(TMP)").GetComponent<TextMeshProUGUI>();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            this.UpdateData();
        }

        public virtual void UpdateData(){
            this.UpdateResNumber();
        }

        protected virtual void UpdateResNumber(){

        }
    }
}
