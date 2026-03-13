using System.Collections;
using System.Collections.Generic;
using NTPackage.UI;
using Rubik.DataType;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rubik.PlayerLand
{
    public class LandSelectionItem : NTButtonEffect
    {
        public Image ImageLand;
        public TextMeshProUGUI Title;

        public RaceType Race;
        public LandSelectionUI LandSelectionUI;

        public void SetData(RaceType race, LandSelectionUI landSelectionUI){
            this.LandSelectionUI = landSelectionUI;
            this.Race = race;
            this.ImageLand.sprite = PlayerLandManager.Instance.GetLandSprite(race);
            this.Title.text = PlayerLandManager.Instance.GetPlayerLandName(race);
        }

        public void _OnClick()
        {
            this.LandSelectionUI.ChoseLand(this.Race);
        }
    

    }
}
