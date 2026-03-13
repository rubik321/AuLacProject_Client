using GOA.Item;
using GOA.UserData;
using NTPackage.UI;
using NTPackage_old.Functions;
using Rubik._2DGPS.Gear;
using Rubik.CharacterGear;
using Rubik.CharacterPlayer;
using Rubik.Combat;
using Rubik.Common.AudioHelper;
using Rubik.Myrk.BattleTeam;
using Sirenix.OdinInspector;
using Spine;
using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rubik.CharacterGear
{
  
    public class CharacterGearUI : PopupUI
    {
        public ItemData[] lsItemsData;
        [SerializeField] SkeletonGraphic characterSkin;
        [SerializeField] Sprite slotOn, slotOff;
        [SerializeField] GameObject inventoryPrefab;
        [SerializeField] Transform gearContent;
        [SerializeField] List<TabInven> lsButtonSlots, lsTabButton;
        [SerializeField] public List<SlotInventory> lsSlotInventory;
        [SerializeField] List<Sprite> lsRare;
        [SerializeField] string[] gearIDs;
        [SerializeField] GearStats gearStats;
        [SerializeField] List<GameObject> lsEmptySlots;
        int currentTab = 0;

        public int IndexSelected = 0;
        //  [SerializeField] GearDetail_UI gearDetail;

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        public override void OnUI(object data = null, bool isDefaultSound = true)
        {
            base.OnUI(data, isDefaultSound);
            IndexSelected = (int)data;
            gearIDs = BattleTeamManager.Instance.GetCharacterGearIDsByIndex(this.IndexSelected).ToArray();
            currentTab = 0;
            LoadAllGear();
            Debug.Log(JsonUtility.ToJson(new NTFunctions_old.NTList<CharacterGear>(BattleTeamManager.Instance.GetCharacterGearByIndex(this.IndexSelected))));
            AssetLoader.Instance.MixSkinWithGearsUI(characterSkin,BattleTeamManager.Instance.GetGearIndexsByIndex(this.IndexSelected));
            Rubik.DataType.GearStats heroStat = BattleTeamManager.Instance.GetHeroStatsBattle();
            gearStats.SetInfo( heroStat.TeamHp,  heroStat.HeroAtk, heroStat.TeamSpeed);
            OnButtonTabs(0);

        }
        public void OnButtonTabs(int index)
        {
            AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
            foreach (SlotInventory slot in lsSlotInventory)
            {
                slot.gameObject.SetActive(false);
            }
            foreach (TabInven slot in lsButtonSlots)
            {
                slot.gameObject.SetActive(false);
            }
            if (index == 1)
            {
               
                for(int i = 0; i < 3; i++)
                {
                    lsSlotInventory[i].gameObject.SetActive(true);
                    lsButtonSlots[i].gameObject.SetActive(true);
                }
                ButtonTabsSlot(0);
                lsTabButton[index].TabOn(true);
                lsTabButton[0].TabOn();
            }
            else
            {
                for (int i = 3; i < lsSlotInventory.Count; i++)
                {
                    lsSlotInventory[i].gameObject.SetActive(true);
                    lsButtonSlots[i].gameObject.SetActive(true);
                }
                ButtonTabsSlot(3);
                lsTabButton[index].TabOn(true);
                lsTabButton[1].TabOn();
            }
            
        }
        public void ButtonTabsSlot(int indexSlot)
        {

            AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
            foreach (TabInven btn in lsButtonSlots)
            {
                // btn.GetComponent<Image>().color = Color.black;
                btn.TabOn(false);

            }
            lsButtonSlots[indexSlot].TabOn(true);
            foreach (BagInventoryItemUI inven in lsInvens)
            {
                inven.gameObject.SetActive(indexSlot  == inven.Index);

            }
            currentTab = indexSlot;
        }
        public BagInventoryItemUI GetInvenItem(int indexSlot)
        {
            foreach (BagInventoryItemUI inven in lsInvens)
            {
               if(indexSlot == inven.Index)
                {
                    return inven;
                }

            }
            return null;
        }
        public void OnButtonTabGear(int index)
        {
            if (index == 0)
            {
                ButtonTabsSlot(0);
            }

        }
        public string GearName = "";
        [ContextMenu("LoadGearToCharacter")]
        public void LoadGear(string nameGear)
        {
            Debug.LogWarning("LoadGear");
            // characterGear.LoadGearForUI(nameGear);

        }
 
        public void LoadAllGear()
        {
            foreach (BagInventoryItemUI item in lsInvens)
            {
                Destroy(item.gameObject);
            }
            lsInvens.Clear();
            for (int i = 0; i < lsEmptySlots.Count; i++)
            {
                lsEmptySlots[i].SetActive(false);
                //lsEmptySlots[i].transform.SetAsLastSibling();
            }
            for (int i = 0; i < lsSlotInventory.Count; i++)
            {
                lsSlotInventory[i].item.gameObject.SetActive(false);
                //lsEmptySlots[i].transform.SetAsLastSibling();
            }
            // GearManager
            
            Common.Common.ResetContent(gearContent);
            foreach (CharacterGear gear in CharacterGearManager.Instance.GearDic.ToList())
            {
                var temp = CharacterGearManager.Instance.GetGearDataByIndex(gear.Index);
                InitInventory(gear);
            }
            for(int i = 0; i < lsEmptySlots.Count; i++)
            {
                lsEmptySlots[i].SetActive(true);
                lsEmptySlots[i].transform.SetAsLastSibling();
            }
            ButtonTabsSlot(currentTab);
        }
      



        public void ResetInventoryItem()
        {
            this.isShowInven = false;
            //ObjectPoolingManager.instance.PushChildObjectIntoPooling(inventoryContent);
        }

        bool isShowInven = false;

        //GOA.Item.BagInventoryItemUI bag = new GOA.Item.BagInventoryItemUI();
        public List<BagInventoryItemUI> lsInvens = new List<BagInventoryItemUI>();
        List<GameObject> lsCloths = new List<GameObject>();
        
        
        void InitInventory(CharacterGear gear)
        {
            GameObject go = Instantiate(inventoryPrefab);
            go.transform.SetParent(gearContent, false);
            var temp = go.GetComponent<BagInventoryItemUI>();
            temp.SetData(gear);
            lsInvens.Add(temp);
            temp.tick.SetActive(false);
            if (gear.IsEquipedByIndex(IndexSelected))
            {
                temp.tick.SetActive(true);
                NTLog.LogMessage("Gear index : " + temp.Index);
                lsSlotInventory[temp.Index].item.gameObject.SetActive(true);
                lsSlotInventory[temp.Index].item.SetData(gear);
                gearIDs[temp.Index] = gear._id;
                lsSlotInventory[temp.Index].item.GetComponent<Button>().onClick.RemoveAllListeners();
                lsSlotInventory[temp.Index].item.GetComponent<Button>().onClick.AddListener(() =>
                {
                    AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
                    PopupManager.Instance.OnUI(PopupCode.GearInfo_UI, gear, (popupUI) =>
                    {
                        GearInfo_UI gearInfoUI = popupUI as GearInfo_UI;
                        gearInfoUI.SetData(gear._id, IndexSelected, () =>
                        {
                            gearIDs[temp.Index] = gear._id;
                            BattleTeamManager.Instance.UpdateBattleGears(gearIDs, IndexSelected);
                            BattleTeamManager.Instance.ConfirmUpdateBattleGears(IndexSelected, (battleTeamData) => {
                                lsSlotInventory[temp.Index].item.gameObject.SetActive(true);
                                lsSlotInventory[temp.Index].item.SetData(gear);
                                Debug.Log(JsonUtility.ToJson(new NTFunctions_old.NTList<CharacterGear>(BattleTeamManager.Instance.GetCharacterGearByIndex(this.IndexSelected))));
                                AssetLoader.Instance.MixSkinWithGearsUI(characterSkin, BattleTeamManager.Instance.GetGearIndexsByIndex(this.IndexSelected));
                                //PopupManager.Instance.GetPopupUI(PopupCode.LineUpUI).GetComponent<HeroPanel>().SetANim();
                                foreach (BagInventoryItemUI item in lsInvens)
                                {
                                    if (item.Index == temp.Index)
                                    {
                                        item.tick.SetActive(false);
                                    }
                                }
                                temp.tick.SetActive(true);
                            });


                        });
                    });
                });
            }

            temp.press.onClick.RemoveAllListeners();
            Action popup = () => {
                PopupManager.Instance.OnUI(PopupCode.GearInfo_UI, gear, (popupUI) =>
                {
                    GearInfo_UI gearInfoUI = popupUI as GearInfo_UI;
                    gearInfoUI.SetData(gear._id, IndexSelected, () =>
                    {
                        gearIDs[temp.Index] = gear._id;
                        BattleTeamManager.Instance.UpdateBattleGears(gearIDs, IndexSelected);
                        BattleTeamManager.Instance.ConfirmUpdateBattleGears(IndexSelected, (battleTeamData) => {
                            lsSlotInventory[temp.Index].item.gameObject.SetActive(true);
                            lsSlotInventory[temp.Index].item.SetData(gear);
                            Debug.Log(JsonUtility.ToJson(new NTFunctions_old.NTList<CharacterGear>(BattleTeamManager.Instance.GetCharacterGearByIndex(this.IndexSelected))));
                            AssetLoader.Instance.MixSkinWithGearsUI(characterSkin, BattleTeamManager.Instance.GetGearIndexsByIndex(IndexSelected));
                           // PopupManager.Instance.GetPopupUI(PopupCode.LineUpUI).GetComponent<HeroPanel>().SetANim();
                            foreach (BagInventoryItemUI item in lsInvens)
                            {
                                if (item.Index == temp.Index)
                                {
                                    item.tick.SetActive(false);
                                }
                            }
                            temp.tick.SetActive(true);
                            lsSlotInventory[temp.Index].item.GetComponent<Button>().onClick.RemoveAllListeners();
                            lsSlotInventory[temp.Index].item.GetComponent<Button>().onClick.AddListener(() =>
                            {

                                PopupManager.Instance.OnUI(PopupCode.GearInfo_UI, gear, (popupUI) =>
                                {
                                    GearInfo_UI gearInfoUI = popupUI as GearInfo_UI;
                                    gearInfoUI.SetData(gear._id, IndexSelected, () =>
                                    {
                                        gearIDs[temp.Index] = gear._id;
                                        BattleTeamManager.Instance.UpdateBattleGears(gearIDs, IndexSelected);
                                        BattleTeamManager.Instance.ConfirmUpdateBattleGears(IndexSelected, (battleTeamData) => {
                                            lsSlotInventory[temp.Index].item.gameObject.SetActive(true);
                                            lsSlotInventory[temp.Index].item.SetData(gear);
                                           
                                            foreach (BagInventoryItemUI item in lsInvens)
                                            {
                                                if (item.Index == temp.Index)
                                                {
                                                    item.tick.SetActive(false);
                                                }
                                            }
                                            temp.tick.SetActive(true);
                                        });


                                    });
                                });
                            });
                        });


                    });
                });
            };
            temp.press.onClick.AddListener(() =>
            {
                popup.Invoke();
               
            });
            

        }
        public void ClosePopup()
        {
           // if (IndexSelected == 101)
            {
                FindFirstObjectByType<HeroPanel>().SetANim();
            }
            OffUI();
           
        }

        public void AddGearTest()
        {
            GearManager.instance.AddRandomGear();
        }
    }

}
