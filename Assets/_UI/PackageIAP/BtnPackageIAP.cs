using System.Collections;
using System.Collections.Generic;
using NTPackage.Functions;
using NTPackage.UI;
using UnityEngine;

namespace Rubik.Myrk.PackageIAP
{
    public class BtnPackageIAP : NTButtonEffect
    {
        public string PackageIAPIndex;
        public PackageIAP PackageIAP;

        public void SetData(PackageIAP packageIAP){
            this.PackageIAP = packageIAP;
        }

        public void _OnClick(){
            PopupManager.Instance.OnUI(PopupCode.PackageIAPPopupUI, this.PackageIAP, (popupUI) =>
            {
                PackageIAPPopupUI packageIAPPopupUI = popupUI as PackageIAPPopupUI;
                packageIAPPopupUI.SetData(() =>
                {
                    gameObject.SetActive(false);
                });
            });
        }
    }
}