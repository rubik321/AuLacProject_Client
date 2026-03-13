using System.Collections.Generic;
using UnityEngine;

namespace Rubik.Myrk.TileMap
{
    using NTPackage.Functions;
    using Rubik.Myrk.GeoPoint;
    using Rubik.Myrk.Portal;

    public class TileMap : MonoBehaviour
    {
        public long TileX;
        public long TileY;
        public TileGeoPoint TileGeoPoint;
        public bool IsLoad = false;

        public void SetData(TileGeoPoint tileGeoPoint){
            this.TileGeoPoint = tileGeoPoint;
            this.IsLoad = true;
            this.UpdateData();
        }

        public void UpdateData(){
            foreach (GeoPoint geoPoint in this.TileGeoPoint.GeoPoint)
            {
                PortalOnMap portalOnMap = PortalOnMapController.Instance.GeneratePortalOnMap(geoPoint);
                portalOnMap.transform.SetParent(this.transform);
                portalOnMap.UpdateData();
            }

            OutpostOnMap outpostOnMap = OutpostOnMapController.Instance.GenerateOutpostOnMap(this.TileX, this.TileY);
            outpostOnMap.transform.SetParent(this.transform);
            outpostOnMap.UpdateData();
            NTFunction.ResetPosition(outpostOnMap.transform);
        }
    }
}