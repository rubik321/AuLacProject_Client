using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoShared;
using GoMap;
using LocationManagerEnums;
using Rubik.Combat;
using Rubik.Chat;
using Sirenix.OdinInspector;
using GOA.UIWorldMap;
using GOA.Building;
using Rubik.Myrk.GeoPoint;
using NTPackage.Functions;
using Rubik.Myrk.Controller;

namespace GOA.WorldMap
{
    public class TeleportSystem : NTBehaviour
    {
        public static TeleportSystem instance;
        protected override void Awake()
        {
            if (TeleportSystem.instance != null)
            {
                Debug.LogWarning("Only 1 instance allow");
                return;
            }
            TeleportSystem.instance = this;
        }

        [NTButton]    
        public void TestTeleport(){
            this.Teleport(new Coordinates(10.65716 + UnityEngine.Random.Range(-0.002f, 0.002f), 105.28459 + UnityEngine.Random.Range(-0.002f, 0.002f)));
        }

        public void Teleport(Coordinates coordinates)
        {
            if(!GeoPointManager.Instance.IsTeleport){
                GeoPointManager.Instance.GPSLocation = GeoPointManager.Instance.LocationManager.currentLocation;
            }
            GeoPointManager.Instance.TeleportLocation = coordinates;
            PanelMainToolUI.instance.BtnGlobalPortal.gameObject.SetActive(false);
            PanelMainToolUI.instance.BtnBackLocal.gameObject.SetActive(true);
            GeoPointManager.Instance.IsTeleport = true;
            GeoPointManager.Instance.LocationManager.useLocationServices = false;
            GeoPointManager.Instance.LocationManager.motionMode = MotionMode.Avatar;
            WorldMapController.Instance.GOMap.BuildMapPortionInsideEditor(coordinates, coordinates);
        }

        public void ComeBack()
        {
            PanelMainToolUI.instance.BtnBackLocal.gameObject.SetActive(false);
            PanelMainToolUI.instance.BtnGlobalPortal.gameObject.SetActive(true);
            GeoPointManager.Instance.IsTeleport = false;
            GeoPointManager.Instance.TeleportLocation = GeoPointManager.Instance.GPSLocation;
            GeoPointManager.Instance.LocationManager.useLocationServices = true;
            GeoPointManager.Instance.LocationManager.motionMode = MotionMode.GPS;
            WorldMapController.Instance.GOMap.BuildMapPortionInsideEditor(GeoPointManager.Instance.GPSLocation, GeoPointManager.Instance.GPSLocation);
        }
    }

}
