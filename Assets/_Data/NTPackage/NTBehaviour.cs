using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace NTPackage.Functions
{
    public class NTBehaviour : MonoBehaviour
    {
        public bool funnyCheck;
        protected virtual void Reset()
        {
            this.LoadComponents();
            this.ResetValues();
        }

        protected virtual void Awake()
        {
            //For Override
        }

        protected virtual void Start()
        {
            //For Overide
            this.LoadComponents();
        }

        protected virtual void Update()
        {
            //For Overide
        }

        protected virtual void FixedUpdate()
        {
            //For Overide
        }

        protected virtual void OnDisable()
        {
            //For Overide
        }

        protected virtual void OnEnable()
        {
            // this.LoadComponents();
            //For Overide
        }

        protected virtual void OnDestroy(){
            //For Overide
        }

        [NTButton]
        public virtual void LoadComponents()
        {
            //For Overide
        }

        public virtual void ResetValues()
        {
            //For Overide
        }

        public virtual void SetActive(bool visible = true)
        {
            gameObject.SetActive(visible);
        }

        public virtual void SetName(string _name)
        {
            transform.name = _name;
        }

        public virtual void SetParent(Transform parent)
        {
            transform.SetParent(parent);
        }

        public virtual void ResetPosition()
        {
            transform.localPosition = Vector3.zero;
        }
        public virtual void ResetScale()
        {
            transform.localScale = Vector3.one;
        }

        public Transform FindByPath(string path)
        {
            string[] nameComs = path.Split('/');
            Transform comp = transform;
            foreach (string item in nameComs)
            {
                try
                {
                    comp = comp.Find(item);
                }
                catch (System.Exception)
                {
                    Debug.LogWarning("Not found: " + item, gameObject);
                    return null;
                }
            }
            return comp;
        }
    }
}
