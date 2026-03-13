using GOA.LandEvent;
using GOA.WorldMap;
using UnityEngine;

namespace GOA.Building
{
    [System.Serializable]
    public class BuildingInfo{
        public GeoType BuildingType;
        public MatRequire[] MatRequire;
        public string ImagePath;
        public Sprite Image;
        public string PrefabPath;
        public BuildingSkin Prefab;
    }
}