using System.Collections;
using System.Collections.Generic;
using NTPackage.Functions;
using UnityEngine;

namespace NTPackage.UI{
    public class MultiTabUI : NTBehaviour
    {
        public List<TabUI> tabUIs;
        public List<BtnTab> btnTabs;

        public override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadTabUI();
            this.LoadBtnTab();
        }

        protected void LoadTabUI(){
            if(this.tabUIs != null && this.tabUIs.Count > 0) return;
            this.tabUIs.Clear();
            int index = 0;
            foreach (TabUI tabUI in transform.GetComponentsInChildren<TabUI>(true))
            {
                this.tabUIs.Add(tabUI);
                tabUI.multiTab = this;
                tabUI.number = index;
                index ++;
            }
        }

        protected void LoadBtnTab(){
            if(this.btnTabs != null && this.btnTabs.Count > 0) return;
            this.btnTabs.Clear();
            int index = 0;
            foreach (BtnTab btnTab in transform.GetComponentsInChildren<BtnTab>())
            {
                this.btnTabs.Add(btnTab);
                btnTab.multiTab = this;
                btnTab.number = index;
                index ++;
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            this.ResetData();
        }

        //Function
        public void OnUI(int tabIndex = 0){
            this.OffAllTab();
            this.BtnTabOnclick(tabIndex);
        }

        public void ResetData(){
            foreach (TabUI item in this.tabUIs)
            {
                item.ResetData();
            }
        }

        public virtual void BtnTabOnclick(int number){
            TabUI tabUI = this.GetTabUIByNumber(number);
            if(tabUI == null) return;
            BtnTab btnTab = this.GetBtnTabByNumber(number);
            this.OffAllTab(tabUI);
            tabUI.OnUI();
            btnTab.Chose();
        }

        public void OffAllTab(TabUI exceptTabUI = null){
            foreach (TabUI tabUI in this.tabUIs)
            {
                if(tabUI == exceptTabUI) continue;
                tabUI.OffUI();
            }
            foreach (BtnTab btnTab in this.btnTabs)
            {
                btnTab.Unchose();
            }
        }
        
        protected TabUI GetTabUIByNumber(int number){
            return this.tabUIs.Find((tabUI) => (tabUI.number == number));
        }
        protected BtnTab GetBtnTabByNumber(int number){
            return this.btnTabs.Find((btnTab) => (btnTab.number == number));
        }
    }
}
