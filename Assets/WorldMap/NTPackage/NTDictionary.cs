using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NTPackage_old.Functions{
    [System.Serializable]
    public class NTDictionary<T>{
        public Dictionary<string, T> Dictionary = new Dictionary<string, T>();
        #if UNITY_EDITOR
            [SerializeField]
            private List<string> Keys = new List<string>();
            [SerializeField]
            private List<T> Values = new List<T>();
        #endif

        public void Clear(){
            this.Dictionary.Clear();
            #if UNITY_EDITOR
                this.Keys.Clear();
                this.Values.Clear();
            #endif
        }

        public void Add(string key,T value){
            this.Dictionary[key] = value;
            #if UNITY_EDITOR
                string key_1 = this.Keys.Find((respone)=>(respone.Equals(key)));
                if(key_1 != null) return;
                this.Keys.Add(key);
                this.Values.Add(value);
            #endif
        }

        public T Get(string key)
        {
            try
            {
                return this.Dictionary[key];
            }
            catch (System.Exception)
            {
                return default(T);
            }
        }

        public void Remove(string key){
            this.Dictionary.Remove(key);
        }

        public List<T> ToList(){
            List<T> list = new List<T>();
            foreach (KeyValuePair<string,T> item in this.Dictionary)
            {
                list.Add(item.Value);
            }
            return list;
        }

        public int Count(){
            if(this.Dictionary == null) this.Dictionary = new Dictionary<string,T>();
            return this.Dictionary.Count;
        }
    } 
}