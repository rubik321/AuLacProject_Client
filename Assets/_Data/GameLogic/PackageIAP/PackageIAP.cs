using System.Collections;
using System.Collections.Generic;
using Rubik.CharacterGear;
using UnityEngine;

namespace Rubik.Myrk.PackageIAP
{
    public class IndexPackageIAP
    {
        public static string StarterPack = "StarterPack";
        public static string RemoveAds = "RemoveAds";
    }

    [System.Serializable]
    public class PackageIAP
    {
        public string _id;
        public string Index;
        public int Group = 0;
        public long ExpiredTime; // 0: never expired
        public bool IsBought = false;

        public void Update(PackageIAP packageIAP)
        {
            this._id = packageIAP._id;
            this.Index = packageIAP.Index;
            this.Group = packageIAP.Group;
            this.ExpiredTime = packageIAP.ExpiredTime;
            this.IsBought = packageIAP.IsBought;
        }
    }

    [System.Serializable]
    public class PackageIAPData
    {
        public string Index;

        public ItemData[] OfferItems;
        public ChracterGearRarity[] OfferCharacterGearRarity;

        public ItemData[] PayItems;

        public string ProductId = "";

        public int Group = 0;

        public int Limit = 0; // -1: unlimited, 1: once, >1: limit
        public long ExpiredTime = 0; // -1: never expired
    }
}