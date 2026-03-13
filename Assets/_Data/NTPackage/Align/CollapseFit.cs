using System.Collections;
using System.Collections.Generic;
using NTPackage.Functions;
using UnityEngine;

namespace NTPackage.Functions{
    public class CollapseFit : NTBehaviour
    {
        public bool isCollapse = true;
        public RectTransform mainTrans;
        public float minHeight = -1;
        public float maxHeight = -1;

        public RectTransform minRect;
        public RectTransform maxRect;

        public CheckUI checkUI;

        protected override void OnEnable()
        {
            base.OnEnable();
            this.Collapse();
        }

        public void Onclick(){
            if(this.isCollapse) this.UnCollapse();
            else Collapse();
        }

        [ContextMenu("Collapse")]
        public void Collapse(){
            this.isCollapse = true;
            float height = mainTrans.sizeDelta.y;
            if(minHeight > 0){
                height = minHeight;
            }
            if(minRect != null){
                height = minRect.sizeDelta.y;
            }
            mainTrans.sizeDelta = new Vector2(mainTrans.sizeDelta.x, height);
            if(checkUI == null) return;
            this.checkUI.SetState(this.isCollapse);
        }

        [ContextMenu("UnCollapse")]
        public void UnCollapse(){
            this.isCollapse = false;
            float height = mainTrans.sizeDelta.y;
            if(maxHeight > 0){
                height = maxHeight;
            }
            if(maxRect != null){
                height = maxRect.sizeDelta.y;
            }
            mainTrans.sizeDelta = new Vector2(mainTrans.sizeDelta.x, height);
            if(checkUI == null) return;
            this.checkUI.SetState(this.isCollapse);
        }
    }
}
