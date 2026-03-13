using System.Collections;
using System.Collections.Generic;
using GoMap;
using NTPackage.Functions;
using Rubik.Myrk.Controller;
using Rubik.Myrk.GeoPoint;
using Rubik.Myrk.Monster;
using UnityEngine;

namespace Rubik.Myrk.TileMap
{
    public class TileMapController : NTBehaviour
    {
        public TileMap TileMapPrefab;

        public List<TileMap> TileMapList;

        public Coroutine CorUpdateData;

        public static TileMapController Instance;
        protected override void Awake()
        {
            base.Awake();
            if (TileMapController.Instance != null)
            {
                NTLog.LogError("Only 1 Instance allow");
                return;
            }
            TileMapController.Instance = this;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if(this.CorUpdateData != null) StopCoroutine(this.CorUpdateData);
        }

        protected override void Start()
        {
            base.Start();
            if(this.CorUpdateData != null) StopCoroutine(this.CorUpdateData);
            this.CorUpdateData = StartCoroutine(this.IEUpdateData());
        }

        public IEnumerator IEUpdateData()
        {
            while(true){
                yield return new WaitForSeconds(1);
                NTLog.LogMessage("TileMapController UpdateData");
                this.UpdateData();
            }
        }

        [NTButton]
        public void UpdateData()
        {
            this.TileMapList = new List<TileMap>();
            List<TileData> tileDatas = new List<TileData>();
            foreach (Transform child in WorldMapController.Instance.GOMap.transform)
            {
                Transform tile = child.Find("Tile");
                if (tile != null) continue;
                GOMapboxTile goMapboxTile = child.GetComponent<GOMapboxTile>();
                if (goMapboxTile == null) continue;
                TileMap tileMap = Instantiate(this.TileMapPrefab, child);
                tileMap.name = "Tile";
                tileMap.gameObject.SetActive(true);
                tileMap.TileX = (long)goMapboxTile.goTile.tileCoordinates.x;
                tileMap.TileY = (long)goMapboxTile.goTile.tileCoordinates.y;
                NTFunction.ResetPosition(tileMap.transform);
                this.TileMapList.Add(tileMap);
                TileGeoPoint tileGeoPoint = GeoPointManager.Instance.GetTileGeoPoint(tileMap.TileX, tileMap.TileY);
                if (tileGeoPoint != null)
                {
                    tileMap.SetData(tileGeoPoint);
                }
                else
                {
                    tileDatas.Add(new TileData(tileMap.TileX, tileMap.TileY));
                }
            }

            if (tileDatas.Count > 0)
            {
                StartCoroutine(GeoPointManager.Instance.IEGetGeoPoint(tileDatas, () =>
                {
                    foreach (TileMap tileMap in this.TileMapList)
                    {
                        if(tileMap.IsLoad) continue;
                        TileGeoPoint tileGeoPoint = GeoPointManager.Instance.GetTileGeoPoint(tileMap.TileX, tileMap.TileY);
                        tileMap.SetData(tileGeoPoint);
                    }
                }));

            }
            
            MonsterManager.Instance.UpdateData();
        }
    }
}
