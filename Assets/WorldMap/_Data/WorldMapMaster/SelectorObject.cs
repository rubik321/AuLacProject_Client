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

    public enum SelectorObjectType{
        Outpost,
        Portal,
        Monster,
    }

    public class SelectorObject : NTBehaviour
    {
        public SelectorObjectType SelectorObjectType;

        public virtual void OnClick(){
            
        }
    }
}