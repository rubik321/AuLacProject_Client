using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NTPackage.Functions;
using UnityEngine;

namespace NTPackage.Functions.UIAnimation
{
    public class ItemAnimation : NTBehaviour
    {
        public float Duration = 0.6f;
        public bool RunEnable = false;

        protected override void OnEnable()
        {
            base.OnEnable();
            if(RunEnable) this.DoAnim();
        }

        public Transform Target;
        public bool DoneAnimation = true;
        public virtual void DoAnim(float delay = 0){
            this.DoneAnimation = false;
            if (this.Target == null) this.Target = transform;
            this.Target.DOKill();
        }
    }

}