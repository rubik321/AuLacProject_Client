using System.Collections;
using System.Collections.Generic;
using NTFunctions_old;
using NTPackage_old.Functions;
using SimpleJSON;
using UnityEngine;

namespace GOA.Gear
{
    [System.Serializable]
    public class GearInfoData
    {
        public string Index;
        public string Name;
        public string Des;
        public int Slot;
        public float WeaponType;
        public float WeaponRestricted;
        public float DMG;
        public float STR;
        public float MND;
        public float CRI;
        public float CRD;
        public float HIT;
        public float EVA;
        public float HP;
        public float MP;
        public float VIT;
        public float SPI;
        public float DEX;
        public float SPD;
        public float Class;
    }

    public class GearAsset : LoadBehaviour
    {
        public NTDictionary<GearInfoData> GearDataDictionary = new NTDictionary<GearInfoData>();
        public TextAsset DataItem;

        public static GearAsset instance;
        protected override void Awake()
        {
            base.Awake();
            if (GearAsset.instance != null)
            {
                Debug.LogWarning("Only 1 instance allow");
                return;
            }
            GearAsset.instance = this;
        }

        protected override void Start()
        {
            base.Start();
            this.LoadData();
        }

        [ContextMenu("LoadData")]
        public void LoadData()
        {
            JSONNode dataItem = JSON.Parse(this.DataItem.text);
            for (int i = 0; i < dataItem.Count; i++)
            {
                GearInfoData gearData = JsonUtility.FromJson<GearInfoData>(dataItem[i].ToString());
                this.GearDataDictionary.Add(gearData.Index, gearData);
            }
        }

        public GearInfoData GetItemDataByIndex(string index)
        {
            return this.GearDataDictionary.Get(index);
        }
    }
}
