using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTFunctions_old;
using GoShared;
using GOA.WorldMap.Outpost;
using SimpleJSON;
using Rubik.UI;
using Rubik.Combat;

namespace GOA.WorldMap
{
    using System;
    using System.Linq;
    using GOA.Building;
    using GOA.Item;
    using GOA.LandEvent;
    using GOA.ShopBuilding;
    using GOA.WorldMap.PortalGlobal;
    using NTPackage_old.Functions;
    using Portal;
    using Rubik.Chat;
    using Rubik.Myrk.GeoPoint;

    [System.Serializable]
    public class RewardData
    {
        public string Type;
        public string Id;
        public int Coin = 0;
        public int Gin = 0;
        public int RarityGear = -1;
        public int RarityCompanion = -1;
        public int RarityOrb = -1;
        public RewardItem[] RewardItems;
        public int MonsterSkilled = 0;
    }
    [System.Serializable]
    public class RewardItem
    {
        public string Type;
        public int Amount;
    }

    public class WorldMapMaster : LoadBehaviour
    {
        public bool IsInit = false;

        public Coordinates CoordinatesPortal;
        // public Portal GlobalPortalModel;
        public Transform BtnTeleport;

        public List<Outpost.Outpost> Outposts;

        public Transform TransReward;
        public List<RewardData> RewardDatas;

        public static WorldMapMaster instance;

        public const int MaxPeople = 20;

        protected override void Awake()
        {
            base.Awake();
            if (WorldMapMaster.instance != null) Debug.LogError("Only 1 WorldMapMaster allow");
            WorldMapMaster.instance = this;
        }

        protected override void Start()
        {
            try
            {
                int verOfMap = PlayerPrefs.GetInt("VersionOfMap");
                if (verOfMap < 1) throw null;
            }
            catch (System.Exception)
            {
                this.GOMap.ClearCache();
                PlayerPrefs.SetInt("VersionOfMap", 1);
            }
            GameMaster.instance.goMap.BuildMapPortionInsideEditor(GeoPointManager.Instance.GetCurrentLocation(), GeoPointManager.Instance.GetCurrentLocation());
            if (GeoPointManager.Instance.IsTeleport)
            {
                this.IsInit = true;
                StartCoroutine(this.Init(0.5f, true));
            }
            else
            {
                GameMaster.instance.locationManager.currentLocation = GeoPointManager.Instance.GetCurrentLocation();
                GameMaster.instance.locationManager.worldOrigin = GeoPointManager.Instance.GetCurrentLocation();
                if (LocationManager.status == LocationServiceStatus.Running)
                {
                    this.IsInit = true;
                    StartCoroutine(this.Init(0.5f));
                }
                else
                {
                    this.IsInit = true;
                    StartCoroutine(this.Init(2));
                }
            }

        }

        public List<Portal> portals;
        public Portal portalSample;

        public Transform PortalHolder;
        IEnumerator Init(float time, bool isTeleport = false)
        {
            HUDCanvas.Instance.ShowLoadingPanel();
            yield return new WaitForSeconds(time);
            if (isTeleport)
            {
                if (LandEventManager.instance.CheckEventIsEnd())
                {
                    TeleportSystem.instance.ComeBack();
                }
                else
                    TeleportSystem.instance.Teleport(GeoPointManager.Instance.TeleportLocation);
            }
            GameMaster.instance.goMap.BuildMapPortionInsideEditor(GameMaster.instance.locationManager.currentLocation, GameMaster.instance.locationManager.worldOrigin);
            PortalGlobalManager.instance.Init();
            this.UpdateData();
            HUDCanvas.Instance.HideLoadingPanel();
        }

        public void UpdateData()
        {
            PortalGlobalManager.instance.UpdateData();
            BuildingManager.instance.UpdateData();
            BuildingManager.instance.ResetData();
            this.DeleteAllPlayer();
            this.UpdateOutpost();
            MobManager.instance.Init();
            ShopBuildingManager.instance.UpdateData();
            this.portals.Clear();
            foreach (Portal item in this.portals)
            {
                item.UpdateData();
            }
            StartCoroutine(this.UpdateGeoPoint());
        }

