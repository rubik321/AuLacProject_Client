using NTPackage.Functions;
using NTPackage.UI;
using TMPro;
using UnityEngine;

namespace Rubik.Myrk.Portal
{
    public class SelectionLevelItem : NTButtonEffect
    {
        public TextMeshProUGUI LevelText;
        public int Level;
        public SelectionLevelUI SelectionLevelUI;

        public void SetData(int level, SelectionLevelUI selectionLevelUI){
            this.Level = level;
            this.LevelText.text = (level+1).ToString();
            this.SelectionLevelUI = selectionLevelUI;
        }

        public void _Onclick(){
            this.SelectionLevelUI._OnclickLevel(this.Level);
        }
    }
}