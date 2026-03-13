using UnityEngine;
using UnityEngine.UI;

namespace Rubik.UI
{
    public class ImageAspectRatio : MonoBehaviour
    {
        public AspectRatioFitter AspectRatioFitter;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (this.AspectRatioFitter == null) this.AspectRatioFitter = this.GetComponent<AspectRatioFitter>();
            this.AspectRatioFitter.aspectRatio = (float)Screen.width / Screen.height;
        }

    }
}