        public GoMap.GOMap GOMap;
        public Transform Outpost;
        public Outpost.Outpost OutpostSample;
        public void UpdateOutpost()
        {
            return;
            this.Outposts.Clear();
            foreach (Transform item in GOMap.transform)
            {
                if (item.TryGetComponent<GoMap.GOTile>(out GoMap.GOTile tile))
                {
                    Transform trans = item.transform.Find("Outpost");
                    if (trans == null)
                    {
                        Outpost.Outpost outpost = Instantiate<Outpost.Outpost>(OutpostSample);
                        outpost.transform.SetParent(item.transform);
                        outpost.SetData(tile.goTile.tileCoordinates);
                        outpost.name = "Outpost";
                        this.Outposts.Add(outpost);
                        NTFunction.ResetPosition(outpost.transform);
                        BuildingManager.instance.AddBuilding(outpost);
                    }
                    else
                    {
                        this.Outposts.Add(trans.GetComponent<Outpost.Outpost>());
                    }
                }
            }
        }

        public void UpdateStatusOutpost()
        {
            foreach (Outpost.Outpost item in this.Outposts)
            {
                item.UpdateData();
            }
        }

        public IEnumerator UpdateGeoPoint()
        {
            this.portals.Clear();
            NTFunction.ClearChild(this.PortalHolder);
            StartCoroutine(APIManager.Instance.GetPortalData((data) =>
            {
                JSONNode dataPosition = JSONNode.Parse(data);
                NTFunction.ClearChild(this.PortalHolder);
                JSONNode listPortal = dataPosition["Data"]["Portal"];
                int numberReward = 0;
                this.TransReward.gameObject.SetActive(false);
                try
                {
                    numberReward = dataPosition["Data"]["Reward"];
                    if (numberReward > 0) this.TransReward.gameObject.SetActive(true);
                }
                catch (System.Exception) { }
                // foreach (JSONNode item in dataPosition["Data"]["Portal"])
                // foreach (JSONNode item in listPortal)
                // {
                //     if (this.portals.Count > 10) break;
                //     try
                //     {
                //         GOA.Portal.GeoPointData geoPointData = new GOA.Portal.GeoPointData();
                //         geoPointData.longitude = item["Location"]["coordinates"][0];
                //         geoPointData.latitude = item["Location"]["coordinates"][1];
                //         geoPointData.Type = item["Type"];
                //         geoPointData.PointID = item["PointID"];
                //         geoPointData._id = item["_id"];
                //         geoPointData.Opened = item["Opened"];
                //         geoPointData.Country = item["Country"];
                //         geoPointData.State = item["State"];
                //         geoPointData.City = item["City"];
                //         geoPointData.BuidingType = item["BuidingType"];
                //         geoPointData.Ownwer = item["Ownwer"];
                //         geoPointData.OwnerName = item["OwnerName"];
                //         geoPointData.Index = MobManager.fixedIndexReaperPortal[geoPointData.PointID % MobManager.fixedIndexReaperPortal.Length];
                //         if (geoPointData.BuidingType > (int)GeoType.Portal)
                //         {
                //             // BuildingManager.instance.InitBuilding(geoPointData);
                //         }
                //         else
                //         {
                //             Portal portal = Instantiate(this.portalSample);
                //             this.portals.Add(portal);
                //             portal.transform.SetParent(this.PortalHolder);
                //             portal.SetData(geoPointData);
                //             portal.transform.forward = GameMaster.instance.player.transform.position - portal.transform.position;
                //             // this.CoordinatesPortal.longitude = portalData.longitude-0.001;
                //             // this.CoordinatesPortal.latitude = portalData.latitude-0.001;
                //             portal.gameObject.SetActive(true);
                //         }
                //     }
                //     catch (System.Exception) { }

                // }
            }));
            yield return null;
        }


        //Global Event
        public Dictionary<string, PlayerData> PlayerDataDictionary = new System.Collections.Generic.Dictionary<string, PlayerData>();
        public Dictionary<string, Transform> PlayerDictionary = new System.Collections.Generic.Dictionary<string, Transform>();
        public Transform CharacterHolder;
        public BaseCharacter CharacterSample;

