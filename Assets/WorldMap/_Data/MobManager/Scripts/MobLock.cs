using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTFunctions_old;
using NTPackage.UI;
using Rubik.Myrk;
using Rubik.Myrk.Monster;

namespace GOA.WorldMap
{
    public class MobLock : LoadBehaviour
    {
        public MobCtrl mobCtrl;

        public override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadMobCtrl();
        }

        protected void LoadMobCtrl()
        {
            if (mobCtrl != null) return;
            this.mobCtrl = transform.parent.GetComponent<MobCtrl>();
        }

        private static float timeHold = 0.2f;
        private float countTimeHold = 0;
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            if (countTimeHold < 0) return;
            this.countTimeHold -= Time.fixedDeltaTime;
        }

        public void Chose()
        {
            this.countTimeHold = timeHold;
        }

        public void ChoseMob()
        {
            this.Chose();
            this.OnMouseUp();
        }

        void OnMouseUp()
        {
            if (this.countTimeHold <= 0) return;
            // ViewInforMob viewInforMob = (ViewInforMob) UIManager.instance.GetPopupUIByCode(PopupCode.ViewInforMobUI);
            // viewInforMob.OnUI(this.mobCtrl.mob);
            PopupManager.Instance.OnUI(NTPackage.UI.PopupCode.MonsterOnMapUI, null, (popup) =>
            {
                MonsterOnMapUI mosterOnMapUI = (MonsterOnMapUI)popup;
                mosterOnMapUI.SetData(this.mobCtrl.mob.MonsterOnMapData);
            });
        }
    }
}
