using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GOA.Gear;
using GOA.Item;
using GOA.LandEvent;
using GOA.Mat;
using GOA.UserData;
using NTFunctions_old;
using NTPackage_old.UI;
using Rubik.Format;
using Rubik.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rubik.GOA.Blacksmith
{

    public class Blacksmith_UpgradeUI : PopupUI
    {
        public Transform GearHolder;
        public List<GearItem> GearItems;
        public GearItem GearItemSample;

        public GearData GearSelect;
        public Action UpdateAct;
        public Action UnselectAct;

        public Image IconGearPre;
        public Image IconGearNext;
        public TextMeshProUGUI TextNameGearPre;
        public TextMeshProUGUI TextNameGearNext;
        public StarIcon StarPre;
        public StarIcon StarNext;
        public RarityShadow RarityPre;
        public RarityShadow RarityNext;

        public List<TextMeshProUGUI> StatsPre;
        public List<TextMeshProUGUI> StatsNext;

        public MatListUI MatListUI;
        public bool IsEnoughMat = false;

        public TextMeshProUGUI TextSuccess;

        public NTButtonEffect BtnUpgrade;

        public Vector3 ShowPos;
        public Vector3 HidePos;
        public Transform TransInventory;
        public NTButtonEffect BtnCollaps;

        public bool IsBoost;
        public NTButtonEffect BtnBoost;

        public void OnUI()
        {
            this.GearSelect = null;
            this.UpdateAct = null;
            this.UnselectAct = null;
            this.IsBoost = false;
            this.BtnBoost.UnChose();
            this.IconGearPre.gameObject.SetActive(false);
            this.IconGearNext.gameObject.SetActive(false);
            this.TextNameGearPre.gameObject.SetActive(false);
            this.TextNameGearNext.gameObject.SetActive(false);
            this.StarPre.gameObject.SetActive(false);
            this.StarNext.gameObject.SetActive(false);
            this.RarityPre.gameObject.SetActive(false);
            this.RarityNext.gameObject.SetActive(false);
            this.TextSuccess.gameObject.SetActive(false);
            this.BtnUpgrade.SetActive(false);
            this.TransInventory.localPosition = this.HidePos;
            this.MatListUI.gameObject.SetActive(false);

            this.CollapsInventory();
            foreach (var item in StatsPre)
            {
                item.gameObject.SetActive(false);
            }
            foreach (var item in StatsNext)
            {
                item.gameObject.SetActive(false);
            }
            foreach (GearItem item in GearItems)
            {
                item.gameObject.SetActive(false);
            }
            UserData.Instance.gearData.Data.SortGear();
            for (int i = 0; i < UserData.Instance.gearData.Data.GearData.Count; i++)
            {
                GearItem gearItem;
                try
                {
                    gearItem = GearItems[i];
                }
                catch (System.Exception)
                {
                    gearItem = Instantiate(this.GearItemSample);
                    this.GearItems.Add(gearItem);
                    gearItem.transform.SetParent(this.GearHolder);
                    gearItem.transform.localScale = Vector3.one;
                }
                gearItem.SetData(UserData.Instance.gearData.Data.GearData[i]);

                gearItem.Onclick.RemoveAllListeners();
                gearItem.Onclick.AddListener(() =>
                {
                    this.SelectGear(gearItem);
                });
            }
            this.Show();
        }

        public void SelectGear(GearItem gearItem)
        {
            this.BtnUpgrade.gameObject.SetActive(true);
            this.TextSuccess.gameObject.SetActive(true);
            this.GearSelect = gearItem.GearData;
            this.UnselectAct?.Invoke();
            gearItem.Chose();
            this.UnselectAct = gearItem.UnSellect;
            this.UpdateAct = gearItem.UpdateData;
            this.UpdateData();
            this.IconGearPre.gameObject.SetActive(true);
            this.IconGearNext.gameObject.SetActive(true);
            this.TextNameGearPre.gameObject.SetActive(true);
            this.TextNameGearNext.gameObject.SetActive(true);
            this.StarPre.gameObject.SetActive(true);
            this.StarNext.gameObject.SetActive(true);
            this.RarityPre.gameObject.SetActive(true);
            this.RarityNext.gameObject.SetActive(true);
        }

        public override void UpdateData()
        {
            base.UpdateData();
            if (this.GearSelect == null) return;
            this.IconGearPre.sprite = SpriteHelper.Instance.GetIconGearSprite(GearSelect.GearCode);
            this.TextNameGearPre.text = GearSelect.GearName + " +" + GearSelect.UpgradeLv + " Lv." + GearSelect.Level;
            this.StarPre.SetData(GearSelect.Star);
            this.RarityPre.SetData(GearSelect.Rarity);

            GearInfoData gearInfoData = GearAsset.instance.GetItemDataByIndex(GearSelect.GearCode);
            GearTypeData gearTypeData = BlacksmithManager.instance.GearTypeDataDic.Get(gearInfoData.Slot.ToString());

            float cur_Stats_Increase = 0;
            if (GearSelect.UpgradeLv > 0)
            {
                cur_Stats_Increase = BlacksmithManager.instance.ListUpgradeLevelData[(int)gearTypeData.IndexUpgrade].Data[GearSelect.UpgradeLv - 1].Stats_Increase;
            }
            UpgradeLevelData upgradeLevelData = BlacksmithManager.instance.ListUpgradeLevelData[(int)gearTypeData.IndexUpgrade].Data[GearSelect.UpgradeLv];

            for (int i = 0; i < StatsPre.Count; i++)
            {
                try
                {
                    StatsPre[i].text = GearSelect.BaseStats[i].StatKey + ": " + (GearSelect.BaseStats[i].StatValue * (1 + cur_Stats_Increase));
                    StatsPre[i].gameObject.SetActive(true);
                }
                catch (System.Exception)
                {
                    StatsPre[i].gameObject.SetActive(false);
                }
            }


            if (CheckCapLv())
            {
                this.BtnUpgrade.UnChose();
                this.IconGearNext.gameObject.SetActive(false);
                this.TextNameGearNext.gameObject.SetActive(false);
                this.StarNext.gameObject.SetActive(false);
                this.RarityNext.gameObject.SetActive(false);
                this.MatListUI.gameObject.SetActive(false);
            }
            else
            {
                this.BtnUpgrade.Chose();
                this.MatListUI.gameObject.SetActive(true);
                this.IconGearNext.sprite = SpriteHelper.Instance.GetIconGearSprite(GearSelect.GearCode);
                this.TextNameGearNext.text = GearSelect.GearName + " +" + (GearSelect.UpgradeLv + 1) + " Lv." + GearSelect.Level;
                this.StarNext.SetData(GearSelect.Star);
                this.RarityNext.SetData(GearSelect.Rarity);
                for (int i = 0; i < StatsNext.Count; i++)
                {
                    try
                    {
                        StatsNext[i].text = GearSelect.BaseStats[i].StatKey + ": " + (GearSelect.BaseStats[i].StatValue * (1 + upgradeLevelData.Stats_Increase));
                        StatsNext[i].gameObject.SetActive(true);
                    }
                    catch (System.Exception)
                    {
                        StatsNext[i].gameObject.SetActive(false);
                    }
                }

                this.IsEnoughMat = true;
                List<(Sprite, string, Color)> values = new List<(Sprite, string, Color)>();
                //Mat_1
                ItemInfoData itemData = ItemAsset.instance.GetItemDataByCode((ItemCode)gearTypeData.Mat_1);
                Sprite icon = SpriteHelper.Instance.GetSprite(itemData.Images);
                int amountInventory = UserData.Instance.Inventory.GetInventoryByCode((ItemCode)gearTypeData.Mat_1);
                string detail = FormatData.GetFriendlyShortNumber(upgradeLevelData.Mat_1) + "";
                Color color = Color.white;
                if (upgradeLevelData.Mat_1 > amountInventory)
                {
                    color = Color.red;
                    this.IsEnoughMat = false;
                }
                values.Add((icon, detail, color));
                //Mat_2
                itemData = ItemAsset.instance.GetItemDataByCode((ItemCode)gearTypeData.Mat_2);
                icon = SpriteHelper.Instance.GetSprite(itemData.Images);
                amountInventory = UserData.Instance.Inventory.GetInventoryByCode((ItemCode)gearTypeData.Mat_2);
                detail = FormatData.GetFriendlyShortNumber(upgradeLevelData.Mat_2) + "";
                color = Color.white;
                if (upgradeLevelData.Mat_2 > amountInventory)
                {
                    color = Color.red;
                    this.IsEnoughMat = false;
                }
                values.Add((icon, detail, color));
                //Gold
                itemData = ItemAsset.instance.GetItemDataByCode(ItemCode.Coin);
                icon = SpriteHelper.Instance.GetSprite(itemData.Images);
                amountInventory = UserData.Instance.Inventory.GetInventoryByCode(ItemCode.Coin);
                detail = FormatData.GetFriendlyShortNumber(upgradeLevelData.Gold) + "";
                color = Color.white;
                if (upgradeLevelData.Gold > amountInventory)
                {
                    color = Color.red;
                    this.IsEnoughMat = false;
                }
                values.Add((icon, detail, color));
                //Gin
                var costBoost = this.IsBoost ? upgradeLevelData.Boost : 0;
                itemData = ItemAsset.instance.GetItemDataByCode(ItemCode.Gin);
                icon = SpriteHelper.Instance.GetSprite(itemData.Images);
                amountInventory = UserData.Instance.Inventory.GetInventoryByCode(ItemCode.Gin);
                detail = FormatData.GetFriendlyShortNumber(upgradeLevelData.Gin + costBoost) + "";
                color = Color.white;
                if (upgradeLevelData.Gin + costBoost > amountInventory)
                {
                    color = Color.red;
                    this.IsEnoughMat = false;
                }
                values.Add((icon, detail, color));

                this.MatListUI.SetData(values);

                this.TextSuccess.text = "Success Rate: " + (((this.IsBoost ? upgradeLevelData.BoostRate : upgradeLevelData.Rate)+ GetRateByLand()) * 100) + "%";
            }


        }

        public void ClickUpgrade()
        {
            if (CheckCapLv())
            {
                HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("cap_lv", "This gear reach max level."), "Message", null, null);
                return;
            }
            if (CheckCapLvStar())
            {
                HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("cap_lv_star", "You need upgrade gear stars to continue upgrade levels."), "Message", null, null);
                return;
            }
            if (!IsEnoughMat)
            {
                HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("dont_enough_material", "You have insufficient Material."), "Message", null, null);
                return;
            }
            BlacksmithManager.instance.UpgradeLv(GearSelect._id, this.IsBoost, (data) =>
            {
                if (data)
                {
                    HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("upgrade_success", "Upgrade Success!"), "Message", null, null);
                    GearSelect.UpgradeLv++;
                    GearSelect.SetUp();
                    this.UpdateAct?.Invoke();
                    this.UpdateData();
                }
                else
                {
                    HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("upgrade_fail", "Upgrade Fail!"), "Message", null, null);
                }
            });
        }

        public bool CheckCapLvStar()
        {
            if (GearSelect.Star < 1)
            {
                if (GearSelect.UpgradeLv >= 6) return true;
            }
            if (GearSelect.Star < 2)
            {
                if (GearSelect.UpgradeLv >= 12) return true;
            }
            if (GearSelect.Star < 3)
            {
                if (GearSelect.UpgradeLv >= 16) return true;
            }
            if (GearSelect.UpgradeLv >= 20) return true;
            return false;
        }

        public bool CheckCapLv()
        {
            if (GearSelect.UpgradeLv >= 20) return true;
            return false;
        }

        public float GetRateByLand()
        {
            if (UserData.Instance.CountLand < 20) return 0f;
            if (UserData.Instance.CountLand < 50) return 0.02f;
            if (UserData.Instance.CountLand < 100) return 0.05f;
            if (UserData.Instance.CountLand < 150) return 0.07f;
            if (UserData.Instance.CountLand < 200) return 0.1f;
            return 0.15f;
        }

        [ContextMenu("Test")]
        public void CollapsInventory()
        {
            if (this.TransInventory.localPosition == this.HidePos)
            {
                this.BtnCollaps.Chose();
                this.TransInventory.DOLocalMove(ShowPos, 0.5f);
            }
            else
            {
                this.BtnCollaps.UnChose();
                this.TransInventory.DOLocalMove(HidePos, 0.5f);
            }
        }

        public void ClickBoost()
        {
            if (this.IsBoost)
            {
                this.BtnBoost.UnChose();
                this.IsBoost = false;
            }
            else
            {
                this.BtnBoost.Chose();
                this.IsBoost = true;
            }
            this.UpdateData();
        }
    }
}
