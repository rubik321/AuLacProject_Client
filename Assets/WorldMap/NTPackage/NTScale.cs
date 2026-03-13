using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace NTFunctions_old{
    public class NTScale : LoadBehaviour
    {
        public float speedScale = 1f;
        public Vector3 minScale = new Vector3(0,0,0);
        public Vector3 maxScale = new Vector3(0,0,0);

        protected override void OnEnable()
        {
            base.OnEnable();
            transform.localScale = this.minScale;
            this.Scale();
        }

        public void Scale(){
            transform.DOScale(maxScale, this.speedScale).SetLoops(-1,LoopType.Yoyo);

        }

        protected override void OnDisable()
        {
            base.OnDisable();
            transform.DOPause();
            transform.localScale = this.minScale;
        }
    }
}
