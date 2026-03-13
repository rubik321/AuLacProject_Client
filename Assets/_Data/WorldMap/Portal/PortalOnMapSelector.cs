using GOA.WorldMap;
using UnityEngine;

namespace Rubik.Myrk.Portal
{
    public class PortalOnMapSelector : SelectorObject
    {
        public PortalOnMap PortalOnMap;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Start()
        {
            base.Start();
            this.SelectorObjectType = SelectorObjectType.Portal;
        }
        public override void OnClick()
        {
            this.PortalOnMap.ShowUI();
        }
    }
}