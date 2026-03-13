using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Rubik.Myrk.BattleTeam
{
    public class EquipedIconUIItem : MonoBehaviour
    {
        public Image Icon;
        public TextMeshProUGUI Text;
        public void SetData(Sprite sprite, string text){
            this.Icon.sprite = sprite;
            this.Text.text = text;
        }
    }
}