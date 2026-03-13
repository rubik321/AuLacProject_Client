using System.Collections;
using System.Collections.Generic;
using GOA.WorldMap;
using GoMap;
using NTPackage.Functions;
using Rubik.Banner;
using Rubik.ItemPlayer;
using Rubik.Myrk.GeoPoint;
using Rubik.UserDataPlayer;
using UnityEngine;
using UnityEngine.UI;
namespace Rubik.Myrk.Controller
{
    public class WorldMapController : NTBehaviour
    {
        public GOMap GOMap;
        public static WorldMapController Instance;
        protected override void Awake()
        {
            base.Awake();
            if (WorldMapController.Instance != null)
            {
                NTLog.LogError("Only 1 Instance allow");
                return;
            }
            WorldMapController.Instance = this;
            this.GOMap.locationManager = GeoPointManager.Instance.LocationManager;
        }

        protected override void Start()
        {
            base.Start();
            StartCoroutine(NTFunction.WaitSecond(1f, () =>
            {
                if (GeoPointManager.Instance.IsTeleport)
                {
                    TeleportSystem.instance.Teleport(GeoPointManager.Instance.TeleportLocation);
                }
                else
                {
                    // TeleportSystem.instance.ComeBack();
                }
            }));
            ItemDataManager.Instance.CheckCapSlotInventoryBag();
        }
    }
}
