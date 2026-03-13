using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rubik.Common
{
    public class AutoFitText : MonoBehaviour
    {
        public bool enableMinSize;
        public Vector2 minSize;
        

        private void OnEnable()
        {
            Refresh();
        }

        public void Refresh()
        {
            var tmpText = GetComponent<TMP_Text>();
            if (tmpText != null)
            {
                var fitSize = new Vector2(tmpText.preferredWidth, tmpText.preferredHeight);
                if (enableMinSize)
                {
                    fitSize = new Vector2(Mathf.Max(minSize.x, tmpText.preferredWidth), Mathf.Max(minSize.y, tmpText.preferredHeight));
                }
                
                GetComponent<RectTransform>().sizeDelta = fitSize;
                return;
            }
            
            var text = GetComponent<Text>();
            if (text != null)
            {
                var fitSize = new Vector2(tmpText.preferredWidth, tmpText.preferredHeight);
                if (enableMinSize)
                {
                    fitSize = new Vector2(Mathf.Max(minSize.x, tmpText.preferredWidth), Mathf.Max(minSize.y, tmpText.preferredHeight));
                }

                GetComponent<RectTransform>().sizeDelta = fitSize;
            }
        }
    }
}
