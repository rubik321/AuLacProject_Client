using System;
using System.Collections;
using System.Collections.Generic;
using GOA.UserData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class CharacterCompanionUI : MonoBehaviour
{
    [SerializeField] GameObject btnUp, btnDownInfo, InfoGo, ClassGo, ScrollInfo,noCompanion,companionPos,skillGo;
    public ItemData[] lsItemsData;
    [SerializeField] TextMeshProUGUI characterNameTxt, classTxt, levelTxt;
    [SerializeField] Sprite[] lsRarity,lsButtonEquip;
    [SerializeField] Image rareImg,equipBtnImg;
    [SerializeField] OrbItem[] lsOrbs;
    [SerializeField] GameObject[] compaModels;
    Dictionary<string, GameObject> lsCompanionModels = new Dictionary<string, GameObject>();
    int index = 0;
    void Start()
    {
        foreach (GameObject com in compaModels)
        {
            try
            {
                Debug.Log(com.name);
                lsCompanionModels.TryAdd(com.name, com);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        SetItemData();
        
    }

    public void Onbutton_Next()
    {
        if (index < UserData.Instance.companions.Companions.Count - 1)
        {
            index++;
        }
        if (index == UserData.Instance.companions.Companions.Count - 1)
            btnUp.SetActive(false);
        else
            btnUp.SetActive(true);
        SetItemData();
        btnDownInfo.SetActive(true);
    }
    public void Onbutton_Pre()
    {
        if (index >0)
        {
            index--;
        }
        if (index == 0)
            btnDownInfo.SetActive(false);
        else
            btnDownInfo.SetActive(true);
        SetItemData();
        btnUp.SetActive(true);
    }
    public void OnbuttonPlus()
    {

    }

    public void SetItemData()
    {

        var charData = UserData.Instance.characterData;
        if (UserData.Instance.companions.Companions == null || UserData.Instance.companions.Companions.Count == 0)
        {
            companionPos.SetActive(false);
            noCompanion.SetActive(true);
            skillGo.SetActive(false);
            equipBtnImg.gameObject.SetActive(false);
            lsItemsData[0].itemNameTxt.text = "Strength";
            lsItemsData[0].itemValueTxtl.text = "0";
            lsItemsData[1].itemNameTxt.text = "Mind";
            lsItemsData[1].itemValueTxtl.text = "0";
            lsItemsData[2].itemNameTxt.text = "Speed";
            lsItemsData[2].itemValueTxtl.text = "0";
            lsItemsData[3].itemNameTxt.text = "Hit Rate";
            lsItemsData[3].itemValueTxtl.text = "0";
            lsItemsData[4].itemNameTxt.text = "Crit Rate";
            lsItemsData[4].itemValueTxtl.text = "0";
            lsItemsData[5].itemNameTxt.text = "Crit Damage";
            lsItemsData[5].itemValueTxtl.text = "0";
            return;
        }

        var comp = UserData.Instance.GetCompanionData(UserData.Instance.companions.Companions[index].CompCode);
        if (UserData.Instance.companions.Companions[index].IsInAdventure)
        {
            equipBtnImg.sprite = lsButtonEquip[0];
            equipBtnImg.GetComponentInChildren<TextMeshProUGUI>().text = "On Adventure";
        }
        else
        {
            equipBtnImg.gameObject.SetActive(true);
            if (!UserData.Instance.companions.Companions[index].Equiped)
            {
                equipBtnImg.sprite = lsButtonEquip[1];
                equipBtnImg.GetComponentInChildren<TextMeshProUGUI>().text = "Equip";
            }
            else
            {
                equipBtnImg.sprite = lsButtonEquip[0];
                equipBtnImg.GetComponentInChildren<TextMeshProUGUI>().text = "Equipped";
            }
        }
       
        characterNameTxt.text = comp.CompName;
        //classTxt.text = "WARRIOR";
        if (index == 0)
            btnDownInfo.SetActive(false);
        rareImg.sprite = lsRarity[UserData.Instance.companions.Companions[index].Rarity];
        levelTxt.text = "Level : " + 1;
        lsItemsData[0].itemNameTxt.text = "Strength";
        lsItemsData[0].itemValueTxtl.text =((int)(comp.STR* charData.Str)).ToString();
        lsItemsData[1].itemNameTxt.text = "Mind";
        lsItemsData[1].itemValueTxtl.text = ((int)(comp.MND*charData.Mind)).ToString();
        lsItemsData[2].itemNameTxt.text = "Speed";
        lsItemsData[2].itemValueTxtl.text = ((int)(comp.SPD*charData.Speed)).ToString();
        lsItemsData[3].itemNameTxt.text = "Hit Rate";
        lsItemsData[3].itemValueTxtl.text = (int)((Math.Round(comp.HIT, 2)) * 100) + "%";
        lsItemsData[4].itemNameTxt.text = "Crit Rate";
        lsItemsData[4].itemValueTxtl.text = (Math.Round(comp.CRI , 2)) * 100 + "%";
        lsItemsData[5].itemNameTxt.text = "Crit Damage";
        lsItemsData[5].itemValueTxtl.text = (Math.Round(comp.CRD , 2)) * 100 + "%";
        //lsItemsData[6].itemNameTxt.text = "HitRate";
        //lsItemsData[6].itemValueTxtl.text = (Math.Round(charData.HitRate, 2)).ToString();
        //lsItemsData[7].itemNameTxt.text = "Evade";
        //lsItemsData[7].itemValueTxtl.text = (Math.Round(charData.Evade, 2)).ToString();
        //lsItemsData[8].itemNameTxt.text = "CritRate";
        //lsItemsData[8].itemValueTxtl.text = (Math.Round(charData.CritRate, 3)) * 100 + "%";
        //lsItemsData[9].itemNameTxt.text = "CritDame";
        //lsItemsData[9].itemValueTxtl.text = (Math.Round(charData.CritDame, 3)) * 100 + "%";
        //lsItemsData[10].itemNameTxt.text = "Vis";
        //lsItemsData[10].itemValueTxtl.text = charData.Vis.ToString();
        if (!String.IsNullOrEmpty(comp.Skill_1))
        {
            SetOrbItem(0, UserData.Instance.DictionaryOrbData[comp.Skill_1]);

        }
        else
        {
            lsOrbs[0].gameObject.SetActive(false);
        }
        if (!String.IsNullOrEmpty(comp.Skill_2))
        {
            SetOrbItem(1, UserData.Instance.DictionaryOrbData[comp.Skill_2]);
        }
        else
        {
            lsOrbs[1].gameObject.SetActive(false);
        }
        foreach (GameObject com in compaModels)
        {
            com.SetActive(false);
        }
        lsCompanionModels[UserData.Instance.lsGoIdToName[comp.CompCode]].SetActive(true);
        if (UserData.Instance.companions.Companions.Count <=1)
        {
            btnUp.SetActive(false);
            btnDownInfo.SetActive(false);
        }
    }

    public void SetOrbItem(int index, OrbData data)
    {
        if (data != null)
            lsOrbs[index].SetData(data);
        else
        {
            lsOrbs[index].SetUpOrbItem(false, UserData.Instance.characterData.CharType);
        }
    }
    public void OnButtonEquiped()
    {
        Rubik.Common.AudioHelper.AudioCtrl.Instance.Play(Rubik.Common.AudioHelper.AudioName.UI_Button_Equip);
        if (UserData.Instance.companions.Companions[index].IsInAdventure)
            return;
        if (!UserData.Instance.companions.Companions[index].Equiped)
        {
            //UserData.Instance.companions.Companions[index].Equiped = true;
            UserData.Instance.SetCompanion(UserData.Instance.GetCompanionData(UserData.Instance.companions.Companions[index].CompCode));
            //equipBtnImg.sprite = lsButtonEquip[0];
            //equipBtnImg.GetComponentInChildren<TextMeshProUGUI>().text = "Equiped";
            //for (int i = 0; i < UserData.Instance.companions.Companions.Count; i++)
            //{
            //    if (i != index)
            //    {
            //        UserData.Instance.companions.Companions[index].Equiped = false;
            //    }
            //}
            APIManager.Instance.EquipCompanion(UserData.Instance.companions.Companions[index]._id,true,()=> { SetItemData(); });
            
        }
        else
        {
            APIManager.Instance.EquipCompanion(UserData.Instance.companions.Companions[index]._id, false, () => {
                UserData.Instance.companion = new MobInfo();
                SetItemData(); });
        }
        
    }
}
