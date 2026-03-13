using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.UI
{
    public class AuctionConfirmPanel : MonoBehaviour
    {
        Action onConfirm;

        public void Show(Action onConfirm)
        {
            gameObject.SetActive(true);
            this.onConfirm = onConfirm;
        }

        public void OnClickConfirm()
        {
            onConfirm?.Invoke();
            gameObject.SetActive(false);
        }

        public void OnClickClose()
        {
            gameObject.SetActive(false);
        }
    }
}
