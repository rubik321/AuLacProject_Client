using System.Collections;
using System.Collections.Generic;
using GOA.WorldMap;
using NTFunctions_old;
using NTPackage_old.Functions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace GOA.Building
{

    public class Building : LoadBehaviour
    {
        public Transform BuildingArea;
        public Transform Skin;
        public PointMap PointMap;

        public const float TimeDelay = 0.2f;
        public bool Holding = false;

        [Button]
        public override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadBuilding();
            this.LoadSkin();
            this.LoadPointMap();
        }

        protected void LoadSkin(){
            if(this.Skin != null) return;
            this.Skin = transform.Find("Skin");
        }

        protected void LoadBuilding(){
            if(this.BuildingArea != null) return;
            this.BuildingArea = transform.Find("BuildingArea");
        }

        protected void LoadPointMap(){
            if(this.PointMap != null) return;
            try
            {
                this.PointMap = this.FindByPath("PointMap").GetComponent<PointMap>();
            }
            catch (System.Exception){}
        }

        public void OnBuildingArea(){
            this.BuildingArea.gameObject.SetActive(true);
        }
        public void OffBuildingArea(){
            this.BuildingArea.gameObject.SetActive(false);
        }

        public virtual void UpdateSkin(GeoType geoType){
            ObjectPoolingManager.instance.PushChildObjectIntoPooling(this.Skin);
            try
            {
                BuildingSkin buildingSkin = ObjectPoolingManager.instance.GetObjectFromPooling<BuildingSkin>("Building"+geoType.ToString());
                if(buildingSkin == null) throw null;
            }
            catch (System.Exception)
            {
                BuildingSkin buildingSkin = Instantiate(BuildingManager.instance.GetBuildingPrefab(geoType));
                if(buildingSkin == null) return;
                buildingSkin.SetName("Building"+geoType.ToString());
                buildingSkin.SetParent(this.Skin);
                buildingSkin.ResetPosition();
                buildingSkin.ResetScale();
            }
        }
    }
}