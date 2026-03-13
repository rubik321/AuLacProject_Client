using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using NTPackage.Functions;

using NTPackage.EventDispatcher;
using Rubik.Common.AudioHelper;
using Unity.VisualScripting;

namespace NTPackage.UI
{
    public enum KindPopup
    {
        none = 0,
        oneStep = 1,
        twoStep = 2,
        threeStep = 3,
        moveTo = 4,
        fade = 5,
    }

    public class PopupUI : NTBehaviour
    {
        public PopupCode popupCode = PopupCode.Unknown;
        public Transform transPanel;

        public KindPopup show = KindPopup.none;
        public KindPopup hide = KindPopup.none;

        public Transform ScreenDim;

        [NTShowIf("show", (int)KindPopup.moveTo)]
        public RectTransform TransShowMove;
        [NTShowIf("show", (int)KindPopup.moveTo)]
        [HideInInspector] public List<RectTransform> ShowPos;
        [NTShowIf("show", (int)KindPopup.moveTo)]
        [HideInInspector] public float DurationMoveShow = 0.2f;
        [NTShowIf("hide", (int)KindPopup.moveTo)]
        public RectTransform TransHideMove;
        [NTShowIf("hide", (int)KindPopup.moveTo)]
        [HideInInspector] public List<RectTransform> HidePos;
        [NTShowIf("hide", (int)KindPopup.moveTo)]
        [HideInInspector] public float DurationMoveHide = 0.2f;
        public Coroutine Coroutine;

        public double LastTimeOpen;
        public bool IsBlockDoubleClickOffUI = false;

        public override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadTransPanel();
            this.popupCode = PopupCodeParser.FromString(transform.name);
        }

        protected void LoadTransPanel()
        {
            if (this.transPanel != null) return;
            try
            {
                this.transPanel = transform.Find("Panel");
            }
            catch (System.Exception)
            {
                Debug.LogWarning("Can't LoadImageItem");
            }

        }
        //Function
        protected override void Start()
        {
            base.Start();
            PopupManager.Instance.PopupDic.Add(this.popupCode, this);
            this.OffUIWithoutSound();
        }

        public Action ActionOnUI;
        public virtual void OnUI(object data = null, bool isDefaultSound = true)
        {
            this.LastTimeOpen = Time.time;
            PopupManager.Instance.LsPopupUIOn.Add(this);
            this.Show();
            if (isDefaultSound) this.ShowSound();
            this.UpdateData();
        }

        public virtual void OffUIWithoutSound()
        {
            // return if open popup in 0.1s
            if (this.IsBlockDoubleClickOffUI && Time.time - this.LastTimeOpen < 0.5f) return;
            try
            {
                this.ActionOffUI.Invoke();
                this.ActionOffUI = null;
            }
            catch (System.Exception) { }
            PopupManager.Instance.LsPopupUIOn.Remove(this);
            this.Hide(false);
        }

        public Action ActionOffUI;
        public virtual void OffUI()
        {
            // return if open popup in 0.1s
            if (this.IsBlockDoubleClickOffUI && Time.time - this.LastTimeOpen < 0.5f) return;
            this.ScriptOffUI();
        }

        public virtual void ScriptOffUI()
        {
            try
            {
                this.ActionOffUI.Invoke();
                this.ActionOffUI = null;
            }
            catch (System.Exception) { }
            PopupManager.Instance.LsPopupUIOn.Remove(this);
            this.Hide(true);
        }

        public virtual void UpdateData(object data = null) { }

        public virtual void Show()
        {
            if (this.ScreenDim != null) this.ScreenDim.gameObject.SetActive(true);
            if (this.IsShow())
            {
                this.ShowNone();
                return;
            }
            switch (show)
            {
                case KindPopup.oneStep:
                    this.ShowOneDG();
                    break;
                case KindPopup.twoStep:
                    this.ShowDoubleDG();
                    break;
                case KindPopup.threeStep:
                    this.ShowTrippleDG();
                    break;
                case KindPopup.moveTo:
                    this.ShowMoveTo();
                    break;
                case KindPopup.fade:
                    this.ShowFade();
                    break;
                default:
                    this.ShowNone();
                    break;
            }
            try
            {
                // UIManager.instance.UpdateData();
            }
            catch (System.Exception) { }
        }

        protected virtual void ShowSound()
        {
            AudioCtrl.Instance.Play(AudioName.UI_Popup_Panel_Open_Default);
        }

