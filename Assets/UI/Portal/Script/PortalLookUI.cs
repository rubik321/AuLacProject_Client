using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTFunctions_old;

namespace GOA.Portal{
    public class PortalLookUI : PopupUI
    {
        public override void LoadComponents()
        {
            base.LoadComponents();
        }

        public void OnUI(){
            this.Show();
        }
    }
}
