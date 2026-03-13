using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOA.WorldMap;
using NTFunctions_old;

namespace GOA.WorldMap
{
    public class CameraManager : LoadBehaviour
    {
        public Transform perCam;
        public Transform godCam;
        //public GameObject uiCam;

        public static CameraManager instance;
        protected override void Awake()
        {
            base.Awake();
            if (CameraManager.instance != null) Debug.LogError("Only 1 CameraManager allow");
            CameraManager.instance = this;
        }

        public void ChangePerCam(){
            this.perCam.gameObject.SetActive(true);
            this.godCam.gameObject.SetActive(false);
            CharacterUIController.Instance.CameraUI.SetActive(false);
        }
        public void ChangeGodCam(){
            this.perCam.gameObject.SetActive(false);
            this.godCam.gameObject.SetActive(true);
            CharacterUIController.Instance.CameraUI.SetActive(false);
        }
    }
}