        [NTButton]
        public virtual void ShowNone()
        {
            transform.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            this.transPanel.localScale = new Vector3(1, 1, 1);
            this.transPanel.gameObject.SetActive(true);
        }
        public virtual void ShowOneDG()
        {
            transform.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            this.transPanel.localScale = new Vector3(0, 0, 0);
            this.transPanel.gameObject.SetActive(true);
            this.transPanel.DOScale(new Vector3(1f, 1f, 1f), 0.3f);
        }

        public virtual void ShowDoubleDG()
        {
            transform.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            this.transPanel.localScale = new Vector3(0, 0, 0);
            this.transPanel.gameObject.SetActive(true);
            this.transPanel.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.2f).OnComplete(() =>
            {
                this.transPanel.DOScale(new Vector3(1f, 1f, 1f), 0.1f);
            });
        }

        public virtual void ShowTrippleDG()
        {
            transform.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            this.transPanel.localScale = new Vector3(0, 0, 0);
            this.transPanel.gameObject.SetActive(true);
            this.transPanel.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.2f).OnComplete(() =>
            {
                this.transPanel.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.06f).OnComplete(() =>
                {
                    this.transPanel.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.04f);
                });
            });
        }

        public virtual void ShowMoveTo()
        {
            transform.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            this.transPanel.localScale = new Vector3(1, 1, 1);
            this.transPanel.gameObject.SetActive(true);
            this.TransShowMove.DOComplete();
            if (this.Coroutine != null) StopCoroutine(this.Coroutine);
            this.Coroutine = StartCoroutine(this.MoveTo(this.DurationMoveShow, ShowPos));
        }

        public virtual void ShowFade()
        {
            this.transPanel.localScale = new Vector3(0, 0, 0);
            this.transPanel.gameObject.SetActive(true);
            PopupManager.Instance.FadeAnim(() =>
            {
                this.ShowNone();
            });
        }

        public virtual void Hide(bool isDefaultSound = true)
        {
            if (this.ScreenDim != null) this.ScreenDim.gameObject.SetActive(false);
            if (!this.IsShow())
            {
                this.HideNone();
                return;
            }
            try
            {
                if (isDefaultSound) this.HideSound();
            }
            catch (System.Exception) { }
            switch (hide)
            {
                case KindPopup.oneStep:
                    this.HideOneDG();
                    break;
                case KindPopup.twoStep:
                    this.HideDoubleDG();
                    break;
                case KindPopup.moveTo:
                    this.HideMoveTo();
                    break;
                case KindPopup.fade:
                    this.HideFade();
                    break;
                default:
                    this.HideNone();
                    break;
            }
        }

        protected virtual void HideSound()
        {
            AudioCtrl.Instance.Play(AudioName.UI_Popup_Panel_Close_Default);
        }

        [NTButton]
        public virtual void HideNone()
        {
            if (this.transPanel != null)
            {
                this.transPanel.gameObject.SetActive(false);
                transform.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
                this.transPanel.localScale = new Vector3(0, 0, 0);
            }

        }

        public virtual void HideOneDG()
        {
            this.transPanel.DOScale(new Vector3(0f, 0f, 0f), 0.25f).OnComplete(() =>
            {
                this.transPanel.gameObject.SetActive(false);
                transform.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
            });
        }

        public virtual void HideDoubleDG()
        {
            this.transPanel.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.075f).OnComplete(() =>
            {
                this.transPanel.DOScale(new Vector3(0f, 0f, 0f), 0.15f).OnComplete(() =>
                {
                    this.transPanel.gameObject.SetActive(false);
                    transform.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
                });
            });
        }
        public void HideMoveTo()
        {
            this.TransShowMove.DOComplete();

            if (this.Coroutine != null) StopCoroutine(this.Coroutine);
            this.Coroutine = StartCoroutine(this.MoveTo(this.DurationMoveHide, HidePos, () =>
            {
                this.transPanel.gameObject.SetActive(false);
                transform.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
            }));
        }

        public virtual void HideFade()
        {
            PopupManager.Instance.FadeAnim(() =>
            {
                this.HideNone();
            });
        }

        public IEnumerator MoveTo(float duration, List<RectTransform> listPos, Action done = null)
        {
            this.TransShowMove.anchoredPosition = listPos[0].anchoredPosition;
            int index = 1;
            while (true)
            {
                if (index > listPos.Count - 1) break;
                this.TransShowMove.DOAnchorPos(listPos[index].anchoredPosition, duration);
                yield return new WaitForSeconds(duration);
                index++;
                duration /= 2;
            }
            done?.Invoke();
        }

        public bool IsShow()
        {
            return this.transPanel.gameObject.activeSelf;
        }
    }
}
