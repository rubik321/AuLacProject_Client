using System.Collections;
using System.Collections.Generic;
using NTPackage.Functions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NTPackage.UI
{
    public enum ScrollType
    {
        None,
        Original,
        Switch,
    }

    public class ScrollRectController : NTBehaviour
    {

        public List<HorizontalScrollRect> HorizontalScrollRects = new List<HorizontalScrollRect>();
        public List<VerticalScrollRect> VerticalScrollRects = new List<VerticalScrollRect>();

        protected override void OnEnable()
        {
            base.OnEnable();
            this.Init();
        }

        protected override void Start()
        {
            base.Start();
            this.Init();
        }

        protected override void Update()
        {
            base.Update();
            if (Input.GetMouseButtonUp(0))
            {
                foreach (HorizontalScrollRect horizontalScrollRect in this.HorizontalScrollRects)
                {
                    horizontalScrollRect.ScrollType = ScrollType.None;
                }
                foreach (VerticalScrollRect verticalScrollRect in this.VerticalScrollRects)
                {
                    verticalScrollRect.ScrollType = ScrollType.None;
                }
            }
        }
        public void Init()
        {
            if (this.HorizontalScrollRects == null || this.VerticalScrollRects == null || this.HorizontalScrollRects.Count == 0 || this.VerticalScrollRects.Count == 0)
            {
                this.HorizontalScrollRects = new List<HorizontalScrollRect>();
                transform.GetComponentsInChildren<HorizontalScrollRect>(this.HorizontalScrollRects);
                this.VerticalScrollRects = new List<VerticalScrollRect>();
                transform.GetComponentsInChildren<VerticalScrollRect>(this.VerticalScrollRects);
                foreach (HorizontalScrollRect horizontalScrollRect in this.HorizontalScrollRects)
                {
                    horizontalScrollRect.ScrollRectController = this;
                }
                foreach (VerticalScrollRect verticalScrollRect in this.VerticalScrollRects)
                {
                    verticalScrollRect.ScrollRectController = this;
                }
            }
        }

        public void OnHorizontalScroll(PointerEventData eventData)
        {
            foreach (HorizontalScrollRect horizontalScrollRect in this.HorizontalScrollRects)
            {
                horizontalScrollRect.ScrollType = ScrollType.Original;
                horizontalScrollRect.OnDrag(eventData);
            }

        }

        public void OnVerticalScroll(PointerEventData eventData)
        {
            foreach (VerticalScrollRect verticalScrollRect in this.VerticalScrollRects)
            {
                verticalScrollRect.ScrollType = ScrollType.Original;
                verticalScrollRect.OnDrag(eventData);
            }

        }

        public void OnHorizontalScrollBegin(PointerEventData eventData)
        {
            foreach (HorizontalScrollRect horizontalScrollRect in this.HorizontalScrollRects)
            {
                horizontalScrollRect.ScrollType = ScrollType.Original;
                horizontalScrollRect.OnBeginDrag(eventData);
            }
        }

        public void OnVerticalScrollBegin(PointerEventData eventData)
        {
            foreach (VerticalScrollRect verticalScrollRect in this.VerticalScrollRects)
            {
                verticalScrollRect.ScrollType = ScrollType.Original;
                verticalScrollRect.OnBeginDrag(eventData);
            }
        }

        public void OnHorizontalScrollEnd(PointerEventData eventData)
        {
            foreach (HorizontalScrollRect horizontalScrollRect in this.HorizontalScrollRects)
            {
                horizontalScrollRect.ScrollType = ScrollType.Original;
                horizontalScrollRect.OnEndDrag(eventData);
            }
        }

        public void OnVerticalScrollEnd(PointerEventData eventData)
        {
            foreach (VerticalScrollRect verticalScrollRect in this.VerticalScrollRects)
            {
                verticalScrollRect.ScrollType = ScrollType.Original;
                verticalScrollRect.OnEndDrag(eventData);
            }
        }
    }
}