using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GOA.Item;
using GOA.LandEvent;
using GOA.Mat;
using GOA.Portal;
using GOA.UserData;
using NTFunctions_old;
using NTPackage_old.UI;
using Rubik.Format;
using Rubik.UI;
using TMPro;
using UnityEngine;

namespace Rubik.GOA.Blacksmith
{
    public class Blacksmith_FusionUI : PopupUI
    {
        public Transform GearHolder;
        public List<GearItem> GearItems;
        public GearItem GearItemSample;

        public GearData Mat1;
        public GearData Mat2;
        public GearData Mat3;

        public GearItem ItemMat1;
        public GearItem ItemMat2;
        public GearItem ItemMat3;

        public Action UnselectMat1Act;
        public Action UnselectMat2Act;
        public Action UnselectMat3Act;

        public GearItem ItemResult;

        public MatListUI MatListUI;
        public NTButtonEffect BtnConfirm;

        public TextMeshProUGUI TextSuccess;
        public Vector3 ShowPos;
        public Vector3 HidePos;
        public Transform TransInventory;
        public NTButtonEffect BtnCollaps;
        public bool IsEnoughMat;
        public bool IsBoost;
        public NTButtonEffect BtnBoost;
        public void OnUI()
        {
            this.Mat1 = null;
            this.Mat2 = null;
            this.Mat3 = null;
            this.IsBoost = false;
            this.BtnBoost.UnChose();
            this.ItemMat1.gameObject.SetActive(false);
            this.ItemMat2.gameObject.SetActive(false);
            this.ItemMat3.gameObject.SetActive(false);
            this.ItemMat1.UnChose();
            this.ItemMat2.UnChose();
            this.ItemMat3.UnChose();
            this.ItemResult.gameObject.SetActive(false);
            this.BtnConfirm.UnChose();
            this.TextSuccess.gameObject.SetActive(false);
            this.TransInventory.localPosition = this.HidePos;
            this.MatListUI.gameObject.SetActive(false);

            this.CollapsInventory();
            foreach (GearItem item in GearItems)
            {
                item.gameObject.SetActive(false);
                item.GearData = null;
            }
            UserData.Instance.gearData.Data.SortGear();
            for (int i = 0; i < UserData.Instance.gearData.Data.GearData.Count; i++)
            {
                if(UserData.Instance.gearData.Data.GearData[i].Equiped) continue;
                if (UserData.Instance.gearData.Data.GearData[i].Rarity == 4) continue;
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
                    if (gearItem.IsChose())
                    {
                        if (this.Mat1 != null && this.Mat1._id.Equals(gearItem.GearData._id))
                        {
                            this.UnSellect(1);
                        }
                        else if (this.Mat2 != null && this.Mat2._id.Equals(gearItem.GearData._id))
                        {
                            this.UnSellect(2);
                        }
                        else if (this.Mat3 != null && this.Mat3._id.Equals(gearItem.GearData._id))
                        {
                            this.UnSellect(3);
                        }
                        else this.UnSellect(1);
                    }
                    else
                    {
                        this.SelectGear(gearItem);
                    }
                });
            }
            this.Show();
        }

        public override void UpdateData()
        {
            if (CanFusion())
            {
                this.BtnConfirm.Chose();
                this.TextSuccess.gameObject.SetActive(true);
                FusionGear fusionGear = BlacksmithManager.instance.FusionGearDic.Get(this.Mat1.Rarity.ToString());
                this.TextSuccess.text = "Success Rate: " + (this.IsBoost ? fusionGear.BoostRate : fusionGear.Rate) * 100 + "%";
                this.MatListUI.gameObject.SetActive(true);

                this.IsEnoughMat = true;
                List<(Sprite, string, Color)> values = new List<(Sprite, string, Color)>();
                //Gold
                ItemInfoData itemData = ItemAsset.instance.GetItemDataByCode(ItemCode.Coin);
                Sprite icon = SpriteHelper.Instance.GetSprite(itemData.Images);
                int amountInventory = UserData.Instance.Inventory.GetInventoryByCode(ItemCode.Coin);
                string detail = FormatData.GetFriendlyShortNumber(fusionGear.Gold) + "";
                Color color = Color.white;
                if (fusionGear.Gold > amountInventory)
                {
                    color = Color.red;
                    this.IsEnoughMat = false;
                }
                values.Add((icon, detail, color));
                //Gin
                var costBoost = this.IsBoost ? fusionGear.Boost : 0;
                itemData = ItemAsset.instance.GetItemDataByCode(ItemCode.Gin);
                icon = SpriteHelper.Instance.GetSprite(itemData.Images);
                amountInventory = UserData.Instance.Inventory.GetInventoryByCode(ItemCode.Gin);
                detail = FormatData.GetFriendlyShortNumber(fusionGear.GIN + costBoost) + "";
                color = Color.white;
                if (fusionGear.GIN + costBoost > amountInventory)
                {
                    color = Color.red;
                    this.IsEnoughMat = false;
                }
                values.Add((icon, detail, color));

                this.MatListUI.SetData(values);
            }
            else
            {
                this.BtnConfirm.UnChose();
                this.TextSuccess.gameObject.SetActive(false);
                this.MatListUI.gameObject.SetActive(false);
            }
        }

        public void ClickConfirm()
        {
            List<string> gearIds = new();
            gearIds.Add(Mat1._id);
            gearIds.Add(Mat2._id);
            gearIds.Add(Mat3._id);
            BlacksmithManager.instance.FusionGear(gearIds.ToArray(), this.IsBoost, (data) =>
            {
                if (data.Success)
                {
                    HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("upgrade_success", "Upgrade Success!"), "Message", null, null);
                    this.Mat1.Rarity++;
                    UserData.Instance.gearData.Data.RemoveById(Mat2._id);
                    UserData.Instance.gearData.Data.RemoveById(Mat3._id);
                    this.Mat2._id = "";
                    this.Mat3._id = "";
                    this.ItemResult.SetData(Mat1);
                    this.UnSellect(1);
                }
                else
                {
                    HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("upgrade_fail", "Upgrade Fail!"), "Message", null, null);
                    foreach (string item in data.GearLost)
                    {
                        if (item.Equals(Mat2._id))
                        {
                            UserData.Instance.gearData.Data.RemoveById(Mat2._id);
                            this.Mat2._id = "";
                            this.UnSellect(2);
                        }
                        if (item.Equals(Mat3._id))
                        {
                            UserData.Instance.gearData.Data.RemoveById(Mat3._id);
                            this.Mat3._id = "";
                            this.UnSellect(3);
                        }
                    }
                }
                this.UpdateData();
            });
        }

        public void SelectGear(GearItem gearItem)
        {
            this.ItemResult.gameObject.SetActive(false);
            if (this.Mat1 == null || this.Mat1._id == null || this.Mat1._id.Length == 0)
            {
                this.Mat1 = gearItem.GearData;
                this.ItemMat1.SetData(this.Mat1);
                for (int i = 0; i < this.GearItems.Count; i++)
                {
                    GearItem item = this.GearItems[i];
                    if(item.GearData == null){
                        item.gameObject.SetActive(false);
                        continue;
                    }
                    item.UpdateData();
                    if (item.GearData.Rarity != this.Mat1.Rarity)
                    {
                        Debug.LogWarning(item.GearData.Rarity);
                        item.gameObject.SetActive(false);
                    }
                }
                this.UnselectMat1Act = gearItem.UnSellect;
                this.UpdateData();
                gearItem.Chose();
                return;
            }
            if (this.Mat2 == null || this.Mat2._id == null|| this.Mat2._id.Length == 0)
            {
                this.Mat2 = gearItem.GearData;
                this.ItemMat2.SetData(this.Mat2);
                this.UnselectMat2Act = gearItem.UnSellect;
                this.UpdateData();
                gearItem.Chose();
                return;
            }
            if (this.Mat3 == null || this.Mat3._id == null|| this.Mat3._id.Length == 0)
            {
                this.Mat3 = gearItem.GearData;
                this.ItemMat3.SetData(this.Mat3);
                this.UnselectMat3Act = gearItem.UnSellect;
                this.UpdateData();
                gearItem.Chose();
                return;
            }

        }

        public void UnSellect(int slot)
        {
            if (slot == 1)
            {
                this.Mat1 = null;
                this.ItemMat1.gameObject.SetActive(false);
                this.Mat2 = null;
                this.ItemMat2.gameObject.SetActive(false);
                this.Mat3 = null;
                this.ItemMat3.gameObject.SetActive(false);
                this.UnselectMat1Act?.Invoke();
                this.UnselectMat2Act?.Invoke();
                this.UnselectMat3Act?.Invoke();
                foreach (GearItem item in this.GearItems)
                {
                    if (item.GearData != null && item.GearData._id.Length > 0)
                    {
                        item.gameObject.SetActive(true);
                    }
                }
                this.UpdateData();
                return;
            }
            if (slot == 2)
            {
                this.Mat2 = null;
                this.ItemMat2.gameObject.SetActive(false);
                this.UnselectMat2Act?.Invoke();
                this.UpdateData();
                return;
            }
            if (slot == 3)
            {
                this.Mat3 = null;
                this.ItemMat3.gameObject.SetActive(false);
                this.UnselectMat3Act?.Invoke();
                this.UpdateData();
                return;
            }
        }

        public bool CanFusion()
        {
            if (this.Mat1 == null || this.Mat1._id.Length == 0)
            {
                return false;
            }
            if (this.Mat2 == null || this.Mat2._id.Length == 0)
            {
                return false;
            }
            if (this.Mat3 == null || this.Mat3._id.Length == 0)
            {
                return false;
            }
            return true;
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