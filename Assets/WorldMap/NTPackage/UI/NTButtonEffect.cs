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
    public class NTButtonEffect : LoadBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public Vector3 SizeUp = new Vector3(1.05f, 1.05f, 1.05f);
        public Vector3 SizeDown = new Vector3(0.95f, 0.95f, 0.95f);
        public Transform Main;

        public Transform Light;
        public Transform Dark;

        public UnityEvent Onclick;
        public UnityEvent Onhold;

        public bool IsClick = false;
        public bool IsHold = false;

        public Vector2 MouseDownPos;

        public Coroutine HoldCor;
        public bool IsIn = false;

        public void OnPointerDown(PointerEventData eventData)
        {
            this.IsClick = true;
            this.IsHold = false;
            this.MouseDownPos = Input.mousePosition;
            if (this.HoldCor != null) StopCoroutine(this.HoldCor);
            this.HoldCor = StartCoroutine(this.Holding());
            if (this.Main == null) return;
            this.Main.DOScale(this.SizeDown, 0.1f);
        }

        IEnumerator Holding()
        {
            this.IsHold = true;
            yield return new WaitForSeconds(0.5f);
            if (this.IsClick && this.IsHold)
            {
                CheckIn();
                this.IsClick = false;
                this.IsHold = false;
                if (this.IsIn) this.Onhold.Invoke();
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            CheckIn();
            if (this.HoldCor != null) StopCoroutine(this.HoldCor);
            if (this.Main == null) return;
            this.Main.DOScale(this.SizeUp, 0.1f).OnComplete(() =>
            {
                this.Main.DOScale(new Vector3(1f, 1f, 1f), 0.05f);
                if (this.IsClick)
                {
                    this.IsClick = false;
                    this.IsHold = false;
                    if (this.IsIn) this.Onclick.Invoke();
                }
            });
        }

        public virtual void Chose()
        {
            if (this.Light != null && this.Dark != null)
            {
                this.Light.gameObject.SetActive(true);
                this.Dark.gameObject.SetActive(false);
            }
        }
        public virtual void UnChose()
        {
            if (this.Light != null && this.Dark != null)
            {
                this.Dark.gameObject.SetActive(true);
                this.Light.gameObject.SetActive(false);
            }
        }

        public bool IsChose()
        {
            if (this.Light == null) return false;
            return this.Light.gameObject.activeSelf;
        }

        public void CheckIn()
        {
            Vector2 pos = Input.mousePosition;
            if (Mathf.Abs(pos.x - this.MouseDownPos.x) > 1) { this.IsIn = false; return; }
            if (Mathf.Abs(pos.y - this.MouseDownPos.y) > 1) { this.IsIn = false; return; }
            this.IsIn = true;
        }
    }
}
