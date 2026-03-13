using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTPackage.Functions;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.Events;


namespace NTPackage.UI
{
    public class NTButtonEffect : NTBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, ISubmitHandler
    {
        public bool AvailableClick = true;
        public Vector3 SizeUp = new Vector3(1.05f, 1.05f, 1.05f);
        public Vector3 SizeDown = new Vector3(0.95f, 0.95f, 0.95f);

        public Transform Main;
        public Transform Light;
        public Transform Dark;
        public bool MoreTrans = false;
        [NTShowIf("MoreTrans", true)]
        public List<Transform> MainSub = new List<Transform>();
        [NTShowIf("MoreTrans", true)]
        public List<Transform> LightSub = new List<Transform>();
        [NTShowIf("MoreTrans", true)]
        public List<Transform> DarkSub = new List<Transform>();

        [NTShowIf("AvailableClick")]
        public UnityEvent Onclick;
        [NTShowIf("AvailableClick")]
        public UnityEvent Onhold;
        [NTShowIf("AvailableClick")]
        public UnityEvent OnMouseUp;
        [NTShowIf("AvailableClick")]
        public UnityEvent OnMouseDown;

        public Coroutine HoldCor;

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (!this.AvailableClick) return;
            if (this.HoldCor != null) StopCoroutine(this.HoldCor);
            this.HoldCor = StartCoroutine(this.Holding());
            if (this.Main == null) return;
            this.Main.DOScale(this.SizeDown, 0.1f);
            foreach (Transform sub in this.MainSub)
            {
                sub.DOScale(this.SizeDown, 0.1f);
            }
            this.OnMouseDown?.Invoke();
        }

        IEnumerator Holding()
        {
            yield return new WaitForSeconds(0.5f);
            this.Onhold.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!this.AvailableClick) return;
            if (this.HoldCor != null) StopCoroutine(this.HoldCor);
            if (this.Main == null) return;
            this.Main.DOScale(this.SizeUp, 0.1f).OnComplete(() =>
            {
                this.Main.DOScale(new Vector3(1f, 1f, 1f), 0.05f);
            });
            foreach (Transform sub in this.MainSub)
            {
                if (sub == null) continue;
                sub.DOScale(this.SizeUp, 0.1f).OnComplete(() =>
                {
                    sub.DOScale(new Vector3(1f, 1f, 1f), 0.05f);
                });
            }
            this.OnMouseUp?.Invoke();
        }

        public virtual void Chose()
        {
            if (this.Light != null && this.Dark != null)
            {
                this.Light.gameObject.SetActive(true);
                this.Dark.gameObject.SetActive(false);
            }
            foreach (Transform sub in this.LightSub)
            {
                if (sub == null) continue;
                sub.gameObject.SetActive(true);
            }
            foreach (Transform sub in this.DarkSub)
            {
                if (sub == null) continue;
                sub.gameObject.SetActive(false);
            }
        }
        public virtual void Unchose()
        {
            if (this.Light != null && this.Dark != null)
            {
                this.Dark.gameObject.SetActive(true);
                this.Light.gameObject.SetActive(false);
            }
            foreach (Transform sub in this.LightSub)
            {
                if (sub == null) continue;
                sub.gameObject.SetActive(false);
            }
            foreach (Transform sub in this.DarkSub)
            {
                if (sub == null) continue;
                sub.gameObject.SetActive(true);
            }
        }

        public bool IsChose()
        {
            if (this.Light == null) return false;
            return this.Light.gameObject.activeSelf;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!this.AvailableClick) return;
            this.Onclick.Invoke();
        }

        public void OnSubmit(BaseEventData eventData)
        {
            // throw new NotImplementedException();
        }
    }
}
