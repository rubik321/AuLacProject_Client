using System;
using System.Collections;
using System.Collections.Generic;
using NTPackage.Functions;
using NTPackage.UI;
using TMPro;
using UnityEngine;

namespace Rubik.Myrk.PackageIAP
{
    public class PackageIAPUI : NTBehaviour
    {
        public string PackageIAPIndex;
        public PackageIAP PackageIAP;
        public Animator Animator;

        public Action OnPurchase;
        public Action OnClose;

        public TextMeshProUGUI TextPrice;

        public void SetData(PackageIAP packageIAP, Action onPurchase, Action onClose)
        {
            this.PackageIAP = packageIAP;
            this.OnPurchase = onPurchase;
            this.OnClose = onClose;
            PackageIAPData packageIAPData = PackageIAPManager.Instance.GetPackageIAPData(this.PackageIAP.Index);
            this.TextPrice.text = "N/A";
            this.TextPrice.text = IAPManager.Instance.getPriceProduct(packageIAPData.ProductId);
        }

        public void _Purchase()
        {
            PackageIAPData packageIAPData = PackageIAPManager.Instance.GetPackageIAPData(this.PackageIAP.Index); 
            if (packageIAPData == null)
            {
                // TODO: Handle error
                return;
            }
            PackageIAPManager.Instance.Buy(packageIAPData.ProductId, this.PackageIAP._id, (success) =>
            {
                if (success)
                {
                    this.OnPurchase?.Invoke();
                }
            });
        }

        public void _Close()
        {
            this.OnClose?.Invoke();
        }
    }
}