using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTFunctions_old;
using DG.Tweening;
using NTPackage_old.UI;

namespace GOA.WorldMap
{
    public class BtnPortalTeleport : NTButtonEffect
    {
        public void OnClick()
        {
            PortalTeleportUI portalTeleportUI = (PortalTeleportUI)UIManager.instance.GetPopupUIByCode(PopupCode.PortalTeleportUI);
            if (portalTeleportUI == null) return;
            portalTeleportUI.OnUI();
            this.OnSound();
        }

        protected void OnSound()
        {
            Rubik.Common.AudioHelper.AudioCtrl.Instance.Play(Rubik.Common.AudioHelper.AudioName.UI_Button_Worldmap);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            transform.DOComplete();
            transform.localScale = Vector3.zero;
            transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.2f).OnComplete(() =>
            {
                transform.DOScale(new Vector3(1f, 1f, 1f), 0.1f);
            });

        }
    }
}
