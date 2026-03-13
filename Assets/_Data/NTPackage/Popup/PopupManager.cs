using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTPackage.Functions;
using System;
using Spine.Unity;
using Rubik.AddressablesLoader;
using Rubik.UI;
namespace NTPackage.UI
{
    public class PopupManager : NTBehaviour
    {
        public ListTransformAddressable PopupTransAddressable;
        public ListTransformAddressable HUDPopupAddressable;
        public float currentLvUI = 0;
        public NTDictionary<PopupCode, PopupUI> PopupDic = new NTDictionary<PopupCode, PopupUI>();
        public List<PopupUI> LsPopupUIOn = new List<PopupUI>();

        public static PopupManager Instance;
        public SkeletonGraphic fadeAnim;
        public Coroutine CorFade;
        protected override void Awake()
        {
            base.Awake();
            if (PopupManager.Instance != null) Debug.LogWarning("Only 1 UIManager allow");
            PopupManager.Instance = this;
            this.PopupDic = new NTDictionary<PopupCode, PopupUI>();
        }

        public override void LoadComponents()
        {
            base.LoadComponents();
        }
        //Function

        protected override void Start()
        {
            base.Start();
            this.OffAllPopupUI();
            this.LsPopupUIOn = new List<PopupUI>();
        }

        public IEnumerator LoadData()
        {
            int amount = 0;

            AddressablesLoader.Instance.LoadAssetsFromLocal(AddressablesAssetsConfig.PopupTrans, (result) =>
            {
                this.PopupTransAddressable = result.GetComponent<ListTransformAddressable>();
                this.PopupTransAddressable.transform.SetParent(transform);
                foreach (Transform item in this.PopupTransAddressable.ListTransform)
                {
                    item.SetParent(transform);
                    NTPackage.Functions.NTFunction.ResetPosition(item);
                    if (item.TryGetComponent<RectTransform>(out RectTransform rectTransform))
                    {
                        rectTransform.anchoredPosition = Vector2.zero;
                        rectTransform.sizeDelta = Vector2.one;
                    }
                }
                amount++;
            });

            AddressablesLoader.Instance.LoadAssetsFromLocal(AddressablesAssetsConfig.HUDPopup, (result) =>
            {
                this.HUDPopupAddressable = result.GetComponent<ListTransformAddressable>();
                this.HUDPopupAddressable.transform.SetParent(transform);
                foreach (Transform item in this.HUDPopupAddressable.ListTransform)
                {
                    item.SetParent(HUDCanvas.Instance.transform);
                    NTPackage.Functions.NTFunction.ResetPosition(item);
                    if (item.TryGetComponent<RectTransform>(out RectTransform rectTransform))
                    {
                        rectTransform.anchoredPosition = Vector2.zero;
                        rectTransform.sizeDelta = Vector2.one;
                    }
                }
                amount++;
            });

            yield return new WaitUntil(() => amount >= 2);

        }

        public virtual void OffAllPopupUI()
        {
            foreach (PopupUI popupUI in this.PopupDic.ToList())
            {
                popupUI.ActionOffUI = null;
                popupUI.HideNone();
            }
        }

        public PopupUI GetPopupUIByCode(PopupCode popupCode)
        {
            return this.PopupDic.Get(popupCode);
        }

        public void OnUI(PopupCode popupCode, object data = null, Action<PopupUI> action = null, bool isDefaultSound = true)
        {

           try
            {
                NTPackage.Functions.NTLog.LogMessage("OnUI:" + popupCode.ToString(), gameObject);
                PopupUI popupUI = this.GetPopupUIByCode(popupCode);
                popupUI.OnUI(data, isDefaultSound);
                popupUI.transform.SetAsLastSibling();
                action?.Invoke(popupUI);
            }
            catch (System.Exception e)
            {
                NTPackage.Functions.NTLog.LogError(popupCode + ":" + e.ToString(), gameObject);
            }
        }
        public void OffUI(PopupCode popupCode)
        {
            try
            {
                NTPackage.Functions.NTLog.LogMessage("OffUI:" + popupCode.ToString(), gameObject);
                this.GetPopupUIByCode(popupCode).OffUI();
            }
            catch (System.Exception e)
            {
                NTPackage.Functions.NTLog.LogError(popupCode + ":" + e.ToString(), gameObject);
            }
        }
        public void UpdateDataUI(PopupCode popupCode, object data = null)
        {
            try
            {
                NTPackage.Functions.NTLog.LogMessage("UpdateData:" + popupCode.ToString(), gameObject);
                this.GetPopupUIByCode(popupCode).UpdateData(data);
            }
            catch (System.Exception e)
            {
                NTPackage.Functions.NTLog.LogError(e.ToString(), gameObject);
            }
        }

        public void OffNearestPopupUI()
        {
            if (this.LsPopupUIOn.Count > 0)
            {
                this.LsPopupUIOn[this.LsPopupUIOn.Count - 1].OffUI();
            }
        }

        public void FadeAnim(Action doneFadeIn = null)
        {
            if (this.CorFade != null) StopCoroutine(this.CorFade);
            this.CorFade = StartCoroutine(this.CoFade(doneFadeIn));
        }

        public IEnumerator CoFade(Action doneFadeIn = null)
        {
            fadeAnim.gameObject.SetActive(true);
            fadeAnim.AnimationState.SetAnimation(0, "fade in", false);
            yield return new WaitForSeconds(.5f);
            doneFadeIn?.Invoke();
            fadeAnim.AnimationState.SetAnimation(0, "fade out", false);
            yield return new WaitForSeconds(.5f);
            fadeAnim.gameObject.SetActive(false);

        }

        public PopupUI GetPopupUI(PopupCode popupCode)
        {
            return this.PopupDic.Get(popupCode);
        }
    }
}
