using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GOA.LandEvent;
using NTFunctions_old;
using NTPackage_old.UI;
using UnityEngine;

namespace GOA.WorldMap
{
    public class BtnLandEvent : NTButtonEffect
    {
        public void OnClick()
        {
            LandEventManager.instance.GetLandEvent(()=>{
                LandEventUI landEventUI = (LandEventUI)UIManager.instance.GetPopupUIByCode(PopupCode.LandEventUI);
                landEventUI.OnUI();
            });
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