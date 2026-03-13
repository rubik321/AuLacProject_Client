using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NTFunctions_old
{
    public class MaxWidth : LoadBehaviour {
    
        public RectTransform textTransform;
        public LayoutElement layoutElementMin;
        public LayoutElement layoutElementMax;

        protected override void OnEnable()
        {
            base.OnEnable();
            layoutElementMax.enabled = false;
        }

        protected override void FixedUpdate(){
            checkWidth();
        }
    
        public void checkWidth(){
            if(textTransform.rect.width < layoutElementMax.preferredWidth){
                layoutElementMax.enabled = false;
            }
            else{
                layoutElementMax.enabled = true;
            }

        }
    
    }
}
