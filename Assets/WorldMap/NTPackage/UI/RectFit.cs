using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTFunctions_old;


namespace Rubik.Chat
{
    public class RectFit : LoadBehaviour
    {
        public RectTransform baseOnTrans;
        public RectTransform mainTrans;

        public float minHeight = 50;
        public float minWidth = 50;

        public override void LoadComponents()
        {
            base.LoadComponents();
            this.mainTrans = transform.GetComponent<RectTransform>();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            this.ChangeSize();
        }

        [ContextMenu("ChangeSize")]
        public void ChangeSize(){
            float height;
            float width;
            if(minHeight > 0){
                height = Mathf.Max(baseOnTrans.sizeDelta.y, minHeight);
            }else{
                height = mainTrans.sizeDelta.y;
            }
            if(minWidth > 0){
                width = Mathf.Max(baseOnTrans.sizeDelta.x, minWidth);
            }else{
                width = mainTrans.sizeDelta.x;
            }
            Vector2 sizeDelta = new Vector2(width, height);
            mainTrans.sizeDelta = sizeDelta;
        }
    }
}