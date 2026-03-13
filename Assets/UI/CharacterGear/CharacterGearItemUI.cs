using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rubik.CharacterGear
{
    public class CharacterGearItemUI : MonoBehaviour
    {
        public Image IconGear;
        public Image BackgroundGear;
        public Image BorderGear;
        public TextMeshProUGUI LevelGear;

        public CharacterGear CharacterGear;

        public void SetData(CharacterGear characterGear, bool isShowToolTip = false){
            this.CharacterGear = characterGear;
            IconGear.sprite = CharacterGearManager.Instance.GetGearSpriteByIndex(characterGear.Index);
            BorderGear.sprite = CharacterGearManager.Instance.GetGearBorderRarityByRarity(characterGear.Rarity);
            BackgroundGear.sprite = CharacterGearManager.Instance.GetGearBackgroundRarityByRarity(characterGear.Rarity);
            LevelGear.text = (characterGear.Lv+1).ToString();
        }
    }
}
