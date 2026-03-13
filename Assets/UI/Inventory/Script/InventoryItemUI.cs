using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GOA.Item;
using GOA.UserData;
using NTFunctions_old;
using GOA.ChangeName;
using Rubik.Common.AudioHelper;
public class InventoryItemUI : MonoBehaviour
{
    Animator anim;
    public TextMeshProUGUI nameOfItem,numberOfItem,desOfItem;
    public Image icon;
    public Transform BntUse;
    public string IndexItem;
    public ItemCode ItemCode;
    public BagInventoryItemUI BagInventoryItemUI;

    // Start is called before the first frame update
    void Awake()
    {
     
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if (anim != null)
        {
            anim.Play("ShowPopup");
        }
    }
    private void OnDisable()
    {
        if (anim != null)
        {
            anim.Play("HidePopup");
        }

    }
    public void ShowUp(string index, ItemCode itemCode,string number,Sprite ava, bool able_use = false, BagInventoryItemUI bagInventoryItemUI = null)
    {
        AudioCtrl.Instance.Play(AudioName.UI_Button_Worldmap);
        this.BagInventoryItemUI = bagInventoryItemUI;
        gameObject.SetActive(true);
        nameOfItem.text = Lean.Localization.LeanLocalization.GetTranslationText(index+"_Name", "Item");
        numberOfItem.text = number;
        desOfItem.text = Lean.Localization.LeanLocalization.GetTranslationText(index+"_Description", "");
        icon.sprite = ava;
        this.IndexItem = index;
        this.ItemCode = itemCode;
        if(able_use){
            this.BntUse.gameObject.SetActive(true);
        }else{
            this.BntUse.gameObject.SetActive(false);
        }
        if(itemCode == ItemCode.Potion || itemCode ==ItemCode.HiPotion){
            if(UserData.Instance.characterData.CurrentHP >= UserData.Instance.characterData.HP){
                this.BntUse.gameObject.SetActive(false);
            }
        }
        if(itemCode == ItemCode.Ether || itemCode ==ItemCode.HiEther){
            if(UserData.Instance.characterData.CurrentMP >= UserData.Instance.characterData.MP){
                this.BntUse.gameObject.SetActive(false);
            }
        }
        if(itemCode == ItemCode.Elixir){
            if(UserData.Instance.characterData.CurrentHP >= UserData.Instance.characterData.HP&&UserData.Instance.characterData.CurrentMP >= UserData.Instance.characterData.MP){
                this.BntUse.gameObject.SetActive(false);
            }
        }

    }
    public void OnButtonClose()
    {
        gameObject.SetActive(false);

    }

    public void OnBtnUse(){
        if(this.ItemCode == ItemCode.NameTag){
            CharacterUIController.Instance.changeNameUI.OnUI();
        }else{
            APIManager.Instance.UserItem(ItemCode, (System.Action<string>)((data)=>{
                if(this.BagInventoryItemUI != null){
                  //  this.BagInventoryItemUI.UpdateData();
                 
                    if(ItemCode == ItemCode.Potion || ItemCode == ItemCode.HiPotion || ItemCode == ItemCode.Ether || ItemCode == ItemCode.HiEther || ItemCode == ItemCode.Elixir){
                        QuestManager.Instance.UpdateQuest(QuestType.Use_Potion, 1);
                        AudioCtrl.Instance.Play(AudioName.Heal_Sound);
                    }else{
                        AudioCtrl.Instance.Play(AudioName.UI_Button_Worldmap_2);
                    }
                }
            }));
        }
        gameObject.SetActive(false);

    }
   
}
