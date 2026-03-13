using System.Collections;
using System.Collections.Generic;
using GOA.WorldMap;
using NTPackage.Functions;
using NTPackage.UI;
using UnityEngine;

namespace Rubik.Myrk.Monster
{
    public class MonsterSelection : SelectorObject
    {
        public MonsterOnMapModel MonsterOnMapModel;

        public override void OnClick(){
            PopupManager.Instance.OnUI(NTPackage.UI.PopupCode.MonsterOnMapUI, null, (popup) =>
            {
                MonsterOnMapUI mosterOnMapUI = (MonsterOnMapUI)popup;
                mosterOnMapUI.SetData(this.MonsterOnMapModel.GetMonsterOnMapData());
            });
        }
    }
}