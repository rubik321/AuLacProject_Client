using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GOA.Gear;
using GOA.Item;
using GOA.UserData;
using NTFunctions_old;
using NTPackage_old.UI;
using Rubik.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rubik.GOA.Blacksmith
{
    public class Blacksmith_UpStarUI : PopupUI
    {
        public Transform GearHolder;
        public List<GearItem> GearItems;
        public GearItem GearItemSample;

        public GearData GearSelect;
        public Action UpdateAct;
        public Action UnselectAct;

        public Image IconGear;
        public TextMeshProUGUI TextNameGear;
        public Star_Shadow Star;
        public RarityShadow Rarity;
        public List<TextMeshProUGUI> Stats;

        public bool IsEnoughMat = false;
        public NTButtonEffect BtnUpgrade;

        public Vector3 ShowPos;
        public Vector3 HidePos;
        public Transform TransInventory;
        public NTButtonEffect BtnCollaps;
        public Image IconCost;
        public TextMeshProUGUI TextCost;
        public TextMeshProUGUI NameCost;

        public void OnUI()
        {
            this.GearSelect = null;
            this.UpdateAct = null;
            this.UnselectAct = null;
            this.IconGear.gameObject.SetActive(false);
            this.TextNameGear.gameObject.SetActive(false);
            this.Star.gameObject.SetActive(false);
            this.Rarity.gameObject.SetActive(false);
            this.BtnUpgrade.SetActive(false);
            this.IconCost.gameObject.SetActive(false);
            this.TextCost.gameObject.SetActive(false);
            this.NameCost.gameObject.SetActive(false);
            this.TransInventory.localPosition = this.HidePos;
            this.CollapsInventory();
            foreach (var item in Stats)
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

        public override void UpdateData()
        {
            base.UpdateData();
            if (this.GearSelect == null) return;
            this.IconGear.sprite = SpriteHelper.Instance.GetIconGearSprite(GearSelect.GearCode);
            this.TextNameGear.text = GearSelect.GearName + " +" + GearSelect.UpgradeLv + " Lv." + GearSelect.Level;
            this.Star.SetData(GearSelect.Star);
            this.Rarity.SetData(GearSelect.Rarity);

            GearInfoData gearInfoData = GearAsset.instance.GetItemDataByIndex(GearSelect.GearCode);
            GearTypeData gearTypeData = BlacksmithManager.instance.GearTypeDataDic.Get(gearInfoData.Slot.ToString());

            float cur_Stats_Increase = 0;
            if (GearSelect.UpgradeLv > 0)
            {
                cur_Stats_Increase = BlacksmithManager.instance.ListUpgradeLevelData[(int)gearTypeData.IndexUpgrade].Data[GearSelect.UpgradeLv - 1].Stats_Increase;
            }

            for (int i = 0; i < Stats.Count; i++)
            {
                Stats[i].gameObject.SetActive(true);
                try
                {
                    switch (GearSelect.OptionStats[i].StatKey)
                    {
                        case "HP_Per":
                            Stats[i].text = "HP" + ": " + (GearSelect.OptionStats[i].StatValue * 100) + "%";
                            break;
                        case "MP_Per":
                            Stats[i].text = "MP" + ": " + (GearSelect.OptionStats[i].StatValue * 100) + "%";
                            break;
                        default:
                            Stats[i].text = GearSelect.OptionStats[i].StatKey + ": " + GearSelect.OptionStats[i].StatValue;
                            break;
                    }
                }
                catch (System.Exception)
                {
                    Stats[i].text = (i + 3) + " stars unlock.";
                }
            }


            if (CheckCapStar())
            {
                this.BtnUpgrade.UnChose();
                this.IconCost.gameObject.SetActive(false);
                this.TextCost.gameObject.SetActive(false);
                this.NameCost.gameObject.SetActive(false);
            }
            else
            {
                this.BtnUpgrade.Chose();
                this.IconCost.gameObject.SetActive(true);
                this.TextCost.gameObject.SetActive(true);
                this.NameCost.gameObject.SetActive(true);
                this.IsEnoughMat = true;
                this.TextCost.text = OrbCostUpStar() + "";
                int amountInventory = UserData.Instance.Inventory.GetInventoryByCode(BlacksmithManager.instance.GetOrbName((int)gearInfoData.Class));
                ItemInfoData itemData = ItemAsset.instance.GetItemDataByCode(BlacksmithManager.instance.GetOrbName((int)gearInfoData.Class));
                this.IconCost.sprite = SpriteHelper.Instance.GetSprite(itemData.Images);
                this.NameCost.text = Lean.Localization.LeanLocalization.GetTranslationText(itemData.Index + "_Name", "Orbs");
                if (OrbCostUpStar() > amountInventory)
                {
                    this.TextCost.color = Color.red;
                    this.IsEnoughMat = false;
                }
                else
                {
                    this.TextCost.color = Color.white;
                }
            }
        }

        public void SelectGear(GearItem gearItem)
        {
            this.BtnUpgrade.gameObject.SetActive(true);
            this.GearSelect = gearItem.GearData;
            this.UnselectAct?.Invoke();
            gearItem.Chose();
            this.UnselectAct = gearItem.UnSellect;
            this.UpdateAct = gearItem.UpdateData;
            this.UpdateData();
            this.IconGear.gameObject.SetActive(true);
            this.TextNameGear.gameObject.SetActive(true);
            this.Star.gameObject.SetActive(true);
            this.Rarity.gameObject.SetActive(true);
        }

        public void ClickUpgrade()
        {
            if (CheckCapStar())
            {
                HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("cap_star", "This gear reach max stars."), "Message", null, null);
                return;
            }
            if (!IsEnoughMat)
            {
                HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("dont_enough_material", "You have insufficient Material."), "Message", null, null);
                return;
            }
            BlacksmithManager.instance.UpStar(GearSelect._id, (data) =>
            {
                HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("upgrade_success", "Upgrade Success!"), "Message", null, null);
                this.GearSelect.OptionStats = data.OptionStats;
                GearSelect.Star++;
                GearSelect.SetUp();
                this.UpdateAct?.Invoke();
                this.UpdateData();
            });
        }

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

        public bool CheckCapStar()
        {
            if (GearSelect.Star >= 5) return true;
            return false;
        }

        public int OrbCostUpStar()
        {
            if (this.GearSelect.Star == 0) return 5;
            if (this.GearSelect.Star == 1) return 10;
            if (this.GearSelect.Star == 2) return 15;
            if (this.GearSelect.Star == 3) return 20;
            return 25;
        }
    }
}