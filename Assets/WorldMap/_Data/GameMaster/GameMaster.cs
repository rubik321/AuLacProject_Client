using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoShared;
using GoMap;
using LocationManagerEnums;
using GOA.WorldMap;
using NTFunctions_old;
using NTPackage_old.EventDispatcher;
using Rubik.Myrk.GeoPoint;

namespace GOA.WorldMap
{
    public class GameMaster : LoadBehaviour
    {
        public LocationManager locationManager;
        public GOMap goMap;
        public Player player;
        public Coordinates oldPosPlayer;
        public ViewModeCode viewModeCode = ViewModeCode.unknown; 

        public static GameMaster instance;
        protected override void Awake()
        {
            base.Awake();
            if (GameMaster.instance != null) Debug.LogError("Only 1 GameMaster allow");
            GameMaster.instance = this;
            this.locationManager = Rubik.Myrk.GeoPoint.GeoPointManager.Instance.LocationManager;
        }

        public void ChangeViewMode(){
            if(this.viewModeCode == ViewModeCode.per){
                this.viewModeCode = ViewModeCode.god;
                this.ChangeGod();
                EventListenerManager.instance.PostEvent(EventCode.ChangeRegionView);
            }else{
                this.viewModeCode = ViewModeCode.per;
                StartCoroutine(this.ChangePer());
                EventListenerManager.instance.PostEvent(EventCode.ChangeNormalView);
            }
        }

        IEnumerator ChangePer(){
            CameraManager.instance.godCam.transform.position = new Vector3(this.player.transform.position.x, CameraManager.instance.godCam.transform.position.y, this.player.transform.position.z);
            yield return new WaitForSeconds(0.2f);
            this.locationManager.currentLocation = this.oldPosPlayer;
            this.locationManager.motionMode = MotionMode.GPS;
            this.player.gameObject.SetActive(true);
            this.locationManager.avatar = this.player.gameObject;
            CameraManager.instance.ChangePerCam();
        }

        public void ChangeGod(){
            this.oldPosPlayer = this.locationManager.currentLocation;
            this.locationManager.motionMode = MotionMode.Avatar;
            // this.player.gameObject.SetActive(false);
            this.locationManager.avatar = CameraManager.instance.godCam.gameObject;
            CameraManager.instance.godCam.transform.position = new Vector3(this.player.transform.position.x, CameraManager.instance.godCam.transform.position.y, this.player.transform.position.z);
            CameraManager.instance.ChangeGodCam();
        }
    }
}
