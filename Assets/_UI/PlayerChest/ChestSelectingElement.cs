using NTPackage.Functions;
using NTPackage.UI;
using Rubik.ItemPlayer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rubik.Myrk.PlayerChest
{
    public class ChestSelectingElement : NTButtonEffect
    {
        public Image Icon;
        public TextMeshProUGUI TextAmount;
        public ItemType ChestType;
        public PlayerChestSelectUI PlayerChestSelectUI;

        public void SetData(ItemType chestType, PlayerChestSelectUI playerChestSelectUI){
            this.ChestType = chestType;
            this.PlayerChestSelectUI = playerChestSelectUI;
            this.UpdateData();
        }

        public void UpdateData(){
            this.Icon.sprite = ItemDataManager.Instance.GetIcon(this.ChestType);
            this.TextAmount.text = "x" + ItemDataManager.Instance.GetItem(this.ChestType).Amount.ToString();
        }

        public void OnClick(){
            this.PlayerChestSelectUI.OnClickChest(this.ChestType);
        }
    }
}