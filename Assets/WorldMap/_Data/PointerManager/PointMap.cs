using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOA.WorldMap;
using NTFunctions_old;
using NTPackage_old.EventDispatcher;
using UnityEngine.UI;

namespace GOA.WorldMap
{
    public class PointMap : LoadBehaviour
    {
        public Transform NearView;
        public Transform FarView;

        public Image Icon;

        public override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadIcon();
        }

        protected void LoadIcon(){
            if(this.Icon != null) return;
            try
            {
                this.Icon = this.FindByPath("NearView/Canvas/Icon/mask/ava").GetComponent<Image>();
            }
            catch (System.Exception e){
                Debug.LogWarning(e, gameObject);
            }
        }

        protected override void Start()
        {
            base.Start();
            string id = NTFunction.GenerateId();
            EventListenerManager.instance.Register(EventCode.ChangeNormalView, "PointMap" + id, ChangeNormalView);
            EventListenerManager.instance.Register(EventCode.ChangeRegionView, "PointMap" + id, ChangeRegionView);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            this.UpdateData();
            this.ChangeNormalView();
            try
            {
                if(GameMaster.instance.viewModeCode == ViewModeCode.god){
                    this.ChangeRegionView();
                }else{
                    ChangeNormalView();
                }
            }
            catch (System.Exception)
            {
                ChangeNormalView();
            }
        }

        public void ChangeNormalView(object data = null)
        {
            try
            {
                this.NearView.transform.gameObject.SetActive(false);
                this.FarView.transform.gameObject.SetActive(false);
                transform.parent.Find("Skin").gameObject.SetActive(true);
                this.NearView.transform.rotation = Quaternion.identity;
            }
            catch (System.Exception) { }
        }
        public void ChangeRegionView(object data = null)
        {
            try
            {
                this.NearView.gameObject.SetActive(true);
                transform.parent.Find("Skin").gameObject.SetActive(false);
                this.NearView.transform.rotation = Quaternion.identity;
            }
            catch (System.Exception) { }
        }

        public void UpdateData()
        {
        }
    }
}
