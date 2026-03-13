using System;
using System.Collections;
using System.Collections.Generic;
using NTFunctions_old;
using NTPackage_old.Functions;
using NTPackage_old.UI;
using Rubik.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rubik.GOA.Blacksmith
{
    public class GearItem : NTButtonEffect
    {
        public StarIcon StarIcon;
        public RarityShadow RarityShadow;
        public TextMeshProUGUI TextUpgradeLv;
        public Image IconGear;

        public GearData GearData;

        public void SetData(GearData gearData)
        {
            this.GearData = gearData;
            if(this.GearData == null || this.GearData._id.Length == 0){
                gameObject.SetActive(false);
                return;
            }
            gameObject.SetActive(true);
            this.UpdateData();
            this.UnChose();
        }

        public void UpdateData()
        {
            if(this.GearData == null ||this.GearData._id == null|| this.GearData._id.Length == 0){
                gameObject.SetActive(false);
                return;
            }
            this.StarIcon.SetData(GearData.Star);
            this.RarityShadow.SetData(GearData.Rarity);
            this.TextUpgradeLv.text = "+" + GearData.UpgradeLv;
            if (GearData.UpgradeLv < 1) this.TextUpgradeLv.gameObject.SetActive(false);
            else this.TextUpgradeLv.gameObject.SetActive(true);
            IconGear.sprite = SpriteHelper.Instance.GetIconGearSprite(GearData.GearCode);
        }

        public void UnSellect(){
            this.UpdateData();
            this.UnChose();
        }
    }
}