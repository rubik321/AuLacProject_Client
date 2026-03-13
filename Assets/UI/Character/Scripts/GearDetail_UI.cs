using GOA.UserData;
using Rubik._2DGPS.Gear;
using Rubik.Combat;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Spine.Unity;
using UnityEngine.UI;
public class GearDetail_UI : MonoBehaviour
{
    public TextMeshProUGUI txtTitle;
    public TextMeshProUGUI textDetail;
    public GearTypeSlot gearType;
    public GameObject gearPrefab;
    public Transform content;
    public Sprite equipSprite, unequipTxt;
    public SkeletonGraphic charAnim,heroGrapAnim;
    public SkeletonAnimation heroAnim;
    public Button equipButton;
    // Start is called before the first frame update
    private void Start()
    {
       // SetUpAllGears();
    }
    void OnEnable()
    {
        // SetUpGear();
        
    }
    public List<bool> lsHeros = new List<bool>();
    bool isStart = false;
    public void SetUpCharacter()
    {
        txtTitle.text = "Hero";
        
        if (lsGearIteam != null && lsGearIteam.Count > 0)
        {
            for (int i = 0; i < lsGearIteam.Count; i++)
            {
                Destroy(lsGearIteam[i].gameObject);
            }
            lsGearIteam.Clear();
        }
        else
        {
            lsGearIteam = new List<GearItem>();
        }
        lsHeros = UserData.Instance.lsHeros;
        
    //    for (int i = 0; i < AssetLoader.Instance.lsHeros.Length; i++)
    //    {
    //        int temp = i;
    //        var data = AssetLoader.Instance.lsHeros[temp];
    //        GearItem gearItem = Instantiate(gearPrefab).GetComponent<GearItem>();
    //        gearItem.transform.SetParent(content, false);
    //        gearItem.transform.localScale = Vector2.one;
    //        gearItem.gearIcon.sprite = data.icon;
    //        if(lsHeros[i])
    //        {
    //            MixSkinBoby(i);
    //            gearItem.rareHightlightImg.gameObject.SetActive(true);
    //            equipButton.image.sprite = unequipTxt;
    //            equipButton.GetComponentInChildren<TextMeshProUGUI>().text = "UnEquip";
    //        }
    //        lsGearIteam.Add(gearItem.GetComponent<GearItem>());
    //        gearItem.pressBtn.onClick.AddListener(() =>
    //        {
    //            for (int j = 0; j < lsGearIteam.Count; j++)
    //            {
    //                lsGearIteam[j].IsEquipped = false;
    //                lsGearIteam[j].rareHightlightImg.gameObject.SetActive(false);
    //            }
    //            gearItem.rareHightlightImg.gameObject.SetActive(true);
    //            MixSkinBoby(temp);
    //            if (!lsHeros[temp])
    //            {
    //                Rubik.Common.AudioHelper.AudioCtrl.instance.Play(Rubik.Common.AudioHelper.AudioName.UI_Button_Unequip);
    //                equipButton.gameObject.SetActive(true);
    //                equipButton.image.sprite = equipSprite;
    //                equipButton.GetComponentInChildren<TextMeshProUGUI>().text = "Equip";
                    
    //                //CharacterUIController.Instance.MixSkinBoby(temp);
                   
    //                equipButton.onClick.RemoveAllListeners();
    //                equipButton.onClick.AddListener(()=> {
    //                    for(int j = 0;j<lsHeros.Count;j++)
    //                    {
    //                        lsHeros[j] = false;
    //                    }
    //                    MixSkinBobyAnim(temp);
    //                    lsHeros[temp] = true;
    //                    // equipButton.gameObject.SetActive(false);
    //                    equipButton.image.sprite = unequipTxt;
    //                    equipButton.GetComponentInChildren<TextMeshProUGUI>().text = "UnEquip";
                        
    //                });
    //            }
    //            else
    //            {
    //                //equipButton.gameObject.SetActive(false);
    //                equipButton.onClick.RemoveAllListeners();
    //                equipButton.image.sprite = unequipTxt;
    //                equipButton.GetComponentInChildren<TextMeshProUGUI>().text = "UnEquip";
    //                Rubik.Common.AudioHelper.AudioCtrl.instance.Play(Rubik.Common.AudioHelper.AudioName.UI_Button_Equip);
                   
    //            }
                
               
    //        });

    //    }
    //    isStart = true;
    }
    public void MixSkinBoby(int index)
    {
        Rubik._2DGPS.UserData.UserDataManager.instance.UserData.SkinData.Suit = "Suit/suit " + (index + 1);
        
        AssetLoader.Instance.MixSkin(charAnim);

    }
    public void MixSkinBobyAnim(int index)
    {
        Rubik._2DGPS.UserData.UserDataManager.instance.UserData.SkinData.Suit = "Suit/suit " + (index + 1);
        if(heroAnim!=null)
            AssetLoader.Instance.MixSkin(heroAnim);
        if (heroGrapAnim != null)
            AssetLoader.Instance.MixSkin(heroGrapAnim);

    }
    public void SetUpAllGears(int index)
    {
        txtTitle.text = "Gear";
        if (lsGearIteam != null && lsGearIteam.Count > 0)
        {
            for (int i = 0; i < lsGearIteam.Count; i++)
            {
                Destroy(lsGearIteam[i].gameObject);
            }
            lsGearIteam.Clear();
        }
        else
        {
            lsGearIteam = new List<GearItem>();
        }
        //var lsGears =  GearManager.instance.GearDic.ToList();
        //var lsGearsData = GearManager.instance.GearDataDic.ToList();
        for (int i = 0;i < AssetLoader.Instance.lsGears.Length; i++)
        {
            // var dataGear = GearManager.instance.GearDataDic.Get(lsGears[i].Index.ToString());
            //  if (dataGear == null)
            //     continue;
            //Debug.Log(lsGears[i].Index.ToString());

            //if (GearManager.instance.GearDataDic.Get(lsGears[i].Index.ToString()).Slot != index)
            //    continue;
            int temp = i;
            var data = AssetLoader.Instance.lsGears[temp];
            if ((int)data.gearType != index)
               continue;
            //Debug.Log("Slot  : " + GearManager.instance.GearDataDic.Get(lsGears[i].Index.ToString()).Slot);
            GearItem gearItem = Instantiate(gearPrefab).GetComponent<GearItem>();
            gearItem.transform.SetParent(content, false);
            gearItem.transform.localScale = Vector2.one;
            gearItem.gearIcon.sprite = data.gearAva;
            //gearItem.rareIgm.sprite = SpriteHelper.Instance.lsRareSprites[data.rarity];
            lsGearIteam.Add(gearItem.GetComponent<GearItem>());
            gearItem.IsEquipped = data.isEquiped;
            if(gearItem.IsEquipped)
                gearItem.rareHightlightImg.gameObject.SetActive(true);
            gearItem.pressBtn.onClick.AddListener((UnityEngine.Events.UnityAction)(() =>
            {
                if (!gearItem.IsEquipped)
                {
                    Rubik.Common.AudioHelper.AudioCtrl.Instance.Play(Rubik.Common.AudioHelper.AudioName.UI_Button_Unequip);
                    Debug.Log((GearTypeSlot)index);
                    for (int j = 0; j < lsGearIteam.Count; j++)
                    {
                        lsGearIteam[j].IsEquipped = false;
                        lsGearIteam[j].rareHightlightImg.gameObject.SetActive(false);
                    }
                    for (int j = 0; j < AssetLoader.Instance.lsGears.Length; j++)
                    {
                       if(AssetLoader.Instance.lsGears[j].gearType == data.gearType)
                            AssetLoader.Instance.lsGears[j].isEquiped = false;
                    }
                    //Equip(index);
                    gearItem.rareHightlightImg.gameObject.SetActive(true);
                    if (!string.IsNullOrEmpty(data.GearName))
                    {
                        if(data.gearType == GearTypeSlot.Body)
                        {
                            Rubik._2DGPS.UserData.UserDataManager.instance.UserData.SkinData.Suit = data.GearName;
                        }
                        if (data.gearType == GearTypeSlot.Weapon)
                        {
                            Rubik._2DGPS.UserData.UserDataManager.instance.UserData.SkinData.Weapon = data.GearName;
                        }
                    }
                    MixSkinBobyAnim(temp);

                }
                else
                {
                    Rubik.Common.AudioHelper.AudioCtrl.Instance.Play(Rubik.Common.AudioHelper.AudioName.UI_Button_Equip);
                    for (int j = 0; j < lsGearIteam.Count; j++)
                    {
                        lsGearIteam[j].rareHightlightImg.gameObject.SetActive(false);
                    }
                }
                gearItem.IsEquipped = !gearItem.IsEquipped;
                data.isEquiped = !data.isEquiped;
            }));
        }
    }
    void Equip(int slot)
    {
        for (int i = 0; i < 24; i++)
        {
            var data = AssetLoader.Instance.lsGears[i];
            //if ((int)data.gearType == slot)
            {
                data.isEquiped = false;
                lsGearIteam[i].IsEquipped = false;
                lsGearIteam[i].rareHightlightImg.gameObject.SetActive(false);
            }
           
        }
    }
    List<GearItem> lsGearIteam = new List<GearItem>();
    public void SetUpGear(int index,int slotNumber)
    {
        if (lsGearIteam != null && lsGearIteam.Count > 0)
        {
            for (int i = 0; i < lsGearIteam.Count; i++)
            {
                Destroy(lsGearIteam[i].gameObject);
            }
            lsGearIteam.Clear();
        }
        else
        {
            lsGearIteam = new List<GearItem>();
        }

        CreateGearItem(index,slotNumber);

    }
    void CreateGearItem(int index, int slotNumber)
    {
        var datas = UserData.Instance.gearData;
        List<BaseDataStat> temps = new List<BaseDataStat>();
        int idCount = 0;
        for (int i = 0; i < datas.Data.GearData.Count; i++)
        {
            if (datas.Data.GearData[i].Slot != index || (datas.Data.GearData[i].Slot>4 && datas.Data.GearData[i].EquipedSlot>4 && datas.Data.GearData[i].EquipedSlot !=slotNumber 
                ))
                continue;
            GearItem gearItem = Instantiate(gearPrefab).GetComponent<GearItem>();
            gearItem.transform.SetParent(content, false);
            //go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y, -600);
            gearItem.transform.localScale = Vector2.one;
            gearItem.transform.eulerAngles = Vector2.zero;
            lsGearIteam.Add(gearItem.GetComponent<GearItem>());
            temps.Clear();
            foreach (BaseDataStat data in datas.Data.GearData[i].BaseStats)
            {
                temps.Add(data);
            }
            foreach (BaseDataStat data in datas.Data.GearData[i].ModStats)
            {
                temps.Add(data);
            }
            gearItem.SetUpGearItem(datas.Data.GearData[i], datas.Data.GearData[i].GearCode, datas.Data.GearData[i].GearName, datas.Data.GearData[i].EquipedSlot, temps);
            gearItem.SetButtonEquip(datas.Data.GearData[i].Equiped);

            idCount++;
            int tempId = idCount;
            int temp = i;

            gearItem.pressBtn.onClick.AddListener((UnityEngine.Events.UnityAction)(() =>
            {
                if (gearItem.IsEquipped)
                {
                    Rubik.Common.AudioHelper.AudioCtrl.Instance.Play(Rubik.Common.AudioHelper.AudioName.UI_Button_Unequip);
                    datas.Data.GearData[temp].Equiped = false;
                    lsGearIteam[tempId - 1].SetButtonEquip(false);
                  
                    datas.Data.GearData[temp].EquipedSlot = slotNumber;
                    datas.Data.GearData[temp].Slot = slotNumber;
                   

                }
                else
                {
                    Rubik.Common.AudioHelper.AudioCtrl.Instance.Play(Rubik.Common.AudioHelper.AudioName.UI_Button_Equip);
                    for (int j = 0; j < lsGearIteam.Count; j++)
                    {
                        // if(lsGearIteam[j].EquipSlot == datas.Data.GearData[temp].Slot)
                        lsGearIteam[j].SetButtonEquip(false);
                    }
                    datas.Data.GearData[temp].Equiped = true;
                    datas.Data.GearData[temp].EquipedSlot = slotNumber;
                    datas.Data.GearData[temp].Slot = slotNumber;
                    lsGearIteam[tempId - 1].SetButtonEquip(true);
                    ChangeGear(datas.Data.GearData[temp]);
                    datas.Data.GearData[temp].EquipedSlot = slotNumber;
                   
                }
            }));
        }
        switch (index)
        {
            case 0:
                //txtTitle.text = "Main Head";
                txtTitle.text = Lean.Localization.LeanLocalization.GetTranslationText("main_Head");
                textDetail.text = Lean.Localization.LeanLocalization.GetTranslationText("choose_head", "Choose Head Armor");
                break;
            case 1:
                txtTitle.text = Lean.Localization.LeanLocalization.GetTranslationText("main_Body");
                textDetail.text = Lean.Localization.LeanLocalization.GetTranslationText("choose_body", "Choose Body Armor");
                break;
            case 2:
                txtTitle.text = Lean.Localization.LeanLocalization.GetTranslationText("main_Leg");
                textDetail.text = Lean.Localization.LeanLocalization.GetTranslationText("choose_legging", "Choose Legging");
                break;
            case 3:
                txtTitle.text = Lean.Localization.LeanLocalization.GetTranslationText("main_Hand");
                textDetail.text = Lean.Localization.LeanLocalization.GetTranslationText("choose_equipment", "Choose An Equipment");
                break;
            case 4:
                txtTitle.text = Lean.Localization.LeanLocalization.GetTranslationText("off_Hand");
                textDetail.text = Lean.Localization.LeanLocalization.GetTranslationText("choose_equipment", "Choose An Equipment");
                break;
            default:
                txtTitle.text = Lean.Localization.LeanLocalization.GetTranslationText("accessory");
                textDetail.text = Lean.Localization.LeanLocalization.GetTranslationText("choose_accessory", "Choose An Accessory");
                break;
        }
    }
    public void ChangeGear(GearData data)
    {
       // CharacterUIController.Instance.charGearUI.LoadGear(data);

      
    } 
    public void UnLoadGear(GearData data)
    {
       // CharacterUIController.Instance.charGearUI.UnLoadGear(data);
      
    }
    public void Quit()
    {
        Rubik.Common.AudioHelper.AudioCtrl.Instance.Play(Rubik.Common.AudioHelper.AudioName.UI_Button_Back_Exit);
        CharacterUIController.Instance.Character_Gears(2);
    }
    public void QuitPopup()
    {
        Rubik.Common.AudioHelper.AudioCtrl.Instance.Play(Rubik.Common.AudioHelper.AudioName.UI_Button_Back_Exit);
        gameObject.SetActive(false);
    }
    public void RotateCharacter()
    {

    }
}
