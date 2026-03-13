using Lean.Localization;
using NTPackage.Functions;
using NTPackage.UI;
using Rubik.Myrk.Controller;
using Rubik.UI;
using Rubik.UIController;
using Rubik.UserProfile;
using UnityEngine;
using UnityEngine.UI;

namespace Rubik.Myrk.PlayerChest
{
    public class BtnPlayerChest : NTButtonEffect
    {
        public Image Lock;

        protected override void Start()
        {
            base.Start();
            if(UserProfileManager.Instance.IsFunctionLocked(LockFunctionType.Chest)){
                this.Lock.gameObject.SetActive(true);
            }
            else{
                this.Lock.gameObject.SetActive(false);
            }
        }

        public void _OnClick(){
            if(!UserProfileManager.Instance.IsFunctionLocked(LockFunctionType.Chest)){
                PopupManager.Instance.OnUI(PopupCode.PlayerChestUI);
            }
            else{
                PopupManager.Instance.OnUI(PopupCode.MessagePanel, null, (PopupUI popupUI) =>
                {
                    popupUI.GetComponent<MessagePanel>().SetData(LeanLocalization.GetTranslationText("lock_title", "Feature Locked"), string.Format(LeanLocalization.GetTranslationText("unlock_feature", "Feature unlocks at Level {0}"), UserProfileManager.Instance.GetLevelUnlockFunction(LockFunctionType.Chest)));
                });
            }
        }
    }
}