using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NTFunctions_old
{
    [ExecuteInEditMode]
    public class AlignFit : LoadBehaviour
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

        protected override void OnEnable()
        {
            base.OnEnable();
            if(Master == null){
                return;
            }
            this.Fit();
        }

        public virtual void Fit(){
            this.FitChilds();
            this.FitSelf();
        }

        public virtual void FitSelf(){
            //this.FitParent();
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