        public void UpdatePlayer(List<string> datas)
        {
            foreach (string item in datas)
            {
                try
                {
                    PlayerData playerData = JsonUtility.FromJson<PlayerData>(item);
                    if (playerData.UserId.Equals(UserData.UserData.Instance.data.UserId)) continue;
                    PlayerDataDictionary[playerData.UserId] = playerData;
                }
                catch (System.Exception e)
                {
                    NTPackage_old.Functions.NTLog.LogError(e.ToString(), gameObject);
                }
            }
            int count = 0;
            foreach (KeyValuePair<string, PlayerData> item in PlayerDataDictionary)
            {
                if (item.Value.Join)
                {
                    count++;
                    if (count > MaxPeople) return;
                    DeletePlayer(item.Value);
                    GeneratePlayer(item.Value);
                }
            }
        }

        public void GetPlayerData(string data)
        {
            try
            {
                PlayerData playerData = JsonUtility.FromJson<PlayerData>(data);
                if (playerData.UserId.Equals(UserData.UserData.Instance.data.UserId)) return;
                PlayerDataDictionary[playerData.UserId] = playerData;
                if (playerData.Join)
                {
                    if (this.PlayerDataDictionary.Count > MaxPeople) return;
                    GeneratePlayer(playerData);
                }
                else
                {
                    this.PlayerDataDictionary.Remove(playerData.UserId);
                    DeletePlayer(playerData);
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public void GeneratePlayers()
        {
            foreach (KeyValuePair<string, GOA.WorldMap.PlayerData> item in PlayerDataDictionary)
            {
                if (item.Value.Join)
                {
                    GeneratePlayer(item.Value);
                }
            }
        }

        public void GeneratePlayer(GOA.WorldMap.PlayerData data)
        {
            BaseCharacter character = ObjectPoolingManager.instance.GetObjectFromPooling<BaseCharacter>("OtherPlayer");
            if (character == null) character = Instantiate(this.CharacterSample);
            character.transform.name = "OtherPlayer";
            character.transform.SetParent(CharacterHolder);
            PlayerDictionary[data.UserId] = character.transform;
            foreach (string gearCode in data.GearCodes)
            {
                character.LoadGear(gearCode);
            }
            character.SetAnimator(data.Class);
            Coordinates coordinates = new Coordinates(data.latitude, data.longitude);
            character.transform.position = coordinates.convertCoordinateToVector(0);
            character.gameObject.SetActive(true);
        }

        public void DeleteAllPlayer()
        {
            foreach (Transform item in this.PlayerDictionary.Values)
            {
                ObjectPoolingManager.instance.PushObjectIntoPooling(item);
            }
            this.PlayerDataDictionary.Clear();
            this.PlayerDictionary.Clear();
            // NTFunctions.NTFunction.ClearChild(this.CharacterHolder);
        }

        public void DeletePlayer(GOA.WorldMap.PlayerData data)
        {
            try
            {
                Transform transform = this.PlayerDictionary[data.UserId];
                ObjectPoolingManager.instance.PushObjectIntoPooling(transform);
                this.PlayerDictionary.Remove(data.UserId);
            }
            catch (System.Exception e)
            {
                NTPackage_old.Functions.NTLog.LogError(e.ToString(), gameObject);
            }
        }

        public void GetReward()
        {
            this.RewardDatas = new List<RewardData>();
            StartCoroutine(APIManager.Instance.GetRewards((callback) =>
            {
                if (callback == null)
                {
                    return;
                }
                JSONNode data = JSONNode.Parse(callback);
                foreach (JSONNode item in data["Data"])
                {
                    try
                    {
                        this.RewardDatas.Add(JsonUtility.FromJson<RewardData>(item.ToString()));
                    }
                    catch (System.Exception) { }
                }
                if (this.RewardDatas == null || this.RewardDatas.Count == 0) return;
                foreach (RewardData item in RewardDatas)
                {
                    UserData.UserData.Instance.data.Coin += item.Coin;
                    UserData.UserData.Instance.data.Gin += item.Gin;
                    foreach (RewardItem ele in item.RewardItems)
                    {
                        UserData.UserData.Instance.Inventory.AddInventoryByCode((ItemCode)Enum.Parse(typeof(ItemCode), ele.Type), ele.Amount);
                    }
                }
                PopupRewardUI popupRewardUI = (PopupRewardUI)UIManager.instance.GetPopupUIByCode(PopupCode.PopupRewardUI);
                if (popupRewardUI == null) return;
                popupRewardUI.OnUI(this.RewardDatas);
                // this.RewardDatas.Clear();
                APIManager.Instance.GetUserOrb();
                APIManager.Instance.GetUserGear();
                APIManager.Instance.GetUserCompanion();
            }));
        }



    }

}
