using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GOA.Gear;
using GOA.Item;
using GOA.Mat;
using GOA.UserData;
using NTFunctions_old;
using NTPackage_old.UI;
using Rubik.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rubik.GOA.Blacksmith
{
    public class Blacksmith_FragmentUI : PopupUI
    {
        public Transform GearHolder;
        public List<GearItem> GearItems;
        public GearItem GearItemSample;
        public GearData GearSelect;
        public Action UnselectAct;

        public Image IconGear;
        public TextMeshProUGUI TextNameGear;
        public StarIcon Star;
        public RarityShadow Rarity;
        public NTButtonEffect BtnConfirm;
        public Vector3 ShowPos;
        public Vector3 HidePos;
        public Transform TransInventory;
        public NTButtonEffect BtnCollaps;
        public MatListUI MatListUI;

        public void OnUI()
        {
            this.GearSelect = null;
            this.UnselectAct = null;
            this.IconGear.gameObject.SetActive(false);
            this.TextNameGear.gameObject.SetActive(false);
            this.Star.gameObject.SetActive(false);
            this.Rarity.gameObject.SetActive(false);
            this.BtnConfirm.SetActive(false);
            this.TransInventory.localPosition = this.HidePos;
            this.MatListUI.gameObject.SetActive(false);
            this.CollapsInventory();
            foreach (GearItem item in GearItems)
            {
                item.gameObject.SetActive(false);
            }
            UserData.Instance.gearData.Data.SortGear();
            for (int i = 0; i < UserData.Instance.gearData.Data.GearData.Count; i++)
            {
                if (UserData.Instance.gearData.Data.GearData[i].Equiped) continue;
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
            this.MatListUI.gameObject.SetActive(true);
            this.IconGear.sprite = SpriteHelper.Instance.GetIconGearSprite(GearSelect.GearCode);
            this.TextNameGear.text = GearSelect.GearName + " +" + GearSelect.UpgradeLv + " Lv." + GearSelect.Level;
            this.Star.SetData(GearSelect.Star);
            this.Rarity.SetData(GearSelect.Rarity);

            GearInfoData gearInfoData = GearAsset.instance.GetItemDataByIndex(GearSelect.GearCode);
            GearTypeData gearTypeData = BlacksmithManager.instance.GearTypeDataDic.Get(gearInfoData.Slot.ToString());
            FragmentData fragmentData = BlacksmithManager.instance.FragmentDataDic.Get(GearSelect.Rarity.ToString());
            List<(Sprite, string, Color)> values = new List<(Sprite, string, Color)>();
            //Mat_1
            ItemInfoData itemData = ItemAsset.instance.GetItemDataByCode((ItemCode)gearTypeData.Mat_1);
            Sprite icon = SpriteHelper.Instance.GetSprite(itemData.Images);
            string detail = (fragmentData.Mat_1_Min + GearSelect.UpgradeLv * 2) + "-" + (fragmentData.Mat_1_Max + GearSelect.UpgradeLv * 2);
            Color color = Color.green;
            values.Add((icon, detail, color));
            //Mat_2
            itemData = ItemAsset.instance.GetItemDataByCode((ItemCode)gearTypeData.Mat_2);
            icon = SpriteHelper.Instance.GetSprite(itemData.Images);
            detail = (fragmentData.Mat_2_Min + GearSelect.UpgradeLv * 1) + "-" + (fragmentData.Mat_2_Max + GearSelect.UpgradeLv * 1);
            color = Color.green;
            values.Add((icon, detail, color));
            //Mat_3
            itemData = ItemAsset.instance.GetItemDataByCode(BlacksmithManager.instance.GetOrbName((int)gearInfoData.Class));
            icon = SpriteHelper.Instance.GetSprite(itemData.Images);
            detail = (int)(fragmentData.Mat_3_Min + GearSelect.Star) + "-" + (int)(fragmentData.Mat_3_Max + GearSelect.Star);
            color = Color.green;
            if ((int)(fragmentData.Mat_3_Max + GearSelect.Star) > 0) values.Add((icon, detail, color));

            this.MatListUI.SetData(values);

        }

        public void ClickConfirm()
        {
            BlacksmithManager.instance.FragmentGear(GearSelect._id, () =>
            {
                UserData.Instance.gearData.Data.RemoveById(GearSelect._id);
                this.GearSelect._id = "";
                this.UnselectAct?.Invoke();
                this.GearSelect = null;
                this.UnselectAct = null;
                this.IconGear.gameObject.SetActive(false);
                this.TextNameGear.gameObject.SetActive(false);
                this.Star.gameObject.SetActive(false);
                this.Rarity.gameObject.SetActive(false);
                this.BtnConfirm.SetActive(false);
                this.MatListUI.gameObject.SetActive(false);
            });
        }

        public void SelectGear(GearItem gearItem)
        {
            this.BtnConfirm.gameObject.SetActive(true);
            this.BtnConfirm.Chose();
            this.GearSelect = gearItem.GearData;
            this.UnselectAct?.Invoke();
            gearItem.Chose();
            this.UnselectAct = gearItem.UnSellect;
            this.UpdateData();
            this.IconGear.gameObject.SetActive(true);
            this.TextNameGear.gameObject.SetActive(true);
            this.Star.gameObject.SetActive(true);
            this.Rarity.gameObject.SetActive(true);
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
    }
}

