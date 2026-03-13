using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTFunctions_old;

namespace GOA.WorldMap{
    public class MobAssets : LoadBehaviour
    {
        public Dictionary<string, MobSO> MobSODictionary = new Dictionary<string, MobSO>();
        public List<MobSO> MobSOs;

        public static MobAssets instance;
        protected override void Awake()
        {
            base.Awake();
            if (MobAssets.instance != null) Debug.LogError("Only 1 instance allow");
            MobAssets.instance = this;
            this.MobSODictionary.Clear();
            foreach (MobSO item in MobSOs)
            {
                this.MobSODictionary.Add(item.Index, item);
            }
        }
        public MobSO MobSOSample;
        public MobSO GetMobScriptableObjectByIndex(string index){
            try{
                return this.MobSODictionary[index];
            }catch(System.Exception e){
                Debug.LogWarning(e);
                return this.MobSOSample;
            }
        }
    }
}
