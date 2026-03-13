using Lean.Localization;
using NTPackage.UI;
using Rubik.UI;
using Rubik.UserProfile;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.PlayerMail
{
    public class BtnPlayerMail : NTButtonEffect
    {
        public Transform TransNotic;
        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
            this.TransNotic.gameObject.SetActive(PlayerMailManager.instance.IsNoticMail);
        }

        public void _OnClick(){
            if (UserProfileManager.Instance.IsFunctionLocked(LockFunctionType.Mail))
            {
                PopupManager.Instance.OnUI(PopupCode.MessagePanel, null, (PopupUI popupUI) =>
                {
                    popupUI.GetComponent<MessagePanel>().SetData(LeanLocalization.GetTranslationText("lock_title", "Feature Locked"), string.Format(LeanLocalization.GetTranslationText("unlock_feature", "Feature unlocks at Level {0}"), UserProfileManager.Instance.GetLevelUnlockFunction(LockFunctionType.Mail)));
                });
                return;
            }
            PopupManager.Instance.OnUI(PopupCode.PlayerMailUI);
        }
    }
}
