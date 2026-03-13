using System.Collections;
using System.Collections.Generic;
using GOA.UIWorldMap;
using NTPackage_old.UI;
using UnityEngine;

namespace GOA.Building
{
    public class BuildingMenu : MonoBehaviour
    {
        public List<NTCheckBox> CheckBoxes;

        public void OffMenu(){
            PanelMainToolUI.instance.OffBuilding();
        }

        public void Chose(NTCheckBox checkBox){
            foreach (NTCheckBox item in CheckBoxes)
            {
                if(item == checkBox) continue;
                item.UnCheck();
            }
            checkBox.Check();
        }

        public void UnChose(NTCheckBox checkBox){
            checkBox.UnCheck();
        }
    }
}
