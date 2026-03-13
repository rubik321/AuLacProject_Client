using System.Collections.Generic;
using NTPackage.Functions;
using SimpleJSON;
using UnityEngine;

namespace Rubik.DataCenter
{
    [System.Serializable]
    public class HolderData{
        public DataName DataName;
        public string Data;
    }

    public class DataCenterHolder : NTBehaviour
    {
        public DataVersion DataVersion;
        public TextAsset DataCenterHolderText;
        public List<HolderData> HolderDataList;

        [NTButton]
        public void LoadDataCenterHolder()
        {
            this.DataVersion = new DataVersion();
            this.HolderDataList = new List<HolderData>();
            JSONNode jdata = JSONNode.Parse(this.DataCenterHolderText.text);
            this.DataVersion = JsonUtility.FromJson<DataVersion>(jdata["DataVersion"].ToString());
            // foreach key in jdata
            foreach (string key in jdata.Keys)
            {
                HolderData holderData = new HolderData();
                holderData.DataName = (DataName)System.Enum.Parse(typeof(DataName), key);
                holderData.Data = jdata[key].ToString();
                this.HolderDataList.Add(holderData);
            }
        }
    }
}