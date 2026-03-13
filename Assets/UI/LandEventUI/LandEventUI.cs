using System.Collections;
using System.Collections.Generic;
using NTFunctions_old;
using UnityEngine;

namespace GOA.LandEvent
{
    public class LandEventUI : PopupUI
    {
        public List<LandEventItemUI> LandEventItemUIs;

        public override void LoadComponents()
        {
            base.LoadComponents();
            this.lvUI = new PopupLv().GetValue(transform.name);
            this.LoadLandEventItemUIs();
        }

        protected void LoadLandEventItemUIs()
        {
            this.LandEventItemUIs.Clear();
            foreach (Transform item in transform.Find("Panel").Find("Content").Find("Scroll View").Find("Viewport").Find("Content"))
            {
                if (item.TryGetComponent<LandEventItemUI>(out LandEventItemUI landEventItemUI))
                {
                    this.LandEventItemUIs.Add(landEventItemUI);
                }
            }
        }

        public void OnUI()
        {
            if (!this.CanShow()) return;
            this.Show();
        }

        public override void UpdateData()
        {
            List<LandEventData> landEventDatas = LandEventManager.instance.LandGlobalEventDataDic.ToList();
            for (int i = 0; i < LandEventItemUIs.Count; i++)
            {
                try
                {
                    this.LandEventItemUIs[i].SetData(landEventDatas[i]);
                }
                catch (System.Exception)
                {
                    this.LandEventItemUIs[i].gameObject.SetActive(false);
                }
            }
        }
    }

}
