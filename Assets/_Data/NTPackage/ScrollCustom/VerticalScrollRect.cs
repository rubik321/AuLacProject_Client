using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace NTPackage.UI
{
    public class VerticalScrollRect : ScrollRect
    {
        public ScrollRectController ScrollRectController;
        public ScrollType ScrollType = ScrollType.None;
        public override void OnDrag(PointerEventData eventData)
        {
            if (this.ScrollType == ScrollType.Switch)
            {
                Debug.Log("HorizontalScrollRect: Scrolling");
                ScrollRectController.OnHorizontalScroll(eventData);
                return;
            }
            if (this.ScrollType == ScrollType.Original)
            {
                Debug.Log("VerticalScrollRect: Scrolling");
                base.OnDrag(eventData);
                return;
            }
            // Check if swipe is mostly horizontal
            if (Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y))
            {
                this.ScrollType = ScrollType.Switch;
                Debug.Log("VerticalScrollRect OnHorizontalScroll:" + eventData.delta.x + " " + eventData.delta.y);
                ScrollRectController.OnHorizontalScroll(eventData);
            }
            else
            {
                this.ScrollType = ScrollType.Original;
                base.OnDrag(eventData);
            }
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            if (this.ScrollType == ScrollType.Switch)
            {
                ScrollRectController.OnHorizontalScrollBegin(eventData);
                return;
            }
            if (this.ScrollType == ScrollType.Original)
            {
                base.OnBeginDrag(eventData);
                return;
            }
            // Check if swipe is mostly horizontal
            if (Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y))
            {
                this.ScrollType = ScrollType.Switch;
                ScrollRectController.OnHorizontalScroll(eventData);
            }
            else
            {
                this.ScrollType = ScrollType.Original;
                base.OnBeginDrag(eventData);
            }
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            if (this.ScrollType == ScrollType.Switch)
            {
                ScrollRectController.OnVerticalScrollEnd(eventData);
                return;
            }
            if (this.ScrollType == ScrollType.Original)
            {
                base.OnEndDrag(eventData);
                return;
            }
            // Check if swipe is mostly horizontal
            if (Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y))
            {
                this.ScrollType = ScrollType.Switch;
                ScrollRectController.OnHorizontalScrollEnd(eventData);
            }
            else
            {
                this.ScrollType = ScrollType.Original;
                base.OnEndDrag(eventData);
            }
        }
    }
}
