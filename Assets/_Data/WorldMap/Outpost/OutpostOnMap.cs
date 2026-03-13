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
    using Rubik.Myrk.Outpost;

    public class OutpostOnMap : SelectorObject
    {
        public long TileX;
        public long TileY;

        public Transform Skin;
        public Transform SkinOccupied;

        protected override void OnDestroy()
        {
            base.OnDestroy();
            EventListenerManager.instance.RemoveListener(EventCode.OutpostWorldMapManager_UpdateData, OutpostWorldMapManager.Instance.GetKeyOccupied(this.TileX, this.TileY));
        }

        public void SetData(long tileX, long tileY){
            this.TileX = tileX;
            this.TileY = tileY;
            this.UpdateData();
            EventListenerManager.instance.Register(EventCode.OutpostWorldMapManager_UpdateData, OutpostWorldMapManager.Instance.GetKeyOccupied(this.TileX, this.TileY), (data)=>{
                this.UpdateData();
            });
        }

        [NTButton]
        public void UpdateData(){
            if(OutpostWorldMapManager.Instance.IsOccupied(this.TileX, this.TileY)){
                this.Skin.gameObject.SetActive(false);
                this.SkinOccupied.gameObject.SetActive(true);
            }else{
                this.Skin.gameObject.SetActive(true);
                this.SkinOccupied.gameObject.SetActive(false);
            }
        }

        public override void OnClick(){
            if(OutpostWorldMapManager.Instance.IsOccupied(this.TileX, this.TileY)){
                return;
            }
            PopupManager.Instance.OnUI(PopupCode.OutpostOnMapUI, null, (popup) => {
                OutpostOnMapUI outpostOnMapUI = popup as OutpostOnMapUI;
                outpostOnMapUI.SetData(this.TileX, this.TileY);
            });
        }
    }
}