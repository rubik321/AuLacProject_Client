using NTPackage;
using NTPackage.Functions;
using UnityEngine;

namespace Rubik.Myrk.Portal
{
    using Rubik.Myrk.GeoPoint;
    public class PortalOnMapController : NTBehaviour
    {
        public NTDictionary<string, PortalOnMap> PortalOnMap;

        public PortalOnMap PortalOnMapPrefab;

        public static PortalOnMapController Instance;
        protected override void Awake()
        {
            base.Awake();
            if (PortalOnMapController.Instance != null)
            {
                NTLog.LogError("Only 1 Instance allow");
                return;
            }
            PortalOnMapController.Instance = this;
        }

        #region Function
        public PortalOnMap GeneratePortalOnMap(GeoPoint geoPoint)
        {
            PortalOnMap portalOnMap = Instantiate(this.PortalOnMapPrefab);
            portalOnMap.SetData(geoPoint);
            portalOnMap.name = "PortalOnMap";
            portalOnMap.gameObject.SetActive(true);
            return portalOnMap;
        }
        #endregion
    }
}