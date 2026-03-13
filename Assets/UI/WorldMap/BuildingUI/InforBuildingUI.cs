using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using NTFunctions_old;
using NTPackage_old.Functions;
using System;
using GOA.Item;
using UnityEngine.UI;
using GOA.WorldMap;
using GOA.Mat;
using GOA.LandEvent;
using Rubik.Format;
using GOA.UIWorldMap;

namespace GOA.Building
{
    public class InforBuildingUI : PopupUI
    {
        public TextMeshProUGUI Title;
        public TextMeshProUGUI TitleName;
        public TextMeshProUGUI Des;

        public MatListUI MatListUI;
        public BuildingInfo BuildingInfo;
        public bool IsEnoughMat = true;

        public Transform BtnBuid;

        public Transform Holder;

        public Action<GeoType> Build;
        public Image Icon;

        public void OnUI(GeoType buildingType)
        {
            this.BuildingInfo = BuildingManager.instance.BuildingInfoDic.Get(buildingType.ToString());
            if (this.BuildingInfo == null) return;
            this.UpdateData();
            this.Show();
        }

        public override void UpdateData()
        {
            base.UpdateData();
            switch (this.BuildingInfo.BuildingType)
            {
                case GeoType.ShopPlayer:
                    this.TitleName.text = UserData.UserData.Instance.data.AmountShopBuilding + "/" + Config.Configs.CapShopBuilding;
                    this.Title.text = Lean.Localization.LeanLocalization.GetTranslationText("title_shop", "Shop");
                    this.Des.text = Lean.Localization.LeanLocalization.GetTranslationText("des_shop", "This is Shop");
                    break;
                case GeoType.Blacksmith:
                    this.TitleName.text = UserData.UserData.Instance.data.AmountBlacksmithBuilding + "/" + Config.Configs.CapBlackSmithBuilding;
                    this.Title.text = Lean.Localization.LeanLocalization.GetTranslationText("title_blacksmith", "Blacksmith");
                    this.Des.text = Lean.Localization.LeanLocalization.GetTranslationText("des_blacksmith", "This is Blacksmith.");
                    break;
                case GeoType.Dungeon:
                    this.TitleName.text = UserData.UserData.Instance.data.AmountDungeonBuilding + "/" + Config.Configs.CapDungeonBuilding;
                    this.Title.text = Lean.Localization.LeanLocalization.GetTranslationText("title_dungeon", "Dungeon");
                    this.Des.text = Lean.Localization.LeanLocalization.GetTranslationText("des_dungeon", "This is Dungeon.");
                    break;
                case GeoType.Castle:
                    this.TitleName.text = UserData.UserData.Instance.data.AmountCastleBuilding + "/" + Config.Configs.CapCastleBuilding;
                    this.Title.text = Lean.Localization.LeanLocalization.GetTranslationText("title_castle", "Castle");
                    this.Des.text = Lean.Localization.LeanLocalization.GetTranslationText("des_castle", "This is Castle.");
                    break;
            }

            this.Icon.sprite = BuildingManager.instance.GetBuildingIcon(this.BuildingInfo.BuildingType);

            this.IsEnoughMat = true;
            List<(Sprite, string, Color)> values = new List<(Sprite, string, Color)>();
            foreach (MatRequire item in this.BuildingInfo.MatRequire)
            {
                GOA.Item.ItemInfoData itemData = ItemAsset.instance.GetItemDataByCode(item.Code);
                Sprite icon = SpriteHelper.Instance.GetSprite(itemData.Images);
                int amountInventory = UserData.UserData.Instance.Inventory.GetInventoryByCode(item.Code);
                string detail = FormatData.GetFriendlyShortNumber(item.Amount) + "";
                Color color = Color.white;
                if (item.Amount > amountInventory)
                {
                    color = Color.red;
                    this.IsEnoughMat = false;
                }
                values.Add((icon, detail, color));
            }
            this.MatListUI.SetData(values);

            this.BtnBuid.gameObject.SetActive(true);
            switch (this.BuildingInfo.BuildingType)
            {
                case GeoType.ShopPlayer:
                    if (UserData.UserData.Instance.data.AmountShopBuilding >= Config.Configs.CapShopBuilding)
                    {
                        this.BtnBuid.gameObject.SetActive(false);
                    }
                    break;
                case GeoType.Blacksmith:
                    if (UserData.UserData.Instance.data.AmountBlacksmithBuilding >= Config.Configs.CapBlackSmithBuilding)
                    {
                        this.BtnBuid.gameObject.SetActive(false);
                    }
                    break;
                case GeoType.Dungeon:
                    if (UserData.UserData.Instance.data.AmountDungeonBuilding >= Config.Configs.CapDungeonBuilding)
                    {
                        this.BtnBuid.gameObject.SetActive(false);
                    }
                    break;
                case GeoType.Castle:
                    if (UserData.UserData.Instance.data.AmountCastleBuilding >= Config.Configs.CapCastleBuilding)
                    {
                        this.BtnBuid.gameObject.SetActive(false);
                    }
                    break;
                default:
                    this.BtnBuid.gameObject.SetActive(false);
                    break;
            }

            BuildingManager.instance.Building.UpdateData();
            if (!BuildingManager.instance.Building.Available.gameObject.activeSelf)
            {
                this.BtnBuid.gameObject.SetActive(false);
            }
            if (!this.IsEnoughMat) this.BtnBuid.gameObject.SetActive(false);
        }

        public void Confirm()
        {
            this.Hide();
            PanelMainToolUI.instance.OffBuilding();
            if (!BuildingManager.instance.CanBuild(this.BuildingInfo.BuildingType)) return;
            this.Build.Invoke(this.BuildingInfo.BuildingType);
        }
    }
}
