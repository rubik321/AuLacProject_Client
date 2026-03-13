using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTFunctions_old;
using SimpleJSON;

namespace GOA.UIWorldMap{
    

    public class BtnReward : BaseButton
    {
        protected override void OnClick(){
            base.OnClick();
            WorldMap.WorldMapMaster.instance.GetReward();
        }
    }
}
