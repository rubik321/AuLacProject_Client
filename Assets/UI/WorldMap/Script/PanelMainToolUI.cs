using System.Collections;
using System.Collections.Generic;
using GOA.Building;
using GOA.WorldMap;
using GoShared;
using NTPackage.Functions;
using NTPackage.UI;
using Rubik.UI;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace GOA.UIWorldMap
{
    public class PanelMainToolUI : NTBehaviour
    {
        public static PanelMainToolUI instance;

        public Transform Bottom;
        public Transform SideRightBottom;
        public Transform LeftBottonSide;
        public Transform BuildingMenu;

        public TextMeshProUGUI EventBanner;

        public Transform BtnGlobalPortal;
        public Transform BtnBackLocal;

        [SerializeField]
        protected Transform _btnLandEvent;
        public Transform BtnLandEvent{
            get {
                if(this._btnLandEvent == null) this.LoadBtnLandEvent();
                return this._btnLandEvent;
            }
        }
        [SerializeField]
        protected Transform _btnRankLandEvent;
        public Transform BtnRankLandEvent{
            get {
                if(this._btnRankLandEvent == null) this.LoadBtnRankLandEvent();
                return this._btnRankLandEvent;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            if (PanelMainToolUI.instance != null){
               Debug.LogWarning("Only 1 instance allow");
               return;
             }
            PanelMainToolUI.instance = this;
        }

        [Button]
        public override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadBtnLandEvent();
            this.LoadBtnRankLandEvent();
        }

        protected void LoadBtnLandEvent(){
            if(this._btnLandEvent != null) return;
            this._btnLandEvent = transform.Find("RightUpperSide").Find("BtnLandEvent");
        }
        protected void LoadBtnRankLandEvent(){
            if(this._btnRankLandEvent != null) return;
            this._btnRankLandEvent = transform.Find("LeftUpperSide").Find("BtnRankLandEvent");
        }

        public void OnBuilding()
        {   
            this.Bottom.gameObject.SetActive(false);
            this.SideRightBottom.gameObject.SetActive(false);
            this.LeftBottonSide.gameObject.SetActive(false);
            this.BuildingMenu.gameObject.SetActive(true);
            BuildingManager.instance.Build(true);
        }

        public void OffBuilding()
        {
            this.Bottom.gameObject.SetActive(true);
            this.SideRightBottom.gameObject.SetActive(true);
            this.LeftBottonSide.gameObject.SetActive(true);
            this.BuildingMenu.gameObject.SetActive(false);
            BuildingManager.instance.OffBuild();
            BuildingManager.instance.Build(false);
        }

        public void TeleportGlobalPortal(){
            PopupManager.Instance.OnUI(PopupCode.MessageOptionPanel, null, (popup)=>{
                MessageOptionPanel messageOptionPanel = (MessageOptionPanel)popup;
                messageOptionPanel.SetData(
                    Lean.Localization.LeanLocalization.GetTranslationText("teleport_global_portal", "Global Portal Teleport"),
                    Lean.Localization.LeanLocalization.GetTranslationText("teleport_global_portal_des", "Do you wish to channel your power and teleport to the Global Portal? Once there, mighty gatekeepers await, and only the brave shall claim the rewards.")
                );
                messageOptionPanel.SetActionConfirm(()=>{
                    TeleportSystem.instance.TestTeleport();
                }, Lean.Localization.LeanLocalization.GetTranslationText("agree", "Agree"));
                messageOptionPanel.SetActionReject(()=>{
                    messageOptionPanel.OffUI();
                }, Lean.Localization.LeanLocalization.GetTranslationText("cancel", "Cancel"));
            });
        }

        public void TeleportBackLocal(){
            TeleportSystem.instance.ComeBack();
        }
    }
}
