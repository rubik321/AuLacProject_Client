using System.Collections;
using System.Collections.Generic;
using NTPackage.UI;
using Rubik.DataType;
using Rubik.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rubik.PlayerLand
{
    public class LandSelectionUI : PopupUI
    {
        public static List<RaceType> LsRace = new List<RaceType> { RaceType.Gaia, RaceType.Mechtronic, RaceType.Nepharian, RaceType.Emberfolk, RaceType.Arcanian, RaceType.Shifterian };
        public List<LandSelectionItem> LsLandSelectionItem;

        public RaceType RaceSelected = RaceType.Gaia;

        public TextMeshProUGUI Title;
        public TextMeshProUGUI Description;
        public Image ImageLand;

        public override void OnUI(object data = null, bool isDefaultSound = true)
        {
            base.OnUI(data, isDefaultSound);
            for (int i = 0; i < LsRace.Count; i++)
            {
                RaceType race = LsRace[i];
                this.LsLandSelectionItem[i].SetData(race, this);
            }
        }

        public void ChoseLand(RaceType race)
        {
            this.RaceSelected = race;
            this.UpdateData(null);
        }


        // Update is called once per frame
        public override void UpdateData(object data)
        {
            base.UpdateData(data);
            foreach (NTButtonEffect item in this.LsLandSelectionItem)
            {
                item.Unchose();
            }
            this.LsLandSelectionItem.Find(item => item.Race == this.RaceSelected)?.Chose();
            this.Title.text = PlayerLandManager.Instance.GetPlayerLandName(this.RaceSelected);
            this.Description.text = PlayerLandManager.Instance.GetPlayerLandDescription(this.RaceSelected);
            this.ImageLand.sprite = PlayerLandManager.Instance.GetLandSprite(this.RaceSelected);
        }

        public void _OnClickConfirm()
        {
            StartCoroutine(PlayerLandManager.Instance.IEChosePlayerLand(this.RaceSelected, () =>
            {
                this.OffUI();
            }));
        }
    }
}