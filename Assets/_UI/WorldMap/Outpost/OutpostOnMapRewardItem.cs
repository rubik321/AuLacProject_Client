using System.Collections.Generic;
using NTPackage.Functions;
using NTPackage.UI;
using Rubik.ItemPlayer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rubik.Myrk.Outpost
{
    using Rubik.ItemPlayer;
    public class OutpostOnMapRewardItem : NTBehaviour
    {
        public Image BG;
        public Image Current;
        public TextMeshProUGUI TextAmount;
        public ListItemDataUI ListItemDataUI;

        public void SetData(string textAmount, List<ItemData> rewardDatas){
            this.Current.gameObject.SetActive(false);
            this.Clear();
            this.ListItemDataUI.SetData(rewardDatas.ToArray(), null, true, true);
            this.TextAmount.text = textAmount;
        }

        public void Clear(){
            this.ListItemDataUI.Clear();
        }

        public void SetBG(int index){
            if(index%2 == 0){
                this.BG.gameObject.SetActive(true);
            }
            else{
                this.BG.gameObject.SetActive(false);
            }
        }
    }
}