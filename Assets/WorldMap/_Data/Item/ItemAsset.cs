using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using NTPackage_old.Functions;

namespace GOA.Item{
    public class ItemAsset : NTFunctions_old.NTFunction
    {
        public NTDictionary<ItemInfoData> ItemDataDictionary2 = new NTDictionary<ItemInfoData>();
        public NTDictionary<ItemInfoData> ItemDataDictionary1 = new NTDictionary<ItemInfoData>();
        public TextAsset DataItem;

        public static ItemAsset instance;
        protected override void Awake()
        {
            base.Awake();
            if (ItemAsset.instance != null){
               Debug.LogWarning("Only 1 instance allow");
               return;
             }
            ItemAsset.instance = this;
        }

        protected override void Start()
        {
            base.Start();
            this.LoadData();
        }

        [ContextMenu("LoadData")]
        public void LoadData(){
            JSONNode dataItem = JSON.Parse(this.DataItem.text);
            for (int i = 0; i < dataItem.Count; i++)
            {
                ItemInfoData itemData = JsonUtility.FromJson<ItemInfoData>(dataItem[i].ToString());
                this.ItemDataDictionary2.Add(itemData.Index, itemData);
                this.ItemDataDictionary1.Add(itemData.Code.ToString(), itemData);
            }
        }

        public ItemInfoData GetItemDataByCode(ItemCode itemCode){
            return this.ItemDataDictionary1.Get(itemCode.ToString());
        }
    }

}
