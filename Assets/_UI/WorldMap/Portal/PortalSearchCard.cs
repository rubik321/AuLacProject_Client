using NTPackage.Functions;
using UnityEngine;

namespace Rubik.Myrk.Portal
{
    using System;
    using Rubik.CardPlayer;
    using Rubik.Myrk.GeoPoint;
    using Rubik.UI.Statitic;
    using TMPro;

    public class PortalSearchCard : NTBehaviour
    {
        public GeoPoint GeoPoint;

        public CardPlayerIndex CardPlayerIndex;
        public CardPlayerData CardPlayerData;
        public PortalAttackData PortalAttackData;
        public Transform SkinHolder;

        public StarUI StarUI;
        public Transform StarUnknow;

        public bool IsClose = false;
        public bool IsEnd = false;

        public TextMeshProUGUI TextLocation;

        public void SetData(GeoPoint geoPoint){
            this.GeoPoint = geoPoint;
            this.UpdateData();
        }

        public void UpdateData(){
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.SkinHolder.transform);
            this.CardPlayerIndex = PortalWorldMapManager.Instance.GetBossCardPlayerIndex(this.GeoPoint.PointID);
            this.CardPlayerData = CardPlayerManager.Instance.GetCardPlayerDataByIndex(this.CardPlayerIndex);
            this.PortalAttackData = PortalWorldMapManager.Instance.GetPortalAttackData(this.GeoPoint.PointID);
            RectTransform skeletonGraphic = CardPlayerManager.Instance.InstantiatePlayerSkeletonGraphic(this.CardPlayerIndex);
            skeletonGraphic.SetParent(this.SkinHolder.transform);
            NTFunction.ResetPosition(skeletonGraphic);
            this.TextLocation.text = this.GeoPoint.GetLongitude().ToString("F3") + "~" + this.GeoPoint.GetLatitude().ToString("F3");

            long timeReset = PortalWorldMapManager.Instance.GetTimeResetPortal(this.PortalAttackData.OpenTime);
            if (timeReset > 0)
            {
                this.IsClose = false;
                if (PortalWorldMapManager.Instance.GetRemainingTimeAttackPortal(this.PortalAttackData.OpenTime) > 0)
                {
                    this.IsEnd = false;
                }
                else
                {
                    this.IsEnd = true;
                }
            }
            else
            {
                this.IsEnd = true;
                this.IsClose = true;
            }

            if (this.IsClose)
            {
                // this.BtnOnTabRank.gameObject.SetActive(false);
                // this.BtnOnTabReward.gameObject.SetActive(false);
                // this.HP = PortalWorldMapManager.Instance.GetPortalHp(this.Level);
                // this.TextHp.text = NTFunction.FormatNumber(this.HP);
            }
            else
            {
                // this.BtnOnTabRank.gameObject.SetActive(true);
                // this.BtnOnTabReward.gameObject.SetActive(true);
                // this.HP = this.PortalAttackData.HP;
                // if (this.HP <= 0) this.HP = 0;
                // this.TextHp.text = NTFunction.FormatNumber(this.HP);
            }
            // this.TextAtk.text = NTFunction.FormatNumber(PortalWorldMapManager.Instance.GetPortalAttack(this.Level));

            // this.StarUI.SetStar(this.Level);
        }
    }
}