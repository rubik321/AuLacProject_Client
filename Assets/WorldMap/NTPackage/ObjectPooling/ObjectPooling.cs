using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NTFunctions_old;

namespace NTFunctions_old
{
    public class ObjectPooling : LoadBehaviour
    {
        public bool useObjectPooling = true;

        public Transform objectPoolingHolder;

        public override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadObjectPoolingHolder();
        }

        protected void LoadObjectPoolingHolder(){
            if(this.objectPoolingHolder != null) return;
            try
            {
                this.objectPoolingHolder = transform.Find("Trash");
            }
            catch (System.Exception)
            {
                Debug.LogWarning("Can't LoadObjectPoolingHolder");
            }
        }

        //Function

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
            // foreach (Transform transChild in trans)
            // {
            //     transChild.gameObject.SetActive(false);
            //     this.PushObjectIntoPooling(transChild);
            // }
        }

        public virtual void PushObjectIntoPooling(Transform trans){
            trans.gameObject.SetActive(false);
            trans.SetParent(this.objectPoolingHolder);
        }

        public Transform GetObjectFromPooling(string nameObject){
            if(!this.useObjectPooling) return null;
            foreach (Transform item in this.objectPoolingHolder)
            {
                if(item.name.Equals(nameObject)){
                    return item;
                }
            }
            return null;
        }
    }
}