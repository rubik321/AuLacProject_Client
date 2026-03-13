using System.Collections;
using System.Collections.Generic;
using NTFunctions_old;
using NTPackage_old.Functions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Rubik.GOA.Blacksmith
{
    public class BlacksmithUI : PopupUI
    {
        public void OnUI(){
            this.Show();
        }

        public void OnUIUpgradeLv()
        {
            try
            {
                Blacksmith_UpgradeUI blacksmith_UpgradeUI = (Blacksmith_UpgradeUI)UIManager.instance.GetPopupUIByCode(PopupCode.Blacksmith_UpgradeUI);
                blacksmith_UpgradeUI.OnUI();
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString(), gameObject);
            }
        }
        public void OnUIUpgradeStar()
        {
            try
            {
                Blacksmith_UpStarUI blacksmith_UpStarUI = (Blacksmith_UpStarUI)UIManager.instance.GetPopupUIByCode(PopupCode.Blacksmith_UpStarUI);
                blacksmith_UpStarUI.OnUI();
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString(), gameObject);
            }
        }
        public void OnUIFusion()
        {
            try
            {
                Blacksmith_FusionUI blacksmith_FusionUI = (Blacksmith_FusionUI)UIManager.instance.GetPopupUIByCode(PopupCode.Blacksmith_FusionUI);
                blacksmith_FusionUI.OnUI();
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString(), gameObject);
            }
        }
        public void OnUIFragment()
        {
            try
            {
                Blacksmith_FragmentUI blacksmith_FragmentUI = (Blacksmith_FragmentUI)UIManager.instance.GetPopupUIByCode(PopupCode.Blacksmith_FragmentUI);
                blacksmith_FragmentUI.OnUI();
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString(), gameObject);
            }
        }
    }
}