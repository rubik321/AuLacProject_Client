using System.Collections.Generic;
using NTPackage.UI;
using UnityEngine;

namespace Rubik.Myrk.Portal
{
    using System.Collections;
    using NTPackage.EventDispatcher;
    using NTPackage.Functions;
    using Rubik.Myrk.GeoPoint;
    public class PortalSearchUI : PopupUI
    {
        public List<PortalSearchCard> PortalSearchCards = new List<PortalSearchCard>();

        public Coroutine CorUpdateData;

        [NTButton]
        public void TestOnUI(){
            this.OnUI();
        }

        public override void OnUI(object data = null, bool isDefaultSound = true)
        {
            base.OnUI(data, isDefaultSound);
            EventListenerManager.instance.RemoveListener(EventCode.UpdatePortalAttackData, "PortalSearchUI");
            if(this.CorUpdateData != null)
            {
                this.StopCoroutine(this.CorUpdateData);
            }
            this.CorUpdateData = this.StartCoroutine(this.IEUpdateData());
        }

        public override void ScriptOffUI()
        {
            base.ScriptOffUI();
            if (this.CorUpdateData != null)
            {
                this.StopCoroutine(this.CorUpdateData);
            }
        }

        public override void UpdateData(object data = null)
        {
            base.UpdateData(data);
            foreach (PortalSearchCard portalSearchCard in this.PortalSearchCards)
            {
                portalSearchCard.gameObject.SetActive(false);
            }

            for (int i = 0; i < PortalWorldMapManager.Instance.PortalRandomResponse.GeoPoints.Count; i++)
            {
                GeoPoint geoPoint = PortalWorldMapManager.Instance.PortalRandomResponse.GeoPoints[i];
                this.PortalSearchCards[i].SetData(geoPoint);
                this.PortalSearchCards[i].gameObject.SetActive(true);
            }
        }

        public IEnumerator IEUpdateData()
        {
            while (true)
            {
                yield return new WaitForSeconds(3f);
                for (int i = 0; i < PortalWorldMapManager.Instance.PortalRandomResponse.GeoPoints.Count; i++){
                    PortalWorldMapManager.Instance.SendMsgGetPortalAttackData(PortalWorldMapManager.Instance.PortalRandomResponse.GeoPoints[i].PointID);
                }
            }
        }

        public void OnUpdatePortalAttackData(long pointID)
        {
            foreach (PortalSearchCard portalSearchCard in this.PortalSearchCards)
            {
                if(portalSearchCard.gameObject.activeSelf == false) continue;
                if (portalSearchCard.GeoPoint.PointID == pointID)
                {
                    portalSearchCard.UpdateData();
                }
            }
        }
    }
}
