using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NTPackage_old.Functions{
    [System.Serializable]
    public class NTDictionary<K, V>{
        public Dictionary<K, V> Dictionary = new Dictionary<K, V>();
        #if UNITY_EDITOR
            public List<K> Keys = new List<K>();
            public List<V> Values = new List<V>();
        #endif

        public void Clear(){
            NTPackage_old.Functions.NTLog.LogMessage("NTDictionany Clear", null);
            this.Dictionary.Clear();
            #if UNITY_EDITOR
                this.Keys.Clear();
                this.Values.Clear();
            #endif
        }
        
        public void Add(K key,V value){
            this.Dictionary[key] = value;
            #if UNITY_EDITOR
                K key_1 = this.Keys.Find((respone)=>(respone.Equals(key)));
                if(key_1 != null){
                    int index = this.Keys.IndexOf(key_1);
                    try
                    {
                        this.Values[index] = value;
                    }
                    catch (System.Exception)
                    {}
                    return;
                }
                this.Keys.Add(key);
                this.Values.Add(value);
            #endif
        }

        public V Get(K key)
        {
            try
            {
                return this.Dictionary[key];
            }
            catch (System.Exception)
            {
                return default(V);
            }
        }

        public void Remove(K key){
            this.Dictionary.Remove(key);
        }

        public List<V> ToList(){
            List<V> list = new List<V>();
            foreach (KeyValuePair<K,V> item in this.Dictionary)
            {
                list.Add(item.Value);
            }
            return list;
        }
    }
 
 }