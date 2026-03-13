using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NTFunctions_old{
    public class WidthFit : AlignFit
    {
        public RectTransform mainTrans;
        public List<RectTransform> baseOnTrans;

        public float minWidth = -1;
        public float maxWidth = -1;

        float oldWidth = 0;

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            if(baseOnTrans.Count == 0) return;
            if(mainTrans == null) return;
            this.FitSelf();
            // if(this.oldWidth != baseOnTrans.sizeDelta.y){
            //     this.FitSelf();
            // }
        }

        public override void FitSelf()
        {
            if(baseOnTrans == null) return;
            if(mainTrans == null) return;
            float width = 0;
            foreach (RectTransform item in this.baseOnTrans)
            {
                if(width < item.sizeDelta.x) width = item.sizeDelta.x;
            }
            if(this.minWidth > -1 && width < this.minWidth) width = this.minWidth;
            if(this.maxWidth > -1 && width > this.maxWidth) width = this.maxWidth;
            mainTrans.sizeDelta = new Vector2(width, mainTrans.sizeDelta.y);
            this.oldWidth = width;
            base.FitSelf();
        }
    }

}
