using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.Combat
{
    public class StageObject : MonoBehaviour
    {
        public PositionHolder[] enemyHolders;
        public PositionHolder[] allyHolders;
        public Transform focusCamPos;
        public Transform strategicCamPos;

        private void OnEnable()
        {
            CameraFollow[] cams = FindObjectsOfType<CameraFollow>();
            foreach (CameraFollow cam in cams)
            {
                switch (cam.cameraType)
                {
                    case CameraFollow.CameraType.Focus:
                        cam.SetFollowTransform(focusCamPos);
                        break;
                    case CameraFollow.CameraType.Strategic:
                        cam.SetFollowTransform(strategicCamPos);
                        break;
                }
            }
        }

        [System.Serializable]
        public class PositionHolder
        {
            public Transform characterHolder;
            public Transform companionHolder;
        }
    }
}
