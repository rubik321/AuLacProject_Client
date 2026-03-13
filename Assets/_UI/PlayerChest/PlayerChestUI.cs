using System.Collections.Generic;
using UnityEngine;
using NTPackage.UI;
using NTPackage.Functions;
using TMPro;
using UnityEngine.UI;

namespace Rubik.Myrk.PlayerChest
{
    public class PlayerChestUI : PopupUI
    {
        public TextMeshProUGUI TextLevel;
        public TextMeshProUGUI TextExp;
        public Image ExpBar;
        public List<PlayerChestSlotUI> PlayerChestSlotUI;

        [NTButton]
        public void TestOnUI(){
            this.OnUI();
        }

        public override void UpdateData(object data = null)
        {
            base.UpdateData(data);
            (int Level, long Exp, long ExpMax) levelData = PlayerChestManager.Instance.GetLevelData();
            this.TextLevel.text = "Lv. " + (levelData.Level + 1);
            this.TextExp.text = NTFunction.FormatNumber(levelData.Exp) + "/" + NTFunction.FormatNumber(levelData.ExpMax);
            this.ExpBar.fillAmount = (float)levelData.Exp / levelData.ExpMax;
            for (int i = 0; i < PlayerChestManager.Instance.PlayerChestSlotData.Count; i++)
            {
                PlayerChestSlotUI[i].SetData(PlayerChestManager.Instance.PlayerChestSlotData[i]);
                PlayerChestSlotUI[i].Lock();
            }

            foreach (PlayerChestSlot item in PlayerChestManager.Instance.PlayerChest.PlayerChestSlot)
            {
                this.PlayerChestSlotUI.Find(x => x.Index == item.Index).SetPlayerData(item);
            }
            
        }


    }
}

