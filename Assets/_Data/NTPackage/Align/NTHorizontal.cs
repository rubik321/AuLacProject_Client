using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace NTPackage.Functions
{
    public class NTHorizontal : AlignFit
    {
        public RectTransform rectTransform;
        public List<RectTransform> BaseOn;

        public bool AutoGetChild = false;

        public float left = 0;
        public float right = 0;

        public float space = 0;

        public bool IsLeft = false;

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

        protected override void Update()
        {
            base.Update();
            this.FitSelf();
        }

        public override void FitSelf()
        {
            float width = this.left;
            if (AutoGetChild)
            {
                foreach (RectTransform rt in transform)
                {
                    if (!rt.gameObject.activeSelf) continue;
                    rt.anchoredPosition = new Vector2(width, 0);
                    width += rt.rect.width + this.space;
                    if (IsLeft)
                    {
                        rt.pivot = new Vector2(1, 1);
                        rt.anchorMin = new Vector2(1, 1);
                        rt.anchorMax = new Vector2(1, 1);
                    }
                    else
                    {
                        rt.pivot = new Vector2(0, 1);
                        rt.anchorMin = new Vector2(0, 1);
                        rt.anchorMax = new Vector2(0, 1);
                    }
                }
                width -= this.space;
            }
            else
            {
                foreach (RectTransform rt in BaseOn)
                {
                    if (!rt.gameObject.activeSelf) continue;
                    rt.anchoredPosition = new Vector2(width, 0);
                    width += rt.rect.width + this.space;
                    if (IsLeft)
                    {
                        rt.pivot = new Vector2(1, 1);
                        rt.anchorMin = new Vector2(1, 1);
                        rt.anchorMax = new Vector2(1, 1);
                    }
                    else
                    {
                        rt.pivot = new Vector2(0, 1);
                        rt.anchorMin = new Vector2(0, 1);
                        rt.anchorMax = new Vector2(0, 1);
                    }
                }
                width -= this.space;
            }

            width += this.right;
            this.rectTransform.sizeDelta = new Vector2(width, this.rectTransform.rect.height);
            base.FitSelf();
        }
    }
}
