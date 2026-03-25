using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using NTPackage.Functions;
// using MinhPN;
namespace MNP.MNPopup
{
    public enum MNPopupType
    {
        none = 0,
        oneStep = 1,
        twoStep = 2,
        threeStep = 3,
        moveTo = 4,
        fade = 5,
    }

    public class MNPopup : MonoBehaviour
    {
        [Header("Popup Settings")]
        public MNPopupType popupType = MNPopupType.none;
        public GameObject popupBackground;
        public GameObject popupPanel;
        public Button closeButton;

        [Header("Animation Settings")]
        // [ShowIf("@popupType == MNPopupType.oneStep&&popupType == MNPopupType.twoStep&&popupType == MNPopupType.threeStep")]
        public float animationDuration = 0.3f;
        // [ShowIf("@popupType == MNPopupType.oneStep&&popupType == MNPopupType.twoStep&&popupType == MNPopupType.threeStep")]

        public AnimationCurve easeCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        [Header("MoveTo Animation Settings")]
        // [ShowIf("@popupType == MNPopupType.moveTo")]
        public Vector2 moveFromOffset = new Vector2(0, 500f);
        // [ShowIf("@popupType == MNPopupType.moveTo")]
        public Vector2 moveToPosition = Vector2.zero;

        private RectTransform panelRectTransform;
        private CanvasGroup backgroundCanvasGroup;
        private CanvasGroup panelCanvasGroup;
        private Vector3 originalPanelScale;
        private Vector2 originalPanelPosition;
        private Coroutine currentAnimationCoroutine;
        private bool isShowing = false;
        private bool hasStarted = false;
        private bool isAnimating = false;

        #region Unity Lifecycle

        protected virtual void Awake()
        {
            InitializeComponents();
        }

        protected virtual void Start()
        {
            SetupCloseButton();
            if (!isShowing && !hasStarted)
            {
                gameObject.SetActive(false);
                isShowing = false;
            }
            hasStarted = true;
        }
        protected virtual void OnEnable()
        {
            // SetupCloseButton();
        }
        #endregion

        private void InitializeComponents()
        {
            if (popupPanel != null)
            {
                panelRectTransform = popupPanel.GetComponent<RectTransform>();
                if (panelRectTransform != null)
                {
                    originalPanelScale = panelRectTransform.localScale;
                    originalPanelPosition = panelRectTransform.anchoredPosition;
                }

                panelCanvasGroup = popupPanel.GetComponent<CanvasGroup>();
                if (panelCanvasGroup == null)
                {
                    panelCanvasGroup = popupPanel.AddComponent<CanvasGroup>();
                }
            }

            if (popupBackground != null)
            {
                backgroundCanvasGroup = popupBackground.GetComponent<CanvasGroup>();
                if (backgroundCanvasGroup == null)
                {
                    backgroundCanvasGroup = popupBackground.AddComponent<CanvasGroup>();
                }
            }
        }

        private void SetupCloseButton()
        {
            if (closeButton != null)
            {
                closeButton.onClick.RemoveAllListeners();
                closeButton.onClick.AddListener(() =>
                {
                    if (!isAnimating)
                    {
                        Hide();
                    }
                });
            }

            // if (popupBackground != null)
            // {
            //     Button bgButton = popupBackground.GetComponent<Button>();
            //     if (bgButton == null)
            //     {
            //         bgButton = popupBackground.AddComponent<Button>();
            //     }
            //     bgButton.onClick.RemoveAllListeners();
            //     bgButton.onClick.AddListener(() => 
            //     {
            //         if (!isAnimating)
            //         {
            //             Hide();
            //         }
            //     });
            // }
        }

