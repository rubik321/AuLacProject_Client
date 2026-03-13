using UnityEngine;
using GOA.WorldMap;

namespace Rubik.Myrk.Portal
{
    using System.Collections;
    using GoShared;
    using NTPackage.EventDispatcher;
    using NTPackage.Functions;
    using NTPackage.UI;
    using Rubik.DataType;
    using Rubik.Myrk.GeoPoint;
    using TMPro;

    public class PortalOnMap : NTBehaviour
    {
        public GeoPoint GeoPoint;

        public Transform Skin;
        public SpriteRenderer Gate;
        public OriginType OriginType;
        public Coordinates Coordinates;

        public Coroutine CorUpdateData;

        public PortalAttackData PortalAttackData;
        public TextMeshProUGUI TextTitle;
        public long PointID;

        protected override void OnDestroy()
        {
            if (this.CorUpdateData != null)
            {
                this.StopCoroutine(this.CorUpdateData);
            }
            EventListenerManager.instance.RemoveListener(EventCode.UpdatePortalAttackData, "PortalOnMapUI");
        }

        public void SetData(GeoPoint geoPoint)
        {
            this.GeoPoint = geoPoint;
            this.PointID = geoPoint.PointID;
            if (this.CorUpdateData != null)
            {
                this.StopCoroutine(this.CorUpdateData);
            }
            this.CorUpdateData = this.StartCoroutine(this.IEUpdateData());
            EventListenerManager.instance.Register(EventCode.UpdatePortalAttackData, "PortalOnMapUI", (data) =>
            {
                this.OnUpdatePortalAttackData((long)data);
            });
            this.TextTitle.text = Lean.Localization.LeanLocalization.GetTranslationText("portal_title", "Portal") + " #" + this.PointID;;
            this.UpdateData();
        }

        [NTButton]
        public void UpdateData()
        {
            this.PortalAttackData = PortalWorldMapManager.Instance.GetPortalAttackData(this.GeoPoint.PointID);
            this.OriginType = PortalWorldMapManager.Instance.GetOriginType(this.GeoPoint.PointID);
            this.UpdateSkin();
            this.UpdatePos();
        }

        protected void UpdatePos()
        {
            this.Coordinates = new Coordinates(this.GeoPoint.GetLatitude(), this.GeoPoint.GetLongitude());
            Vector3 pos = this.Coordinates.convertCoordinateToVector(0);
            transform.position = pos;
        }

        protected void UpdateSkin()
        {
            this.Gate.sprite = PortalWorldMapManager.Instance.GetGateSprite(this.OriginType);
        }

        public IEnumerator IEUpdateData()
        {
            while (true)
            {
                yield return new WaitForSeconds(3f);
                PortalWorldMapManager.Instance.SendMsgGetPortalAttackData(this.GeoPoint.PointID);
            }
        }

        public void OnUpdatePortalAttackData(long pointID)
        {
            if (pointID != this.PointID) return;
            this.PortalAttackData = PortalWorldMapManager.Instance.GetPortalAttackData(this.PointID);
            this.UpdateData();
        }

        public void ShowUI()
        {
            if (this.PortalAttackData == null)
            {
                PortalWorldMapManager.Instance.GetPortalAttackAPI(this.GeoPoint.PointID, (data) =>
                {
                    this.PortalAttackData = data;
                    this.UpdateData();
                    PopupManager.Instance.OnUI(PopupCode.PortalOnMapUI, this.PointID, (popup) =>
                    {
                        PortalOnMapUI portalOnMapUI = popup as PortalOnMapUI;
                        portalOnMapUI.SetLocation(this.GetLocation());
                    });
                });
            }
            else
            {
                PopupManager.Instance.OnUI(PopupCode.PortalOnMapUI, this.PointID, (popup) =>
                {
                    PortalOnMapUI portalOnMapUI = popup as PortalOnMapUI;
                    portalOnMapUI.SetLocation(this.GetLocation());
                });
            }
        }

        public string GetLocation()
        {
            string local = "";

            string city = this.GeoPoint.City != null && this.GeoPoint.City.Length > 0 ? this.GeoPoint.City : "";
            string state = this.GeoPoint.State != null && this.GeoPoint.State.Length > 0 ? this.GeoPoint.State : "";
            string country = this.GeoPoint.Country != null && this.GeoPoint.Country.Length > 0 ? this.GeoPoint.Country : "";
            // if(city.Length > 0){
            //     local += city;
            // }
            // if(state.Length > 0){
            //     if(local.Length > 0){
            //         local += " - ";
            //     }
            //     local += state;
            // }
            if(country.Length > 0){
                if(local.Length > 0){
                    local += " - ";
                }
                local += country;
            }
            if(local.Length == 0) local = Lean.Localization.LeanLocalization.GetTranslationText("portal_unknow_local", "Myrk");
            return local;
        }
    }
}