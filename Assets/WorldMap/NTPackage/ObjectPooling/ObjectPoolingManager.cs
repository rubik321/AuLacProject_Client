using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NTPackage_old.Functions{
    public class ObjectPoolingManager : NTFunctions_old.LoadBehaviour
    {
        public NTDictionary<List<Transform>> ObjectNTDictionary = new NTDictionary<List<Transform>>();
        public Transform Holder;

        public static ObjectPoolingManager instance;
        protected override void Awake()
        {
            base.Awake();
            if (ObjectPoolingManager.instance != null){
               Debug.LogWarning("Only 1 instance allow");
               return;
             }
            ObjectPoolingManager.instance = this;
        }

        public override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadHolder();
        }

        protected void LoadHolder(){
            if(Holder != null) return;
            this.Holder = transform.Find("Holder");
        } 

        public virtual void PushChildObjectIntoPooling(Transform trans){
            List<Transform> listTrans = new List<Transform>();
            foreach (Transform item in trans)
            {
                listTrans.Add(item);
            }
            for (int i = listTrans.Count -1; i >=0; i--)
            {
                this.PushObjectIntoPooling(listTrans[i]);
            }
        }

        public void PushObjectIntoPooling(Transform trans){
            trans.SetParent(this.Holder);
            try
            {
                this.ObjectNTDictionary.Get(trans.name).Add(trans);
            }
            catch (System.Exception e)
            {
                List<Transform> lsTrans = new List<Transform>();
                lsTrans.Add(trans);
                this.ObjectNTDictionary.Add(trans.name, lsTrans);
            }
        }

        public Transform GetObjectFromPooling(string nameOb){
            try
            {
                Transform transform = this.ObjectNTDictionary.Get(nameOb)[0];
                this.ObjectNTDictionary.Get(nameOb).RemoveAt(0);
                return transform;
            }
            catch (System.Exception e)
            {
                try
                {
                    Debug.LogError(this.ObjectNTDictionary.Get(nameOb).Count);
                }
                catch (System.Exception)
                {}
                return null;
            }
        }
        public T GetObjectFromPooling<T>(string nameOb){
            Transform trans;
            try
            {
                trans = this.ObjectNTDictionary.Get(nameOb)[0];
                this.ObjectNTDictionary.Get(nameOb).RemoveAt(0);
                return trans.GetComponent<T>();
            }
            catch (System.Exception e)
            {
                return default(T);
            }
        }
    }
}