        #region Show and Hide
        [NTButton]
        public void Show()
        {
            if (isShowing) return;

            // Nếu đang animate hide, đợi nó xong
            if (isAnimating) return;

            isShowing = true;
            isAnimating = true;
            gameObject.SetActive(true);

            if (currentAnimationCoroutine != null)
            {
                StopCoroutine(currentAnimationCoroutine);
                currentAnimationCoroutine = null;
            }

            ResetToInitialState();

            switch (popupType)
            {
                case MNPopupType.oneStep:
                    if (panelRectTransform != null)
                        currentAnimationCoroutine = StartCoroutine(OneStepShowAnimation());
                    else
                        isShowing = false;
                    break;
                case MNPopupType.twoStep:
                    if (panelRectTransform != null)
                        currentAnimationCoroutine = StartCoroutine(TwoStepShowAnimation());
                    else
                        isShowing = false;
                    break;
                case MNPopupType.threeStep:
                    if (panelRectTransform != null)
                        currentAnimationCoroutine = StartCoroutine(ThreeStepShowAnimation());
                    else
                        isShowing = false;
                    break;
                case MNPopupType.moveTo:
                    if (panelRectTransform != null)
                        currentAnimationCoroutine = StartCoroutine(MoveToShowAnimation());
                    else
                        isShowing = false;
                    break;
                case MNPopupType.fade:
                    if (panelCanvasGroup != null)
                        currentAnimationCoroutine = StartCoroutine(FadeShowAnimation());
                    else
                        isShowing = false;
                    break;
                case MNPopupType.none:
                default:
                    if (panelCanvasGroup != null)
                    {
                        panelCanvasGroup.alpha = 1f;
                        panelCanvasGroup.interactable = true;
                    }
                    if (backgroundCanvasGroup != null)
                    {
                        backgroundCanvasGroup.alpha = 1f;
                        backgroundCanvasGroup.interactable = true;
                    }
                    break;
            }
        }

        [NTButton]
        public void Hide()
        {
            if (!isShowing) return;

            if (isAnimating) return;

            isAnimating = true;

            if (currentAnimationCoroutine != null)
            {
                StopCoroutine(currentAnimationCoroutine);
                currentAnimationCoroutine = null;
            }

            if (closeButton != null)
            {
                closeButton.interactable = false;
            }
            if (panelCanvasGroup != null)
            {
                panelCanvasGroup.interactable = false;
            }

            switch (popupType)
            {
                case MNPopupType.oneStep:
                    currentAnimationCoroutine = StartCoroutine(OneStepHideAnimation());
                    break;
                case MNPopupType.twoStep:
                    currentAnimationCoroutine = StartCoroutine(TwoStepHideAnimation());
                    break;
                case MNPopupType.threeStep:
                    currentAnimationCoroutine = StartCoroutine(ThreeStepHideAnimation());
                    break;
                case MNPopupType.moveTo:
                    currentAnimationCoroutine = StartCoroutine(MoveToHideAnimation());
                    break;
                case MNPopupType.fade:
                    currentAnimationCoroutine = StartCoroutine(FadeHideAnimation());
                    break;
                case MNPopupType.none:
                default:
                    gameObject.SetActive(false);
                    isShowing = false;
                    isAnimating = false;
                    break;
            }
        }

        public bool IsAnimating()
        {
            return isAnimating;
        }

        public bool CanHide()
        {
            return isShowing && !isAnimating;
        }

        private void ResetToInitialState()
        {
            if (panelRectTransform != null)
            {
                panelRectTransform.localScale = Vector3.zero;

                if (popupType == MNPopupType.moveTo)
                {
                    panelRectTransform.anchoredPosition = originalPanelPosition + moveFromOffset;
                }
                else
                {
                    panelRectTransform.anchoredPosition = originalPanelPosition;
                }
            }

            if (panelCanvasGroup != null)
            {
                panelCanvasGroup.alpha = 0f;
                panelCanvasGroup.interactable = false;
            }

            if (backgroundCanvasGroup != null)
            {
                backgroundCanvasGroup.alpha = 0f;
                backgroundCanvasGroup.interactable = false;
            }

            if (popupPanel != null)
            {
                popupPanel.SetActive(true);
            }
            if (popupBackground != null)
            {
                popupBackground.SetActive(true);
            }
        }

        #region OneStep Animation
        private IEnumerator OneStepShowAnimation()
        {
            if (panelRectTransform == null) yield break;

            panelRectTransform.anchoredPosition = originalPanelPosition;

            float elapsed = 0f;
            Vector3 startScale = Vector3.zero;
            Vector3 endScale = originalPanelScale;

            if (backgroundCanvasGroup != null)
            {
                StartCoroutine(FadeCanvasGroup(backgroundCanvasGroup, 0f, 1f, animationDuration));
            }

            while (elapsed < animationDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / animationDuration);
                float curveValue = easeCurve.Evaluate(t);
                panelRectTransform.localScale = Vector3.Lerp(startScale, endScale, curveValue);

                if (panelCanvasGroup != null)
                {
                    panelCanvasGroup.alpha = curveValue;
                }

                yield return null;
            }

