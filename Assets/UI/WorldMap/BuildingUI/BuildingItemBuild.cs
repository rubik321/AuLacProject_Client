using System.Collections;
using System.Collections.Generic;
using Michsky.MUIP;
using NTFunctions_old;
using NTPackage_old.UI;
using UnityEngine;
using DG;
using DG.Tweening;
using GOA.WorldMap;

namespace GOA.Building
{
    public class BuildingItemBuild : NTCheckBox
    {
        public Transform BtnBuild;
        public BuildingMenu BuildingMenu;
        
        public GeoType BuildingType;

        protected override void OnEnable()
        {
            base.OnEnable();
            this.BtnBuild.localScale = Vector3.one;
        }

        public override void Check()
        {
            base.Check();
            this.BtnBuild.gameObject.SetActive(true);
            this.BtnBuild.localScale = Vector3.zero;
            this.BtnBuild.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.3f).OnComplete(() =>
            {
                this.BtnBuild.DOScale(new Vector3(1f, 1f, 1f), 0.1f);
            }
            );
        }
        public override void UnCheck()
        {
            base.UnCheck();
            this.BtnBuild.DOScale(new Vector3(0f, 0f, 0f), 0.2f).OnComplete(() =>
            {
                this.BtnBuild.gameObject.SetActive(false);
            }
            );
        }

        public void Onclick()
        {
            Debug.LogWarning("Onclick");
            if (!BuildingManager.instance.Building.Available.gameObject.activeSelf)
            {
                return;
            }
            if(!BuildingManager.instance.CanBuild(this.BuildingType)) return;
            if(!this.IsCheck){
                this.BuildingMenu.Chose(this);
                BuildingManager.instance.Build(this.BuildingType);
            }else{
                this.BuildingMenu.UnChose(this);
                BuildingManager.instance.Building.OffSkin();
            }
        }

        public void ConfigBuild()
        {
            this.Onclick();
            if(!BuildingManager.instance.CanBuild(this.BuildingType)) return;
            this.BtnBuild.gameObject.SetActive(false);
            InforBuildingUI inforBuildingUI = (InforBuildingUI)NTFunctions_old.UIManager.instance.GetPopupUIByCode(PopupCode.InforBuildingUI);
            if(inforBuildingUI == null) return;
            inforBuildingUI.OnUI(this.BuildingType);
            string title = "Build";
            string des = "";
            if(this.BuildingType == GeoType.ShopPlayer){
                title = Lean.Localization.LeanLocalization.GetTranslationText("title_shop", "Shop");
                des = Lean.Localization.LeanLocalization.GetTranslationText("des_shop", "Shop");
            }
            inforBuildingUI.Build = BuildingManager.instance.ConfirmBuild;
        }
    }

}
