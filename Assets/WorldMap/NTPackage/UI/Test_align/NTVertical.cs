using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NTFunctions_old
{
    public class NTVertical : AlignFit
    {
        public RectTransform rectTransform;
        public List<RectTransform> BaseOn;

        public bool AutoGetChild = false;

        public float top = 0;
        public float bottom = 0;

        public float space = 0;

        public override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadRectTransform();
            this.LoadBaseOn();
        }

        protected void LoadRectTransform()
        {
            this.rectTransform = transform.GetComponent<RectTransform>();
        }

        protected void LoadBaseOn()
        {
            if (this.BaseOn == null) this.BaseOn = new List<RectTransform>();
            if (this.BaseOn.Count > 0) return;
            foreach (RectTransform rt in transform)
            {
                this.BaseOn.Add(rt);
            }
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            this.FitSelf();
        }

        public override void FitSelf()
        {
            float height = this.top;
            if (AutoGetChild)
            {
                foreach (RectTransform rt in transform)
                {
                    if (!rt.gameObject.activeSelf) continue;
                    rt.anchoredPosition = new Vector2(0, -height);
                    height += rt.rect.height + this.space;
                }
            }
            else
            {
                foreach (RectTransform rt in BaseOn)
                {
                    if (!rt.gameObject.activeSelf) continue;
                    rt.anchoredPosition = new Vector2(0, -height);
                    height += rt.rect.height + this.space;
                }
            }

            height += this.bottom;
            this.rectTransform.sizeDelta = new Vector2(this.rectTransform.rect.width, height);
            base.FitSelf();
        }
    }
}
