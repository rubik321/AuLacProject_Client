using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace NTPackage.UI
{

    public class HorizontalScrollRect : ScrollRect
    {
        public ScrollRectController ScrollRectController;
        public ScrollType ScrollType = ScrollType.None;
        public override void OnDrag(PointerEventData eventData)
        {
            if (this.ScrollType == ScrollType.Switch)
            {
                ScrollRectController.OnVerticalScroll(eventData);
                return;
            }
            if (this.ScrollType == ScrollType.Original)
            {
                base.OnDrag(eventData);
                return;
            }
            // Check if swipe is mostly horizontal
            if (Mathf.Abs(eventData.delta.x) < Mathf.Abs(eventData.delta.y))
            {
                this.ScrollType = ScrollType.Switch;
                ScrollRectController.OnVerticalScroll(eventData);
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
                ScrollRectController.OnVerticalScrollBegin(eventData);
                return;
            }
            if (this.ScrollType == ScrollType.Original)
            {
                base.OnBeginDrag(eventData);
                return;
            }
            // Check if swipe is mostly horizontal
            if (Mathf.Abs(eventData.delta.x) < Mathf.Abs(eventData.delta.y))
            {
                this.ScrollType = ScrollType.Switch;
                ScrollRectController.OnVerticalScrollBegin(eventData);
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
            if (Mathf.Abs(eventData.delta.x) < Mathf.Abs(eventData.delta.y))
            {
                this.ScrollType = ScrollType.Switch;
                ScrollRectController.OnVerticalScrollEnd(eventData);
            }
            else
            {
                this.ScrollType = ScrollType.Original;
                base.OnEndDrag(eventData);
            }
        }
    }
}
