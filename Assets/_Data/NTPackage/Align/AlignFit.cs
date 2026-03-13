using System;
using System.Collections;
using System.Collections.Generic;
using NTPackage.Functions;

using UnityEngine;

namespace NTPackage.Functions
{
    [ExecuteInEditMode]
    public class AlignFit : NTBehaviour
    {
        public AlignFit Master{
            get {
                    if(transform.parent == null) return null;
                    if(transform.parent.TryGetComponent<AlignFit>(out AlignFit alignFit)){
                        return alignFit;
                    }
                    return null;
                }
        }

        public Action FitEvent;

        protected override void OnEnable()
        {
            base.OnEnable();
            if(Master == null){
                return;
            }
            // this.Fit();
            this.FitSelf();
        }

        public virtual void Fit(){
            this.FitChilds();
            this.FitSelf();
        }

        [NTButton]
        public virtual void FitSelf(){
            //this.FitParent();
            FitEvent?.Invoke();
        }

        public virtual void FitChilds(){
            foreach (Transform item in transform)
            {
                if(item.TryGetComponent<AlignFit>(out AlignFit alignFit)){
                    alignFit.FitChilds();
                }
            }
            this.FitSelf();
        }

        public virtual void FitParent(){
            if(transform.parent.TryGetComponent<AlignFit>(out AlignFit alignFit)){
                alignFit.FitSelf();
            }
        }
    }
    
}
