using System.Collections;
using System.Collections.Generic;
using GOA.Building;
using GOA.Portal;
using GOA.WorldMap;
using NTFunctions_old;
using NTPackage_old.Functions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GOA.ShopBuilding
{
    public class ShopBuildingManager : LoadBehaviour
    {
        // public List<ShopBuildingData> PlayerShopDatas = new List<ShopBuildingData>();
        public List<ShopBuilding> CollectionShopPlayers = new List<ShopBuilding>();
        public Transform Holder;

        public static ShopBuildingManager instance;
        protected override void Awake()
        {
            base.Awake();
            if (ShopBuildingManager.instance != null){
               Debug.LogWarning("Only 1 instance allow");
               return;
             }
            ShopBuildingManager.instance = this;
        }

        public override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadShopPlayerCollection();
            this.LoadHolder();
        }

        protected void LoadShopPlayerCollection(){
            this.CollectionShopPlayers.Clear();
            foreach (Transform item in transform.Find("Collections"))
            {
                if(item.TryGetComponent<ShopBuilding>(out ShopBuilding shopPlayer)){
                    this.CollectionShopPlayers.Add(shopPlayer);
                }
            }
        }

        protected void LoadHolder(){
            if(this.Holder != null) return;
            this.Holder = transform.Find("Holder");
        }

        protected override void Start()
        {
            base.Start();
        }

        public void UpdateData(){
            this.UpdateNeutralShop();
            this.UpdateWanderingDealer();
        }

        public void UpdateNeutralShop(){
            return;
            foreach (Transform item in WorldMapMaster.instance.GOMap.transform)
            {
                if(item.TryGetComponent<GoMap.GOTile>(out GoMap.GOTile tile)){
                    Transform trans = item.transform.Find("NeutralShop");
                    if(trans != null) continue;
                    double latitude = tile.goTile.tileCoordinates.x;
                    double longitude = tile.goTile.tileCoordinates.y;
                    if(latitude %2 == 0 && longitude %2 ==0){
                        ShopBuilding shop = Instantiate(this.GetCollectionShopPlayerByCode(ShopBuildingCode.NeutralShop));
                        shop.transform.SetParent(item);
                        ShopBuildingData shopPlayerData = new ShopBuildingData();
                        shopPlayerData.Id = latitude+"&"+longitude;
                        shopPlayerData.Code = ShopBuildingCode.NeutralShop;
                        shopPlayerData.latitude = latitude;
                        shopPlayerData.longitude = longitude;
                        shopPlayerData.IsBuild = true;
                        shop.shopPlayerData = shopPlayerData;
                        shop.transform.name = "NeutralShop";
                        System.Random rand = new System.Random((int)latitude);
                        int x = rand.Next(370)+100;
                        rand = new System.Random((int)longitude);
                        int z = rand.Next(370)+100;
                        shop.transform.localPosition = new Vector3(x,0,z);
                        BuildingManager.instance.AddBuilding(shop);
                    }
                }
            }
        }

        public void UpdateWanderingDealer(){
            return;
            foreach (Transform item in WorldMapMaster.instance.GOMap.transform)
            {
                if(item.TryGetComponent<GoMap.GOTile>(out GoMap.GOTile tile)){
                    Transform trans = item.transform.Find("WanderingDealer");
                    if(trans != null) continue;
                    double latitude = tile.goTile.tileCoordinates.x;
                    double longitude = tile.goTile.tileCoordinates.y;
                    if(latitude %3 == 0 && longitude %3 ==0){
                        ShopBuilding wanderingDealer = Instantiate(this.GetCollectionShopPlayerByCode(ShopBuildingCode.WanderingDealer));
                        wanderingDealer.transform.SetParent(item);
                        ShopBuildingData shopPlayerData = new ShopBuildingData();
                        shopPlayerData.Id = latitude+"&"+longitude;
                        shopPlayerData.Code = ShopBuildingCode.WanderingDealer;
                        shopPlayerData.latitude = latitude;
                        shopPlayerData.longitude = longitude;
                        shopPlayerData.IsBuild = true;
                        wanderingDealer.shopPlayerData = shopPlayerData;
                        wanderingDealer.transform.name = "WanderingDealer";
                        System.Random rand = new System.Random((int)latitude+1);
                        int x = rand.Next(370)+100;
                        rand = new System.Random((int)longitude+1);
                        int z = rand.Next(370)+100;
                        wanderingDealer.transform.localPosition = new Vector3(x,0,z);
                        BuildingManager.instance.AddBuilding(wanderingDealer);
                    }
                }
            }
        }

        public ShopBuilding GetCollectionShopPlayerByCode(ShopBuildingCode code){
            return this.CollectionShopPlayers.Find((element) =>{return element.Code == code;});
        }
    }
}