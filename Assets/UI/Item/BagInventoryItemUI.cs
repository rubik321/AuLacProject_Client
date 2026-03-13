using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using GOA.UserData;
using UnityEngine.Events;
using Rubik.CharacterGear;
using Rubik._2DGPS.Gear;

namespace GOA.Item{
    public class BagInventoryItemUI : MonoBehaviour
    {
        public Image Icon,bgIcon, borderIcon;
        public TextMeshProUGUI Number;
        public int Index;
        public Button press;
        public CharacterGearData ItemData;
        public GameObject tick,light1,light2;

        private void Start()
        {
           // press = GetComponent<Button>();
        }

        private void OnEnable() {
           // this.UpdateData();
        }

        //public void ShowUp(UnityAction callback = null) {
        //    ItemData = ItemAsset.instance.ItemDataDictionary2.Get(Index);
        //    this.Icon.sprite = SpriteHelper.Instance.GetSprite(ItemData.Images);
        //    this.Number.text = UserData.UserData.Instance.Inventory.GetPropValue(ItemData.Name) +"";
        //    press.onClick.AddListener(() => {
        //        //CharacterUIController.Instance.inventoryInfo.ShowUp(ItemData.Index, ItemData.Code,this.Number.text, Icon.sprite, ItemData.AbleUse, this);
        //            });
        //}

        //public void UpdateData(){
        //    try
        //    {
        //        float number = UserData.UserData.Instance.Inventory.GetPropValue(ItemData.Name);
        //        if(number <= 0){
        //            gameObject.SetActive(false);
        //            return;
        //        }
        //        gameObject.SetActive(true);
        //        this.Number.text =  number+"";
        //    }
        //    catch (System.Exception e)
        //    {
        //        NTPackage.Functions.NTLog.LogError(e+"", gameObject);
        //    }
        //}

        //use in quest data
        public void SetData(CharacterGear gear)
        {
            Number.text = "Lv."+(gear.Lv+1);// "Gear / "+(GearSlot)(gear.Slot())+" / "+gear.Index;
            ItemData = CharacterGearManager.Instance.GetGearDataByIndex(gear.Index);
            Index = (int)ItemData.Type;
            Icon.sprite =CharacterGearManager.Instance.GetGearSpriteByIndex(gear.Index);
           
            bgIcon.sprite = CharacterGearManager.Instance.GetGearBackgroundRarityByRarity(gear.Rarity);
            if (borderIcon != null) borderIcon.sprite = CharacterGearManager.Instance.GetGearBorderRarityByRarity(gear.Rarity);
            light1.SetActive(false);
            light2.SetActive(false);
            if (gear.Rarity == Rubik.DataType.RarityType.Legendary)
            {
                light2.SetActive(true);
            }
            if (gear.Rarity == Rubik.DataType.RarityType.Epic)
            {
                light1.SetActive(true);
            }
        }
    }
}
