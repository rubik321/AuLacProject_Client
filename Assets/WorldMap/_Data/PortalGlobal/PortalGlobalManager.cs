using System.Collections;
using System.Collections.Generic;
using GOA.UIWorldMap;
using NTFunctions_old;
using Rubik.Chat;
using Rubik.Combat;
using SimpleJSON;
using UnityEngine;

namespace GOA.WorldMap.PortalGlobal
{
    [System.Serializable]
    public class GlobalPortalData
    {
        public double latitude;
        public double longitude;
        public int PointID = 0;
        public string Name;
        public double EndTime;
    }

    public class PortalGlobalManager : LoadBehaviour
    {
        public static PortalGlobalManager instance;
        protected override void Awake()
        {
            base.Awake();
            if (PortalGlobalManager.instance != null){
               Debug.LogWarning("Only 1 instance allow");
               return;
             }
            PortalGlobalManager.instance = this;
        }

        public void Init()
        {
            // if (GeoPointManager.Instance.GlobalPortalData == null || GeoPointManager.Instance.GlobalPortalData.PointID == 0)
            // {
            //     StartCoroutine(APIManager.Instance.GetGlobalPortal((callback) =>
            //     {
            //             // ChatManager.Instance.sendChatChannel(callback, "SystemLog");
            //             JSONNode data = JSONNode.Parse(callback);
            //             GeoPointManager.Instance.GlobalPortalData = JsonUtility.FromJson<GlobalPortalData>(data["Data"]);
            //             Debug.LogError(data["Data"].ToString());
            //             Debug.LogError(JsonUtility.ToJson(GeoPointManager.Instance.GlobalPortalData));
            //             GeoPointManager.Instance.IsInitGlobalPortal = true;
            //             this.UpdateData();
            //     }));
            // }
        }

        public void UpdateData()
        {
            PanelMainToolUI.instance.BtnGlobalPortal.gameObject.SetActive(true);
            // if (GeoPointManager.Instance.GlobalPortalData == null || GeoPointManager.Instance.GlobalPortalData.PointID == 0)
            // {
            //     PanelMainToolUI.instance.BtnGlobalPortal.gameObject.SetActive(false);
            // }
            if(ChatManager.Instance.EventChannelJoin.Equals(ChatChannelConfig.GetGlobalPortal())) PanelMainToolUI.instance.BtnGlobalPortal.gameObject.SetActive(false);
        }
    }
}
