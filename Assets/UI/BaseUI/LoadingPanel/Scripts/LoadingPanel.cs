using System.Collections;
using System.Collections.Generic;
using NTPackage.UI;
using Pixelplacement;
using UnityEngine;
namespace Rubik.UI
{
    public class LoadingPanel : PopupUI
    {
        public Transform BlackScreen;

        public const float TimeBlackScreen = 1f;
        public const float TimeOffUI = 5f;
        public Coroutine CorWaitTime;

        public override void OnUI(object data = null, bool isDefaultSound = true)
        {
            this.BlackScreen.gameObject.SetActive(false);
            base.OnUI(data, isDefaultSound);
            if (this.CorWaitTime != null)
            {
                StopCoroutine(this.CorWaitTime);
            }
            this.CorWaitTime = StartCoroutine(this.IEWaitTime());
        }

        public override void ScriptOffUI()
        {
            base.ScriptOffUI();
            if (this.CorWaitTime != null)
            {
                StopCoroutine(this.CorWaitTime);
            }
        }

        public IEnumerator IEWaitTime()
        {
            yield return new WaitForSeconds(TimeBlackScreen);
            this.BlackScreen.gameObject.SetActive(true);
            yield return new WaitForSeconds(TimeOffUI);
            this.OffUI();
        }
    }
}