            panelRectTransform.localScale = endScale;
            panelRectTransform.anchoredPosition = originalPanelPosition;
            if (panelCanvasGroup != null)
            {
                panelCanvasGroup.alpha = 1f;
                panelCanvasGroup.interactable = true;
            }
            if (backgroundCanvasGroup != null)
            {
                backgroundCanvasGroup.interactable = true;
            }

            if (closeButton != null)
            {
                closeButton.interactable = true;
            }

            isAnimating = false;
            currentAnimationCoroutine = null;
        }

        private IEnumerator OneStepHideAnimation()
        {
            if (panelRectTransform == null)
            {
                gameObject.SetActive(false);
                isShowing = false;
                yield break;
            }

            float elapsed = 0f;
            Vector3 startScale = panelRectTransform.localScale;
            Vector3 endScale = Vector3.zero;

            if (backgroundCanvasGroup != null)
            {
                StartCoroutine(FadeCanvasGroup(backgroundCanvasGroup, 1f, 0f, animationDuration));
            }

            while (elapsed < animationDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / animationDuration);
                float curveValue = easeCurve.Evaluate(t);
                panelRectTransform.localScale = Vector3.Lerp(startScale, endScale, curveValue);

                if (panelCanvasGroup != null)
                {
                    panelCanvasGroup.alpha = 1f - curveValue;
                }

                yield return null;
            }

