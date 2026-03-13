using System.Collections;
using System.Collections.Generic;
using GOA.WorldMap;
using NTPackage_old.Functions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GOA.Building
{
    public class BuildBuilding : Building
    {
        public Transform Unavailable;
        public Transform Available;

        [Button]
        public override void LoadComponents()
        {
            base.LoadComponents();
        }

        public void OnSkin(GeoType geoType = GeoType.ShopPlayer){
            this.OffSkin();
            this.UpdateSkin(geoType);
        }

        public void OffSkin(){
            ObjectPoolingManager.instance.PushChildObjectIntoPooling(this.Skin);
        }

        public void UpdateData(){
            foreach(Building building in BuildingManager.instance.Buildings){
                float dis = Vector3.Distance(building.transform.position, transform.position);
                if(dis < BuildingManager.LimitDistance){
                    this.Unavailable.gameObject.SetActive(true);
                    this.Available.gameObject.SetActive(false);
                    return;
                }
            }
            this.Unavailable.gameObject.SetActive(false);
            this.Available.gameObject.SetActive(true);
        }
    }
}