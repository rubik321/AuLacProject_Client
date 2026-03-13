using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NTPackage.Functions;
using UnityEngine;

namespace NTPackage.Functions.UIAnimation
{
    public class FadeUp : NTBehaviour
    {
        public CanvasGroup CanvasGroup;
        public float Duration = 0.8f;
        public bool DoneAnimation = true;

        public Transform StartPoint;
        public Transform EndPoint;

        [NTButton]
        public void DoAnim()
        {
            this.DoneAnimation = false;
            transform.position = this.StartPoint.position; 
            // Start "invisible"
            this.CanvasGroup.alpha = 0;

            transform.DOKill();

            // visible
            this.CanvasGroup.DOFade(1, this.Duration*0.75f);

            transform.DOMove(this.EndPoint.position, this.Duration).OnComplete(()=>{
                this.DoneAnimation = true;
            });
        }
    }
}

