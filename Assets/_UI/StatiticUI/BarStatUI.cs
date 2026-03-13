using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rubik.UI.Statitic
{
    public class BarStatUI : MonoBehaviour
    {
        public Image Icon;
        public TextMeshProUGUI TextName;
        public TextMeshProUGUI TextValue;


        public StatData StatData;

        public void SetData(StatData statData, bool isCross = false, bool isBlack = false){
            this.StatData = statData;
            this.Icon.sprite = isBlack ? StatiticAssets.Instance.GetIconStatsColorful(statData.TypeStat) : StatiticAssets.Instance.GetIconStatsBlack(statData.TypeStat);
            this.TextName.text = StatiticAssets.Instance.GetNameStat(statData.TypeStat);
            if(isCross){
                this.TextValue.text = "+" + statData.Value.ToString();
            }
            else{
                this.TextValue.text = statData.Value.ToString();
            }
        }


        public void Onclick(){
            string title = StatiticAssets.Instance.GetNameStat(this.StatData.TypeStat);
            string description = StatiticAssets.Instance.GetDescriptionStat(this.StatData.TypeStat);
            HUDCanvas.Instance.ShowToolTip(title, description);
        }
    }
}