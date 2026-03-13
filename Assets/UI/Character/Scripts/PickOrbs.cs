using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOA.UserData;
public class PickOrbs : MonoBehaviour
{
   
    public GameObject gearPrefab;
    public Transform content;

    int indexGearChoose = 0;
    // Start is called before the first frame update
    void Awake()
    {
        //SetUpOrbs();
    }
    private void OnEnable()
    {
        Rubik.Common.AudioHelper.AudioCtrl.Instance.Play(Rubik.Common.AudioHelper.AudioName.UI_Popup_Panel_Open_Default);
        //for (int i = 0; i < lsGearIteam.Count; i++)
        //{
        //    if (lsGearIteam[i].IsEquipped && lsGearIteam[i].EquipSlot != CharacterUIController.Instance.orbIndex)
        //    {
        //        lsGearIteam[i].gameObject.SetActive(false);
        //    }
        //    else
        //    {
        //        lsGearIteam[i].gameObject.SetActive(true);
        //    }
        //}
        SetUpOrbs();
    }
    List<OrbItem> lsGearIteam;
    public void SetUpOrbs()
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
            lsGearIteam = new List<OrbItem>();
        }
        CreateGearItem();
       
    }
    
    void CreateGearItem()
    {
        for (int i = 0; i < UserData.Instance.SkillDatas.SkillData.Count; i++)
        {
            if(UserData.Instance.SkillDatas.SkillData[i].OrbCode== "SK_55003" || UserData.Instance.SkillDatas.SkillData[i].OrbCode == "SK_55010")
            {
                continue;
            }
            var data = UserData.Instance.DictionaryOrbData[UserData.Instance.SkillDatas.SkillData[i].OrbCode];
            data.Equiped = UserData.Instance.SkillDatas.SkillData[i].Equiped;
            data.EquipedSlot = UserData.Instance.SkillDatas.SkillData[i].EquipedSlot;
            Debug.LogWarning(data.Equiped + " --- " + data.EquipedSlot + " -- " + CharacterUIController.Instance.orbIndex);
            if (data.Equiped && data.EquipedSlot != CharacterUIController.Instance.orbIndex)
            {
                continue;
            }
            GameObject go = Instantiate(gearPrefab);
            go.transform.SetParent(content);
            go.transform.localScale = Vector3.one;
            go.transform.eulerAngles = Vector3.zero;
            lsGearIteam.Add(go.GetComponent<OrbItem>());
            data.EquipedSlot = UserData.Instance.SkillDatas.SkillData[i].EquipedSlot;
            data.Equiped = UserData.Instance.SkillDatas.SkillData[i].Equiped;
        
            var item = go.GetComponent<OrbItem>();
            var temp = UserData.Instance.DictionaryOrbData[UserData.Instance.SkillDatas.SkillData[i].OrbCode];
            temp._id = UserData.Instance.SkillDatas.SkillData[i]._id;
            temp.EquipedSlot = UserData.Instance.SkillDatas.SkillData[i].EquipedSlot;
            temp.Equiped = UserData.Instance.SkillDatas.SkillData[i].Equiped;
            temp.StatSkill = UserData.Instance.DictionaryOrbData[temp.Index].StatSkill;
            // data.SkillOrb = UserData.Instance.SkillDatas.SkillData[i].SkillOrb;
            temp.Multiplier = UserData.Instance.DictionaryOrbData[temp.Index].Multiplier;
            var tempIndex = i;
            item.SetUpOrbItem(true,temp.SkillOrb, temp, ()=> {
                for (int i = 0; i < lsGearIteam.Count; i++)
                {
                    if (lsGearIteam[i].IsEquipped && lsGearIteam[i].EquipSlot == CharacterUIController.Instance.orbIndex)
                    {
                        lsGearIteam[i].SetButtonEquip(false);
                       // UserData.Instance.DictionaryOrbData[UserData.Instance.SkillDatas.SkillData[tempIndex].OrbCode].Equiped = false;
                    }
                    
                }
            });
        }

    }
    //public void ChangeGear()
    //{
    //    CharacterUIController.Instance.charGearUI.LoadGear(lsGearIteam[indexGearChoose].itemName.text);
    //    CharacterUIController.Instance.Character_Gears(1);
    //}
    public void RotateCharacter()
    {

    }
    public void Quit()
    {
        Rubik.Common.AudioHelper.AudioCtrl.Instance.Play(Rubik.Common.AudioHelper.AudioName.UI_Button_Back_Exit);
        CharacterUIController.Instance.Character_Gears(1);
    }
}
