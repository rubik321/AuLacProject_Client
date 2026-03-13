using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Rubik.Common.Popup
{
    [RequireComponent(typeof(Animator))]
    public class BasePopup : MonoBehaviour
    {
        public UnityEvent onAccept = new UnityEvent();
        public UnityEvent onDecline = new UnityEvent();
        public UnityAction<BasePopup> onCompleted;

        private void Awake()
        {
            GetComponent<Canvas>().worldCamera = Camera.main;

            try
            {
                var popupPanel = transform.Find("Panel").GetComponent<Image>();
                popupPanel.color = new Color(0, 0, 0, 0);
            }
            catch (Exception e)
            {
                Debug.Log("Do not find popup");
            }
            
            try
            {
                var popupContent = transform.Find("Panel/Popup");
                popupContent.localScale = new Vector3(0, 1, 1);
            }
            catch (Exception e)
            {
                Debug.Log("Do not find popup");
            }
        }
        

        protected void CloseAndInvoke(UnityEvent clickedEvent)
        {
            GetComponent<Animator>().Play("Hide");
            DOTween.Sequence()
                .AppendInterval(0.3f)
                .AppendCallback(() => clickedEvent?.Invoke())
                .OnComplete(() =>
                {
                    onCompleted?.Invoke(this);
                    Destroy(gameObject);
                })
                .Play();
        }

        public virtual void OnAccept()
        {
            CloseAndInvoke(onAccept);
        }

        public virtual void OnDecline()
        {
            CloseAndInvoke(onDecline);
        }
    }
}