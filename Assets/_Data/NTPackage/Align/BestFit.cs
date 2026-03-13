using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NTPackage.Functions
{
    public class BestFit : AlignFit
    {

        public RectTransform Main;
        public RectTransform BaseOn;

        public float top = 0;
        public float bot = 0;
        public float right = 0;
        public float left = 0;

        protected override void Update()
        {
            if (this.BaseOn == null) return;
            if (this.Main == null) this.Main = transform.GetComponent<RectTransform>();
            Vector2 size = new Vector2(this.BaseOn.sizeDelta.x + right + left, this.BaseOn.sizeDelta.y + top + bot);
            if (this.Main.sizeDelta != size) this.FitSelf();
        }

        public override void FitSelf()
        {
            this.Main.sizeDelta = new Vector2(this.BaseOn.sizeDelta.x + right + left, this.BaseOn.sizeDelta.y + top + bot);
            this.BaseOn.anchoredPosition = new Vector2(left, -top);
            base.FitSelf();
        }
    }
}