using System.Collections;
using System.Collections.Generic;
using GoShared;
using NTPackage.Functions;
using UnityEngine;

namespace Rubik.Myrk.GeoPoint
{
    [System.Serializable]
    public class TileData
    {
        public long TileX;
        public long TileY;

        public TileData(long tileX, long tileY){
            this.TileX = tileX;
            this.TileY = tileY;
        }
    }

    [System.Serializable]
    public class Location
    {
        public double[] coordinates; // [longitude, latitude]
    }

    [System.Serializable]
    public class GeoPoint
    {
        public string _id;
        public int PointID;
        public string Country;
        public string State;
        public string City;
        public Location Location;
        public long TileX;
        public long TileY;

        public double GetLongitude()
        {
            return Location.coordinates[0];
        }

        public double GetLatitude()
        {
            return Location.coordinates[1];
        }
    }

    [System.Serializable]
    public class TileGeoPoint
    {
        public long TileX;
        public long TileY;
        public List<GeoPoint> GeoPoint = new List<GeoPoint>();

        public TileGeoPoint(long tileX, long tileY){
            this.TileX = tileX;
            this.TileY = tileY;
            this.GeoPoint = new List<GeoPoint>();
        }
    }
}