using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NTPackage.Functions{
    public class HeightFit : AlignFit
    {
        public RectTransform mainTrans;
        public List<RectTransform> baseOnTrans;

        public float minHeight = -1;
        public float maxHeight = -1;

        public float MarginHeight = 0;

        public float oldHeight = 0;

        protected override void Update()
        {
            base.Update();
            if(baseOnTrans.Count == 0) return;
            if(mainTrans == null) return;
            this.FitSelf();
            // if(this.oldHeight != baseOnTrans.sizeDelta.y){
            //     this.FitSelf();
            // }
        }

        public override void FitSelf()
        {
            if(baseOnTrans == null) return;
            if(mainTrans == null) return;
            float height = 0;
            foreach (RectTransform item in this.baseOnTrans)
            {
                if(!item.gameObject.activeSelf) continue;
                if(height < item.sizeDelta.y) height = item.sizeDelta.y * item.localScale.y;
            }
            height+= MarginHeight;
            if(this.minHeight > -1 && height < this.minHeight) height = this.minHeight;
            if(this.maxHeight > -1 && height > this.maxHeight) height = this.maxHeight;
            mainTrans.sizeDelta = new Vector2(mainTrans.sizeDelta.x, height);
            this.oldHeight = height;
            base.FitSelf();
        }
    }

}
