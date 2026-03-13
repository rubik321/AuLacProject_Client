using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GOA.WorldMap
{
    using GOA.Building;
    using NTPackage.Functions;
    using Portal;
    using Rubik.Myrk.Monster;
    using Rubik.Myrk.Portal;
    using ShopBuilding;

    public class Selector : NTBehaviour
    {
        public const float MaxDistance = 2000;
        public float TimeHold = 0.2f;
        public float CountTimeHold = 0;
        public SelectorObject SelectorObject;

        protected override void Update()
        {
            if (this.CountTimeHold > 0)
            {
                this.CountTimeHold -= Time.deltaTime;
            }
            if (Input.GetMouseButtonDown(0))
            {
                this.SelectorObject = null;
                this.CountTimeHold = TimeHold;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, MaxDistance))
                {
                    if (IsPointerOverUIObject() || EventSystem.current.IsPointerOverGameObject())
                    {
                        //Debug.Log("Clicked on the UI");
                        return;
                    }
                    if (hit.collider.gameObject.TryGetComponent<SelectorObject>(out SelectorObject selectorObject))
                    {
                        this.SelectorObject = selectorObject;
                        return;
                    }
                }
            }

            if(Input.GetMouseButtonUp(0)){
                if(this.SelectorObject != null && this.CountTimeHold > 0){
                    this.SelectorObject.OnClick();
                }
            }
        }

        //CheckInMobile
        private bool IsPointerOverUIObject()
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                // Check if the touch is over a UI element
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId))
                {
                    // The touch is over a UI element
                    return true;
                }
            }
            return false;
        }

        
    }
}