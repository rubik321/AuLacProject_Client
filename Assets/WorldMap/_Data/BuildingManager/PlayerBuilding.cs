using System.Collections;
using GOA.Portal;
using GOA.ShopBuilding;
using GOA.WorldMap;
using GoShared;
using NTFunctions_old;
using Rubik.GOA.Blacksmith;
using TMPro;
using UnityEngine;

namespace GOA.Building
{
    public class PlayerBuilding : Building{

        public GeoType BuildingType;
        public GeoPointData GeoPointData;

        public TextMeshProUGUI Title;

        public void Init(GeoPointData geoPointData){
            this.GeoPointData = geoPointData;
            this.BuildingType = (GeoType) geoPointData.BuidingType;
            string buildingName = "Bulding";
            switch (this.BuildingType)
            {
                case GeoType.ShopPlayer:
                    buildingName = Lean.Localization.LeanLocalization.GetTranslationText("title_shop", buildingName);
                    break;
                case GeoType.Blacksmith:
                    buildingName = Lean.Localization.LeanLocalization.GetTranslationText("title_blacksmith", buildingName);
                    break;
                case GeoType.Dungeon:
                    buildingName = Lean.Localization.LeanLocalization.GetTranslationText("title_dungeon", buildingName);
                    break;
                case GeoType.Castle:
                    buildingName = Lean.Localization.LeanLocalization.GetTranslationText("title_castle", buildingName);
                    break;
            }
            this.Title.text = geoPointData.OwnerName+"'s "+ buildingName;
            this.PointMap.Icon.sprite = BuildingManager.instance.GetBuildingIcon(this.BuildingType);
            this.UpdateData();
            this.UpdateSkin(this.BuildingType);
        }

        public virtual void UpdateData(){
            Coordinates coordinates = new Coordinates(this.GeoPointData.latitude, this.GeoPointData.longitude);
            this.ResetScale();
            Vector3 pos = coordinates.convertCoordinateToVector();
            pos.y = 0;
            transform.position = pos;
        }

        public void Chose()
        {
            this.Holding = true;
            StartCoroutine(this.CountHolding());
        }

        IEnumerator CountHolding()
        {
            yield return new WaitForSeconds(TimeDelay);
            this.Holding = false;
        }

        void OnMouseUp()
        {
            if (!this.Holding) return;
            if (this.BuildingType == GeoType.ShopPlayer)
            {
                PlayerShopUI playerShopUI = (PlayerShopUI) UIManager.instance.GetPopupUIByCode(PopupCode.PlayerShopUI);
                if(playerShopUI == null) return;
                playerShopUI.OnUI(GeoPointData._id);
            }
            if (this.BuildingType == GeoType.Blacksmith)
            {
                BlacksmithUI blacksmithUI = (BlacksmithUI) UIManager.instance.GetPopupUIByCode(PopupCode.BlacksmithUI);
                if(blacksmithUI == null) return;
                blacksmithUI.OnUI();
            }

            if (this.BuildingType == GeoType.Dungeon)
            {
                UIManager.instance.OnBtnDungeon_Onclick();
            }
        }
    }
}