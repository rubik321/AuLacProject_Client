using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace EasyARGeoSpatialAnchors
{

    public enum AnchorType
    {
        Geospatial = 0,
        Rooftop = 1,
        Terrain = 2,
    }

    [Serializable]
    public struct GeospatialAnchorHistory
    {
        public string SerializedTime;
        public double Latitude;
        public double Longitude;
        public double Altitude;
        public double Heading;
        public AnchorType AnchorType;
        public Quaternion EunRotation;


        public GeospatialAnchorHistory(DateTime time, double latitude, double longitude,
            double altitude, AnchorType anchorType, Quaternion eunRotation)
        {
            SerializedTime = time.ToString();
            Latitude = latitude;
            Longitude = longitude;
            Altitude = altitude;
            Heading = 0.0f;
            AnchorType = anchorType;
            EunRotation = eunRotation;
        }


        public GeospatialAnchorHistory(
            double latitude, double longitude, double altitude, AnchorType anchorType,
            Quaternion eunRotation) :
            this(DateTime.Now, latitude, longitude, altitude, anchorType,
            eunRotation)
        {
        }

        public DateTime CreatedTime => Convert.ToDateTime(SerializedTime);

        public override string ToString()
        {
            return JsonUtility.ToJson(this);
        }
    }

    [Serializable]
    public class GeospatialAnchorHistoryCollection
    {
        public List<GeospatialAnchorHistory> Collection = new List<GeospatialAnchorHistory>();
    }

}