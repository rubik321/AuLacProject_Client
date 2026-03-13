using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace NTPackage
{
    [System.Serializable]
    public class NTDictionary<K, V>
    {
        public Dictionary<K, V> Dictionary = new Dictionary<K, V>();
        [SerializeField, ReadOnly] private List<K> Keys = new List<K>();
        [SerializeField, ReadOnly] private List<V> Values = new List<V>();

        public void Clear()
        {
            this.Dictionary.Clear();
#if UNITY_EDITOR
            this.Keys.Clear();
            this.Values.Clear();
#endif
        }

        public void Add(K key, V value)
        {
            this.Dictionary[key] = value;
#if UNITY_EDITOR
            int index = this.Keys.IndexOf(key);
            if (index != -1)
            {                
                try
                {
                    this.Values[index] = value;
                }
                catch (System.Exception) { }
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
                V data = this.Dictionary[key];
                if (data == null)
                {
                    return default(V);
                }
                return data;
            }
            catch (KeyNotFoundException)
            {
                return default(V);
            }
        }

        public bool Contains(K key)
        {
            return this.Dictionary.ContainsKey(key);
        }

        public void Remove(K key)
        {
            if(key == null) return;
            this.Dictionary.Remove(key);
#if UNITY_EDITOR
            int index = this.Keys.IndexOf(key);
            if (index != -1)
            {
                this.Keys.RemoveAt(index);
                this.Values.RemoveAt(index);
            }
#endif
        }

        public List<V> ToList()
        {
            List<V> list = new List<V>();
            foreach (KeyValuePair<K, V> item in this.Dictionary)
            {
                list.Add(item.Value);
            }
            return list;
        }

        public int Count
        {
            get => this.Dictionary.Count;
        }
    }

}