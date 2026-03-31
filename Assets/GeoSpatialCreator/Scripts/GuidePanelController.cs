using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EasyARGeoSpatialAnchors
{

    public class GuidePanelController : MonoBehaviour
    {

        public GameObject guidePanel;
        public Sprite[] guideSprites;
        public Text guideText;
        public Image spriteImage;

        public void ShowGuidedText(string text, int num)
        {
            guideText.text = text;
            spriteImage.sprite = guideSprites[num];
            StartCoroutine(ShowPanel());
        }

        IEnumerator ShowPanel()
        {
            guidePanel.SetActive(true);
            yield return new WaitForSeconds(3);
            guidePanel.SetActive(false);
            if (FindObjectOfType<GeoSpatialController>())
            {
                FindObjectOfType<GeoSpatialController>().showNotStable = false;
            }

        }
    }

}