            gameObject.SetActive(false);
            isShowing = false;
            isAnimating = false;
            currentAnimationCoroutine = null;
        }
        #endregion

        #region TwoStep Animation
        private IEnumerator TwoStepShowAnimation()
        {
            if (panelRectTransform == null) yield break;

            panelRectTransform.anchoredPosition = originalPanelPosition;

            if (backgroundCanvasGroup != null)
            {
                StartCoroutine(FadeCanvasGroup(backgroundCanvasGroup, 0f, 1f, animationDuration));
            }

            float elapsed = 0f;
            Vector3 startScale = Vector3.zero;
            Vector3 overshootScale = originalPanelScale * 1.1f;
            Vector3 endScale = originalPanelScale;

            while (elapsed < animationDuration * 0.6f)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / (animationDuration * 0.6f));
                float curveValue = easeCurve.Evaluate(t);
                panelRectTransform.localScale = Vector3.Lerp(startScale, overshootScale, curveValue);

                if (panelCanvasGroup != null)
                {
                    panelCanvasGroup.alpha = curveValue;
                }

                yield return null;
            }

            elapsed = 0f;
            while (elapsed < animationDuration * 0.4f)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / (animationDuration * 0.4f));
                float curveValue = easeCurve.Evaluate(t);
                panelRectTransform.localScale = Vector3.Lerp(overshootScale, endScale, curveValue);

                yield return null;
            }

            panelRectTransform.localScale = endScale;
            if (panelCanvasGroup != null)
            {
                panelCanvasGroup.alpha = 1f;
                panelCanvasGroup.interactable = true;
            }
            if (backgroundCanvasGroup != null)
            {
                backgroundCanvasGroup.interactable = true;
            }

            if (closeButton != null)
            {
                closeButton.interactable = true;
            }

            isAnimating = false;
            currentAnimationCoroutine = null;
        }

        private IEnumerator TwoStepHideAnimation()
        {
            if (panelRectTransform == null)
            {
                gameObject.SetActive(false);
                isShowing = false;
                yield break;
            }

            if (backgroundCanvasGroup != null)
            {
                StartCoroutine(FadeCanvasGroup(backgroundCanvasGroup, 1f, 0f, animationDuration));
            }

            float elapsed = 0f;
            Vector3 startScale = panelRectTransform.localScale;
            Vector3 shrinkScale = originalPanelScale * 0.9f;
            Vector3 endScale = Vector3.zero;

            while (elapsed < animationDuration * 0.3f)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / (animationDuration * 0.3f));
                float curveValue = easeCurve.Evaluate(t);
                panelRectTransform.localScale = Vector3.Lerp(startScale, shrinkScale, curveValue);

                yield return null;
            }

            elapsed = 0f;
            while (elapsed < animationDuration * 0.7f)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / (animationDuration * 0.7f));
                float curveValue = easeCurve.Evaluate(t);
                panelRectTransform.localScale = Vector3.Lerp(shrinkScale, endScale, curveValue);

                if (panelCanvasGroup != null)
                {
                    panelCanvasGroup.alpha = 1f - curveValue;
                }

                yield return null;
            }

            gameObject.SetActive(false);
            isShowing = false;
            isAnimating = false;
            currentAnimationCoroutine = null;
        }
        #endregion

        #region ThreeStep Animation
        private IEnumerator ThreeStepShowAnimation()
        {
            if (panelRectTransform == null) yield break;

            if (backgroundCanvasGroup != null)
            {
                StartCoroutine(FadeCanvasGroup(backgroundCanvasGroup, 0f, 1f, animationDuration));
            }

            float elapsed = 0f;
            Vector3 startScale = Vector3.zero;
            Vector3 overshoot1 = originalPanelScale * 1.2f;
            Vector3 undershoot = originalPanelScale * 0.9f;
            Vector3 endScale = originalPanelScale;

            while (elapsed < animationDuration * 0.4f)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / (animationDuration * 0.4f));
                float curveValue = easeCurve.Evaluate(t);
                panelRectTransform.localScale = Vector3.Lerp(startScale, overshoot1, curveValue);

                if (panelCanvasGroup != null)
                {
                    panelCanvasGroup.alpha = curveValue;
                }

                yield return null;
            }

            elapsed = 0f;
            while (elapsed < animationDuration * 0.3f)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / (animationDuration * 0.3f));
                float curveValue = easeCurve.Evaluate(t);
                panelRectTransform.localScale = Vector3.Lerp(overshoot1, undershoot, curveValue);

                yield return null;
            }

            elapsed = 0f;
            while (elapsed < animationDuration * 0.3f)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / (animationDuration * 0.3f));
                float curveValue = easeCurve.Evaluate(t);
                panelRectTransform.localScale = Vector3.Lerp(undershoot, endScale, curveValue);

                yield return null;
            }

            panelRectTransform.localScale = endScale;
            if (panelCanvasGroup != null)
            {
                panelCanvasGroup.alpha = 1f;
                panelCanvasGroup.interactable = true;
            }
            if (backgroundCanvasGroup != null)
            {
                backgroundCanvasGroup.interactable = true;
            }

            if (closeButton != null)
            {
                closeButton.interactable = true;
            }

            isAnimating = false;
            currentAnimationCoroutine = null;
        }

        private IEnumerator ThreeStepHideAnimation()
        {
            if (panelRectTransform == null)
            {
                gameObject.SetActive(false);
                isShowing = false;
                yield break;
            }

            if (backgroundCanvasGroup != null)
            {
                StartCoroutine(FadeCanvasGroup(backgroundCanvasGroup, 1f, 0f, animationDuration));
            }

            float elapsed = 0f;
            Vector3 startScale = panelRectTransform.localScale;
            Vector3 expandScale = originalPanelScale * 1.1f;
            Vector3 shrinkScale = originalPanelScale * 0.8f;
            Vector3 endScale = Vector3.zero;

            while (elapsed < animationDuration * 0.2f)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / (animationDuration * 0.2f));
                float curveValue = easeCurve.Evaluate(t);
                panelRectTransform.localScale = Vector3.Lerp(startScale, expandScale, curveValue);

                yield return null;
            }

            elapsed = 0f;
            while (elapsed < animationDuration * 0.3f)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / (animationDuration * 0.3f));
                float curveValue = easeCurve.Evaluate(t);
                panelRectTransform.localScale = Vector3.Lerp(expandScale, shrinkScale, curveValue);

                yield return null;
            }

            elapsed = 0f;
            while (elapsed < animationDuration * 0.5f)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / (animationDuration * 0.5f));
                float curveValue = easeCurve.Evaluate(t);
                panelRectTransform.localScale = Vector3.Lerp(shrinkScale, endScale, curveValue);

                if (panelCanvasGroup != null)
                {
                    panelCanvasGroup.alpha = 1f - curveValue;
                }

                yield return null;
            }

            gameObject.SetActive(false);
            isShowing = false;
            isAnimating = false;
            currentAnimationCoroutine = null;
        }
        #endregion

        #region MoveTo Animation
        private IEnumerator MoveToShowAnimation()
        {
            if (panelRectTransform == null) yield break;

            Vector2 startPos = originalPanelPosition + moveFromOffset;
            Vector2 endPos = originalPanelPosition;
            panelRectTransform.anchoredPosition = startPos;

            if (backgroundCanvasGroup != null)
            {
                StartCoroutine(FadeCanvasGroup(backgroundCanvasGroup, 0f, 1f, animationDuration));
            }

            float elapsed = 0f;
            while (elapsed < animationDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / animationDuration);
                float curveValue = easeCurve.Evaluate(t);
                panelRectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, curveValue);

                if (panelCanvasGroup != null)
                {
                    panelCanvasGroup.alpha = curveValue;
                }

                yield return null;
            }

            panelRectTransform.anchoredPosition = endPos;
            if (panelCanvasGroup != null)
            {
                panelCanvasGroup.alpha = 1f;
                panelCanvasGroup.interactable = true;
            }
            if (backgroundCanvasGroup != null)
            {
                backgroundCanvasGroup.interactable = true;
            }

            if (closeButton != null)
            {
                closeButton.interactable = true;
            }

            isAnimating = false;
            currentAnimationCoroutine = null;
        }

        private IEnumerator MoveToHideAnimation()
        {
            if (panelRectTransform == null)
            {
                gameObject.SetActive(false);
                isShowing = false;
                yield break;
            }

            Vector2 startPos = panelRectTransform.anchoredPosition;
            Vector2 endPos = originalPanelPosition + moveFromOffset;

            if (backgroundCanvasGroup != null)
            {
                StartCoroutine(FadeCanvasGroup(backgroundCanvasGroup, 1f, 0f, animationDuration));
            }

            float elapsed = 0f;
            while (elapsed < animationDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / animationDuration);
                float curveValue = easeCurve.Evaluate(t);
                panelRectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, curveValue);

                if (panelCanvasGroup != null)
                {
                    panelCanvasGroup.alpha = 1f - curveValue;
                }

                yield return null;
            }

            gameObject.SetActive(false);
            isShowing = false;
            isAnimating = false;
            currentAnimationCoroutine = null;
        }
        #endregion

        #region Fade Animation
        private IEnumerator FadeShowAnimation()
        {
            if (backgroundCanvasGroup != null)
            {
                StartCoroutine(FadeCanvasGroup(backgroundCanvasGroup, 0f, 1f, animationDuration));
            }

            if (panelCanvasGroup != null)
            {
                yield return StartCoroutine(FadeCanvasGroup(panelCanvasGroup, 0f, 1f, animationDuration));
                panelCanvasGroup.interactable = true;
            }

            if (backgroundCanvasGroup != null)
            {
                backgroundCanvasGroup.interactable = true;
            }

            if (closeButton != null)
            {
                closeButton.interactable = true;
            }

            isAnimating = false;
            currentAnimationCoroutine = null;
        }

        private IEnumerator FadeHideAnimation()
        {
            if (backgroundCanvasGroup != null)
            {
                StartCoroutine(FadeCanvasGroup(backgroundCanvasGroup, 1f, 0f, animationDuration));
            }

            if (panelCanvasGroup != null)
            {
                yield return StartCoroutine(FadeCanvasGroup(panelCanvasGroup, 1f, 0f, animationDuration));
            }

            gameObject.SetActive(false);
            isShowing = false;
            isAnimating = false;
            currentAnimationCoroutine = null;
        }

        private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float from, float to, float duration)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                float curveValue = easeCurve.Evaluate(t);
                canvasGroup.alpha = Mathf.Lerp(from, to, curveValue);
                yield return null;
            }
            canvasGroup.alpha = to;
        }
        #endregion
        #endregion

        public void SetPopupType(MNPopupType newType)
        {
            popupType = newType;
        }
    }
}
