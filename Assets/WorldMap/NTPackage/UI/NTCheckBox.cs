using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTPackage_old.Functions;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using NTFunctions_old;

namespace NTPackage_old.UI
{
    public class NTCheckBox : LoadBehaviour
    {
        public List<Transform> CheckTrans;
        public List<Transform> UnCheckTrans;

        public bool IsCheck = false;

        protected override void OnEnable()
        {
            base.OnEnable();
            this.UnCheck();
        }

        public virtual void Check()
        {
            Debug.LogWarning("Check");
            foreach (Transform item in CheckTrans)
            {
                item.gameObject.SetActive(true);
            }
            foreach (Transform item in UnCheckTrans)
            {
                item.gameObject.SetActive(false);
            }
            this.IsCheck = true;
        }

        public virtual void UnCheck()
        {
            Debug.LogWarning("UnCheck");
            foreach (Transform item in CheckTrans)
            {
                item.gameObject.SetActive(false);
            }
            foreach (Transform item in UnCheckTrans)
            {
                item.gameObject.SetActive(true);
            }
            this.IsCheck = false;
        }
    }
}