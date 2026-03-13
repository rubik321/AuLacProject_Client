using System.Collections;
using System.Collections.Generic;
using GOA.Item;
using GOA.LandEvent;
using GOA.Portal;
using GOA.ShopBuilding;
using GOA.UserData;
using GOA.WorldMap;
using GoMap;
using GoShared;
using NTFunctions_old;
using NTPackage_old.Functions;
using Rubik.UI;
using SimpleJSON;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GOA.Building
{

    [System.Serializable]
    public class DataItemConsume
    {
        public string Index;
        public ItemCode Code;
        public int Amount;
    }
    [System.Serializable]
    public class BuildConsume
    {
        public List<DataItemConsume> DataItemConsumes = new List<DataItemConsume>();
        public float Gin = 0;
        public float Coin = 0;
    }

    public class BuildingManager : LoadBehaviour
    {
        public BuildConsume BuildPlayerShopConsume;
        public TextAsset DataBuildingInfo;

        public NTDictionary<BuildingInfo> BuildingInfoDic;

        public Transform PlayerBuildingHolder;
        public PlayerBuilding PlayerBuilding;

        public List<Building> Buildings;

        public BuildBuilding Building;

        public bool IsBuilding = false;

        public const float LimitDistance = 100;

        public static BuildingManager instance;
        protected override void Awake()
        {
            base.Awake();
            if (BuildingManager.instance != null)
            {
                Debug.LogWarning("Only 1 instance allow");
                return;
            }
            BuildingManager.instance = this;
        }

        [Button]
        public override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadBuildBuilding();
            this.LoadDataBuildingInfo();
            this.LoadPlayerBuildingHolder();
        }

        protected void LoadBuildBuilding()
        {
            if (this.Building != null) return;
            this.Building = transform.Find("Building").GetComponent<BuildBuilding>();
        }

        protected void LoadDataBuildingInfo()
        {
            if (this.DataBuildingInfo != null) return;
            this.DataBuildingInfo = Resources.Load<TextAsset>("DataBuildingInfo");
        }
        protected void LoadPlayerBuildingHolder()
        {
            if (this.PlayerBuildingHolder != null) return;
            this.PlayerBuildingHolder = transform.Find("PlayerBuildingHolder");
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            this.Buildings.Clear();
            this.Building.gameObject.SetActive(false);
            this.BuildingInfoDic.Clear();
            JSONNode jdata = JSONNode.Parse(this.DataBuildingInfo.text);
            foreach (JSONNode item in jdata)
            {
                BuildingInfo buildingInfo = JsonUtility.FromJson<BuildingInfo>(item.ToString());
                this.BuildingInfoDic.Add(buildingInfo.BuildingType.ToString(), buildingInfo);
            }
        }

        public void UpdateData()
        {
            if (!this.IsBuilding) return;
        }

        public void ResetData()
        {
            this.Buildings.Clear();
            ObjectPoolingManager.instance.PushChildObjectIntoPooling(this.PlayerBuildingHolder);
        }

        public void Build(bool IsBuild)
        {
            if (!IsBuild)
            {
                this.OffBuild();
            }
            else
            {
                this.IsBuilding = true;
                this.Building.gameObject.SetActive(true);
                this.Building.transform.position = GameMaster.instance.player.transform.position;
                for (int i = this.Buildings.Count - 1; i >= 0; i--)
                {
                    if (this.Buildings[i] == null)
                    {
                        this.Buildings.RemoveAt(i);
                        continue;
                    }
                    Buildings[i].OnBuildingArea();
                }
                this.Building.UpdateData();
            }
        }
        [ContextMenu("ConfirmBuild")]
        public void ConfirmBuild(GeoType buildingType)
        {
            if (!this.Building.Available.gameObject.activeSelf) return;
            Coordinates coordinates = Coordinates.convertVectorToCoordinates(GameMaster.instance.player.transform.position);
            APIManager.Instance.BuildBuildingPlayer(coordinates.longitude, coordinates.latitude, buildingType, (data) =>
            {
                JSONNode jdata = JSONNode.Parse(data);
                Portal.GeoPointData geoPointData = new Portal.GeoPointData();
                geoPointData.longitude = jdata["Data"]["Location"]["coordinates"][0];
                geoPointData.latitude = jdata["Data"]["Location"]["coordinates"][1];
                geoPointData.Type = jdata["Data"]["Type"];
                geoPointData.PointID = jdata["Data"]["PointID"];
                geoPointData._id = jdata["Data"]["_id"];
                geoPointData.Opened = jdata["Data"]["Opened"];
                geoPointData.Country = jdata["Data"]["Country"];
                geoPointData.State = jdata["Data"]["State"];
                geoPointData.City = jdata["Data"]["City"];
                geoPointData.BuidingType = jdata["Data"]["BuidingType"];
                geoPointData.Ownwer = jdata["Data"]["Ownwer"];
                geoPointData.OwnerName = jdata["Data"]["OwnerName"];
                foreach (MatRequire item in BuildingManager.instance.GetBuildingInfoByType(buildingType).MatRequire)
                {
                    UserData.UserData.Instance.Inventory.AddInventoryByCode(item.Code, -item.Amount);
                }
                switch (buildingType)
                {
                    case GeoType.ShopPlayer:
                        UserData.UserData.Instance.data.AmountShopBuilding++;
                        break;
                    case GeoType.Blacksmith:
                        UserData.UserData.Instance.data.AmountBlacksmithBuilding++;
                        break;
                    case GeoType.Dungeon:
                        UserData.UserData.Instance.data.AmountDungeonBuilding++;
                        break;
                    case GeoType.Castle:
                        UserData.UserData.Instance.data.AmountCastleBuilding++;
                        break;
                }
                BuildingManager.instance.InitBuilding(geoPointData);
                this.OffBuild();
            });
        }

        [ContextMenu("Building")]
        public void Build(GeoType geoType)
        {
            this.Building.OnSkin(geoType);
        }

        [ContextMenu("OffBuilding")]
        public void OffBuild()
        {
            this.IsBuilding = false;
            this.Building.gameObject.SetActive(false);
            for (int i = this.Buildings.Count - 1; i >= 0; i--)
            {
                if (this.Buildings[i] == null)
                {
                    this.Buildings.RemoveAt(i);
                    continue;
                }
                Buildings[i].OffBuildingArea();
            }
            this.Building.OffSkin();
        }

        public void AddBuilding(Building building)
        {
            this.Buildings.Add(building);
        }

        public BuildingInfo GetBuildingInfoByType(GeoType buildingType)
        {
            return this.BuildingInfoDic.Get(buildingType.ToString());
        }

        [Button]
        public Sprite GetBuildingIcon(GeoType buildingType)
        {
            BuildingInfo buildingInfo = this.GetBuildingInfoByType(buildingType);
            if (buildingInfo == null) return null;
            if (buildingInfo.Image == null)
            {
                buildingInfo.Image = Resources.Load<Sprite>(buildingInfo.ImagePath);
                return buildingInfo.Image;
            }
            else return buildingInfo.Image;
        }

        [Button]
        public BuildingSkin GetBuildingPrefab(GeoType buildingType)
        {
            BuildingInfo buildingInfo = this.GetBuildingInfoByType(buildingType);
            if (buildingInfo == null) return null;
            if (buildingInfo.Prefab == null)
            {
                buildingInfo.Prefab = Resources.Load<BuildingSkin>(buildingInfo.PrefabPath);
                return buildingInfo.Prefab;
            }
            else return buildingInfo.Prefab;
        }

        public void InitBuilding(Portal.GeoPointData geoPointData)
        {
            PlayerBuilding playerBuilding = null;
            try
            {
                playerBuilding = ObjectPoolingManager.instance.GetObjectFromPooling<PlayerBuilding>("PlayerBuilding" + geoPointData.BuidingType);
                if (playerBuilding == null) throw null;
            }
            catch (System.Exception)
            {
                playerBuilding = Instantiate<PlayerBuilding>(this.PlayerBuilding);
            }
            this.Buildings.Add(playerBuilding);
            playerBuilding.SetParent(this.PlayerBuildingHolder);
            playerBuilding.SetActive(true);
            playerBuilding.Init(geoPointData);
        }

        public bool CanBuild(GeoType type)
        {
            if(type == GeoType.ShopPlayer || type == GeoType.Blacksmith) return true;
            // if (!GeoPointManager.Instance.IsOwnLand)
            // {
            //     HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("available_on_your_land", "Available on your land."));
            //     return false;
            // }

            // if (GeoPointManager.Instance.CdTele.DistanceFromPoint(GameMaster.instance.locationManager.currentLocation) > Config.Configs.RangeBuildBuilding)
            // {
            //     HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("too_far_from_your_land", "Too far from your land."));
            //     return false;
            // }
            return true;
        }
    }
}