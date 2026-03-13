using System.Collections;
using System.Collections.Generic;
using NTPackage.Functions;
using Rubik.CardPlayer;
using Rubik.Myrk.GeoPoint;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Rubik.Myrk.Monster
{
    public class MonsterOnMapModel : NTBehaviour
    {
        private MonsterOnMapData MonsterOnMapData;
        public MonsterSelection MonsterSelection;

        public bool isDestroy = false;

        public Transform Skin;
        public TextMeshProUGUI TextLevel;
        public TextMeshProUGUI TextLevelElite;

        public Transform Elite;
        public Transform Monster;

        public void SetData(MonsterOnMapData monsterOnMapData)
        {
            this.MonsterOnMapData = monsterOnMapData;
            if (this.MonsterOnMapData.coordinates == null)
            {
                this.MonsterOnMapData.coordinates = GeoPointManager.Instance.GetRandomRange(GeoPointManager.Instance.GetCurrentLocation(), MonsterConfig.Visibility);
            }
            this.TextLevel.text = "Lv. " + (this.MonsterOnMapData.Lv + 1);
            this.TextLevelElite.text = "Lv. " + (this.MonsterOnMapData.Lv + 1);
            if (this.MonsterOnMapData.AttackType == AttackType.Shard)
            {
                this.Elite.gameObject.SetActive(true);
                this.Monster.gameObject.SetActive(false);
            }
            else
            {
                this.Elite.gameObject.SetActive(false);
                this.Monster.gameObject.SetActive(true);
            }
            this.UpdateSkin();
            this.UpdateData();
        }

        //Function
        [NTButton]
        public void UpdateData()
        {
            if (this.isDestroy) return;
            NTLog.LogMessage("MonsterOnMapModel UpdateData");
            this.UpdateDis();
            this.UpdatePos();
        }

        public double Dis;
        protected void UpdateDis()
        {
            
            double dis = this.MonsterOnMapData.coordinates.DistanceFromOtherGPSCoordinate(GeoPointManager.Instance.GetCurrentLocation()) * 1000000d;
            if (dis > MonsterConfig.Visibility)
            {
                this.Dis = -1;
                this.MonsterOnMapData.coordinates = GeoPointManager.Instance.GetRandomRange(GeoPointManager.Instance.GetCurrentLocation(), MonsterConfig.Visibility);
            }
            if (dis != this.Dis)
            {
                this.Dis = dis;
                // Update some thing related distance
            }
        }

        protected void UpdatePos()
        {
            Vector3 pos = this.MonsterOnMapData.coordinates.convertCoordinateToVector(0);
            if (transform.position != pos)
            {
                transform.position = pos;
            }
        }

        protected void UpdateSkin()
        {
            if (this.isDestroy) return;
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.Skin);
            Transform ske = CardPlayerManager.Instance.InstantiatePlayerAvatar(this.MonsterOnMapData.Index);
            ske.SetParent(this.Skin);
            NTFunction.ResetPosition(ske);
        }

        public MonsterOnMapData GetMonsterOnMapData()
        {
            return this.MonsterOnMapData;
        }

        [NTButton]
        public void Test()
        {
            double dis = this.MonsterOnMapData.coordinates.DistanceFromOtherGPSCoordinate(GeoPointManager.Instance.GetCurrentLocation()) * 1000000d;
            NTLog.LogMessage("Test : " + dis);
        }
    }
}