using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NTPackage.Functions;
using UnityEngine;

namespace NTPackage.Functions.UIAnimation
{
    public class Rubber : ItemAnimation
    {
        public override void DoAnim(float delay = 0)
        {
            base.DoAnim(delay);

            this.Target.localScale = Vector3.zero;

            this.Target.DOScale(new Vector3(0.8f,1.2f,1), this.Duration/2).OnComplete(() =>
            {
                this.Target.DOScale(new Vector3(1.2f,0.8f,1), this.Duration/4).OnComplete(()=>{
                    this.Target.DOScale(new Vector3(1f,1f,1), this.Duration/4);
                });
                this.DoneAnimation = true;
            }).SetDelay(delay);
        }


    }
}