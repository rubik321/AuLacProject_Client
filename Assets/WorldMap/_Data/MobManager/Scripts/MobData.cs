using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoShared;
using Newtonsoft.Json;

namespace GOA.WorldMap
{
    [System.Serializable]
    public class MobData
    {
        public Coordinates coordinates;
        public string Index;
        public int Lv;
        public string SpawnTime;

        [SerializeField]
        [JsonIgnore]
        private MobSO _mobSO;
        public MobSO MobSO{
            get {
                if(this._mobSO == null) this._mobSO = MobManager.instance.GetMobScriptableObjectByIndex(this.Index);
                return this._mobSO;
            }
            set{
                this._mobSO = value;
            }
        }

        public MobData(){
            this.coordinates = new Coordinates(0d, 0d);
        }
        
        public MobData (Coordinates coordinates, string index){
            this.coordinates = coordinates;
            this.Index = index;
        }

    }
}