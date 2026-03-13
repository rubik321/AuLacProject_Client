using NTPackage.Functions;
using NTPackage.UI;
using Rubik.ItemPlayer;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.Myrk.PlayerChest
{
    using ItemPlayer;
    using Rubik.Common.AudioHelper;
    using Rubik.UI;
    using System.Linq;
    using TMPro;

    public class PlayerChestSelectUI : PopupUI
    {

        public int Index;
        public ItemType ChestType;

        public List<ChestSelectingElement> ChestSelectingElements;

        public List<ChestRewardItemUI> ChestRewardItemUIs;

        public TextMeshProUGUI TextTimeOpen;

        public override void OnUI(object data = null, bool isDefaultSound = true)
        {
            this.Index = (int)data;
            this.ChestType = ItemType.WoodChest;
            base.OnUI(data, isDefaultSound);
        }


        public override void UpdateData(object data = null)
        {
            base.UpdateData(data);

            this.ChestSelectingElements[0].SetData(ItemType.WoodChest, this);
            this.ChestSelectingElements[1].SetData(ItemType.BronzeChest, this);
            this.ChestSelectingElements[2].SetData(ItemType.SilverChest, this);
            this.ChestSelectingElements[3].SetData(ItemType.GoldChest, this);

            foreach (ChestSelectingElement chestSelectingElement in this.ChestSelectingElements){
                chestSelectingElement.Unchose();
                if (chestSelectingElement.ChestType == this.ChestType){
                    chestSelectingElement.Chose();
                }
            }

            foreach (ChestRewardItemUI chestRewardItemUI in this.ChestRewardItemUIs){
                chestRewardItemUI.gameObject.SetActive(false);
            }

            PlayerChestData playerChestData = PlayerChestManager.Instance.GetPlayerChestData(this.ChestType);
            this.ChestRewardItemUIs[0].SetData(new ItemData(ItemType.ExpChest, playerChestData.Exp));
            this.ChestRewardItemUIs[0].gameObject.SetActive(true);
            int index = 1;
            for (int i = 0; i < playerChestData.ItemRewards.Length; i++)
            {
                this.ChestRewardItemUIs[index].SetData(playerChestData.ItemRewards[i]);
                this.ChestRewardItemUIs[index].gameObject.SetActive(true);
                index++;
            }
            this.ChestRewardItemUIs[index].gameObject.SetActive(true);
            this.ChestRewardItemUIs[index].SetData(playerChestData.ShardCardRewards.ToList());
            this.TextTimeOpen.text = NTFunction.FormatTimeHour(playerChestData.TimeUnlock);
        }

        public void OnClickChest(ItemType chestType){
            AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
            this.ChestType = chestType;
            this.UpdateData();
        }

        public void _OnclickConfirm(){
            AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
            if (ItemDataManager.Instance.GetItem(this.ChestType).Amount <= 0){
                string str = Lean.Localization.LeanLocalization.GetTranslationText("dont_enough_item", "You have insufficient {0}!");
                str = string.Format(str, ItemDataManager.Instance.GetItemName(this.ChestType));
                HUDCanvas.Instance.ShowNotification(str);
                return;
            }
            this.OffUI();
            AudioCtrl.Instance.Play(AudioName.Collect_Gem_Sound);
            StartCoroutine(PlayerChestManager.Instance.IEUnlockChest(this.Index, this.ChestType, ()=>{
               
                PlayerChestUI playerChestUI = PopupManager.Instance.GetPopupUI(PopupCode.PlayerChestUI) as PlayerChestUI;
                playerChestUI.UpdateData();
            }));
        }
    }
}