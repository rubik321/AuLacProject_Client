using NTPackage;
using NTPackage.Functions;
using UnityEngine;

namespace Rubik.Myrk.Portal
{
    using Rubik.Myrk.GeoPoint;
    public class OutpostOnMapController : NTBehaviour
    {
        public OutpostOnMap OutpostOnMapPrefab;

        public static OutpostOnMapController Instance;
        protected override void Awake()
        {
            base.Awake();
            if (PortalOnMapController.Instance != null)
            {
                NTLog.LogError("Only 1 Instance allow");
                return;
            }
            OutpostOnMapController.Instance = this;
        }

        #region Function
        public OutpostOnMap GenerateOutpostOnMap(long tileX, long tileY)
        {
            OutpostOnMap outpostOnMap = Instantiate(this.OutpostOnMapPrefab);
            outpostOnMap.SetData(tileX, tileY);
            outpostOnMap.name = "OutpostOnMap";
            outpostOnMap.gameObject.SetActive(true);
            return outpostOnMap;
        }
        #endregion
    }
}