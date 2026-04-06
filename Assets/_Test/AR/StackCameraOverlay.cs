using System.Collections.Generic;
using NTPackage.Functions;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Rubik.AR
{
    public class StackCameraOverlay : MonoBehaviour
    {
        public Camera Cam;
        void Start()
        {
            
            if(Cam == null)
            {
                Cam = GetComponent<Camera>();
            }
            if(Cam == null){
                NTLog.LogError("Cam is not set");
                return;
            }

            UniversalAdditionalCameraData camBaseData = this.Cam.GetUniversalAdditionalCameraData();
            camBaseData.cameraStack.Clear();

            Camera[] overlayCam = FindObjectsByType<Camera>(FindObjectsSortMode.None);
            foreach(Camera cam in overlayCam)
            {
                UniversalAdditionalCameraData camOverlayData = cam.GetUniversalAdditionalCameraData();
                if(camOverlayData.renderType == CameraRenderType.Overlay){
                    camBaseData.cameraStack.Add(cam);
                }
            }
        }
    }
}
