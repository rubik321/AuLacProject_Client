using System;
using System.Collections;
using System.Collections.Generic;
using NTPackage;
using NTPackage.Functions;
using Rubik.Config;
using Rubik.DataCenter;
using Rubik.IAP;
using Rubik.Manager;
using SimpleJSON;
using UnityEngine;

namespace Rubik.Myrk.PackageIAP
{
    public class PackageIAPManager : NTBehaviour
    {
        #region User Data
        public NTDictionary<string, PackageIAP> PackageIAP = new NTDictionary<string, PackageIAP>();
        #endregion

        #region Game Data
        [SerializeField] private NTDictionary<string, PackageIAPData> PackageIAPData = new NTDictionary<string, PackageIAPData>();
        #endregion

        #region Resource
        public List<PackageIAPUI> PackageIAPUIs;
        public List<BtnPackageIAP> BtnPackageIAPs;
        public NTDictionary<string, PackageIAPUI> PackageIAPUIsDic = new NTDictionary<string, PackageIAPUI>();
        public NTDictionary<string, BtnPackageIAP> BtnPackageIAPsDic = new NTDictionary<string, BtnPackageIAP>();
        #endregion

        public static PackageIAPManager Instance;
        protected override void Awake()
        {
            base.Awake();
            if (PackageIAPManager.Instance != null)
            {
                NTLog.LogError("Only 1 Instance allow");
                return;
            }
            PackageIAPManager.Instance = this;
        }


        #region Function

        public void LoadData()
        {
            this.PackageIAP = new NTDictionary<string, PackageIAP>();
            this.PackageIAPData = new NTDictionary<string, PackageIAPData>();

            JSONNode jdata = JSONNode.Parse(DataCenterManager.Instance.GetData(DataName.PackageIAPData));
            foreach (JSONNode item in jdata)
            {
                PackageIAPData packageIAPData = JsonUtility.FromJson<PackageIAPData>(item.ToString());
                this.PackageIAPData.Add(packageIAPData.Index, packageIAPData);
            }

            this.PackageIAPUIsDic.Clear();
            foreach (PackageIAPUI item in this.PackageIAPUIs)
            {
                this.PackageIAPUIsDic.Add(item.PackageIAPIndex, item);
            }

            this.BtnPackageIAPsDic.Clear();
            foreach (BtnPackageIAP item in this.BtnPackageIAPs)
            {
                this.BtnPackageIAPsDic.Add(item.PackageIAPIndex, item);
            }
        }

        public void Logout()
        {
            this.PackageIAP = new NTDictionary<string, PackageIAP>();
        }

        public void UpdatePackageIAP(PackageIAP[] packageIAP)
        {
            return;
            foreach (PackageIAP item in packageIAP)
            {
                if (item == null || item._id == null || item._id == "") continue;
                if (this.PackageIAP.Contains(item._id))
                {
                    this.PackageIAP.Get(item._id).Update(item);
                }
                else
                {
                    this.PackageIAP.Add(item._id, item);
                }
            }
        }

        public void Buy(string productId, string packageId, Action<bool> done = null)
        {
            IAP_Controller.Instance.Purchase(productId, packageId, done);
        }


        #endregion

        #region API
        #endregion

        #region Getter
        public PackageIAPData GetPackageIAPData(string index)
        {
            return this.PackageIAPData.Get(index);
        }

        public PackageIAPUI GetPackageIAPUI(string index)
        {
            return this.PackageIAPUIsDic.Get(index);
        }

        public BtnPackageIAP GetBtnPackageIAP(string index)
        {
            return this.BtnPackageIAPsDic.Get(index);
        }

        public List<PackageIAP> GetPackageIAPAvailable()
        {
            List<PackageIAP> packageIAPs = new List<PackageIAP>();
            foreach (PackageIAP item in this.PackageIAP.ToList())
            {
                if (item.IsBought) continue;
                if (item.ExpiredTime > 0 && item.ExpiredTime < ServerManager.Instance.GetTimeServer())
                {
                    continue;
                }
                packageIAPs.Add(item);
            }
            return packageIAPs;
        }

        public bool IsPurchaseRemoveAds(){
            return this.PackageIAP.Get(IndexPackageIAP.RemoveAds).IsBought;
        }
        #endregion
    }
}