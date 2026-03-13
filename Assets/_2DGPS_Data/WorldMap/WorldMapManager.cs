using System.Collections;
using System.Collections.Generic;
using NTPackage_old.Functions;
using SimpleJSON;
using UnityEngine;

namespace Rubik._2DGPS.WorldMap
{
    using DataCenter;
    using NTFunctions_old;

    public class WorldMapManager : LoadBehaviour
    {
        
        public static WorldMapManager instance;
        protected override void Awake()
        {
            base.Awake();
            if (WorldMapManager.instance != null){
               NTLog.LogWarning("Only 1 instance allow");
               return;
             }
            WorldMapManager.instance = this;
        }

        public List<WorldMapLvData> WorldMapLvDatas;

        public IEnumerator LoadData(){
            this.LoadCardData();
            yield return null;
        }

        public void LoadCardData(){
            this.WorldMapLvDatas = new List<WorldMapLvData>();
            JSONNode jdata = JSONNode.Parse(DataCenterManager.instance.GetData(DataName.WorldMapLvData));
            foreach (JSONNode item in jdata)
            {
                WorldMapLvData worldMapLvData = JsonUtility.FromJson<WorldMapLvData>(item.ToString());
                this.WorldMapLvDatas.Add(worldMapLvData);
            }
        }
    }
}