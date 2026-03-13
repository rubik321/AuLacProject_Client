using System.Collections;
using System.Collections.Generic;
using NTPackage.Functions;

using UnityEngine;
using UnityEngine.UI;

namespace NTPackage.Functions
{
    public class MaxWidth : NTBehaviour {
        
        public float _MinWidth;
        public float _MaxWidth;

        public float _OldWidth = 0;

        public RectTransform TargetTransform;
        public LayoutElement layoutElement;

        protected override void OnEnable()
        {
            base.OnEnable();
            this.layoutElement.minWidth = this._MinWidth;
            this.layoutElement.preferredWidth = -1f;
        }

        protected override void Update(){
            base.Update();
            checkWidth();
        }

        [NTButton]
        public void checkWidth(){
            if(this._OldWidth == TargetTransform.rect.width) return;
            this.layoutElement.minWidth = this._MinWidth;
            if(TargetTransform.rect.width < this._MaxWidth){
                layoutElement.preferredWidth = -1f;
            }
            else{
                layoutElement.preferredWidth = this._MaxWidth;
            }
            this._OldWidth = this.TargetTransform.rect.width;
        }
    
    }
}
