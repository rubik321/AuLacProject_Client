using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTFunctions_old;
using TMPro;

namespace GOA.WorldMap
{
    public class CompassUI : LoadBehaviour
    {
        public RectTransform rtRotate;
        // protected override void FixedUpdate()
        // {
        //     base.FixedUpdate();
        //     this.RotateCompass();
        // }
        public void RotateCompass(){
            // this.rtRotate.rotation = Quaternion.Euler(0,0,CameraManager.instance.perCam.eulerAngles.y);
        }
    }
}
