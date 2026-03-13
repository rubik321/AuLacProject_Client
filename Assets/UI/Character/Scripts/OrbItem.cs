using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GOA.UserData;
using UnityEngine.Events;
public class OrbItem : MonoBehaviour
{
    public TextMeshProUGUI itemName,targetText,desTxt,powerTxt;
    public GameObject chooseObj;
    public Button pressBtn,equipButton;
    public Image orbIcon,powerIcon;
    [SerializeField] Image[] orbDecoAva;
    public Sprite equipSprite,defaultRed,defaultGren,defaultGreen;
    public Sprite unequipSprite;
    public Sprite[] redSlot,greenSlot,blueSlot,costType;
    List<GameObject> lsGearIteam;

    public int EquipSlot =-1;
    public bool IsEquipped;
    public int index;
    [SerializeField] OrbData dataOrb = new OrbData();
    private void Start()
    {
        index = transform.GetSiblingIndex();
    }
    public void SetButtonEquip(bool isEquipped)
    {
        this.IsEquipped = isEquipped;
        
        if (isEquipped)
        {
            EquipSlot = CharacterUIController.Instance.orbIndex;
            equipButton.GetComponent<Image>().sprite = unequipSprite;
            equipButton.GetComponentInChildren<TextMeshProUGUI>().text = Lean.Localization.LeanLocalization.GetTranslationText("unequipe");
        }
        else
        {
            equipButton.GetComponent<Image>().sprite = equipSprite;
            equipButton.GetComponentInChildren<TextMeshProUGUI>().text = Lean.Localization.LeanLocalization.GetTranslationText("equip");
           // if (data.SkillType == 0)
              //  UserData.Instance.SetSkillPassive((StatSkill)UserData.Instance.DictionaryOrbData[data.OrbCode].StatSkill, -UserData.Instance.DictionaryOrbData[data.OrbCode].Multiplier);
        }
    }


    public void SetUpOrbItem(bool isShowEquip = false,int slotOrb = 0,OrbData data = null,UnityAction callback = null)
    {
        dataOrb = data;
        switch (slotOrb)
        {
            case 0:
                for (int i = 0; i < orbDecoAva.Length; i++)
                {
                    orbDecoAva[i].sprite = redSlot[i];
                }
                break;
            case 1:
                for (int i = 0; i < orbDecoAva.Length; i++)
                {
                    orbDecoAva[i].sprite = greenSlot[i];
                }
                break;
            case 2:
                for (int i = 0; i < orbDecoAva.Length; i++)
                {
                    orbDecoAva[i].sprite = blueSlot[i];
                }
                break;
                
        }
        if (data == null)
            SetDefault();
        else
        {
            SetData(data);
        }
        if (!isShowEquip)
        {
            equipButton.gameObject.SetActive(false);
            pressBtn.onClick.AddListener(() => {
                CharacterUIController.Instance.orbIndex = index;
                CharacterUIController.Instance.Character_OrbsPick();
                
            });
        }
        else
        {
            string temp = data.OrbCode;
            Debug.Log(temp);
            if (slotOrb == UserData.Instance.characterData.CharType)
            {
                equipButton.gameObject.SetActive(true);
                SetButtonEquip(data.Equiped);
                equipButton.onClick.AddListener(() =>
                {

                    if(callback!=null)
                        callback();
                    Debug.Log(data.Equiped);
                    if (!data.Equiped)
                    {
                        
                        //CharacterUIController.Instance.charGearUI.SetOrbItem(CharacterUIController.Instance.orbIndex, data);
                        data.Equiped = true;
                        data.EquipedSlot = CharacterUIController.Instance.orbIndex;
                        APIManager.Instance.EquipOrb(data);
                        EquipSlot = CharacterUIController.Instance.orbIndex; 
                        SetButtonEquip(data.Equiped);
                        
                     
                    }
                    else
                    {
                        Debug.Log(dataOrb.OrbCode);
                        EquipSlot = -1;
                        data.Equiped = false;
                        data.EquipedSlot = CharacterUIController.Instance.orbIndex;
                        APIManager.Instance.EquipOrb(data,false);
                       // CharacterUIController.Instance.charGearUI.SetOrbItem(CharacterUIController.Instance.orbIndex, null);
                        SetButtonEquip(data.Equiped);
                        data.EquipedSlot = -1;
                       
                    }
                });
            }
               
            else
                equipButton.gameObject.SetActive(false);
        }
    }
    public void SetData(OrbData data)
    {
        itemName.text = data.Name;
        //orbIcon.sprite = SpriteHelper.Instance.GetSprite(data.Index);
        //orbIcon.SetNativeSize();
        powerTxt.text = data.Cost.ToString();
        targetText.text = Lean.Localization.LeanLocalization.GetTranslationText(((SkillType)data.SkillType).ToString(), "");;
        desTxt.text = data.Des;
        if (data.CostType > 0)
            powerIcon.sprite = costType[data.CostType - 1];
        else
            powerIcon.gameObject.SetActive(false);
    }
    void SetDefault()
    {
        switch (UserData.Instance.characterData.CharType)
        {
            case 0:
                itemName.text = " Skill Slot";
               // orbIcon.sprite = defaultRed;
                desTxt.text = Lean.Localization.LeanLocalization.GetTranslationText("Click_add_skill_orb", "Click To Add Skill Orb");
                break;
            case 1:
                itemName.text = " Skill Slot";
               // orbIcon.sprite = defaultGreen;
                desTxt.text = Lean.Localization.LeanLocalization.GetTranslationText("Click_add_skill_orb", "Click To Add Skill Orb");
                break;
            case 2:
                itemName.text = " Skill Slot";
               // orbIcon.sprite = defaultGren;
                desTxt.text = Lean.Localization.LeanLocalization.GetTranslationText("Click_add_skill_orb", "Click To Add Skill Orb");
                break;
        }
        powerIcon.gameObject.SetActive(false);
        
       // orbIcon.SetNativeSize();
        powerTxt.text = "";
        targetText.text = "";
        
        equipButton.gameObject.SetActive(false);
    }
   
}

public enum SkillType
{
    Passive,
    Single_target,
    All_target
}
