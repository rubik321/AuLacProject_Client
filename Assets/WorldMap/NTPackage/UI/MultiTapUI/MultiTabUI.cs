using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NTFunctions_old{
    public class MultiTabUI : LoadBehaviour
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
            this.tabUIs.Clear();
            int index = 0;
            foreach (Transform trans in transform.Find("Tabs"))
            {
                TabUI tabUI = trans.GetComponent<TabUI>();
                if(tabUI == null) continue;
                this.tabUIs.Add(tabUI);
                tabUI.multiTab = this;
                tabUI.number = index;
                index ++;
            }
        }

        protected void LoadBtnTab(){
            this.btnTabs.Clear();
            int index = 0;
            foreach (Transform trans in transform.Find("BtnTabs"))
            {
                BtnTab btnTab = trans.GetComponent<BtnTab>();
                if(btnTab == null) continue;
                btnTab.multiTab = this;
                this.btnTabs.Add(btnTab);
                btnTab.number = index;
                index ++;
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            this.ResetData();
            this.BtnTabOnclick(0);
        }

        //Function
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
            this.OffAllTab();
            tabUI.OnUI();
            btnTab.OnUI();
        }

        protected void OffAllTab(){
            foreach (TabUI tabUI in this.tabUIs)
            {
                tabUI.OffUI();
            }
            foreach (BtnTab btnTab in this.btnTabs)
            {
                btnTab.OffUI();
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
